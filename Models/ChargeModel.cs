using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class ChargeModel
    {
        [Key]
        public int IdCharge { get; set; }

        [Required(ErrorMessage = "Type de charge obligatoire")]
        public string TypeCharge { get; set; } = string.Empty;// timbre, impôt, OTR

        [Required(ErrorMessage = "Montant obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le montant ne peut pas être négatif")]
        public decimal MontantCharge { get; set; }

        [Required(ErrorMessage = "Date obligatoire")]
        public DateTime DateCharge { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Payé par obligatoire")]
        public string PayePar { get; set; } = string.Empty; // débiteur, caution, banque

        [Required(ErrorMessage = "Statut de la charge obligatoire")]
        public string StatutCharge { get; set; } = string.Empty;// payée, en attente, contestée





        // FK vers TypesActe
        [ForeignKey(nameof(TypesActModel))] // Clé unique TypesActe
        public int IdActe { get; set; }
        public TypesActModel TypesActModel { get; set; } = null!;
    }
}
