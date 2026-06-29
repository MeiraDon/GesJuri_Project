using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GesCPSI_Project.Services
{
    public class DashboardService : IDashboard
    {
        private readonly GesDbContext _db;
        private readonly UserManager<UserModel> _userManager;

        public DashboardService(GesDbContext db, UserManager<UserModel> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ============================================================
        // STATS PRINCIPALES
        // ============================================================
        public async Task<DashboardStats> GetStatsAsync(int? restrictToAgentId = null)
        {
            var query = _db.TypesActModels.AsQueryable();

            if (restrictToAgentId.HasValue)
                query = query.Where(a => a.IdUser == restrictToAgentId.Value);

            var actes = await query.ToListAsync();

            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var weekStart = DateTime.SpecifyKind(now.Date.AddDays(-(int)now.Date.DayOfWeek), DateTimeKind.Utc);
            var threeDaysAgo = now.AddDays(-3);
            var sevenDaysAgo = now.AddDays(-7);   // ← NOUVEAU

            return new DashboardStats
            {
                Total = actes.Count,
                Brouillons = actes.Count(a => a.StatutWorkflow == ActeStatut.Brouillon),
                EnAttente = actes.Count(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation),
                Valides = actes.Count(a => a.StatutWorkflow == ActeStatut.Valide),
                Archives = actes.Count(a => a.StatutWorkflow == ActeStatut.Archive),
                Rejetes = actes.Count(a => a.StatutWorkflow == ActeStatut.Rejete),

                TotalCeMois = actes.Count(a => a.DateCreation >= monthStart),
                ValidesCetteSemaine = actes.Count(a =>
                    a.StatutWorkflow == ActeStatut.Valide
                    && a.DateValidation.HasValue
                    && a.DateValidation.Value >= weekStart),

                EnAttenteUrgents = actes.Count(a =>
                    a.StatutWorkflow == ActeStatut.EnAttenteValidation
                    && a.DateEnvoiValidation.HasValue
                    && a.DateEnvoiValidation.Value <= threeDaysAgo),

                // ← NOUVEAU : brouillons qui traînent depuis + de 7 jours
                BrouillonsAnciens = actes.Count(a =>
                    a.StatutWorkflow == ActeStatut.Brouillon
                    && a.DateCreation <= sevenDaysAgo)
            };
        }

        // ============================================================
        // ÉVOLUTION MENSUELLE (6 derniers mois)
        // ============================================================
        public async Task<List<MonthlyDataPoint>> GetMonthlyEvolutionAsync(int? restrictToAgentId = null)
        {
            var now = DateTime.UtcNow;
            var sixMonthsAgo = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-11);

            var query = _db.TypesActModels
                .Where(a => a.DateCreation >= sixMonthsAgo);

            if (restrictToAgentId.HasValue)
                query = query.Where(a => a.IdUser == restrictToAgentId.Value);

            var grouped = await query
                .GroupBy(a => new { a.DateCreation.Year, a.DateCreation.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            // Génère les 6 mois (même s'il n'y a pas d'actes) pour avoir une courbe continue
            var result = new List<MonthlyDataPoint>();
            var frenchCulture = new CultureInfo("fr-FR");

            for (int i = 11; i >= 0; i--)
            {
                var date = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-i);
                var data = grouped.FirstOrDefault(g => g.Year == date.Year && g.Month == date.Month);

                result.Add(new MonthlyDataPoint
                {
                    Label = date.ToString("MMM yy", frenchCulture).ToLower(),
                    Count = data?.Count ?? 0,
                    MonthStart = date
                });
            }

            return result;
        }


        // ============================================================
        // ÉVOLUTION JOURNALIÈRE (30 derniers jours)
        // ============================================================
        public async Task<List<DailyDataPoint>> GetDailyEvolutionAsync(int? restrictToAgentId = null)
        {
            var now = DateTime.UtcNow;
            var thirtyDaysAgo = DateTime.SpecifyKind(now.Date.AddDays(-29), DateTimeKind.Utc); // 30 jours dont aujourd'hui

            var query = _db.TypesActModels
                .Where(a => a.DateCreation >= thirtyDaysAgo);

            if (restrictToAgentId.HasValue)
                query = query.Where(a => a.IdUser == restrictToAgentId.Value);

            var grouped = await query
                .GroupBy(a => a.DateCreation.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Valides = g.Count(a => a.StatutWorkflow == ActeStatut.Valide),
                    EnAttente = g.Count(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation),
                    Rejetes = g.Count(a => a.StatutWorkflow == ActeStatut.Rejete)
                })
                .ToListAsync();

            var result = new List<DailyDataPoint>();
            var frenchCulture = new CultureInfo("fr-FR");

            for (int i = 29; i >= 0; i--)
            {
                var date = now.Date.AddDays(-i);
                var data = grouped.FirstOrDefault(g => g.Date == date);

                result.Add(new DailyDataPoint
                {
                    Label = date.ToString("dd MMM", frenchCulture).ToLower(),
                    Count = data?.Count ?? 0,
                    Valides = data?.Valides ?? 0,
                    EnAttente = data?.EnAttente ?? 0,
                    Rejetes = data?.Rejetes ?? 0,
                    Date = date
                });
            }

            return result;
        }

        // ============================================================
        // ÉVOLUTION HEBDOMADAIRE (12 dernières semaines)
        // ============================================================
        public async Task<List<WeeklyDataPoint>> GetWeeklyEvolutionAsync(int? restrictToAgentId = null)
        {
            var now = DateTime.UtcNow;
            var twelveWeeksAgo = DateTime.SpecifyKind(now.Date.AddDays(-7 * 11), DateTimeKind.Utc); // 12 semaines

            var query = _db.TypesActModels
                .Where(a => a.DateCreation >= twelveWeeksAgo);

            if (restrictToAgentId.HasValue)
                query = query.Where(a => a.IdUser == restrictToAgentId.Value);

            var actes = await query.ToListAsync();
            var frenchCulture = new CultureInfo("fr-FR");
            var result = new List<WeeklyDataPoint>();

            for (int i = 11; i >= 0; i--)
            {
                // Calcul du début de semaine (lundi)
                var weekStart = now.Date.AddDays(-7 * i);
                var daysFromMonday = ((int)weekStart.DayOfWeek + 6) % 7;
                weekStart = weekStart.AddDays(-daysFromMonday);
                var weekEnd = weekStart.AddDays(7);

                var weekActes = actes.Where(a => a.DateCreation >= weekStart && a.DateCreation < weekEnd).ToList();
                var weekNumber = frenchCulture.Calendar.GetWeekOfYear(weekStart, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                result.Add(new WeeklyDataPoint
                {
                    Label = $"S{weekNumber:D2}",
                    Count = weekActes.Count,
                    Valides = weekActes.Count(a => a.StatutWorkflow == ActeStatut.Valide),
                    EnAttente = weekActes.Count(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation),
                    Rejetes = weekActes.Count(a => a.StatutWorkflow == ActeStatut.Rejete),
                    WeekStart = weekStart
                });
            }

            return result;
        }



        // ============================================================
        // ACTES À TRAITER (adaptatif par rôle)
        // ============================================================
        public async Task<List<PendingActeItem>> GetPendingActesAsync(string userRole, int? userId, int limit = 5)
        {
            IQueryable<TypesActModel> query;

            if (userRole == RoleNames.Agent && userId.HasValue)
            {
                // Agent : ses actes en Brouillon ou Rejeté
                query = _db.TypesActModels
                    .Where(a => a.IdUser == userId.Value
                            && (a.StatutWorkflow == ActeStatut.Brouillon
                                || a.StatutWorkflow == ActeStatut.Rejete));
            }
            else
            {
                // Responsable & Admin : actes en attente de validation
                query = _db.TypesActModels
                    .Where(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation);
            }

            var actes = await query
                .OrderBy(a => a.DateEnvoiValidation ?? a.DateCreation)
                .Take(limit)
                .ToListAsync();

            // Enrichir avec les noms d'agents
            var agentIds = actes.Where(a => a.IdUser.HasValue)
                                .Select(a => a.IdUser!.Value)
                                .Distinct()
                                .ToList();

            var agents = await _db.Users
                .Where(u => agentIds.Contains(u.Id))
                .Select(u => new { u.Id, u.NomComplet, u.Email })
                .ToListAsync();

            var now = DateTime.UtcNow;

            return actes.Select(a =>
            {
                var agent = a.IdUser.HasValue ? agents.FirstOrDefault(u => u.Id == a.IdUser.Value) : null;
                var reference = a.DateEnvoiValidation ?? a.DateCreation;
                var diff = now - reference;
                var isUrgent = diff.TotalDays >= 3;

                return new PendingActeItem
                {
                    IdActe = a.IdActe,
                    NomTypesActe = a.NomTypesActe,
                    Statut = a.StatutWorkflow,
                    DateEnvoiValidation = a.DateEnvoiValidation,
                    DateCreation = a.DateCreation,
                    AgentNom = agent?.NomComplet ?? agent?.Email,
                    WaitingTime = FormatDuration(diff),
                    IsUrgent = isUrgent
                };
            }).ToList();
        }

        // ============================================================
        // ACTIVITÉ RÉCENTE
        // ============================================================
        public async Task<List<RecentActivityItem>> GetRecentActivityAsync(int? restrictToAgentId = null, int limit = 10)
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

            var logs = await query
                .OrderByDescending(l => l.DateAction)
                .Take(limit)
                .ToListAsync();

            if (!logs.Any()) return new List<RecentActivityItem>();

            // Enrichir avec les noms d'users
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

            var now = DateTime.UtcNow;

            return logs.Select(l =>
            {
                var user = l.IdUser.HasValue ? users.FirstOrDefault(u => u.Id == l.IdUser.Value) : null;
                var diff = now - l.DateAction;

                return new RecentActivityItem
                {
                    IdActe = l.IdActe,
                    Action = l.Action,
                    Details = l.Details,
                    DateAction = l.DateAction,
                    UserName = user?.NomComplet ?? user?.Email,
                    UserRole = user is not null && userRoles.TryGetValue(user.Id, out var r) ? r : null,
                    TimeAgo = FormatDuration(diff) + " ago",
                    StatutApres = l.StatutApres
                };
            }).ToList();
        }

        // ============================================================
        // HELPER : formate "il y a Xh Ymin"
        // ============================================================
        private static string FormatDuration(TimeSpan diff)
        {
            if (diff.TotalDays >= 1)
                return $"{(int)diff.TotalDays}j {(int)diff.Hours}h";
            if (diff.TotalHours >= 1)
                return $"{(int)diff.TotalHours}h {(int)diff.Minutes}min";
            if (diff.TotalMinutes >= 1)
                return $"{(int)diff.TotalMinutes} min";
            return "à l'instant";
        }
    }
}