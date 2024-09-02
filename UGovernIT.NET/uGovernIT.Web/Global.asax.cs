using System;
using System.Linq;
using System.Web;
using DevExpress.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web.Http;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Util.Cache;
using System.Web.UI;
using System.Reflection;
using System.Web.Compilation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Net;


[assembly: PreApplicationStartMethod(
    typeof(uGovernIT.Web.PageInitializerModule),
    "Initialize")]
namespace uGovernIT.Web
{
    public sealed class PageInitializerModule : IHttpModule
    {
        public static void Initialize()
        {
            DynamicModuleUtility.RegisterModule(typeof(PageInitializerModule));
        }

        void IHttpModule.Init(HttpApplication app)
        {
            app.PreRequestHandlerExecute += (sender, e) =>
            {
                var handler = app.Context.CurrentHandler;
                if (handler != null)
                {
                    string name = handler.GetType().Assembly.FullName;
                    if (!name.StartsWith("System.Web") &&
                        !name.StartsWith("Microsoft"))
                    {
                        Global_asax.InitializeHandler(handler);
                    }
                }
            };
        }

        void IHttpModule.Dispose() { }
    }

    public class Global_asax : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            Bootstrap();
            RegisterCache.Register();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            ASPxWebControl.BackwardCompatibility.DataControlAllowReadUnlistedFieldsFromClientApiDefaultValue = true;
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };
        }

        void Application_End(object sender, EventArgs e)
        {
            //CacheHelper<object>.Clear();
            //TransCacheHelper<object>.Clear();
            // Code that runs on application shutdown
        }

        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection && Context.Request.Headers["X-Forwarded-Proto"] != "https")
            {
                Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
            }
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UserProfileManager manager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            context.SetCurrentUser(HttpContext.Current.CurrentUser());
            context.SetUserManager(manager);
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception lastException = Server.GetLastError();
            if (lastException != null)
                Util.Log.ULog.WriteException(lastException, lastException.Message.ToString(), "error");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //ApplicationContext context = HttpContext.Current.GetManagerContext();
            HttpContext.Current.Session["TenantID"] = TenantHelper.GetTanantID();
            //RegisterCache.ClearCache(context.TenantID);
            //RegisterCache.ReloadAllConfigCache(context);
            //RegisterCache.ReloadProfileCache(context);
            //RegisterCache.ReloadTicketsCache(context);
            //RegisterCache.GetCacheDetail(context);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }        


        public static void InitializeHandler(IHttpHandler handler)
        {
           
        }

        private static void Bootstrap()
        {
            
        }
    }
}