using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class CautionnementService : ServiceBase<CautionnementModel>, Interfaces.ICautionnement
    {
        public CautionnementService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<CautionnementModel>> UpdateAsync(CautionnementModel entity)
        {
            try
            {
                var existing = await _context.CautionnementModels
                    .FirstOrDefaultAsync(x => x.IdCautionnement == entity.IdCautionnement);

                if (existing is null)
                    return Result<CautionnementModel>.Failure($"Cautionnement introuvable (Id={entity.IdCautionnement}).");

                // Tous les champs scalaires
                existing.NomTypesActe = entity.NomTypesActe;
                existing.NumeroCautionmt = entity.NumeroCautionmt;
                existing.MontantCautionne = entity.MontantCautionne;
                existing.MontantLettreCautionmt = entity.MontantLettreCautionmt;
                existing.DateSignatureCautionmt = entity.DateSignatureCautionmt;
                existing.DateEffetCautionmt = entity.DateEffetCautionmt;
                existing.DateExpirationCautionmt = entity.DateExpirationCautionmt;
                existing.LieuSignatureCautionmt = entity.LieuSignatureCautionmt;
                existing.EtatCautionmt = entity.EtatCautionmt;
                existing.ConditionsRevocationCautionmt = entity.ConditionsRevocationCautionmt;
                existing.ObligationsBanque = entity.ObligationsBanque;
                existing.ObligationsCaution = entity.ObligationsCaution;
                existing.ClauseSubrogation = entity.ClauseSubrogation;
                existing.ClauseNonNovation = entity.ClauseNonNovation;
                existing.ElectionDomicile = entity.ElectionDomicile;
                existing.ReglementDifferend = entity.ReglementDifferend;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<CautionnementModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<CautionnementModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
