using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class TypesActService : ServiceBase<TypesActModel>, Interfaces.ITypesAct
    {
        public TypesActService(GesDbContext context) : base(context)
        {
        }

        public new async Task<TypesActModel?> GetByIdAsync(int id)
        {
            return await _context.TypesActModels
                .AsNoTracking()
                .Include(x => x.BanqueModel)
                .Include(x => x.AjoutActModel)
                .Include(x => x.UserModel)
                .FirstOrDefaultAsync(x => x.IdActe == id);
        }

        public override async Task<Result<TypesActModel>> UpdateAsync(TypesActModel entity)
        {
            try
            {
                // 1. Récupère l'entité trackée directement de la BD
                var existing = await _context.TypesActModels
                    .FirstOrDefaultAsync(x => x.IdActe == entity.IdActe);

                if (existing is null)
                    return Result<TypesActModel>.Failure($"Acte introuvable (Id={entity.IdActe}).");

                // 2. Mets à jour les champs scalaires
                existing.NomTypesActe = entity.NomTypesActe;
                existing.Statut = entity.Statut;
                existing.DateMaj = DateTime.UtcNow;
                existing.MotifRejet = entity.MotifRejet;
                existing.DateValidation = entity.DateValidation;
                existing.DateRejet = entity.DateRejet;
                existing.ValidateurId = entity.ValidateurId;
                existing.FichierValidationPath = entity.FichierValidationPath;
                existing.PdfGenerePath = entity.PdfGenerePath;
                existing.PdfSignePath = entity.PdfSignePath;
                existing.JsonSnapshotPath = entity.JsonSnapshotPath;
                existing.DateGenerationPdf = entity.DateGenerationPdf;
                existing.DateUploadSignature = entity.DateUploadSignature;

                // 3. FK uniquement — JAMAIS les objets de navigation
                existing.IdAjout = entity.IdAjout;
                existing.IdUser = entity.IdUser;
                existing.IdBnq = entity.IdBnq;

                await _context.SaveChangesAsync();
                return Result<TypesActModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<TypesActModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
