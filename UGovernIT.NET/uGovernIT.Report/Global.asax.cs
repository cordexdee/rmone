using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using uGovernIT.Util.Cache;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility;

namespace uGovernIT.Report
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {

            // Code that runs on application startup
            //CacheHelper<object> objCacheHelper = new CacheHelper<object>();
            CacheHelper<object>.AddCacheInstance();
            //BTS-21-000594: To enable the caching of UGITModule for Report project.
            CacheHelper<UGITModule>.AddCacheInstance();
            //TransCacheHelper<object> obCacheHelper = new TransCacheHelper<object>();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            
        }

        void Application_End(object sender, EventArgs e)
        {
            //CacheHelper<object>.Clear();
            //TransCacheHelper<object>.Clear();
            // Code that runs on application shutdown
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UserProfileManager manager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            context.SetCurrentUser(HttpContext.Current.CurrentUser());
            context.SetUserManager(manager);
        }
    }
}