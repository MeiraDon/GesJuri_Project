using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class CautionnementViewModel
    {

        public string NomTypesActe { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numéro obligatoire")]
        public string NumeroCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Montant obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantCautionne { get; set; }   // corrigé

        [Required(ErrorMessage = "Montant en lettres obligatoire")]
        public string MontantLettreCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date de signature obligatoire")]
        public DateTime DateSignatureCautionmt { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Date d'effet obligatoire")]
        public DateTime DateEffetCautionmt { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Date d'expiration obligatoire")]
        public DateTime DateExpirationCautionmt { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Lieu de signature obligatoire")]
        public string LieuSignatureCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "État de cautionnement obligatoire")]
        public string EtatCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Conditions de révocation obligatoires")]
        public string ConditionsRevocationCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Obligations de la Banque obligatoires")]
        public string ObligationsBanque { get; set; } = string.Empty;

        [Required(ErrorMessage = "Obligations de la Caution obligatoires")]
        public string ObligationsCaution { get; set; } = string.Empty;

        [Required(ErrorMessage = "Clause de subrogation obligatoire")]
        public string ClauseSubrogation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Clause de non novation obligatoire")]
        public string ClauseNonNovation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Élection de domicile obligatoire")]
        public string ElectionDomicile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Règlement des différends obligatoire")]
        public string ReglementDifferend { get; set; } = string.Empty;
    }
}
