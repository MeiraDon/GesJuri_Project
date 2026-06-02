using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class ClientActService : ServiceBase<ClientActModel>, Interfaces.IClientAct
    {
        public ClientActService(GesDbContext context) : base(context)
        {
        }
    }
}
