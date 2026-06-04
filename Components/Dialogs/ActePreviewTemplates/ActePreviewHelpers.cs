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
    }
}
