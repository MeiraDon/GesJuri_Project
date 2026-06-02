using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class RolesClientActService : ServiceBase<RolesClientActModel>, Interfaces.IRolesClientAct
    {
        public RolesClientActService(GesDbContext context) : base(context)
        {
        }
    }
}
