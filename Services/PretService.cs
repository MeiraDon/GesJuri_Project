using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class PretService : ServiceBase<PretModel>, Interfaces.IPret
    {
        public PretService(GesDbContext context) : base(context)
        {
        }
    }
}
