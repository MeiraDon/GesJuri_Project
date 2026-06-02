using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class BanqueService : ServiceBase<BanqueModel>, Interfaces.IBanque
    {
        public BanqueService(GesDbContext context) : base(context)
        {
        }
    }
}
