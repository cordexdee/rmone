using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Manager;
using System.Threading;

namespace uGovernIT.Web
{
    public partial class ShowDashboardView : UserControl
    {
        protected string filterTicketsPageUrl = string.Empty;
        protected string dasbhoardViewPage = string.Empty;
        public string DashboardView { get; set; }
        public int DashboardViewID { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DashboardPanelView dashboardView = null;
        DashboardManager dManager = null;
        DataTable filterValues = new DataTable();
        static Object lockObj = new object();
        protected override void OnInit(EventArgs e)
        {
            
            dManager = new DashboardManager(context);
            var dViewManager = new DashboardPanelViewManager(context);

            if (DashboardViewID > 0)
                dashboardView = dViewManager.LoadViewByID(DashboardViewID);
            else
                dashboardView = dViewManager.LoadViewByName(DashboardView);

            List<Dashboard> dashboards = null;

            Control exedashDiv = FindControl("exeDashboard");
            exedashDiv.Visible= false;
            lblExecDash.Visible= false;
            if (dashboardView.Title.ToUpper() == "PROJECTS")
            {
                exedashDiv.Visible = true;
                lblExecDash.Visible = true;
                lblExecDash.Text = "Executive Dashboard";
            }
            if (dashboardView != null)
                switch (dashboardView.ViewType.ToLower())
                {
                    case "indivisible dashboards":
                        IndivisibleDashboardsView indiView = (dashboardView.ViewProperty as IndivisibleDashboardsView);
                        var Dashborad = indiView.Dashboards;
                        var Dashborad_globalfilter = indiView.GlobalFilers;
                        Dashborad.OrderBy(m => m.ItemOrder);
                        dashboards = dManager.LoadDashboardsByNames(Dashborad.Select(x => x.DashboardName).ToList(), true);
                        dashboards = dashboards.GroupBy(x => x.ID).Select(y => y.First()).Distinct().ToList();
                        for (int i = 0; i < dashboards.Count; i++)
                        {
                            var DashboardIndView = Dashborad.First(x => x.DashboardName == dashboards[i].Title);
                            if (DashboardIndView.DisableInheritDefault)
                            {
                                dashboards[i].PanelHeight = DashboardIndView.Height;
                                dashboards[i].ThemeColor = DashboardIndView.Theme;
                                dashboards[i].ItemOrder = DashboardIndView.ItemOrder;
                                dashboards[i].PanelWidth = DashboardIndView.Width;
                                dashboards[i].Title = DashboardIndView.DisplayName;
                                dashboards[i].Icon = DashboardIndView.IconUrl;
                                dashboards[i].panel.StartFromNewLine = DashboardIndView.StartFromNewLine;
                            }
                        }

                        //for (int i = 0; i < Dashborad_globalfilter.Count; i++)
                        //{
                        //    var DashboardIndView = Dashborad_globalfilter.First(x => x.ColumnName == Dashborad_globalfilter[i].ColumnName);
                        //    if (DashboardIndView.ColumnName.EndsWith("Lookup"))
                        //    {
                        //        DashboardIndView.ColumnName = DashboardIndView.ColumnName + "$";
                        //    }
                        //}

                        dashboards = dashboards.OrderBy(t => t.ItemOrder).ToList();
                        lock (lockObj)
                        {
                            foreach (Dashboard dashboard in dashboards)
                            {
                                //Check current user has permission to see dashboard or not
                                if (string.IsNullOrEmpty(dashboard.DashboardPermission) || dashboard.DashboardPermission.Contains(HttpContext.Current.CurrentUser().Id))
                                {
                                    CustomDashboardPanel panel = (CustomDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomDashboardPanel.ascx");
                                    panel.ID = "paneCustomDashboard" + dashboard.ID;
                                    panel.dashboard = dashboard;
                                    panel.UseAjax = true;

                                    panel.EnableViewState = false;
                                    panel.ViewID = Convert.ToInt32(dashboardView.ID);
                                    dashobardPanelsContainer.Controls.Add(panel);

                                    ////if (o == 0)
                                    ////{
                                    //    o++;

                                    //    DevxChartControl panel = (DevxChartControl)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/DevxChartControl.ascx");
                                    //    panel.Dashboard = dashboard;
                                    //    panel.ViewID = dashboardView.ID;
                                    //    dashobardPanelsContainer.Controls.Add(panel);
                                    ////}
                                }
                            }
                            LoadDashboardFilter(Convert.ToInt32(dashboardView.ID), indiView.GlobalFilers);
                        }
                        
                        break;
                    case "super dashboards":
                        SuperDashboardsView superView = (dashboardView.ViewProperty as SuperDashboardsView);
                        var SuperDashborad = superView.DashboardGroups.OrderBy(t => t.ItemOrder).ToList();

                        for (int i = 0; i < SuperDashborad.Count; i++)
                        {
                            dashboards = dManager.LoadDashboardsByNames(SuperDashborad[i].Dashboards.Select(x => x.DashboardName).ToList());

                            for (int j = 0; j < SuperDashborad[i].Dashboards.Count; j++)
                            {
                                DashboardPanelProperty dashboardProperty = SuperDashborad[i].Dashboards[j];
                                Dashboard uDashboard = dashboards.FirstOrDefault(x => x.Title == dashboardProperty.DashboardName);
                                int index = dashboards.IndexOf(uDashboard);
                                if (uDashboard != null && index >= 0)
                                {
                                    if (uDashboard != null && dashboardProperty.DisableInheritDefault)
                                    {
                                        dashboards[index].PanelHeight = dashboardProperty.Height;
                                        dashboards[index].ItemOrder = dashboardProperty.ItemOrder;
                                        dashboards[index].PanelWidth = SuperDashborad[i].Width;
                                        dashboards[index].Title = dashboardProperty.DisplayName;
                                        dashboards[index].Icon = dashboardProperty.IconUrl;
                                    }
                                    else
                                    {
                                        dashboards[index].PanelWidth = SuperDashborad[i].Width;
                                    }
                                }
                                else
                                {
                                    Util.Log.ULog.WriteLog("Dashboard not found: " + dashboardProperty.DashboardName);
                                }
                            }

                            dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
                            CustomGroupDashboardPanel panel1 = (CustomGroupDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanel.ascx");
                            panel1.ID = ("customGroupPanel_" + i + panel1.ViewID + dashboardView.ViewName).Trim();
                            panel1.DashboardList = dashboards;
                            panel1.PanelWidth = new Unit(string.Format("{0}px", SuperDashborad[i].Width));
                            panel1.ViewID = Convert.ToInt32(dashboardView.ID);
                            dashobardPanelsContainer.Controls.Add(panel1);
                        }
                        LoadDashboardFilter(Convert.ToInt32(dashboardView.ID), superView.GlobalFilers);
                        break;
                    case "side dashboards":
                        //uGovernITSideBarUserControl sideBarPanel = (uGovernITSideBarUserControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/uGovernITSideBarUserControl.ascx");
                        //sideBarPanel.ID = "sideBarPanel";
                        //sideBarPanel.ViewName = DashboardView;
                        //dashobardPanelsContainer.Controls.Add(sideBarPanel);
                        break;
                }


            /*   //Load Dashboard group by dashboard group title
               //UDashboardGroup dashboardGroup = UDashboardGroup.LoadByTitle();
               if (dashboardGroup != null)
               {
               

                   #region Create all global filter

                   HtmlTable filterTable = new HtmlTable();
                   filterTable.Width = "100%";
                   filterTable.CellPadding = 0;
                   filterTable.CellSpacing = 0;
                   HtmlTableRow filterRow = new HtmlTableRow();
                   HtmlTableCell filterCell = new HtmlTableCell();
                   filterTable.Rows.Add(filterRow);
                   filterRow.Cells.Add(filterCell);

                   //Get Globalfilters based on dashboard Group
                   DataTable globalFilterTable = uGITCache.GetDataTable(DatabaseObjects.Lists.ChartGlobalFilters);
                   DataRow[] selectedGlobalFilters = uHelper.GetMultipleLookupValueExistData(globalFilterTable, DatabaseObjects.Columns.DashboardMultiLookup, dashboardGroup.Title);
               
                   if (selectedGlobalFilters.Length > 0)
                   {
                       dashobardPanelsContainer.Width = "80%";
                       globalFilterContainer.Width = "20%";
                       globalFilterContainer.VAlign = "top";

                       #region Globalfilter header which show clear filter button start
                       Panel globalFilterHead = new Panel();
                       Label filterLabel = new Label();
                       filterLabel.EnableViewState = true;
                       filterLabel.Text = "Filters";
                       filterLabel.CssClass = "filterheader";
                       LinkButton bt = new LinkButton();
                       bt.Text = "Clear&nbsp;Filters";
                       bt.CssClass = "filterlink removeallglobalfilterbutton ugitsellinkbg ugitsellinkborder faddedlink";
                       bt.ID = "btClearAllFilter";
                       bt.Attributes.Add("href", "javascript:");
                       bt.Enabled = false;
                       globalFilterHead.Controls.Add(filterLabel);
                       globalFilterHead.Controls.Add(bt);
                       globalFilterContainer.Controls.Add(globalFilterHead);
                       #endregion

                       #region Create global filter
                       Panel dynamicFilter = new Panel();
                       dynamicFilter.ID = "dynamicFilter";

                       if (globalFilterTable.Columns.Contains(DatabaseObjects.Columns.ColumnName))
                       {
                           // Load to show all global filter
                           foreach (DataRow globalFilter in selectedGlobalFilters)
                           {
                               if (!string.IsNullOrEmpty(Convert.ToString(globalFilter[DatabaseObjects.Columns.ColumnName])))
                               {   
                                   //Get global filter data based on which user can apply fitler
                                   string filterDataType = "String";
                                   DataTable filterValues = GetDatatableForGlobalFilter(globalFilter, ref filterDataType);
                                   if (filterValues.Rows.Count > 0)
                                   {
                                       ListBox listBox = new ListBox();
                                       listBox.AutoPostBack = false;
                                       listBox.Attributes.Add("globalfilterid", Convert.ToString(globalFilter[DatabaseObjects.Columns.Id]));
                                       listBox.Attributes.Add("dataType", filterDataType);
                                       listBox.CssClass = "globalfilterlistbox";
                                       listBox.SelectionMode = ListSelectionMode.Multiple;
                                       foreach (DataRow row in filterValues.Rows)
                                       {
                                           listBox.Items.Add(new ListItem(row["Value"].ToString(), row["Value"].ToString()));
                                       }
                                       Panel globalFilterpanel = new Panel();
                                       globalFilterpanel.Controls.Add(listBox);
                                       if (filterDataType == "DateTime")
                                       {
                                           globalFilterpanel.Controls.Add(new LiteralControl("<span style='float:left;padding-left:2px;' class='daterangep'><input type='text' class='to watermark' watermark='MMYYYY' value='MMYYYY' style='width:75px;height:15px;font-size:11px'/>&nbsp;To&nbsp;<input type='text' style='width:75px;height:15px;font-size:11px' class='from watermark'  watermark='MMYYYY' value='MMYYYY'/><input class='globalfilterdaterangeButton' type='button' value='Go'/></span>"));
                                       }
                                       CustomPanelBox filterPanelBlock = (CustomPanelBox)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/CustomPanelBox.ascx");
                                       filterPanelBlock.ContentControl = globalFilterpanel;
                                       filterPanelBlock.BoxTitle = Convert.ToString(uHelper.GetSPItemValue(globalFilter, DatabaseObjects.Columns.Title));
                                       dynamicFilter.Controls.Add(filterPanelBlock);
                                   }
                               }
                           }
                       }

                       globalFilterContainer.Controls.Add(dynamicFilter);

                       #endregion
                   }

                   #endregion
               }
             * */

            base.OnInit(e);
        }

        private void LoadDashboardFilter(int viewID, List<DashboardFilterProperty> sfilters)
        {

            if (sfilters == null || sfilters.Count <= 0)
                return;

            List<DashboardFilterProperty> filters = sfilters.Where(x => !x.Hidden).ToList();

            #region Create all global filter

            HtmlTable filterTable = new HtmlTable();
            filterTable.Width = "100%";
            filterTable.CellPadding = 0;
            filterTable.CellSpacing = 0;
            HtmlTableRow filterRow = new HtmlTableRow();
            HtmlTableCell filterCell = new HtmlTableCell();
            filterTable.Rows.Add(filterRow);
            filterRow.Cells.Add(filterCell);


            dashobardPanelsContainer.Width = "99%";
            globalFilterContainer.Width = "20%";
            globalFilterContainer.VAlign = "top";

            #region Globalfilter header which show clear filter button start
            Panel globalFilterHead = new Panel();
            Label filterLabel = new Label();
            filterLabel.EnableViewState = true;
            filterLabel.Text = "Filters";
            filterLabel.CssClass = "filterheader";
            LinkButton bt = new LinkButton();
            bt.Text = "Clear&nbsp;Filters";
            bt.CssClass = "filterlink removeallglobalfilterbutton ugitsellinkbg ugitsellinkborder faddedlink";
            //bt.ID = "btClearAllFilter";
            bt.Attributes.Add("href", "javascript:");
            bt.Enabled = false;
            LinkButton btApply = new LinkButton();
            btApply.Text = "Apply&nbsp;Filters";
            btApply.CssClass = "filterlink applyglobalfilterbutton marggin ugitsellinkbg ugitsellinkborder faddedlink";
            btApply.Attributes.Add("href", "javascript:");
            btApply.Enabled = false;
            globalFilterHead.Controls.Add(filterLabel);
            globalFilterHead.Controls.Add(bt);
            globalFilterHead.Controls.Add(btApply);
            globalFilterContainer.Controls.Add(globalFilterHead);

            #endregion

            #region Create global filter
            Panel dynamicFilter = new Panel();
            //dynamicFilter.ID = "dynamicFilter";


            // Load to show all global filter
            List<DashboardFilterProperty> gFilters = filters.OrderBy(x => x.ItemOrder).ToList();
            foreach (DashboardFilterProperty globalFilter in gFilters)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(globalFilter.ColumnName)))
                {
                    //Get global filter data based on which user can apply fitler
                    string filterDataType = "String";
                    DataTable filterValues = DevxChartHelper.GetDatatableForGlobalFilter(context, globalFilter, ref filterDataType);
                    if (filterValues.Rows.Count > 0)
                    {
                        ListBox listBox = new ListBox();
                        listBox.PreRender += listBox_PreRender;
                        listBox.AutoPostBack = false;
                        listBox.Attributes.Add("globalfilterid", Convert.ToString(globalFilter.ID));
                        listBox.Attributes.Add("dataType", filterDataType);
                        listBox.CssClass = "globalfilterlistbox";
                        listBox.SelectionMode = ListSelectionMode.Multiple;
                        listBox.Attributes.Add("viewID", viewID.ToString());

                        foreach (DataRow row in filterValues.Rows)
                        {

                            listBox.Items.Add(new ListItem(row["Value"].ToString(), row["Value"].ToString()));
                        }

                        if (globalFilter.DefaultValues != null)
                        {
                            ListItem item = null;
                            foreach (string val in globalFilter.DefaultValues)
                            {
                                item = listBox.Items.FindByValue(val);
                                if (item != null)
                                    item.Selected = true;
                            }
                        }

                        Panel globalFilterpanel = new Panel();
                        globalFilterpanel.Controls.Add(listBox);
                        if (filterDataType == "DateTime")
                        {
                            globalFilterpanel.Controls.Add(new LiteralControl("<span style='float:left;padding-left:2px;' class='daterangep'><input type='text' class='to watermark' watermark='MM/YYYY' value='MM/YYYY' style='width:75px;height:15px;font-size:11px'/>&nbsp;To&nbsp;<input type='text' style='width:75px;height:15px;font-size:11px' class='from watermark'  watermark='MM/YYYY' value='MM/YYYY'/><input class='globalfilterdaterangeButton' type='button' value='Go'/></span>"));
                        }
                        CustomPanelBox filterPanelBlock = (CustomPanelBox)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomPanelBox.ascx");
                        filterPanelBlock.ContentControl = globalFilterpanel;
                        filterPanelBlock.BoxTitle = globalFilter.Title;
                        dynamicFilter.Controls.Add(filterPanelBlock);
                    }
                }
            }


            globalFilterContainer.Controls.Add(dynamicFilter);

            #endregion


            #endregion

        }

        private void CreateGroupView(DashboardGroupProperty dashboardGroup)
        {
            List<Dashboard> dashboards = null;
            dashboards = dManager.LoadDashboardsByNames(dashboardGroup.Dashboards.Select(x => x.DashboardName).ToList());

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
                }
                else
                {
                    dashboards[index].PanelWidth = dashboardGroup.Width;
                }
            }

            dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
            CustomGroupDashboardPanel panel1 = (CustomGroupDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/dashboard/CustomGroupDashboardPanel.ascx");
            panel1.DashboardList = dashboards;
            panel1.PanelWidth = new Unit(string.Format("{0}px", 150));

            dashobardPanelsContainer.Controls.Add(panel1);
        }

        ///// <summary>
        ///// Create DataTable for Sorting
        ///// </summary>
        //public void CreateTable()
        //{
        //    DataTable table = new DataTable();
        //    DataColumn col1 = new DataColumn(DatabaseObjects.Columns.Id, typeof(Int32));
        //    DataColumn col2 = new DataColumn(DatabaseObjects.Columns.Title, typeof(String));
        //    DataColumn col3 = new DataColumn(DatabaseObjects.Columns.DashboardType, typeof(String));
        //    table.Columns.Add(col1);
        //    table.Columns.Add(col2);
        //    table.Columns.Add(col3);
        //}    

        protected void Page_Load(object sender, EventArgs e)
        {
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ShowDashboardDetails.aspx");
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            //Get FilterTicket page url from config
            //ConfigurationVariable callLogVariable = configManager.Load("FilterTicketsPageUrl")[0]; // original code

            ConfigurationVariable callLogVariable = null;
            if (configManager.Load($"KeyName='FilterTicketsPageUrl'").Count > 0)
            {
                callLogVariable = configManager.Load($"KeyName='FilterTicketsPageUrl'")[0];
            }

            if (callLogVariable != null)
            {
                filterTicketsPageUrl = UGITUtility.GetAbsoluteURL(callLogVariable.KeyValue.Trim());
            }
        }
        void listBox_PreRender(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            foreach (ListItem item in listBox.Items)
            {
                item.Attributes.Add("title", Uri.EscapeUriString(item.Text));
            }

            //throw new NotImplementedException();
        }
    }
}
