using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class EntiteJurService : ServiceBase<EntiteJurModel>, Interfaces.IEntiteJur
    {
        public EntiteJurService(GesDbContext context) : base(context)
        {
        }


        public override async Task<Result<EntiteJurModel>> UpdateAsync(EntiteJurModel entity)
        {
            try
            {
                var existing = await _context.EntiteJurModels
                    .FirstOrDefaultAsync(x => x.IdEntiteJur == entity.IdEntiteJur);

                if (existing is null)
                    return Result<EntiteJurModel>.Failure($"Entité juridique introuvable (Id={entity.IdEntiteJur}).");

                // Champs scalaires
                existing.DateActeJuridique = entity.DateActeJuridique;
                existing.Reference = entity.Reference;
                existing.NomHuissier = entity.NomHuissier;
                existing.VilleHuissier = entity.VilleHuissier;
                existing.NumeroDossier = entity.NumeroDossier;
                existing.Responsable = entity.Responsable;
                existing.FonctionResponsable = entity.FonctionResponsable;
                existing.NomDossier = entity.NomDossier;

                // FK seulement
                existing.IdActe = entity.IdActe;

                await _context.SaveChangesAsync();
                return Result<EntiteJurModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<EntiteJurModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
