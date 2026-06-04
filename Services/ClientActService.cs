using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class ClientActService : ServiceBase<ClientActModel>, Interfaces.IClientAct
    {
        public ClientActService(GesDbContext context) : base(context)
        {
        }

        public override async Task<Result<ClientActModel>> UpdateAsync(ClientActModel entity)
        {
            try
            {
                var existing = await _context.ClientActModels
                    .FirstOrDefaultAsync(x => x.IdCltActe == entity.IdCltActe);

                if (existing is null)
                    return Result<ClientActModel>.Failure($"Lien Client-Acte introuvable (Id={entity.IdCltActe}).");

                existing.DescriptionRole = entity.DescriptionRole;
                existing.IdClt = entity.IdClt;
                existing.IdActe = entity.IdActe;
                existing.IdRole = entity.IdRole;

                await _context.SaveChangesAsync();
                return Result<ClientActModel>.Success(existing);
            }
            catch (Exception ex)
            {
                return Result<ClientActModel>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }

    }
}
