using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class EntiteJurService : ServiceBase<EntiteJurModel>, Interfaces.IEntiteJur
    {
        public EntiteJurService(GesDbContext context) : base(context)
        {
        }
    }
}
