using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web;
using DevExpress.Web;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using DevExpress.XtraPrinting;
using uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class DailyTicketCountTrendsControl : UserControl
    {
        DataTable finalResult = new DataTable();
        List<DataColumn> lstOfColumnsToDraw = new List<DataColumn>();
        DateTime dtStartDate;
        DateTime dtEndDate;
        ApplicationContext context;
        DataTable dtTicketCountTrends;
        DataTable rowTable;
        string pickerMode;
        DateTime dateRangeStart = DateTime.MinValue;
        DateTime dateRangeEnd = DateTime.MinValue;
        public string dailyTicketCountTrends = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=dailyticketcounttrends");
        string currentExportType = "pdf";
        protected bool resolveStageExists;
        private bool needScrollPositioning;

        public string ContentTitle { get; set; }
        public string StringOfSelectedModule { get; set; }
        public string DefaultModule { get; set; }

        TicketCountTrendsManager ticketCountTrendsManager;

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            grdCountTrends.SettingsBehavior.AllowGroup = false;
            grdCountTrends.SettingsBehavior.AllowSort = false;
            grdCountTrends.Width = Unit.Pixel(902);
            ticketCountTrendsManager = new TicketCountTrendsManager(context);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !Page.IsCallback)
            {
                needScrollPositioning = true;
                BindModule();
                IsResolveStageExist();
                WeekStartEndDate();
            }

            setRowStyle();

            grdCountTrends.DataBind();
        }

        protected void pnlcallCountTrends_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string view = Convert.ToString(ddlview.Value);
            if (view.Equals("weekly"))
                grdCountTrends.Width = Unit.Pixel(782);
            else
                grdCountTrends.Width = Unit.Pixel(902);

            if (!string.IsNullOrEmpty(e.Parameter))
                pickerMode = e.Parameter;

            IsResolveStageExist();
            setRowStyle();

            WeekStartEndDate();
            grdCountTrends.DataBind();
        }

        private void setRowStyle()
        {
            if (resolveStageExists)
            {
                grdCountTrends.Styles.Row.Reset();
                grdCountTrends.Styles.Row.CssClass = "clsrowResolved";
            }
            else
            {
                grdCountTrends.Styles.Row.Reset();
                grdCountTrends.Styles.Row.CssClass = "clsrowNoResolved";
            }
        }

        protected void grdCountTrends_DataBinding(object sender, EventArgs e)
        {
            CreateEachDayAsColumn();
            CreateTableSchema();

            //Set scroll position for daily view first time
            bool valid = grdCountTrends.Columns != null && grdCountTrends.Columns.Count > 0 && Convert.ToString(ddlview.Value) == "daily";
            if (needScrollPositioning && valid)
            {
                DateTime firstDayOfMonth = uHelper.FirstDayOfMonth(DateTime.Now.Date);
                DateTime currentWeekSunday = uHelper.FirstDayOfWeek(context, DateTime.Now.Date);
                if (currentWeekSunday.DayOfWeek == DayOfWeek.Sunday && currentWeekSunday >= firstDayOfMonth)
                {
                    string colName = Convert.ToString(uHelper.FirstDayOfWeek(context, DateTime.Now.Date));
                    GridViewColumn col = grdCountTrends.Columns[colName];
                    if (col != null)
                        ScrollGridToColumn(grdCountTrends, col);
                }

            }
            grdCountTrends.DataSource = finalResult;
        }

        private void WeekStartEndDate()
        {
            DateTime tempDate = DateTime.MinValue;
            DateTime tempSDate = DateTime.MinValue;
            DateTime tempEDate = DateTime.MinValue;

            if (hdnkeepdaterange.Contains("range"))
            {
                string range = Convert.ToString(hdnkeepdaterange.Get("range"));

                if (!string.IsNullOrEmpty(range))
                {
                    string[] dateArr = new string[0];
                    if (Convert.ToString(ddlview.Value) == "monthly")
                    {
                        dateArr = new string[] { range };
                        int year = UGITUtility.StringToInt(dateArr[0]);
                        tempSDate = new DateTime(year, 1, 1).Date;
                        tempEDate = new DateTime(year, 12, 31).Date;
                        if (!string.IsNullOrEmpty(pickerMode))
                        {
                            if (pickerMode == "p")
                            {
                                tempDate = tempSDate;
                                dtStartDate = new DateTime(tempDate.Year - 1, 1, 1);
                                dtEndDate = new DateTime(dtStartDate.Year, 12, 31);
                            }
                            else if (pickerMode == "date")
                            {
                                if (dtcStartdate.Value != null && dtcStartdate.SelectedDate.Date != DateTime.MinValue.Date)
                                {
                                    tempDate = dtcStartdate.SelectedDate.Date;
                                    dtStartDate = new DateTime(tempDate.Year, 1, 1).Date;
                                    dtEndDate = new DateTime(dtStartDate.Year, 12, 31).Date;
                                }
                                else
                                {
                                    tempDate = DateTime.Today.Date;
                                    dtStartDate = new DateTime(tempDate.Year, 1, 1).Date;
                                    dtEndDate = new DateTime(dtStartDate.Year, 12, 31).Date;
                                }
                            }
                            else
                            {
                                tempDate = tempEDate;
                                dtStartDate = new DateTime(tempEDate.Year + 1, 1, 1).Date;
                                dtEndDate = new DateTime(dtStartDate.Year, 12, 31).Date;
                            }
                        }
                        else
                        {
                            dtStartDate = tempSDate.Date;
                            dtEndDate = tempEDate.Date;
                        }

                        hdnkeepdaterange.Set("range", dtStartDate.Year);
                    }
                    else if (Convert.ToString(ddlview.Value) == "weekly")
                    {
                        dateArr = range.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                        tempSDate = Convert.ToDateTime(dateArr[0]);
                        tempEDate = Convert.ToDateTime(dateArr[1]);
                        if (!string.IsNullOrEmpty(pickerMode))
                        {
                            if (pickerMode == "p")
                            {
                                DateTime dt = tempSDate.StartOfWeek(DayOfWeek.Monday);

                                tempEDate = tempSDate;
                                tempSDate = dt.AddDays(-(9 * 7));
                                dtStartDate = tempSDate.Date;
                                dtEndDate = tempEDate;
                            }
                            else if (pickerMode == "date")
                            {
                                if (dtcStartdate.Value != null && dtcStartdate.SelectedDate.Date != DateTime.MinValue.Date)
                                {
                                    tempDate = dtcStartdate.SelectedDate.Date;

                                    DateTime dt = tempDate.StartOfWeek(DayOfWeek.Monday);

                                    tempEDate = tempDate;
                                    tempSDate = dt.AddDays(-(9 * 7));
                                    dtStartDate = tempSDate.Date;
                                    dtEndDate = tempEDate;
                                }
                                else
                                {

                                    tempDate = DateTime.Today.Date;
                                    DateTime dt = DateTime.Today.StartOfWeek(DayOfWeek.Monday);

                                    tempEDate = DateTime.Today.Date;
                                    tempSDate = dt.AddDays(-(9 * 7));
                                    dtStartDate = tempSDate.Date;
                                    dtEndDate = tempEDate;
                                }
                            }
                            else
                            {
                                tempDate = tempEDate;
                                DateTime dt = tempDate.StartOfWeek(DayOfWeek.Monday);
                                tempSDate = tempDate;
                                dtStartDate = tempSDate;
                                dtEndDate = dt.AddDays((9 * 7));
                            }
                        }
                        else
                        {
                            dtStartDate = tempSDate.Date;
                            dtEndDate = tempEDate.Date;
                        }
                        hdnkeepdaterange.Set("range", string.Format("{0};#{1}", dtStartDate.Date, dtEndDate.Date));
                    }
                    else
                    {
                        dateArr = range.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                        tempSDate = Convert.ToDateTime(dateArr[0]);
                        tempEDate = Convert.ToDateTime(dateArr[1]);
                        if (!string.IsNullOrEmpty(pickerMode))
                        {
                            if (pickerMode == "p")
                            {
                                tempDate = tempSDate;
                                dtStartDate = uHelper.FirstDayOfMonth(tempDate).AddMonths(-1);
                                dtEndDate = uHelper.LastDayOfMonth(dtStartDate);
                            }
                            else if (pickerMode == "date")
                            {
                                if (dtcStartdate.Value != null && dtcStartdate.SelectedDate.Date != DateTime.MinValue.Date)
                                {
                                    tempDate = dtcStartdate.SelectedDate.Date;
                                }
                                else
                                {
                                    tempDate = DateTime.Today.Date;
                                }
                                dtStartDate = uHelper.FirstDayOfMonth(tempDate);
                                dtEndDate = uHelper.LastDayOfMonth(tempDate);
                            }
                            else
                            {
                                tempDate = tempEDate;
                                dtStartDate = uHelper.FirstDayOfMonth(tempDate.AddDays(1));
                                dtEndDate = uHelper.LastDayOfMonth(dtStartDate);
                            }

                        }
                        else
                        {
                            dtStartDate = tempSDate.Date;
                            dtEndDate = tempEDate.Date;
                            dtStartDate = uHelper.FirstDayOfMonth(dtStartDate);
                            dtEndDate = uHelper.LastDayOfMonth(dtEndDate);
                        }

                        hdnkeepdaterange.Set("range", string.Format("{0};#{1}", dtStartDate.Date, dtEndDate.Date));
                    }
                }
                else
                {
                    if (Convert.ToString(ddlview.Value) == "monthly")
                    {
                        tempDate = DateTime.Now.Date;
                        tempSDate = new DateTime(tempDate.Year, 1, 1).Date;
                        tempEDate = new DateTime(tempDate.Year, 12, 31).Date;
                        dtStartDate = tempSDate.Date;
                        dtEndDate = tempEDate.Date;
                        hdnkeepdaterange.Set("range", dtStartDate.Year);
                    }
                    else if (Convert.ToString(ddlview.Value) == "weekly")
                    {
                        tempDate = DateTime.Now.Date;
                        DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

                        tempEDate = DateTime.Now.Date;
                        tempSDate = dt.AddDays(-(9 * 7));
                        dtStartDate = tempSDate.Date;
                        dtEndDate = tempEDate;
                        hdnkeepdaterange.Set("range", string.Format("{0};#{1}", dtStartDate.Date, dtEndDate.Date));
                    }
                    else
                    {
                        tempDate = DateTime.Now.Date;
                        dtStartDate = uHelper.FirstDayOfMonth(tempDate);
                        dtEndDate = uHelper.LastDayOfMonth(tempDate);
                        hdnkeepdaterange.Set("range", string.Format("{0};#{1}", dtStartDate.Date, dtEndDate.Date));
                    }
                }
            }
            else
            {
                tempDate = DateTime.Now.Date;
                dtStartDate = uHelper.FirstDayOfMonth(tempDate);
                dtEndDate = uHelper.LastDayOfMonth(tempDate);
                hdnkeepdaterange.Set("range", string.Format("{0};#{1}", dtStartDate.Date, dtEndDate.Date));
            }

            if (dtStartDate != DateTime.MinValue)
            {
                dtcStartdate.SelectedDate = dtStartDate;
                dtcStartdate.VisibleDate = dtStartDate;
            }

            nextWeek.Enabled = true;
            if (dtStartDate.Year == DateTime.Now.Year)
                nextWeek.Enabled = false;
            SetLabelDate();

        }

        private void CreateEachDayAsColumn()
        {
            finalResult.Columns.Clear();
            if (finalResult == null || finalResult.Columns.Count == 0)
            {
                lstOfColumnsToDraw.Clear();
                DataColumn dc = new DataColumn();
                dc.ColumnName = "Mode";
                dc.Caption = "";
                lstOfColumnsToDraw.Add(dc);
                DateTime tempDate = dtStartDate.Date;
                int counter = 1;
                string view = Convert.ToString(ddlview.Value);
                while (tempDate <= dtEndDate.Date)
                {
                    dc = new DataColumn();
                    dc.ColumnName = tempDate.Date.ToString();
                    dc.DefaultValue = "0";
                    if (view == "monthly")
                        dc.Caption = tempDate.Date.ToString("MMM");
                    else if (view == "weekly")
                        dc.Caption = tempDate.Date.ToString("MMM dd");
                    else
                        dc.Caption = tempDate.Date.ToString("ddd dd");

                    lstOfColumnsToDraw.Add(dc);
                    if (view == "monthly")
                        tempDate = tempDate.AddMonths(1);
                    else if (view == "weekly")
                    {
                        DateTime dt = tempDate.StartOfWeek(DayOfWeek.Monday);
                        // if (dt < tempDate.AddDays(6))
                        tempDate = tempDate.AddDays(7);
                    }
                    else
                        tempDate = tempDate.AddDays(1);

                    counter++;
                }

                //Total column
                dc = new DataColumn();
                dc.ColumnName = "Total";
                dc.Caption = "Total";
                dc.DefaultValue = "0";
                lstOfColumnsToDraw.Add(dc);
                finalResult.Columns.AddRange(lstOfColumnsToDraw.ToArray());
            }
            GenerateGridColumns();
        }

        private void GenerateGridColumns()
        {
            if (grdCountTrends.Columns.Count > 0)
                grdCountTrends.Columns.Clear();

            string view = Convert.ToString(ddlview.Value);
            GridViewDataTextColumn col = null;
            foreach (DataColumn dc in lstOfColumnsToDraw)
            {
                col = new GridViewDataTextColumn();
                col.FieldName = dc.ColumnName;
                col.Caption = dc.Caption;
                col.ExportCellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                col.ExportWidth = 50;
                int width = 60;
                col.Width = Unit.Pixel(width);
                if (col.FieldName == "Mode")
                {
                    col.CellStyle.Font.Bold = true;
                    col.Width = Unit.Pixel(120);
                    col.ExportWidth = 90;
                    if (view.Equals("daily"))
                        col.FixedStyle = GridViewColumnFixedStyle.Left;
                }
                else if (col.FieldName == "Total")
                {
                    col.CellStyle.Font.Bold = true;
                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                else
                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                grdCountTrends.Columns.Add(col);
            }
        }

        private void CreateTableSchema()
        {
            FillResultTable();
            if (lstOfColumnsToDraw.Count > 0 && rowTable != null && rowTable.Rows.Count > 0)
            {
                finalResult.Clear();
                var grp = rowTable.AsEnumerable().GroupBy(x => Convert.ToString(x["Mode"]));
                foreach (var varobj in grp)
                {
                    DataRow[] rowColl = varobj.ToArray();
                    DataRow dr = finalResult.NewRow();
                    dr["Mode"] = varobj.Key;
                    //fill row values
                    foreach (DataRow cRow in rowColl)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(cRow["Date"])))
                            dr["Total"] = Convert.ToString(cRow["Count"]);
                        else
                            dr[Convert.ToString(cRow["Date"])] = Convert.ToString(cRow["Count"]);

                    }

                    finalResult.Rows.Add(dr);
                }
            }
        }

        private void FillResultTable()
        {
            dtTicketCountTrends = ticketCountTrendsManager.GetDataTable();
            if (dtTicketCountTrends == null)
                return;

            string query = $"{DatabaseObjects.Columns.ModuleName}='{Convert.ToString(ddlModule.Value)}' And {DatabaseObjects.Columns.EndOfDay} >= '{dtStartDate.Date}' And {DatabaseObjects.Columns.EndOfDay} <= '{dtEndDate.Date}'";
            DataRow[] collTrendItems = dtTicketCountTrends.Select(query);

            try
            {
                query = $"{DatabaseObjects.Columns.ModuleName}='{Convert.ToString(ddlModule.Value)}'";
                DataRow[] totalItems = dtTicketCountTrends.Select(query);
                
                if (totalItems != null && totalItems.Length > 0)
                {
                    dateRangeEnd = totalItems.AsEnumerable().Where(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]).Date != DateTime.MinValue.Date).Max(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]));
                    dateRangeStart = totalItems.AsEnumerable().Where(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]).Date != DateTime.MinValue.Date).Min(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]));
                    previousWeek.Enabled = true;
                    nextWeek.Enabled = true;
                    previousWeek.Visible = true;
                    nextWeek.Visible = true;
                    
                    if (dateRangeStart.Date == DateTime.MinValue.Date || dateRangeStart.Date >= dtStartDate.Date)
                    {
                        previousWeek.Enabled = false;
                        previousWeek.Visible = false;
                    }

                    if (dateRangeEnd.Date == DateTime.MinValue.Date || dtEndDate.Date >= DateTime.Today.Date)
                    {
                        nextWeek.Enabled = false;
                        nextWeek.Visible = false;
                    }
                }

                if (collTrendItems != null)
                {
                    var ticketCountTrends = collTrendItems.AsEnumerable().Where(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]).Date >= dtStartDate && Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay]).Date <= dtEndDate.Date).GroupBy(x => Convert.ToDateTime(x[DatabaseObjects.Columns.EndOfDay])).OrderBy(x => x.Key);
                    DateTime tempdate = dtStartDate;
                    DataRow item = null;
                    string viewPostfix = "(EOM)";
                    
                    if (Convert.ToString(ddlview.Value) == "daily")
                        viewPostfix = "(EOD)";
                    else if (Convert.ToString(ddlview.Value) == "weekly")
                        viewPostfix = "(EOW)";

                    List<Tuple<string, string>> lstOfColumnsToGet = null;
                    if (resolveStageExists)
                    {
                        lstOfColumnsToGet = new List<Tuple<string, string>>() {
                                                     new Tuple<string, string>(DatabaseObjects.Columns.NumCreated, "Created"),
                                                     new Tuple<string, string>( DatabaseObjects.Columns.NumResolved, "Resolved"),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.NumClosed, "Closed"),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalActive, "Active "+viewPostfix),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalOnHold, "On Hold "+viewPostfix),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalResolved, "Resolved "+viewPostfix),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalClosed, "Closed "+viewPostfix) };
                    }
                    else
                    {
                        lstOfColumnsToGet = new List<Tuple<string, string>>() {
                                                     new Tuple<string, string>(DatabaseObjects.Columns.NumCreated, "Created"),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.NumClosed, "Closed"),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalActive, "Active "+viewPostfix),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalOnHold, "On Hold "+viewPostfix),
                                                     new Tuple<string, string>(DatabaseObjects.Columns.TotalClosed, "Closed "+viewPostfix) };
                    }

                    DataTable tempdt = new DataTable();
                    tempdt.Columns.Add("Mode");
                    tempdt.Columns.Add("Date");
                    DataColumn col = new DataColumn();
                    col.ColumnName = "Count";
                    col.Caption = "Count";
                    col.DefaultValue = 0;
                    tempdt.Columns.Add(col);

                    foreach (Tuple<string, string> tup in lstOfColumnsToGet)
                    {
                        tempdate = dtStartDate;
                        DataRow row = null;
                        int totalCount = 0;
                        
                        while (tempdate <= dtEndDate)
                        {
                            row = tempdt.NewRow();
                            row["Mode"] = tup.Item2;
                            row["Date"] = tempdate.Date;
                            DateTime endTempDate = DateTime.MinValue.Date;
                            if (viewPostfix == "(EOM)")
                                endTempDate = tempdate.AddMonths(1).AddDays(-1).Date;
                            else if (viewPostfix == "(EOW)")
                                endTempDate = tempdate.AddDays(7);
                            else
                                endTempDate = tempdate;

                            if (tup.Item1 == DatabaseObjects.Columns.NumCreated || tup.Item1 == DatabaseObjects.Columns.NumResolved || tup.Item1 == DatabaseObjects.Columns.NumClosed)
                            {
                                var gitem = ticketCountTrends.Where(x => x.Key >= tempdate && x.Key <= endTempDate);
                                if (gitem != null)
                                {
                                    int currentCount = 0;
                                    foreach (var innerItem in gitem)
                                    {
                                        item = innerItem.ToArray()[0];
                                        currentCount += Convert.ToInt32(item[tup.Item1]);
                                    }

                                    row["Count"] = currentCount;
                                    if (viewPostfix == "(EOM)")
                                        totalCount += currentCount;
                                    else if (viewPostfix == "(EOW)")
                                        totalCount += currentCount;
                                    else
                                        totalCount += currentCount;
                                }
                            }
                            else
                            {
                                var gitem = ticketCountTrends.OrderBy(x => x.Key).LastOrDefault(x => x.Key >= tempdate && x.Key <= endTempDate);
                                if (gitem != null)
                                {
                                    int currentCount = 0;
                                    foreach (var innerItem in gitem)
                                    {
                                        item = innerItem;
                                        currentCount += Convert.ToInt32(item[tup.Item1]);
                                    }

                                    row["Count"] = currentCount;
                                    totalCount = currentCount;

                                }
                            }

                            tempdt.Rows.Add(row);
                            if (viewPostfix == "(EOM)")
                                tempdate = tempdate.AddMonths(1);
                            else if (viewPostfix == "(EOW)")
                                tempdate = tempdate.AddDays(7);
                            else
                                tempdate = tempdate.AddDays(1);
                        }

                        row = tempdt.NewRow();
                        row["Mode"] = tup.Item2;

                        if (totalCount > 0)
                            row["Count"] = totalCount;
                        tempdt.Rows.Add(row);
                    }

                    rowTable = tempdt;
                    tempdt = null;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteLog("Error throw from FillResultTable in DailyTicketCountTrendsControl" + ex.Message);
            }
        }

        /// <summary>
        /// Bind KeepTicketCounts flag enbaled module only
        /// </summary>
        private void BindModule()
        {
            try
            {
                ddlModule.Items.Clear();
                string selectedModuleList = StringOfSelectedModule;
                if (!string.IsNullOrEmpty(selectedModuleList))
                {
                    string[] selectedModules = selectedModuleList.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string moduleTitle in selectedModules)
                    {
                        string[] arrKeyVal = moduleTitle.Split('(');
                        string moduleName = (arrKeyVal.Length > 0) ? arrKeyVal[1].Replace(')', ' ').Trim() : moduleTitle;
                        ListEditItem editItem = new ListEditItem(moduleTitle, moduleName);
                        ddlModule.Items.Add(editItem);
                    }
                }

                if (ddlModule.Items.Count == 0)
                    GetAllModules();

                if (ddlModule.Items.Count >= 0)
                {
                    if (!string.IsNullOrEmpty(DefaultModule))
                        ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByText(DefaultModule));
                }
                else
                    ddlModule.Items.Insert(0, new ListEditItem("--Select--", ""));

                if (ddlModule.SelectedIndex == -1)
                    ddlModule.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ULog.WriteLog("Bind modules: " + ex.Message);
            }
        }

        private void GetAllModules()
        {
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            List<UGITModule> modules = moduleManager.Load(x => x.EnableModule == true && x.KeepTicketCounts == true && x.ModuleName != ModuleNames.RMM);
            
            if (modules != null && modules.Count > 0)
            {
                ddlModule.DataSource = modules;
                ddlModule.TextField = DatabaseObjects.Columns.Title;
                ddlModule.ValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataBind();
            }
        }

        private void SetLabelDate()
        {
            if (Convert.ToString(ddlview.Value) == "monthly")
                lbWeekDuration.Text = string.Format("{0}", dtStartDate.Year);
            else if (Convert.ToString(ddlview.Value) == "weekly")
            {
                lbWeekDuration.Text = string.Format("{0} - {1}", dtStartDate.Date.ToString("MMM dd,yyyy"), dtEndDate.Date.ToString("MMM dd,yyyy"));
            }
            else
                lbWeekDuration.Text = string.Format("{0}", dtStartDate.Date.ToString("MMM, yyyy"));
        }

        protected void grdCountTrends_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "Mode")
            {
                // Highlight Saturday, Sunday
                if (e.DataColumn.Caption.StartsWith("Sat") || e.DataColumn.Caption.StartsWith("Sun"))
                    e.Cell.BackColor = System.Drawing.Color.FromArgb(243, 244, 245);

                DateTime dateCol = DateTime.MinValue;
                bool istotaldrilldown = false;
                if (e.DataColumn.FieldName == "Total" && dtEndDate != DateTime.MinValue)
                {
                    DateTime.TryParse(Convert.ToString(dtEndDate), out dateCol);
                    istotaldrilldown = true;
                }
                else
                    DateTime.TryParse(e.DataColumn.FieldName, out dateCol);

                string mode = Convert.ToString(e.GetValue("Mode"));
                if (dateCol.Date != DateTime.MinValue.Date && Convert.ToInt32(e.CellValue) > 0 && e.DataColumn.FieldName != "Total")
                {
                    // Set Drilldown date ranges for each view
                    DateTime dtdateRangeStart;
                    DateTime dtdateRangeEnd;
                    if (Convert.ToString(ddlview.Value) == "monthly")
                    {
                        dtdateRangeStart = Convert.ToDateTime(e.DataColumn.FieldName);
                        dtdateRangeEnd = dtdateRangeStart.AddMonths(1).AddMinutes(-1);
                    }
                    else if (Convert.ToString(ddlview.Value) == "weekly")
                    {
                        dtdateRangeStart = Convert.ToDateTime(e.DataColumn.FieldName);
                        dtdateRangeEnd = dtdateRangeStart.AddDays(7).AddMinutes(-1);
                    }
                    else
                    {
                        dtdateRangeStart = Convert.ToDateTime(e.DataColumn.FieldName);
                        dtdateRangeEnd = dtdateRangeStart.AddDays(1).AddMinutes(-1);
                    }

                    string strFun = string.Format("DailyTicketCountDrillDown(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\");", Convert.ToString(ddlModule.Value), dateCol.Date, Convert.ToString(ddlview.Value), mode, istotaldrilldown, dtdateRangeStart, dtdateRangeEnd);

                    if (istotaldrilldown && mode.EndsWith(")"))
                        e.Cell.Text = "-";
                    else if (mode.EndsWith(")"))
                        e.Cell.Text = Convert.ToString(e.CellValue);
                    else if (Convert.ToInt32(e.CellValue) > 0)
                        e.Cell.Text = string.Format("<a style='cursor: pointer;text-decoration: underline;' onclick='{0}'>{1}</a>", strFun, e.CellValue);
                }
                else if (istotaldrilldown && mode.EndsWith(")"))
                {
                    e.Cell.Text = "-";
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            currentExportType = "pdf";
            IsResolveStageExist();
            WeekStartEndDate();
            grdCountTrends.DataBind();
            PdfExportOptions option = new PdfExportOptions
            {
                ShowPrintDialogOnOpen = true,
            };

            exporter.ReportHeader = string.Format("Module: {0}     View: {1}       Range: {2} - {3}", ddlModule.Text, ddlview.Text, dtStartDate.Date.ToString("MMM dd,yyyy"), dtEndDate.Date.ToString("MMM dd,yyyy"));
            exporter.Landscape = true;
            exporter.WritePdfToResponse("DailyTicketCount", option);
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Header)
            {
                var info = new PaddingInfo(4, 4, 4, 4);
                e.BrickStyle.Padding = info;
            }
            if (e.RowType == GridViewRowType.Data && e.Column != null && e.Column.Caption != string.Empty)
            {
                if (currentExportType == "excel")
                    e.BrickStyle.TextAlignment = TextAlignment.MiddleCenter;
                else if (currentExportType == "pdf")
                    e.BrickStyle.SetAlignment(DevExpress.Utils.HorzAlignment.Center, DevExpress.Utils.VertAlignment.Center);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            currentExportType = "excel";
            IsResolveStageExist();
            WeekStartEndDate();
            grdCountTrends.DataBind();

            exporter.WriteCsvToResponse("DailyTicketCount");
        }

        protected void IsResolveStageExist()
        {
            resolveStageExists = false;
            Ticket ticketRequest = new Ticket(context, uHelper.getModuleIdByModuleName(context, UGITUtility.ObjectToString(ddlModule.Value)));
            if (ticketRequest != null && ticketRequest.Module != null) // Check in case module was not set
            {
                LifeCycle lifeCycle = ticketRequest.Module.List_LifeCycles.FirstOrDefault();
                if (lifeCycle != null)
                {
                    LifeCycleStage resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved");
                    if (resolvedStage != null)
                        resolveStageExists = true;
                }
            }
        }

        private void ScrollGridToColumn(ASPxGridView gridView, GridViewColumn gridViewColumn)
        {
            int offset = 0;
            int modeColWidth = 0;

            foreach (GridViewColumn col in gridView.Columns)
            {
                if (((DevExpress.Web.GridViewDataColumn)col).FieldName == "Mode")
                    modeColWidth = (int)col.Width.Value;

                if (col.Visible && (col.VisibleIndex < gridViewColumn.VisibleIndex))
                    offset += (int)col.Width.Value;
            }

            if (offset > modeColWidth)
                offset -= modeColWidth;

            hf.Set(String.Format("{0}_ScrollOffset", gridView.ClientID), offset);
        }
    }
}
