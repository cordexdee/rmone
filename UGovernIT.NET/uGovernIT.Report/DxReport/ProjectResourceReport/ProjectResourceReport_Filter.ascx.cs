using System;
using System.Web.UI;
using System.Text;
using System.Linq;
using System.Data;
using System.Globalization;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Report.Helpers;
using System.Collections.Generic;

namespace uGovernIT.Report.DxReport
{
    public partial class ProjectResourceReport_Filter : UserControl
    {
        #region Member Variables
        public int PMMId { get; set; }
        private DateTime startDate;
        private DateTime endDate;
        private bool isStartDate;
        private bool isEndDate;
        private string reportType;
        private int startMonth;
        private int endMonth;
        private int startWeek;
        private int endWeek;
        private int startYear;
        private int endYear;

        private string projectTitle;
        public string projectPublicID { get; set; }
        protected bool printReport = false;
        public StringBuilder urlBuilder = new StringBuilder();
        private string reportURL = string.Empty;
        TicketManager ObjTicketManager = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleViewManager = null;
        UGITModule uGITModule = null;
        #endregion

        #region Event Handlers
        /// <summary>
        /// Page Load Event handler .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["alltickets"] != null)
            {
                List<string> lstOfpublicids = Request["alltickets"].ToString().Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                if (lstOfpublicids.Count > 0)
                    projectPublicID = lstOfpublicids.FirstOrDefault();
            }

            DataRow project = null;
            ObjTicketManager = new TicketManager(context);
            moduleViewManager = new ModuleViewManager(context);
            uGITModule = moduleViewManager.LoadByName(ModuleNames.PMM);
            project= ObjTicketManager.GetByTicketID(uGITModule, projectPublicID);
            projectPublicID = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.TicketId));
            projectTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
        }

        /// <summary>
        /// Pre Render Event Handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
            if (string.IsNullOrEmpty(Request.Url.Query))
            {
                urlBuilder.Append("?");
                urlBuilder.Append(reportURL);
            }
            else
            {
                urlBuilder.Append("&");
                urlBuilder.Append(reportURL);
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// Render Event handler
        /// It check the export type(pdf/excel/print) and call the corresponding method.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            // If request is for export the report in given form like pdf,excel etc.
            if (Request["ExportReport"] != null)
            {
                string exportType = Request["exportType"];
                isStartDate = Convert.ToBoolean(Request["isStartDate"]);
                if (isStartDate)
                {
                    startDate = Convert.ToDateTime(Request["StartDate"]);
                }
                isEndDate = Convert.ToBoolean(Request["isEndDate"]);
                if (isEndDate)
                {
                    endDate = Convert.ToDateTime(Request["EndDate"]);
                }
                 reportType = Request["ReportType"];
                if (exportType == "pdf" || exportType == "image")
                {

                    GeneratePdf(ref writer, exportType);
                }
                else if (exportType == "excel")
                {
                    GenerateExcel();
                }
                // In case of print is called from report
                else if (exportType == "print")
                {
                    printReport = false;
                }
            }

            base.Render(writer);
        }

        /// <summary>
        /// Event handler for click event of button run
        /// It binds the resources grid based on the criteria selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.Value!=null)
            {
                startDate = dtDateFrom.Date;
                isStartDate = true;
            }
            else
            {
                isStartDate = false;
            }
            reportURL = reportURL + "StartDate=" + startDate.ToString() + "&isStartDate="+isStartDate.ToString();
            if (dtDateTo.Value!=null)
            {
                isEndDate = true;
                endDate = dtDateTo.Date;
            }
            else
            {
                isEndDate = false;
            }
            reportURL = reportURL + "&EndDate=" + endDate.ToString() + "&isEndDate=" + isEndDate.ToString(); 

            if (!string.IsNullOrEmpty(ddlResourceReportType.SelectedItem.Value))
            {
                reportType = ddlResourceReportType.SelectedItem.Value.ToString();
                reportURL = reportURL + "&ReportType=" + ddlResourceReportType.SelectedItem.Value.ToString();
            }
            /// Get the resources data based on the selected dropdown value (monthly/weekly).
            DataTable dtResources = GetResourceReportData();
            gvResource.DataSource = dtResources;
            gvResource.DataBind();
            if (gvResource.Rows.Count > 1)
            {
                gvResource.Rows[gvResource.Rows.Count - 1].CssClass = "titleHeaderBackground tablerow";
             }
            else
            {
                HideControls();

            }
            PnlBudgetReportPopup.Visible = false;
            projectPlanPanel.Visible = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the resource on weekly basis
        /// </summary>
        /// <returns></returns>
        private DataTable GetProjectResourceWorksByWeek()
        {
            // gets data from ResourceUsageSummaryWeekWise list.
            DataTable weekSummary = DashboardCache.GetCachedDashboardData(context, DatabaseObjects.Tables.ResourceUsageSummaryWeekWise);
            DataTable work = new DataTable();
            if (weekSummary != null && weekSummary.Rows.Count > 0)
            {
                DataColumn workItemColumn = weekSummary.Columns[DatabaseObjects.Columns.WorkItem];
                int workItemColumnIndex = weekSummary.Columns.IndexOf(workItemColumn);

                DataColumn weekStartDateColumn = weekSummary.Columns[DatabaseObjects.Columns.WeekStartDate];
                int weekStartDateColumnIndex = weekSummary.Columns.IndexOf(weekStartDateColumn);

                ///Sets the start and end week based on the data retrieved from ResourceUsageSummaryWeekWise list.
                GetStartEndMonthWeek(weekSummary, workItemColumnIndex, weekStartDateColumnIndex);

                if (startYear > 0 && endYear > 0 && startWeek > 0 && endWeek > 0)
                {
                    //Sets the starting and ending monday date based on start and end week  and year.
                    DateTime startMonday = GetDateFromWeek(startYear, startWeek - 1);
                    DateTime endMonday = GetDateFromWeek(endYear, endWeek - 1);

                    //Gets all the data between starting and ending monday
                    DataRow[] selectedRows = weekSummary.AsEnumerable().Where(x => x.ItemArray[workItemColumnIndex] != null && x.ItemArray[weekStartDateColumnIndex] != null).Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItem) == projectPublicID
                        && x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate) >= startMonday
                        && x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate) <= endMonday).ToArray();

                    if (selectedRows.Length <= 0)
                    {
                        return null;
                    }

                    //Creates schema of datatabble for the week.
                    work = CreateSchemaWeek();

                    // Inserts data in work datatable,  resource name wise
                    var resoruceWorks = selectedRows.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key).ToArray();

                    foreach (var resoureData in resoruceWorks)
                    {
                        DataRow[] rows = resoureData.ToArray();
                        DataRow weekRow = work.NewRow();
                        work.Rows.Add(weekRow);
                        weekRow[DatabaseObjects.Columns.Resource] = resoureData.Key;
                        double totalSum = 0;
                        for (int i = startYear; i <= endYear; i++)
                        {
                            int firstWeek = 1;
                            int lastWeek = 53;
                            if (i == startYear)
                            {
                                firstWeek = startWeek - 1;
                            }
                            if (i == endYear)
                            {
                                lastWeek = endWeek - 1;
                            }
                            for (int j = firstWeek; j <= lastWeek; j++)
                            {
                                //Get the date of monday for the week number in a year for selecting particular column of datatable.
                                DateTime result = GetDateFromWeek(i, j);
                                string monthString = result.ToString("MMM dd");
                                monthString = monthString + " '" + (i.ToString().Substring(2, 2));

                                DataRow[] monthData = rows.Where(x => Convert.ToDateTime(x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate).Date) == result).ToArray();
                                if (monthData.Length > 0)
                                {
                                    //Finds the actual hours
                                    int actualHourIndex = weekSummary.Columns.IndexOf(weekSummary.Columns[DatabaseObjects.Columns.ActualHour]);
                                    DataRow[] rosss = monthData.Where(x => x.ItemArray[actualHourIndex] != null).ToArray();
                                    double monthWork = monthData.Where(x => x.ItemArray[actualHourIndex] != null && x.ItemArray[actualHourIndex].ToString() != string.Empty).Sum(x => x.Field<double>(DatabaseObjects.Columns.ActualHour));
                                    totalSum += monthWork;
                                    weekRow[monthString] = string.Format("{0}", monthWork);
                                }
                                else
                                {
                                    weekRow[monthString] = "0";
                                }
                            }
                            weekRow["Total"] = totalSum.ToString("N2", CultureInfo.InvariantCulture);
                        }
                    }
                    return SetDatatable(work);
                }
            }
            return work;
        }
        /// <summary>
        /// Gets the resource on monthly basis
        /// </summary>
        /// <returns></returns>
        private DataTable GetProjectResourceWorksByMonth()
        {
            // gets data from ResourceUsageSummaryMonthWise list.
            DataTable monthSummary = DashboardCache.GetCachedDashboardData(context,DatabaseObjects.Tables.ResourceUsageSummaryMonthWise);
            DataTable work = new DataTable();
            if (monthSummary == null || monthSummary.Rows.Count <= 0)
            {
                return null;
            }

            DataColumn workItemColumn = monthSummary.Columns[DatabaseObjects.Columns.WorkItem];
            int workItemColumnIndex = monthSummary.Columns.IndexOf(workItemColumn);

            DataColumn monthStartDateColumn = monthSummary.Columns[DatabaseObjects.Columns.MonthStartDate];
            int monthStartDateColumnIndex = monthSummary.Columns.IndexOf(monthStartDateColumn);

            ///Sets the start and end month based on the data retrieved from ResourceUsageSummaryMonthWise list.
            GetStartEndMonthWeek(monthSummary, workItemColumnIndex, monthStartDateColumnIndex);
            if (startYear > 0 && endYear > 0 && startMonth > 0 && endMonth > 0)
            {
                //Sets the first and last date based on start and end month and year.
                DateTime dtStartMonth = new DateTime(startYear, startMonth, 1);
                DateTime dtEndMonthFirst = new DateTime(endYear, endMonth, 1);

                //Gets all the data between first and last date
                DataRow[] selectedRows = monthSummary.AsEnumerable().Where(x => x.ItemArray[workItemColumnIndex] != null && x.ItemArray[monthStartDateColumnIndex] != null).Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItem) == projectPublicID && x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate) >= dtStartMonth
                    && x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate) <= dtEndMonthFirst).ToArray();

                if (selectedRows.Length <= 0)
                {
                    return null;
                }

                //Creates schema of datatabble for the week.
                work = CreateSchema();
                var resoruceWorks = selectedRows.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key).ToArray();

                // Inserts data in work datatable,  resource name wise
                foreach (var resoureData in resoruceWorks)
                {
                    DataRow[] rows = resoureData.ToArray();
                    DataRow monthRow = work.NewRow();
                    work.Rows.Add(monthRow);
                    monthRow[DatabaseObjects.Columns.Resource] = resoureData.Key;
                    double totalSum = 0;
                    for (int i = startYear; i <= endYear; i++)
                    {
                        int firstMonth = 1;
                        int lastMonth = 12;
                        if (i == startYear)
                        {
                            firstMonth = startMonth;
                        }
                        if (i == endYear)
                        {
                            lastMonth = endMonth;
                        }
                        for (int j = firstMonth; j <= lastMonth; j++)
                        {
                            DataRow[] monthData = rows.Where(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month == j && x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Year == i).ToArray();

                            //Get the month name for selecting particular column of datatable.
                            DateTime dtDate = new DateTime(i, j, 1);
                            string monthName = dtDate.ToString("MMM");
                            string headerText = monthName + " '" + (i.ToString().Substring(2, 2));

                            if (monthData.Length > 0)
                            {
                                //Finds the actual hours
                                int actualHourIndex = monthSummary.Columns.IndexOf(monthSummary.Columns[DatabaseObjects.Columns.ActualHour]);
                                DataRow[] rosss = monthData.Where(x => x.ItemArray[actualHourIndex] != null).ToArray();
                                double monthWork = monthData.Where(x => x.ItemArray[actualHourIndex] != null && x.ItemArray[actualHourIndex].ToString() != string.Empty).Sum(x => x.Field<double>(DatabaseObjects.Columns.ActualHour));
                                totalSum += monthWork;
                                monthRow[headerText] = string.Format("{0}", monthWork);
                            }
                            else
                            {
                                monthRow[headerText] = "0";
                            }
                        }

                    }
                    monthRow["Total"] = totalSum.ToString("N2", CultureInfo.InvariantCulture);
                }

                return SetDatatable(work);
            }
            return work;
        }

        /// <summary>
        /// Creates the schema for the month datatable
        /// </summary>
        /// <returns></returns>
        private DataTable CreateSchema()
        {
            DataTable table = new DataTable();
            table.Columns.Add(DatabaseObjects.Columns.Resource);
            for (int i = startYear; i <= endYear; i++)
            {
                int firstMonth = 1;
                int lastMonth = 12;
                if (i == startYear)
                {
                    firstMonth = startMonth;
                }
                if (i == endYear)
                {
                    lastMonth = endMonth;
                }
                for (int j = firstMonth; j <= lastMonth; j++)
                {
                    DateTime dtDate = new DateTime(i, j, 1);
                    string monthName = dtDate.ToString("MMM");
                    string headerText = monthName + " '" + (i.ToString().Substring(2, 2));
                    DataColumn column = new DataColumn();
                    column.ColumnName = headerText;
                    table.Columns.Add(column);
                }
            }

            DataColumn colTotal = new DataColumn();
            colTotal.ColumnName = "Total";
            table.Columns.Add(colTotal);

            return table;
        }

        /// <summary>
        /// Creates the schema for the Week datatable
        /// </summary>
        /// <returns></returns>
        private DataTable CreateSchemaWeek()
        {
            DataTable table = new DataTable();
            table.Columns.Add(DatabaseObjects.Columns.Resource);
            for (int i = startYear; i <= endYear; i++)
            {
                int firstWeek = 1;
                int lastWeek = 53;
                if (i == startYear)
                {
                    firstWeek = startWeek - 1;
                }
                if (i == endYear)
                {
                    lastWeek = endWeek - 1;
                }
                for (int j = firstWeek; j <= lastWeek; j++)
                {
                    string result = GetDateFromWeek(i, j).ToString("MMM dd");
                    result = result + " '" + (i.ToString().Substring(2, 2));
                    DataColumn column = new DataColumn();
                    column.ColumnName = result;
                    if (!table.Columns.Contains(result))
                    {
                        table.Columns.Add(column);
                    }
                }
            }

            DataColumn colTotal = new DataColumn();
            colTotal.ColumnName = "Total";
            table.Columns.Add(colTotal);

            return table;
        }

        /// <summary>
        /// Gets first Monday date of a particular week in a year.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekNum"></param>
        /// <returns></returns>
        private DateTime GetDateFromWeek(int year, int weekNum)
        {
            DateTime dtJanFirst = new DateTime(year, 1, 1);
            DayOfWeek day = DayOfWeek.Monday;
            int daysToFirstCorrectDay = (((int)day - (int)dtJanFirst.DayOfWeek) + 7) % 7;
            DateTime FirstDay = dtJanFirst.AddDays(7 * (weekNum - 1) + daysToFirstCorrectDay);
            return FirstDay.Date;
        }

        /// <summary>
        /// Sets the start Month, start Week, end Month, end Year based on the selected criteria
        /// </summary>
        /// <param name="PrmSelectedDate"></param>
        /// <param name="isWeek"></param>
        /// <param name="isDateFrom"></param>
        private void SetMonthAndWeek(DateTime PrmSelectedDate, bool isWeek, bool isDateFrom)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            if (isDateFrom)
            {
                startYear = PrmSelectedDate.Year;
                if (isWeek)
                {
                    startWeek = currentCulture.Calendar.GetWeekOfYear(PrmSelectedDate, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
                }
                else
                {
                    startMonth = PrmSelectedDate.Month;
                }
            }
            else
            {
                endYear = PrmSelectedDate.Year;
                if (isWeek)
                {
                    endWeek = currentCulture.Calendar.GetWeekOfYear(PrmSelectedDate, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
                }
                else
                {
                    endMonth = PrmSelectedDate.Month;
                }
            }
        }

        /// <summary>
        /// Gets the start Month, start Week, end Month, end Year based on the selection criteria and data fetched from list.
        /// </summary>
        /// <param name="PrmSummary"></param>
        /// <param name="PrmWorkItemColumnIndex"></param>
        /// <param name="PrmStartDateColumnIndex"></param>
        private void GetStartEndMonthWeek(DataTable PrmSummary, int PrmWorkItemColumnIndex, int PrmStartDateColumnIndex)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;

                var query = PrmSummary.AsEnumerable().Where(x => x.ItemArray[PrmWorkItemColumnIndex] != null && x.ItemArray[PrmStartDateColumnIndex] != null)
                                .Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItem) == projectPublicID && x.Field<double>(DatabaseObjects.Columns.ActualHour) > 0);
                if (query != null && query.Count() > 0)
                {
                    var item = new DateTime();

                    if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "monthly" && !isStartDate)
                    {
                        item = query.Min(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate));
                        if (item != null)
                        {
                            SetMonthAndWeek(item, false, true);
                        }
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "weekly" && !isStartDate)
                    {
                        item = query.Min(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));
                        if (item != null)
                        {
                            SetMonthAndWeek(item, true, true);
                        }
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "monthly" && isStartDate)
                    {
                         item = startDate;
                        SetMonthAndWeek(item, false, true);
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "weekly" && isStartDate)
                    {
                        item = startDate;
                        SetMonthAndWeek(item, true, true);
                    }

                    if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "monthly" && !isEndDate)
                    {
                        item = query.Max(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate));
                        if (item != null)
                        {
                            SetMonthAndWeek(item, false, false);
                        }
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "weekly" && !isEndDate)
                    {
                        item = query.Max(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));
                        if (item != null)
                        {
                            SetMonthAndWeek(item, true, false);
                        }
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "monthly" && isEndDate)
                    {
                        item = endDate;
                        SetMonthAndWeek(item, false, false);
                    }
                    else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "weekly" && isEndDate)
                    {
                        item=endDate;
                        SetMonthAndWeek(item, true, false);
                    }

                }
            }
            catch (Exception ex)
            {

                uGovernIT.Util.Log.ULog.WriteException(ex, "ERROR: GetStartEndMonthWeek: in setting start and end month/week.");
            }
        }

        /// <summary>
        /// Adds the total row in a datatable and removes zero columns.
        /// </summary>
        /// <param name="PrmDtWork"></param>
        /// <returns></returns>
        private DataTable SetDatatable(DataTable PrmDtWork)
        {
            //Adds the total row in a datatable
            DataRow totalColumn = PrmDtWork.NewRow();

            foreach (DataColumn col in PrmDtWork.Columns)
            {
                if (col.ColumnName.ToLower() == DatabaseObjects.Columns.Resource.ToLower())
                {
                    totalColumn[col.ColumnName] = "Total";
                }
                else
                {
                    double colTotal = 0;
                    foreach (DataRow row in PrmDtWork.Rows)
                    {
                        colTotal += Convert.ToDouble(row[col]);
                    }
                    totalColumn[col.ColumnName] = colTotal.ToString("N2", CultureInfo.InvariantCulture);
                }
            }
            PrmDtWork.Rows.Add(totalColumn);
            string dateRangeText = "<b>&nbsp;Report</b>";

            //Removes columns  that have zero data
            if(!isStartDate || !isEndDate)
            {
                foreach (var column in PrmDtWork.Columns.Cast<DataColumn>().ToArray())
                {
                    //Removes columns  that have zero data
                    if (column.ColumnName != DatabaseObjects.Columns.Resource && PrmDtWork.AsEnumerable().All(dr => dr.IsNull(column) || Convert.ToDouble(dr[column]) == 0))
                    {
                        PrmDtWork.Columns.Remove(column);
                    }
                }
            }
            
            //Condition to set Report from and report to text.
            string strReportNonZeroText = string.Empty;
            if(isStartDate)
            {
                dateRangeText += "<b> From: </b>" + PrmDtWork.Columns[1].ColumnName.ToString();
                lblReportDateRange.Visible = true;
            }
            if(isEndDate)
            {
                dateRangeText += "<b> To: </b>" + PrmDtWork.Columns[PrmDtWork.Columns.Count - 2].ColumnName.ToString();
                lblReportDateRange.Visible = true;
            }
            if (!isStartDate || !isEndDate)
            {
                strReportNonZeroText = " (report is only showing non-zero data)";
            }
            lblReportDateRange.Text = dateRangeText;
            lblRightHeading.Text = "<b>View: </b>" + reportType + " Resource Hours" + strReportNonZeroText;

            lblReportTitle.Text = "<b>" + projectPublicID + ": " + projectTitle + "</b>";
            return PrmDtWork;
        }

        /// <summary>
        /// Generates Excel
        /// </summary>
        private void GenerateExcel()
        {

            DataTable excelTable = GetResourceReportData();
            // Convert the data in csv format.
            string csvData = UGITUtility.ConvertTableToCSV(excelTable);
            string attachment = string.Format("attachment; filename={0}.csv", "Export");
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.Write(csvData.ToString());
            Response.Flush();
            Response.End();
        }

        /// <summary>
        /// Generates the pdf
        /// </summary>
        /// <param name="PrmWriter"></param>
        /// <param name="PrmExportType"></param>
        private void GeneratePdf(ref HtmlTextWriter PrmWriter, string PrmExportType)
        {
            string headerTitle = string.Empty;
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter(sb));
            exportPanel.Visible = false;
            projectPlanPanel.Visible = true;
            btnCloseResourcePopUp.Visible = false;
            //Render the page to the new HtmlTextWriter which actually writes to the stringbuilder
            // Filter && Copy the resource hour table in temporary table.

            stylegvResource.RenderControl(tw);
            DataTable dtResources = GetResourceReportData();
            gvResource.DataSource = dtResources;
            gvResource.DataBind();
            projectPlanPanel.RenderControl(tw);
            //Get the rendered content
            string sContent = sb.ToString();

            //Now output it to the page, if you want
            PrmWriter.Write(sContent);
            string html = sb.ToString();

            ExportReport convert = new ExportReport();
            convert.ScriptsEnabled = true;
            convert.ShowFooter = true;
            convert.ShowHeader = true;
            int reportType = 0;
            string reportTypeString = "pdf";
            string contentType = "Application/pdf";
            if (PrmExportType == "IMAGE")
            {
                reportType = 1;
                reportTypeString = "png";
                contentType = "image/png";
            }
            convert.ReportType = reportType;
            html = string.Format(@"<html><head></head><body>{0}</body></html>", html);
            byte[] bytes = convert.GetReportFromHTML(html,string.Empty);

            string fileName = string.Format("export.{0}", reportTypeString);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            exportPanel.Visible = true;
            btnCloseResourcePopUp.Visible = true;
        }

        /// <summary>
        /// Hides the control when the resource hour report has no data
        /// </summary>
        private void HideControls()
        {
            exportPanel.Visible = false;
            btnCloseResourcePopUp.Visible = false;
            lblReportDateRange.Text = "No data to display.";
            lblReportDateRange.Attributes.Add("style", "font-weight:bold;");
            lblReportDateRange.Visible = true;
        }

        /// <summary>
        /// Gets the Resource hour report data based on monthly/weekly basis
        /// </summary>
        /// <returns></returns>
        private DataTable GetResourceReportData()
        {
            DataTable projectWork = new DataTable();
            if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "monthly")
            {
                projectWork = GetProjectResourceWorksByMonth();
            }
            else if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "weekly")
            {
                projectWork = GetProjectResourceWorksByWeek();
            }

            return projectWork;
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnCloseResourcePopUp_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
