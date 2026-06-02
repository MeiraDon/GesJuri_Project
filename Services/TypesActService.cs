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
    }
}
