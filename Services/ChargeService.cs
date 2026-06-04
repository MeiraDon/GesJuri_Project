using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class ChargeService : ServiceBase<ChargeModel>, Interfaces.ICharge
    {
        public ChargeService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<ChargeModel>> UpdateAsync(ChargeModel entity)
        {
            try
            {
                var existing = await _context.ChargeModels
                    .FirstOrDefaultAsync(x => x.IdCharge == entity.IdCharge);

                if (existing is null)
                    return Result<ChargeModel>.Failure($"Charge introuvable (Id={entity.IdCharge}).");

                // Champs scalaires
                existing.TypeCharge = entity.TypeCharge;
                existing.MontantCharge = entity.MontantCharge;
                existing.DateCharge = entity.DateCharge;
                existing.PayePar = entity.PayePar;
                existing.StatutCharge = entity.StatutCharge;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<ChargeModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<ChargeModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
