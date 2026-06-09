using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GesCPSI_Project.Models
{
    /// <summary>
    /// Journal d'audit des actions effectuées sur un acte.
    /// Chaque changement de statut (soumission, validation, rejet, archivage)
    /// y est tracé pour répondre aux exigences de conformité bancaire.
    /// </summary>
    public class AuditLogModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Acte concerné par l'action.</summary>
        public int IdActe { get; set; }
        [ForeignKey(nameof(IdActe))]
        public TypesActModel? Acte { get; set; }

        /// <summary>Code de l'action (SOUMISSION, VALIDATION, REJET, ARCHIVAGE, REOUVERTURE).</summary>
        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        /// <summary>Détails complémentaires (motif de rejet, commentaire...).</summary>
        public string? Details { get; set; }

        /// <summary>Quand l'action a été effectuée.</summary>
        public DateTime DateAction { get; set; } = DateTime.UtcNow;

        /// <summary>Utilisateur ayant effectué l'action.</summary>
        public int? IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public UserModel? User { get; set; }

        /// <summary>
        /// Statut "avant" l'action (snapshot — facilite la lecture de l'historique).
        /// </summary>
        public ActeStatut? StatutAvant { get; set; }

        /// <summary>Statut "après" l'action.</summary>
        public ActeStatut? StatutApres { get; set; }
    }
}