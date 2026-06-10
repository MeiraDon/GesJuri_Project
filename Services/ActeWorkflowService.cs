using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class ActeWorkflowService : IActeWorkflow
    {
        private readonly GesDbContext _db;

        public ActeWorkflowService(GesDbContext db)
        {
            _db = db;
        }

        // ============================================================
        // SOUMETTRE : Brouillon/Rejeté → EnAttenteValidation
        // ============================================================
        public async Task<WorkflowResult> SoumettreAsync(int acteId, int agentId)
        {
            var acte = await _db.TypesActModels.FirstOrDefaultAsync(a => a.IdActe == acteId);
            if (acte is null)
                return WorkflowResult.Fail("Acte introuvable.");

            if (acte.StatutWorkflow != ActeStatut.Brouillon && acte.StatutWorkflow != ActeStatut.Rejete)
                return WorkflowResult.Fail(
                    $"Impossible de soumettre un acte au statut '{ActeStatutHelper.GetLabel(acte.StatutWorkflow)}'.");

            var statutAvant = acte.StatutWorkflow;

            acte.StatutWorkflow = ActeStatut.EnAttenteValidation;
            acte.Statut = "EnAttenteValidation";
            acte.DateEnvoiValidation = DateTime.UtcNow;
            acte.DateMaj = DateTime.UtcNow;
            acte.MotifRejet = null;
            acte.IdUser ??= agentId;

            LogAction(acteId, agentId, "SOUMISSION",
                "Acte envoyé en validation.",
                statutAvant, ActeStatut.EnAttenteValidation);

            await _db.SaveChangesAsync();
            return WorkflowResult.Ok(ActeStatut.EnAttenteValidation);
        }

        // ============================================================
        // VALIDER : EnAttenteValidation → Valide
        // ============================================================
        public async Task<WorkflowResult> ValiderAsync(int acteId, int validateurId)
        {
            var acte = await _db.TypesActModels.FirstOrDefaultAsync(a => a.IdActe == acteId);
            if (acte is null)
                return WorkflowResult.Fail("Acte introuvable.");

            if (acte.StatutWorkflow != ActeStatut.EnAttenteValidation)
                return WorkflowResult.Fail(
                    $"Seul un acte en attente peut être validé. Statut actuel : '{ActeStatutHelper.GetLabel(acte.StatutWorkflow)}'.");

            if (acte.IdUser.HasValue && acte.IdUser.Value == validateurId)
                return WorkflowResult.Fail(
                    "Vous ne pouvez pas valider un acte que vous avez vous-même saisi (séparation des pouvoirs).");

            var statutAvant = acte.StatutWorkflow;

            acte.StatutWorkflow = ActeStatut.Valide;
            acte.Statut = "Valide";
            acte.DateValidation = DateTime.UtcNow;
            acte.DateMaj = DateTime.UtcNow;
            acte.ValidateurId = validateurId;
            acte.MotifRejet = null;

            LogAction(acteId, validateurId, "VALIDATION",
                "Acte validé.",
                statutAvant, ActeStatut.Valide);

            await _db.SaveChangesAsync();
            return WorkflowResult.Ok(ActeStatut.Valide);
        }

        // ============================================================
        // REJETER : EnAttenteValidation → Rejete (avec motif)
        // ============================================================
        public async Task<WorkflowResult> RejeterAsync(int acteId, int validateurId, string motif)
        {
            if (string.IsNullOrWhiteSpace(motif))
                return WorkflowResult.Fail("Le motif du rejet est obligatoire.");

            if (motif.Trim().Length < 10)
                return WorkflowResult.Fail("Le motif doit faire au moins 10 caractères.");

            var acte = await _db.TypesActModels.FirstOrDefaultAsync(a => a.IdActe == acteId);
            if (acte is null)
                return WorkflowResult.Fail("Acte introuvable.");

            if (acte.StatutWorkflow != ActeStatut.EnAttenteValidation)
                return WorkflowResult.Fail(
                    $"Seul un acte en attente peut être rejeté. Statut actuel : '{ActeStatutHelper.GetLabel(acte.StatutWorkflow)}'.");

            if (acte.IdUser.HasValue && acte.IdUser.Value == validateurId)
                return WorkflowResult.Fail(
                    "Vous ne pouvez pas traiter un acte que vous avez vous-même saisi.");

            var statutAvant = acte.StatutWorkflow;

            acte.StatutWorkflow = ActeStatut.Rejete;
            acte.Statut = "Rejete";
            acte.DateRejet = DateTime.UtcNow;
            acte.DateMaj = DateTime.UtcNow;
            acte.ValidateurId = validateurId;
            acte.MotifRejet = motif.Trim();

            LogAction(acteId, validateurId, "REJET",
                $"Acte rejeté. Motif : {motif.Trim()}",
                statutAvant, ActeStatut.Rejete);

            await _db.SaveChangesAsync();
            return WorkflowResult.Ok(ActeStatut.Rejete);
        }

        // ============================================================
        // ARCHIVER : Valide → Archive
        // ============================================================
        public async Task<WorkflowResult> ArchiverAsync(int acteId, int adminId)
        {
            var acte = await _db.TypesActModels.FirstOrDefaultAsync(a => a.IdActe == acteId);
            if (acte is null)
                return WorkflowResult.Fail("Acte introuvable.");

            if (acte.StatutWorkflow != ActeStatut.Valide)
                return WorkflowResult.Fail(
                    $"Seul un acte validé peut être archivé. Statut actuel : '{ActeStatutHelper.GetLabel(acte.StatutWorkflow)}'.");

            var statutAvant = acte.StatutWorkflow;

            acte.StatutWorkflow = ActeStatut.Archive;
            acte.Statut = "Archive";
            acte.DateArchivage = DateTime.UtcNow;
            acte.DateMaj = DateTime.UtcNow;

            LogAction(acteId, adminId, "ARCHIVAGE",
                "Acte archivé après signature.",
                statutAvant, ActeStatut.Archive);

            await _db.SaveChangesAsync();
            return WorkflowResult.Ok(ActeStatut.Archive);
        }

        // ============================================================
        // ROUVRIR : Rejete → Brouillon (pour modification)
        // ============================================================
        public async Task<WorkflowResult> RouvrirAsync(int acteId, int userId)
        {
            var acte = await _db.TypesActModels.FirstOrDefaultAsync(a => a.IdActe == acteId);
            if (acte is null)
                return WorkflowResult.Fail("Acte introuvable.");

            if (acte.StatutWorkflow != ActeStatut.Rejete)
                return WorkflowResult.Fail(
                    $"Seul un acte rejeté peut être ré-ouvert. Statut actuel : '{ActeStatutHelper.GetLabel(acte.StatutWorkflow)}'.");

            var statutAvant = acte.StatutWorkflow;

            acte.StatutWorkflow = ActeStatut.Brouillon;
            acte.Statut = "Brouillon";
            acte.DateMaj = DateTime.UtcNow;

            LogAction(acteId, userId, "REOUVERTURE",
                "Acte ré-ouvert pour modification suite au rejet.",
                statutAvant, ActeStatut.Brouillon);

            await _db.SaveChangesAsync();
            return WorkflowResult.Ok(ActeStatut.Brouillon);
        }

        // ============================================================
        // COMPTEUR : actes en attente (pour badge NavMenu)
        // ============================================================
        public async Task<int> CountEnAttenteAsync()
        {
            return await _db.TypesActModels
                .CountAsync(a => a.StatutWorkflow == ActeStatut.EnAttenteValidation);
        }

        // ============================================================
        // LOG CRÉATION : tracé à la première sauvegarde d'un nouvel acte
        // ============================================================
        public async Task LogCreationAsync(int acteId, int userId)
        {
            LogAction(acteId, userId, "CREATION",
                "Acte créé.",
                null, ActeStatut.Brouillon);

            await _db.SaveChangesAsync();
        }


        // ============================================================
        // AUDIT : enregistre dans AuditLogModel
        // ============================================================
        private void LogAction(
            int acteId,
            int userId,
            string action,
            string details,
            ActeStatut? statutAvant,
            ActeStatut? statutApres)
        {
            _db.AuditLogs.Add(new AuditLogModel
            {
                IdActe = acteId,
                Action = action,
                Details = details,
                DateAction = DateTime.UtcNow,
                IdUser = userId,
                StatutAvant = statutAvant,
                StatutApres = statutApres
            });
        }
    }
}
