using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Manager
{
    public class QueryPreviewHelperManager
    {
        private ASPxGridView gridView { get; set; }
        public ApplicationContext  _context = null;
        public long queryID { get; set; }
        public DashboardQuery query { get; set; }
        public ZoomLevel zoomLevel { get; set; }
        public bool FilterMode { get; set; }
        public bool diplayGantt { get; set; }
        public DataTable ResultData { get; set; }
        public List<WhereInfo> whereInfo { get; set; }
        public string where { get; set; }

        private DataTable moduleMonitorsTable = null;
        private DataTable projectMonitorsStateTable = null;

        public QueryPreviewHelperManager(ASPxGridView gv, ApplicationContext context)
        {
            gridView = gv;
            _context = context;
        }
        public QueryPreviewHelperManager(long id, ASPxGridView gv, ApplicationContext context)
        {
            gridView = gv;
            _context = context;
            queryID = id;
        }

        public ASPxGridView InitializeGrid()
        {
            gridView.Settings.ShowFilterRowMenu = true;
            gridView.Settings.ShowHeaderFilterButton = true;
            gridView.Settings.ShowFooter = true;
            gridView.SettingsPopup.HeaderFilter.Height = 200;
            gridView.Settings.ShowGroupPanel = true;
            gridView.SettingsPager.PageSize = 15;
            gridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            gridView.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleAlways;
            gridView.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;

            gridView.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gridView.Styles.AlternatingRow.BackColor = Color.FromArgb(234, 234, 234);
            gridView.Styles.Row.BackColor = Color.White;
            gridView.Styles.GroupRow.Font.Bold = true;
            return gridView;
        }

        public ASPxGridView BindGrid(DataTable dataTable)
        {
            if (gridView.Columns.Count <= 0)
            {
                var columns = new List<Utility.ColumnInfo>();
                string groupedcol = string.Empty;

                // Load the dashboard
                DashboardManager dManager = new DashboardManager(_context);
                Dashboard uDashboard = dManager.LoadPanelById(queryID, false);
                DashboardQuery panel = uDashboard.panel as DashboardQuery;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (panel.QueryInfo.Tables != null && panel.QueryInfo.Tables.Count > 0)
                    {
                        foreach (var t in panel.QueryInfo.Tables)
                        {
                            columns.AddRange(t.Columns);
                        }
                    }
                    
                    // Adding Columns in Grid 
                    foreach (var col in columns.OrderBy(x => x.Sequence).ToList())
                    {
                        if (col.FieldName == DatabaseObjects.Columns.ProjectHealth)
                        {
                            if (moduleMonitorsTable == null)
                            {
                                // Load All Monitor in case of pmm projects
                                projectMonitorsStateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");

                                // cache isn't implemented yet so we are getting the monitors directly from table
                                string monitorQuery = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM, DatabaseObjects.Columns.TenantID, _context.TenantID);
                                moduleMonitorsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, monitorQuery);
                            }
                            if (moduleMonitorsTable != null && moduleMonitorsTable.Rows.Count > 0)
                            {
                                int i = 1;
                                foreach (DataRow monitor in moduleMonitorsTable.Rows)
                                {
                                    int visibleindex = col.Sequence + i;
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.Title))
                                    {
                                        GridViewDataTextColumn colId = new GridViewDataTextColumn();
                                        colId.PropertiesTextEdit.EncodeHtml = false;
                                        colId.FieldName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ProjectMonitorName).ToString();
                                        colId.Name = string.Format("projecthealth{0}", moduleMonitorsTable.Rows.IndexOf(monitor));
                                        colId.Caption = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.Title).ToString(); // "Issues Scope $$$$ OnTime Risk";  // 5 monitors: Issues, Scope, Budget, Time, Risk
                                        colId.Width = new Unit("50px");
                                        colId.PropertiesEdit.DisplayFormatString = "picture";
                                        //colId.VisibleIndex = visibleindex;
                                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                        colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                        colId.HeaderStyle.Font.Bold = true;
                                        gridView.Columns.Add(colId);
                                        i = i + 1;
                                    }
                                }
                            }
                        }
                        else if (col.FieldName == "GanttView")
                        {
                            diplayGantt = true;
                        }
                        else if (!col.Hidden)
                            AddColumn(col);
                        else
                        {
                            if (panel.QueryInfo.GroupBy != null && panel.QueryInfo.GroupBy.Count > 0)
                            {
                                if (panel.QueryInfo.GroupBy.Exists(x => x.Column.FieldName == col.FieldName))
                                {
                                    AddColumn(col);
                                }
                            }
                        }
                    }

                    // Group by Section
                    if (panel.QueryInfo.GroupBy != null && panel.QueryInfo.GroupBy.Count > 0)
                    {
                        foreach (GroupByInfo groupby in panel.QueryInfo.GroupBy)
                        {
                            gridView.GroupBy(gridView.Columns[groupby.Column.DisplayName]);
                        }
                    }

                    // Order by section
                    if (panel.QueryInfo.OrderBy != null && panel.QueryInfo.OrderBy.Count > 0)
                    {
                        foreach (OrderByInfo ob in panel.QueryInfo.OrderBy)
                        {
                            DevExpress.Data.ColumnSortOrder orderby = (ob.orderBy == OrderBY.ASC) ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending;

                            gridView.SortBy(gridView.Columns[ob.Column.DisplayName], orderby);
                        }
                    }

                    // Adding Total Fields.
                    if (panel.QueryInfo.Totals != null && panel.QueryInfo.Totals.Count > 0)
                    {
                        foreach (Utility.ColumnInfo c in panel.QueryInfo.Totals)
                        {
                            DevExpress.Data.SummaryItemType sumtype = DevExpress.Data.SummaryItemType.None;
                            switch (c.Function)
                            {
                                case "Sum":
                                    sumtype = DevExpress.Data.SummaryItemType.Sum;
                                    break;
                                case "Count":
                                    sumtype = DevExpress.Data.SummaryItemType.Count;
                                    break;
                                case "Avg":
                                    sumtype = DevExpress.Data.SummaryItemType.Average;
                                    break;
                                case "Min":
                                    sumtype = DevExpress.Data.SummaryItemType.Min;
                                    break;
                                case "Max":
                                    sumtype = DevExpress.Data.SummaryItemType.Max;
                                    break;
                                default:
                                    break;
                            }

                            ASPxSummaryItem sumItem = new ASPxSummaryItem(c.DisplayName, sumtype);
                            sumItem.ShowInGroupFooterColumn = c.DisplayName;
                            gridView.GroupSummary.Add(sumItem);
                            gridView.TotalSummary.Add(new ASPxSummaryItem(c.DisplayName, sumtype));
                        }
                    }

                    //To create gantt view.
                    GenerateGanttColumn(dataTable);
                }
            }
            gridView.DataSource = dataTable;
            gridView.DataBind();
            return gridView;
        }

        private void AddColumn(Utility.ColumnInfo columnInfo)
        {
            GridViewDataTextColumn gvDatatextCol = new GridViewDataTextColumn();
            gvDatatextCol.FieldName = columnInfo.DisplayName;
            gvDatatextCol.Caption = columnInfo.DisplayName;
            gvDatatextCol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            if (FilterMode)
            {
                gvDatatextCol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                gvDatatextCol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                gvDatatextCol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                gvDatatextCol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            }

            gvDatatextCol.HeaderStyle.Font.Bold = true;
            gvDatatextCol.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            if (columnInfo.DisplayName != "Title")
            {
                gvDatatextCol.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                gvDatatextCol.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                gvDatatextCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvDatatextCol.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            switch (columnInfo.DataType)
            {
                case "Currency":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:c}";
                    break;
                case "DateTime":
                case "System.DateTime":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:MMM-d-yyyy hh:mm tt}";
                    break;
                case "Date":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:MMM-d-yyyy}";
                    break;
                case "Time":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:HH:mm}";
                    break;
                case "Double":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:#,0.##}";
                    break;
                case "Percent":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:0.#}%";
                    break;
                case "Percent*100":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:0.#%}";
                    break;
                case "Boolean":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "Yes;No";
                    break;
                default:
                    break;
            }
            gridView.Columns.Add(gvDatatextCol);
        }

        public ASPxGridView GenerateGanttColumn(DataTable dtable)
        {
            if (diplayGantt && dtable != null && dtable.Rows.Count > 0)
            {
                //dvZoomLevel.Visible = true;
                gridView.Settings.ShowGroupPanel = false;
                gridView.SettingsBehavior.AllowDragDrop = false;
                gridView.Settings.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Auto;

                DateTime dtMax = DateTime.MaxValue;
                DateTime dtMin = DateTime.MinValue;

                query.QueryInfo.Tables.ForEach(x =>
                {
                    if (x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate) != null)
                    {
                        string dspName = x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate).DisplayName;
                        var compDate = dtable.AsEnumerable().Where(m => !m.IsNull(dspName));
                        dtMax = compDate.Count() == 0 ? DateTime.MaxValue : compDate.Max(m => m.Field<DateTime>(dspName));
                    }

                    if (x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate) != null)
                    {
                        string dspName = x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate).DisplayName;

                        var stDate = dtable.AsEnumerable().Where(m => !m.IsNull(dspName));
                        dtMin = stDate.Count() == 0 ? DateTime.MinValue : stDate.Min(m => m.Field<DateTime>(dspName));
                    }
                });

                if (dtMax != DateTime.MaxValue && dtMin != DateTime.MinValue)
                {
                    switch (zoomLevel)
                    {
                        case ZoomLevel.Monthly:

                            for (DateTime startDate = dtMin; startDate < dtMax.AddMonths(6); startDate = startDate.AddMonths(6))
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                int monthBar = startDate.Month > 6 ? 12 : 6;
                                bndColumn.Name = (monthBar == 6 ? 1 : 7) + " " + startDate.Year.ToString();

                                bndColumn.Caption = (monthBar == 6 ? "January" : "July") + " " + startDate.Year.ToString();

                                bndColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = monthBar > 6 ? 7 : 1; i <= monthBar; i++)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = i.ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);

                                    bndColumn.Columns.Add(subcol);
                                }

                                gridView.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.Quarterly:

                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = 0; i < 12; i += 3)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = (i + 1).ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);

                                    bndColumn.Columns.Add(subcol);
                                }
                                gridView.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.HalfYearly:
                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = 0; i < 12; i += 6)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = (i + 1).ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);
                                    bndColumn.Columns.Add(subcol);
                                }
                                gridView.Columns.Add(bndColumn);
                            }
                            break;

                        case ZoomLevel.Yearly:
                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                subcol.Name = "1"; // For january
                                subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1);
                                subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;

                                subcol.HeaderStyle.Font.Bold = true;
                                bndColumn.Columns.Add(subcol);

                                string colName = subcol.Name + "-" + bndColumn.Name;
                                if (!ResultData.Columns.Contains(colName))
                                    ResultData.Columns.Add(colName);

                                gridView.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.Weekly:
                            for (DateTime startDate = dtMin; startDate < dtMax; startDate = startDate.AddMonths(1))
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Name = startDate.Month.ToString() + " " + startDate.Year.ToString();
                                bndColumn.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month) + " " + startDate.Year.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;
                                DateTime dt = new DateTime(startDate.Year, startDate.Month, 1);
                                for (DateTime ctrDate = dt; ctrDate.Day < DateTime.DaysInMonth(startDate.Year, startDate.Month); ctrDate = ctrDate.AddDays(1))
                                {
                                    if (ctrDate.DayOfWeek == DayOfWeek.Monday)
                                    {
                                        GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                        subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                        subcol.Name = (ctrDate.Day).ToString();
                                        subcol.Caption = (ctrDate.Day).ToString();
                                        subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                        subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                        subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                        subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                        subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                        subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.HeaderStyle.Font.Bold = true;

                                        string colName = subcol.Name + "-" + bndColumn.Name;
                                        if (!ResultData.Columns.Contains(colName))
                                            ResultData.Columns.Add(colName);


                                        bndColumn.Columns.Add(subcol);
                                    }
                                }
                                gridView.Columns.Add(bndColumn);
                            }
                            break;
                    }
                }
            }
            return gridView;
        }

        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            DataTable dtTotals = new DataTable();
            QueryHelperManager queryHelper = new QueryHelperManager(_context);
            dt = queryHelper.GetReportData(query.QueryInfo, where, ref dtTotals, false);
            return dt;
        }

        private Unit GetWidthByZoomLevel(ZoomLevel zoom)
        {
            Unit width = Unit.Pixel(30);
            switch (zoom)
            {
                case ZoomLevel.Yearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.HalfYearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.Quarterly:
                    width = Unit.Pixel(45);
                    break;
                case ZoomLevel.Monthly:
                    width = Unit.Pixel(30);
                    break;
                case ZoomLevel.Weekly:
                    width = Unit.Pixel(30);
                    break;
                default:
                    width = Unit.Pixel(30);
                    break;
            }
            return width;
        }

        public void FillGantViewdata()
        {
            List<GridViewColumn> gvDataColumn = gridView.AllColumns.Where(m => m as GridViewBandColumn == null).ToList();
            List<GridViewColumn> gvBandColumn = gridView.AllColumns.Where(m => m as GridViewBandColumn != null).ToList();

            if (gvBandColumn.Count == 0)
                return;

            for (int i = 0; i < ResultData.Rows.Count; i++)
            {
                DataRow currentRow = ResultData.Rows[i];// gridView.GetDataRow(i);

                if (currentRow == null)
                    continue;

                DateTime dtStart = DateTime.MinValue;
                DateTime dtEnd = DateTime.MinValue;
                DateTime? pctCompleteDate = null;
                
                if (currentRow != null)
                {
                    query.QueryInfo.Tables.ForEach(x =>
                    {
                        if (x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate).DisplayName;
                            DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtStart);
                        }

                        if (x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate).DisplayName;

                            DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtEnd);
                        }

                        if (x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => ResultData.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete).DisplayName;
                            if (!(currentRow[colName] is DBNull) && dtEnd != DateTime.MinValue && dtStart != DateTime.MinValue)
                            {
                                int days = (int)(Convert.ToDouble(currentRow[colName]) * (dtEnd - dtStart).Days);
                                pctCompleteDate = dtStart.AddDays(days);
                            }
                        }
                    });
                }

                for (int j = 0; j < gvDataColumn.Count; j++)
                {
                    #region GanttView
                    int ganttStartIndex = gridView.Columns.Count - gvBandColumn.Count;

                    if (!string.IsNullOrEmpty(((GridViewDataColumn)gvDataColumn[j]).Name) &&
                        !((GridViewDataColumn)gvDataColumn[j]).Name.StartsWith("projecthealth"))
                    {
                        if (dtStart != DateTime.MinValue && dtEnd != DateTime.MinValue)
                        {
                            string header = ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Name;
                            int yr = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] : header);
                            int month = 1;
                            int day = 1;

                            if (zoomLevel == ZoomLevel.Weekly)
                            {
                                month = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] : "1");
                                day = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);

                                DateTime dtGrid = new DateTime(yr, month, day);
                                if (dtGrid >= dtStart && dtGrid <= dtEnd)
                                {
                                    int bandcolumncount = 0;

                                    for (int k = ganttStartIndex + 1; ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index >= k; k++)
                                    {
                                        bandcolumncount += ((GridViewBandColumn)gridView.Columns[k - 1]).Columns.Count;
                                    }

                                    int index = j + ganttStartIndex + bandcolumncount;
                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "████████████████";
                                }
                            }
                            else
                            {
                                month = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);
                                int period = zoomLevel == ZoomLevel.Yearly ? 12 : (zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1));

                                DateTime dtGrid = new DateTime(yr, month, day);
                                DateTime dtPreGrid = dtGrid.AddMonths(-period);
                                DateTime dtPostGrid = dtGrid.AddMonths(+period);

                                if (dtGrid >= dtStart && dtGrid <= dtEnd && dtPostGrid <= dtEnd)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gridView.Columns[ganttStartIndex + 1]).Columns.Count;

                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "████████████████";
                                }
                                else if (dtStart >= dtPreGrid && dtStart <= dtPostGrid && dtPostGrid <= dtEnd)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gridView.Columns[ganttStartIndex + 1]).Columns.Count;
                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "███████";
                                }
                                else if (dtEnd >= dtGrid && dtEnd <= dtPostGrid)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gridView.Columns[ganttStartIndex + 1]).Columns.Count;
                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "██████";
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
        }
    }
}