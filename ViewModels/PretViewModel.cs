using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class PretViewModel
    {
        [Required(ErrorMessage = "Référence obligatoire")]
        public string ReferenceConvention { get; set; } = string.Empty;

        [Required(ErrorMessage = "Montant du concours obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantConcours { get; set; }

        [Required(ErrorMessage = "Le taux est obligatoire")]
        [Range(0, 100, ErrorMessage = "Le taux doit être entre 0 et 100")]
        public decimal Taux { get; set; }

        [Required(ErrorMessage = "Montant en lettres obligatoire")]
        public string MontantLettrePret { get; set; } = string.Empty;

        public string Accessoires { get; set; } = string.Empty;

        [Required(ErrorMessage = "Objet du prêt obligatoire")]
        public string ObjetPret { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date de signature obligatoire")]
        public DateTime DateSignaturePret { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Durée obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "La durée doit être supérieure à 0")]
        public int DureMoisPret { get; set; }

        [Required(ErrorMessage = "Date début remboursement obligatoire")]
        public DateTime DateDebutRemboursemnt { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Date première échéance obligatoire")]
        public DateTime DatePremiereEcheance { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Montant première échéance obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantPremiereEcheance { get; set; }

        [Required(ErrorMessage = "Date dernière échéance obligatoire")]
        public DateTime DateDerniereEcheance { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Montant dernière échéance obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantDerniereEcheance { get; set; }

        [Required(ErrorMessage = "État du prêt obligatoire")]
        public string EtatPret { get; set; } = string.Empty;
    }
}
