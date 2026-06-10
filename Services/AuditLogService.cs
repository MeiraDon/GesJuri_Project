using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class AuditLogService : IAuditLog
    {
        private readonly GesDbContext _db;
        private readonly UserManager<UserModel> _userManager;

        public AuditLogService(GesDbContext db, UserManager<UserModel> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<AuditLogEntry>> GetForActeAsync(int acteId)
        {
            // Récupère les logs (du plus récent au plus ancien)
            var logs = await _db.AuditLogs
                .Where(l => l.IdActe == acteId)
                .OrderByDescending(l => l.DateAction)
                .ToListAsync();

            if (!logs.Any()) return new List<AuditLogEntry>();

            // Récupère tous les users impliqués en une seule requête
            var userIds = logs.Where(l => l.IdUser.HasValue)
                              .Select(l => l.IdUser!.Value)
                              .Distinct()
                              .ToList();

            var users = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            // Récupère les rôles pour chaque user
            var userRoles = new Dictionary<int, string>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userRoles[u.Id] = roles.FirstOrDefault() ?? "—";
            }

            // Map vers DTO
            var entries = logs.Select(log =>
            {
                var user = log.IdUser.HasValue ? users.FirstOrDefault(u => u.Id == log.IdUser.Value) : null;
                return new AuditLogEntry
                {
                    Id = log.Id,
                    IdActe = log.IdActe,
                    Action = log.Action,
                    Details = log.Details,
                    DateAction = log.DateAction,
                    IdUser = log.IdUser,
                    UserEmail = user?.Email,
                    UserNomComplet = user?.NomComplet ?? user?.Email,
                    UserRole = user is not null && userRoles.TryGetValue(user.Id, out var r) ? r : "—",
                    StatutAvant = log.StatutAvant,
                    StatutApres = log.StatutApres
                };
            }).ToList();

            return entries;
        }

        public async Task<List<AuditLogEntry>> GetAllAsync(AuditLogFilter? filter = null)
        {
            var query = _db.AuditLogs.AsQueryable();

            // Filtre par date
            if (filter?.From.HasValue == true)
                query = query.Where(l => l.DateAction >= filter.From.Value);

            if (filter?.To.HasValue == true)
            {
                var to = filter.To.Value.AddDays(1); // inclure toute la journée de fin
                query = query.Where(l => l.DateAction < to);
            }

            // Filtre par action
            if (!string.IsNullOrWhiteSpace(filter?.Action))
                query = query.Where(l => l.Action == filter.Action);

            // Filtre par utilisateur ayant fait l'action
            if (filter?.UserId.HasValue == true)
                query = query.Where(l => l.IdUser == filter.UserId.Value);

            // Filtre par acte
            if (filter?.ActeId.HasValue == true)
                query = query.Where(l => l.IdActe == filter.ActeId.Value);

            // 🛡️ Restriction Agent : ne voit que les actions sur SES actes
            if (filter?.RestrictToAgentId.HasValue == true)
            {
                var agentActeIds = await _db.TypesActModels
                    .Where(a => a.IdUser == filter.RestrictToAgentId.Value)
                    .Select(a => a.IdActe)
                    .ToListAsync();

                query = query.Where(l => agentActeIds.Contains(l.IdActe));
            }

            var logs = await query
                .OrderByDescending(l => l.DateAction)
                .Take(500) // Limite de sécurité
                .ToListAsync();

            return await EnrichWithUsersAsync(logs);
        }

        public async Task<AuditLogStats> GetStatsAsync(int? restrictToAgentId = null)
        {
            var query = _db.AuditLogs.AsQueryable();

            if (restrictToAgentId.HasValue)
            {
                var agentActeIds = await _db.TypesActModels
                    .Where(a => a.IdUser == restrictToAgentId.Value)
                    .Select(a => a.IdActe)
                    .ToListAsync();

                query = query.Where(l => agentActeIds.Contains(l.IdActe));
            }

            var now = DateTime.UtcNow;
            var today = now.Date;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var monthStart = new DateTime(today.Year, today.Month, 1);

            return new AuditLogStats
            {
                Total = await query.CountAsync(),
                Today = await query.CountAsync(l => l.DateAction >= today),
                ThisWeek = await query.CountAsync(l => l.DateAction >= weekStart),
                ThisMonth = await query.CountAsync(l => l.DateAction >= monthStart)
            };
        }

        // ============ HELPER PRIVÉ ============
        private async Task<List<AuditLogEntry>> EnrichWithUsersAsync(List<AuditLogModel> logs)
        {
            if (!logs.Any()) return new List<AuditLogEntry>();

            var userIds = logs.Where(l => l.IdUser.HasValue)
                              .Select(l => l.IdUser!.Value)
                              .Distinct()
                              .ToList();

            var users = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var userRoles = new Dictionary<int, string>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userRoles[u.Id] = roles.FirstOrDefault() ?? "—";
            }

            return logs.Select(log =>
            {
                var user = log.IdUser.HasValue ? users.FirstOrDefault(u => u.Id == log.IdUser.Value) : null;
                return new AuditLogEntry
                {
                    Id = log.Id,
                    IdActe = log.IdActe,
                    Action = log.Action,
                    Details = log.Details,
                    DateAction = log.DateAction,
                    IdUser = log.IdUser,
                    UserEmail = user?.Email,
                    UserNomComplet = user?.NomComplet ?? user?.Email,
                    UserRole = user is not null && userRoles.TryGetValue(user.Id, out var r) ? r : "—",
                    StatutAvant = log.StatutAvant,
                    StatutApres = log.StatutApres
                };
            }).ToList();
        }
    }
}