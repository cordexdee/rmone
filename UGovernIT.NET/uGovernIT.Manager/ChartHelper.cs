using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Threading;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using System.Globalization;

namespace uGovernIT.Manager
{
    public class ChartHelper
    {
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
        public ChartSetting ChartSetting { get; private set; }
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
        private string legendFont = "Verdana";
        private int legendFontSize = 8;
        private string dimensionFilter;
        /// <summary>
        /// 0 = Single chart view
        /// 1 = Multi chart view
        /// </summary>
        public int ViewType { get; set; }

        int workingHoursInDays;
        private const string InternalTitleColumn = "Temp_Title_";
        ApplicationContext _context;
        public ChartHelper(ChartSetting chartSetting, ApplicationContext context)
        {
            _context = context;
            GlobalFilter = string.Empty;
            ChartTitle = string.Empty;
            ChartSetting = chartSetting;
            DatapointFilter = string.Empty;
            LocalFilter = string.Empty;
            workingHoursInDays = uHelper.GetWorkingHoursInADay(context);
            //Load dashboard fact table
            DataTable table = DashboardCache.GetCachedDashboardData(context, ChartSetting.FactTable);

            if (table != null)
            {
                table.AcceptChanges();
                dashboardTable = table;
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
                DataTable workItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"TenantID='{_context.TenantID}'");
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
                    DataTable resourceAllocations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, $"TenantID='{_context.TenantID}'");
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
                            ULog.WriteException(ex, "");
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
                    DataTable resourceTimeSheet = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet, $"TenantID='{_context.TenantID}'");
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
            datapointTable = ExpressionCalc.RemoveExpressionTempColumns(datapointTable);
            if (datapointTable != null && datapointTable.Columns.Contains(InternalTitleColumn))
                datapointTable.Columns.Remove(InternalTitleColumn);

            return datapointTable;
        }

        /// <summary>
        /// Get chart points in table view
        /// </summary>
        /// <returns></returns>
        private DataTable GetChartPointsTable()
        {
            //set next dimensionindex based on dimensionfilter and datapoint filter
            SetNewDimensionIndex();

            //Dimension filter data
            DataTable data = GetDataAgainstDimension();

            //Apply Global Filter. It works when somebody change global filter
            data = ApplyGlobalFilter(data);

            //Apply Local Filter.It works when somebody select local filter option
            data = ApplyLocalFilter(data);

            //Apply DataPoint filter. It works when somebody click on datapoint
            data = ApplyDatapointFilter(data);

            DataTable datapointTable = new DataTable("FilteredData");
            //Create fitlered table schema
            datapointTable = CreateDatapointTableSchema(data);

            //Fill datapoint data after appling all filter. Now we have right data to create datapoint
            FillDataPointsTable(data, datapointTable);

            if (datapointTable != null)
                datapointTable = datapointTable.DefaultView.ToTable();

            return datapointTable;
        }

        /// <summary>
        /// Create basic chart without series
        /// </summary>
        /// <param name="isPostRequired"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isPreview"></param>
        /// <returns></returns>

        public Chart CreateChart(bool isPostRequired, string width, string height, bool isPreview)
        {
            //Set action is required or not on datapoints
            this.IsPostRequired = isPostRequired;
            this.isPreview = isPreview;

            //Get chart width basic setting
            Chart cPChart = BasicChartSetting();

            try
            {
                //Set width and height of chart according to the specification
                cPChart.ID = "customChart_" + ChartSetting.ChartId.ToString();
                if (width != null && !width.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                {
                    cPChart.Width = Unit.Parse(width.Trim());
                }

                if (height != null && !height.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                {
                    cPChart.Height = Unit.Parse(height.Trim());
                }

                bool isLoadChart = true;
                bool loadInThread = false;
                bool cacheChart = false;

                //Checks chart cache is enable or not. chart cache only cache initial chart load so for cache filter should be empty.
                if (ChartSetting.IsCacheChart && string.IsNullOrEmpty(this.DatapointFilter) && string.IsNullOrEmpty(this.GlobalFilter) && string.IsNullOrEmpty(this.LocalFilter))
                {
                    //Gets cached chart's datapoint and checked 
                    ChartCachedDataPoints chartCachedDatapoints = ChartCache.GetChartCache(ChartSetting.ChartId.ToString());
                    if (chartCachedDatapoints != null && chartCachedDatapoints.ChartSeries != null && chartCachedDatapoints.ChartSeries.Count > 0)
                    {
                        isLoadChart = false;
                        //Sets cached data point to chart object
                        cPChart.Series.Clear();
                        foreach (Series currentSeries in chartCachedDatapoints.ChartSeries)
                        {
                            cPChart.Series.Add(currentSeries);
                            if (this.isPreview)
                            {
                                currentSeries.IsVisibleInLegend = false;
                                currentSeries.Label = string.Empty;
                                currentSeries.MarkerStyle = MarkerStyle.None;
                                currentSeries.IsValueShownAsLabel = false;
                                currentSeries.MarkerStyle = MarkerStyle.None;
                                foreach (DataPoint point in currentSeries.Points)
                                {
                                    point.IsValueShownAsLabel = false;
                                    point.Label = string.Empty;
                                }
                            }
                            else
                            {

                                currentSeries.IsVisibleInLegend = !ChartSetting.HideLegend;

                                int exIndex = 0;
                                int.TryParse(currentSeries.GetCustomProperty("expressionIndex"), out exIndex);
                                if (ChartSetting.Expressions.Count > exIndex)
                                {
                                    bool showLabel = !ChartSetting.Expressions[exIndex].HideLabel;
                                    currentSeries.IsValueShownAsLabel = showLabel;
                                    foreach (DataPoint point in currentSeries.Points)
                                    {
                                        point.IsValueShownAsLabel = showLabel;
                                    }

                                    if (string.IsNullOrEmpty(ChartSetting.Expressions[exIndex].LabelColor))
                                        ChartSetting.Expressions[exIndex].LabelColor = "#000000";

                                    currentSeries.LabelForeColor = System.Drawing.ColorTranslator.FromHtml(ChartSetting.Expressions[exIndex].LabelColor);
                                }

                                cPChart.Palette = currentSeries.Palette;

                            }
                        }

                        this.ChartTitle = chartCachedDatapoints.Title;

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
                                        cacheChart = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        isLoadChart = true;
                        cacheChart = true;
                    }
                }

                //Load datapoint again if loadinthread enable
                if (loadInThread)
                {
                    ChartCachedDataPoints chartDatapoints = ChartCache.GetChartCache(ChartSetting.ChartId.ToString());
                    if (chartDatapoints != null)
                    {
                        this.ChartTitle = ChartSetting.ContainerTitle;
                        //ThreadStart refreshChartDelegate = delegate ()
                        //{

                        Chart cChart = CreateChartObj(isPostRequired, width, height, isPreview);
                        if (ChartSetting.IsCacheChart)
                        {
                            ChartCache.RefreshChartCache(cChart, this);
                        }
                        //};
                        //Thread refreshChartThread = new Thread(refreshChartDelegate);
                        //refreshChartThread.IsBackground = true;
                        //refreshChartThread.Start();
                    }
                    else
                    {
                        isLoadChart = true;
                    }
                }

                //Load datapoint again if isLoadChard enable
                if (isLoadChart)
                {
                    cPChart = CreateChartObj(isPostRequired, width, height, isPreview);
                    if (ChartSetting.IsCacheChart && cacheChart)
                    {
                        ChartCache.RefreshChartCache(cPChart, this);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Creating Chart");
            }

            return cPChart;
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
        private Chart CreateChartObj(bool isPostRequired, string width, string height, bool isPreview)
        {
            //Set action is required or not on datapoints
            this.IsPostRequired = isPostRequired;
            this.isPreview = isPreview;

            //Get chart width basic setting
            Chart cPChart = BasicChartSetting();

            //Set width and height of chart according to the specification
            cPChart.ID = "customChart_" + ChartSetting.ChartId.ToString();
            if (width != null && !width.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
            {
                cPChart.Width = Unit.Parse(width.Trim());
            }

            if (height != null && !height.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
            {
                cPChart.Height = Unit.Parse(height.Trim());
            }

            //Get datapoints table view
            DataTable datapointsTable = GetChartPointsTable();

            if (ChartSetting.ReversePlotting)
            {
                //Create series for chart
                CreateChartSeriesUsingRow(cPChart, datapointsTable);
                PlotChartPointsReverse(cPChart, datapointsTable);
            }
            else
            {
                //Create series for chart
                CreateChartSeries(cPChart, datapointsTable);
                //Plot chart points using datapointsTable
                PlotChartPoints(cPChart, datapointsTable);
            }

            //Sets showDetail property which can be used to open detail popup when next dimension is empty
            if (ChartSetting.Dimensions.Count > 0 && WhoTriggered == 1)
            {
                ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
                if (dimension.DataPointClickEvent == DatapointClickeEventType.Detail)
                {
                    if (datapointsTable == null || datapointsTable.Rows.Count <= 0)
                    {
                        ShowDetail = true;
                    }

                    DataTable uniquePoints = datapointsTable.DefaultView.ToTable(true, datapointsTable.Columns[0].ColumnName);
                    if (uniquePoints.Rows.Count == 1 && (uniquePoints.Rows[0][0] == null || Convert.ToString(uniquePoints.Rows[0][0]) == "None"))
                    {
                        ShowDetail = true;
                    }
                }
            }

            return cPChart;
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
            if (ChartSetting.Dimensions.Count > 0)
            {
                ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
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
            foreach (ChartExpression expression in ChartSetting.Expressions)
            {
                SeriesChartType chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), expression.ChartType);

                //if charttype: pie, funnel, pyramit, doughnut then you can not set group field so reset expression GroupByField setting
                if (chartType == SeriesChartType.Pie || chartType == SeriesChartType.Funnel || chartType == SeriesChartType.Pyramid || chartType == SeriesChartType.Doughnut)
                {
                    expression.GroupByField = string.Empty;
                }

                if (!string.IsNullOrEmpty(expression.GroupByField) && data != null && data.Rows.Count > 0 && data.Columns.Contains(expression.GroupByField))
                {
                    int columnIndex = data.Columns.IndexOf(data.Columns[expression.GroupByField]);
                    //Groups facttable based on GroupByField
                    data.DefaultView.Sort = string.Format("{0} desc", expression.GroupByField);
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

                            if (expression.Operator != string.Empty)
                                newTable.Columns.Add(columnName, typeof(double));
                            else
                                newTable.Columns.Add(columnName, typeof(string));
                        }

                        //Store expressionindex in column extended property so that we can get it
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
                        else
                            newTable.Columns.Add(string.Format("{0}-{1}", expression.Title, newTable.Columns.Count), typeof(string));
                    }
                    else
                    {
                        if (expression.Operator != string.Empty)
                            newTable.Columns.Add(Convert.ToString(expression.Title), typeof(double));
                        else
                            newTable.Columns.Add(Convert.ToString(expression.Title), typeof(string));
                    }

                    //Store expressionindex in column extended property so that we can get it
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
                    //Code changed to fix the charts in TSR ticket summary report.
                    DataRow[] rows = dashboardTable.Select(ChartSetting.BasicFilter);//Select()
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
                DataRow[] filteredRows = inputTable.Copy().Select(GlobalFilter);
                if (filteredRows.Length > 0)
                {
                    return filteredRows.CopyToDataTable();
                }
                else
                {
                    return inputTable.Clone();
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
        private DataTable ApplyLocalFilter(DataTable inputTable)
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
            filteredTable.Columns["IsInRange_"].Expression = string.Format("IIF(([{2}] < #{0}#   and  [{3}] >= #{1}#) , true, false)", endDate.Date.AddDays(1).ToString("MM/dd/yyyy"), startDate.Date.ToString("MM/dd/yyyy"), startColumn.ColumnName, endColumn.ColumnName);

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
                filterString = HttpUtility.UrlDecode(filterArray[1]);
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
                            filterExpn = string.Format("{0}='{1}'", expression.GroupByField, HttpUtility.UrlDecode(filterArray[3]));
                        }
                        else if (string.IsNullOrEmpty(filterArray[3]) && (eColumn.DataType == typeof(Int32) || eColumn.DataType == typeof(Int64) || eColumn.DataType == typeof(double) || eColumn.DataType == typeof(decimal) || eColumn.DataType == typeof(float)))
                        {
                            filterExpn = string.Format("({0} is null or {0}=0)", expression.GroupByField);
                        }
                        else if (string.IsNullOrEmpty(filterArray[3]) && (eColumn.DataType == typeof(bool)))
                        {
                            filterExpn = string.Format("{0}={1}", expression.GroupByField, "False");
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
                DataRow[] rows = expCalc.FactTable.Select(expCalc.ResolveFunctions(_context, expressionFilter));
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
                    else if(dimension.SelectedField == DatabaseObjects.Columns.TicketRequestTypeLookup && filterString.Contains("&gt;"))
                    {
                        //Changed by Inderjeet Kaur on 5 Sept 2022 to resolve the drill down feature 
                        //in Top 3 Change Categories chart in dashboard.
                        rows = table.Select(string.Format("{0} ='{1}'", dimension.SelectedField, filterString.Replace("&gt;", ">")));
                    }
                    else
                    {
                        rows = table.Select(string.Format("{0} ='{1}'", dimension.SelectedField, HttpUtility.UrlDecode(filterString)));
                    }
                }
                else if (column.DataType == typeof(DateTime))
                {
                    //In case of datetime apply datapoint fitler according to dateviewtype (year, month, days)

                    if (dimension.DateViewType == "year")
                    {
                        int year = dateForDateView.Year;

                        if (DateTime.TryParse(filterString, out DateTime datefilter))
                        {
                            year = datefilter.Year;
                        }

                        dateForDateView = new DateTime(year, 1, 1);
                        rows = table.Select(string.Format("{0} is not null", dimension.SelectedField)).Where(x => x.Field<DateTime>(dimension.SelectedField).Year == year).ToArray();
                    }
                    else if (dimension.DateViewType == "month")
                    {
                        int year = dateForDateView.Year;
                        int month = dateForDateView.Month;
                        //DateTime datefilter = Convert.ToDateTime(filterString);
                        if (DateTime.TryParse(filterString, out DateTime datefilter))
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
                        //DateTime datefilter = DateTime.ParseExact(filterString,"MM-dd-yyyy", CultureInfo.InvariantCulture);
                        int day = dateForDateView.Day;
                        if (DateTime.TryParse(filterString, out DateTime datefilter))
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
                            lookups = filteredData.Select(string.Format("{0} is not null", dimension.SelectedField)).OrderBy(x => x.Field<DateTime>(dimension.SelectedField)).ToLookup(x => (object)string.Format("{0}/{1}", x.Field<DateTime>(dimension.SelectedField).Month, x.Field<DateTime>(dimension.SelectedField).Year));
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
                if (dimension.PickTopDataPoint > 0)
                {
                    topPoints = PickTopDimensionPoint(filteredData, lookups);
                }
                if (topPoints.Count <= 0)
                {
                    topPoints = lookups.Select(x => x.Key).ToList();
                }


                object definedTypeObj = topPoints.FirstOrDefault(x => x != null);
                for (int j = 0; j < topPoints.Count; j++)
                {
                    if (topPoints[j] == null || Convert.ToString(topPoints[j]) == string.Empty)
                    {
                        topPoints[j] = "None";
                    }
                }


                //Order datapoint 
                if (dimension.PickTopDataPoint <= 0)
                {
                    if (dimension.DataPointOrder.ToLower() == "descending")
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
                            topPoints = topPoints.OrderBy(x => x).ToList();
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
                    CreateDataPoint(filteredData, datapointTable, title, dimensionFilterRows);
                }

                if (dimension.PickTopDataPoint > 0 && datapointTable.Columns.Count > 2)
                {
                    DataColumn column = datapointTable.Columns[2];
                    if (dimension.DataPointOrder.ToLower() == "descending")
                        datapointTable.DefaultView.Sort = string.Format("{0} desc", column.ColumnName);
                    else
                        datapointTable.DefaultView.Sort = string.Format("{0} asc", column.ColumnName);
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
        private void CreateDataPoint(DataTable filteredData, DataTable datapointTable, string Title, DataRow[] filterRows)
        {
            DataRow datapointRow = datapointTable.NewRow();
            datapointRow[InternalTitleColumn] = Title;
            Title = UGITUtility.RemoveIDsFromLookupString(Title);
            datapointRow[1] = Title;

            datapointTable.Rows.Add(datapointRow);

            for (int i = 2; i < datapointTable.Columns.Count; i++)
            {
                DataColumn column = datapointTable.Columns[i];

                //get series expression index and name
                int srIndex = 0;
                int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out srIndex);
                string srName = column.ColumnName;

                try
                {
                    if (filterRows.Length <= 0 || ChartSetting.Expressions.Count <= srIndex)
                    {
                        //Creates empty point if filter data is not available for current dimension point or series is empty.
                        datapointRow[column] = 0;
                    }
                    else
                    {
                        ChartExpression expression = ChartSetting.Expressions[srIndex];
                        DataRow[] dashboardfilterRows = filterRows;

                        if (filterRows.Length > 0 && !string.IsNullOrEmpty(expression.ExpressionFormula))
                        {
                            DataTable dashboardFilterRowsTable = filterRows.CopyToDataTable();
                            ExpressionCalc exCalc = new ExpressionCalc(dashboardFilterRowsTable, true);
                            string filter = exCalc.ResolveFunctions(_context, expression.ExpressionFormula);
                            filterRows = exCalc.FactTable.Select();
                            dashboardfilterRows = exCalc.FactTable.Select(filter);
                            if (dashboardfilterRows.Length <= 0)
                            {
                                datapointRow[column] = 0;
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
                                expressionGroupLookups = filterRows.Where(x => x.Field<System.String>(groupColumn) == null || x.Field<System.String>(groupColumn) == string.Empty).ToArray();
                            }
                            else
                            {
                                expressionGroupLookups = filterRows.Where(x => x.Field<System.String>(groupColumn) == srName).ToArray();
                            }

                            if (expressionGroupLookups.Length <= 0)
                            {
                                //set datapoint point is empty
                                datapointRow[column] = 0;
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
                            datapointRow[column] = 0;
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
                                    if (expression.Operator != string.Empty)
                                    {
                                        aggregateResult = GetAggregateData(sRows, expression.Operator, "expressionData");
                                    }
                                    else
                                    {
                                        string columnStr = expression.FunctionExpression.Replace("[", string.Empty).Replace("]", string.Empty);
                                        if (filteredData != null && filteredData.Columns.Contains(columnStr) && filteredData.Columns[columnStr].DataType.Name == "DateTime")
                                        {
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
                                if (expression.FunctionExpression.Contains("Daysdiff") && expression.Operator == "average")
                                    expression.Operator = "";
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
                            }

                            //Add datapoint



                            datapointRow[column] = aggregateResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }

        /// <summary>
        /// Plot chart datapoints
        /// </summary>
        /// <param name="cPChart"></param>
        /// <param name="datapointsTable"></param>
        private void PlotChartPoints(Chart cPChart, DataTable datapointsTable)
        {
            DataColumnCollection columns = datapointsTable.Columns;
            if (datapointsTable == null && datapointsTable.Rows.Count <= 1)
            {
                return;
            }
            try
            {
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

                            object val = new object();
                            if (expression.Operator != string.Empty)
                            {
                                val = UGITUtility.StringToDouble(datapoint[column.ColumnName]);
                            }
                            else
                            {
                                val = UGITUtility.ObjectToString(datapoint[column.ColumnName]);
                            }

                            //Calculate percentage if property is true;
                            if (expression.ShowInPercentage)
                            {
                                double totalCount = datapointsTable.AsEnumerable().Sum(x => x.Field<double>(column.ColumnName));
                                val = Math.Round((UGITUtility.StringToDouble(val) / totalCount) * 100, 0);
                            }

                            Series sr = cPChart.Series[column.ColumnName];

                            //Add datapoint
                            object xVal = datapoint[1];
                            object internalXValue = datapoint[0];

                            DatapointClickeEventType clickEvent = expression.DataPointClickEvent;
                            object dataPointDetailVal = internalXValue;
                            ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
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
                                dataPointDetailVal = string.Format("{0:dd-MMM-yy}", new DateTime(UGITUtility.StringToInt(xVal), 1, 1));
                                xVal = string.Format("{0:yyyy}", new DateTime(UGITUtility.StringToInt(xVal), 1, 1));

                            }

                            if (expression.DataPointClickEvent == DatapointClickeEventType.Inherit)
                            {
                                clickEvent = dimension.DataPointClickEvent;
                            }

                            //skip zero value
                            if (dimension.PickTopDataPoint > 0 && expression.Operator != string.Empty && UGITUtility.StringToDouble(val) == 0)
                            {
                                continue;
                            }

                            int index = -1;
                            if (expression.Operator != string.Empty)
                            {
                                index = sr.Points.AddXY(xVal, val);
                            }
                            else
                            {
                                index = sr.Points.AddXY(xVal, 1);
                            }

                            //Set other properties of datapoint
                            if (!expression.HideLabel)
                            {
                                if (!string.IsNullOrWhiteSpace(expression.LabelFormat))
                                {
                                    sr.Points[index].Label = uHelper.FormatNumber(UGITUtility.StringToDouble(val), expression.LabelFormat);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(expression.LabelText))
                                    {
                                        sr.Points[index].Label = expression.LabelText.Replace("$Exp$", uHelper.FormatNumber(UGITUtility.StringToDouble(val), string.Empty));
                                    }
                                    else
                                    {
                                        sr.Points[index].Label = uHelper.FormatNumber(Convert.ToDouble(val), string.Empty);
                                    }
                                }
                            }

                            if (Convert.ToString(val) == string.Empty || (val.GetType().ToString() == "String.Double" && UGITUtility.StringToDouble(val) == 0))
                            {
                                sr.Points[index].Label = " ";
                                sr.Points[index].MarkerStyle = MarkerStyle.None;
                            }

                            //Set tooltip
                            string toolTip = string.Format("{0}: {1}", xVal, uHelper.FormatNumber(UGITUtility.StringToDouble(val), string.Empty));
                            if (!string.IsNullOrWhiteSpace(expression.LabelFormat))
                            {
                                toolTip = string.Format("{0}: {1}", xVal, uHelper.FormatNumber(UGITUtility.StringToDouble(val), expression.LabelFormat));
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(expression.LabelText))
                                {
                                    toolTip = string.Format("{0}: {1}", xVal, expression.LabelText.Replace("$Exp$", uHelper.FormatNumber(UGITUtility.StringToDouble(val), string.Empty)));
                                }
                            }
                            sr.Points[index].ToolTip = toolTip;

                            if (this.isPreview)
                            {
                                sr.IsValueShownAsLabel = false;
                                sr.Label = string.Empty;
                                sr.Points[index].Label = string.Empty;
                                sr.MarkerStyle = MarkerStyle.None;
                                sr.Points[index].IsValueShownAsLabel = false;
                            }


                            sr.Points[index].LegendToolTip = sr.Name;
                            sr.Points[index].LegendText = string.Format("{0} ({1}{2})", datapoint[0], val, expression.ShowInPercentage ? "%" : string.Empty);

                            if (clickEvent != DatapointClickeEventType.None && clickEvent != DatapointClickeEventType.Inherit)
                            {
                                string dimsVal = Convert.ToString(dataPointDetailVal);
                                if (dimensionFilter != null)
                                    dimsVal = string.Format("{0}>{1}", dimensionFilter, dimsVal);

                                sr.Points[index].PostBackValue = string.Format("javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})", dimensionIndex, HttpUtility.UrlEncode(dimsVal), expressionIndex, HttpUtility.UrlEncode(sr.Name), (int)clickEvent, HttpUtility.UrlEncode(internalXValue.ToString()));
                                sr.Points[index].MapAreaAttributes = string.Format("onclick=\"javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})\"", dimensionIndex, HttpUtility.UrlEncode(dimsVal), expressionIndex, HttpUtility.UrlEncode(sr.Name), (int)clickEvent, HttpUtility.UrlEncode(internalXValue.ToString()));
                            }
                        }
                    }

                }
                else if (datapointsTable.Rows.Count > 0)
                {
                    Series sr = cPChart.Series[0];
                    cPChart.Legends.Clear();
                    for (int i = 1; i < cPChart.Series.Count; i++)
                    {
                        cPChart.Series[i].IsVisibleInLegend = false;
                        cPChart.Series.Remove(cPChart.Series[i]);
                    }

                    for (int j = 2; j < columns.Count; j++)
                    {
                        DataColumn column = columns[j];

                        int expressionIndex = 0;
                        int.TryParse(Convert.ToString(column.ExtendedProperties["expressionIndex"]), out expressionIndex);
                        ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                        double val = UGITUtility.StringToDouble(datapointsTable.Rows[0][column]);
                        int index = sr.Points.AddXY(column.ColumnName, val);

                        if (val == 0)
                        {
                            sr.Points[index].MarkerStyle = MarkerStyle.None;
                        }

                        //Set tooltip
                        string toolTip = string.Format("{0}: {1}", column.ColumnName, uHelper.FormatNumber(UGITUtility.StringToDouble(val), string.Empty));
                        if (!string.IsNullOrWhiteSpace(expression.LabelFormat))
                        {
                            toolTip = string.Format("{0}: {1}", column.ColumnName, uHelper.FormatNumber(UGITUtility.StringToDouble(val), expression.LabelFormat));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(expression.LabelText))
                            {
                                toolTip = string.Format("{0}: {1}", column.ColumnName, expression.LabelText.Replace("$Exp$", uHelper.FormatNumber(UGITUtility.StringToDouble(val), string.Empty)));
                            }
                        }


                        sr.Points[index].LegendText = string.Format("{0} ({1}{2})", column.ColumnName, val, expression.ShowInPercentage ? "%" : string.Empty);
                        if (expression.DataPointClickEvent == DatapointClickeEventType.Detail)
                        {
                            sr.Points[index].PostBackValue = string.Format("javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})", dimensionIndex, string.Empty, expressionIndex, HttpUtility.UrlEncode(column.ColumnName), (int)expression.DataPointClickEvent, string.Empty);
                            sr.Points[index].MapAreaAttributes = string.Format("onclick=\"javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})\"", dimensionIndex, string.Empty, expressionIndex, HttpUtility.UrlEncode(column.ColumnName), (int)expression.DataPointClickEvent, string.Empty);
                        }
                    }

                }

                //Only in case of chart type: line, spline, stepline. if Series has less then equal to 2 point then chanage chart type to column
                List<Series> seriesList = cPChart.Series.Where(x => x.ChartType == SeriesChartType.Line || x.ChartType == SeriesChartType.Spline || x.ChartType == SeriesChartType.StepLine).ToList();
                foreach (Series s in seriesList)
                {
                    if (s.Points.Count <= 2)
                    {
                        s.ChartType = SeriesChartType.Bubble;
                        s.SetCustomProperty("BubbleMaxSize", "7");
                        s.SetCustomProperty("BubbleMinSize", "7");
                    }
                }

                foreach (Legend legend in cPChart.Legends)
                {
                    legend.Font = new Font(legendFont, legendFontSize - 1, FontStyle.Regular, GraphicsUnit.Point);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            
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

        private List<object> PickTopDimensionPoint(DataTable data, ILookup<object, DataRow> lookupData)
        {
            ChartDimension dimension = ChartSetting.Dimensions[dimensionIndex];
            List<object> topPoints = new List<object>();
            if (dimension.PickTopDataPoint <= 0)
            {
                return topPoints;
            }

            if (data == null || !data.Columns.Contains(dimension.OperatorField))
            {
                return topPoints;
            }

            if (dimension.Operator == string.Empty)
            {

                if (dimension.DataPointOrder.ToLower() == "descending")
                {
                    topPoints = (from m in lookupData
                                 select new { key = m.Key, result = m.ToArray()[0][dimension.OperatorField] }).OrderByDescending(x => x.result).Take(dimension.PickTopDataPoint).Select(x => x.key).ToList();


                }
                else
                {

                    topPoints = (from m in lookupData
                                 select new { key = m.Key, result = m.ToArray()[0][dimension.OperatorField] }).OrderBy(x => x.result).Take(dimension.PickTopDataPoint).Select(x => x.key).ToList();


                }
            }
            else
            {
                if (dimension.DataPointOrder.ToLower() == "descending")
                {
                    topPoints = (from m in lookupData
                                 select new { key = m.Key, result = GetAggregateData(m.ToArray(), dimension.Operator, dimension.OperatorField) }).OrderByDescending(x => x.result).Take(dimension.PickTopDataPoint).Select(x => x.key).ToList();
                }
                else
                {

                    topPoints = (from m in lookupData
                                 select new { key = m.Key, result = GetAggregateData(m.ToArray(), dimension.Operator, dimension.OperatorField) }).OrderBy(x => x.result).Take(dimension.PickTopDataPoint).Select(x => x.key).ToList();
                }
            }

            return topPoints;
        }

        /// <summary>
        /// DrillDown Settings, work only in case of dimensions
        /// </summary>
        /// <param name="cPChart"></param>
        private void DrillDownSetting(Chart cPChart)
        {
            if (ChartSetting.Dimensions.Count > 1 && cPChart.Legends.IsUniqueName("DrillDown") && !this.isPreview)
            {
                Legend drillDownLegend = new Legend("DrillDown");
                cPChart.Legends.Add(drillDownLegend);
                drillDownLegend.Alignment = StringAlignment.Far;
                drillDownLegend.Docking = Docking.Bottom;
                drillDownLegend.DockedToChartArea = "NotSet";
                drillDownLegend.ShadowOffset = 0;
                drillDownLegend.HeaderSeparatorColor = Color.Bisque;

                LegendItem legendItem = new LegendItem();
                if (ChartSetting.Dimensions.Count > dimensionIndex)
                {
                    legendItem.PostBackValue = string.Format("javascript:setDrillDownFilter(this,'{0}')", dimensionIndex + 1);
                    legendItem.MapAreaAttributes = string.Format("onclick=\"javascript:setDrillDownFilter(this,'{0}')\"", dimensionIndex + 1);
                }
                else
                {
                    legendItem.PostBackValue = string.Format("javascript:setDrillDownFilter(this,'{0}')", 0);
                    legendItem.MapAreaAttributes = string.Format("onclick=\"javascript:setDrillDownFilter(this,'{0}')\"", 0);
                }

                //Show next dimension label
                string label = ChartSetting.Dimensions[dimensionIndex].Title;
                if (ChartSetting.Dimensions.Count - 1 == dimensionIndex)
                {
                    label = ChartSetting.Dimensions[0].Title;
                }
                else
                {
                    label = ChartSetting.Dimensions[dimensionIndex + 1].Title;
                }

                legendItem.Name = label;
                drillDownLegend.CustomItems.Add(legendItem);
                legendItem.ImageStyle = LegendImageStyle.Rectangle;
                legendItem.Image = "/_layouts/15/images/uGovernIT/down_arrow.png";
            }

        }

        /// <summary>
        /// Apply some basic property on chart using specified configuration and create a chart
        /// </summary>
        /// <returns></returns>
        private Chart BasicChartSetting()
        {
            Chart cPChart = new Chart();
            ChartArea cArea = new ChartArea("ChartArea1");
            cPChart.ChartAreas.Add(cArea);
            cArea.BorderWidth = 0;

            cArea.BackGradientStyle = GradientStyle.TopBottom;
            cArea.BackColor = ColorTranslator.FromHtml("#E7EAFE");
            cArea.BackSecondaryColor = Color.White;
            if (ViewType == 1 || ViewType == 0)
            {
                cArea.BackColor = ColorTranslator.FromHtml("#E7EAFE");
                cPChart.BackColor = ColorTranslator.FromHtml("#F8F8F8");

            }

            //Checks legend if required or not and set the Alignment and LegendDocking
            if (!ChartSetting.HideLegend)
            {
                Legend legend = new Legend("Legend1");
                if (ViewType == 1 || ViewType == 0)
                {
                    legend.BackColor = ColorTranslator.FromHtml("#F8F8F8");
                }
                cPChart.Legends.Add(legend);

                if (!string.IsNullOrEmpty(ChartSetting.LegendAlignment))
                {
                    legend.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), ChartSetting.LegendAlignment);
                }

                if (!string.IsNullOrEmpty(ChartSetting.LegendDocking))
                {
                    legend.Docking = (Docking)Enum.Parse(typeof(Docking), ChartSetting.LegendDocking);
                }
            }

            cArea.AxisX.LineColor = Color.Gainsboro;
            cArea.AxisY.LineColor = Color.Gainsboro;
            cArea.AxisX2.LineColor = Color.Gainsboro;
            cArea.AxisY2.LineColor = Color.Gainsboro;
            cArea.AxisX.MajorGrid.LineColor = Color.Gainsboro;
            cArea.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            cArea.AxisX2.MajorGrid.LineColor = Color.Gainsboro;
            cArea.AxisY2.MajorGrid.LineColor = Color.Gainsboro;

            cArea.AxisX.LabelStyle.ForeColor = cArea.AxisX2.LabelStyle.ForeColor = ColorTranslator.FromHtml("#2D2D2D");
            cArea.AxisY.LabelStyle.ForeColor = cArea.AxisX2.LabelStyle.ForeColor = ColorTranslator.FromHtml("#2D2D2D");

            cArea.AxisX.LabelStyle.Font = new Font(legendFont, legendFontSize, FontStyle.Regular, GraphicsUnit.Point);
            cArea.AxisY.LabelStyle.Font = new Font(legendFont, legendFontSize, FontStyle.Regular, GraphicsUnit.Point);
            cArea.AxisX2.LabelStyle.Font = new Font(legendFont, legendFontSize, FontStyle.Regular, GraphicsUnit.Point);
            cArea.AxisY2.LabelStyle.Font = new Font(legendFont, legendFontSize, FontStyle.Regular, GraphicsUnit.Point);
            cPChart.FormatNumber += cPChart_FormatNumber;
            cArea.AxisX.Interval = 1;


            //Check HideGrid property is on or off and set some chart appearance
            if (ChartSetting.HideGrid)
            {
                cArea.AxisX.MajorGrid.Enabled = false;
                cArea.AxisX.MajorTickMark.Enabled = false;
                cArea.AxisX.LineWidth = 0;
                cArea.AxisY.MajorGrid.Enabled = false;
                cArea.AxisY.MajorTickMark.Enabled = false;
                cArea.AxisY.LabelStyle.Enabled = false;
                cArea.AxisY.LineWidth = 0;
                cArea.AxisY2.MajorGrid.Enabled = false;
                cArea.AxisY2.MajorTickMark.Enabled = false;
                cArea.AxisY2.LabelStyle.Enabled = false;
                cArea.AxisY2.LineWidth = 0;
                if (ViewType == 1)
                {
                    cArea.BackColor = ColorTranslator.FromHtml("#F8F8F8");
                    cArea.BackSecondaryColor = ColorTranslator.FromHtml("#F8F8F8");
                }

            }

            if (this.isPreview)
            {
                string font = "Verdana";
                int fontSize = 8;
                cArea.AxisX.LabelStyle.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                cArea.AxisY.LabelStyle.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                cArea.AxisX2.LabelStyle.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                cArea.AxisY2.LabelStyle.Font = new Font(font, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                cArea.AxisX.LabelStyle.Angle = -45;
            }

            if (ChartSetting.AxisLabelStyleAngle != 0)
            {
                cArea.AxisX.LabelStyle.Angle = ChartSetting.AxisLabelStyleAngle;
            }

            return cPChart;
        }

        void cPChart_FormatNumber(object sender, FormatNumberEventArgs e)
        {
            Axis axis = (Axis)sender;

            string format = string.Empty;
            foreach (ChartExpression cExpression in ChartSetting.Expressions)
            {
                if (!string.IsNullOrWhiteSpace(cExpression.LabelFormat) && cExpression.YAsixType == ChartAxisType.Primary && axis.AxisName.ToString() == "Y")
                {
                    format = cExpression.LabelFormat;
                    break;
                }
                else if (!string.IsNullOrWhiteSpace(cExpression.LabelFormat) && cExpression.YAsixType == ChartAxisType.Secondary && axis.AxisName.ToString() == "Y2")
                {
                    format = cExpression.LabelFormat;
                    break;
                }
            }

            if (e.ElementType == ChartElementType.AxisLabels)
            {
                e.LocalizedValue = uHelper.FormatNumber(e.Value, format);
            }
        }

        /// <summary>
        /// Create chart series based on column of datapoints table
        /// </summary>
        /// <param name="cPChart"></param>
        /// <param name="datapointsTable"></param>
        private void CreateChartSeries(Chart cPChart, DataTable datapointsTable)
        {
            List<Series> seriesList = new List<Series>();
            cPChart.Series.Clear();

            DataColumnCollection columns = datapointsTable.Columns;
            for (int i = 2; i < columns.Count; i++)
            {
                DataColumn column = columns[i];

                //Get Expression associated with each column
                int expressionIndex = 0;
                int.TryParse(Convert.ToString(columns[i].ExtendedProperties["expressionIndex"]), out expressionIndex);
                ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                SeriesChartType chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), expression.ChartType);
                if (chartType == SeriesChartType.Pie || chartType == SeriesChartType.Funnel || chartType == SeriesChartType.Pyramid || chartType == SeriesChartType.Doughnut)
                {
                    expression.GroupByField = string.Empty;
                }

                Series series = new Series(column.ColumnName);
                series.ChartType = chartType;
                //Set expressionindex and expressionname property for each series
                series.SetCustomProperty("expressionIndex", ChartSetting.Expressions.IndexOf(expression).ToString());
                series.SetCustomProperty("expressionName", column.ColumnName);
                series.LegendText = column.ColumnName;
                series.IsValueShownAsLabel = true;
                series.SetCustomProperty("LabelStyle", expression.LabelStyle);
                series.SetCustomProperty("BarLabelStyle", expression.LabelStyle);
                series.EmptyPointStyle.IsValueShownAsLabel = false;


                //Changes chart palette
                if (!string.IsNullOrEmpty(expression.Palette))
                {
                    series.Palette = (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), expression.Palette);
                    cPChart.Palette = series.Palette;
                }

                series.YAxisType = AxisType.Primary;
                if (expression.YAsixType == ChartAxisType.Secondary)
                {
                    series.YAxisType = AxisType.Secondary;
                }

                if (string.IsNullOrEmpty(expression.LabelColor))
                    expression.LabelColor = "#000000";
                series.LabelForeColor = System.Drawing.ColorTranslator.FromHtml(expression.LabelColor);
                if (expression.HideLabel || this.isPreview)
                {
                    series.IsValueShownAsLabel = false;
                    series.MarkerStyle = MarkerStyle.None;
                }

                series.IsVisibleInLegend = true;
                if (ChartSetting.HideLegend || this.isPreview)
                {
                    series.IsVisibleInLegend = false;
                }


                if (expression.ChartType == "Spline" || expression.ChartType == "Line")
                {
                    series.SetCustomProperty("DrawingStyle", "Cylinder");
                }

                if (expression.ChartType == "Bar" || expression.ChartType == "StackedBar" || expression.ChartType == "Column" || expression.ChartType == "StackedColumn")
                {
                    series.SetCustomProperty("DrawingStyle", expression.DrawingStyle);
                }

                series.BorderWidth = ChartSetting.BorderWidth > 0 ? ChartSetting.BorderWidth : 1;
                seriesList.Add(series);
            }

            //Set some basic setting depend on charttype
            foreach (Series sers in seriesList)
            {
                if (sers.ChartType == SeriesChartType.Pie)
                {
                    sers.SetCustomProperty("DoughnutRadius", "25");
                    sers.SetCustomProperty("PieDrawingStyle", "SoftEdge"); // "Concave"
                }
                else if (sers.ChartType == SeriesChartType.Doughnut)
                {
                    sers.SetCustomProperty("PieDrawingStyle", "SoftEdge"); // "Concave"
                }
                else if (sers.ChartType == SeriesChartType.Pyramid)
                {
                    sers.SetCustomProperty("FunnelMinPointHeight", "2");
                    sers.SetCustomProperty("PyramidPointGap", "2");
                    sers.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                    cPChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                }
                else if (sers.ChartType == SeriesChartType.Funnel)
                {
                    sers.SetCustomProperty("FunnelMinPointHeight", "2");
                    sers.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                    cPChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                }
                else if (sers.ChartType == SeriesChartType.Bar)
                {
                    sers.SetCustomProperty("BarLabelStyle", "Center");
                }

                cPChart.Series.Add(sers);
            }
        }


        /// <summary>
        /// This method saves and return chart url based on panelid
        /// </summary>
        /// <param name="panelID"></param>
        /// <returns></returns>
        public static string[] GetChartByPanelId(string panelID, int width, int height, ApplicationContext context)
        {
            string[] chartDetails = new string[2];
            string chartUrl = string.Empty;
            try
            {
                int panelId = Convert.ToInt32(panelID);
                DashboardManager dManager = new DashboardManager(context);
                Dashboard dashboard = dManager.LoadPanelById(panelId);
                if (width == 0)
                {
                    width = dashboard.PanelWidth;
                }
                if (height == 0)
                {
                    height = dashboard.PanelHeight;
                }

                if (dashboard.DashboardType == DashboardType.Chart)
                {
                    ChartSetting panel = (ChartSetting)dashboard.panel;
                    ChartHelper chartH = new ChartHelper(panel, context);
                    chartH.ChartTitle = dashboard.Title;
                    Chart cPChart = chartH.CreateChart(true, Convert.ToString(width), Convert.ToString(height), false);
                    cPChart.ImageStorageMode = ImageStorageMode.UseImageLocation;

                    string tempPath = uHelper.GetTempFolderPath();
                    string fileName = Guid.NewGuid() + "chart.png";
                    tempPath = tempPath + "/" + fileName;
                    try
                    {
                        cPChart.SaveImage(tempPath, ChartImageFormat.Png);
                    }
                    catch (Exception) { }
                    chartUrl = string.Format("/_layouts/15/images/ugovernittemp/{0}", fileName);
                    chartDetails[0] = chartUrl;
                    chartDetails[1] = chartH.ChartTitle;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return chartDetails;
        }


        private void CreateChartSeriesUsingRow(Chart cPChart, DataTable datapointsTable)
        {
            List<Series> seriesList = new List<Series>();
            ChartExpression firstExpression = ChartSetting.Expressions.FirstOrDefault();
            cPChart.Palette = (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), firstExpression.Palette);
            cPChart.Series.Clear();
            DataColumnCollection columns = null; ;
            if (datapointsTable != null)
            {
                columns = datapointsTable.Columns;
            }

            for (int i = 0; i < datapointsTable.Rows.Count; i++)
            {
                DataColumn column = columns[i];

                //Get Expression associated with each column
                int expressionIndex = 0;
                int.TryParse(Convert.ToString(columns[i].ExtendedProperties["expressionIndex"]), out expressionIndex);
                ChartExpression expression = ChartSetting.Expressions[expressionIndex];

                DataRow row = datapointsTable.Rows[i];
                string name = Convert.ToString(row[1]);
                Series series = new Series(Convert.ToString(name));
                series.ChartType = SeriesChartType.StackedColumn;
                series.LegendText = name;
                series.IsValueShownAsLabel = !firstExpression.HideLabel;
                series.SetCustomProperty("LabelStyle", "Auto");
                series.SetCustomProperty("BarLabelStyle", "Auto");
                series.SetCustomProperty("DrawingStyle", "Cylinder");
                series.EmptyPointStyle.IsValueShownAsLabel = false;
                series.YAxisType = AxisType.Primary;

                series.IsVisibleInLegend = true;
                if (ChartSetting.HideLegend || this.isPreview)
                {
                    series.IsVisibleInLegend = false;
                }

                if (string.IsNullOrEmpty(firstExpression.LabelColor))
                    firstExpression.LabelColor = "#000000";
                series.LabelForeColor = System.Drawing.ColorTranslator.FromHtml(firstExpression.LabelColor);

                series.BorderWidth = ChartSetting.BorderWidth > 0 ? ChartSetting.BorderWidth : 1;
                if (expression.ChartType == "Spline" || expression.ChartType == "Line")
                {
                    series.SetCustomProperty("DrawingStyle", "Cylinder");
                }

                if (expression.ChartType == "Bar" || expression.ChartType == "StackedBar" || expression.ChartType == "Column" || expression.ChartType == "StackedColumn")
                {
                    series.SetCustomProperty("DrawingStyle", expression.DrawingStyle);
                }
                seriesList.Add(series);
            }

            //Set some basic setting depend on charttype
            foreach (Series sers in seriesList)
            {
                cPChart.Series.Add(sers);
            }

        }

        private void PlotChartPointsReverse(Chart cPChart, DataTable datapointsTable)
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
                        ChartExpression expression = ChartSetting.Expressions.FirstOrDefault(x => x.Title.ToLower() == column.ColumnName.ToLower());
                        ChartDimension dimension = ChartSetting.Dimensions[0];

                        if (expression != null)
                        {
                            double val = UGITUtility.StringToDouble(datapoint[column.ColumnName]);
                            //Calculate percentage if property is true;
                            if (expression != null && expression.ShowInPercentage)
                            {
                                double totalCount = datapointsTable.AsEnumerable().Sum(x => x.Field<double>(column.ColumnName));
                                val = Math.Round((val / totalCount) * 100, 0);
                            }

                            string name = Convert.ToString(datapoint[1]);
                            Series sr = cPChart.Series[name];
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

                            int index = sr.Points.AddXY(column.ColumnName, val);

                            //Set other properties of datapoint
                            if (!expression.HideLabel)
                            {
                                if (!string.IsNullOrEmpty(expression.LabelText))
                                {
                                    sr.Points[index].Label = expression.LabelText.Replace("$Exp$", val.ToString());
                                }
                                else
                                {
                                    sr.Points[index].Label = val.ToString();
                                }
                            }

                            if (val == 0)
                            {
                                sr.Points[index].Label = " ";
                                sr.Points[index].MarkerStyle = MarkerStyle.None;
                            }

                            //Set tooltip
                            string toolTip = string.Format("{0}: {1}", xVal, val);
                            if (!string.IsNullOrEmpty(expression.LabelText))
                            {
                                toolTip = string.Format("{0}: {1}", xVal, expression.LabelText.Replace("$Exp$", val.ToString()));
                            }
                            sr.Points[index].ToolTip = toolTip;

                            if (this.isPreview)
                            {
                                sr.IsValueShownAsLabel = false;
                                sr.Label = string.Empty;
                                sr.Points[index].Label = string.Empty;
                                sr.MarkerStyle = MarkerStyle.None;
                                sr.Points[index].IsValueShownAsLabel = false;
                            }

                            sr.Points[index].LegendToolTip = sr.Name;
                            sr.Points[index].LegendText = string.Format("{0} ({1}{2})", datapoint[1], val, expression.ShowInPercentage ? "%" : string.Empty);

                            if (clickEvent != DatapointClickeEventType.None && clickEvent != DatapointClickeEventType.Inherit)
                            {
                                string dimsVal = Convert.ToString(dataPointDetailVal);
                                if (dimensionFilter != null)
                                    dimsVal = string.Format("{0}>{1}", dimensionFilter, dimsVal);

                                dimensionIndex = 0;
                                int expressionIndex = ChartSetting.Expressions.IndexOf(expression);
                                sr.Points[index].PostBackValue = string.Format("javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})", dimensionIndex, HttpUtility.UrlEncode(dimsVal), expressionIndex, HttpUtility.UrlEncode(sr.Name), (int)clickEvent, HttpUtility.UrlEncode(internalTitleValue.ToString()));
                                sr.Points[index].MapAreaAttributes = string.Format("onclick=\"javascript:setDatapointFilter(this,'{0}*{1}*{2}*{3}*{4}*{5}',{4})\"", dimensionIndex, HttpUtility.UrlEncode(dimsVal), expressionIndex, HttpUtility.UrlEncode(sr.Name), (int)clickEvent, HttpUtility.UrlEncode(internalTitleValue.ToString()));
                            }
                        }
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
                if (view.ViewProperty is IndivisibleDashboardsView || view.ViewProperty is SuperDashboardsView)
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
                            query.AppendFormat("{0} Like '%{1}%' ", column.ColumnName, UGITUtility.RemoveWildCardFromQuery(values[i]));
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
            DataTable factTable = DashboardCache.GetCachedDashboardData(context, Convert.ToString(item.ListName));
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
                //List<string> lookups = string.Format("0;#{0}", mRow["Value"]);

                //foreach (SPFieldLookupValue lookup in lookups)
                //{
                //    filterValues.Rows.Add(lookup.LookupValue);
                //}
                string lookups = string.Format("0;#{0}", mRow["Value"]);
                filterValues.Rows.Add(lookups);
                filterValues.Rows.Remove(mRow);
            }

            filterValues.DefaultView.Sort = "Value asc";
            return filterValues.DefaultView.ToTable(true, "Value");
        }
    }
}