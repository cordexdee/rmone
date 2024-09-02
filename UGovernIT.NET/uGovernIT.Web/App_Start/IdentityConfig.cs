using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Models;

namespace uGovernIT.Web
{
    public class ApplicationSignInManager : SignInManager<UserProfile, string>
    {
        public ApplicationSignInManager(UserProfileManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserProfile user)
        {
            UserProfileManager manager = UserManager as UserProfileManager;
            return manager.GenerateUserIdentityAsync(user);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<UserProfileManager>(), context.Authentication);
        }
    }
}
