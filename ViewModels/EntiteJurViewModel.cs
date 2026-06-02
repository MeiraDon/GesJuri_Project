using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class EntiteJurViewModel
    {
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

        [Required(ErrorMessage = "Nom du dossier obligatoire")]
        public string NomDossier { get; set; } = string.Empty;   // ajouté

        [Required(ErrorMessage = "Responsable obligatoire")]
        public string Responsable { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fonction du responsable obligatoire")]
        public string FonctionResponsable { get; set; } = string.Empty;
    }
}
