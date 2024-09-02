using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings, new WebMethodFriendlyUrlResolver());

            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("SitePages",
                        "pages/{name}",
                        "~/RoutingPage.aspx");

            routes.MapPageRoute("Dashboard",
                         "Dashboard/{name}",
                         "~/Dashboards.aspx");

            routes.MapRoute(name: "Default",
            url: "{controller}/{action}"
            );
        }
    }
}
