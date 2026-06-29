using GesCPSI_Project.Interfaces;

namespace GesCPSI_Project.Models
{
    public class RapportActiviteDto
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string GenereParEmail { get; set; } = "";
        public string GenereParRole { get; set; } = "";
        public DateTime DateGeneration { get; set; } = DateTime.Now;

        // Statistiques globales
        public int Total { get; set; }
        public int Valides { get; set; }
        public int EnAttente { get; set; }
        public int Brouillons { get; set; }
        public int Rejetes { get; set; }
        public int Archives { get; set; }

        // Taux
        public double TauxValidation { get; set; }
        public double TauxRejet { get; set; }
        public double DureeMoyenneValidation { get; set; } // en jours

        // Détails par utilisateur (pour Admin/Responsable)
        public List<UserActivityStat> ParUtilisateur { get; set; } = new();

        // Évolution mensuelle (12 mois)
        public List<MonthlyDataPoint> Evolution { get; set; } = new();

        // Top banques concernées
        public List<BankStat> TopBanques { get; set; } = new();
    }

    public class UserActivityStat
    {
        public string NomComplet { get; set; } = "";
        public string Email { get; set; } = "";
        public int Total { get; set; }
        public int Valides { get; set; }
        public int Rejetes { get; set; }
    }

    public class BankStat
    {
        public string NomBanque { get; set; } = "";
        public int NombreActes { get; set; }
    }
}