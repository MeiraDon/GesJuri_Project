using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class AutorisationService : ServiceBase<AutorisationModel>, Interfaces.IAutorisation
    {
        public AutorisationService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<AutorisationModel>> UpdateAsync(AutorisationModel entity)
        {
            try
            {
                var existing = await _context.AutorisationModels
                    .FirstOrDefaultAsync(x => x.IdAutorisation == entity.IdAutorisation);

                if (existing is null)
                    return Result<AutorisationModel>.Failure($"Autorisation introuvable (Id={entity.IdAutorisation}).");

                // Champs scalaires
                existing.NumeroAutorisation = entity.NumeroAutorisation;
                existing.DateAutorisation = entity.DateAutorisation;
                existing.LieuAutorisation = entity.LieuAutorisation;
                existing.MontantMaxAutorise = entity.MontantMaxAutorise;
                existing.ConditionsAutorisation = entity.ConditionsAutorisation;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<AutorisationModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<AutorisationModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
