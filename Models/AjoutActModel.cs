using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class AjoutActModel
    {
        [Key]
        public int IdAjout { get; set; }

        [Required]
        public string NomAct { get; set; } = string.Empty;
        public string? LibelleAct { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? CategorieClient { get; set; } = "Personne Physique";

        // 🆕 NOUVEAU CHAMP
        public TemplateType TemplateType { get; set; } = TemplateType.CautionnementSpecifiquePhysique;

        // Navigation : un type ajouté peut servir à créer plusieurs actes
        public ICollection<TypesActModel> TypesActModels { get; set; } = new List<TypesActModel>();
    }
}
