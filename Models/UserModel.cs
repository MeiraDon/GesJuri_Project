using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class UserModel
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        public string NameUser { get; set; } = string.Empty;
        [Required]
        public string SurnameUser { get; set; } = string.Empty;
        [Required]
        public string EmailUser { get; set; } = string.Empty;
        [Required]
        public string KeycloakId { get; set; } = string.Empty; // ID Keycloak (obligatoire)
        public string Departement { get; set; } = "Juridique";// par défaut
        public string Fonction { get; set; } = string.Empty;// Agent, Responsable, Admin

        public DateTime DateCreation { get; set; } = DateTime.UtcNow; /*date de creatiion de l'acte*/
        public DateTime? DateModification { get; set; }  /*date de modification de l'acte*/
        public bool IsActive { get; set; } = true;


        // Navigation: un utilisateur peut créer plusieurs actes
        public ICollection<TypesActModel> TypesActModels { get; set; } = new List<TypesActModel>();
    }
}
