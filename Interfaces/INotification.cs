namespace GesCPSI_Project.Interfaces
{
    /// <summary>
    /// Service de notifications temps réel via SignalR.
    /// Permet de notifier les utilisateurs (par rôle) lors d'événements
    /// clés du workflow CPSI (soumission, validation, rejet, etc.).
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Notifie tous les utilisateurs d'un rôle donné qu'un nouvel acte
        /// a été soumis pour validation.
        /// Exemple : Agent envoie un acte → tous les Responsables connectés
        /// reçoivent un badge incrémenté + un snackbar.
        /// </summary>
        /// <param name="targetRole">Le rôle destinataire (ex: "Responsable")</param>
        /// <param name="acteId">L'ID de l'acte concerné</param>
        /// <param name="acteName">Le nom/type de l'acte (pour affichage)</param>
        /// <param name="senderName">Le nom de l'expéditeur (pour affichage)</param>
        Task NotifyActeSoumisAsync(string targetRole, int acteId, string acteName, string senderName);

        /// <summary>
        /// Notifie un utilisateur spécifique (l'agent qui a créé l'acte)
        /// que son acte a été validé par le Responsable.
        /// </summary>
        /// <param name="targetRole">Le rôle destinataire (ex: "Agent")</param>
        /// <param name="acteId">L'ID de l'acte validé</param>
        /// <param name="acteName">Le nom/type de l'acte</param>
        /// <param name="validateurName">Le nom du validateur</param>
        Task NotifyActeValideAsync(string targetRole, int acteId, string acteName, string validateurName);

        /// <summary>
        /// Notifie un utilisateur spécifique (l'agent qui a créé l'acte)
        /// que son acte a été rejeté avec un motif.
        /// </summary>
        /// <param name="targetRole">Le rôle destinataire (ex: "Agent")</param>
        /// <param name="acteId">L'ID de l'acte rejeté</param>
        /// <param name="acteName">Le nom/type de l'acte</param>
        /// <param name="validateurName">Le nom du validateur (qui a rejeté)</param>
        /// <param name="motif">Le motif du rejet</param>
        Task NotifyActeRejeteAsync(string targetRole, int acteId, string acteName, string validateurName, string motif);
    }
}