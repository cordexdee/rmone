using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;
using System.Linq;
using uGovernIT.Web.ControlTemplates.DockPanels;
//using uGovernIT.Web.ControlTemplates.DockPanels;

namespace uGovernIT.Web.ControlTemplates.GlobalPage
{
    public class UPage : Page
    {
        public PageConfiguration PageConfiguration { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            //if (Request.Path.Contains("undefined"))
            //{
            //    return;
            //}
            string pageName = Page.AppRelativeVirtualPath.Replace("~/", "");
            if (!pageName.StartsWith("RoutingPage.aspx", StringComparison.OrdinalIgnoreCase) && !pageName.StartsWith("default", StringComparison.OrdinalIgnoreCase))
            {
                PageConfiguration = null;
                return;
            }

            if (pageName.EndsWith(".aspx", StringComparison.CurrentCultureIgnoreCase))
                pageName = pageName.Substring(0, pageName.LastIndexOf(".aspx"));

            if (pageName != "default" && pageName.Split('/').Length > 1)
                pageName = pageName.Split('/')[1];
            else
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(Page.RouteData.Values["name"])))
                {
                    pageName = Convert.ToString(Page.RouteData.Values["name"]);
                }
            }

            if (pageName == "default" || string.IsNullOrEmpty(pageName))
                pageName = "Home";

            PageConfiguration = HttpContext.Current.GetManagerContext().PageManager.GetCachePage(pageName);
            base.OnPreInit(e);
        }

    }

    public static class PageEx
    {
        public static PageConfiguration PageConfig(this Page page)
        {
            if (!(page is UPage))
                return null;

            return (page as UPage).PageConfiguration;
        }

        public static void SetDockContainer(Control ctrl)
        { 
            PageConfiguration pageConfig = ctrl.Page.PageConfig();
            if (pageConfig != null && pageConfig.ControlInfoList != null && pageConfig.ControlInfoList.Count > 0)
            {
                Panel dockContainer = new Panel();
                dockContainer.ID = "controlPanel";
                ctrl.Controls.Add(dockContainer);
                foreach (DockPanelSetting setting in pageConfig.ControlInfoList)
                {
                    if (pageConfig.LayoutInfoList!=null && pageConfig.LayoutInfoList.ContainsKey(setting.ControlID) && pageConfig.LayoutInfoList[setting.ControlID].Count >= 8)
                        setting.PanelOrder = UGITUtility.StringToInt(pageConfig.LayoutInfoList[setting.ControlID][7]);
                }
                
                pageConfig.ControlInfoList = pageConfig.ControlInfoList.OrderBy(x => x.PanelOrder).ToList();

                foreach (DockPanelSetting setting in pageConfig.ControlInfoList)
                {
                    DockPanel obj = null;
                    if (setting != null && !string.IsNullOrEmpty(setting.AssemblyName))
                    {
                        setting.Name = pageConfig.Name;
                        Type type = Type.GetType(setting.AssemblyName);
                        obj = Activator.CreateInstance(type) as DockPanel;
                        obj.DockSetting = setting;
                        obj.NeedData = true;
                        obj.CssClass = "homeRightPanel_wrap";
                    }
                    if (obj != null)
                    {
                        Control ctr = obj.LoadControl(ctrl.Page);
                        if (ctr != null)
                            dockContainer.Controls.Add(ctr);
                        else
                            dockContainer.Controls.Add(obj);

                        Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.GetManagerContext()?.CurrentUser?.Name}: Visited Page: {pageConfig.RootFolder}/{pageConfig.Name}");
                    }
                }
            }
        }
    }
}