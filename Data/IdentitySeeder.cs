using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace GesCPSI_Project.Data
{
    /// <summary>
    /// Seed initial des rôles et du compte administrateur par défaut.
    /// Exécuté au démarrage de l'application.
    /// </summary>
    public class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
            var userManager = services.GetRequiredService<UserManager<UserModel>>();

            // ===== 1. Créer les rôles s'ils n'existent pas =====
            foreach (var roleName in RoleNames.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new UserRole(roleName)
                    {
                        Description = roleName switch
                        {
                            RoleNames.Admin => "Administrateur — gère tout (utilisateurs, banques, types d'actes, paramètres)",
                            RoleNames.Agent => "Agent juridique — saisit les actes et les envoie en validation",
                            RoleNames.Responsable => "Responsable juridique — valide les actes envoyés",
                            _ => null
                        }
                    };

                    await roleManager.CreateAsync(role);
                }
            }

            // ===== 2. Créer le compte admin par défaut s'il n'existe pas =====
            const string adminEmail = "admin@boatogo.com";
            const string adminPassword = "Admin@2026";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new UserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NomComplet = "Administrateur Système",
                    Fonction = "Administrateur",
                    IsActive = true,
                    MustChangePassword = true,
                    DateCreation = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RoleNames.Admin);
                }
                else
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Échec de création du compte admin par défaut : {errors}");
                }
            }
        }
    }
}
