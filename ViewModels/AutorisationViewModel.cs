using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class AutorisationViewModel
    {
        [Required(ErrorMessage = "Numéro d'autorisation obligatoire")]
        public string NumeroAutorisation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date d'autorisation obligatoire")]
        public DateTime DateAutorisation { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Lieu d'autorisation obligatoire")]
        public string LieuAutorisation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Montant maximum obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantMaxAutorise { get; set; }

        public string ConditionsAutorisation { get; set; } = string.Empty;
    }
}
