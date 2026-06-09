using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GesCPSI_Project.Models
{
    public class TypesActModel
    {
        [Key]
        public int IdActe { get; set; }

        [Required]
        public string NomTypesActe { get; set; } = string.Empty; /*le nom ou types ou titre des actes*/
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public string Statut { get; set; } = "Brouillon"; // Brouillon | Valide
        // 🆕 Nouveau statut typé avec enum (utilisé par le workflow)
        public ActeStatut StatutWorkflow { get; set; } = ActeStatut.Brouillon;
        public DateTime DateMaj { get; set; } = DateTime.UtcNow;
        public string? MotifRejet { get; set; }
        // 🆕 Date à laquelle l'agent envoie l'acte en validation
        public DateTime? DateEnvoiValidation { get; set; }

        public DateTime? DateValidation { get; set; } 
        public DateTime? DateRejet { get; set; } 
        public DateTime? DateArchivage { get; set; } 
        public int? ValidateurId { get; set; }

        // ============ FICHIERS ============
        public string? FichierValidationPath { get; set; } // chemin MinIO du fichier uploadé par le validateur.
        public string? PdfGenerePath { get; set; }
        public string? PdfSignePath { get; set; }
        public string? JsonSnapshotPath { get; set; }
        public DateTime? DateGenerationPdf { get; set; }
        public DateTime? DateUploadSignature { get; set; }

        // ================= RELATION AVEC AjoutTypeAct =================
        public int IdAjout { get; set; }

        [ForeignKey(nameof(IdAjout))]
        public AjoutActModel AjoutActModel { get; set; } = null!;

        // ================= CREATEUR DE L'ACTE =================
        public int? IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        public UserModel? UserModel { get; set; } 

        // ================= BANQUE =================
        public int? IdBnq { get; set; }

        [ForeignKey(nameof(IdBnq))]
        public BanqueModel? BanqueModel { get; set; }

        // Spécialisations (1–1)
        public CautionnementModel? CautionnementModel { get; set; }
        public AutorisationModel? AutorisationModel { get; set; }
        public PretModel? PretModel { get; set; }
        public ChargeModel? ChargeModel { get; set; }
        public EntiteJurModel? EntiteJurModel { get; set; }

        // ================= NAVIGATIONS =================
        public ICollection<ClientActModel> ClientActModels { get; set; } = new List<ClientActModel>();
        public ICollection<JournalisationModel> JournalisationModels { get; set; } = new List<JournalisationModel>();
    }
}
