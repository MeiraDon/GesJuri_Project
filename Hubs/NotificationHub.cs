using Microsoft.AspNetCore.SignalR;

namespace GesCPSI_Project.Hubs
{
    /// <summary>
    /// Hub SignalR pour les notifications temps réel entre les acteurs
    /// du workflow CPSI (Agent → Responsable, Responsable → Agent, etc.).
    /// 
    /// Permet l'envoi de notifications instantanées sans rechargement de page :
    /// - Badge de menu auto-actualisé
    /// - Snackbar de notification
    /// - Mise à jour automatique des listes d'actes
    /// </summary>
    public class NotificationHub : Hub
    {
        // ════════════════════════════════════════════════
        // GESTION DES GROUPES PAR RÔLE
        // ════════════════════════════════════════════════

        /// <summary>
        /// Méthode appelée par le client à la connexion
        /// pour rejoindre le groupe correspondant à son rôle.
        /// Exemples de groupes : "Responsable", "Agent", "Admin"
        /// </summary>
        public async Task JoinRoleGroup(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, roleName);
        }

        /// <summary>
        /// Méthode appelée par le client à la déconnexion
        /// pour quitter son groupe de rôle.
        /// </summary>
        public async Task LeaveRoleGroup(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roleName);
        }

        // ════════════════════════════════════════════════
        // HOOKS DE CYCLE DE VIE DES CONNEXIONS
        // ════════════════════════════════════════════════

        /// <summary>
        /// Appelé automatiquement quand un client se connecte au Hub.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"[SignalR] Client connecté : {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Appelé automatiquement quand un client se déconnecte du Hub.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[SignalR] Client déconnecté : {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}