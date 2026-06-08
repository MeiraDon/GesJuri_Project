using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GesCPSI_Project.Services
{
    public class UserAdminService : IUserAdmin
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly GesDbContext _db;

        public UserAdminService(
            UserManager<UserModel> userManager,
            RoleManager<UserRole> roleManager,
            GesDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task<List<UserAdminDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserAdminDto>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(MapToDto(u, roles.FirstOrDefault()));
            }

            return result.OrderByDescending(x => x.DateCreation).ToList();
        }

        public async Task<UserAdminDto?> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return MapToDto(user, roles.FirstOrDefault());
        }

        public async Task<UserAdminResult> CreateAsync(UserCreateInput input)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(input.Email))
                return UserAdminResult.Fail("L'email est obligatoire.");

            if (string.IsNullOrWhiteSpace(input.NomComplet))
                return UserAdminResult.Fail("Le nom complet est obligatoire.");

            if (!RoleNames.All.Contains(input.Role))
                return UserAdminResult.Fail($"Le rôle '{input.Role}' n'existe pas.");

            // Email déjà pris ?
            var existing = await _userManager.FindByEmailAsync(input.Email);
            if (existing is not null)
                return UserAdminResult.Fail("Cet email est déjà utilisé.");

            // Mot de passe : utilise celui fourni ou en génère un
            var password = string.IsNullOrWhiteSpace(input.TemporaryPassword)
                ? GenerateTemporaryPassword()
                : input.TemporaryPassword;

            var user = new UserModel
            {
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = true,
                NomComplet = input.NomComplet,
                Fonction = input.Fonction,
                IsActive = true,
                MustChangePassword = true,
                DateCreation = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return UserAdminResult.Fail($"Création échouée : {errors}");
            }

            // Assigner le rôle
            await _userManager.AddToRoleAsync(user, input.Role);

            var dto = MapToDto(user, input.Role);
            return UserAdminResult.Ok(dto, password);
        }

        public async Task<UserAdminResult> UpdateAsync(UserUpdateInput input)
        {
            var user = await _userManager.FindByIdAsync(input.Id.ToString());
            if (user is null)
                return UserAdminResult.Fail("Utilisateur introuvable.");

            if (!RoleNames.All.Contains(input.Role))
                return UserAdminResult.Fail($"Le rôle '{input.Role}' n'existe pas.");

            // Mise à jour des champs
            user.NomComplet = input.NomComplet;
            user.Fonction = input.Fonction;
            user.IsActive = input.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                return UserAdminResult.Fail($"Mise à jour échouée : {errors}");
            }

            // Mise à jour du rôle si changé
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(input.Role))
            {
                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, input.Role);
            }

            var dto = MapToDto(user, input.Role);
            return UserAdminResult.Ok(dto);
        }

        public async Task<UserAdminResult> ToggleActiveAsync(int id, int currentUserId)
        {
            // Sécurité : l'admin ne peut pas se désactiver lui-même
            if (id == currentUserId)
                return UserAdminResult.Fail("Vous ne pouvez pas désactiver votre propre compte.");

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return UserAdminResult.Fail("Utilisateur introuvable.");

            // Si on désactive un admin, vérifier qu'il reste au moins un autre admin actif
            if (user.IsActive)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(RoleNames.Admin))
                {
                    var activeAdmins = await GetActiveAdminCountAsync();
                    if (activeAdmins <= 1)
                        return UserAdminResult.Fail("Impossible de désactiver le dernier administrateur actif.");
                }
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            var rolesAfter = await _userManager.GetRolesAsync(user);
            var dto = MapToDto(user, rolesAfter.FirstOrDefault());
            return UserAdminResult.Ok(dto);
        }

        public async Task<UserAdminResult> ResetPasswordAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return UserAdminResult.Fail("Utilisateur introuvable.");

            // Génère un nouveau mot de passe temporaire
            var newPassword = GenerateTemporaryPassword();

            // Reset via Identity
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return UserAdminResult.Fail($"Reset échoué : {errors}");
            }

            // Forcer le changement au prochain login
            user.MustChangePassword = true;
            await _userManager.UpdateAsync(user);

            return UserAdminResult.Ok(generatedPassword: newPassword);
        }

        public async Task<UserAdminResult> DeleteAsync(int id, int currentUserId)
        {
            if (id == currentUserId)
                return UserAdminResult.Fail("Vous ne pouvez pas supprimer votre propre compte.");

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return UserAdminResult.Fail("Utilisateur introuvable.");

            // Si c'est un admin, vérifier qu'il reste au moins un autre admin actif
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(RoleNames.Admin))
            {
                var activeAdmins = await GetActiveAdminCountAsync();
                if (activeAdmins <= 1)
                    return UserAdminResult.Fail("Impossible de supprimer le dernier administrateur.");
            }

            // Vérifier si l'utilisateur a saisi des actes (intégrité référentielle)
            var hasActes = await _db.TypesActModels.AnyAsync(t => t.IdUser == id);
            if (hasActes)
                return UserAdminResult.Fail("Cet utilisateur a saisi des actes. Désactivez-le plutôt que de le supprimer.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return UserAdminResult.Fail($"Suppression échouée : {errors}");
            }

            return UserAdminResult.Ok();
        }

        // ===== HELPERS =====

        private async Task<int> GetActiveAdminCountAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync(RoleNames.Admin);
            return admins.Count(u => u.IsActive);
        }

        private static UserAdminDto MapToDto(UserModel user, string? role) => new()
        {
            Id = user.Id,
            Email = user.Email ?? "",
            NomComplet = user.NomComplet,
            Fonction = user.Fonction,
            Role = role,
            IsActive = user.IsActive,
            MustChangePassword = user.MustChangePassword,
            DateCreation = user.DateCreation,
            DerniereConnexion = user.DerniereConnexion
        };

        /// <summary>
        /// Génère un mot de passe temporaire respectant la policy Identity :
        /// majuscule, minuscule, chiffre, 8 caractères minimum.
        /// </summary>
        private static string GenerateTemporaryPassword()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnpqrstuvwxyz";
            const string digits = "23456789";
            const string special = "!@#$%";

            var rng = RandomNumberGenerator.Create();
            char Pick(string source)
            {
                var bytes = new byte[1];
                rng.GetBytes(bytes);
                return source[bytes[0] % source.Length];
            }

            // 2 majuscules + 3 minuscules + 2 chiffres + 1 spécial = 8 caractères
            var chars = new[]
            {
                Pick(upper), Pick(upper),
                Pick(lower), Pick(lower), Pick(lower),
                Pick(digits), Pick(digits),
                Pick(special)
            };

            // Mélanger
            return new string(chars.OrderBy(_ =>
            {
                var b = new byte[1];
                rng.GetBytes(b);
                return b[0];
            }).ToArray());
        }
    }
}