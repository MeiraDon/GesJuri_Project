using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GesCPSI_Project.Models
{
    /// <summary>
    /// Utilisateur de l'application.
    /// Hérite d'IdentityUser<int> qui apporte : Id, UserName, Email, PasswordHash,
    /// PhoneNumber, EmailConfirmed, LockoutEnabled, AccessFailedCount, etc.
    /// </summary>
    public class UserModel : IdentityUser<int>
    {
        // ============ Champs métier supplémentaires ============

        public string? NomComplet { get; set; }

        public string? Fonction { get; set; }

        /// <summary>
        /// Si true, l'utilisateur DOIT changer son mot de passe à la prochaine connexion.
        /// Mis à true à la création par l'admin ; mis à false après changement effectif.
        /// </summary>
        public bool MustChangePassword { get; set; } = true;

        /// <summary>
        /// Désactivation logique (préférable à la suppression pour la traçabilité).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Identifiant Keycloak — pour la future migration. Null tant qu'on est sur Identity.
        /// </summary>
        public string? KeycloakId { get; set; }

        // ============ Audit ============

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime? DerniereConnexion { get; set; }

        // ============ Navigation ============

        /// <summary>
        /// Actes saisis par cet utilisateur (lien depuis TypesActModel.IdUser).
        /// </summary>
        public ICollection<TypesActModel> ActesSaisis { get; set; } = new List<TypesActModel>();
    }

}
