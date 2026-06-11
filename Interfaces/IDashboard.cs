using GesCPSI_Project.Models;

namespace GesCPSI_Project.Interfaces
{
    // ============ DTOs ============

    /// <summary>Stats principales pour les 4 cards en haut.</summary>
    public class DashboardStats
    {
        public int Total { get; set; }
        public int Brouillons { get; set; }
        public int EnAttente { get; set; }
        public int Valides { get; set; }
        public int Archives { get; set; }
        public int Rejetes { get; set; }

        /// <summary>Nombre d'actes créés ce mois-ci.</summary>
        public int TotalCeMois { get; set; }

        /// <summary>Nombre d'actes validés cette semaine.</summary>
        public int ValidesCetteSemaine { get; set; }

        /// <summary>Nombre d'actes en attente depuis +3 jours (urgent).</summary>
        public int EnAttenteUrgents { get; set; }
    }

    /// <summary>Point de la courbe d'évolution mensuelle.</summary>
    public class MonthlyDataPoint
    {
        public string Label { get; set; } = "";   // ex: "Jan 26"
        public int Count { get; set; }
        public DateTime MonthStart { get; set; }
    }

    /// <summary>Item de la liste "À traiter".</summary>
    public class PendingActeItem
    {
        public int IdActe { get; set; }
        public string NomTypesActe { get; set; } = "";
        public ActeStatut Statut { get; set; }
        public DateTime? DateEnvoiValidation { get; set; }
        public DateTime DateCreation { get; set; }
        public string? AgentNom { get; set; }
        public string WaitingTime { get; set; } = "";
        public bool IsUrgent { get; set; }
    }

    /// <summary>Événement d'activité récente.</summary>
    public class RecentActivityItem
    {
        public int IdActe { get; set; }
        public string Action { get; set; } = "";
        public string? Details { get; set; }
        public DateTime DateAction { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string TimeAgo { get; set; } = "";
        public ActeStatut? StatutApres { get; set; }
    }

    public interface IDashboard
    {
        /// <summary>
        /// Récupère toutes les stats principales.
        /// Si agentId fourni, retourne uniquement les stats de cet agent.
        /// </summary>
        Task<DashboardStats> GetStatsAsync(int? restrictToAgentId = null);

        /// <summary>Évolution mensuelle sur les 6 derniers mois.</summary>
        Task<List<MonthlyDataPoint>> GetMonthlyEvolutionAsync(int? restrictToAgentId = null);

        /// <summary>
        /// Liste des actes prioritaires à traiter.
        /// Pour Agent : ses actes en Brouillon ou Rejeté
        /// Pour Responsable/Admin : actes en attente de validation
        /// </summary>
        Task<List<PendingActeItem>> GetPendingActesAsync(string userRole, int? userId, int limit = 5);

        /// <summary>10 derniers événements d'activité.</summary>
        Task<List<RecentActivityItem>> GetRecentActivityAsync(int? restrictToAgentId = null, int limit = 10);
    }
}