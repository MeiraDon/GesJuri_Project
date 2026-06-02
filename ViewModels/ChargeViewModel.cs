using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class ChargeViewModel
    {
        [Required(ErrorMessage = "Type de charge obligatoire")]
        public string TypeCharge { get; set; } = string.Empty;

        [Required(ErrorMessage = "Montant obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le montant ne peut pas être négatif")]
        public decimal MontantCharge { get; set; }

        [Required(ErrorMessage = "Date obligatoire")]
        public DateTime DateCharge { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Payé par obligatoire")]
        public string PayePar { get; set; } = string.Empty;

        [Required(ErrorMessage = "Statut de la charge obligatoire")]
        public string StatutCharge { get; set; } = string.Empty;
    }
}
