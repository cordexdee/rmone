using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using uGovernIT.Utility;
using System.Data;

namespace uGovernIT.Web
{
    public partial class _Default : UPage
    {
        private ApplicationContext _context = null;
        private LandingPagesManager _LandingPagesManager = null;
        //UserProfile currentUser = null;
        //UserProfileManager userManager;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected LandingPagesManager LandingPagesManager
        {
            get
            {
                if (_LandingPagesManager == null)
                {
                    _LandingPagesManager = new LandingPagesManager(ApplicationContext);
                }
                return _LandingPagesManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserProfile user = HttpContext.Current.CurrentUser();
            var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
            var landingPageUrl = LandingPagesManager.GetLandingPageById(user.UserRoleId);
            string param = string.Empty;

            Util.Log.ULog.WriteLog($"{ApplicationContext.TenantAccountId}|{user.Name}: Visited Page: Default.aspx");

            if (Request.Url.ToString().Contains('?'))
                param = "?" + Request.Url.ToString().Split('?')[1];

            if (!string.IsNullOrEmpty(landingPageUrl))
            {
                landingPageUrl = new LandingPagesManager(ApplicationContext).LandingPagesfinal(user, "frommail");
                var siteUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/');
                if (user != null)
                {                    
                    var isSuperAdmin = manager.IsUGITSuperAdmin(user);

                    if (isSuperAdmin)
                    {
                        Response.Redirect("~/SuperAdmin/SuperAdmin.aspx");
                    }
                }
                
                if (landingPageUrl.Contains("UserHomePage"))
                {
                    Response.Redirect(siteUrl + landingPageUrl + param);
                }
                else
                {
                    Response.Redirect(siteUrl + landingPageUrl);
                }
            }

            //if (currentUser != null)
            //{
            //    if (currentUser.UserRoleId == null)
            //    {
            //        Response.Redirect("~/Pages/UserHomePage" + param);
            //    }

            //    var isSuperAdmin = userManager.IsUGITSuperAdmin(currentUser);

            //    if (isSuperAdmin)
            //    {
            //        Response.Redirect("~/SuperAdmin/SuperAdmin.aspx");
            //    }

            //}
            
            var isUGITSuperAdmin = manager.IsRole(RoleType.UGITSuperAdmin, user.UserName);

            if (isUGITSuperAdmin)
            {
                Response.Redirect("~/SuperAdmin/SuperAdmin.aspx"); ;
            }
            else
            {
                if (user.UserRoleId == null)
                {
                    DataTable dt = GetTableDataManager.GetTableData($"{DatabaseObjects.Tables.ConfigurationVariable}", $"{DatabaseObjects.Columns.KeyName}='ClientType'", $"{DatabaseObjects.Columns.KeyValue}", null);
                    string ClientType = (dt != null && dt.Rows.Count > 0) ? UGITUtility.ObjectToString(dt.Rows[0][0]) : string.Empty;
                    //Response.Redirect("~/Pages/UserHomePage" + param);
                    if (ClientType.EqualsIgnoreCase("GC"))
                    {
                        if (uHelper.IsDataAllSet(_context))
                        {
                            string landingPage = uHelper.GetDefaultLandingPage(_context, user, param);
                            //Response.Redirect("~/Pages/UserHomePage" + param);
                            Response.Redirect("~" + landingPage);
                        }
                        else
                            Response.Redirect("~/SitePages/NewLoginWizard" + param); 
                    }
                    else
                        Response.Redirect("~/Pages/UserHomePage" + param);
                }
                else
                {
                    landingPageUrl = new LandingPagesManager(ApplicationContext).GetLandingPageById(user.UserRoleId);
                    landingPageUrl = new LandingPagesManager(ApplicationContext).LandingPagesfinal(user);

                    if (landingPageUrl.Contains("UserHomePage"))
                    {
                        Response.Redirect($"~/Pages/{landingPageUrl}" + param);
                    }
                    else
                    {
                        Response.Redirect($"~/Pages/{landingPageUrl}");
                    }

                }
            }
        }

    }
        
    }


