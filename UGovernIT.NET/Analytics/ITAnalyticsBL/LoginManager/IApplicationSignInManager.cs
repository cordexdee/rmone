using System.Security.Claims;
using System.Threading.Tasks;
using ITAnalyticsBL.DB;

namespace ITAnalyticsBL.LoginManager
{
    public interface IApplicationSignInManager
    {
        Task<ClaimsIdentity> CreateUserIdentityAsync(User user);
    }
}