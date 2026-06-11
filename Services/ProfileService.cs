using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class ProfileService : IProfile
    {
        private readonly GesDbContext _db;
        private readonly UserManager<UserModel> _userManager;

        public ProfileService(GesDbContext db, UserManager<UserModel> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ProfileDto?> GetProfileAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "";

            // Décomposer NomComplet pour avoir Nom + Prenoms séparés à l'affichage
            var (nom, prenoms) = DecomposeNomComplet(user.NomComplet);

            return new ProfileDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Nom = nom,
                Prenoms = prenoms,
                NomComplet = user.NomComplet,
                Telephone = user.PhoneNumber,
                Role = role,
                RoleLabel = GetRoleLabel(role),
                DateCreation = user.DateCreation,
                DerniereConnexion = user.DerniereConnexion,
                IsActive = user.IsActive
            };
        }

        public async Task<ProfileUpdateResult> UpdateProfileAsync(int userId, ProfileUpdateInput input)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return ProfileUpdateResult.Fail("Utilisateur introuvable.");

            // Recomposer NomComplet à partir de Nom + Prenoms saisis
            var nom = (input.Nom ?? "").Trim();
            var prenoms = (input.Prenoms ?? "").Trim();
            var nomComplet = $"{nom} {prenoms}".Trim();

            if (!string.IsNullOrWhiteSpace(nomComplet))
            {
                user.NomComplet = nomComplet;
            }

            if (input.Telephone != null)
            {
                user.PhoneNumber = input.Telephone.Trim();
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ProfileUpdateResult.Fail(errors);
            }

            return ProfileUpdateResult.Ok();
        }

        public async Task<ProfileStats> GetStatsAsync(int userId, string userRole)
        {
            var stats = new ProfileStats();

            var mesActes = await _db.TypesActModels
                .Where(a => a.IdUser == userId)
                .ToListAsync();

            stats.ActesCrees = mesActes.Count;
            stats.ActesValides = mesActes.Count(a => a.StatutWorkflow == ActeStatut.Valide);
            stats.ActesRejetes = mesActes.Count(a => a.StatutWorkflow == ActeStatut.Rejete);
            stats.ActesEnAttente = mesActes.Count(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation);
            stats.ActesArchives = mesActes.Count(a => a.StatutWorkflow == ActeStatut.Archive);

            if (userRole == RoleNames.Responsable || userRole == RoleNames.Admin)
            {
                stats.ActesValidesParMoi = await _db.TypesActModels
                    .CountAsync(a => a.ValidateurId == userId && a.StatutWorkflow == ActeStatut.Valide);

                stats.ActesRejetesParMoi = await _db.TypesActModels
                    .CountAsync(a => a.ValidateurId == userId && a.StatutWorkflow == ActeStatut.Rejete);
            }

            var lastAction = await _db.AuditLogs
                .Where(l => l.IdUser == userId)
                .OrderByDescending(l => l.DateAction)
                .FirstOrDefaultAsync();

            if (lastAction != null)
            {
                stats.LastActionLabel = GetActionLabel(lastAction.Action);
                stats.LastActionDate = lastAction.DateAction;
                stats.LastActionActeId = lastAction.IdActe;
            }

            return stats;
        }

        // ============ HELPERS ============

        /// <summary>
        /// Décompose "Jean Kouassi" en (nom="Kouassi", prenoms="Jean")
        /// ou laisse vide si NomComplet contient un seul mot.
        /// </summary>
        private static (string? nom, string? prenoms) DecomposeNomComplet(string? nomComplet)
        {
            if (string.IsNullOrWhiteSpace(nomComplet))
                return (null, null);

            var parts = nomComplet.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return (null, null);
            if (parts.Length == 1) return (parts[0], null);

            // 🇹🇬 Convention togolaise : premier mot = NOM, le reste = PRÉNOMS
            var nom = parts[0];
            var prenoms = string.Join(" ", parts.Skip(1));

            return (nom, prenoms);
        }

        private static string GetRoleLabel(string role) => role switch
        {
            RoleNames.Admin => "Administrateur",
            RoleNames.Responsable => "Responsable Juridique",
            RoleNames.Agent => "Agent Juridique",
            _ => role
        };

        private static string GetActionLabel(string action) => action switch
        {
            "CREATION" => "Création d'acte",
            "SOUMISSION" => "Envoi en validation",
            "VALIDATION" => "Validation d'acte",
            "REJET" => "Rejet d'acte",
            "REOUVERTURE" => "Ré-ouverture d'acte",
            "ARCHIVAGE" => "Archivage d'acte",
            _ => action
        };
    }
}