using GesCPSI_Project.Data;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Services
{
    public class UserService : ServiceBase<UserModel>, Interfaces.IUser
    {
        public UserService(GesDbContext context) : base(context)
        {
        }
    }
}
