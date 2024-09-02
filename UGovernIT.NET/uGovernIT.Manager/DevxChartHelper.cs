using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using DevexChart = DevExpress.XtraCharts;
using DevExpress.XtraCharts;
using uGovernIT.Utility;
using DevExpress.XtraCharts.Web;
using System.Web;
using System.Web.Script.Serialization;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using System.Globalization;
using uGovernIT.Util.Cache;
using System.Dynamic;

namespace uGovernIT.Manager
{
    public class DevxChartHelper
    {
        DashboardManager dManager;
        ApplicationContext context;
        DashboardPanelViewManager objDashboardPanelView;
        public DevxChartHelper(ApplicationContext _context)
        {
            context = _context;
            dManager = new DashboardManager(context);
            objDashboardPanelView = new DashboardPanelViewManager(context);
        }

        static Object lockObj = new object();
        /// <summary>
        /// Sets and Gets which dimension you want to display
        /// </summary>
        public int DrillDownFilter { get; set; }
        /// <summary>
        /// Sets and Gets global filter (date)
        /// </summary>
        public string GlobalFilter { get; set; }
        /// <summary>
        /// Sets and Gets Global filter 
        /// </summary>
        public string LocalFilter { get; set; }
        /// <summary>
        /// datapointfilter contain filter in following format : "dimensionIndex*dimensionval*expressionindex*expressionval*clickevent"
        /// </summary>
        public string DatapointFilter { get; set; }
        /// <summary>
        /// Gets ChartSettin which helps in creating chart
        /// </summary>
        public ChartSetting ChartSetting { get; set; }
        public bool IsCreateDataPoint { get; set; }
        public bool UseAjax { get; set; }
        public bool IsPostRequired { get; private set; }
        public string ChartTitle { get; set; }
        public bool ShowDetail { get; private set; }
        /// <summary>
        /// Who triggered to change chart
        /// 1=triggered by datapoint, 2= triggered by localfilter, 3=triggered by drilldown, 4=triggered from global filter
        /// </summary>
        public int WhoTriggered { get; set; }
        public string ExpternalFilter { get; set; }
        private int dimensionIndex;
        DateTime dateForDateView = DateTime.Now.Date;
        private DataTable dashboardTable;
        private bool isPreview;
        private string legendFont = "Poppins";

        private int axisXfont = 10;
        private int axisYfont = 9;
        private string dimensionFilter;
        /// <summary>
        /// 0 = Single chart view
        /// 1 = Multi chart view
        /// </summary>
        public int ViewType { get; set; }

        int workingHoursInDays;
        private const string InternalTitleColumn = "Temp_Title_";
        ChartSetting setting = null;
        DevxChartHelper chartHelper = null;
        //  static string filter;
        Dashboard dashBoard;
        public DataTable GetChart(bool isPosbackReruired, Dashboard Dashboard, int ViewId, int PanelId, int PanelHeight, int PanelWidth, bool isPreview)
        {
            string viewType = "0";
            string localFilter = string.Empty;
            string globalFilter = string.Empty;
            string whoTriggered = string.Empty;
            string drillDown = string.Empty;
            string datapointFilter = string.Empty;
            string sidebar = string.Empty;
            string zoom = string.Empty;
            dashBoard = Dashboard;
            chartHelper = this;
            int dashboardViewID = 0;
            int.TryParse(ViewId.ToString(), out dashboardViewID);
            DashboardPanelView view = objDashboardPanelView.LoadViewByID(dashboardViewID, true);
            int dashboardViewType = 0;
            int.TryParse(viewType, out dashboardViewType);
            chartHelper.ViewType = dashboardViewType;
            chartHelper.GlobalFilter = DevxChartHelper.GetGlobalFilter(context, globalFilter, view);
            chartHelper.LocalFilter = localFilter;
            int dimensionIndex = 0;
            int.TryParse(drillDown, out dimensionIndex);
            chartHelper.DrillDownFilter = dimensionIndex;
            chartHelper.DatapointFilter = datapointFilter;
            string chartTitle = "$Date$";
            //get Date range for chart:start
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            string range = string.Empty;
            if (string.IsNullOrEmpty(LocalFilter))
            {
                LocalFilter = ChartSetting.BasicDateFilterDefaultView;
            }
            uHelper.GetStartEndDateFromDateView(chartHelper.LocalFilter, ref startDate, ref endDate, ref range);
            ChartTitle = chartTitle.Replace("$Date$", range);
            DevexChart.ChartTitle title = new DevexChart.ChartTitle();
            title.Text = ChartTitle;
            //chart.Titles.AddRange(new ChartTitle[]{title});
            title.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //get Date range for chart:end
            int dashboardWidth = 0;
            int.TryParse(PanelWidth.ToString(), out dashboardWidth);
            if (dashboardWidth <= 0)
            {
                dashboardWidth = Dashboard.PanelWidth;
            }
            int dashboardHeight = 0;
            int.TryParse(PanelHeight.ToString(), out dashboardHeight);
            if (dashboardHeight <= 0)
            {
                dashboardHeight = Dashboard.PanelHeight;
            }
            bool isSideBar = UGITUtility.StringToBoolean(sidebar);
            //Check click is coming from where
            //1= user clicked on datapoint, 2=user clicked on localfilter, 3=user click on drilldown
            int WhoTriggeredEvent = 0;
            int.TryParse(whoTriggered, out WhoTriggeredEvent);
            bool isZoom = UGITUtility.StringToBoolean(zoom);
            chartHelper.WhoTriggered = WhoTriggeredEvent;
            int aheight = dashboardHeight - 25;
            if (view == null || view.ViewProperty is IndivisibleDashboardsView || isZoom)
            {
                aheight = dashboardHeight - 60;
            }
            var seriesData = CreateChartObj();
            return seriesData;
        }
        public DevexChart.Web.WebChartControl GetChart(bool isPosbackReruired, Dashboard Dashboard, Unit PanelHeight, bool isPreview)
        {
            //lock (lockObj)
            //{
                WebChartControl chart = new WebChartControl();
                ChartSetting panel = (ChartSetting)Dashboard.panel;
                dashBoard = Dashboard;
                chartHelper = this;
                chart = chartHelper.CreateChartSkelton(isPosbackReruired, (Dashboard.PanelWidth - 20).ToString().ToString(), (PanelHeight.Value - 65).ToString(), isPreview);
                //chart.EnableClientSideAPI = true;
                //chart.EnableTheming = true;
                chart.Theme = "Default";
                chart.ClientSideEvents.EndCallback = "OnEndCallBack";
                chart.CssClass = "devexpressChart";
                //chart.EndInit();
                chart.ObjectSelected += chart_ObjectSelected;
                chart.CustomCallback += chart_CustomCallback;
                chart.EnableCallBacks = true;
                chart.SaveStateOnCallbacks = true;
                chart.EnableCallbackCompression = true;
                return chart;
            //}
        }

        void chart_BoundDataChanged(object sender, EventArgs e)
        {
            WebChartControl chart = sender as WebChartControl;

            if (chart.Series.Count != 0)
            {
                for (int i = 0; i <= chart.Series.Count - 1; i++)
                {
                    if (chart.Series[i].Points.Count <= 2)
                    {
                        DevexChart.ViewType vt = DevExpress.XtraCharts.Native.SeriesViewFactory.GetViewType(chart.Series[i].View);
                        if (vt == DevexChart.ViewType.Bar || vt == DevexChart.ViewType.StackedBar)
                        {
                            ((DevexChart.BarSeriesView)chart.Series[i].View).BarWidth = 0.2;
                        }
                    }
                }
            }

        }
        void chart_CustomCallback(object sender, CustomCallbackEventArgs e)
        {
            //lock (lockObj)
            //{
                WebChartControl chart = sender as WebChartControl;
                setting = dashBoard.panel as ChartSetting;
                chartHelper = this;
                string viewID = string.Empty;
                string panelID = string.Empty;
                string sidebar = string.Empty;
                string width = string.Empty;
                string height = string.Empty;
                string viewType = string.Empty;
                string datapointFilter = string.Empty;
                string drillDown = string.Empty;
                string localFilter = string.Empty;
                string globalFilter = string.Empty;
                string whoTriggered = string.Empty;
                string zoom = string.Empty;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, string> dataFilter = serializer.Deserialize<Dictionary<string, string>>(e.Parameter);
                dataFilter.TryGetValue("viewID", out viewID);
                dataFilter.TryGetValue("panelID", out panelID);
                dataFilter.TryGetValue("sidebar", out sidebar);
                dataFilter.TryGetValue("width", out width);
                dataFilter.TryGetValue("height", out height);
                dataFilter.TryGetValue("viewType", out viewType);
                dataFilter.TryGetValue("datapointFilter", out datapointFilter);

                datapointFilter = Uri.UnescapeDataString(datapointFilter);

                dataFilter.TryGetValue("drillDown", out drillDown);
                dataFilter.TryGetValue("localFilter", out localFilter);
                dataFilter.TryGetValue("globalFilter", out globalFilter);
                dataFilter.TryGetValue("whoTriggered", out whoTriggered);
                dataFilter.TryGetValue("zoom", out zoom);

                int panelId = Convert.ToInt32(panelID);
                dManager = new DashboardManager(context);
                Dashboard dashboard = dManager.LoadPanelById(panelId, true);
                dashboard.PanelWidth = 240;
                dashboard.PanelHeight = 150;

                int dashboardViewID = 0;
                int.TryParse(viewID, out dashboardViewID);
                DashboardPanelView view = objDashboardPanelView.LoadViewByID(dashboardViewID, true);

                int dashboardViewType = 0;
                int.TryParse(viewType, out dashboardViewType);
                chartHelper.ViewType = dashboardViewType;
                //chartHelper.UseAjax = false;
                //chart.EnableViewState = false;
                chartHelper.GlobalFilter = DevxChartHelper.GetGlobalFilter(context, globalFilter, view);
                chartHelper.LocalFilter = localFilter;
                int dimensionIndex = 0;
                int.TryParse(drillDown, out dimensionIndex);
                chartHelper.DrillDownFilter = dimensionIndex;
                chartHelper.DatapointFilter = datapointFilter;
                string chartTitle = "$Date$";
                //get Date range for chart:start
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                string range = string.Empty;

                //Gets specified localfilter if not exist then get defaultlocalfilter if any
                if (string.IsNullOrEmpty(LocalFilter))
                {
                    //if (string.IsNullOrEmpty(ChartSetting.BasicDateFilterDefaultView))
                    //{

                    //}
                    LocalFilter = ChartSetting.BasicDateFilterDefaultView;
                }
                //Get start and end date using localfilter value            
                uHelper.GetStartEndDateFromDateView(chartHelper.LocalFilter, ref startDate, ref endDate, ref range);
                ChartTitle = chartTitle.Replace("$Date$", range);
                DevexChart.ChartTitle title = new DevexChart.ChartTitle();
                title.Text = ChartTitle;
                //chart.Titles.AddRange(new ChartTitle[]{title});
                title.Visibility = DevExpress.Utils.DefaultBoolean.False;
                chart.Titles.Clear();
                chart.Titles.Add(title);
                //get Date range for chart:end
                int dashboardWidth = 0;
                int.TryParse(width, out dashboardWidth);
                if (dashboardWidth <= 0)
                {
                    dashboardWidth = dashboard.PanelWidth;
                }
                int dashboardHeight = 0;
                int.TryParse(height, out dashboardHeight);
                if (dashboardHeight <= 0)
                {
                    dashboardHeight = dashboard.PanelHeight;
                }
                bool isSideBar = UGITUtility.StringToBoolean(sidebar);
                //Check click is coming from where
                //1= user clicked on datapoint, 2=user clicked on localfilter, 3=user click on drilldown
                int WhoTriggeredEvent = 0;
                int.TryParse(whoTriggered, out WhoTriggeredEvent);
                bool isZoom = UGITUtility.StringToBoolean(zoom);
                chartHelper.WhoTriggered = WhoTriggeredEvent;
                int aheight = dashboardHeight - 25;
                if (view == null || view.ViewProperty is IndivisibleDashboardsView || isZoom)
                {
                    aheight = dashboardHeight - 60;
                }
                chart = chartHelper.CreateChart(true, false, chart);
            //}
        }

        void chart_ObjectSelected(object sender, DevexChart.HotTrackEventArgs e)
        {
            WebChartControl chart = sender as WebChartControl;

            if (chart.EmptyChartText.Text != "No data found" && e.HitInfo.Series != null)
            {
                SeriesPoint point = e.HitInfo.SeriesPoint;
                if (point == null)
                    return;
                TextAnnotation txt = point.Annotations[0] as TextAnnotation;
                if (txt == null)
                    return;
                string[] extraInfo = UGITUtility.SplitString(txt.Text, "::");
                //string[] extraInfo = uHelper.SplitString(e.HitInfo.SeriesPoint.ToolTipHint, "::");
                string filter = extraInfo[0];
                string clickEventType = extraInfo[1];
                chart.CssPostfix = string.Format("{0}::{1}", filter, clickEventType);
                if (UGITUtility.StringToInt(clickEventType) == 1)
                {
                    setting = dashBoard.panel as ChartSetting;
                    chartHelper = new DevxChartHelper(setting, context);
                    chartHelper.DatapointFilter = filter;
                    chartHelper.CreateChart(true, false, chart);
                }
            }
        }

        public DevxChartHelper(ChartSetting chartSetting, ApplicationContext _context)
        {
            GlobalFilter = string.Empty;
            ChartTitle = string.Empty;
            ChartSetting = chartSetting;
            DatapointFilter = string.Empty;
            LocalFilter = string.Empty;
            workingHoursInDays = uHelper.GetWorkingHoursInADay(_context);
            //Load dashboard fact table
            context = _context;
            dManager = new DashboardManager(_context);
            objDashboardPanelView = new DashboardPanelViewManager(_context);


        }


        private void LoadFactTable()
        {
            DataTable table = DashboardCache.GetCachedDashboardData(context, ChartSetting.FactTable);
            if (table != null)
            {
                dashboardTable = table.DefaultView.ToTable();
                if (!dashboardTable.Columns.Contains("today"))
                {
                    dashboardTable.Columns.Add("today", typeof(DateTime), string.Format("'{0}'", System.DateTime.Now.ToLongDateString()));
                }
                if (!dashboardTable.Columns.Contains("me"))
                {
                    if (context.CurrentUser != null)
                        dashboardTable.Columns.Add("me", typeof(string), string.Format("'{0}'", context.CurrentUser.Name.Replace("'", "''")));
                }
            }
        }

        /// <summary>
        /// Get filtered data after applying all kind of filter
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilteredData()
        {
            //set next dimensionindex based on dimensionfilter and datapoint filter
            string[] filterArray = DatapointFilter.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);
            dimensionIndex = 0;
            if (filterArray.Length > 0)
            {
                int.TryParse(filterArray[0], out dimensionIndex);
            }

            //Load facttable
            LoadFactTable();

            DataTable datapointTable = new DataTable("FilteredData");

            //Create fitlered table schema
            datapointTable = CreateDatapointTableSchema(dashboardTable);

            //Dimension filter data
            #region skip it for now. Special handling for resource type data.
            if (ChartSetting.FactTable == DatabaseObjects.Tables.ResourceUsageSummaryMonthWise && ChartSetting.FactTable == DatabaseObjects.Tables.ResourceUsageSummaryWeekWise)
            {

                int expressionIndex = 0;
                string expressionPointValue = string.Empty;
                string dimensionPointValue = string.Empty;
                if (filterArray.Length > 4)
                {
                    dimensionPointValue = filterArray[1];
                    int.TryParse(filterArray[2], out expressionIndex);
                    expressionPointValue = filterArray[3];
                }

                //Gets specified localfilter if not exist then get defaultlocalfilter if any
                if (string.IsNullOrEmpty(LocalFilter))
                {
                    LocalFilter = ChartSetting.BasicDateFilterDefaultView;
                }

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                string range = string.Empty;


                uHelper.GetStartEndDateFromDateView(LocalFilter, ref startDate, ref endDate, ref range);
                ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
                ChartExpression expression = ChartSetting.Expressions[expressionIndex];
                DataTable workItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"TenantID='{context.TenantID}'");// uGITCache.LoadTable(DatabaseObjects.Tables.ResourceWorkItems);
                DataRow[] filteredWorkItems = new DataRow[0];
                if ((new string[] { DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem, DatabaseObjects.Columns.SubWorkItem, DatabaseObjects.Columns.Resource }).Contains(dimension.SelectedField))
                {
                    filteredWorkItems = workItems.Select(string.Format("{0}='{1}'", dimension.SelectedField, dimensionPointValue));
                }
                if (filteredWorkItems.Length > 0)
                {
                    workItems = filteredWorkItems.CopyToDataTable();
                }

                string exp = expression.FunctionExpression;
                if (exp != null)
                    exp = expression.FunctionExpression.Replace("[", string.Empty).Replace("]", string.Empty);

                if (exp == DatabaseObjects.Columns.PctAllocation || exp == DatabaseObjects.Columns.AllocationHour)
                {
                    DataTable resourceAllocations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, $"TenantID='{context.TenantID}'");// uGITCache.LoadTable(DatabaseObjects.Tables.ResourceAllocation);
                    resourceAllocations = ApplyDatapointFilter(resourceAllocations, true);

                    DataTable filteredAlloations = new DataTable();
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.WorkItemType);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.WorkItem);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.SubWorkItem);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.Resource);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.AllocationStartDate);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.AllocationEndDate);
                    filteredAlloations.Columns.Add(DatabaseObjects.Columns.PctAllocation);

                    DataRow[] filterResourceAllocations = new DataRow[0];
                    if (string.IsNullOrEmpty(LocalFilter) || LocalFilter.ToLower() == "select all")
                    {
                        filterResourceAllocations = resourceAllocations.Select();
                    }
                    else
                    {
                        filterResourceAllocations = resourceAllocations.Select(string.Format("{0}<= #{3}# and {1} >= #{2}#", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, startDate.Date.ToString("MM/dd/yyyy"), endDate.Date.ToString("MM/dd/yyyy")));
                    }

                    //Apply basic filter in actual table (allocation table) then replace ManagerLookup to Manager
                    if (!string.IsNullOrEmpty(ChartSetting.BasicFilter) && filterResourceAllocations.Length > 0)
                    {
                        DataTable filteredData = filterResourceAllocations.CopyToDataTable();
                        string filter = ChartSetting.BasicFilter.Replace(DatabaseObjects.Columns.ManagerLookup, DatabaseObjects.Columns.Manager);

                        try
                        {
                            filterResourceAllocations = filteredData.Select(filter);
                        }
                        catch (Exception ex)
                        {
                            //consume
                            ULog.WriteException(ex);
                        }
                    }

                    //All dimension filter
                    if (dimension != null && !string.IsNullOrEmpty(dimension.SelectedField) && filterResourceAllocations.Length > 0)
                    {
                        string dimensionFilterField = dimension.SelectedField.Replace(DatabaseObjects.Columns.ManagerLookup, DatabaseObjects.Columns.Manager);
                        if (resourceAllocations.Columns.Contains(dimensionFilterField))
                        {
                            DataColumn dimensionColumn = resourceAllocations.Columns[dimensionFilterField];
                            string dimenionQuery = string.Empty;
                            if (dimensionColumn.DataType == typeof(string))
                            {
                                dimenionQuery = string.Format("{0}='{1}'", dimensionColumn.ColumnName, dimensionPointValue);
                            }
                            else if (dimensionColumn.DataType == typeof(DateTime))
                            {
                                dimenionQuery = string.Format("{0}=#{1}#", dimensionColumn.ColumnName, dimensionPointValue);
                            }
                            else
                            {
                                dimenionQuery = string.Format("{0}={1}", dimensionColumn.ColumnName, dimensionPointValue);
                            }

                            try
                            {
                                DataTable filterData = filterResourceAllocations.CopyToDataTable();
                                filterResourceAllocations = filterData.Select(dimenionQuery);
                            }
                            catch (Exception ex)
                            {
                                //consume
                                ULog.WriteException(ex);
                            }
                        }
                    }


                    foreach (DataRow row in filterResourceAllocations)
                    {
                        DataRow[] workItemRows = workItems.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, row[DatabaseObjects.Columns.ResourceWorkItemLookup]));
                        if (workItemRows.Length > 0)
                        {
                            DataRow newRow = filteredAlloations.NewRow();
                            filteredAlloations.Rows.Add(newRow);
                            newRow[DatabaseObjects.Columns.Id] = row[DatabaseObjects.Columns.Id];
                            newRow[DatabaseObjects.Columns.WorkItemType] = workItemRows[0][DatabaseObjects.Columns.WorkItemType];
                            newRow[DatabaseObjects.Columns.WorkItem] = workItemRows[0][DatabaseObjects.Columns.WorkItem];
                            newRow[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                            newRow[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                            newRow[DatabaseObjects.Columns.AllocationStartDate] = Convert.ToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]).ToString("MM/dd/yyyy");
                            newRow[DatabaseObjects.Columns.AllocationEndDate] = Convert.ToDateTime(row[DatabaseObjects.Columns.AllocationEndDate]).ToString("MM/dd/yyyy");
                            newRow[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                        }
                    }


                    if (filteredAlloations != null && filteredAlloations.Rows.Count > 0)
                    {
                        return filteredAlloations;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    DataTable resourceTimeSheet = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet, $"TenantID='{context.TenantID}'");// uGITCache.LoadTable(DatabaseObjects.Tables.ResourceTimeSheet);
                    resourceTimeSheet = ApplyDatapointFilter(resourceTimeSheet, true);

                    DataTable filteredTimeSheet = new DataTable();
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.WorkItemType);
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.WorkItem);
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.SubWorkItem);
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.Resource);
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.WorkDate);
                    filteredTimeSheet.Columns.Add(DatabaseObjects.Columns.HoursTaken);

                    DataRow[] filterResourceActuals = new DataRow[0];
                    if (string.IsNullOrEmpty(LocalFilter) || LocalFilter.ToLower() == "select all")
                    {
                        filterResourceActuals = resourceTimeSheet.Select();
                    }
                    else
                    {
                        filterResourceActuals = resourceTimeSheet.Select(string.Format("{0}>= #{1}# and {2} <=#{3}#", DatabaseObjects.Columns.WorkDate, startDate.Date.ToString("MM/dd/yyyy"), DatabaseObjects.Columns.WorkDate, endDate.Date.ToString("MM/dd/yyyy")));
                    }

                    //Apply basic filter in actual table (allocation table) then replace ManagerLookup to Manager
                    if (!string.IsNullOrEmpty(ChartSetting.BasicFilter) && filterResourceActuals.Length > 0)
                    {
                        DataTable filteredData = filterResourceActuals.CopyToDataTable();
                        string filter = ChartSetting.BasicFilter.Replace(DatabaseObjects.Columns.ManagerLookup, DatabaseObjects.Columns.Manager);

                        try
                        {
                            filterResourceActuals = filteredData.Select(filter);
                        }
                        catch (Exception ex)
                        {
                            //consume
                            ULog.WriteException(ex);
                        }
                    }

                    //All dimension filter
                    if (dimension != null && !string.IsNullOrEmpty(dimension.SelectedField) && filterResourceActuals.Length > 0)
                    {
                        string dimensionFilterField = dimension.SelectedField.Replace(DatabaseObjects.Columns.ManagerLookup, DatabaseObjects.Columns.Manager);
                        if (resourceTimeSheet.Columns.Contains(dimensionFilterField))
                        {
                            DataColumn dimensionColumn = resourceTimeSheet.Columns[dimensionFilterField];
                            string dimenionQuery = string.Empty;
                            if (dimensionColumn.DataType == typeof(string))
                            {
                                dimenionQuery = string.Format("{0}='{1}'", dimensionColumn.ColumnName, dimensionPointValue);
                            }
                            else if (dimensionColumn.DataType == typeof(DateTime))
                            {
                                dimenionQuery = string.Format("{0}=#{1}#", dimensionColumn.ColumnName, dimensionPointValue);
                            }
                            else
                            {
                                dimenionQuery = string.Format("{0}={1}", dimensionColumn.ColumnName, dimensionPointValue);
                            }

                            try
                            {
                                DataTable filterData = filterResourceActuals.CopyToDataTable();
                                filterResourceActuals = filterData.Select(dimenionQuery);
                            }
                            catch (Exception ex)
                            {
                                //consume
                                ULog.WriteException(ex);
                            }
                        }
                    }


                    foreach (DataRow row in filterResourceActuals)
                    {
                        DataRow[] workItemRows = workItems.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, row[DatabaseObjects.Columns.ResourceWorkItemLookup]));
                        if (workItemRows.Length > 0)
                        {
                            DataRow newRow = filteredTimeSheet.NewRow();
                            filteredTimeSheet.Rows.Add(newRow);
                            newRow[DatabaseObjects.Columns.Id] = row[DatabaseObjects.Columns.Id];
                            newRow[DatabaseObjects.Columns.WorkItemType] = workItemRows[0][DatabaseObjects.Columns.WorkItemType];
                            newRow[DatabaseObjects.Columns.WorkItem] = workItemRows[0][DatabaseObjects.Columns.WorkItem];
                            newRow[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                            newRow[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                            newRow[DatabaseObjects.Columns.WorkDate] = Convert.ToDateTime(row[DatabaseObjects.Columns.WorkDate]).ToString("MM/dd/yyyy");
                            newRow[DatabaseObjects.Columns.HoursTaken] = row[DatabaseObjects.Columns.HoursTaken];
                        }
                    }

                    if (filteredTimeSheet != null && filteredTimeSheet.Rows.Count > 0)
                    {
                        return filteredTimeSheet;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            #endregion

            DataTable data = GetDataAgainstDimension();
            data = ApplyDatapointFilter(data, true);

            //Apply Global Filter
            data = ApplyGlobalFilter(data);

            //Apply Local Filter
            data = ApplyLocalFilter(data);

            return data;
        }

        /// <summary>
        /// Get table which has all datapoints based on all filters
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableView()
        {
            DataTable datapointTable = new DataTable("FilteredData");
            datapointTable = GetChartPointsTable();
            if (datapointTable != null)
                datapointTable = datapointTable.DefaultView.ToTable();

            datapointTable = ExpressionCalc.RemoveExpressionTempColumns(datapointTable);
            if (datapointTable != null && datapointTable.Columns.Contains(InternalTitleColumn))
                datapointTable.Columns.Remove(InternalTitleColumn);

            return datapointTable;
        }

        /// <summary>
        /// Get chart points in table view
        /// </summary>
        /// <returns></returns>
        private DataTable GetChartPointsTable(bool forceLoad = false)
        {
            DataTable datapointTable = new DataTable("FilteredData");
            //set next dimensionindex based on dimensionfilter and datapoint filter
            SetNewDimensionIndex();
            if (ChartSetting.IsCacheChart && !forceLoad && dimensionIndex <= 0)
            {
                datapointTable = GetCachedChartPointsTable();
                if (datapointTable != null)
                    return datapointTable;
            }

            //Load facttable
            LoadFactTable();

            //Dimension filter data
            DataTable data = GetDataAgainstDimension();

            if (data == null || data.Rows.Count <= 0)
                return datapointTable;

            // var rows = data.AsEnumerable().Where(row => row.Field<String>("TenantId") == context.TenantID);
            if (data.Columns[0].ColumnName.Contains(DatabaseObjects.Columns.TenantID))
            {
                var rows = data.AsEnumerable().Where(row => row.Field<String>(DatabaseObjects.Columns.TenantID).ToUpper() == context.TenantID.ToUpper());
                if (rows.Any())
                {
                    var dt = rows.CopyToDataTable();
                    data = dt;
                }
                else
                {
                    data.Clear();
                }
            }


            //Apply Global Filter. It works when somebody change global filter
            data = ApplyGlobalFilter(data);

            //Apply Local Filter.It works when somebody select local filter option
            data = ApplyLocalFilter(data);

            //Apply DataPoint filter. It works when somebody click on datapoint
            data = ApplyDatapointFilter(data);


            //Create fitlered table schema
            datapointTable = CreateDatapointTableSchema(data);

            //Fill datapoint data after appling all filter. Now we have right data to create datapoint
            FillDataPointsTable(data, datapointTable);
            datapointTable = PickDataPoints(datapointTable);

            return datapointTable;
        }

        private DataTable GetCachedChartPointsTable()
        {
            DataTable dataPoints = null;

            if (!ChartSetting.IsCacheChart || ChartSetting.CacheSchedule <= 0 || !string.IsNullOrEmpty(this.DatapointFilter) || !string.IsNullOrEmpty(this.GlobalFilter) ||
                (!string.IsNullOrEmpty(this.LocalFilter) && this.LocalFilter != ChartSetting.BasicDateFilterDefaultView))
                return dataPoints;

            bool loadInThread = false;

            ChartCachedDataPoints chartCachedDatapoints = ChartCache.GetChartCache(ChartSetting.ChartId.ToString());
            if (chartCachedDatapoints != null && chartCachedDatapoints.DataPoints != null)
            {
                this.ChartTitle = chartCachedDatapoints.Title;
                dataPoints = chartCachedDatapoints.DataPoints;


                //Checks cached chart is expired or not. if so then enable loadinthread flag
                if (chartCachedDatapoints.LastUpdated.AddMinutes(ChartSetting.CacheSchedule) < DateTime.Now)
                {
                    if (!chartCachedDatapoints.IsUpdating)
                    {
                        lock (lockObj)
                        {
                            //Checks chart is being refreshed or not
                            ChartCachedDataPoints chartDatapoints = ChartCache.GetChartCache(ChartSetting.ChartId.ToString());
                            if (!chartDatapoints.IsUpdating)
                            {
                                ChartCache.SetChartToUpdate(chartCachedDatapoints.ChartID);
                                loadInThread = true;
                            }
                        }
                    }
                }
            }
            else
            {
                loadInThread = true;
                ChartCachedDataPoints chartDatapoints = new ChartCachedDataPoints();
                ChartCache.RefreshChartCache(this);
                ChartCache.SetChartToUpdate(chartDatapoints.ChartID);
            }

            //Load datapoint again if loadinthread enable
            if (loadInThread)
            {
                string webUrl = HttpContext.Current.Request.Url.ToString();
                ThreadStart refreshChartDelegate = delegate ()
                {
                    //using (SPSite mySiteCollection = new SPSite(webUrl))
                    //{
                    //    using (SPWeb myWeb = mySiteCollection.OpenWeb())
                    //    {
                    DevxChartHelper chartH = new DevxChartHelper(ChartSetting, context);
                    chartH.ChartTitle = this.ChartTitle;
                    chartH.GlobalFilter = this.GlobalFilter;
                    chartH.LocalFilter = this.LocalFilter;
                    chartH.DrillDownFilter = this.DrillDownFilter;
                    chartH.DatapointFilter = this.ExpternalFilter;
                    chartH.ChartTitle = this.ChartTitle;
                    chartH.DatapointFilter = this.DatapointFilter;
                    chartH.ExpternalFilter = this.ExpternalFilter;
                    chartH.dimensionFilter = this.dimensionFilter;

                    DataTable data = chartH.GetChartPointsTable(true);
                    ChartCache.RefreshChartCache(this, data);
                };
                Thread refreshChartThread = new Thread(refreshChartDelegate);
                refreshChartThread.IsBackground = true;
                refreshChartThread.Start();
            }

            return dataPoints;
        }

        /// <summary>
        /// Create basic chart without series
        /// </summary>
        /// <param name="isPostRequired"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isPreview"></param>
        /// <returns></returns>

        public DevexChart.Web.WebChartControl CreateChart(bool isPostRequired, bool isPreview, DevexChart.Web.WebChartControl dcpChart)
        {
            //Load datapoint again if isLoadChard enable
            //if (isLoadChart)
            {
                dcpChart = CreateChartObj(isPostRequired, isPreview, dcpChart);
            }

            return dcpChart;
        }

        public DevexChart.Web.WebChartControl CreateChartSkelton(bool isPostRequired, string width, string height, bool isPreview)
        {
            //Set action is required or not on datapoints
            this.IsPostRequired = isPostRequired;
            this.isPreview = isPreview;

            //Get chart width basic setting           
            DevexChart.Web.WebChartControl dcpChart = new DevexChart.Web.WebChartControl();
            dcpChart = DxBasicChartSetting();

            //Set width and height of chart according to the specification            
            dcpChart.ID = "customChart_" + ChartSetting.ChartId.ToString();

            if (width != null && !width.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
            {
                dcpChart.Width = Unit.Parse(width.Trim());
            }

            if (height != null && !height.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
            {
                dcpChart.Height = Unit.Parse(height.Trim());
            }
            return dcpChart;
        }



        /// <summary>
        /// Create chart
        /// </summary>
        /// <param name="chartSetting"></param>
        /// <param name="isPostRequired"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isPreview"></param>
        /// <returns></returns>
        public DevexChart.Web.WebChartControl CreateChartObj(bool isPostRequired, bool isPreview, DevexChart.Web.WebChartControl dcpChart)
        {

            //Set action is required or not on datapoints
            this.IsPostRequired = isPostRequired;
            this.isPreview = isPreview;

            //Get datapoints table view
            DataTable datapointsTable = GetChartPointsTable();

            if (datapointsTable != null && datapointsTable.Rows.Count != 0)
            {
                if (ChartSetting.ReversePlotting)
                {
                    //Create series for chart
                    if (ChartSetting.Dimensions.Count == 1)
                    {
                        CreateDxChartSeriesUsingRow(dcpChart, datapointsTable);
                        PlotChartPointsReverse(dcpChart, datapointsTable);
                    }
                    else
                    {
                        dcpChart.Series.Clear();
                        dcpChart.EmptyChartText.Text = "No data found";
                        dcpChart.EmptyChartText.TextColor = System.Drawing.Color.Red;
                    }

                }
                else
                {
                    CreateChartSeries(dcpChart, datapointsTable);
                    //Plot chart points using datapointsTable
                    PlotDxChartPoints(dcpChart, datapointsTable);
                }

            }
            else
            {
                dcpChart.Series.Clear();
                dcpChart.EmptyChartText.Text = "No data found";
                dcpChart.EmptyChartText.TextColor = System.Drawing.Color.Red;
            }


            return dcpChart;



            //Sets showDetail property which can be used to open detail popup when next dimension is empty
            //if (ChartSetting.Dimensions.Count > 0 && WhoTriggered == 1)
            //{
            //    ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
            //    if (dimension.DataPointClickEvent == DatapointClickeEventType.Detail)
            //    {
            //        if (datapointsTable == null || datapointsTable.Rows.Count <= 0)
            //        {
            //            ShowDetail = true;
            //        }

            //        DataTable uniquePoints = datapointsTable.DefaultView.ToTable(true, datapointsTable.Columns[0].ColumnName);
            //        if (uniquePoints.Rows.Count == 1 && (uniquePoints.Rows[0][0] == null || Convert.ToString(uniquePoints.Rows[0][0]) == "None"))
            //        {
            //            ShowDetail = true;
            //        }
            //    }
            //}


        }


        /// <summary>
        /// Set new dimension index based on dimensionfilter and expressionfitler property
        /// </summary>
        private void SetNewDimensionIndex()
        {
            // set dimensionindex to -1 then return if dimension is not exist
            if (ChartSetting.Dimensions.Count <= 0)
            {
                dimensionIndex = -1;
                return;
            }


            int nextDimensionIndex = 0;
            //if datapointfilter is empty the move to next dimension
            if (string.IsNullOrEmpty(DatapointFilter))
            {
                if ((ChartSetting.Dimensions.Count - 1) >= DrillDownFilter)
                {
                    nextDimensionIndex = DrillDownFilter;
                }
            }
            // Parsr datapointfilter the set next dimension in nextdimensionindex
            else
            {
                string[] datapointFilterArray = DatapointFilter.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);

                string dIndex = datapointFilterArray[0];
                string eIndex = datapointFilterArray[2];
                int dIndexVal = 0;
                int.TryParse(dIndex.Trim(), out dIndexVal);

                int eIndexVal = 0;
                int.TryParse(eIndex.Trim(), out eIndexVal);

                //Gets selected datapoint dimension
                ChartDimension currentDimension = ChartSetting.Dimensions[dIndexVal];
                ChartExpression currentExpression = ChartSetting.Expressions[eIndexVal];

                //Checks expression clienkevent if interhit then apply dimension clickevent settings otherwise apply expression clickevent setting
                if (currentExpression.DataPointClickEvent == DatapointClickeEventType.Inherit)
                {
                    nextDimensionIndex = dIndexVal;
                    //if dimension
                    if (currentDimension.DataPointClickEvent == DatapointClickeEventType.NextDimension && (ChartSetting.Dimensions.Count - 1) > dIndexVal)
                    {
                        nextDimensionIndex = dIndexVal + 1;
                    }
                }
                else
                {
                    nextDimensionIndex = dIndexVal;
                    //if dimension
                    if (currentExpression.DataPointClickEvent == DatapointClickeEventType.NextDimension && (ChartSetting.Dimensions.Count - 1) > dIndexVal)
                    {
                        nextDimensionIndex = dIndexVal + 1;
                    }
                }
            }
            dimensionIndex = nextDimensionIndex;
        }

        /// <summary>
        /// Create table schema for datapoint which is used to plot chart points
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDatapointTableSchema(DataTable data)
        {
            DataTable newTable = new DataTable();

            //Creates column which store x-axis points
            //if Dimension exist then dimension title will be the columnname otherwise put charttitle as colmnname
            newTable.Columns.Add(InternalTitleColumn);
            ChartDimension dimension = null;
            if (ChartSetting.Dimensions.Count > 0)
            {
                dimension = ChartSetting.Dimensions[dimensionIndex];
                if (!newTable.Columns.Contains(dimension.Title))
                {
                    newTable.Columns.Add(dimension.Title, typeof(string));
                }
            }
            else
            {
                newTable.Columns.Add(ChartTitle, typeof(string));
            }

            if (data == null)
            {
                return newTable;
            }

            //Create column to store eache expression point value
            //if expression contains groupfield then group the dashboardtable based on group and then create column for each group
            List<ChartExpression> expressions = ChartSetting.Expressions;
            if (dimension != null)
                expressions = ChartSetting.Expressions.Where(x => x.Dimensions == null || x.Dimensions.Count == 0 || x.Dimensions.Exists(y => y.ToLower() == dimension.Title.ToLower())).ToList();

            foreach (ChartExpression expression in expressions)
            {
                DevexChart.ViewType chartType = DevexChart.ViewType.Bar;
                Enum.TryParse<DevexChart.ViewType>(expression.ChartType, out chartType);

                //if charttype: pie, funnel, pyramit, doughnut then you can not set group field so reset expression GroupByField setting
                if (chartType == DevexChart.ViewType.Pie || chartType == DevexChart.ViewType.Funnel || chartType == DevexChart.ViewType.Doughnut)
                {
                    expression.GroupByField = string.Empty;
                }

                if (!string.IsNullOrEmpty(expression.GroupByField) && data != null && data.Rows.Count > 0 && data.Columns.Contains(expression.GroupByField))
                {
                    int columnIndex = data.Columns.IndexOf(data.Columns[expression.GroupByField]);
                    //Groups facttable based on GroupByField
                    //data.DefaultView.Sort = string.Format("{0} desc", expression.GroupByField);
                    DataTable tableGroup = data.DefaultView.ToTable(true, expression.GroupByField);

                    foreach (DataRow row in tableGroup.Rows)
                    {
                        //Checks group item is already exist in newTable or not if exist
                        //then append the incremental number of that item
                        if (newTable.Columns.Contains(Convert.ToString(row[expression.GroupByField])))
                        {
                            newTable.Columns.Add(string.Format("{0}-{1}", row[expression.GroupByField], newTable.Columns.Count), typeof(double));
                        }
                        //Add Item in chart datapoint schema
                        else
                        {
                            string columnName = Convert.ToString(row[expression.GroupByField]);
                            if (string.IsNullOrEmpty(columnName))
                            {
                                columnName = "None";
                            }

                            if (!newTable.Columns.Contains(columnName))
                            {
                                if (expression.Operator != string.Empty)
                                    newTable.Columns.Add(columnName, typeof(double));
                                else if (data.Columns.Contains(expression.FunctionExpression))
                                {
                                    if (!newTable.Columns.Contains(columnName))
                                        newTable.Columns.Add(columnName, data.Columns[expression.FunctionExpression].DataType);
                                }
                                else
                                    newTable.Columns.Add(columnName, typeof(string));
                            }
                        }

                        //Store expressionindex in column extended property so that we can get it
                        if (!newTable.Columns[newTable.Columns.Count - 1].ExtendedProperties.ContainsKey("expressionIndex"))
                            newTable.Columns[newTable.Columns.Count - 1].ExtendedProperties.Add("expressionIndex", ChartSetting.Expressions.IndexOf(expression));
                    }
                }
                else
                {
                    //Checks expression title is already exist in chart datapoint schema table or not
                    //If exist then append the incremental number of that item 
                    if (newTable.Columns.Contains(expression.Title))
                    {
                        if (expression.Operator != string.Empty)
                            newTable.Columns.Add(string.Format("{0}-{1}", expression.Title, newTable.Columns.Count), typeof(double));
                        else if (data.Columns.Contains(expression.FunctionExpression))
                        {
                            newTable.Columns.Add(string.Format("{0}-{1}", expression.Title, newTable.Columns.Count), data.Columns[expression.FunctionExpression].DataType);
                        }
                        else
                            newTable.Columns.Add(string.Format("{0}-{1}", expression.Title, newTable.Columns.Count), typeof(string));
                    }
                    else
                    {
                        if (expression.Operator != string.Empty)
                            newTable.Columns.Add(Convert.ToString(expression.Title), typeof(double));

                        else
                        //This if loop Code is added by Inderjeet Kaur on 9 Sept 2022. If the column given in FunctionExpression is present in the 
                        //original "data" datatable, then keep its datatype same while adding in the newTable. Code will be able to sort Date as
                        //DateTime instead of sorting the values as string.
                            if (data.Columns.Contains(expression.FunctionExpression))
                            {
                                newTable.Columns.Add(expression.Title, data.Columns[expression.FunctionExpression].DataType);
                            }
                            else
                                newTable.Columns.Add(Convert.ToString(expression.Title), typeof(string));
                    }

                    //Store expressionindex in column extended property so that we can get it
                    if (!newTable.Columns[newTable.Columns.Count - 1].ExtendedProperties.ContainsKey("expressionIndex"))
                        newTable.Columns[newTable.Columns.Count - 1].ExtendedProperties.Add("expressionIndex", ChartSetting.Expressions.IndexOf(expression));
                }
            }

            return newTable;
        }

        /// <summary>
        /// Get Datatable for dimension
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataAgainstDimension()
        {
            //return  if table null or rows is empty
            if (dashboardTable == null)
            {
                return null;
            }
            if (dashboardTable.Rows.Count <= 0)
            {
                return dashboardTable.Clone();
            }

            //Apply Basic filter if any other return dashboard table
            if (!string.IsNullOrEmpty(ChartSetting.BasicFilter))
            {
                try
                {
                    //check chart filter@chetan
                    if (ChartSetting.BasicFilter.Contains("[Closed] = '1'"))
                    {
                        ChartSetting.BasicFilter = ("[Closed] = 'True'");
                    }
                    DataRow[] rows = dashboardTable.Select(ChartSetting.BasicFilter);
                    if (rows.Length > 0)
                    {
                        dashboardTable = rows.CopyToDataTable();
                    }
                    else
                    {
                        dashboardTable = dashboardTable.Clone();
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }

            return dashboardTable;
        }

        /// <summary>
        /// Apply specified global filter on dashboard table
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        private DataTable ApplyGlobalFilter(DataTable inputTable)
        {
            //return if input table is null or empty
            if (inputTable == null)
            {
                return null;
            }

            if (inputTable.Rows.Count <= 0)
            {
                return inputTable.Clone();
            }

            // return if there is no global filter
            if (GlobalFilter == null || GlobalFilter.Trim() == string.Empty)
            {
                return inputTable.Copy();
            }

            //Apply global filter on input table
            try
            {
                if (ChartSetting.FactTable == DatabaseObjects.Tables.DashboardSummary)
                {
                    DataTable dt = new DataTable();
                    dt = inputTable;
                    DataRow[] filteredRows = inputTable.Copy().Select(GlobalFilter);
                    if (filteredRows.Length > 0)
                    {
                        return filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        if (filteredRows.Length == 0 && dt.Rows.Count > 0 && !string.IsNullOrEmpty(ChartSetting.BasicFilter))
                        {
                            return inputTable = dt;
                        }
                        else
                        {
                            return inputTable.Clone();
                        }

                    }
                }
                else
                {
                    return inputTable;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return inputTable.Clone();
            }
        }

        /// <summary>
        /// Apply Local filter on dashboard table
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        public DataTable ApplyLocalFilter(DataTable inputTable)
        {
            //store charttitle in local variable and Replace $Date$ token in acutual chartitle with empty
            string chartTitle = ChartTitle;
            ChartTitle = ChartTitle.Replace("$Date$", "All");


            //Gets specified localfilter if not exist then get defaultlocalfilter if any
            if (string.IsNullOrEmpty(LocalFilter))
            {
                if (string.IsNullOrEmpty(ChartSetting.BasicDateFilterDefaultView))
                {
                    return inputTable;
                }
                LocalFilter = ChartSetting.BasicDateFilterDefaultView;
            }

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            string range = string.Empty;
            //Get start and end date using localfilter value
            uHelper.GetStartEndDateFromDateView(LocalFilter, ref startDate, ref endDate, ref range);
            ChartTitle = chartTitle.Replace("$Date$", range);

            //return if inputtable is null or empty
            if (inputTable == null)
            {
                return inputTable;
            }
            if (inputTable.Rows.Count <= 0)
            {
                return inputTable.Clone();
            }

            //return if basicdatefilterstart field and basicdatefilterend field not exist
            if (string.IsNullOrEmpty(ChartSetting.BasicDateFitlerStartField) || !inputTable.Columns.Contains(ChartSetting.BasicDateFitlerStartField) || inputTable.Columns[ChartSetting.BasicDateFitlerStartField].DataType != typeof(DateTime))
            {
                return inputTable;
            }


            //Return if localfitler value "Select All". its means to show all
            if (LocalFilter == "Select All")
            {
                return inputTable;
            }

            //Does local Copy of inputtable
            DataTable filteredTable = inputTable.Copy();

            //return if start and end date both have mean value
            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                return filteredTable;
            }
            //Adds IsInRange_ field in fitleredTable which is used to get data exist between start and end date
            if (!filteredTable.Columns.Contains("IsInRange_"))
            {
                filteredTable.Columns.Add("IsInRange_", typeof(bool));
            }
            DataColumn startColumn = filteredTable.Columns[ChartSetting.BasicDateFitlerStartField];
            DataColumn endColumn = filteredTable.Columns[ChartSetting.BasicDateFitlerStartField];
            if (!string.IsNullOrEmpty(ChartSetting.BasicDateFitlerEndField))
            {
                endColumn = filteredTable.Columns[ChartSetting.BasicDateFitlerEndField];
            }
            //Apply Expression in IsInRange_ If row comes under start and end date then set value to "true" ortherwise set it as "false"
            filteredTable.Columns["IsInRange_"].Expression = string.Format("IIF(([{2}] < #{0}#   and  [{3}] >= #{1}#) , true, false)", endDate.AddDays(1).Date.ToString("MM/dd/yyyy"), startDate.Date.ToString("MM/dd/yyyy"), startColumn.ColumnName, endColumn.ColumnName);

            //Gets rows where IsInRange_ comlumn rhas tue value
            DataRow[] filteredRows = filteredTable.Select("[IsInRange_]=true");
            if (filteredRows.Length > 0)
            {
                if (filteredTable.Columns.Contains("IsInRange_"))
                    filteredTable.Columns.Remove("IsInRange_");

                filteredTable = filteredRows.CopyToDataTable();
                return filteredTable;
            }
            else
            {
                return filteredTable.Clone();
            }
        }

        private DataTable ApplyDatapointFilter(DataTable table)
        {
            return ApplyDatapointFilter(table, false);
        }

        /// <summary>
        /// Apply Datapoint filter, mean when somebody click on datapoint
        /// Note:datapointfilter contain filter in following format : "dimensionIndex*dimensionval*expressionindex*expressionval*clickevent"
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private DataTable ApplyDatapointFilter(DataTable table, bool applyPointFilter)
        {
            // return if table is null or empty
            if (table == null)
            {
                return table;
            }
            if (table.Rows.Count <= 0)
            {
                return table.Clone();
            }

            //Converts datapointfilter string into string array. 
            string[] filterArray = DatapointFilter.Split(new string[] { "*" }, StringSplitOptions.None);
            string filterString = null;

            //Get current dimension value 
            if (filterArray.Length > 1)
            {
                filterString = HttpContext.Current.Server.UrlDecode(filterArray[1]);
            }

            //Checks filter contains expression value or not
            //Creates expressionfilter using datapointfilter
            string expressionFilter = string.Empty;
            //Dont Field Data based on group value
            if (applyPointFilter && filterArray.Length > 3)
            {
                int eIndex = 0;
                int.TryParse(filterArray[2], out eIndex);
                ChartExpression expression = ChartSetting.Expressions[eIndex];

                expressionFilter = expression.ExpressionFormula;
                if (!string.IsNullOrEmpty(expression.GroupByField))
                {
                    DataColumn eColumn = null;
                    if (table.Columns.Contains(expression.GroupByField))
                    {
                        eColumn = table.Columns[expression.GroupByField];
                    }

                    if (eColumn != null)
                    {
                        if (filterArray[3] == "None")
                        {
                            filterArray[3] = string.Empty;
                        }

                        string filterExpn = string.Empty;
                        if (eColumn.DataType == typeof(string) || eColumn.DataType == typeof(DateTime))
                        {
                            filterExpn = string.Format("{0}='{1}'", expression.GroupByField, HttpContext.Current.Server.UrlDecode(filterArray[3]));
                        }
                        else
                        {
                            filterExpn = string.Format("{0}={1}", expression.GroupByField, filterArray[3]);
                        }

                        if (filterExpn != string.Empty && expressionFilter != string.Empty)
                        {
                            expressionFilter += " And ";
                        }
                        if (filterExpn != string.Empty)
                        {
                            expressionFilter += " " + filterExpn;
                        }
                    }
                }
            }

            //Checks dimension filter is not null and chart has dimensions
            if (filterString != null && ChartSetting.Dimensions.Count > 0)
            {
                dimensionFilter = string.Format("{0}", filterString);
                int dIndex = 0;
                int.TryParse(filterArray[0], out dIndex);

                string[] filterStringArray = filterString.Split(new string[] { ">" }, StringSplitOptions.None);
                if (filterStringArray.Length > 1)
                {
                    for (int i = 0; i <= dIndex; i++)
                    {
                        filterString = filterStringArray[i];
                        ChartDimension dimension = ChartSetting.Dimensions[i];
                        table = GetDimensionFilteredData(table, filterString, dimension);
                    }
                }
                else if (filterStringArray.Length == 1)
                {
                    filterString = filterStringArray[0];
                    ChartDimension dimension = ChartSetting.Dimensions[dIndex];
                    table = GetDimensionFilteredData(table, filterString, dimension);
                }
            }

            //Apply Expression filter after dimension filter
            if (table != null && !string.IsNullOrEmpty(expressionFilter))
            {
                ExpressionCalc expCalc = new ExpressionCalc(table, false);
                DataRow[] rows = expCalc.FactTable.Select(expCalc.ResolveFunctions(context, expressionFilter));
                if (rows.Length > 0)
                {
                    table = rows.CopyToDataTable();
                }
                else
                {
                    table = table.Clone();
                }
            }

            return table;
        }


        private DataTable GetDimensionFilteredData(DataTable table, string filterString, ChartDimension dimension)
        {
            if (table.Columns.Contains(dimension.SelectedField))
            {
                DataColumn column = table.Columns[dimension.SelectedField];
                DataRow[] rows = new DataRow[0];

                //Checks datatype of dimension field
                if (column.DataType == typeof(string))
                {
                    //Filter filterstring has value "none" then search for empty, null data otherwise search filtersting data
                    if (filterString.ToLower() == "none")
                    {
                        rows = table.Select(string.Format("{0} ='{1}' or {0} ='' or {0} is null", dimension.SelectedField, filterString));
                    }
                    else
                    {
                        rows = table.Select(string.Format("{0} ='{1}'", dimension.SelectedField, HttpContext.Current.Server.UrlDecode(filterString)));
                    }
                }
                else if (column.DataType == typeof(DateTime))
                {
                    //In case of datetime apply datapoint fitler according to dateviewtype (year, month, days)

                    if (dimension.DateViewType == "year")
                    {
                        int year = dateForDateView.Year;
                        if (!int.TryParse(filterString, out year))
                            year = dateForDateView.Year;

                        dateForDateView = new DateTime(year, 1, 1);
                        rows = table.Select(string.Format("{0} is not null", dimension.SelectedField)).Where(x => x.Field<DateTime>(dimension.SelectedField).Year == year).ToArray();
                    }
                    else if (dimension.DateViewType == "month")
                    {
                        int year = dateForDateView.Year;
                        int month = dateForDateView.Month;
                        DateTime datefilter = Convert.ToDateTime(filterString);
                        if (datefilter != DateTime.MinValue)
                        {
                            year = datefilter.Year;
                            month = datefilter.Month;
                        }

                        dateForDateView = new DateTime(year, month, 1);
                        rows = table.Select(string.Format("{0} is not null", dimension.SelectedField)).Where(x => x.Field<DateTime>(dimension.SelectedField).Year == year && x.Field<DateTime>(dimension.SelectedField).Month == month).ToArray();
                    }
                    else if (dimension.DateViewType == "day")
                    {

                        int year = dateForDateView.Year;
                        int month = dateForDateView.Month;
                        DateTime datefilter = Convert.ToDateTime(filterString);
                        int day = dateForDateView.Day;
                        if (datefilter != DateTime.MinValue)
                        {
                            year = datefilter.Year;
                            month = datefilter.Month;
                            day = datefilter.Day;
                        }

                        dateForDateView = new DateTime(year, month, day);
                        rows = table.Select(string.Format("{0} is not null", dimension.SelectedField)).Where(x => x.Field<DateTime>(dimension.SelectedField).Date == dateForDateView.Date).ToArray();
                    }
                }
                else
                {
                    rows = table.Select(string.Format("{0}={1}", dimension.SelectedField, filterString));
                }

                if (rows.Length > 0)
                {
                    table = rows.CopyToDataTable();
                }
                else
                {
                    table = table.Clone();
                }
            }

            return table;
        }
        /// <summary>
        /// Pick (N) Data Points based on specified Expression and sort the axis based on selection
        /// </summary>
        /// <param name="datapointTable"></param>
        private DataTable PickDataPoints(DataTable datapointTable)
        {
            if (dimensionIndex < 0 && ChartSetting.Dimensions.Count == 0)
            {
                return datapointTable;
            }

            ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
            if (datapointTable == null || datapointTable.Rows.Count <= 0 || dimension == null)
            {
                return datapointTable;
            }

            if (dimension.PickTopDataPoint <= 0 && !dimension.EnableSorting)
                return datapointTable;

            DataRow[] dr = datapointTable.Select();
            if (dimension.PickTopDataPoint > 0)
            {
                if (dimension.DataPointExpression <= 0)
                {
                    if (!datapointTable.Columns.Contains(dimension.Title))
                    {
                        ULog.WriteLog(string.Format("Dashboard: ({0}), Pick (N)Point having wrong dimension field.", ChartSetting.ContainerTitle));
                    }
                    else
                    {
                        if (dimension.DataPointOrder != null && dimension.DataPointOrder.ToLower() == "ascending")
                            dr = dr.OrderBy(x => x[dimension.Title]).Take(dimension.PickTopDataPoint).ToArray();
                        else
                            dr = dr.OrderByDescending(x => x[dimension.Title]).Take(dimension.PickTopDataPoint).ToArray();
                        dr = dr.Take(dimension.PickTopDataPoint).ToArray();
                    }
                }
                else if (ChartSetting.Expressions.Count >= dimension.DataPointExpression)
                {
                    ChartExpression expression = ChartSetting.Expressions[dimension.DataPointExpression - 1];
                    if (!datapointTable.Columns.Contains(expression.Title))
                    {
                        ULog.WriteLog(string.Format("Dashboard: ({0}), Pick (N)Point having wrong expression field.", ChartSetting.ContainerTitle));
                    }
                    else
                    {
                        if (dimension.DataPointOrder != null && dimension.DataPointOrder.ToLower() == "ascending")
                            dr = dr.OrderBy(x => x[expression.Title]).Take(dimension.PickTopDataPoint).ToArray();
                        else
                            dr = dr.OrderByDescending(x => x[expression.Title]).Take(dimension.PickTopDataPoint).ToArray();
                    }
                }

                if (datapointTable.Columns.Contains(dimension.Title))
                    dr = dr.OrderBy(x => x[dimension.Title]).ToArray();
            }

            if (dimension.EnableSorting)
            {
                string orderBy = string.Empty;
                if (dimension.OrderByExpression <= 0)
                {
                    orderBy = dimension.Title;
                }
                else if (ChartSetting.Expressions.Count >= dimension.OrderByExpression)
                {
                    ChartExpression orderExpression = ChartSetting.Expressions[dimension.OrderByExpression - 1];
                    orderBy = orderExpression.Title;
                }

                if (!datapointTable.Columns.Contains(orderBy))
                {
                    ULog.WriteLog(string.Format("Dashboard: ({0}), column is not existing to order dashboard.", ChartSetting.ContainerTitle));
                }
                else
                {
                    if (dimension.OrderBy != null && dimension.OrderBy.ToLower() == "descending")
                    {
                        if (!string.IsNullOrEmpty(orderBy))
                            dr = dr.OrderByDescending(x => x[orderBy]).ToArray();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(orderBy))
                            dr = dr.AsEnumerable().OrderBy(x => x[orderBy]).ToArray();
                    }
                }


            }

            DataTable data = dr.CopyToDataTable();
            foreach (DataColumn column in datapointTable.Columns)
            {
                if (column.ExtendedProperties != null && column.ExtendedProperties.Count > 0)
                {
                    foreach (var item in column.ExtendedProperties.Keys)
                    {
                        data.Columns[column.ColumnName].ExtendedProperties.Add(item, column.ExtendedProperties[item]);
                    }
                }
            }

            return data;
        }
        /// <summary>
        /// Fill datapoints in datapointtable
        /// </summary>
        /// <param name="filteredData"></param>
        /// <param name="datapointTable"></param>
        private void FillDataPointsTable(DataTable filteredData, DataTable datapointTable)
        {
            if (dimensionIndex >= 0 && ChartSetting.Dimensions.Count > 0)
            {
                ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];

                if (filteredData == null || filteredData.Rows.Count <= 0)
                {
                    return;
                }

                //Get columns index if dimension field exist in table
                int columnIndex = filteredData.Columns.IndexOf(filteredData.Columns[dimension.SelectedField]);
                if (columnIndex < 0)
                {
                    return;
                }


                //Create lookup object which group by the table based on dimension field
                ILookup<object, DataRow> lookups = null;

                if (filteredData.Columns[columnIndex].DataType == typeof(DateTime))
                {
                    //if dimension field is of datetime type the do lookup by year, month , day 
                    try
                    {
                        if (dimension.DateViewType == "year")
                        {
                            lookups = filteredData.Select(string.Format("{0} is not null", dimension.SelectedField)).OrderBy(x => x.Field<DateTime>(dimension.SelectedField)).ToLookup(x => (object)x.Field<DateTime>(dimension.SelectedField).Year);
                        }
                        else if (dimension.DateViewType == "month")
                        {
                            lookups = filteredData.Select(string.Format("{0} is not null", dimension.SelectedField)).OrderBy(x => x.Field<DateTime>(dimension.SelectedField)).ToLookup(x => (object)string.Format("{0}/{1}", x.Field<DateTime>(dimension.SelectedField).Year, x.Field<DateTime>(dimension.SelectedField).Month.ToString("d2")));
                        }
                        else
                        {
                            lookups = filteredData.Select(string.Format("{0} is not null", dimension.SelectedField)).OrderBy(x => x.Field<DateTime>(dimension.SelectedField)).ToLookup(x => (object)x.Field<DateTime>(dimension.SelectedField).ToString("MM/dd/yyyy"));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    lookups = filteredData.AsEnumerable().ToLookup(x => x.ItemArray[columnIndex]);
                }

                //Pick top rows if pickstop exist if not exist then pick all points
                List<object> topPoints = new List<object>();
                //if (dimension.PickTopDataPoint > 0)
                //{
                //    topPoints = PickTopDimensionPoint(filteredData, lookups);
                //}
                //if (topPoints.Count <= 0)
                //{
                //    topPoints = lookups.Select(x => x.Key).ToList();
                //}

                topPoints = lookups.Select(x => x.Key).ToList();

                for (int j = 0; j < topPoints.Count; j++)
                {
                    if (topPoints[j] == null || Convert.ToString(topPoints[j]) == string.Empty)
                    {
                        topPoints[j] = "None";
                    }
                }
                //   topPoints = topPoints.OrderBy(x => x).ToList();

                //Order datapoint 
                if (dimension.PickTopDataPoint <= 0)
                {
                    //if (dimension.OrderBy.ToLower() == "descending")
                    //{
                    //    if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && dimension.DateViewType == "month")
                    //        topPoints = topPoints.OrderByDescending(x => Convert.ToDateTime(x)).ToList();                        
                    //    else if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && dimension.DateViewType == "day")
                    //        topPoints = topPoints.OrderBy(x => DateTime.ParseExact(UGITUtility.ObjectToString(x), "MM-dd-yyyy", CultureInfo.InvariantCulture)).ToList();
                    //    else
                    //        topPoints = topPoints.OrderByDescending(x => x).ToList();
                    //}
                    //else
                    //{
                    //    if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && dimension.DateViewType == "month" )                      
                    //        topPoints = topPoints.OrderBy(x =>UGITUtility.StringToDateTime(UGITUtility.ObjectToString(x))).ToList();
                    //    else if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && dimension.DateViewType == "day")
                    //        topPoints = topPoints.OrderBy(x => DateTime.ParseExact(UGITUtility.ObjectToString(x),"MM-dd-yyyy",CultureInfo.InvariantCulture)).ToList();                                                                                    
                    //    else
                    //        topPoints = topPoints.OrderBy(x => x.ToString()).ToList();
                    //}

                    if (dimension.OrderBy.ToLower() == "descending")
                    {
                        if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && (dimension.DateViewType == "month" || dimension.DateViewType == "day"))
                        {
                            topPoints = topPoints.OrderByDescending(x => Convert.ToDateTime(x)).ToList();
                        }
                        else
                            topPoints = topPoints.OrderByDescending(x => x).ToList();
                    }
                    else
                    {
                        if (filteredData.Columns[columnIndex].DataType == typeof(DateTime) && (dimension.DateViewType == "month" || dimension.DateViewType == "day"))
                        {
                            topPoints = topPoints.OrderBy(x => Convert.ToDateTime(x)).ToList();
                        }
                        else
                            topPoints = topPoints.OrderBy(x => x.ToString()).ToList();
                    }
                }

                foreach (var key in topPoints)
                {
                    var item = lookups[key];
                    if (Convert.ToString(key) == "None")
                    {
                        item = lookups[System.DBNull.Value];
                        if (item.Count() == 0)
                            item = lookups[string.Empty];
                    }

                    DataRow[] dimensionFilterRows = item.ToArray();
                    //create new datapoint row
                    string title = string.IsNullOrEmpty(Convert.ToString(key)) ? "None" : Convert.ToString(key);
                    //Creates datapoint for all expression
                    CreateDataPoint(filteredData, datapointTable, title, dimensionFilterRows, dimension);
                }
            }
            else if (filteredData != null && ChartSetting.Expressions.Count > 0 && filteredData.Rows.Count > 0)
            {
                //Creates datapoint for all expression
                CreateDataPoint(filteredData, datapointTable, "", filteredData.Select());
            }
        }

        /// <summary>
        /// Creates datapoint for all expression
        /// </summary>
        /// <param name="filteredData"></param>
        /// <param name="datapointTable"></param>
        /// <param name="Title"></param>
        /// <param name="filterRows"></param>
        private void CreateDataPoint(DataTable filteredData, DataTable datapointTable, string Title, DataRow[] filterRows, ChartDimension dimension = null)
        {
            DataRow datapointRow = datapointTable.NewRow();
            datapointRow[InternalTitleColumn] = Title;
            Title = UGITUtility.RemoveIDsFromLookupString(Title);
            datapointRow[1] = Title;
            for (int i = 2; i < datapointTable.Columns.Count; i++)
            {
                DataColumn column = datapointTable.Columns[i];
                double cumulativeSum = 0F;
                if (dimension != null && dimension.IsCumulative)
                {
                    int rowCount = datapointTable.Rows.Count;
                    if (rowCount > 0)
                        cumulativeSum = datapointTable.Rows[rowCount - 1][column] is DBNull ? 0 : Convert.ToDouble(datapointTable.Rows[rowCount - 1][column]);
                }

                //get series expression index and name
                int srIndex = 0;
                int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out srIndex);
                string srName = column.ColumnName;

                if (filterRows.Length <= 0 || ChartSetting.Expressions.Count <= srIndex)
                {
                    //Creates empty point if filter data is not available for current dimension point or series is empty.
                    datapointRow[column] = cumulativeSum + 0;
                }
                else
                {
                    ChartExpression expression = ChartSetting.Expressions[srIndex];
                    DataRow[] dashboardfilterRows = filterRows;

                    if (filterRows.Length > 0 && !string.IsNullOrEmpty(expression.ExpressionFormula))
                    {
                        DataTable dashboardFilterRowsTable = filterRows.CopyToDataTable();
                        ExpressionCalc exCalc = new ExpressionCalc(dashboardFilterRowsTable, true);
                        string filter = exCalc.ResolveFunctions(context, expression.ExpressionFormula);
                        filterRows = exCalc.FactTable.Select();
                        dashboardfilterRows = exCalc.FactTable.Select(filter);
                        if (dashboardfilterRows.Length <= 0)
                        {
                            datapointRow[column] = cumulativeSum + 0;
                            continue;
                        }
                    }
                    string expressionGroupField = expression.GroupByField;
                    DataTable seriesFilterTable = null;
                    if (!string.IsNullOrEmpty(expressionGroupField) && filteredData.Columns.Contains(expressionGroupField))
                    {
                        int groupColumnIndex = filteredData.Columns.IndexOf(filteredData.Columns[expressionGroupField]);
                        DataColumn groupColumn = filteredData.Columns[expressionGroupField];
                        DataRow[] expressionGroupLookups = new DataRow[0];
                        if (srName.ToLower() == "none")
                        {
                            expressionGroupLookups = filterRows.Where(x => Convert.ToString(x.Field<object>(groupColumn.ColumnName)) == null || Convert.ToString(x.Field<object>(groupColumn.ColumnName)) == string.Empty).ToArray();
                        }
                        else
                        {
                            expressionGroupLookups = filterRows.Where(x => Convert.ToString(x.Field<object>(groupColumn.ColumnName)) == srName).ToArray();
                        }

                        if (expressionGroupLookups.Length <= 0)
                        {
                            //set datapoint point is empty
                            datapointRow[column] = cumulativeSum + 0;
                            continue;
                        }
                        else
                        {
                            seriesFilterTable = expressionGroupLookups.CopyToDataTable();
                        }
                    }
                    else
                    {
                        seriesFilterTable = dashboardfilterRows.CopyToDataTable();
                    }

                    if (seriesFilterTable == null || seriesFilterTable.Rows.Count <= 0)
                    {
                        //set datapoint point is empty
                        datapointRow[column] = cumulativeSum + 0;
                        continue;
                    }
                    else
                    {
                        object aggregateResult = string.Empty;
                        if ((new string[] { "", "count", "sum", "avg", "max", "min" }).FirstOrDefault(x => x.ToLower() == expression.Operator.ToLower()) != null)
                        {
                            //Resolve expression and calculate date according to expression formula
                            GetExpressionFilteredData(seriesFilterTable, expression);

                            //Apply operator to aggregate the value
                            DataRow[] sRows = seriesFilterTable.Select();

                            if (sRows.Length > 0)
                            {
                                if (!string.IsNullOrWhiteSpace(expression.Operator))
                                {
                                    aggregateResult = GetAggregateData(sRows, expression.Operator, "expressionData");
                                }
                                else
                                {
                                    string columnStr = expression.FunctionExpression.Replace("[", string.Empty).Replace("]", string.Empty);
                                    if (filteredData != null && filteredData.Columns.Contains(columnStr) && filteredData.Columns[columnStr].DataType.Name == "DateTime")
                                    {
                                        if (!string.IsNullOrWhiteSpace(sRows[0]["expressionData"].ToString()) || sRows[0]["expressionData"] != DBNull.Value)
                                            aggregateResult = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(sRows[0]["expressionData"]), false);
                                    }
                                    else
                                    {
                                        aggregateResult = Convert.ToString(sRows[0]["expressionData"]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            aggregateResult = GetAggregateData(seriesFilterTable.Select(), expression.Operator, expression.FunctionExpression);
                        }

                        if (string.IsNullOrEmpty(Convert.ToString(aggregateResult)))
                            aggregateResult = 0D;


                        //Put conversion of data if any.
                        if (aggregateResult is double)
                        {
                            if (!string.IsNullOrWhiteSpace(expression.LabelFormat))
                            {
                                if (expression.LabelFormat.ToLower() == "mintodays")
                                {
                                    double value = (double)aggregateResult;

                                    if (workingHoursInDays > 0)
                                    {
                                        value = value / (60 * workingHoursInDays);
                                    }
                                    else
                                    {
                                        value = value / (60 * 24);
                                    }
                                    aggregateResult = value;
                                }
                            }

                            aggregateResult = cumulativeSum + (double)aggregateResult;
                        }

                        //Add datapoint

                        datapointRow[column] = (aggregateResult);
                    }
                }
            }
            datapointTable.Rows.Add(datapointRow);
        }


        public DataTable PlotDxChartPoints(List<DataColumn> column, DataTable datapointsTable)
        {
            DataColumnCollection columns = datapointsTable.Columns;
            DataTable table = new DataTable("Result");
            if (datapointsTable == null && datapointsTable.Rows.Count <= 1)
            {
                //DevexChart.Series sr = dcpChart.Series[""];
                //sr.Points.Add(new DevexChart.SeriesPoint(0,0));
                return null;
            }
            if (ChartSetting.Dimensions.Count > 0)
            {
                foreach (DataRow datapoint in datapointsTable.Rows)
                {
                    DataRow row = table.NewRow();
                    for (int i = 2; i < columns.Count; i++)
                    {
                        DataColumn _column = columns[i];
                        DataColumn resultColumn = new DataColumn();
                        //DataRow resultRow;

                        int expressionIndex = 0;
                        int.TryParse(Convert.ToString(_column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                        ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                        object val = new object();
                        //bool isConvertCheck=true;
                        if (expression.Operator != string.Empty)
                        {
                            val = UGITUtility.StringToDouble(datapoint[_column.ColumnName]);
                            //isConvertCheck = false;
                        }
                        else
                        {
                            val = datapoint[_column.ColumnName];
                        }

                        //Calculate percentage if property is true;
                        if (expression.ShowInPercentage)
                        {
                            double totalCount = datapointsTable.AsEnumerable().Sum(x => x.Field<double>(_column.ColumnName));
                            val = Math.Round((UGITUtility.StringToDouble(val) / totalCount) * 100, 0);
                            //isConvertCheck = false;
                            resultColumn.ExtendedProperties["totalCount"] = totalCount;
                        }
                        if (table.Columns.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(expression.GroupByField) && !table.Columns.Contains(expression.GroupByField))
                            {
                                table.Columns.Add(expression.GroupByField);
                                // table.Columns.Add(expression.FunctionExpression);
                            }
                            if (!string.IsNullOrEmpty(expression.FunctionExpression) && !table.Columns.Contains(expression.FunctionExpression))
                            {
                                table.Columns.Add(expression.FunctionExpression);
                            }
                        }
                        else
                        {
                            table.Columns.Add(expression.GroupByField);
                            table.Columns.Add(expression.FunctionExpression);
                        }


                        if (!string.IsNullOrEmpty(val.ToString()) && val.ToString() != "0")
                        {
                            if (!string.IsNullOrWhiteSpace(expression.GroupByField))
                                row[expression.GroupByField] = datapoint[1].ToString();
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(expression.FunctionExpression))
                                    row[expression.FunctionExpression] = datapoint[1].ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(expression.FunctionExpression))
                                row[expression.FunctionExpression] = val.ToString();

                            //resultColumn.Caption = datapoint[1].ToString();
                            //resultColumn.ColumnName = datapoint[0].ToString();
                            //resultColumn.DefaultValue = val;
                            //resultColumn.ExtendedProperties["expression"] = expression;

                        }
                        object xVal = datapoint[1];
                        object internalXValue = datapoint[0];

                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        private void PlotDxChartPoints(DevexChart.Web.WebChartControl dcpChart, DataTable datapointsTable)
        {
            DataColumnCollection columns = datapointsTable.Columns;
            if (datapointsTable == null && datapointsTable.Rows.Count <= 1)
            {
                //DevexChart.Series sr = dcpChart.Series[""];
                //sr.Points.Add(new DevexChart.SeriesPoint(0,0));
                return;
            }

            if (ChartSetting.Dimensions.Count > 0)
            {
                foreach (DataRow datapoint in datapointsTable.Rows)
                {
                    for (int i = 2; i < columns.Count; i++)
                    {
                        DataColumn column = columns[i];


                        int expressionIndex = 0;
                        int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                        ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                        DevexChart.Series sr = dcpChart.Series[column.ColumnName];

                        object val = new object();
                        bool isConvertCheck = true;
                        if (expression.Operator != string.Empty)
                        {
                            val = UGITUtility.StringToDouble(datapoint[column.ColumnName]);
                            isConvertCheck = false;
                        }
                        else
                        {
                            val = datapoint[column.ColumnName];
                        }

                        //Calculate percentage if property is true;
                        if (expression.ShowInPercentage)
                        {
                            double totalCount = datapointsTable.AsEnumerable().Sum(x => x.Field<double>(column.ColumnName));
                            val = Math.Round((UGITUtility.StringToDouble(val) / totalCount) * 100, 0);
                            isConvertCheck = false;
                        }
                        //Add datapoint
                        object xVal = datapoint[1];
                        object internalXValue = datapoint[0];

                        if (val is DateTime)
                        {
                            sr.ValueScaleType = ScaleType.DateTime;
                            sr.Label.TextPattern = "{V:MMM-d-yyyy}";
                            //Code added by Inderjeet Kaur on 9 Sept 2022. When dates are plotted on the graph, the crosshhairlabel
                            //displays ":#,0.##" symbols instead of the date. This code will now display date in MMM-d-yyyy format.
                            sr.CrosshairLabelPattern = "{A}: {V:MMM-d-yyyy}";
                            isConvertCheck = false;
                        }

                        DatapointClickeEventType clickEvent = expression.DataPointClickEvent;
                        object dataPointDetailVal = internalXValue;
                        ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];

                        if (expression.DataPointClickEvent == DatapointClickeEventType.Inherit)
                        {
                            clickEvent = dimension.DataPointClickEvent;
                        }

                        int index = -1;
                        if (isConvertCheck)
                            val = UGITUtility.StringToInt(val);

                        //Incase of year view, add 01/01/ before year to set value date format
                        //Which help chart to show year format correctly
                        if (dimension.DateViewType == "year")
                            xVal = "01/01/" + xVal;

                        index = sr.Points.Add(new SeriesPoint(xVal, val));
                        if (dimension.DateViewType == "month")
                        {
                            sr.CrosshairLabelPattern = "{A:MMM-yy}: {V:#,0.##}";
                        }
                        else if (dimension.DateViewType == "day")
                        {
                            sr.CrosshairLabelPattern = "{A:dd-MMM-yy}: {V:#,0.##}";
                        }
                        else if (dimension.DateViewType == "year")
                        {
                            sr.CrosshairLabelPattern = "{A:yyyy}: {V:#,0.##}";
                        }

                        string dimsVal = Convert.ToString(dataPointDetailVal);
                        if (!string.IsNullOrWhiteSpace(dimensionFilter))
                            dimsVal = string.Format("{0}>{1}", dimensionFilter, dimsVal);

                        string argument = string.Format("{0}*{1}*{2}*{3}*{4}*{5}::{4}", dimensionIndex, (dimsVal), expressionIndex, (column.ColumnName), (int)clickEvent, (internalXValue.ToString()));

                        sr.Points[index].Annotations.AddTextAnnotation("tag", argument).Visible = false;

                    }
                }

            }
            else if (datapointsTable.Rows.Count > 0)
            {
                //Series sr = cPChart.Series[0];
                //cPChart.Legends.Clear();
                //for (int i = 1; i < cPChart.Series.Count; i++)
                //{
                //    cPChart.Series[i].IsVisibleInLegend = false;
                //    cPChart.Series.Remove(cPChart.Series[i]);
                //}

                //for (int j = 2; j < columns.Count; j++)
                //{
                //    DataColumn column = columns[j];

                //    int expressionIndex = 0;
                //    int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                //    ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                //    double val = uHelper.StringToDouble(datapointsTable.Rows[0][column]);
                //    int index = sr.Points.AddXY(column.ColumnName, val);

                //    if (val == 0)
                //    {
                //        sr.Points[index].MarkerStyle = MarkerStyle.None;
                //    }

                //    //Set tooltip
                //    string toolTip = string.Format("{0}: {1}", column.ColumnName, uHelper.FormatNumber(uHelper.StringToDouble(val), string.Empty));
                //    if (!string.IsNullOrWhiteSpace(expression.LabelFormat))
                //    {
                //        toolTip = string.Format("{0}: {1}", column.ColumnName, uHelper.FormatNumber(uHelper.StringToDouble(val), expression.LabelFormat));
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(expression.LabelText))
                //        {
                //            toolTip = string.Format("{0}: {1}", column.ColumnName, expression.LabelText.Replace("$Exp$", uHelper.FormatNumber(uHelper.StringToDouble(val), string.Empty)));
                //        }
                //    }


                //    sr.Points[index].LegendText = string.Format("{0} ({1}{2})", column.ColumnName, val, expression.ShowInPercentage ? "%" : string.Empty);
                //    if (expression.DataPointClickEvent == DatapointClickeEventType.Detail)
                //    {
                //        sr.Points[index].PostBackValue = string.Format("javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})", dimensionIndex, string.Empty, expressionIndex, HttpContext.Current.Server.UrlEncode(column.ColumnName), (int)expression.DataPointClickEvent, string.Empty);
                //        sr.Points[index].MapAreaAttributes = string.Format("onclick=\"javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})\"", dimensionIndex, string.Empty, expressionIndex, HttpContext.Current.Server.UrlEncode(column.ColumnName), (int)expression.DataPointClickEvent, string.Empty);
                //    }
                //}

            }

            //Only in case of chart type: line, spline, stepline. if Series has less then equal to 2 point then chanage chart type to column
            //List<Series> seriesList = cPChart.Series.Where(x => x.ChartType == SeriesChartType.Line || x.ChartType == SeriesChartType.Spline || x.ChartType == SeriesChartType.StepLine).ToList();
            //foreach (Series s in seriesList)
            //{
            //    if (s.Points.Count <= 2)
            //    {
            //        s.ChartType = SeriesChartType.Bubble;
            //        s.SetCustomProperty("BubbleMaxSize", "7");
            //        s.SetCustomProperty("BubbleMinSize", "7");
            //    }
            //}

            //foreach (Legend legend in cPChart.Legends)
            //{
            //    legend.Font = new Font(legendFont, legendFontSize - 1, FontStyle.Regular, GraphicsUnit.Point);
            //}
        }


        private void GetExpressionFilteredData(DataTable filteredTable, ChartExpression exp)
        {
            //Adds columns expressionData if filtereddata exist
            if (filteredTable != null && !filteredTable.Columns.Contains("expressionData"))
            {
                if (exp.Operator != string.Empty)
                    filteredTable.Columns.Add("expressionData", typeof(double));
                else
                    filteredTable.Columns.Add("expressionData", typeof(string));
            }

            //returns if filtered data not exist or expressionformula not exist
            if (filteredTable == null || filteredTable.Rows.Count <= 0 || exp == null || string.IsNullOrEmpty(exp.FunctionExpression))
            {
                return;
            }

            try
            {
                if (exp.FunctionExpression.ToLower().StartsWith("daysdiff") || exp.FunctionExpression.ToLower().StartsWith("yeardiff"))
                {

                    System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex("\\(.*?\\)");
                    System.Text.RegularExpressions.Match match = regExp.Match(exp.FunctionExpression);
                    if (exp.FunctionExpression.ToLower().StartsWith("daysdiff"))
                    {

                        string[] fields = match.Value.Split(',');
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fields[i] = fields[i].Replace("[", string.Empty).Replace("]", string.Empty);
                        }

                        if (fields.Length > 1)
                        {
                            fields[0] = fields[0].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                            fields[1] = fields[1].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                            foreach (DataRow row in filteredTable.Rows)
                            {
                                row["expressionData"] = Math.Round((((DateTime)row[fields[0]]).Date.Subtract(((DateTime)row[fields[1]]).Date)).TotalDays, 1);
                            }
                        }
                    }
                    if (exp.FunctionExpression.ToLower().StartsWith("yeardiff"))
                    {

                        string[] fields = match.Value.Split(',');
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fields[i] = fields[i].Replace("[", string.Empty).Replace("]", string.Empty);
                        }
                        if (fields.Length > 1)
                        {
                            fields[0] = fields[0].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                            fields[1] = fields[1].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                            foreach (DataRow row in filteredTable.Rows)
                            {
                                row["expressionData"] = ((DateTime)row[fields[0].Trim()]).Date.Year - ((DateTime)row[fields[1].Trim()]).Date.Year;
                            }
                        }
                    }
                }
                else
                {
                    filteredTable.Columns["expressionData"].Expression = exp.FunctionExpression;
                }
            }
            catch
            {
                filteredTable.Columns["expressionData"].Expression = "0";
            }
        }

        private double GetAggregateData(DataRow[] filteredRows, string aggregateOperator, String aggregateColumn)
        {
            double result = 0D;
            if (filteredRows.Length <= 0 || string.IsNullOrEmpty(aggregateOperator) || string.IsNullOrEmpty(aggregateColumn))
            {
                return result;
            }

            DataTable filteredTable = filteredRows.CopyToDataTable();
            try
            {
                if (aggregateOperator.ToLower() == "variance")
                {
                    List<string> columns = UGITUtility.ConvertStringToList(aggregateColumn, ",");
                    if (columns.Count == 3)
                    {
                        object var1 = 0D, var2 = 0D, var3 = 0D;
                        var1 = filteredTable.Compute(string.Format("sum({0})", columns[0]), string.Format("{0} is not null", columns[0]));
                        var2 = filteredTable.Compute(string.Format("sum({0})", columns[1]), string.Format("{0} is not null", columns[1]));
                        if (columns[0] == columns[2])
                            var3 = var1;
                        else if (columns[1] == columns[2])
                            var3 = var2;
                        else
                            var3 = filteredTable.Compute(string.Format("sum({0})", columns[2]), string.Format("{0} is not null", columns[2]));

                        if (var1 is System.DBNull) var1 = 0D;
                        if (var2 is System.DBNull) var2 = 0D;
                        if (var3 is System.DBNull) var3 = 0D;


                        if (Convert.ToDouble(var3) == 0)
                            result = 0D;
                        else
                        {
                            result = ((Convert.ToDouble(var1) - Convert.ToDouble(var2)) / Convert.ToDouble(var3)) * 100;
                        }
                    }
                }
                else
                {

                    //check datediff 
                    //if (filteredTable != null && !filteredTable.Columns.Contains("expressionData"))
                    //{
                    //    if (aggregateOperator != string.Empty)
                    //        filteredTable.Columns.Add("expressionData", typeof(double));
                    //    else
                    //        filteredTable.Columns.Add("expressionData", typeof(string));
                    //}

                    //if (aggregateColumn.ToLower().StartsWith("daysdiff"))
                    //{
                    //    System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex("\\(.*?\\)");
                    //    System.Text.RegularExpressions.Match match = regExp.Match(aggregateColumn);
                    //    string[] fields = match.Value.Split(',');
                    //    for (int i = 0; i < fields.Length; i++)
                    //    {
                    //        fields[i] = fields[i].Replace("[", string.Empty).Replace("]", string.Empty);
                    //    }

                    //    if (fields.Length > 1)
                    //    {
                    //        fields[0] = fields[0].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                    //        fields[1] = fields[1].Replace("(", string.Empty).Replace(")", string.Empty).Trim();
                    //        foreach (DataRow row in filteredTable.Rows)
                    //        {
                    //            row["expressionData"] = Math.Round((((DateTime)row[fields[0]]).Date.Subtract(((DateTime)row[fields[1]]).Date)).TotalDays, 1);
                    //        }
                    //    }
                    //}
                    object obj = filteredTable.Compute(string.Format("{0}({1})", aggregateOperator.ToLower(), aggregateColumn), string.Format("{0} is not null", aggregateColumn));
                    if (obj is System.DBNull)
                        obj = 0D;

                    result = Convert.ToDouble(obj);
                }

                return Math.Round(result, 1);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return 0D;
            }
        }


        private DevexChart.Web.WebChartControl DxBasicChartSetting()
        {

            DevexChart.Web.WebChartControl dcpChart = new DevexChart.Web.WebChartControl();

            if (dcpChart.Diagram == null)
                dcpChart.Diagram = new DevexChart.XYDiagram();
            DevexChart.XYDiagram diagram = dcpChart.Diagram as DevexChart.XYDiagram;


            if (ChartSetting.BorderWidth != 0)
            {
                dcpChart.BorderOptions.Thickness = ChartSetting.BorderWidth;
            }
            else
            {
                dcpChart.BorderOptions.Thickness = 1;
            }

            dcpChart.BackColor = ColorTranslator.FromHtml("#E7EAFE");

            //set palette
            dcpChart.PaletteName = "Default";
            if (!string.IsNullOrWhiteSpace(ChartSetting.Palette) && ChartSetting.Palette.ToLower() != "none")
                dcpChart.PaletteName = ChartSetting.Palette;



            //set secondary Y-Axis
            if (ChartSetting.Expressions.Exists(x => x.YAsixType == ChartAxisType.Secondary))
            {
                DevexChart.SecondaryAxisY myAxisY = new DevexChart.SecondaryAxisY("my Y-Axis");
                myAxisY.WholeRange.Auto = true;
                ((DevexChart.XYDiagram)dcpChart.Diagram).SecondaryAxesY.Add(myAxisY);

            }


            //legend section setting
            if (!ChartSetting.HideLegend)
            {
                dcpChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                dcpChart.Legend.BackColor = System.Drawing.Color.Transparent;

                if (ChartSetting.HorizontalAlignment != null)
                    dcpChart.Legend.AlignmentHorizontal = (DevexChart.LegendAlignmentHorizontal)Enum.Parse(typeof(LegendHorizontalAlignment), ChartSetting.HorizontalAlignment);

                if (ChartSetting.VerticalAlignment != null)
                    dcpChart.Legend.AlignmentVertical = (DevexChart.LegendAlignmentVertical)Enum.Parse(typeof(LegendVerticalAlignment), ChartSetting.VerticalAlignment);

                if (ChartSetting.MaxHorizontalPercentage != null)
                    dcpChart.Legend.MaxHorizontalPercentage = UGITUtility.StringToDouble(ChartSetting.MaxHorizontalPercentage);

                if (ChartSetting.MaxVerticalPercentage != null)
                    dcpChart.Legend.MaxVerticalPercentage = UGITUtility.StringToDouble(ChartSetting.MaxVerticalPercentage);

                if (ChartSetting.Direction != null)
                    dcpChart.Legend.Direction = (DevexChart.LegendDirection)Enum.Parse(typeof(DlegendDirection), ChartSetting.Direction);

            }
            else
            {
                dcpChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }

            diagram.AxisX.Color = Color.Gainsboro;
            diagram.AxisY.Color = Color.Gainsboro;
            diagram.AxisX.GridLines.MinorColor = Color.Gainsboro;
            diagram.AxisX.GridLines.Color = Color.Gainsboro;
            diagram.AxisY.GridLines.Color = Color.Gainsboro;

            diagram.AxisX.Label.TextColor = ColorTranslator.FromHtml("#565353");
            diagram.AxisY.Label.TextColor = ColorTranslator.FromHtml("#9DB9D9");

            diagram.AxisX.Label.Font = new Font(legendFont, axisXfont, FontStyle.Regular, GraphicsUnit.Point);
            diagram.AxisY.Label.Font = new Font(legendFont, axisYfont, FontStyle.Regular, GraphicsUnit.Point);




            //Check HideGrid property is on or off and set some chart appearance
            if (ChartSetting.HideGrid)
            {
                diagram.AxisX.GridLines.Visible = false;
                diagram.AxisX.Tickmarks.Visible = false;
                diagram.AxisX.Thickness = 1;
                diagram.AxisY.GridLines.Visible = false;
                diagram.AxisY.Tickmarks.Visible = false;
                diagram.AxisY.Label.Visible = false;
                diagram.AxisY.Thickness = 1;

                if (ViewType == 1)
                {
                    dcpChart.BackColor = ColorTranslator.FromHtml("#F8F8F8");
                }
            }

            //if (this.isPreview)
            //{
            //    string font = "Verdana";
            //    int fontSize = 8;
            //    diagram.AxisX.Label.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            //    diagram.AxisY.Label.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            //    diagram.AxisX.Label.Angle = -45;
            //}

            diagram.AxisY.NumericScaleOptions.MeasureUnit = DevexChart.NumericMeasureUnit.Billions;
            dcpChart.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
            dcpChart.CustomDrawAxisLabel += DcpChart_CustomDrawAxisLabel;
            dcpChart.CustomDrawSeriesPoint += DcpChart_CustomDrawSeriesPoint;

            return dcpChart;
        }

        private void DcpChart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.LabelText) && ChartSetting.Expressions != null)
            {
                int expIndex = UGITUtility.StringToInt(e.Series.ArgumentDataMember);
                if (ChartSetting.Expressions != null && ChartSetting.Expressions.Count >= expIndex)
                {
                    ChartExpression exp = ChartSetting.Expressions[expIndex];
                    if (!string.IsNullOrWhiteSpace(exp.LabelFormat))
                    {
                        e.LabelText = uHelper.FormatNumber(UGITUtility.StringToDouble(e.LabelText), exp.LabelFormat);
                    }
                }
            }
        }

        private void DcpChart_CustomDrawAxisLabel(object sender, CustomDrawAxisLabelEventArgs e)
        {
            if (e.Item.Axis is AxisX && ChartSetting.Dimensions != null && ChartSetting.Dimensions.Count > 0)
            {
                ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
                if (dimension.AxisLabelMaxLength > 0)
                {
                    e.Item.Text = UGITUtility.TruncateWithEllipsis(e.Item.Text, dimension.AxisLabelMaxLength, string.Empty, "..");
                }
            }
            else if (e.Item.Axis is AxisY || e.Item.Axis is SecondaryAxisY)
            {
                string format = string.Empty;
                foreach (ChartExpression cExpression in ChartSetting.Expressions)
                {
                    if (!string.IsNullOrWhiteSpace(cExpression.LabelFormat) && cExpression.YAsixType == ChartAxisType.Primary && e.Item.Axis is AxisY)
                    {
                        format = cExpression.LabelFormat;
                        break;
                    }
                    else if (!string.IsNullOrWhiteSpace(cExpression.LabelFormat) && cExpression.YAsixType == ChartAxisType.Secondary && e.Item.Axis is SecondaryAxisY)
                    {
                        format = cExpression.LabelFormat;
                        break;
                    }
                }

                e.Item.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.Item.AxisValue), format);
            }
        }
        private List<DataColumn> CreateChartSeries(DataTable datapointsTable)
        {
            DataColumnCollection columns = datapointsTable.Columns;
            List<DataColumn> dataColumns = new List<DataColumn>();
            for (int i = 2; i < columns.Count; i++)
            {
                dataColumns.Add(columns[i]);
            }
            //dataColumns = dataColumns.OrderBy(x => x.ColumnName).ToList();
            return dataColumns;
        }

        /// <summary>
        /// Create chart series based on column of datapoints table
        /// </summary>
        /// <param name="cPChart"></param>
        /// <param name="datapointsTable"></param>
        private void CreateChartSeries(DevexChart.Web.WebChartControl dcpChart, DataTable datapointsTable)
        {
            ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];

            List<DevexChart.Series> seriesList = new List<DevexChart.Series>();
            dcpChart.Series.Clear();
            //dcpChart.ro
            DataColumnCollection columns = datapointsTable.Columns;
            List<DataColumn> dataColumns = new List<DataColumn>();
            for (int i = 2; i < columns.Count; i++)
            {
                dataColumns.Add(columns[i]);
            }
            dataColumns = dataColumns.OrderBy(x => x.ColumnName).ToList();

            for (int i = 0; i < dataColumns.Count; i++)
            {
                DataColumn column = dataColumns[i];

                //Get Expression associated with each column
                int expressionIndex = 0;
                int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                if (!string.IsNullOrWhiteSpace(expression.LabelText))
                    expression.LabelText = expression.LabelText.Replace("$Exp$", "{V}");

                string chartType = expression.ChartType.ToLower();
                if (chartType == DevexChart.ViewType.Pie.ToString().ToLower() || chartType == DevexChart.ViewType.Funnel.ToString().ToLower() || chartType == DevexChart.ViewType.Doughnut.ToString().ToLower())
                {
                    expression.GroupByField = string.Empty;
                }
                AddSeries(dcpChart, expression, column.ColumnName, dimension);
            }


            if (dimension != null)
            {
                DevexChart.XYDiagram diagram = dcpChart.Diagram as DevexChart.XYDiagram;
                if (diagram != null)
                {
                    if (dimension.DateViewType == "month")
                    {
                        diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
                        diagram.AxisX.Label.TextPattern = "{V:MMM-yy}";

                    }
                    else if (dimension.DateViewType == "day")
                    {
                        diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Automatic;
                        // diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                        diagram.AxisX.Label.TextPattern = "{V:dd-MMM-yy}";
                    }
                    else if (dimension.DateViewType == "year")
                    {
                        diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Year;
                        diagram.AxisX.Label.TextPattern = "{V:yyyy}";
                    }

                    diagram.AxisX.Label.Angle = dimension.AxisLabelStyleAngle;
                }
            }
        }



        private void CreateDxChartSeriesUsingRow(DevexChart.Web.WebChartControl dcpChart, DataTable datapointsTable)
        {
            if (datapointsTable == null)
                return;

            List<DevexChart.Series> seriesList = new List<DevexChart.Series>();
            ChartExpression firstExpression = ChartSetting.Expressions.FirstOrDefault();
            //cPChart.Palette = (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), firstExpression.Palette); 
            //cPChart.Series.Clear();


            //dcpChart.PaletteName = firstExpression.Palette;
            dcpChart.Series.Clear();
            DataColumnCollection columns = datapointsTable.Columns;

            datapointsTable.DefaultView.Sort = string.Format("[{0}] asc", columns[1].ColumnName);
            datapointsTable = datapointsTable.DefaultView.ToTable();

            for (int i = 0; i < datapointsTable.Rows.Count; i++)
            {
                DataRow row = datapointsTable.Rows[i];
                string name = Convert.ToString(row[1]);

                AddSeries(dcpChart, firstExpression, name);
            }

            DevexChart.XYDiagram diagram = dcpChart.Diagram as DevexChart.XYDiagram;
            if (diagram != null && ChartSetting.Dimensions != null && ChartSetting.Dimensions.Count > 0 && ChartSetting.Dimensions[0].AxisLabelStyleAngle != 0)
            {
                diagram.AxisX.Label.Angle = ChartSetting.Dimensions[0].AxisLabelStyleAngle;
            }

        }

        private DevexChart.Series AddSeries(DevexChart.Web.WebChartControl dcpChart, ChartExpression expression, string name, ChartDimension dimension = null)
        {
            DevexChart.Series sers = new DevexChart.Series();
            sers.Name = name;
            //sers.Name = name;

            sers.ArgumentDataMember = ChartSetting.Expressions.IndexOf(expression).ToString();

            string chartType = expression.ChartType.ToLower();
            if (string.IsNullOrWhiteSpace(chartType))
                chartType = "bar";

            string legendName = name;
            if (dimension != null && dimension.LegendTxtMaxLength > 0)
                legendName = UGITUtility.TruncateWithEllipsis(legendName, dimension.LegendTxtMaxLength, string.Empty, "..");

            sers.LegendText = legendName;
            switch (chartType)
            {
                case "bar":
                case "column":
                    {
                        sers.ChangeView(DevexChart.ViewType.Bar);
                    }
                    break;
                case "stackedbar":
                case "stackedcolumn":
                    {
                        sers.ChangeView(DevexChart.ViewType.StackedBar);
                    }
                    break;
                case "line":
                    {
                        sers.ChangeView(DevexChart.ViewType.Line);
                    }
                    break;
                case "spline":
                    {
                        sers.ChangeView(DevexChart.ViewType.Spline);
                    }
                    break;
                case "stepline":
                    {
                        sers.ChangeView(DevexChart.ViewType.StepLine);
                    }
                    break;
                case "pie":
                    {
                        sers.ChangeView(DevexChart.ViewType.Pie);
                    }
                    break;
                case "piramid":
                    {
                        sers.ChangeView(DevexChart.ViewType.Pie);
                    }
                    break;
                case "funnel":
                    {
                        sers.ChangeView(DevexChart.ViewType.Funnel);
                    }
                    break;
                case "doughnut":
                    {
                        sers.ChangeView(DevexChart.ViewType.Doughnut);
                    }
                    break;
                default:
                    {
                        sers.ChangeView(DevexChart.ViewType.Bar);
                    }
                    break;
            }

            dcpChart.Series.Add(sers);

            //set label 
            if (expression.HideLabel == true)
            {
                sers.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                sers.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            }
            sers.Label.TextPattern = "{V}";



            DevExpress.XtraCharts.XYDiagram diagram = dcpChart.Diagram as DevExpress.XtraCharts.XYDiagram;
            if (expression.YAsixType == ChartAxisType.Secondary &&
                diagram.SecondaryAxesY != null && diagram.SecondaryAxesY.Count > 0)
            {
                ((XYDiagramSeriesViewBase)sers.View).AxisY = diagram.SecondaryAxesY[0];
            }

            DevexChart.ViewType vt = DevExpress.XtraCharts.Native.SeriesViewFactory.GetViewType(sers.View);
            if (vt == DevexChart.ViewType.Bar)
            {
                diagram.Rotated = true;
                if (expression.ChartType == "Column")
                    diagram.Rotated = false;

                ((DevexChart.BarSeriesView)sers.View).BarWidth = 0.3;
                if (expression.ChartLevelProperties.Exists(x => x.Key == "barLablPosition"))
                {
                    string barLablPosition = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "barLablPosition").Value);
                    DevexChart.BarSeriesLabelPosition barLabelPosition = DevexChart.BarSeriesLabelPosition.Auto;
                    Enum.TryParse<DevexChart.BarSeriesLabelPosition>(barLablPosition, out barLabelPosition);

                    ((DevexChart.BarSeriesLabel)sers.Label).Position = barLabelPosition;
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "barLabelOrientation"))
                {
                    string barLabelOrientation = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "barLabelOrientation").Value);

                    DevexChart.TextOrientation textOrientation = DevexChart.TextOrientation.BottomToTop;
                    Enum.TryParse<DevexChart.TextOrientation>(barLabelOrientation, out textOrientation);
                    ((DevexChart.BarSeriesLabel)sers.Label).TextOrientation = textOrientation;
                }
            }
            else if (vt == DevexChart.ViewType.SideBySideFullStackedBar)
            {
                if (expression.ChartLevelProperties.Exists(x => x.Key == "sideBySideFullStackedBarValAsPertge"))
                {
                    bool isSideBySideFullStackedBarValAsPertge = UGITUtility.StringToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "sideBySideFullStackedBarValAsPertge").Value);

                    sers.Label.TextPattern = isSideBySideFullStackedBarValAsPertge ? "{A}: {VP:P0}" : "{A}";
                }
            }
            else if (vt == DevexChart.ViewType.Doughnut)
            {
                if (expression.ChartLevelProperties.Exists(x => x.Key == "DNLblPercentage"))
                {
                    bool IsDNLblPercentage = UGITUtility.StringToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNLblPercentage").Value);

                    sers.Label.TextPattern = IsDNLblPercentage ? "{A}: {VP:P0}" : "{A}";
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "DNHoleRadius"))
                {
                    string DNHoleRadius = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNHoleRadius").Value);
                    ((DevexChart.DoughnutSeriesView)sers.View).HoleRadiusPercent = UGITUtility.StringToInt(DNHoleRadius);
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "DNLblPosition"))
                {
                    string DNLblPosition = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNLblPosition").Value);
                    DevexChart.PieSeriesLabelPosition labelPosition = DevexChart.PieSeriesLabelPosition.Radial;
                    Enum.TryParse<DevexChart.PieSeriesLabelPosition>(DNLblPosition, out labelPosition);
                    ((DevexChart.DoughnutSeriesLabel)sers.Label).Position = labelPosition;
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "DNExplodedpoint"))
                {
                    string DNExplodedpoint = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNExplodedpoint").Value);

                    DevexChart.PieExplodeMode exploadedMode = DevexChart.PieExplodeMode.None;

                    Enum.TryParse<DevexChart.PieExplodeMode>(DNExplodedpoint, out exploadedMode);
                    ((DevexChart.DoughnutSeriesView)sers.View).ExplodeMode = exploadedMode;
                }

            }
            else if (vt == DevexChart.ViewType.Funnel)
            {

                sers.LegendTextPattern = "{A}";
                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnelLblAsPercentage"))
                {
                    bool isFunnelLblAsPercentage = Convert.ToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelLblAsPercentage").Value);
                    sers.Label.TextPattern = isFunnelLblAsPercentage ? "{A}: {VP:P0}" : "{A}";
                    sers.LegendTextPattern = isFunnelLblAsPercentage ? "{A}: {VP:P0}" : "{A}";
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnellblposition"))
                {
                    string funnellblposition = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnellblposition").Value);
                    DevexChart.FunnelSeriesLabelPosition labelPosition = DevexChart.FunnelSeriesLabelPosition.Right;
                    Enum.TryParse<DevexChart.FunnelSeriesLabelPosition>(funnellblposition, out labelPosition);
                    ((DevexChart.FunnelSeriesLabel)sers.Label).Position = labelPosition;
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnelPointDist"))
                {
                    string funnelPointDist = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelPointDist").Value);

                    ((DevexChart.FunnelSeriesView)sers.View).PointDistance = Convert.ToInt32(funnelPointDist);
                }

                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnelHeightWidth"))
                {
                    string funnelHeightWidth = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelHeightWidth").Value);

                    ((DevexChart.FunnelSeriesView)sers.View).HeightToWidthRatio = UGITUtility.StringToDouble(funnelHeightWidth);
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnelAutoHeightWidth"))
                {
                    bool isFunnelAutoHeightWidth = UGITUtility.StringToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelAutoHeightWidth").Value);

                    ((DevexChart.FunnelSeriesView)sers.View).HeightToWidthRatioAuto = isFunnelAutoHeightWidth;
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "funnelAligntoCenter"))
                {
                    bool isFunnelAligntoCenter = UGITUtility.StringToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelAligntoCenter").Value);
                    ((DevexChart.FunnelSeriesView)sers.View).AlignToCenter = isFunnelAligntoCenter;
                }
            }
            else if (vt == DevexChart.ViewType.Pie)
            {
                sers.LegendTextPattern = "{A}";
                if (expression.ChartLevelProperties.Exists(x => x.Key == "pieValueAsPer"))
                {
                    bool isPieValueAsPer = UGITUtility.StringToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "pieValueAsPer").Value);

                    sers.Label.TextPattern = isPieValueAsPer ? "{A}: {VP:P0}" : "{V}";
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "pieLblPosition"))
                {
                    string pieLblPosition = Convert.ToString(ChartSetting.Expressions[0].ChartLevelProperties.FirstOrDefault(x => x.Key == "pieLblPosition").Value);

                    DevexChart.PieSeriesLabelPosition labelPosition = DevexChart.PieSeriesLabelPosition.Radial;
                    Enum.TryParse<DevexChart.PieSeriesLabelPosition>(pieLblPosition, out labelPosition);
                    ((DevexChart.PieSeriesLabel)sers.Label).Position = labelPosition;
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "pieExploadPoints"))
                {
                    string pieExploadPoint = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "pieExploadPoints").Value);

                    DevexChart.PieExplodeMode exploadedMode = DevexChart.PieExplodeMode.MinValue;

                    Enum.TryParse<DevexChart.PieExplodeMode>(pieExploadPoint, out exploadedMode);
                    ((DevexChart.PieSeriesView)sers.View).ExplodeMode = exploadedMode;
                }
            }
            else if (vt == DevexChart.ViewType.StackedBar)
            {
                diagram.Rotated = true;
                if (expression.ChartType == "StackedColumn")
                    diagram.Rotated = false;

                diagram.AxisX.VisibleInPanesSerializable = "-1";
                ((DevexChart.StackedBarSeriesView)sers.View).BarWidth = 0.3;

                if (expression.ChartLevelProperties.Exists(x => x.Key == "barStackedLblPosition"))
                {
                    string barStackedLblPosition = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "barStackedLblPosition").Value);

                    DevexChart.BarSeriesLabelPosition labelPosition = DevexChart.BarSeriesLabelPosition.Auto;
                    Enum.TryParse<DevexChart.BarSeriesLabelPosition>(barStackedLblPosition, out labelPosition);
                    ((DevexChart.StackedBarSeriesLabel)sers.Label).Position = labelPosition;
                }


                if (expression.ChartLevelProperties.Exists(x => x.Key == "barStackedLblOrientation"))
                {
                    string barStackedLblOrientation = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "barStackedLblOrientation").Value);

                    DevexChart.TextOrientation lblTextOrientation = DevexChart.TextOrientation.BottomToTop;
                    Enum.TryParse<DevexChart.TextOrientation>(barStackedLblOrientation, out lblTextOrientation);
                    ((DevexChart.StackedBarSeriesLabel)sers.Label).TextOrientation = lblTextOrientation;
                }

            }
            else if (vt == DevexChart.ViewType.Line)
            {
                if (expression.ChartLevelProperties.Exists(x => x.Key == "lineMarkType"))
                {
                    string lineMarkType = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "lineMarkType").Value);
                    DevexChart.MarkerKind type = DevexChart.MarkerKind.Circle;
                    Enum.TryParse<DevexChart.MarkerKind>(lineMarkType, out type);
                    ((DevexChart.LineSeriesView)sers.View).LineMarkerOptions.Kind = type;
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "lineMarkSize"))
                {
                    string lineMarkSize = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "lineMarkSize").Value);
                    ((DevexChart.LineSeriesView)sers.View).LineMarkerOptions.Size = Convert.ToInt32(lineMarkSize);
                }
            }
            else if (vt == DevexChart.ViewType.Spline)
            {

                if (expression.ChartLevelProperties.Exists(x => x.Key == "SPlineTensionPrcntg"))
                {
                    string SPlineTensionPrcntg = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineTensionPrcntg").Value);
                    ((DevexChart.SplineSeriesView)sers.View).LineTensionPercent = UGITUtility.StringToInt(SPlineTensionPrcntg);
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "SPlineMarkerType"))
                {
                    string SPlineMarkerType = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineMarkerType").Value);

                    DevexChart.MarkerKind type = DevexChart.MarkerKind.Circle;
                    Enum.TryParse<DevexChart.MarkerKind>(SPlineMarkerType, out type);
                    ((DevexChart.SplineSeriesView)sers.View).LineMarkerOptions.Kind = type;
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "SPlineMarkerSize"))
                {
                    string SPlineMarkerSize = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineMarkerSize").Value);

                    ((DevexChart.SplineSeriesView)sers.View).LineMarkerOptions.Size = UGITUtility.StringToInt(SPlineMarkerSize);
                }

            }
            else if (vt == DevexChart.ViewType.StepLine)
            {
                if (expression.ChartLevelProperties.Exists(x => x.Key == "stepLineLblAngle"))
                {
                    string stepLineLblAngle = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineLblAngle").Value);
                    ((DevexChart.StepLineSeriesView)sers.View).AxisX.Label.Angle = UGITUtility.StringToInt(stepLineLblAngle);
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "stepLineMarkerType"))
                {
                    string stepLineMarkerType = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineMarkerType").Value);

                    DevexChart.MarkerKind type = DevexChart.MarkerKind.Circle;
                    Enum.TryParse<DevexChart.MarkerKind>(stepLineMarkerType, out type);
                    ((DevexChart.StepLineSeriesView)sers.View).LineMarkerOptions.Kind = type;
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "stepLineMarkerSize"))
                {
                    string stepLineMarkerSize = Convert.ToString(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineMarkerSize").Value);
                    ((DevexChart.StepLineSeriesView)sers.View).LineMarkerOptions.Size = Convert.ToInt32(stepLineMarkerSize);
                }
                if (expression.ChartLevelProperties.Exists(x => x.Key == "steplineInverted"))
                {
                    bool steplineInverted = Convert.ToBoolean(expression.ChartLevelProperties.FirstOrDefault(x => x.Key == "steplineInverted").Value);
                    ((DevexChart.StepLineSeriesView)sers.View).InvertedStep = Convert.ToBoolean(steplineInverted);
                }
            }


            if (!string.IsNullOrWhiteSpace(expression.LabelText))
            {
                sers.Label.TextPattern = expression.LabelText;
            }

            if (!string.IsNullOrWhiteSpace(expression.LabelFormat) && expression.LabelFormat.ToLower() == "currency")
                sers.CrosshairLabelPattern = "{A}: {V:C}";
            else
            {
                if(vt == DevexChart.ViewType.StackedBar)
                    sers.CrosshairLabelPattern = "{S}: {V:#,0.##}";
                else sers.CrosshairLabelPattern = "{A}: {V:#,0.##}";
            }

            sers.ValueScaleType = ScaleType.Numerical;



            return sers;
        }


        private void PlotChartPointsReverse(DevexChart.Web.WebChartControl dcpChart, DataTable datapointsTable)
        {
            DataColumnCollection columns = datapointsTable.Columns;
            if (datapointsTable == null)
            {
                return;
            }

            if (ChartSetting.Expressions.Count > 0 && ChartSetting.Dimensions.Count == 1)
            {
                foreach (DataColumn column in datapointsTable.Columns)
                {
                    if (column.ColumnName == InternalTitleColumn || datapointsTable.Columns.IndexOf(column) == 1)
                        continue;

                    foreach (DataRow datapoint in datapointsTable.Rows)
                    {
                        //ChartExpression expression = ChartSetting.Expressions.FirstOrDefault(x => x.Title.ToLower() == column.ColumnName.ToLower());
                        int expressionIndex = 0;
                        int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                        ChartExpression expression = ChartSetting.Expressions[expressionIndex];
                        ChartDimension dimension = ChartSetting.Dimensions[0];

                        object val = new object();
                        if (expression.Operator != string.Empty)
                        {
                            val = UGITUtility.StringToDouble(datapoint[column.ColumnName]);
                        }
                        else
                        {
                            val = datapoint[column.ColumnName];
                        }

                        string name = Convert.ToString(datapoint[1]);
                        DevexChart.Series sr = dcpChart.Series[name];
                        if (val is DateTime)
                        {
                            sr.ValueScaleType = ScaleType.DateTime;
                            sr.Label.TextPattern = "{V:MMM-d-yyyy}";
                        }
                        //Add datapoint
                        object xVal = datapoint[1];
                        object internalTitleValue = datapoint[InternalTitleColumn];

                        DatapointClickeEventType clickEvent = expression.DataPointClickEvent;
                        object dataPointDetailVal = internalTitleValue;
                        if (dimension.DateViewType == "month")
                        {
                            DateTime date = Convert.ToDateTime(xVal);
                            dataPointDetailVal = string.Format("{0:dd-MMM-yy}", date);
                            xVal = string.Format("{0:MMM-yy}", date);
                        }
                        else if (dimension.DateViewType == "day")
                        {
                            DateTime date = Convert.ToDateTime(xVal);
                            dataPointDetailVal = string.Format("{0:dd-MMM-yy}", date);
                            xVal = string.Format("{0:dd-MMM-yy}", date);
                        }
                        else if (dimension.DateViewType == "year")
                        {
                            dataPointDetailVal = string.Format("{0:dd-MMM-yy}", new DateTime(UGITUtility.StringToInt(xVal), dateForDateView.Month, 1));
                            xVal = string.Format("{0:yyyy}", new DateTime(UGITUtility.StringToInt(xVal), dateForDateView.Month, 1));
                        }

                        if (expression.DataPointClickEvent == DatapointClickeEventType.Inherit)
                        {
                            clickEvent = dimension.DataPointClickEvent;
                        }

                        string dimsVal = Convert.ToString(dataPointDetailVal);
                        int index = sr.Points.Add(new DevexChart.SeriesPoint(column.ColumnName, val));
                        string argument = string.Format("{0}*{1}*{2}*{3}*{4}*{5}::{4}", dimensionIndex, (dimsVal), expressionIndex, (sr.Name), (int)clickEvent, (Convert.ToString(internalTitleValue)));
                        sr.Points[index].Annotations.AddTextAnnotation("tag", argument).Visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// Parse GlobalFilter into actual datatable expression
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public static string GetGlobalFilter(ApplicationContext context, string filterString, DashboardPanelView view)
        {
            Dictionary<string, string> filters = GetGlobalFilterList(context, filterString, view);
            return string.Join(" And ", filters.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Value).ToArray());
        }

        /// <summary>
        /// Parse GlobalFilter into actual datatable expression
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetGlobalFilterList(ApplicationContext context, string filterString, DashboardPanelView view)
        {
            Dictionary<string, string> filterList = new Dictionary<string, string>();
            if (filterString == null || filterString.Trim() == string.Empty)
            {
                return filterList;
            }

            filterString = Uri.UnescapeDataString(filterString);
            string externalFilter = string.Empty;
            string[] filterOpts = UGITUtility.SplitString(filterString, ";*;");
            if (filterOpts.Length > 0)
            {
                filterString = Uri.UnescapeDataString(filterOpts[0]);
                if (filterOpts.Length > 1)
                {
                    externalFilter = Uri.UnescapeDataString(filterOpts[1]);
                }
            }
            filterString = filterString.Replace("~", "=");


            //create where clause using global filter of chart
            if (filterString != null && filterString.Trim() != string.Empty)
            {
                List<DashboardFilterProperty> viewfilters = new List<DashboardFilterProperty>();
                if (view != null && (view.ViewProperty is IndivisibleDashboardsView || view.ViewProperty is SuperDashboardsView))
                {
                    if (view.ViewProperty is IndivisibleDashboardsView)
                    {
                        IndivisibleDashboardsView indiView = view.ViewProperty as IndivisibleDashboardsView;
                        viewfilters = indiView.GlobalFilers;
                    }
                    else
                    {
                        SuperDashboardsView sView = view.ViewProperty as SuperDashboardsView;
                        viewfilters = sView.GlobalFilers;
                    }
                }

                if (viewfilters.Count <= 0)
                {
                    return filterList;
                }


                Dictionary<string, string> filters = UGITUtility.GetCustomProperties(filterString, ";#");
                //return empty when not filter comes in parameter
                if (filters == null || filters.Count <= 0)
                {
                    return filterList;
                }

                StringBuilder query = new StringBuilder();
                //Loop all global filter to create formula expression
                foreach (DashboardFilterProperty filterP in viewfilters)
                {
                    string filterKey = filterP.ID.ToString();
                    if (!filters.ContainsKey(filterKey))
                    {
                        filterList.Add(filterKey, string.Empty);
                        continue;
                    }

                    if (filters[filterKey] == null || filters[filterKey].Trim() == string.Empty)
                    {
                        filterList.Add(filterKey, string.Empty);
                        continue;
                    }

                    DataTable dasbhoardTable = DashboardCache.GetCachedDashboardData(context, filterP.ListName);
                    if (dasbhoardTable == null || dasbhoardTable.Rows.Count <= 0)
                    {
                        filterList.Add(filterKey, string.Empty);
                        continue;
                    }

                    if (!dasbhoardTable.Columns.Contains(filterP.ColumnName))
                    {
                        filterList.Add(filterKey, string.Empty);
                        continue;
                    }

                    query = new StringBuilder();


                    DataColumn column = dasbhoardTable.Columns[filterP.ColumnName];
                    string[] values = filters[filterKey].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length > 1)
                        query.Append(" (");
                    if (column.DataType == typeof(String))
                    {


                        for (int i = 0; i < values.Length; i++)
                        {
                            if (i != 0)
                            {
                                query.Append(" OR ");
                            }
                            query.AppendFormat("{0} = '{1}' ", column.ColumnName, values[i]);

                        }

                    }
                    else if (column.DataType == typeof(DateTime))
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (i != 0)
                            {
                                query.Append(" OR ");
                            }
                            string[] dateRange = values[i].Split(new string[] { "to" }, StringSplitOptions.None);
                            if (dateRange.Length == 1)
                            {
                                DateTime date = Convert.ToDateTime(values[i]);
                                query.AppendFormat("{0}=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                            }
                            else
                            {
                                string expression = "(";
                                if (dateRange[0].Trim().Length == 8)
                                {
                                    DateTime date = Convert.ToDateTime(dateRange[0].Insert(2, "/"));
                                    expression += string.Format("{0}>=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                                    if (dateRange[1].Trim().Length == 8)
                                    {
                                        expression += " AND ";
                                    }
                                }
                                if (dateRange[1].Trim().Length == 8)
                                {
                                    DateTime date = Convert.ToDateTime(dateRange[1].Insert(2, "/"));
                                    expression += string.Format("{0}<=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                                }
                                expression += ")";
                                query.AppendFormat("{0} ", expression);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (i != 0)
                            {
                                query.Append(" OR ");
                            }
                            query.AppendFormat("{0}={1} ", column.ColumnName, values[i]);
                        }
                    }
                    if (values.Length > 1)
                        query.Append(") ");

                    filterList.Add(filterKey, query.ToString());
                }
            }

            //Append External query with filter
            if (externalFilter != null && externalFilter.Trim() != string.Empty)
            {
                filterList.Add("ExternalQuery", string.Format("({0})", externalFilter));
            }
            return filterList;
        }

        /// <summary>
        /// Get global filter data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DataTable GetDatatableForGlobalFilter(ApplicationContext context, DashboardFilterProperty item, ref string filterDataType)
        {
            return GetDatatableForGlobalFilter(context, item, ref filterDataType, string.Empty);
        }

        /// <summary>
        /// Get global filter data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DataTable GetDatatableForGlobalFilter(ApplicationContext context, DashboardFilterProperty item, ref string filterDataType, string filter)
        {
            DataTable filterValues = new DataTable();
            filterValues.Columns.Add("Value");

            string column = Convert.ToString(item.ColumnName);
            DataTable factTable = null;
            if (Convert.ToString(item.ListName) == DatabaseObjects.Tables.DashboardSummary)
            {
                factTable = (DataTable)CacheHelper<object>.Get($"DashboardSummary", context.TenantID);
                if (factTable == null)
                {
                    factTable = DashboardCache.GetCachedDashboardData(context, Convert.ToString(item.ListName));
                }
            }
            else
            {
                factTable = DashboardCache.GetCachedDashboardData(context, Convert.ToString(item.ListName));
            }
            // return empty table if column is not exist in table
            if (factTable == null || string.IsNullOrEmpty(column) || column.Trim() == string.Empty || !factTable.Columns.Contains(column))
            {
                return filterValues;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                DataRow[] rows = factTable.Select(filter);
                if (rows.Length > 0)
                    factTable = rows.CopyToDataTable();
                else
                    return filterValues;
            }

            //return empty table if data is not exist
            DataTable uniqueData = factTable.DefaultView.ToTable(true, column);
            if (uniqueData == null)
            {
                return filterValues;
            }

            filterDataType = uniqueData.Columns[0].DataType.ToString().Replace("System.", string.Empty);
            if (uniqueData.Columns[0].DataType == typeof(DateTime))
            {
                filterDataType = "DateTime";
                var filterByYear = uniqueData.AsEnumerable().ToLookup(x => ((DateTime)x.ItemArray[0]).Year).OrderBy(x => x.Key);
                foreach (var year in filterByYear)
                {
                    DataRow[] filteredRows = year.ToArray();
                    var filterByMonth = filteredRows.ToLookup(x => ((DateTime)x.ItemArray[0]).Month).OrderBy(x => x.Key);
                    foreach (var month in filterByMonth)
                    {
                        DateTime date = new DateTime(year.Key, month.Key, 1);
                        filterValues.Rows.Add(date.ToString("MMM yyyy"));
                    }
                }
            }
            else
            {
                DataView sortView = uniqueData.DefaultView;
                sortView.Sort = string.Format("{0} asc", column.Trim());
                foreach (DataRowView q in sortView)
                {
                    if (q[column.Trim()] != null && q[column.Trim()].ToString().Trim() != string.Empty)
                    {
                        filterValues.Rows.Add(q[column.Trim()].ToString().Trim());
                    }
                }
            }

            DataRow[] multiDataRows = filterValues.Select(string.Format("Value Like '%;#%'"));
            DataRow[] selectedRows = filterValues.Select().Except(multiDataRows).ToArray();

            //filter data from multi values
            foreach (DataRow mRow in multiDataRows)
            {
                //Commented By Munna
                //SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection(string.Format("0;#{0}", mRow["Value"]));

                //foreach (SPFieldLookupValue lookup in lookups)
                //{
                //    filterValues.Rows.Add(lookup.LookupValue);
                //}
                filterValues.Rows.Remove(mRow);
            }

            filterValues.DefaultView.Sort = "Value asc";
            return filterValues.DefaultView.ToTable(true, "Value");
        }
        public DataTable CreateChartObj()
        {
            dynamic output = new ExpandoObject();
            //Get datapoints table view
            DataTable datapointsTable = GetChartPointsTable();
            try
            {
                var result = CreateChartSeries(datapointsTable);
                var table = PlotDxChartPoints(result, datapointsTable);
                if (table != null && table.Columns.Count > 0)
                {
                    return table;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return datapointsTable;
            }

            return datapointsTable;
        }
    }
}
