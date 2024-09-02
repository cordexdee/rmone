using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using System.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class uGovernITDashboardChartsUserControl : UserControl
    {
        public string ViewName { get; set; }
        ApplicationContext _context = null;
        protected override void OnInit(EventArgs e)
        {

            _context = HttpContext.Current.GetManagerContext();
            // Check if the request is coming from a blackberry device.
            // If yes then redirect it to the mobile page.
            // This should be done in compat.browser.
            //if (Redirect.IsMobileRequest(Request.UserAgent))
            //{
            //    Response.Redirect(Redirect.GetMobileDashboardURL(SPContext.Current.Web));
            //}           
                string viewName = string.Empty;
                viewName = ViewName;
                if (string.IsNullOrWhiteSpace(ViewName) && !string.IsNullOrWhiteSpace(Convert.ToString(Page.RouteData.Values["name"])))
                {
                    viewName = Convert.ToString(Page.RouteData.Values["name"]);
                }
                if (!string.IsNullOrWhiteSpace(viewName))
                {
                    spNotAuthorizedUser.Visible = false;
                    DashboardPanelViewManager dviewManager = new DashboardPanelViewManager(_context);

                    DashboardPanelView dbView = dviewManager.LoadViewByName(viewName);// dviewManager.Load((uGovernITDashboardCharts)this.Parent).Dashboard, );
                    if (IsAuthorizedToView(dbView, _context))
                    {
                        if (dbView != null && dbView.ViewType == "Common Dashboards")
                        {
                            DashboardPreview dView = (DashboardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/DashboardPreview.ascx");
                            dView.DashboardViewID = Convert.ToInt32(dbView.ID);
                            dView.ID = "dashboardView" + dbView.ID;
                            dashboardView.Controls.Add(dView);
                        }
                        else
                        {

                            ShowDashboardView dView = (ShowDashboardView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/ShowDashboardView.ascx");
                            dView.DashboardView = viewName;
                            dView.ID = "dashboardView" + dbView.ID;
                            dashboardView.Controls.Add(dView);

                        }
                    }
                    else
                    {
                        spNotAuthorizedUser.Visible = true;
                    }
                }
                else
                {
                    return;
                }

            // Show help icon if there is any help exist
            // try
            // {
            //uGovernITDashboardCharts wepPartObj = (uGovernITDashboardCharts)this.Parent;

            //.DisplayHelpTextLink(wepPartObj, helpTextContainer);
            // }
            // catch
            //  {
            //  Log.WriteLog("Could not load Helper icon", "DashboardUserControl");
            // }


            if (Page != null && Page.IsCallback)
                EnsureChildControls();
        }
        private bool IsAuthorizedToView(DashboardPanelView view, ApplicationContext context)
        {
            bool isAuthorizedToView = false;
            if (view == null)
                return false;
            if ((view.AuthorizedToView == null || view.AuthorizedToView.Count == 0))
                return true;

            if ((view.AuthorizedToView.Exists(x => x.Id == context.CurrentUser.Id)))
                return true;

            List<UserProfile> groupInfo = view.AuthorizedToView.Where(x => x.isRole).ToList();
            foreach (UserProfile item in groupInfo)
            {
                if (item.isRole)
                {
                    isAuthorizedToView = true;
                    break;
                }
            }
            return isAuthorizedToView;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }
    }
}
