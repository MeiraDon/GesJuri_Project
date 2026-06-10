using GesCPSI_Project.Models;

namespace GesCPSI_Project.Interfaces
{
    /// <summary>
    /// DTO enrichi pour l'affichage d'un événement dans la timeline.
    /// </summary>
    public class AuditLogEntry
    {
        public int Id { get; set; }
        public int IdActe { get; set; }
        public string Action { get; set; } = "";
        public string? Details { get; set; }
        public DateTime DateAction { get; set; }

        public int? IdUser { get; set; }
        public string? UserEmail { get; set; }
        public string? UserNomComplet { get; set; }
        public string? UserRole { get; set; }

        public ActeStatut? StatutAvant { get; set; }
        public ActeStatut? StatutApres { get; set; }
    }

    public interface IAuditLog
    {
        /// <summary>Historique d'un acte spécifique (déjà existant)</summary>
        Task<List<AuditLogEntry>> GetForActeAsync(int acteId);

        /// <summary>
        /// Historique global avec filtres.
        /// Si userId fourni, ne retourne QUE les actions sur les actes saisis par ce user
        /// (utilisé pour limiter la vue d'un Agent à ses propres actes).
        /// </summary>
        Task<List<AuditLogEntry>> GetAllAsync(AuditLogFilter? filter = null);

        /// <summary>Stats rapides pour le dashboard de l'historique.</summary>
        Task<AuditLogStats> GetStatsAsync(int? restrictToAgentId = null);
    }

    public class AuditLogFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Action { get; set; }
        public int? UserId { get; set; }
        public int? ActeId { get; set; }
        /// <summary>Si fourni, ne retourne que les logs sur les actes saisis par ce user.</summary>
        public int? RestrictToAgentId { get; set; }
    }

    public class AuditLogStats
    {
        public int Total { get; set; }
        public int Today { get; set; }
        public int ThisWeek { get; set; }
        public int ThisMonth { get; set; }
    }
}