using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class CautionnementService : ServiceBase<CautionnementModel>, Interfaces.ICautionnement
    {
        public CautionnementService(GesDbContext context) : base(context)
        {
        }
    }
}
