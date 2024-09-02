using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Manager.Managers;
namespace uGovernIT.Web
{
    public partial class uGovernITSideBarUserControl : UserControl
    {
        protected string filterTicketsPageUrl = string.Empty;
        protected string dasbhoardViewPage = string.Empty;
        protected string filteredDataDetailPage = string.Empty;
        DashboardPanelView view = null;
        ConfigurationVariableManager configManager;
        DashboardPanelViewManager objDashboardPanelViewManager;
        DashboardManager objDashboardManager;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string ViewName { get; set; }
        protected override void OnInit(EventArgs e)
        {
            configManager = new ConfigurationVariableManager(_context);
            objDashboardPanelViewManager = new DashboardPanelViewManager(_context);
            objDashboardManager = new DashboardManager(_context);
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/ShowDashboardDetails.aspx");
            filteredDataDetailPage = UGITUtility.GetAbsoluteURL(configManager.GetValue("FilterTicketsPageUrl"));

            //Get FilterTicket page url from config
            ConfigurationVariable callLogVariable = configManager.LoadVaribale("FilterTicketsPageUrl");
            if (callLogVariable != null)
            {
                filterTicketsPageUrl = UGITUtility.GetAbsoluteURL(callLogVariable.KeyValue.Trim());
            }

            //  uGovernITSideBar wp = (uGovernITSideBar)this.Parent;

            // view = DashboardPanelView.();
            view = objDashboardPanelViewManager.LoadViewByName(ViewName);
            SideDashboardView viewProperty = null;

            if (view != null && view.ViewProperty is SideDashboardView)
            {
                viewProperty = (SideDashboardView)view.ViewProperty;
            }

            if (viewProperty == null)
            {
                return;
            }

            DataTable table = CreateTable();
            var lstSideGroup = viewProperty.DashboardGroups.OrderBy(t => t.ItemOrder).ToList();
            var lstSideLink = viewProperty.DashboardSideList.OrderBy(t => t.ItemOrder).ToList();
            var lstDashBoards = viewProperty.Dashboards.OrderBy(t => t.ItemOrder).ToList();

            for (int i = 0; lstSideLink.Count > i; i++)
            {
                DataRow dr = table.NewRow();
                dr[DatabaseObjects.Columns.ItemOrder] = lstSideLink[i].ItemOrder;
                dr[DatabaseObjects.Columns.Title] = lstSideLink[i].Title;
                dr["Type"] = "link";
                dr["GroupTheme"] = string.Empty;
                dr["BackgroundImage"] = string.Empty;
                dr["BackgroundIcon"] = string.Empty;
                table.Rows.Add(dr);
            }

            for (int j = 0; lstSideGroup.Count > j; j++)
            {
                DataRow dr = table.NewRow();
                dr[DatabaseObjects.Columns.ItemOrder] = lstSideGroup[j].ItemOrder;
                dr[DatabaseObjects.Columns.Title] = lstSideGroup[j].DashboardGroup;
                dr["Type"] = "group";
                dr["GroupTheme"] = lstSideGroup[j].GroupTheme;
                dr["BackgroundImage"] = lstSideGroup[j].BackgroundImage;
                dr["BackgroundIcon"] = lstSideGroup[j].BackgroundIcon;
                table.Rows.Add(dr);
            }

            for (int j = 0; lstDashBoards.Count > j; j++)
            {
                DataRow dr = table.NewRow();
                dr[DatabaseObjects.Columns.ItemOrder] = lstDashBoards[j].ItemOrder;
                dr[DatabaseObjects.Columns.Title] = lstDashBoards[j].DashboardName;
                dr["Type"] = "dashboard";
                dr["GroupTheme"] = lstDashBoards[j].Theme;
                dr["BackgroundImage"] = lstDashBoards[j].BackGroundUrl;
                dr["BackgroundIcon"] = lstDashBoards[j].IconUrl;
                table.Rows.Add(dr);
            }

            DataView dataView = table.DefaultView;
            dataView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);


            for (int i = 0; i < dataView.Count; i++)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                prviewTable.Rows.Add(row);
                row.Cells.Add(cell);
                cell.VerticalAlign = VerticalAlign.Top;
                cell.HorizontalAlign = HorizontalAlign.Center;

                if (dataView[i]["Type"].ToString().ToLower() == "link")
                {
                    var sideLindProp = lstSideLink.FirstOrDefault(x => x.Title == Convert.ToString(dataView[i][DatabaseObjects.Columns.Title]));
                    CustomSideBarLink panel1 = (CustomSideBarLink)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomSideBarLink.ascx");
                    panel1.ID = "customSideBarLink";
                    //CustomDashboardPanel panel = (CustomDashboardPanel)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/CustomDashboardPanel.ascx");
                    //panel.ContentControl = panel1;
                    panel1.dbSideProperty = sideLindProp;

                    if (sideLindProp.IsFlat)
                    {
                         cell.Controls.Add(panel1);                        
                    }
                    else
                    {
                        CustomGroupDashboardPanel dashboardPanel = (CustomGroupDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanel.ascx");
                        dashboardPanel.ID = "customGroupDashboardPanel";
                        dashboardPanel.ContentControl = panel1;
                        dashboardPanel.PanelWidth = new Unit(132);
                        dashboardPanel.PanelHeight = new Unit(sideLindProp.Height > 30 ? sideLindProp.Height : 30);
                        dashboardPanel.ShowControl = true;
                        cell.Controls.Add(dashboardPanel);
                        // cell.Controls.Add(panel1);
                    }
                }               
                else if (dataView[i]["Type"].ToString().ToLower() == "group")
                {
                    var sideGroupView = lstSideGroup.Find(x => x.DashboardGroup == Convert.ToString(dataView[i][DatabaseObjects.Columns.Title]));
                  
                        CustomGroupDashboardPanel panel = LoadGroupViewCtr(sideGroupView);
                        if (panel != null)
                        {
                            cell.Controls.Add(panel);
                        }                    
                }
                else if (dataView[i]["Type"].ToString().ToLower() == "dashboard")
                {
                    var sideDashBoardView = lstDashBoards.Find(x => x.DashboardName == Convert.ToString(dataView[i][DatabaseObjects.Columns.Title]));
                    if (!string.IsNullOrEmpty(Convert.ToString(dataView[i]["GroupTheme"])) && (Convert.ToString(dataView[i]["GroupTheme"]).ToLower() == "custom"))
                    {
                        CustomGroupDashboardPanelThemed panel = LoadGroupViewCtrThemed(sideDashBoardView);
                        if (panel != null)
                        {
                            cell.Controls.Add(panel);
                        }
                    }
                    else
                    {
                        CustomGroupDashboardPanel panel = LoadGroupViewCtr(sideDashBoardView);
                        if (panel != null)
                        {
                            cell.Controls.Add(panel);
                        }
                    }
                }
            }
            base.OnInit(e);
        }

        private CustomGroupDashboardPanel LoadGroupViewCtr(DashboardGroupProperty dashboardGroup)
        {
            List<Dashboard> dashboards = null;
          
            dashboards = objDashboardManager.LoadDashboardsByNames(dashboardGroup.Dashboards.Select(x => x.DashboardName).ToList());
            if (dashboards == null || dashboards.Count <= 0)
            {
                return null;
            }

            for (int i = 0; i < dashboardGroup.Dashboards.Count; i++)
            {
                DashboardPanelProperty dashboardProperty = dashboardGroup.Dashboards[i];
                Dashboard uDashboard = dashboards.FirstOrDefault(x => x.Title == dashboardProperty.DashboardName);
                int index = dashboards.IndexOf(uDashboard);
                if (uDashboard != null && dashboardProperty.DisableInheritDefault)
                {
                    dashboards[index].PanelHeight = dashboardProperty.Height;
                    dashboards[index].ItemOrder = dashboardProperty.ItemOrder;
                    dashboards[index].PanelWidth = dashboardGroup.Width;
                    dashboards[index].Title = dashboardProperty.DisplayName;
                    dashboards[index].Icon = dashboardProperty.IconUrl;

                    if (!string.IsNullOrEmpty(dashboardProperty.DashboardUrl) && uDashboard.panel is PanelSetting)
                    {
                        PanelSetting pSetting = (PanelSetting)uDashboard.panel;
                        foreach (DashboardPanelLink exp in pSetting.Expressions)
                        {
                            exp.StopLinkDetail = false;
                            exp.DefaultLink = false;
                            exp.ScreenView = dashboardProperty.NavigationType;
                            exp.LinkUrl = dashboardProperty.DashboardUrl;

                        }
                    }
                    
                }
                else
                {
                    dashboards[index].PanelWidth = dashboardGroup.Width;
                }
            }


            dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
            CustomGroupDashboardPanel panel1 = (CustomGroupDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanel.ascx");
            panel1.ID = "customGroupDashboardPanel";
            panel1.DashboardList = dashboards;
            panel1.PanelWidth = new Unit(string.Format("{0}px", 132));
            panel1.Sidebar = true;
            panel1.ViewID = Convert.ToInt32(view.ID);
            return panel1;


        }

        private CustomGroupDashboardPanel LoadGroupViewCtr(DashboardPanelProperty dashboard)
        {
            if (dashboard == null)
            {
                return null;
            }

            List<Dashboard> dashboards = null;
            dashboards = objDashboardManager.LoadDashboardsByNames(new List<string>() { dashboard.DashboardName });

            Dashboard uDashboard = dashboards.FirstOrDefault();
            uDashboard.PanelHeight = dashboard.Height;
            uDashboard.ItemOrder = dashboard.ItemOrder;
            uDashboard.PanelWidth = dashboard.Width;
            uDashboard.Title = dashboard.DisplayName;
            uDashboard.Icon = dashboard.IconUrl;


            if (!string.IsNullOrEmpty(dashboard.DashboardUrl) && uDashboard.panel is PanelSetting)
            {
                PanelSetting pSetting = (PanelSetting)uDashboard.panel;
                foreach (DashboardPanelLink exp in pSetting.Expressions)
                {
                    exp.StopLinkDetail = false;
                    exp.DefaultLink = false;
                    exp.ScreenView = dashboard.NavigationType;
                    exp.LinkUrl = dashboard.DashboardUrl;

                }
            }

            CustomGroupDashboardPanel panel1 = (CustomGroupDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanel.ascx");
            panel1.ID = "customGroupDashboardPanel";
            panel1.DashboardList.Add(uDashboard);
            panel1.PanelWidth = new Unit(string.Format("{0}px", 132));
            panel1.Sidebar = true;
            panel1.ViewID = Convert.ToInt32(view.ID);
            return panel1;
        }   

        private CustomGroupDashboardPanelThemed LoadGroupViewCtrThemed(DashboardPanelProperty dashboard)
        {           
            if (dashboard == null)
            {
                return null;
            }


            List<Dashboard> dashboards = null;
            dashboards = objDashboardManager.LoadDashboardsByNames(new List<string>(){dashboard.DashboardName});
            if (dashboards == null || dashboards.Count==0)
                return null;
            Dashboard uDashboard = dashboards.FirstOrDefault();
            uDashboard.PanelHeight = dashboard.Height;
            uDashboard.ItemOrder = dashboard.ItemOrder;
            uDashboard.PanelWidth = dashboard.Width;
            uDashboard.Title = dashboard.DisplayName;
            uDashboard.Icon = dashboard.IconUrl;

            if (!string.IsNullOrEmpty(dashboard.DashboardUrl) && uDashboard.panel is PanelSetting)
            {
                PanelSetting pSetting = (PanelSetting)uDashboard.panel;
                foreach (DashboardPanelLink exp in pSetting.Expressions)
                {
                    exp.StopLinkDetail = false;
                    exp.DefaultLink = false;
                    exp.ScreenView = dashboard.NavigationType;
                    exp.LinkUrl = dashboard.DashboardUrl;
                }
            }

            CustomGroupDashboardPanelThemed panel1 = (CustomGroupDashboardPanelThemed)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanelThemed.ascx");
            panel1.ID = "customGroupDashboardPanel";
            panel1.DashboardObj = uDashboard;
            panel1.PanelWidth = new Unit(string.Format("{0}px", 132));
            panel1.Sidebar = true;
            panel1.ViewID = Convert.ToInt32(view.ID);
            panel1.BackgroundImage = dashboard.BackGroundUrl;
            panel1.BackgroundIcon = dashboard.IconUrl;
            return panel1;
        }
        /// <summary>
        /// Create DataTable for Sorting
        /// </summary>
        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn col1 = new DataColumn(DatabaseObjects.Columns.ItemOrder, typeof(Int32));
            DataColumn col2 = new DataColumn(DatabaseObjects.Columns.Title, typeof(String));
            DataColumn col3 = new DataColumn("Type", typeof(String));
            DataColumn col4 = new DataColumn("GroupTheme", typeof(String));
            DataColumn col5 = new DataColumn("BackgroundImage", typeof(String));
            DataColumn col6 = new DataColumn("BackgroundIcon", typeof(String));
            table.Columns.Add(col1);
            table.Columns.Add(col2);
            table.Columns.Add(col3);
            table.Columns.Add(col4);
            table.Columns.Add(col5);
            table.Columns.Add(col6);
            return table;
        }
    }
}
