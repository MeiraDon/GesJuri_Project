using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class AutorisationService : ServiceBase<AutorisationModel>, Interfaces.IAutorisation
    {
        public AutorisationService(GesDbContext context) : base(context)
        {
        }
    }
}
