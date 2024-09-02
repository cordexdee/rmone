using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;

namespace OwinAuth.Identity
//namespace uGovernIT.Web
{
    /// <summary>
    /// This application startup class will be called to configure the OWIN middleware pipeline
    /// </summary>
    /// <remarks>
    /// One would normally put this class in App_Start folder by convention, but I have placed it in the  
    /// Identity folder for clarity in teaching purposes.
    /// </remarks>
    //public class IdentityConfig
    //{
    //    /// <summary>
    //    ///  This Configuration method will be called because the class name has been provided in the web.config file
    //    /// and the Microsoft.Owin.Host.SystemWeb assembly has been included in the project. An assembly level
    //    /// option in that assembly will cause this to be bootstrapped by the OwinHttpModule. The name and parameter
    //    /// list are by convention.
    //    /// </summary>
    //    /// <param name="app">The instance of the OWIN AppBuilder class used to configure the middleware pipeline</param>
    //    public void Configuration(Owin.IAppBuilder app) {
    //        //Extension methods allow us to add middleware items to the pipeline, sometimes with lifecycle hints
    //        //note that we are providing the factory methods to be called
    //        //whenever the builder needs to build this middleware
    //        app.CreatePerOwinContext<ApplicationIdentityDbContext>(ApplicationIdentityDbContext.Create);
    //        app.CreatePerOwinContext<UserProfileManager>(UserProfileManager.Create);
    //        app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
    //        app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions {
    //            AuthenticationType = Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults.AuthenticationType
    //                                                                                                        ,
    //            LoginPath = Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults.LoginPath
    //        });
    //    }
    //}

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