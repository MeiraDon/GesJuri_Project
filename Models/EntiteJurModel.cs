using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class EntiteJurModel
    {
        [Key]
        public int IdEntiteJur { get; set; }

        [Required(ErrorMessage = "Date de l'acte obligatoire")]
        public DateTime DateActeJuridique { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Référence obligatoire")]
        public string Reference { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nom de l'huissier obligatoire")]
        public string NomHuissier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ville de l'huissier obligatoire")]
        public string VilleHuissier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numéro de dossier obligatoire")]
        public string NumeroDossier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Responsable obligatoire")]
        public string Responsable { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fonction du responsable obligatoire")]
        public string FonctionResponsable { get; set; } = string.Empty;

        [Required(ErrorMessage = "NomDossier obligatoire")]
        public string NomDossier { get; set; } = string.Empty;




        // FK vers TypesActe
        [ForeignKey(nameof(TypesActModel))] // Clé unique TypesActe
        public int IdActe { get; set; }
        public TypesActModel TypesActModel { get; set; } = null!;
    }
}
