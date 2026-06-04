using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class JournalisationService : ServiceBase<JournalisationModel>, Interfaces.IJournalisation
    {
        public JournalisationService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<JournalisationModel>> UpdateAsync(JournalisationModel entity)
        {
            try
            {
                var existing = await _context.JournalisationModels
                    .FirstOrDefaultAsync(x => x.IdNotif == entity.IdNotif);

                if (existing is null)
                    return Result<JournalisationModel>.Failure($"Journalisation introuvable (Id={entity.IdNotif}).");

                // Notification
                existing.TypeNotif = entity.TypeNotif;
                existing.DateNotif = entity.DateNotif;
                existing.ContenuNotif = entity.ContenuNotif;
                existing.ModeNotif = entity.ModeNotif;
                existing.Destinataire = entity.Destinataire;
                existing.StatutNotif = entity.StatutNotif;

                // Annexe
                existing.TypeAnnexe = entity.TypeAnnexe;
                existing.Description = entity.Description;
                existing.Fichier = entity.Fichier;
                existing.DateUpload = entity.DateUpload;
                existing.UploadedBy = entity.UploadedBy;

                // Archive
                existing.RefPret = entity.RefPret;
                existing.Dossier = entity.Dossier;
                existing.Cheminfichier = entity.Cheminfichier;
                existing.Etat = entity.Etat;
                existing.DateArchive = entity.DateArchive;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<JournalisationModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<JournalisationModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }
    }
}
