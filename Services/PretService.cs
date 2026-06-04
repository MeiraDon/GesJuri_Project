using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class PretService : ServiceBase<PretModel>, Interfaces.IPret
    {
        public PretService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<PretModel>> UpdateAsync(PretModel entity)
        {
            try
            {
                var existing = await _context.PretModels
                    .FirstOrDefaultAsync(x => x.IdPret == entity.IdPret);

                if (existing is null)
                    return Result<PretModel>.Failure($"Prêt introuvable (Id={entity.IdPret}).");

                // Champs scalaires
                existing.ReferenceConvention = entity.ReferenceConvention;
                existing.MontantConcours = entity.MontantConcours;
                existing.Taux = entity.Taux;
                existing.MontantLettrePret = entity.MontantLettrePret;
                existing.DateSignaturePret = entity.DateSignaturePret;
                existing.DureMoisPret = entity.DureMoisPret;
                existing.DateDebutRemboursemnt = entity.DateDebutRemboursemnt;
                existing.DatePremiereEcheance = entity.DatePremiereEcheance;
                existing.MontantPremiereEcheance = entity.MontantPremiereEcheance;
                existing.DateDerniereEcheance = entity.DateDerniereEcheance;
                existing.MontantDerniereEcheance = entity.MontantDerniereEcheance;
                existing.Accessoires = entity.Accessoires;
                existing.ObjetPret = entity.ObjetPret;
                existing.EtatPret = entity.EtatPret;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<PretModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<PretModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }
    }
}
