namespace GesCPSI_Project.Models
{
    /// <summary>
    /// Cycle de vie d'un acte juridique dans GesCPSI.
    ///
    /// Brouillon → EnAttenteValidation → Valide → Archive
    ///                     ↓
    ///                  Rejete (retour à Brouillon possible)
    /// </summary>
    public enum ActeStatut
    {
        /// <summary>L'acte est en cours de saisie par un agent.</summary>
        Brouillon = 0,

        /// <summary>L'acte a été envoyé pour validation par un responsable.</summary>
        EnAttenteValidation = 1,

        /// <summary>L'acte a été validé par un responsable.</summary>
        Valide = 2,

        /// <summary>L'acte a été rejeté avec un motif. Peut être modifié et re-soumis.</summary>
        Rejete = 3,

        /// <summary>L'acte a été signé, scanné et archivé. Plus aucune modification possible.</summary>
        Archive = 4
    }

    /// <summary>Helpers pour l'affichage des statuts.</summary>
    public static class ActeStatutHelper
    {
        public static string GetLabel(ActeStatut statut) => statut switch
        {
            ActeStatut.Brouillon => "Brouillon",
            ActeStatut.EnAttenteValidation => "En attente de validation",
            ActeStatut.Valide => "Validé",
            ActeStatut.Rejete => "Rejeté",
            ActeStatut.Archive => "Archivé",
            _ => "—"
        };

        /// <summary>Renvoie la couleur MudBlazor associée au statut.</summary>
        public static string GetColorName(ActeStatut statut) => statut switch
        {
            ActeStatut.Brouillon => "default",
            ActeStatut.EnAttenteValidation => "warning",
            ActeStatut.Valide => "success",
            ActeStatut.Rejete => "error",
            ActeStatut.Archive => "info",
            _ => "default"
        };

        /// <summary>Renvoie le nom de l'icône Material associée au statut.</summary>
        public static string GetIcon(ActeStatut statut) => statut switch
        {
            ActeStatut.Brouillon => "Edit",
            ActeStatut.EnAttenteValidation => "Schedule",
            ActeStatut.Valide => "CheckCircle",
            ActeStatut.Rejete => "Cancel",
            ActeStatut.Archive => "Archive",
            _ => "HelpOutline"
        };
    }
}
