using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class JournalisationService : ServiceBase<JournalisationModel>, Interfaces.IJournalisation
    {
        public JournalisationService(GesDbContext context) : base(context)
        {
        }
    }
}
