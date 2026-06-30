using GesCPSI_Project.Hubs;
using GesCPSI_Project.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GesCPSI_Project.Services
{
    /// <summary>
    /// Implémentation du service de notifications via SignalR.
    /// Diffuse des messages aux groupes d'utilisateurs (par rôle)
    /// via le NotificationHub.
    /// </summary>
    public class NotificationService : INotification
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // ════════════════════════════════════════════════
        // NOTIFICATIONS
        // ════════════════════════════════════════════════

        /// <summary>
        /// Notifie le groupe (rôle) cible qu'un nouvel acte a été soumis.
        /// Le client recevra l'événement "ActeSoumis" avec les détails.
        /// </summary>
        public async Task NotifyActeSoumisAsync(string targetRole, int acteId, string acteName, string senderName)
        {
            try
            {
                await _hubContext.Clients
                    .Group(targetRole)
                    .SendAsync("ActeSoumis", new
                    {
                        ActeId = acteId,
                        ActeName = acteName,
                        SenderName = senderName,
                        Timestamp = DateTime.UtcNow,
                        Message = $"Nouvel acte CPSI #{acteId} soumis par {senderName}"
                    });

                Console.WriteLine($"[SignalR] Notification ActeSoumis envoyée à '{targetRole}' (Acte #{acteId})");
            }
            catch (Exception ex)
            {
                // On ne casse JAMAIS le workflow métier si SignalR plante
                // La notif temps réel est un "bonus" UX, pas une obligation
                Console.WriteLine($"[SignalR] Erreur NotifyActeSoumisAsync : {ex.Message}");
            }
        }

        /// <summary>
        /// Notifie le groupe (rôle) cible qu'un acte a été validé.
        /// Le client recevra l'événement "ActeValide" avec les détails.
        /// </summary>
        public async Task NotifyActeValideAsync(string targetRole, int acteId, string acteName, string validateurName)
        {
            try
            {
                await _hubContext.Clients
                    .Group(targetRole)
                    .SendAsync("ActeValide", new
                    {
                        ActeId = acteId,
                        ActeName = acteName,
                        ValidateurName = validateurName,
                        Timestamp = DateTime.UtcNow,
                        Message = $"Votre acte CPSI #{acteId} a été validé par {validateurName}"
                    });

                Console.WriteLine($"[SignalR] Notification ActeValide envoyée à '{targetRole}' (Acte #{acteId})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] Erreur NotifyActeValideAsync : {ex.Message}");
            }
        }

        /// <summary>
        /// Notifie le groupe (rôle) cible qu'un acte a été rejeté.
        /// Le client recevra l'événement "ActeRejete" avec le motif.
        /// </summary>
        public async Task NotifyActeRejeteAsync(string targetRole, int acteId, string acteName, string validateurName, string motif)
        {
            try
            {
                await _hubContext.Clients
                    .Group(targetRole)
                    .SendAsync("ActeRejete", new
                    {
                        ActeId = acteId,
                        ActeName = acteName,
                        ValidateurName = validateurName,
                        Motif = motif,
                        Timestamp = DateTime.UtcNow,
                        Message = $"Votre acte CPSI #{acteId} a été rejeté par {validateurName}"
                    });

                Console.WriteLine($"[SignalR] Notification ActeRejete envoyée à '{targetRole}' (Acte #{acteId})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] Erreur NotifyActeRejeteAsync : {ex.Message}");
            }
        }
    }
}