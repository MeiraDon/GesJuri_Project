using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using GesCPSI_Project.ViewModels;

namespace GesCPSI_Project.Services
{
    public class AjoutActService : ServiceBase<AjoutActModel> ,Interfaces.IAjoutAct
    {
        public AjoutActService(GesDbContext context) : base(context)
        {
        }
    }
}
