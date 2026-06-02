using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class CautionnementModel
    {
        [Key]
        public int IdCautionnement { get; set; }

        [Required]
        public string NomTypesActe { get; set; } = string.Empty; /*le nom ou types ou titre des actes*/

        [Required(ErrorMessage = "Numéro obligatoire")]
        public string NumeroCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Montant obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal MontantCautionne { get; set; }

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

        [Required(ErrorMessage = "Conditions de Revocation obligatoire")]
        public string ConditionsRevocationCautionmt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Obligations de la Banque obligatoire")]
        public string ObligationsBanque { get; set; } = string.Empty;

        [Required(ErrorMessage = "Obligations de la Caution obligatoire")]
        public string ObligationsCaution { get; set; } = string.Empty;

        [Required(ErrorMessage = "Clause de Subrogation obligatoire")]
        public string ClauseSubrogation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Clause de Non Novation obligatoire")]
        public string ClauseNonNovation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Élection de domicile obligatoire")]
        public string ElectionDomicile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Règlement des différends obligatoire")]
        public string ReglementDifferend { get; set; } = string.Empty;




        // FK vers TypesActe
        [ForeignKey(nameof(TypesActModel))] // Clé unique TypesActe
        public int IdActe { get; set; }
        public TypesActModel TypesActModel { get; set; } = null!;
    }
}
