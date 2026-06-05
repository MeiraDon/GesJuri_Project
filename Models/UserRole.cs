using Microsoft.AspNetCore.Identity;

namespace GesCPSI_Project.Models
{
    /// <summary>
    /// Rôle utilisateur — wrapper d'IdentityRole avec clé entière (cohérent avec UserModel).
    /// </summary>
    public class UserRole : IdentityRole<int>
    {
        public UserRole() : base() { }
        public UserRole(string roleName) : base(roleName) { }

        public string? Description { get; set; }
    }
}
