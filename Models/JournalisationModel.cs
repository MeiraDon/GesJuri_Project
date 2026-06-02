using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class JournalisationModel
    {
        //NOTIFICATION + ANNEXE + ARCHIVE
        [Key]
        public int IdNotif { get; set; }
        public string TypeNotif { get; set; } = string.Empty;// mise en demeure, prorogation…
        public DateTime DateNotif { get; set; }
        public string ContenuNotif { get; set; } = string.Empty;
        public string ModeNotif { get; set; } = string.Empty; // courrier, exploit huissier
        public string Destinataire { get; set; } = string.Empty; // caution, débiteur, banque
        public string StatutNotif { get; set; } = string.Empty; // envoyé, reçu, AR

        /* =================== ANNEXE =================== */
        public string TypeAnnexe { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Fichier { get; set; } = string.Empty; // chemin ou BLOB
        public DateTime DateUpload { get; set; }
        public string UploadedBy { get; set; } = string.Empty;


        /* =================== ARCHIVE =================== */

        public string RefPret { get; set; } = string.Empty;
        public string Dossier { get; set; } = string.Empty;
        public string Cheminfichier { get; set; } = string.Empty;
        public string Etat { get; set; } = string.Empty;
        public DateTime DateArchive { get; set; }




        // FK vers TypesActe (un historique appartient à un acte)
        public int IdActe { get; set; }

        [ForeignKey(nameof(IdActe))]
        public TypesActModel TypesActModel { get; set; } = null!;
    }
}
