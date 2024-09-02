using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Data;
using System.Collections.Generic;
using DevExpress.Web;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Drawing;
using DevExpress.Web.ASPxTreeList;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class DashboardDesigner : UserControl
    {
        public int viewID { get; set; }
        public List<DashboardPanelProperty> lstDBPanProp = null;
        public string WikiPickerUrl = string.Empty;
        List<Utility.Dashboard> listofDB = null;
        DashboardPanelView View = null;
        CommonDashboardsView commonViewObj = null;
        QueryParameters _QueryParameter = null;
        private int topOffset = 40;
        private int defaultTop = 40;
        private int defaultLeft = 10;
        private int defaultWidth = 150;
        private int defaultHeight = 120;
        public string delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=dashboardpreview&isudlg=1&viewid=");

        private const string absoluteUrlView = "layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&&ControlId={5}";

        private string newParam = "listpicker";
        private string formTitle = "Picker List";

        string CategoryPro { get; set; }
        private string absPath = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DashboardManager dManager = null;
        DashboardPanelViewManager objDashboardPanelViewManager = null;
        ConfigurationVariableManager config = null;
        HelpCardManager helpCardManager = null;
        protected override void OnInit(EventArgs e)
        {
            dManager = new DashboardManager(context);
            objDashboardPanelViewManager = new DashboardPanelViewManager(context);
            config = new ConfigurationVariableManager(context);
            helpCardManager = new HelpCardManager(context);

            listofDB = dManager.LoadAll(false).OrderBy(x => x.DashboardType).ThenBy(x => x.Title).ToList();

            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Link, Title = "Create Link" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MessageBoard", Title = "Message Board" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MyTasks", Title = "My Tasks" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "WaitingOnMe", Title = "Waiting On Me" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MyRequests", Title = "My Requests" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MyProjects", Title = "My Projects" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "CustomFilter", Title = "Custom Filter" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserHotList", Title = "User Hot List" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "WorkflowBottleneck", Title = "Workflow Bottleneck" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "SLAPerformanceTimeLine", Title = "SLA Performance Timeline" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "SLAPerformanceTabular", Title = "SLA Performance" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "OperationalDashboard", Title = "Operational Dashboard" });
            ///New UI Controls
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ScoreCard", Title = "Group Scorecard" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "TicketFlow", Title = "Ticket Flow by Group" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "WeeklyRollingAverage", Title = "Weekly Rolling Average" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "PredictBacklog", Title = "Predicted Backlog" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "OldestUnsolved", Title = "Oldest Unsolved" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "TSRTrendChart", Title = "TSR Trend Report" });
            //listofDB.Add(new UDashboard() { ID = 0, PanelType = DashboardType.Control, DashboardSubType = "ProblemReport", Title = "Problem Report" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "TicketByCategoryReport", Title = "Ticket By Category Report" });
            //listofDB.Add(new UDashboard() { ID = 0, PanelType = DashboardType.Control, DashboardSubType = "RequestReport", Title = "Request Report" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "TicketCreatedByWeek", Title = "Ticket Created Weekly Trend" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "GroupUnsolvedTickets", Title = "Unsolved Tickets by Group" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "AgentPerformance", Title = "Agent Performance" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "LeftTicketCountBar", Title = "RMM Left Count Bar" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserProjectPanel", Title = "User Projects" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MyProjectPanel", Title = "User Open Projects" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "LeftLinkBar", Title = "Left Link Bar" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ProjectPendingAllocation", Title = "Project Pending Allocations" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserWelcomePanel", Title = "User Welcome Panel" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserDetailsPanel", Title = "User Details Panel" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserChartPanel", Title = "User Chart Panel" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "HelpCartCtrl", Title = "Help" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UserChartDetail", Title = "User Chart Detail" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "HelpCardsPanel", Title = "Help Cards" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ITSMImpact", Title = "Impacts" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ServiceCatalog", Title = "Service Catalog" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "DailyTicketTrendsCount", Title = "Daily Tickets Trends Cound" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "SLAMetric", Title = "SLA Metric" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "AllocationTimeline", Title = "Allocation Timeline" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "AllocationTimelineNew", Title = "New Allocation Timeline" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ManagerProjectAllocationView", Title = "Manager Project Allocation View" });

            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "RoleWiseAllocation", Title = "Role Allocations" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "BillingForMonth", Title = "Billings" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "BarGaugeChart", Title = "Bar Gauge" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "SaleByDivision", Title = "Sales by Division" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "CommittedSalesBySector", Title = "Revenue by Sector" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "CommittedSalesByDivision", Title = "Revenue by Division" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "CommonResourceChart", Title = "Financial View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ResourceView", Title = "Resource View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "BillingWorkMonth", Title = "Billing Work Month" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ResourceNeededChart", Title = "Recruitment View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ProjectView", Title = "Project View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "RecruitmentView", Title = "Recruitment View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "CardKpis", Title = "Card Kpis" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "AllocationConflicts", Title = "Allocation Conflicts" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UnfilledAllocations", Title = "Unfilled Allocations" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UnfilledProjectAllocations", Title = "Unfilled Project Allocations" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "UnfilledPipelineAllocations", Title = "Unfilled Pipeline Allocations" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "OuterPanel", Title = "Outer Panel" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ExecutiveView", Title = "Executive View" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "ResourceAllocation", Title = "Resource Allocation" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "MyProjectCount", Title = "Project Count" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "PMOHome", Title = "PMO Home" });
            listofDB.Add(new Dashboard() { ID = 0, DashboardType = DashboardType.Control, DashboardSubType = "BenchAnalytics", Title = "Bench Analytics" });

            if (UGITUtility.StringToInt(Request["viewid"]) > 0)
            {
                viewID = UGITUtility.StringToInt(Request["viewid"]);
                View = objDashboardPanelViewManager.LoadViewByID(viewID, false);
                commonViewObj = View.ViewProperty == null ? new CommonDashboardsView() : (View.ViewProperty as CommonDashboardsView);


                lstDBPanProp = (commonViewObj == null || commonViewObj.Dashboards == null) ? new List<DashboardPanelProperty>() : commonViewObj.Dashboards;
                txtSaveAsName.Text = View.ViewName;
                //SPFieldUserValueCollection collection = new SPFieldUserValueCollection();
                //if (View.AuthorizedToView != null)
                //{
                //    View.AuthorizedToView.ForEach(x => { collection.Add(new SPFieldUserValue(SPContext.Current.Web, x.ID, x.Name)); });
                //    if (collection != null && collection.Count > 0)
                //        peAuthorizedToView.UpdateEntities(uHelper.getUsersListFromCollection(collection));
                //}
                peAuthorizedToView.SetValues(View.AuthorizedToViewUsers);
            }

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtHelp.ClientID));

            WikiPickerUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
            if (!IsPostBack)
            {
                FillUserDropDown();
            }
            FillFontFamily();
            //absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            //lnkbtnPickAssets.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromLibrary);
            lnkbackground.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));



            base.OnInit(e);
        }

        private void FillFontFamily()
        {
            string[] fontNames = { "Times New Roman", "Tahoma", "verdana", "Arial", "MS Sans Serif", "Courier", "Segoe UI", "Helvetica" };

            fontNames.ToList().ForEach(x => { ddlFontName.Items.Add(new ListItem(x)); });
            fontNames.ToList().ForEach(x => { ddlHeaderFontName.Items.Add(new ListItem(x)); });

        }

        private void FillUserDropDown()
        {
            DataTable dtTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.UserInformationList, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (dtTable != null)
            {
                cmbUser.DataSource = dtTable;
                cmbUser.DataBind();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            grid.DataBind();
            gridGlobalFilter.DataBind();
            base.OnPreRender(e);
        }

        private void UpdateDashboards()
        {
            foreach (KeyValuePair<string, object> kVal in hndDashboardsList.ToList())
            {
                if (!lstDBPanProp.Exists(x => x.DashboardName == Convert.ToString(kVal.Value)))
                {
                    DashboardPanelProperty dpProperty = new DashboardPanelProperty();

                    Dashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == Convert.ToString(kVal.Value) && x.DashboardType != DashboardType.Link);

                    if (uDashboard == null)
                    {
                        dpProperty.DisplayName = Convert.ToString(kVal.Value);
                        dpProperty.DashboardName = Convert.ToString(kVal.Value);
                    }
                    else
                    {

                        dpProperty.DisplayName = uDashboard.Title;
                        dpProperty.DashboardName = uDashboard.Title;
                        dpProperty.Theme = uDashboard.ThemeColor;
                    }
                    lstDBPanProp.Add(dpProperty);
                }
            }

            int i = 0;
            while (lstDBPanProp.Count > i)
            {
                var item = lstDBPanProp[i];

                if (hndDashboardsList.Contains(item.DashboardName))
                {
                    item.Width = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_width"]);
                    item.Height = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_height"]);
                    int top = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_top"]);

                    item.Top = GetAdjustedTop(top);
                    item.Left = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_left"]);

                    item.Zindex = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_zindex"]);

                    item.DashboardType = Convert.ToString(hndDashboards[item.DashboardName + "_paneltype"]);
                    item.DashboardSubType = Convert.ToString(hndDashboards[item.DashboardName + "_DashboardSubType"]);

                    if (hndDashboards.Contains(item.DashboardName + "_widthunit"))
                    {
                        item.WidthUnitType = UnitType.Pixel;
                        if (hndDashboards[item.DashboardName + "_widthunit"].ToString().Equals("Percentage"))
                        {
                            item.WidthUnitType = UnitType.Percentage;
                        }
                    }

                    if (hndDashboards.Contains(item.DashboardName + "_leftunit"))
                    {
                        item.LeftUnitType = UnitType.Pixel;
                        if (hndDashboards[item.DashboardName + "_leftunit"].ToString().Equals("Percentage"))
                        {
                            item.LeftUnitType = UnitType.Percentage;
                        }
                    }
                    if (hndDashboards.Contains(item.DashboardName + "_iconwidth"))
                        item.iconWidth = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_iconwidth"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iconheight"))
                        item.iconHeight = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_iconheight"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iconleft"))
                        item.iconLeft = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_iconleft"]);

                    if (hndDashboards.Contains(item.DashboardName + "_icontop"))
                        item.iconTop = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_icontop"]);

                    if (hndDashboards.Contains(item.DashboardName + "_linktype"))
                        item.LinkType = Convert.ToString(hndDashboards[item.DashboardName + "_linktype"]);

                    if (hndDashboards.Contains(item.DashboardName + "_linkdetails"))
                        item.LinkDetails = Convert.ToString(hndDashboards[item.DashboardName + "_linkdetails"]);

                    if (hndDashboards.Contains(item.DashboardName + "_navigationtype"))
                        item.NavigationType = UGITUtility.StringToShort(hndDashboards[item.DashboardName + "_navigationtype"]);

                    if (hndDashboards.Contains(item.DashboardName + "_theme"))
                        item.Theme = Convert.ToString(hndDashboards[item.DashboardName + "_theme"]);

                    if (hndDashboards.Contains(item.DashboardName + "_queryparameter"))
                        item.QueryParameter = Convert.ToString(hndDashboards[item.DashboardName + "_queryparameter"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iconurl"))
                        item.IconUrl = Convert.ToString(hndDashboards[item.DashboardName + "_iconurl"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iconshape"))
                        item.IconShape = Convert.ToString(hndDashboards[item.DashboardName + "_iconshape"]);

                    if (hndDashboards.Contains(item.DashboardName + "_islink"))
                        item.IsLink = Convert.ToBoolean(hndDashboards[item.DashboardName + "_islink"]);

                    if (hndDashboards.Contains(item.DashboardName + "_panelleft"))
                        item.PanelLeft = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_panelleft"]);

                    if (hndDashboards.Contains(item.DashboardName + "_paneltop"))
                        item.PanelTop = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_paneltop"]);

                    if (hndDashboards.Contains(item.DashboardName + "_background"))
                        item.BackGroundUrl = Convert.ToString(hndDashboards[item.DashboardName + "_background"]);

                    if (hndDashboards.Contains(item.DashboardName + "_fontstyle"))
                        item.FontStyle = Convert.ToString(hndDashboards[item.DashboardName + "_fontstyle"]);

                    if (hndDashboards.Contains(item.DashboardName + "_headerfontstyle"))
                        item.HeaderFontStyle = Convert.ToString(hndDashboards[item.DashboardName + "_headerfontstyle"]);

                    if (hndDashboards.Contains(item.DashboardName + "_titletop"))
                        item.TitleTop = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_titletop"]);

                    if (hndDashboards.Contains(item.DashboardName + "_titleleft"))
                        item.TitleLeft = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_titleleft"]);

                    if (hndDashboards.Contains(item.DashboardName + "_bordercolor"))
                        item.BorderColor = Convert.ToString(hndDashboards[item.DashboardName + "_bordercolor"]);

                    if (hndDashboards.Contains(item.DashboardName + "_module"))
                        item.Module = Convert.ToString(hndDashboards[item.DashboardName + "_module"]);

                    if (hndDashboards.Contains(item.DashboardName + "_psize"))
                        item.PageSize = UGITUtility.StringToInt(hndDashboards[item.DashboardName + "_psize"]);

                    if (hndDashboards.Contains(item.DashboardName + "_hidetitle"))
                        item.IsHideTitle = Convert.ToBoolean(hndDashboards[item.DashboardName + "_hidetitle"]);

                    if (hndDashboards.Contains(item.DashboardName + "_priority"))
                        item.Priority = Convert.ToString(hndDashboards[item.DashboardName + "_priority"]);

                    if (hndDashboards.Contains(item.DashboardName + "_duedate"))
                        item.DueDate = Convert.ToString(hndDashboards[item.DashboardName + "_duedate"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iscritical"))
                        item.IsCritical = Convert.ToBoolean(hndDashboards[item.DashboardName + "_iscritical"]);

                    if (hndDashboards.Contains(item.DashboardName + "_iscategory"))
                        item.Category = Convert.ToString(hndDashboards[item.DashboardName + "_iscategory"]);

                    if (hndDashboards.Contains(item.DashboardName + "_issubcategory"))
                        item.SubCategory = Convert.ToString(hndDashboards[item.DashboardName + "_issubcategory"]);

                    if (hndDashboards.Contains(item.DashboardName + "_status"))
                        item.Status = Convert.ToString(hndDashboards[item.DashboardName + "_status"]);

                    if (hndDashboards.Contains(item.DashboardName + "_enablefilter"))
                        item.EnableFilter = Convert.ToBoolean(hndDashboards[item.DashboardName + "_enablefilter"]);

                    if (hndDashboards.Contains(item.DashboardName + "_weeklyaverage"))
                        item.WeeklyAverage = Convert.ToString(hndDashboards[item.DashboardName + "_weeklyaverage"]);

                    if (hndDashboards.Contains(item.DashboardName + "_enablefilterforpredictbacklog"))
                        item.EnableFilterPredictBacklog = Convert.ToBoolean(hndDashboards[item.DashboardName + "_enablefilterforpredictbacklog"]);

                    if (hndDashboards.Contains(item.DashboardName + "_scorecardstartdate"))
                        item.ScoreCardStartDate = Convert.ToDateTime(hndDashboards[item.DashboardName + "_scorecardstartdate"]);

                    if (hndDashboards.Contains(item.DashboardName + "_scorecardenddate"))
                        item.ScoreCardEndDate = Convert.ToDateTime(hndDashboards[item.DashboardName + "_scorecardenddate"]);

                    if (hndDashboards.Contains(item.DashboardName + "_enablefilterticketflow"))
                        item.EnableFilterTicketFlow = Convert.ToBoolean(hndDashboards[item.DashboardName + "_enablefilterticketflow"]);

                    if (hndDashboards.Contains(item.DashboardName + "_ticketflowstartdate"))
                        item.TicketFlowStartDate = Convert.ToDateTime(hndDashboards[item.DashboardName + "_ticketflowstartdate"]);

                    if (hndDashboards.Contains(item.DashboardName + "_ticketflowenddate"))
                        item.TicketFlowEndDate = Convert.ToDateTime(hndDashboards[item.DashboardName + "_ticketflowenddate"]);

                    i++;    
                }
                else
                {
                    lstDBPanProp.Remove(item);
                }
            }

            commonViewObj.Dashboards = lstDBPanProp;
            View.ViewProperty = commonViewObj;
            objDashboardPanelViewManager.Save(View);

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Dashboard dashboard = null;

            if (!string.IsNullOrEmpty(hndDashboardName.Value))
            {
                dashboard = dManager.LoadDashboardsByNames(new List<string>() { hndDashboardName.Value.Trim() }).FirstOrDefault();
            }

            if (dashboard != null && dashboard.DashboardType == DashboardType.Query)
            {
                //_QueryParameter = (QueryParameters)Page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/QueryParameters.ascx");
                _QueryParameter = (QueryParameters)Page.LoadControl("~/ControlTemplates/uGovernIT/QueryParameters.ascx");
                var query = (DashboardQuery)dashboard.panel;
                if (query.QueryInfo.WhereClauses.Exists(x => x.Valuetype == qValueType.Parameter))
                {
                    DashboardPanelProperty dpProperty = lstDBPanProp.Where(x => x.DashboardName == hndDashboardName.Value.Trim()).FirstOrDefault();
                    _QueryParameter.ClearQueryParameterHTML(dashboard.ID);
                    _QueryParameter.QueryId = dashboard.ID;
                    _QueryParameter.Where = (dpProperty != null) ? dpProperty.QueryParameter : string.Empty;
                    _QueryParameter.Id = dashboard.ID;
                    tdParameter.Controls.Add(_QueryParameter);
                }
            }

            if (!IsPostBack)
            {
                List<Services> services = new List<Services>();//Services.LoadCurrentUserServices();
                foreach (Services s in services)
                {
                    ddlService.Items.Add(new ListItem(s.Title, s.ID.ToString()));
                }
                ddlService.Items.Insert(0, "--Select Service--");

                BindModels();
                BindModule();
                fillFactTable();
                //BindHelpCards();
            }
            BindHelpCards();

            foreach (GridViewDataColumn column in grid.Columns)
            {
                column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            }

            UpdateUI();

            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                if (Request.Form["__CALLBACKPARAM"].Contains("GetFilterDetails"))
                {

                    Guid filterID = GetFilterID();


                    if (filterID != Guid.Empty && commonViewObj != null && commonViewObj.GlobalFilers.Count > 0)
                    {
                        DashboardFilterProperty filter = commonViewObj.GlobalFilers.FirstOrDefault(x => x.ID == filterID);
                        txtGFTitle.Text = filter.Title;
                        txtItemOrder.Text = filter.ItemOrder.ToString();
                        ListEditItem leItem = ddlFactTable.Items.FindByValue(filter.ListName);
                        ddlFactTable.SelectedItem = leItem != null ? leItem : null;

                        FillFactTableFields(filter.ListName, null);
                        leItem = ddlFilterField.Items.FindByValue(filter.ColumnName);
                        ddlFilterField.Text = leItem != null ? leItem.Text : string.Empty;
                    }

                }
                else if (Request.Form["__CALLBACKPARAM"].Contains("SaveDashboards") || Request.Form["__CALLBACKPARAM"].Contains("QueryParamenter"))
                {
                    UpdateDashboards();
                }
            }

            if (!IsPostBack)
            {
                CommonDashboardsView commonDashboardsView = View.ViewProperty as CommonDashboardsView;
                if (commonDashboardsView != null)
                {

                    rblLayoutType.Value = commonDashboardsView.LayoutType.ToString();
                    txtRPadding.Text = Convert.ToString(commonDashboardsView.PaddingRight);
                    txtLPadding.Text = Convert.ToString(commonDashboardsView.PaddingLeft);
                    txtTPadding.Text = Convert.ToString(commonDashboardsView.PaddingTop);
                    txtBPadding.Text = Convert.ToString(commonDashboardsView.PaddingBottom);
                    txtViewHeight.Text = Convert.ToString(commonDashboardsView.ViewHeight);
                    txtViewWidth.Text = Convert.ToString(commonDashboardsView.ViewWidth);
                    txtBorderWidth.Text = Convert.ToString(commonDashboardsView.BorderWidth);
                    ceBorderColor.Color = !string.IsNullOrEmpty(commonDashboardsView.BorderColor) ? ColorTranslator.FromHtml(string.Format("#{0}", commonDashboardsView.BorderColor)) : ColorTranslator.FromHtml("#FFFFFF");
                    ceViewBGColor.Color = !string.IsNullOrEmpty(commonDashboardsView.ViewBackgroundColor) ? ColorTranslator.FromHtml(string.Format("#{0}", commonDashboardsView.ViewBackgroundColor)) : ColorTranslator.FromHtml("#FFFFFF");
                    UpdateTabControlValues(commonDashboardsView);
                    chkbxThemable.Checked = commonDashboardsView.IsThemable;

                    if (chkbxThemable.Checked)
                        ceViewBGColor.ClientVisible = false;
                    else
                    {
                        ceViewBGColor.ClientVisible = true;
                        if (!string.IsNullOrEmpty(commonDashboardsView.ViewBackgroundColor) && !commonDashboardsView.ViewBackgroundColor.Contains("#"))
                            ceViewBGColor.Color = !string.IsNullOrEmpty(commonDashboardsView.ViewBackgroundColor) ? ColorTranslator.FromHtml(string.Format("#{0}", commonDashboardsView.ViewBackgroundColor)) : ColorTranslator.FromHtml("#FFFFFF");
                        else
                            ceViewBGColor.Color = !string.IsNullOrEmpty(commonDashboardsView.ViewBackgroundColor) ? ColorTranslator.FromHtml(commonDashboardsView.ViewBackgroundColor) : ColorTranslator.FromHtml("#FFFFFF");
                    }
                    //  ceViewBGColor.Enabled = false;
                }
            }

            if (rblLayoutType.Value.ToString() == DashboardLayoutType.FixedSize.ToString())
            {
                divContainer.Style.Add("width", string.Format("{0}px", UGITUtility.StringToInt(txtViewWidth.Text)));
                divContainer.Style.Add("height", string.Format("{0}px", UGITUtility.StringToInt(txtViewHeight.Text)));
            }
            else
            {
                divContainer.Style.Remove("width");
                divContainer.Style.Remove("height");
            }
        }

        protected void UpdateTabControlValues(CommonDashboardsView view)
        {
            foreach (DashboardPanelProperty dpProperty in view.Dashboards)
            {
                if(dpProperty.DashboardSubType== "UserWelcomePanel")
                {
                    chkprojectcount.Checked = dpProperty.ShowProjectCountOnWelcomeScreen;
                }
                if(dpProperty.DashboardSubType== "AllocationTimeline")
                {
                    chkHideResouceAllocationFilter.Checked = dpProperty.HideResourceAllocationFilter;
                    chkShowCurrentUserDetailsOnly.Checked = dpProperty.ShowCurrentUserDetailsOnly;
                    chkHideAllocationType.Checked = dpProperty.HideAllocationType;
                }
                if (dpProperty.DashboardSubType == "AllocationConflicts")
                {
                    chkShowByUsersDivisionAC.Checked = dpProperty.ShowByUsersDivision;
                }
                if (dpProperty.DashboardSubType == "UnfilledProjectAllocations")
                {
                    chkShowByUsersDivisionPrj.Checked = dpProperty.ShowByUsersDivision;
                }
                if (dpProperty.DashboardSubType == "UnfilledPipelineAllocations")
                {
                    chkShowByUsersDivisionPpl.Checked = dpProperty.ShowByUsersDivision;
                }
                if (dpProperty.DashboardSubType == "LeftTicketCountBar")
                {
                    chkShowTabDetails.Checked = UGITUtility.StringToBoolean(dpProperty.ShowKPIDetail);
                    foreach (KPIDetail kpi in dpProperty.KPIList)
                    {
                        if (kpi.KpiName == "myproject")
                        {
                            myproject.Checked = true;
                            txtmyproject.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbmyproject.SelectedIndex = cmbmyproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkmyprojecticon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "myopenopportunities")
                        {
                            myopenopportunities.Checked = true;
                            txtmyopenopportunities.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbmyopenopportunities.SelectedIndex = cmbmyopenopportunities.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkmyopenopportunitiesicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "allopenproject")
                        {
                            allopenproject.Checked = true;
                            txtallopenproject.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmballopenproject.SelectedIndex = cmballopenproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkallopenprojecticon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }  
                        if (kpi.KpiName == "allcloseproject")
                        {
                            allcloseproject.Checked = true;
                            txtallcloseproject.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmballcloseproject.SelectedIndex = cmballopenproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkallcloseprojecticon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "futureopencpr")
                        {
                            futureopencpr.Checked = true;
                            txtfutureopencpr.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbfutureopencpr.SelectedIndex = cmballopenproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkfutureopencpr.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "currentopencpr")
                        {
                            currentopencpr.Checked = true;
                            txtcurrentopencpr.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbcurretopencpr.SelectedIndex = cmballopenproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkcurrentopencpr.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "totalresource")
                        {
                            totalresource.Checked = true;
                            txttotalresource.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbtotalresource.SelectedIndex = cmballopenproject.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chktotalresource.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "allopenopportunities")
                        {
                            allopenopportunities.Checked = true;
                            txtallopenopportunities.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmballopenopportunities.SelectedIndex = cmballopenopportunities.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkallopenopportunitiesicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "allopenservices")
                        {
                            allopenservices.Checked = true;
                            txtallopenservices.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmballopenservices.SelectedIndex = cmballopenservices.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkallopenservicesicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "recentwonopportunity")
                        {
                            recentwonopportunity.Checked = true;
                            txtrecentwonopportunity.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbrecentwonopportunity.SelectedIndex = cmbrecentwonopportunity.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkrecentwonopportunityicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if (kpi.KpiName == "recentlostopportunity")
                        {
                            recentlostopportunity.Checked = true;
                            txtrecentlostopportunity.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbrecentlostopportunity.SelectedIndex = cmbrecentlostopportunity.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkrecentlostopportunityicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if(kpi.KpiName == "waitingonme")
                        {
                            waitingonme.Checked = true;
                            txtwaitingonme.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbwaitingonme.SelectedIndex = cmbwaitingonme.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkwaitingonmeicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if(kpi.KpiName == "openticketstoday")
                        {
                            openticketstoday.Checked = true;
                            txtopenticketstoday.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbopenticketstoday.SelectedIndex = cmbopenticketstoday.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkopenticketstodayicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if(kpi.KpiName == "closedticketstoday")
                        {
                            closeticketstoday.Checked = true;
                            txtcloseticketstoday.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbcloseticketstoday.SelectedIndex = cmbcloseticketstoday.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkcloseticketstodayicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if(kpi.KpiName == "nprtickets")
                        {
                            nprtickets.Checked = true;
                            txtnprtickets.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbnprtickets.SelectedIndex = cmbnprtickets.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chknprticketsicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                        if(kpi.KpiName == "resolvedtickets")
                        {
                            resolvedtickets.Checked = true;
                            txtresolvedtickets.Text = UGITUtility.ObjectToString(kpi.KpiDisplayName);
                            cmbresolvedtickets.SelectedIndex = cmbnprtickets.Items.IndexOfText(UGITUtility.ObjectToString(kpi.Order));
                            chkresolvedticketsicon.Checked = UGITUtility.StringToBoolean(kpi.HideIcon);
                        }
                    }
                }
            }
        }

        protected void ddlDashbaord_Callback(object source, CallbackEventArgsBase e)
        {
            FillDashbaordCombo(e.Parameter);
        }

        protected void FillDashbaordCombo(string model)
        {
            ddlDashbaord.Items.Clear();
            if (!string.IsNullOrEmpty(model))
            {
                DataTable dashboards = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AnalyticDashboards, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                if (dashboards != null)
                {
                    DataView view = dashboards.DefaultView;
                    view.RowFilter = string.Format("{0}={1}", DatabaseObjects.Columns.AnalyticVID, model);
                    dashboards = view.ToTable();
                    if (dashboards != null && dashboards.Rows.Count > 0)
                    {
                        // ddlDashbaord.AppendDataBoundItems = false;
                        ddlDashbaord.DataSource = dashboards;
                        ddlDashbaord.TextField = DatabaseObjects.Columns.Title;
                        ddlDashbaord.ValueField = DatabaseObjects.Columns.DashboardID;
                        ddlDashbaord.DataBind();
                        ddlDashbaord.Items.Insert(0, new ListEditItem("--Select Dasboard--", ""));
                    }
                }
            }
        }

        private void BindModels()
        {

            DataTable dashboards = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AnalyticDashboards, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            DataTable resultTable = null;
            if (dashboards != null && dashboards.Rows.Count > 0)
            {
                resultTable = dashboards.DefaultView.ToTable(true, DatabaseObjects.Columns.AnalyticVID, DatabaseObjects.Columns.AnalyticName);
            }

            if (resultTable != null)
            {
                ddlModel.DataSource = resultTable;
                ddlModel.TextField = DatabaseObjects.Columns.AnalyticName;
                ddlModel.ValueField = DatabaseObjects.Columns.AnalyticVID;
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListEditItem("--Select Analytic--", ""));
            }
        }

        private void BindModule()
        {
            ddlModule.Items.Clear();

            /*
            DataTable spModuleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules);
            if (spModuleList.Rows.Count > 0)
            {
                DataTable dtModule = spModuleList;
                dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule });
                DataRow[] moduleRows = dtModule.Select(string.Format("{0}='True'", DatabaseObjects.Columns.EnableModule));

                foreach (DataRow moduleRow in moduleRows)
                {
                    string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
                    ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleName });

                }
                ddlModule.DataBind();
            }
            */

            DataTable spModuleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.EnableModule} = 'True'");
            if (spModuleList.Rows.Count > 0)
            {
                DataTable dtModule = spModuleList;
                dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule });
                                                
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataSource = dtModule;
                ddlModule.DataBind();
            }
        }

        private void BindHelpCards()
        {
            List<HelpCard> helpCards = helpCardManager.Load(x => x.Deleted == false).ToList();
            if (helpCards != null && helpCards.Count > 0)
            {
                gvHelpCards.DataSource = helpCards;
                gvHelpCards.DataBind();
            }
        }

        private void UpdateUI()
        {
            string html = string.Empty;

            foreach (DashboardPanelProperty dpProperty in lstDBPanProp)
            {
                int width = dpProperty.Width > 0 ? dpProperty.Width : defaultWidth;
                int height = dpProperty.Height > 0 ? dpProperty.Height : defaultHeight;
                int top = dpProperty.Top > 0 ? dpProperty.Top + topOffset : defaultTop;
                int left = dpProperty.Left > 0 ? dpProperty.Left : defaultLeft;
                string widthUnitType = dpProperty.WidthUnitType == UnitType.Percentage ? "%" : "px";
                string leftUnitType = dpProperty.LeftUnitType == UnitType.Percentage ? "%" : "px";
                int iconWidth = dpProperty.iconWidth > 0 ? dpProperty.iconWidth : 80;
                int iconHeight = dpProperty.iconHeight > 0 ? dpProperty.iconHeight : 30;
                int iconTop = dpProperty.iconTop > 0 ? dpProperty.iconTop : 20;
                int iconLeft = dpProperty.iconLeft > 0 ? dpProperty.iconLeft : 20;
                int panelLeft = dpProperty.PanelLeft > 0 ? dpProperty.PanelLeft : 0;
                int panelTop = dpProperty.PanelTop > 0 ? dpProperty.PanelTop : 0;
                int titleLeft = dpProperty.TitleLeft;
                int titleTop = dpProperty.TitleTop;

                int panelZindex = dpProperty.Zindex;

                dpProperty.LinkDetails = dpProperty.LinkDetails == null ? string.Empty : dpProperty.LinkDetails;
                Dashboard dashboard = dManager.LoadDashboardsByNames(new List<string>() { dpProperty.DashboardName }).FirstOrDefault();

                if (dashboard != null)
                {
                    if (!string.IsNullOrEmpty(dashboard.HeaderFontStyle) && string.IsNullOrEmpty(dpProperty.HeaderFontStyle))
                    {
                        dpProperty.HeaderFontStyle = dashboard.HeaderFontStyle;
                    }

                    if (!string.IsNullOrEmpty(dashboard.FontStyle) && string.IsNullOrEmpty(dpProperty.FontStyle))
                    {
                        dpProperty.FontStyle = dashboard.FontStyle;
                    }
                }

                string dashboardSubType = string.IsNullOrEmpty(dpProperty.DashboardSubType) ? string.Empty : dpProperty.DashboardSubType;
                string panelType = dpProperty.DashboardType;// dashboard != null ? dashboard.PanelType.ToString() : string.Empty;
                bool queryParameter = (dashboard != null && dashboard.DashboardType == DashboardType.Query) ? true : false;
                string fontStyle = string.IsNullOrEmpty(dpProperty.FontStyle) ? string.Empty : dpProperty.FontStyle;
                string headerfontStyle = string.IsNullOrEmpty(dpProperty.HeaderFontStyle) ? string.Empty : dpProperty.HeaderFontStyle;
                bool hideTitle = Convert.ToBoolean(dpProperty.IsHideTitle);

                bool hideSLATabular = Convert.ToBoolean(dpProperty.HideSLATabular);

                string module = string.IsNullOrEmpty(dpProperty.Module) ? string.Empty : dpProperty.Module;
                string status = string.IsNullOrEmpty(dpProperty.Status) ? string.Empty : dpProperty.Status;
                bool enablefilter = Convert.ToBoolean(dpProperty.EnableFilter);
                bool enablepredictbacklog = Convert.ToBoolean(dpProperty.EnableFilterPredictBacklog);
                bool enableticketcreatedbyweek = Convert.ToBoolean(dpProperty.EnableFilterTicketCreatedByWeek);
                bool enablefilterscorecard = Convert.ToBoolean(dpProperty.EnableFilterScoreCard);
                string weeklyaverage = Convert.ToString(dpProperty.WeeklyAverage);
                DateTime scorecardstartdate = Convert.ToDateTime(dpProperty.ScoreCardStartDate);
                DateTime scorecardEnddate = Convert.ToDateTime(dpProperty.ScoreCardEndDate);
                bool enablefilterticketflow = Convert.ToBoolean(dpProperty.EnableFilterTicketFlow);
                DateTime ticketflowstartdate = Convert.ToDateTime(dpProperty.TicketFlowStartDate);
                DateTime ticketflowenddate = Convert.ToDateTime(dpProperty.TicketFlowEndDate);
                int pSize = dpProperty.PageSize;
                string category = Convert.ToString(dpProperty.Category);
                CategoryPro = category;
                if (!IsPostBack)
                    LoadRequestTypeTree(module);
                else
                    LoadRequestTypeTree(ddlModule.SelectedValue);

                string borderColor = string.IsNullOrEmpty(dpProperty.BorderColor) ? string.Empty : dpProperty.BorderColor;


                html += string.Format("<div id='{0}' iconWidth={6} iconHeight={7} iconTop={8} iconLeft={9} linkType='{10}' leftPercentage='{23}' widthPercentage='{22}'  leftUnitType='{21}' widthUnitType='{19}' linkDetails='{11}' navigationtype='{17}' Theme='" + dpProperty.Theme + "' queryParameter='" + queryParameter + "' IconUrl='" + dpProperty.IconUrl + "' IconShape='" + dpProperty.IconShape + "' IsLink='" + dpProperty.IsLink + "' LinkUrl='" + dpProperty.LinkUrl + "' paneltype='" + panelType + "' panelLeft= " + panelLeft + " panelTop=" + panelTop + "  background='" + Uri.EscapeDataString(dpProperty.BackGroundUrl) + "' fontstyle='" + fontStyle + "' headerfontStyle='" + headerfontStyle + "' titleTop= " + titleTop + " titleLeft= " + titleLeft + " borderColor='" + borderColor + "' dashboardSubType = '" + dashboardSubType + "' module = '" + module + "' hideSLATabular = '" + hideSLATabular + "' iscategory = '" + category + "' status = '" + status + "' enablefilter = '" + enablefilter + "' enablefilterforpredictbacklog = '" + enablepredictbacklog + "' enablefilterforticketcreatedbyweek = '" + enableticketcreatedbyweek + "' enablefilterscorecard = '" + enablefilterscorecard + "' weeklyaverage = '" + weeklyaverage + "' scorecardstartdate = '" + scorecardstartdate + "' scorecardenddate = '" + scorecardEnddate + "' enablefilterticketflow = '" + enablefilterticketflow + "' ticketflowstartdate = '" + ticketflowstartdate + "' ticketflowenddate = '" + ticketflowenddate + "' pSize = " + pSize + " hideTitle='" + hideTitle + "' priority = '{12}' duedate='{13}' Zindex='" + panelZindex + "'  iscritical='{14}' userId='{15}' style='z-index:{16};border:1px solid;width: {1}{18};height: {2}px;background-color:#fff;text-align: center;position:absolute;left:{4}{20};top:{3}px;' class='dragresize'><img style='float: right;padding-right: 2px;padding-top: 2px;' onclick='CloseDashboard(this);' src='/content/images/delete.png'/> <img style='float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px' onclick='ShowEditPanel(this);' src='/content/Images/edit-icon.png'/><img style='float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px' onclick='Sendtoback(this);' src='/content/images/backward-16.png'  title='Sent to Back'/> <img style='float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px' onclick='Bringtofront(this);' src='/content/images/forward-16.png' title='Bring to front'/><img style='float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px' onclick='ShowClonePanel(this);' src='/content/images/duplicate.png' title='Duplicate'/><br/><b><p>{5}</p></b></div>"
                    , dpProperty.DashboardName, width, height, top, left, dpProperty.DisplayName, iconWidth, iconHeight, iconTop, iconLeft, dpProperty.LinkType, Uri.EscapeDataString(dpProperty.LinkDetails), string.IsNullOrEmpty(dpProperty.Priority) ? "high" : dpProperty.Priority,
                    string.IsNullOrEmpty(dpProperty.DueDate) ? "both" : dpProperty.DueDate,
                    string.IsNullOrEmpty(dpProperty.IsCritical.ToString()) ? "false" : dpProperty.IsCritical.ToString(),
                   dpProperty.UserID, dpProperty.Zindex, dpProperty.NavigationType, widthUnitType, widthUnitType, leftUnitType, leftUnitType, width, left


              );

                if (!IsPostBack && !string.IsNullOrEmpty(dpProperty.HelpCards))
                {
                    gvHelpCards.Text = dpProperty.HelpCards;
                }
                if (!IsPostBack && !string.IsNullOrEmpty(dpProperty.WelcomeMessage))
                {
                    txtWelcomemsg.Text = dpProperty.WelcomeMessage;
                }
                if (!IsPostBack)
                {
                    chkprojectcount.Checked = dpProperty.ShowProjectCountOnWelcomeScreen;
                    chkHideResouceAllocationFilter.Checked = dpProperty.HideResourceAllocationFilter;
                }
            }


            divContainer.InnerHtml = html;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = listofDB;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {

            UpdateDashboards();
            Dictionary<string, object> formdict = uHelper.ReportScheduleDict;
            DashboardPanelProperty dpProperty = lstDBPanProp.FirstOrDefault(x => x.DashboardName == Convert.ToString(hndDashboardName.Value.Trim()));
            dpProperty.QueryParameter = (formdict != null && formdict.ContainsKey(ReportScheduleConstant.Where)) ? Convert.ToString(formdict[ReportScheduleConstant.Where]) : string.Empty;
            dpProperty.DisplayName = txtTitle.Text;
            dpProperty.Width = UGITUtility.StringToInt(txtWidth.Text);
            dpProperty.Height = UGITUtility.StringToInt(txtHeight.Text);
            dpProperty.WidthUnitType = UnitType.Pixel;
            if (ddlWidthUnit.SelectedItem.Value.Equals("Percentage"))
            {
                dpProperty.WidthUnitType = UnitType.Percentage;
            }
            dpProperty.LeftUnitType = UnitType.Pixel;
            if (ddlLeftUnit.SelectedItem.Value.Equals("Percentage"))
            {
                dpProperty.LeftUnitType = UnitType.Percentage;
            }
            dpProperty.Top = GetAdjustedTop(UGITUtility.StringToInt(txtTop.Text));
            dpProperty.Left = UGITUtility.StringToInt(txtLeft.Text);
            dpProperty.Zindex = UGITUtility.StringToInt(txtzindex.Text);
            dpProperty.Theme = ddlBorder.SelectedValue;
            dpProperty.DashboardType = panalType.Value;
            dpProperty.NavigationType = 0;
            if (rbNewWindow.Checked)
                dpProperty.NavigationType = 1;
            else if (rbPopup.Checked)
                dpProperty.NavigationType = 2;
            dpProperty.IsHideTitle = chkHideTitle.Checked;

            //txtTitleTop
            //UDashboard uDashboard = listofDB.FirstOrDefault(x => x.Title == Convert.ToString(hndDashboardName.Value.Trim()) && x.PanelType != DashboardType.Link);
            if (dpProperty.DashboardType == DashboardType.Panel.ToString() || UGITUtility.StringToInt(hndIsLink.Value) > 0)
            {
                dpProperty.FontStyle = string.Format("{0};#{1};#{2};#{3}", ddlFontStyle.SelectedValue, ddlFontSize.SelectedValue, string.Format("#{0}", ceFont.Color.Name.Substring(2)), ddlFontName.SelectedItem.Text.Trim());
                dpProperty.HeaderFontStyle = string.Format("{0};#{1};#{2};#{3}", ddlHeaderFontStyle.SelectedValue, ddlHeaderFontSize.SelectedValue, ceHeaderFont.Color.Name.Length > 1 ? string.Format("#{0}", ceHeaderFont.Color.Name.Substring(2)) : string.Empty, ddlHeaderFontName.SelectedItem.Text.Trim());
                dpProperty.BorderColor = ceBorder.Color.Name.Length > 1 ? string.Format("#{0}", ceBorder.Color.Name.Substring(2)) : string.Empty;
                dpProperty.TitleTop = UGITUtility.StringToInt(txtTitleTop.Text);
                dpProperty.TitleLeft = UGITUtility.StringToInt(txtTitleLeft.Text, 25);
            }
            else
            {
                dpProperty.FontStyle = string.Empty;
                dpProperty.HeaderFontStyle = string.Empty;
            }

            if (UGITUtility.StringToInt(hndIsLink.Value) > 0)
            {
                dpProperty.DashboardName = txtTitle.Text;
                string uploadFileURL = string.Empty;
                if (fileupload.HasFile)
                {
                    uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileupload.FileName);
                    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileupload.FileName);
                    fileupload.PostedFile.SaveAs(path);
                    txtHelp.Text = uploadFileURL;
                    dpProperty.LinkDetails = fileupload.FileName;
                }
                else if (ddlService.SelectedIndex > 0)
                {
                    txtHelp.Text = string.Format("{0}{1}", UGITUtility.GetAbsoluteURL("/SitePages/ServiceWizard.aspx?serviceID="), ddlService.SelectedValue);
                    dpProperty.NavigationType = 1;
                    dpProperty.LinkDetails = ddlService.SelectedValue;
                }
                else if (lnkType.SelectedValue == "Analytic")
                {
                   string analyticUrl = UGITUtility.GetAbsoluteURL(config.GetValue(ConfigConstants.AnalyticUrl));
                    string modelID = ddlModel.SelectedItem.Value.ToString();
                    string dashboardId = ddlDashbaord.SelectedItem.Value.ToString();
                    txtHelp.Text = string.Format("{0}/runs/QuickSurveyResult?modelID={1}&resultType=True&dashboardId={2}&relativeRunID=0&IsDlg=true&popupId=dashboardRun", analyticUrl, modelID, dashboardId);
                    dpProperty.NavigationType = 1;
                    dpProperty.LinkDetails = ddlDashbaord.Text;
                }
                else if (lnkType.SelectedValue == "Wiki")
                {
                    var values = HttpUtility.ParseQueryString(txtHelp.Text);
                    dpProperty.LinkDetails = Convert.ToString(values["ticketId"]);
                }
                dpProperty.LinkUrl = txtHelp.Text;
                dpProperty.TitleTop = UGITUtility.StringToInt(txtTitleTop.Text);
                dpProperty.TitleLeft = UGITUtility.StringToInt(txtTitleLeft.Text, 25);

                dpProperty.IsLink = true;

            }
            else
            {


                dpProperty.IsLink = false;
                dpProperty.LinkUrl = string.Empty;
            }
            dpProperty.PanelLeft = UGITUtility.StringToInt(txtPanelLeft.Text);
            dpProperty.PanelTop = UGITUtility.StringToInt(txtPanelTop.Text, 20);

            dpProperty.iconLeft = UGITUtility.StringToInt(txtIconLeft.Text, 80);
            dpProperty.iconTop = UGITUtility.StringToInt(txtIconTop.Text, 40);
            dpProperty.iconWidth = UGITUtility.StringToInt(txtIconWidth.Text, 40);
            dpProperty.iconHeight = UGITUtility.StringToInt(txtIconHeight.Text, 40);

            dpProperty.LinkType = lnkType.SelectedValue;

            string iconUrl = string.Empty;
            //if (fileuploadIcon.HasFile)
            //{
            //    iconUrl = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileuploadIcon.FileName.Trim().Replace(" ", "_"));
            //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileuploadIcon.FileName.Trim().Replace(" ", "_"));
            //    fileuploadIcon.PostedFile.SaveAs(path);
            //    txtIconImage.Text = iconUrl;
            //}

            dpProperty.IconUrl = fileUploadIcon.GetImageUrl();
            dpProperty.IconShape = ddlIconShape.SelectedValue;

            //string backgroundUrl = string.Empty;
            //if (fileuploadBackGround.HasFile)
            //{
            //    backgroundUrl = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileuploadBackGround.FileName.Trim().Replace(" ", "_"));
            //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileuploadBackGround.FileName.Trim().Replace(" ", "_"));
            //    fileuploadBackGround.PostedFile.SaveAs(path);
            //    txtBackgroundImage.Text = backgroundUrl;
            //}

            if (!string.IsNullOrEmpty(txtBackgroundImage.Text))
            {
                dpProperty.BackGroundUrl = string.Format("Url;#{0}", txtBackgroundImage.Text);
            }
            else if (dpProperty.DashboardType == DashboardType.Panel.ToString() || UGITUtility.StringToInt(hndIsLink.Value) > 0)
            {
                dpProperty.BackGroundUrl = string.Format("BackgroundColor;#{0}", ceBackGround.Color.Name.Length > 1 ? string.Format("#{0}", ceBackGround.Color.Name.Substring(2)) : string.Empty);
            }
            else
            {
                dpProperty.BackGroundUrl = string.Empty;
            }

            if (panalType.Value == "Control")
            {
                dpProperty.DashboardName = txtTitle.Text;
                if (dpProperty.DashboardSubType != "MessageBoard")
                {
                    int size = 0;
                    int.TryParse(txtPageSize.Text, out size);

                    dpProperty.PageSize = size == 0 ? 10 : size;
                    dpProperty.Module = ddlModule.SelectedValue;
                    dpProperty.HideSLATabular = chkbxHideTabular.Checked;
                    dpProperty.Priority = ddlPriority.SelectedItem.Text.ToLower();
                    dpProperty.DueDate = ddlDueDate.SelectedValue;
                    dpProperty.IsCritical = chkIsCritical.Checked;
                    dpProperty.UserID = -1;
                    dpProperty.Status = ddlStatus.SelectedValue;
                    dpProperty.EnableFilter = chkEnableFilter.Checked;
                    dpProperty.EnableFilterPredictBacklog = chkPredictBacklog.Checked;
                    dpProperty.EnableFilterTicketCreatedByWeek = chkTicketCreatedByWeek.Checked;
                    dpProperty.EnableFilterScoreCard = chkScoreCard.Checked;
                    dpProperty.WeeklyAverage = txtWeeklyAverage.Text.Trim();
                    dpProperty.ScoreCardStartDate = deStartDateScoreCard.Date;
                    dpProperty.ScoreCardEndDate = deEndDateScoreCard.Date;
                    dpProperty.EnableFilterTicketFlow = chkTicketFlow.Checked;
                    dpProperty.TicketFlowStartDate = deStartDateTicketFlow.Date;
                    dpProperty.TicketFlowEndDate = deEndDateTicketFlow.Date;

                    List<TreeListNode> lstnode = requestTypeTreeList.GetSelectedNodes();
                    dpProperty.Category = string.Empty;
                    if (lstnode.Count != requestTypeTreeList.GetAllNodes().Count)
                        dpProperty.Category = string.Join(",", lstnode.Select(x => x.Key));

                    if (rdoUser.SelectedValue == "all")
                    {
                        dpProperty.UserID = 0;
                    }
                    else if (rdoUser.SelectedValue == "specific")
                    {
                        dpProperty.UserID = UGITUtility.StringToInt(cmbUser.Value);
                    }
                }
                if(dpProperty.DashboardSubType == "LeftTicketCountBar")
                {
                    TabViewManager tabManagerObj = new TabViewManager(context);
                    List<KPIDetail> lstkpidetails = new List<KPIDetail>();
                    if (myproject.Checked)
                    {
                        KPIDetail kpimyproject = new KPIDetail()
                        {
                            KpiName = "myproject",
                            KpiDisplayName = UGITUtility.ObjectToString(txtmyproject.Text),
                            Order = UGITUtility.StringToInt(cmbmyproject.Text),
                            HideIcon = chkmyprojecticon.Checked
                        };
                        lstkpidetails.Add(kpimyproject);
                    }
                    if (myopenopportunities.Checked)
                    {
                        KPIDetail kpimyopportunity = new KPIDetail() {
                            KpiName = "myopenopportunities",
                            KpiDisplayName = UGITUtility.ObjectToString(txtmyopenopportunities.Text),
                            Order = UGITUtility.StringToInt(cmbmyopenopportunities.Text),
                            HideIcon = chkmyopenopportunitiesicon.Checked
                        };
                        lstkpidetails.Add(kpimyopportunity);
                    }
                    if (allopenproject.Checked)
                    {
                        KPIDetail kpiallopenproject = new KPIDetail()
                        {
                            KpiName = "allopenproject",
                            KpiDisplayName = UGITUtility.ObjectToString(txtallopenproject.Text),
                            Order = UGITUtility.StringToInt(cmballopenproject.Text),
                            HideIcon = chkallopenprojecticon.Checked
                        };
                        lstkpidetails.Add(kpiallopenproject);
                    }
                    if (allcloseproject.Checked)
                    {
                        KPIDetail kpiallcloseproject = new KPIDetail()
                        {
                            KpiName = "allcloseproject",
                            KpiDisplayName = UGITUtility.ObjectToString(txtallcloseproject.Text),
                            Order = UGITUtility.StringToInt(cmballcloseproject.Text),
                            HideIcon = chkallcloseprojecticon.Checked
                        };
                        lstkpidetails.Add(kpiallcloseproject);
                    }
                    if (currentopencpr.Checked)
                    {
                        KPIDetail kpicurrentopencpr = new KPIDetail()
                        {
                            KpiName = "currentopencpr",
                            KpiDisplayName = UGITUtility.ObjectToString(txtcurrentopencpr.Text),
                            Order = UGITUtility.StringToInt(cmbcurretopencpr.Text),
                            HideIcon = chkcurrentopencpr.Checked
                        };
                        lstkpidetails.Add(kpicurrentopencpr);
                    }
                    if (futureopencpr.Checked)
                    {
                        KPIDetail kpicfutureopencpr = new KPIDetail()
                        {
                            KpiName = "futureopencpr",
                            KpiDisplayName = UGITUtility.ObjectToString(txtfutureopencpr.Text),
                            Order = UGITUtility.StringToInt(cmbfutureopencpr.Text),
                            HideIcon = chkfutureopencpr.Checked
                        };
                        lstkpidetails.Add(kpicfutureopencpr);
                    }
                    if (totalresource.Checked)
                    {
                        KPIDetail kpitotalresource = new KPIDetail()
                        {
                            KpiName = "totalresource",
                            KpiDisplayName = UGITUtility.ObjectToString(txttotalresource.Text),
                            Order = UGITUtility.StringToInt(cmbtotalresource.Text),
                            HideIcon = chktotalresource.Checked
                        };
                        lstkpidetails.Add(kpitotalresource);
                    }
                    if (allopenopportunities.Checked)
                    {
                        KPIDetail kpiallopenopportunities = new KPIDetail()
                        {
                            KpiName = "allopenopportunities",
                            KpiDisplayName = UGITUtility.ObjectToString(txtallopenopportunities.Text),
                            Order = UGITUtility.StringToInt(cmballopenopportunities.Text),
                            HideIcon = chkallopenopportunitiesicon.Checked
                        };
                        lstkpidetails.Add(kpiallopenopportunities);
                    }
                    if (allopenservices.Checked)
                    {
                        KPIDetail kpiallopenservices = new KPIDetail()
                        {
                            KpiName = "allopenservices",
                            KpiDisplayName = UGITUtility.ObjectToString(txtallopenservices.Text),
                            Order = UGITUtility.StringToInt(cmballopenservices.Text),
                            HideIcon = chkallopenservicesicon.Checked
                        };
                        lstkpidetails.Add(kpiallopenservices);
                    }
                    if (recentwonopportunity.Checked)
                    {
                        KPIDetail kpirecentwonopportunity = new KPIDetail() { KpiName = "recentwonopportunity", KpiDisplayName = UGITUtility.ObjectToString(txtrecentwonopportunity.Text), 
                            Order = UGITUtility.StringToInt(cmbrecentwonopportunity.Text), HideIcon=chkrecentwonopportunityicon.Checked };
                        lstkpidetails.Add(kpirecentwonopportunity);
                    }
                    if (recentlostopportunity.Checked)
                    {
                        KPIDetail kpirecentlostopportunity = new KPIDetail() { KpiName = "recentlostopportunity", KpiDisplayName = UGITUtility.ObjectToString(txtrecentlostopportunity.Text),
                            Order = UGITUtility.StringToInt(cmbrecentlostopportunity.Text), HideIcon=chkrecentlostopportunityicon.Checked };
                        lstkpidetails.Add(kpirecentlostopportunity);
                    }
                    if (waitingonme.Checked)
                    {
                        KPIDetail kpiwaitingonme = new KPIDetail() { KpiName = "waitingonme", KpiDisplayName = UGITUtility.ObjectToString(txtwaitingonme.Text), 
                            Order = UGITUtility.StringToInt(cmbwaitingonme.Text), HideIcon=chkwaitingonmeicon.Checked };
                        lstkpidetails.Add(kpiwaitingonme);
                    }
                    if (openticketstoday.Checked)
                    {
                        KPIDetail kpiopenticketstoday = new KPIDetail()
                        {
                            KpiName = "openticketstoday",
                            KpiDisplayName = UGITUtility.ObjectToString(txtopenticketstoday.Text),
                            Order = UGITUtility.StringToInt(cmbopenticketstoday.Text),
                            HideIcon = chkopenticketstodayicon.Checked
                        };
                        lstkpidetails.Add(kpiopenticketstoday);
                    }
                    if (closeticketstoday.Checked)
                    {
                        KPIDetail kpicloseticketstoday = new KPIDetail()
                        {
                            KpiName = "closeticketstoday",
                            KpiDisplayName = UGITUtility.ObjectToString(txtcloseticketstoday.Text),
                            Order = UGITUtility.StringToInt(cmbcloseticketstoday.Text),
                            HideIcon = chkcloseticketstodayicon.Checked
                        };
                        lstkpidetails.Add(kpicloseticketstoday);
                    }
                    if (nprtickets.Checked)
                    {
                        KPIDetail kpinprtickets = new KPIDetail()
                        {
                            KpiName = "nprtickets",
                            KpiDisplayName = UGITUtility.ObjectToString(txtnprtickets.Text),
                            Order = UGITUtility.StringToInt(cmbnprtickets.Text),
                            HideIcon = chknprticketsicon.Checked
                        };
                        lstkpidetails.Add(kpinprtickets);
                    }
                    if (resolvedtickets.Checked)
                    {
                        KPIDetail kpiresolvedtickets = new KPIDetail()
                        {
                            KpiName = "resolvedtickets",
                            KpiDisplayName = UGITUtility.ObjectToString(txtresolvedtickets.Text),
                            Order = UGITUtility.StringToInt(cmbresolvedtickets.Text),
                            HideIcon = chkresolvedticketsicon.Checked
                        };
                        lstkpidetails.Add(kpiresolvedtickets);
                    }
                    dpProperty.KPIList = lstkpidetails;
                    dpProperty.ShowKPIDetail = UGITUtility.StringToBoolean(chkShowTabDetails.Checked);
                }
                if(dpProperty.DashboardSubType == "UserWelcomePanel")
                {
                    dpProperty.WelcomeMessage = txtWelcomemsg.Text;
                    dpProperty.ShowProjectCountOnWelcomeScreen = chkprojectcount.Checked;
                }
                if(dpProperty.DashboardSubType == "HelpCardsPanel")
                {
                    dpProperty.HelpCards = gvHelpCards.Text;
                }
                if (dpProperty.DashboardSubType == "AllocationTimeline")
                {
                    dpProperty.HideResourceAllocationFilter = chkHideResouceAllocationFilter.Checked;
                    dpProperty.ShowCurrentUserDetailsOnly = chkShowCurrentUserDetailsOnly.Checked;
                    dpProperty.HideAllocationType = chkHideAllocationType.Checked;
                }
                if (dpProperty.DashboardSubType == "AllocationConflicts")
                {
                    dpProperty.ShowByUsersDivision = chkShowByUsersDivisionAC.Checked;
                }
                if (dpProperty.DashboardSubType == "UnfilledProjectAllocations")
                {
                    dpProperty.ShowByUsersDivision = chkShowByUsersDivisionPrj.Checked;
                }
                if (dpProperty.DashboardSubType == "UnfilledPipelineAllocations")
                {
                    dpProperty.ShowByUsersDivision = chkShowByUsersDivisionPpl.Checked;
                }
            }

            CommonDashboardsView commonViewObj = View.ViewProperty == null ? new CommonDashboardsView() : (View.ViewProperty as CommonDashboardsView);
            commonViewObj.Dashboards = lstDBPanProp;
            View.ViewProperty = commonViewObj;

            objDashboardPanelViewManager.Save(View);
            CacheHelper<DashboardPanelView>.ClearWithRegion(context.TenantID);
            UpdateUI();

        }

        protected int GetAdjustedTop(int top)
        {
            return top > topOffset ? top - topOffset : 0;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            UpdateDashboards();
            UpdateUI();
            CacheHelper<DashboardPanelView>.ClearWithRegion(context.TenantID);
        }

        protected void gridGlobalFilter_DataBinding(object sender, EventArgs e)
        {
            if (commonViewObj != null)
            {
                gridGlobalFilter.DataSource = commonViewObj.GlobalFilers;
            }
            else
                gridGlobalFilter.DataSource = null;
        }

        protected void aEdit_Load(object sender, EventArgs e)
        {
            HtmlAnchor aHtml = (HtmlAnchor)sender;
            string lsDataKeyValue = (aHtml.NamingContainer as GridViewDataItemTemplateContainer).KeyValue.ToString();
            aHtml.Attributes.Add("onclick", "ShowGlobalFilterEditPanel('" + lsDataKeyValue + "');");
        }

        private Guid GetFilterID()
        {
            Guid filterID = Guid.Empty;
            try
            {
                filterID = new Guid(hndGlobalFilterID.Value.Trim());
            }
            catch
            {
                filterID = Guid.Empty;
            }
            return filterID;
        }

        protected void btnSaveGlobalFilter_Click(object sender, EventArgs e)
        {
            CommonDashboardsView commonDashboardsView = View.ViewProperty as CommonDashboardsView;

            if (commonDashboardsView == null)
            {
                commonDashboardsView = new CommonDashboardsView();
                View.ViewProperty = commonDashboardsView;
            }

            Guid filterID = GetFilterID();


            List<DashboardFilterProperty> filters = commonDashboardsView.GlobalFilers;
            if (filters == null)
            {
                filters = new List<DashboardFilterProperty>();
            }

            DashboardFilterProperty filter = filters.FirstOrDefault(x => x.ID == filterID);
            if (filter == null)
            {
                filter = new DashboardFilterProperty();
                filters.Add(filter);
            }

            filter.Title = txtGFTitle.Text.Trim();
            int itemOrder = 0;
            int.TryParse(txtItemOrder.Text.Trim(), out itemOrder);
            filter.ItemOrder = itemOrder;
            filter.ListName = Convert.ToString(ddlFactTable.Value);
            filter.ColumnName = Convert.ToString(ddlFilterField.Value);

            commonDashboardsView.GlobalFilers = filters;
            objDashboardPanelViewManager.Save(View);

        }

        protected void ddlFilterField_Callback(object sender, CallbackEventArgsBase e)
        {
            FillFactTableFields(e.Parameter, null);
        }

        private void fillFactTable()
        {
            if (ddlFactTable.Items.Count <= 0)
            {
                List<string> factTables = DashboardCache.DashboardFactTables(HttpContext.Current.GetManagerContext());
                foreach (string fTable in factTables)
                {
                    ddlFactTable.Items.Add(new ListEditItem(fTable, fTable));
                }
            }
        }

        private void FillFactTableFields(string factTable, string typeFilter)
        {
            ddlFilterField.Items.Clear();

            if (factTable != null && factTable.Trim() != string.Empty)
            {
                List<FactTableField> factTableFields = DashboardCache.GetFactTableFields(context, factTable.Trim());
                if (!string.IsNullOrEmpty(typeFilter))
                {
                    factTableFields = factTableFields.Where(x => x.DataType.ToLower() == "system.datetime").ToList();

                }

                foreach (FactTableField field in factTableFields.OrderBy(x => x.FieldName))
                {
                    ListEditItem item = new ListEditItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);

                    ddlFilterField.Items.Add(item);
                }
            }
        }

        protected void btnDeleteGlobalFilter_Click(object sender, EventArgs e)
        {
            CommonDashboardsView commonDashboardsView = View.ViewProperty as CommonDashboardsView;
            Guid filterID = GetFilterID();


            List<DashboardFilterProperty> filters = commonDashboardsView.GlobalFilers;

            DashboardFilterProperty filter = filters.FirstOrDefault(x => x.ID == filterID);
            if (filter != null)
            {
                commonDashboardsView.GlobalFilers.Remove(filter);
                objDashboardPanelViewManager.Save(View);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            objDashboardPanelViewManager.Delete(View);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }


        protected void btnSaveLayout_Click(object sender, EventArgs e)
        {
            CommonDashboardsView commonDashboardsView = View.ViewProperty as CommonDashboardsView;

            if (commonDashboardsView == null)
            {
                commonDashboardsView = new CommonDashboardsView();
                View.ViewProperty = commonDashboardsView;
            }
            commonDashboardsView.LayoutType = (DashboardLayoutType)Enum.Parse(typeof(DashboardLayoutType), rblLayoutType.Value.ToString());
            if (commonDashboardsView.LayoutType == DashboardLayoutType.Autosize)
            {
                commonDashboardsView.PaddingRight = UGITUtility.StringToInt(txtRPadding.Text);
                commonDashboardsView.PaddingLeft = UGITUtility.StringToInt(txtLPadding.Text);
                commonDashboardsView.PaddingTop = UGITUtility.StringToInt(txtTPadding.Text);
                commonDashboardsView.PaddingBottom = UGITUtility.StringToInt(txtBPadding.Text);
                commonDashboardsView.ViewHeight = 0;
                commonDashboardsView.ViewWidth = 0;
            }
            else
            {
                commonDashboardsView.ViewHeight = UGITUtility.StringToInt(txtViewHeight.Text);
                commonDashboardsView.ViewWidth = UGITUtility.StringToInt(txtViewWidth.Text);
                commonDashboardsView.PaddingRight = 0;
                commonDashboardsView.PaddingLeft = 0;
                commonDashboardsView.PaddingTop = 0;
                commonDashboardsView.PaddingBottom = 0;

            }
            string viewBackgroundUrl = string.Empty;
            if (fuViewBackground.HasFile)
            {
                viewBackgroundUrl = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fuViewBackground.FileName.Trim().Replace(" ", "_"));
                string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fuViewBackground.FileName.Trim().Replace(" ", "_"));
                fuViewBackground.PostedFile.SaveAs(path);
                txtViewBackGround.Text = viewBackgroundUrl;
            }
            commonDashboardsView.ViewBackground = txtViewBackGround.Text;
            commonDashboardsView.IsThemable = chkbxThemable.Checked;
            commonDashboardsView.BorderColor = ceViewBGColor.Color.Name.Length > 1 ? string.Format("{0}", ceViewBGColor.Color.Name.Substring(2)) : string.Empty;
            double val = UGITUtility.StringToDouble(txtOpacity.Text, 1);
            if (val > 1)
            {
                val = 1;
            }
            else if (val < 0)
            {
                val = 0;
            }

            commonDashboardsView.Opacity = val;
            objDashboardPanelViewManager.Save(View);

        }


        protected void btnSaveAs_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            View.ViewName = txtSaveAsName.Text;
            //View.SaveAs(SPContext.Current.Web);
            objDashboardPanelViewManager.Save(View);
            pcSaveAs.ShowOnPageLoad = false;
            if (btn.ID == "btnSavenClose")
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void chkbxThemable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbxThemable.Checked)
                ceViewBGColor.Enabled = true;
            else
                ceViewBGColor.Enabled = false;
        }

        protected void requestTypeTreeList_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            if (e.Argument.Contains("byshowedit"))
            {
                string category = e.Argument.Split(':')[1];
                List<string> lstnode = new List<string>();
                lstnode = Convert.ToString(category).Split(',').ToList<string>();
                foreach (string node in lstnode)
                {
                    TreeListNode treenode = requestTypeTreeList.FindNodeByKeyValue(node);
                    if (treenode != null)
                    {
                        treenode.Selected = true;
                    }
                }
            }
            else
            {
                requestTypeTreeList.ClearNodes();
                requestTypeTreeList.UnselectAll();
                if (!string.IsNullOrEmpty(ddlModule.SelectedValue))
                {
                    LoadRequestTypeTree(ddlModule.SelectedValue);
                    if (!string.IsNullOrEmpty(CategoryPro))
                    {
                        List<string> lstnode = new List<string>();
                        lstnode = Convert.ToString(CategoryPro).Split(',').ToList<string>();
                        foreach (string node in lstnode)
                        {
                            TreeListNode treenode = requestTypeTreeList.FindNodeByKeyValue(node);
                            if (treenode != null)
                            {
                                treenode.Selected = true;
                            }
                        }
                    }

                }
            }
        }
        private void LoadRequestTypeTree(string moduleName)
        {
            DataTable data = GetRequestTypeData(moduleName);
            requestTypeTreeList.DataSource = data;
            requestTypeTreeList.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        private DataTable GetRequestTypeData(string moduleName)
        {
            DataTable table = new DataTable();

            table.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            table.Columns.Add(DatabaseObjects.Columns.ParentID, typeof(string));
            table.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            string query =  string.Format("{0}='{1}' and {2}!=1 and {3} is not null and {4} is not null", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.Deleted, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.SubCategory);

            DataTable spList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType,query);
            //SPQuery query = new SPQuery();
            // query.ViewFields = string.Format("<FieldRef Name='{0}' Nullable='True'/><FieldRef Name='{1}' Nullable='True'/>", DatabaseObjects.Columns.Category, DatabaseObjects.Columns.RequestTypeSubCategory);
            //query.ViewFieldsOnly = true;
            
            //List<string> exps = new List<string>();
            //exps.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            //exps.Add(string.Format("<Neq><FieldRef Name='{0}'/><Value Type='Boolean'>1</Value></Neq>", DatabaseObjects.Columns.IsDeleted));
            //exps.Add(string.Format("<IsNotNull><FieldRef Name='{0}'/></IsNotNull>", DatabaseObjects.Columns.Category));
            //exps.Add(string.Format("<IsNotNull><FieldRef Name='{0}'/></IsNotNull>", DatabaseObjects.Columns.RequestTypeSubCategory));
            DataTable dt = spList;
            if (dt != null && dt.Rows.Count > 0)
            {
                var groupData = dt.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category));
                foreach (var category in groupData)
                {
                    if (string.IsNullOrEmpty(category.Key))
                        continue;

                    table.Rows.Add(category.Key, string.Empty, category.Key);

                    var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory));
                    foreach (var subCategory in subCategories)
                    {
                        if (!string.IsNullOrEmpty(subCategory.Key) && config.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                        {
                            table.Rows.Add(string.Format("{0}**{1}", category.Key, subCategory.Key), category.Key, subCategory.Key);
                        }
                    }
                }

                return table;
            }

            return null;
        }

        protected void btnSaveAuthorizedToView_Click(object sender, EventArgs e)
        {
           // SPFieldUserValueCollection collection = uHelper.GetFieldUserValueCollection(peAuthorizedToView.ResolvedEntities, SPContext.Current.Web);
            if (View != null)
            {
                //View.AuthorizedToView = new List<UserProfile>();
                //if (collection != null)
                //{
                //    collection.ForEach(x =>
                //    {
                //        View.AuthorizedToView.Add(new Core.UserInfo(x.LookupId, x.LookupValue, x.User == null ? true : false));
                //    });
                //}
                View.AuthorizedToViewUsers = peAuthorizedToView.GetValues();
                objDashboardPanelViewManager.Save(View);
            }

        }
        

        //private DataTable GetRequestTypeData(string moduleName)
        //{

        //    DataTable requestTypeData = null;
        //    SPList spList = SPListHelper.GetSPList(DatabaseObjects.Lists.RequestType);
        //    DataTable dt = spList.Items.GetDataTable();
        //    if (dt != null)
        //    {
        //        DataRow[] dr = dt.Select(string.Format("{0}='{1}' and ({2}={3} or {2} IS NULL or {2}='')", DatabaseObjects.Columns.ModuleNameLookup, moduleName,
        //               DatabaseObjects.Columns.IsDeleted, "0"));

        //        DataTable dtmenu = new DataTable();
        //        if (dr != null && dr.Length > 0)
        //        {
        //            DataTable tp = dr.CopyToDataTable();
        //            DataTable dttemp = dr.CopyToDataTable();

        //            if (!dttemp.Columns.Contains("SortRequestTypeCol"))
        //                dttemp.Columns.Add("SortRequestTypeCol", typeof(int));
        //            dttemp.Columns["SortRequestTypeCol"].Expression = string.Format("IIF([{0}] = '1', '1', '0')", DatabaseObjects.Columns.SortToBottom);


        //            //dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + DatabaseObjects.Columns.TicketRequestType + " ASC";
        //            dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + "SortRequestTypeCol" + " ASC," + DatabaseObjects.Columns.TicketRequestType + " ASC";
        //            dttemp.Columns.Add("ParentID", typeof(int));

        //            var groupData = dttemp.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category));
        //            int counter = 1;
        //            DataRow cRow = null;
        //            foreach (var category in groupData)
        //            {
        //                cRow = dttemp.NewRow();

        //                cRow[DatabaseObjects.Columns.Category] = category.Key;
        //                cRow[DatabaseObjects.Columns.TicketRequestType] = "Category: " + category.Key;
        //                cRow[DatabaseObjects.Columns.Id] = -counter;
        //                cRow["ParentID"] = 0;

        //                dttemp.Rows.Add(cRow);
        //                int cID = -counter;
        //                counter += 1;
        //                var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.RequestTypeSubCategory));
        //                foreach (var subCategory in subCategories)
        //                {
        //                    int scID = 0;
        //                    cRow = dttemp.NewRow();
        //                    if (uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
        //                    {
        //                        cRow[DatabaseObjects.Columns.Category] = category.Key;
        //                        if (!string.IsNullOrEmpty(subCategory.Key))
        //                        {
        //                            cRow[DatabaseObjects.Columns.RequestTypeSubCategory] = subCategory.Key;
        //                            cRow[DatabaseObjects.Columns.TicketRequestType] = "Sub Category: " + subCategory.Key;
        //                            cRow[DatabaseObjects.Columns.Id] = -counter;
        //                            cRow["ParentID"] = cID;
        //                            dttemp.Rows.Add(cRow);
        //                            scID = -counter;
        //                            counter += 1;
        //                        }
        //                        else { scID = cID; }
        //                    }
        //                    else { scID = cID; }

        //                    foreach (var requestTypeR in subCategory.ToArray())
        //                    {
        //                        requestTypeR["ParentID"] = scID;
        //                    }
        //                }
        //            }
        //            requestTypeData = dttemp;
        //        }
        //    }
        //    return requestTypeData;
        //}
    }
}
