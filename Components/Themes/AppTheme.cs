using MudBlazor;

namespace GesCPSI_Project.Components.Theme
{
    public static class AppTheme
    {
        public static readonly MudTheme Default = new()
        {
            PaletteLight = new PaletteLight
            {
                // ===== Couleurs principales =====
                Primary = "#0A2540",          // Bleu marine BOA
                PrimaryDarken = "#06182E",
                PrimaryLighten = "#1F4068",
                PrimaryContrastText = "#FFFFFF",

                Secondary = "#0E7C5C",        // Vert BOA
                SecondaryDarken = "#0A5C44",
                SecondaryLighten = "#1B9C76",

                Tertiary = "#C9A86A",         // Or sobre

                // ===== Statuts =====
                Success = "#0E7C5C",
                Info = "#1F4068",
                Warning = "#D97706",
                Error = "#9B1C2D",

                // ===== Fonds =====
                Background = "#FAFAF7",       // Blanc cassé
                Surface = "#FFFFFF",
                AppbarBackground = "#0A2540", // Header marine
                AppbarText = "#FFFFFF",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#1A2332",
                DrawerIcon = "#0A2540",

                // ===== Textes =====
                TextPrimary = "#1A2332",
                TextSecondary = "#5E6B7D",
                TextDisabled = "#A0A8B5",

                // ===== Lignes & bordures =====
                LinesDefault = "#E1E4E8",
                LinesInputs = "#C7CCD4",
                Divider = "#E1E4E8",

                // ===== Actions =====
                ActionDefault = "#5E6B7D",
                ActionDisabled = "#A0A8B5",
                ActionDisabledBackground = "#F1F3F5",

                HoverOpacity = 0.06
            },

            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Inter", "Segoe UI", "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".9rem",
                    FontWeight = "400",
                    LineHeight = "1.5",
                    LetterSpacing = ".009375em"
                },
                H1 = new H1Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "2.5rem", FontWeight = "700" },
                H2 = new H2Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "2rem", FontWeight = "700" },
                H3 = new H3Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "1.5rem", FontWeight = "600" },
                H4 = new H4Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "1.25rem", FontWeight = "600" },
                H5 = new H5Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "1.125rem", FontWeight = "600" },
                H6 = new H6Typography { FontFamily = new[] { "Inter", "sans-serif" }, FontSize = "1rem", FontWeight = "600" },
                Button = new ButtonTypography { TextTransform = "none", FontWeight = "500", LetterSpacing = ".02em" }
            },

            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "8px",
                AppbarHeight = "64px",
                DrawerWidthLeft = "260px"
            },

            Shadows = new Shadow()
        };
    }
}