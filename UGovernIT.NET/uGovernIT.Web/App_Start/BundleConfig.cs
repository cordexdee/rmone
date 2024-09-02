using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace uGovernIT.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/DMS/jquery-1.12.4.js",
                         "~/Scripts/jquery-ui.custom.js",
                         "~/Scripts/jquery.cookie.js",
                         "~/Scripts/jquery.contextMenu.js",
                         "~/Scripts/toastr.min.js",
                          "~/Scripts/DMS/jquery.dynatree.js",
                          "~/Scripts/DMS/Repository.js",
                          "~/Scripts/DMS/UserAdminRepository.js",
                         "~/Scripts/DMS/FileDetails.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryMasterPageJS").Include(
             "~/Scripts/jquery.cookie.js",
             "~/Scripts/jquery-ui-1.12.1.min.js",
             "~/Scripts/underscore.min.js",
             "~/Scripts/jquery.tablednd.js",
              "~/Scripts/interaction.js",
              "~/Scripts/jquery-scrolltofixed-min.js",
              "~/Scripts/jquery.dialogextend.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            bundles.Add(new ScriptBundle("~/bundles/ugitCommonMasterPageJS").Include(
               "~/Scripts/ugitcommon.js",
              "~/Scripts/uGITDashboard.js",
              "~/Scripts/ugitmodule.js",
              "~/Scripts/Chart.min.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/ugitcommon").Include(
                "~/Scripts/ugitcommon.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ugitdashboard").Include(
               "~/Scripts/uGITDashboard.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/ugitmodule").Include(
               "~/Scripts/ugitmodule.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                 "~/Scripts/toastr.js",
                "~/Scripts/AdminCommon.js",
               "~/Scripts/ugitmodule.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/Admin/Admin.js",
                "~/Scripts/jquery.dialogextend.js",
                "~/Scripts/interaction.js",
                "~/Scripts/jquery-ui.custom.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/commonjs").Include(
                         "~/Scripts/DMS/jquery-1.12.4.js",
                         "~/Scripts/jquery-ui.custom.js",
                         "~/Scripts/jquery.cookie.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/DashboardJs").Include(              
              "~/Scripts/Chart.min.js"
              ));

            // Css bundle
            bundles.Add(new StyleBundle("~/bundles/dmscss").Include(
              "~/Content/bootstrap.css",
              "~/Content/jquery-ui.css",
              "~/Content/ui.dynatree.css",
              "~/Content/jquery.contextMenu.css",
              "~/Content/toastr.min.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
              "~/Content/CSS/Admin/configure.css",
              "~/Content/click.css"));
           
            bundles.Add(new StyleBundle("~/Content/commoncss").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/jquery-ui.css",
                "~/Content/uGITCommon.css",
                "~/Content/UgitNewUI.css",
                "~/Content/UgitNewResponsiveUI.css",
                "~/Content/jquery.barCharts.css",
                "~/Content/jquery.graph.css",
                "~/Content/CSS/font-awesome-5.11.2/css/fontawesomeall.css",
                "~/Content/CSS/MaterialIcons.css",
                "~/Content/CSS/RobotoVarelaRound.css",
                "~/Content/CSS/table.css",
                "~/Content/CSS/Admin/Font/Arimo.css",
                "~/Content/CSS/Admin/Font/Bungee Inline.css",
                "~/Content/CSS/Admin/Font/Bungee Shade.css",
                "~/Content/CSS/Admin/Font/Condensed.css",
                "~/Content/CSS/Admin/Font/Cousine.css",
                "~/Content/CSS/Admin/Font/DroidSans.css",
                "~/Content/CSS/Admin/Font/DroidSerif.css",
                "~/Content/CSS/Admin/Font/Limelight.css",
                "~/Content/CSS/Admin/Font/Merriweather.css",
                "~/Content/CSS/Admin/Font/OpenSans.css",
                "~/Content/CSS/Admin/Font/Oswald.css",
                "~/Content/CSS/Admin/Font/Roboto.css",
                "~/Content/CSS/Admin/Font/RobotoMono.css",
                "~/Content/CSS/Admin/Font/RobotoSlab.css",
                "~/Content/CSS/Admin/Font/Syncopate.css",
                "~/Content/CSS/Admin/Font/Tinos.css",
                "~/Content/CSS/Dashboard/style.css",
                "~/Content/CSS/googleapis.css",
                "~/Content/CSS/googleapisMontserratcss.css"
                ));
            
            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });

            BundleTable.EnableOptimizations = true;
        }
    }
}