using ITAnalyticsBL.DB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITAnalyticsBL.LoginManager
{
    public class EmailService : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return System.Threading.Tasks.Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return System.Threading.Tasks.Task.FromResult(0);
        }
    }

    //// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    //public class UserProfileManager : UserManager<User>
    //{
    //    public UserProfileManager(IUserStore<User> store)
    //        : base(store)
    //    {
    //    }
    //    public UserProfileManager(UserProfileStore store):base(store)
    //    {

    //    }
    //    public static UserProfileManager Create(IdentityFactoryOptions<UserProfileManager> options, IOwinContext context)
    //    {
    //        var manager = new UserProfileManager(new UserProfileStore(new ApplicationDbContext()));
    //        // Configure validation logic for usernames
    //        manager.UserValidator = new UserValidator<User>(manager)
    //        {
    //            AllowOnlyAlphanumericUserNames = false,
    //            RequireUniqueEmail = true
    //        };

    //        // Configure validation logic for passwords
    //        manager.PasswordValidator = new PasswordValidator
    //        {
    //            RequiredLength = 6,
    //            RequireNonLetterOrDigit = true,
    //            RequireDigit = true,
    //            RequireLowercase = true,
    //            RequireUppercase = true,
    //        };

    //        // Configure user lockout defaults
    //        manager.UserLockoutEnabledByDefault = true;
    //        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

    //        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
    //        // You can write your own provider and plug it in here.
    //        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
    //        {
    //            MessageFormat = "Your security code is {0}"
    //        });
    //        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
    //        {
    //            Subject = "Security Code",
    //            BodyFormat = "Your security code is {0}"
    //        });
    //        manager.EmailService = new EmailService();
    //        manager.SmsService = new SmsService();
    //        var dataProtectionProvider = options.DataProtectionProvider;
    //        if (dataProtectionProvider != null)
    //        {
    //            manager.UserTokenProvider =
    //                new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
    //        }
    //        return manager;
    //    }
    //    public bool IsUserExist(User user)
    //    {
    //        return this.Users.FirstOrDefault(x => x.UserName == user.UserName) != null;
    //    }
    //    public string GetUserFirstName(string userName)
    //    {
           
    //        string Name = "";
    //        var user = this.Users.FirstOrDefault(x => x.UserName == userName);
    //        if (user != null)
    //            Name= user.Name;
    //        else
    //            Name= "";            
    //        return Name;
    //    }
    //    public void ActivateUser(string userName)
    //    {
    //        var user = this.Users.FirstOrDefault(x => x.UserName == userName);
    //        if (user != null)
    //            user.Disabled = false;
    //       this.Update(user);
    //    }
    //    public void DeactivateUser(string userName)
    //    {
    //        var user = this.Users.FirstOrDefault(x => x.UserName == userName);
    //        if (user != null)
    //            user.Disabled = true;
    //        this.Update(user);
    //    }


    //}

    //// Configure the application sign-in manager which is used in this application.
    //public class UserSignInManager : SignInManager<User, string>, IApplicationSignInManager
    //{
    //    public UserSignInManager(UserProfileManager userManager, IAuthenticationManager authenticationManager)
    //        : base(userManager, authenticationManager)
    //    {
    //    }

    //    public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
    //    {
    //        return user.GenerateUserIdentityAsync((UserProfileManager)UserManager);
    //    }

    //    public static UserSignInManager Create(IdentityFactoryOptions<UserSignInManager> options, IOwinContext context)
    //    {
    //        return new UserSignInManager(context.GetUserManager<UserProfileManager>(), context.Authentication);
    //    }
    //}
    //public class UserProfileStore : UserStore<User>
    //{
    //    public UserProfileStore(ApplicationDbContext context)
    //        : base(context)
    //    {
    //    }
    //}
   
}
