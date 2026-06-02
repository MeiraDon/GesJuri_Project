using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class ClientService : ServiceBase<ClientModel>, Interfaces.IClient
    {
        public ClientService(GesDbContext context) : base(context)
        {
        }
    }
}
