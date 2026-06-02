using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class ChargeService : ServiceBase<ChargeModel>, Interfaces.ICharge
    {
        public ChargeService(GesDbContext context) : base(context)
        {
        }
    }
}
