using System.Globalization;

namespace GesCPSI_Project.Components.Dialogs.ActePreviewTemplates
{
    public static class ActePreviewHelpers
    {
        private static readonly CultureInfo Fr = new("fr-FR");

        public static string FormatDate(DateTime d)
        {
            if (d == DateTime.MinValue || d == default) return "________________";
            return d.ToString("dd/MM/yyyy", Fr);
        }

        public static string FormatDateLong(DateTime d)
        {
            if (d == DateTime.MinValue || d == default) return "________________";
            return d.ToString("dd MMMM yyyy", Fr);
        }

        public static string FormatMontant(decimal m)
        {
            if (m == 0) return "________________";
            return m.ToString("N0", Fr);
        }

        public static string FormatSituation(string? situation, string? conjoint)
        {
            if (string.IsNullOrWhiteSpace(situation)) return "";
            if (situation.StartsWith("Marié", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(conjoint))
            {
                return $"marié(e) à M {conjoint}";
            }
            return situation.ToLowerInvariant();
        }

        /// <summary>
        /// Formate les informations matrimoniales complètes selon le modèle juridique BOA.
        /// Exemple : "célibataire" ou "époux(se) de M Marie KODJO, mariés le 15/06/2010 
        /// à la mairie de Lomé sous le régime de la communauté réduite aux acquêts"
        /// </summary>
        public static string FormatSituationDetaillee(
            string? situationMatrim,
            string? nomConjoint,
            DateTime? dateMariage,
            string? lieuMariage,
            string? regimeMatrim)
        {
            if (string.IsNullOrWhiteSpace(situationMatrim))
                return "célibataire";

            var situation = situationMatrim.Trim().ToLowerInvariant();

            // Cas célibataire/divorcé/veuf : pas de détails mariage
            if (situation.Contains("célib") || situation.Contains("celib"))
                return "célibataire";

            if (situation.Contains("divorc"))
                return "divorcé(e)";

            if (situation.Contains("veu") || situation.Contains("vve"))
                return "veuf(ve)";

            // Cas marié(e) : on assemble les détails
            var parts = new List<string> { "époux(se)" };

            if (!string.IsNullOrWhiteSpace(nomConjoint))
                parts.Add($"de M {nomConjoint.Trim()}");

            if (dateMariage.HasValue && dateMariage.Value != DateTime.MinValue)
                parts.Add($"mariés le {dateMariage.Value:dd/MM/yyyy}");

            if (!string.IsNullOrWhiteSpace(lieuMariage))
                parts.Add($"à la mairie de {lieuMariage.Trim()}");

            if (!string.IsNullOrWhiteSpace(regimeMatrim))
                parts.Add($"sous le régime de {regimeMatrim.Trim()}");

            return string.Join(", ", parts);
        }

    }
}
