using System;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Models;
using uGovernIT.Web.Providers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using uGovernIT.Util;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class Startup {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationContext.Create);
            //app.Use<OwinContext>();
            ApplicationContext _context = ApplicationContext.Create();

            ULog.Create(_context.TenantAccountId, _context.CurrentUser.Name);
            UserProfileManager objUserManager = new UserProfileManager(_context);
            app.CreatePerOwinContext<UserProfileManager>(objUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "WebAuthCookie",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserProfileManager, UserProfile>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => manager.GenerateUserIdentityAsync(user))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            PublicClientId = "self";
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            int days = uGovernIT.Utility.UGITUtility.StringToInt(configurationVariableManager.GetValue(uGovernIT.Utility.ConfigConstants.AccessTokenExpiration));
            if (days <= 0)
                days = 14;

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(days),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = false
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            //initialize job
            JobStart.Start(app);
            //SettingADFS(app);
        }
        //private static void SettingADFS(IAppBuilder app)
        //{
        //    if (!Utility.UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["ADFS_Enable"]))
        //        return;

        //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
        //    //Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        //    var adfs2 = new WsFederationAuthenticationOptions()
        //    {
        //        MetadataAddress = string.Format("{0}", System.Configuration.ConfigurationManager.AppSettings["ADFS_Url"]),
        //        Wtrealm = System.Configuration.ConfigurationManager.AppSettings["ADFS_TrustUrl"],
        //        Wreply = System.Configuration.ConfigurationManager.AppSettings["ADFS_TrustUrl"],
        //        AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive,
        //        //BackchannelCertificateValidator = new FakeCertificateValidator(),
        //        //BackchannelHttpHandler = GetProxyAwareHttpMessageHandler(),
        //        TokenValidationParameters = new TokenValidationParameters()
        //        {
        //            ValidateLifetime = true,
        //            ValidateIssuer = true,
        //            ValidateAudience = false
        //        },


        //        Notifications = new WsFederationAuthenticationNotifications()
        //        {
        //            RedirectToIdentityProvider = (ctx) =>
        //            {
        //                //To avoid a redirect loop to the federation server send 403 when user is authenticated but does not have access
        //                if (ctx.OwinContext.Response.StatusCode == 401 && ctx.OwinContext.Authentication.User.Identity.IsAuthenticated)
        //                {
        //                    ctx.OwinContext.Response.StatusCode = 403;
        //                    ctx.HandleResponse();
        //                }
        //                return Task.FromResult(0);
        //            },

        //            SecurityTokenValidated = (ctx) =>
        //            {
        //                //Ignore scheme/host name in redirect Uri to make sure a redirect to HTTPS does not redirect back to HTTP
        //                var redirectUri = new Uri(ctx.AuthenticationTicket.Properties.RedirectUri, UriKind.RelativeOrAbsolute);
        //                if (redirectUri.IsAbsoluteUri)
        //                {
        //                    ctx.AuthenticationTicket.Properties.RedirectUri = redirectUri.PathAndQuery;
        //                }
        //                return Task.FromResult(0);
        //            },
        //            MessageReceived = (ctx) =>
        //            {
        //                return Task.FromResult(0);
        //            },
        //            AuthenticationFailed = (ctx) =>
        //            {
        //                return Task.FromResult(0);
        //            },
        //            SecurityTokenReceived = (ctx) =>
        //            {
        //                return Task.FromResult(0);
        //            }
        //        }

        //    };
        //    //app.UseWsFederationAuthentication(adfs1);
        //    app.UseWsFederationAuthentication(adfs2);
        //}
    }
}
