using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ResourceSkillReportControl : System.Web.UI.UserControl
    {
        #region Member Variables
        public DateTime dateFrom;
        public DateTime dateTo;
        public string startWith;
        public string viewType;
        public string DrillDownTo;
        public string unitAllocAct;

        protected int year = System.DateTime.Now.Year;
        private DataTable resourceSummaryTable = null;
        private List<string> columnList = new List<string>();
        private string dateFieldColumnName = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();
        protected bool printReport = false;
        protected string viewUrl= UGITUtility.GetAbsoluteURL("/ControlTemplates/RMM/userinfo.aspx?UpdateUser=1&uID=");
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        UserProfileManager userProfileManager = null;
        public string reportType { get; set; }
        
        List<UserSkills> userSkillList;
        List<UserProfile> userProfilelist;
        UserSkillManager userSkillManager = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            userProfileManager = new UserProfileManager(AppContext);
            userSkillManager = new UserSkillManager(AppContext);
            lblReportDateRange.Text = "<b>Report From: </b>" + UGITUtility.GetDateStringInFormat(dateFrom, false) + "<b> To </b>" + UGITUtility.GetDateStringInFormat(dateTo, false);
            lblRightHeading.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;View: </b>" + viewType;

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            loadPage();
        }

        private void loadPage()
        {
            //Get the view type from request.
            if (!string.IsNullOrEmpty(Request["ViewType"]))
                viewType = Request["ViewType"];

            if (string.IsNullOrEmpty(Request["DateTo"]) ||
                !DateTime.TryParse(Convert.ToString(Request["DateTo"]), out dateTo))
            {
                if (viewType == "Weekly")
                {
                    dateTo = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek); // Sunday of last week
                }
                else
                {
                    dateTo = DateTime.Now.Date;
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, 1);
                    dateTo = dateTo.AddDays(-1);
                }
            }

            if (string.IsNullOrEmpty(Request["DateFrom"]) ||
                !DateTime.TryParse(Convert.ToString(Request["DateFrom"]), out dateFrom))
            {
                if (viewType == "Weekly")
                    dateFrom = dateTo.AddDays(-41); // Go back 6 weeks
                else
                    dateFrom = dateTo.AddMonths(-5); // Go back 6 months
            }

            if (!string.IsNullOrEmpty(Request["StartWith"]))
            {
                startWith = Convert.ToString(Request["StartWith"]);
            }
            //Get unit 
            if (!string.IsNullOrEmpty(Request["unitAllocAct"]))
                unitAllocAct = Convert.ToString(Request["unitAllocAct"]).ToLower();

            //Get the view type from request.
            if (!string.IsNullOrEmpty(Request["isCallBack"]))
                btnClose.Visible = false;

            gvResourceReport.Settings.ShowFilterRowMenu = true;
            gvResourceReport.Settings.ShowHeaderFilterButton = true;
            gvResourceReport.Settings.ShowFooter = true;
            gvResourceReport.SettingsPopup.HeaderFilter.Height = 200;
            gvResourceReport.SettingsPager.PageSize = 15;
            gvResourceReport.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            gvResourceReport.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gvResourceReport.Styles.AlternatingRow.BackColor = Color.FromArgb(245, 245, 245);
            gvResourceReport.Styles.Row.BackColor = Color.White;
            gvResourceReport.Styles.GroupRow.Font.Bold = true;
            gvResourceReport.Settings.GridLines = GridLines.None;
        }
        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(((XRShape)sender).Report.GetCurrentColumnValue("Discontinued")) == true)
                ((XRShape)sender).FillColor = Color.Yellow;
            else
                ((XRShape)sender).FillColor = Color.White;
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //TODO DevExPrinting.PdfExportOptions option = new DevExPrinting.PdfExportOptions();
            //option.ShowPrintDialogOnOpen = true;

            //gridExporter.WritePdfToResponse(lblReportTitle.Text, option);
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.GroupHeadingSufix = "Total: ";
            reportHelper.GrandGroupHeading = "Grand Total: ";
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            DataTable ResultData = GetDataSource();
            XtraReport report = reportHelper.GenerateReport(gvResourceReport, ResultData, lblReportTitle.Text, 6.75F);
            reportHelper.WritePdfToResponse(Response, lblReportTitle.Text + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }
        void reportHelper_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {

        }

        void reportHelper_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Title"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Title").ColumnWidth *= 2;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Rank"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Rank").ColumnWidth = 50;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Priority"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Priority").ColumnWidth = 50;
        }
        protected void btnExcelExport_Click(object sender, EventArgs e)
        {

            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.GroupHeadingSufix = "Total: ";
            reportHelper.GrandGroupHeading = "Grand Total: ";
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            DataTable ResultData = GetDataSource();
            XtraReport report = reportHelper.GenerateReport(gvResourceReport, ResultData, lblReportTitle.Text, 8F, "xls");
            reportHelper.WriteXlsToResponse(Response, lblReportTitle.Text + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }
        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
            gvResourceReport.DataBind();
            base.OnPreRender(e);
        }
        public DataTable GetDataSource()
        {
            DataTable reportTable = new DataTable();

            //SPWeb oWeb = SPContext.Current.Web;
            //DataTable ResourceUsageSummary = null;
            Dictionary<string, object> values = new Dictionary<string, object>();

            string yearStartDateStr = UGITUtility.ObjectToString(dateFrom);
            string yearEndDateStr = UGITUtility.ObjectToString(dateTo);

            // Set the data of the year.
            //if (viewType == "Weekly")
            //{
            //    dateFieldColumnName = DatabaseObjects.Columns.WeekStartDate;
            //    ResourceUsageSummary = oWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryWeekWise];
            //}
            //else
            //{
            //    dateFieldColumnName = DatabaseObjects.Columns.MonthStartDate;
            //    ResourceUsageSummary = oWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryMonthWise];

            //    //Set the dateFrom as start date of month and dateTo as last day of the month. because the report is dependent on the month.
            //    dateTo = new DateTime(dateTo.Year, dateTo.Month, DateTime.DaysInMonth(dateTo.Year, dateTo.Month));
            //    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, 1);
            //}

            values.Add("@TenantID", AppContext.TenantID);
            values.Add("@ViewType", viewType);
            values.Add("@unitAllocAct", unitAllocAct);

            if (viewType == "Weekly")
            {
                dateFieldColumnName = DatabaseObjects.Columns.WeekStartDate;
                values.Add("@DateFrom", yearStartDateStr);
                values.Add("@DateTo", yearEndDateStr);
            }
            else
            {
                //Set the dateFrom as start date of month and dateTo as last day of the month. because the report is dependent on the month.
                dateTo = new DateTime(dateTo.Year, dateTo.Month, DateTime.DaysInMonth(dateTo.Year, dateTo.Month));
                dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, 1);
                values.Add("@DateFrom", dateFrom);
                values.Add("@DateTo", dateTo);
                dateFieldColumnName = DatabaseObjects.Columns.MonthStartDate;
            }
            resourceSummaryTable = GetTableDataManager.GetData("ResourceSummaryReportData", values);

            ////Get all the between selected date.
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><And><Geq><FieldRef Name='{0}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{1}</Value></Geq><Leq><FieldRef Name='{0}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value></Leq></And></Where>", dateFieldColumnName, yearStartDateStr, yearEndDateStr);
            //SPListItemCollection coll = ResourceUsageSummary.GetItems(query);

            //Filter the data for selected category.
            if (resourceSummaryTable != null && resourceSummaryTable.Rows.Count > 0)
            {

                DataTable teampResourceTable = resourceSummaryTable.Clone();
                resourceSummaryTable.DefaultView.Sort = dateFieldColumnName;
                resourceSummaryTable = resourceSummaryTable.DefaultView.ToTable();
                var StartDays = resourceSummaryTable.AsEnumerable().ToLookup(x => x.Field<DateTime>(dateFieldColumnName));

                if (columnList.Count > 0)
                    columnList.Clear();

                if (viewType != "Weekly")
                {
                    DateTime fromDate = dateFrom;
                    DateTime toDate = dateTo;

                    while (fromDate < toDate)
                    {
                        columnList.Add(fromDate.ToString("MMM") + "-" + fromDate.ToString("yy"));
                        fromDate = fromDate.AddMonths(1);
                    }
                }
                else
                {
                    foreach (var startDay in StartDays)
                    {
                        columnList.Add(UGITUtility.GetDateStringInFormat(Convert.ToDateTime(startDay.Key), false));
                    }
                }
                //Load skills and user profiles in Global variables for later use thorughout the page.
                userSkillList = userSkillManager.Load();
                userProfilelist = userProfileManager.GetUsersProfile();

                if (viewType == "Weekly")
                {
                    if (DrillDownTo != "People")
                        reportTable = CreateWeeklyFormatTableForSkill(resourceSummaryTable);
                    else
                        reportTable = CreateWeeklyFormatTableForSkillWithPeopleDrillDown(resourceSummaryTable);
                }
                else
                {
                    if (DrillDownTo != "People")
                        reportTable = CreateMonthlyFormatTableForSkill(resourceSummaryTable);
                    else
                        reportTable = CreateMonthlyFormatTableForSkillWithPeopleDrillDown(resourceSummaryTable);
                }
                
                GenerateColumns();
            }

            if ((reportTable == null || reportTable.Rows.Count == 0) && resourceSummaryTable != null)
                resourceSummaryTable = resourceSummaryTable.Clone();

            return reportTable;
        }
        private void GenerateColumns()
        {
            if (gvResourceReport.Columns.Count <= 0)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                colId.PropertiesTextEdit.EncodeHtml = true;
                colId.FieldName = DatabaseObjects.Columns.CategoryName;
                colId.Caption = "Category";
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.GroupIndex = 0;
                gvResourceReport.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                colId.PropertiesTextEdit.EncodeHtml = true;
                colId.FieldName = "Skill";
                colId.Caption = "Skill";
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                if (DrillDownTo == "People")
                    colId.GroupIndex = 1;
                colId.FooterTemplate = new CustomFooterRowTemplate("GRAND TOTAL:");
                gvResourceReport.Columns.Add(colId);

                if (DrillDownTo == "People")
                {
                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = DatabaseObjects.Columns.ResourceName;
                    colId.Caption = "Person";
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.FooterTemplate = new CustomFooterRowTemplate("GRAND TOTAL:");
                    gvResourceReport.Columns.Add(colId);


                    //new for people...

                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = DatabaseObjects.Columns.ResourceId;
                    colId.Caption = "ResourceId";
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.Visible = false;
                    gvResourceReport.Columns.Add(colId);
                }

                // Week/Month data columns
                if (columnList != null && columnList.Count > 0)
                {
                    gvResourceReport.GroupSummary.Clear();
                    gvResourceReport.TotalSummary.Clear();

                    foreach (string column in columnList)
                    {
                        colId = new GridViewDataTextColumn();
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.GroupFooterCellStyle.Font.Bold = true;
                        colId.FooterCellStyle.Font.Bold = true;
                        colId.PropertiesTextEdit.EncodeHtml = true;
                        colId.FieldName = column;
                        colId.Caption = column;
                        colId.PropertiesTextEdit.DisplayFormatString = "{0:F2}";
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.Width = new Unit(100);
                        gvResourceReport.Columns.Add(colId);

                        ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
                        summary.ShowInGroupFooterColumn = column;
                        summary.DisplayFormat = "{0:F2}";
                        gvResourceReport.GroupSummary.Add(summary);

                        ASPxSummaryItem totalSummary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
                        totalSummary.DisplayFormat = "{0:F2}";
                        gvResourceReport.TotalSummary.Add(totalSummary);
                    }

                    gvResourceReport.Settings.GroupFormat = "{1}";
                }
            }
        }
        private DataTable CreateMonthlyFormatTableForSkill(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn CategoryName = new DataColumn(DatabaseObjects.Columns.CategoryName, typeof(string));
            formatTable.Columns.Add(CategoryName);

            DataColumn Skill = new DataColumn("Skill", typeof(string));
            formatTable.Columns.Add(Skill);

            Dictionary<int, double> workHoursList = new Dictionary<int, double>();

            if (columnList != null && columnList.Count > 0)
            {
                foreach (string column in columnList)
                {
                    DataColumn colName = new DataColumn(column, typeof(double));
                    formatTable.Columns.Add(colName);
                }
            }

            //SPList userSkillList = oWeb.Lists[DatabaseObjects.Lists.UserSkills];
            foreach (UserSkills userSkillItem in userSkillList)
            {
                List<UserProfile> filterUserProfileList = userProfilelist.Where(x => !string.IsNullOrEmpty(x.Skills) && x.Skills.Contains(userSkillItem.ID.ToString())).ToList();
                List<string> userList = filterUserProfileList.Select(x => x.Name).ToList();
                string strExp1 = string.Join(",", userList.Select(x => string.Format("'{0}'", x.Replace("'", "''"))).ToArray());

                DataTable dttemptable = sourceTable;
                if (!string.IsNullOrWhiteSpace(strExp1))
                {
                    string strExp2 = string.Format("{0} IN({1})", DatabaseObjects.Columns.ResourceName, strExp1);
                    DataRow[] dr = dttemptable.Select(strExp2);
                    if (dr.Length > 0)
                    {
                        var allocationByMonth = dr.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month);

                        DataRow newRow = null;

                        foreach (var monthAlloc in allocationByMonth)
                        {
                            DataRow[] monthAllocRows = monthAlloc.ToArray();
                            double totalAllocationHour = 0.0;
                            totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;

                            if (newRow == null)
                            {
                                newRow = formatTable.NewRow();
                                newRow[DatabaseObjects.Columns.CategoryName] = userSkillItem.CategoryName;
                                newRow["Skill"] = userSkillItem.Title;
                            }
                            newRow[Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy")] = totalAllocationHour;
                        }
                        if (newRow != null)
                            formatTable.Rows.Add(newRow);
                    }
                }
            }

            return formatTable;
        }

        private DataTable CreateWeeklyFormatTableForSkill(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn CategoryName = new DataColumn(DatabaseObjects.Columns.CategoryName, typeof(string));
            formatTable.Columns.Add(CategoryName);

            DataColumn Skill = new DataColumn("Skill", typeof(string));
            formatTable.Columns.Add(Skill);

            Dictionary<DateTime, double> workHoursList = new Dictionary<DateTime, double>();

            if (columnList != null && columnList.Count > 0)
            {
                foreach (string column in columnList)
                {
                    DataColumn colName = new DataColumn(column, typeof(double));
                    formatTable.Columns.Add(colName);
                }
            }

            //SPList userSkillList = oWeb.Lists[DatabaseObjects.Lists.UserSkills];
            foreach (UserSkills userSkillItem in userSkillList)
            {
                //List<UserProfile> filterUserProfileList = userProfilelist.Where(x => x.Skills.Contains(userSkillItem.ID.ToString())).ToList();
                List<UserProfile> filterUserProfileList = userProfilelist.Where(x=> !string.IsNullOrEmpty(x.Skills) && x.Skills.Contains(userSkillItem.ID.ToString())).ToList();
                List<string> userList = filterUserProfileList.Select(x => x.Name).ToList();
                string strExp1 = string.Join(",", userList.Select(x => string.Format("'{0}'", x.Replace("'", "''"))).ToArray());

                DataTable dttemptable = sourceTable;
                if (!string.IsNullOrWhiteSpace(strExp1))
                {
                    string strExp2 = string.Format("{0} IN({1})", DatabaseObjects.Columns.ResourceName, strExp1);
                    DataRow[] dr = dttemptable.Select(strExp2);
                    if (dr.Length > 0)
                    {
                        var allocationByWeek = dr.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));

                        DataRow newRow = null;

                        foreach (var monthAlloc in allocationByWeek)
                        {
                            DataRow[] monthAllocRows = monthAlloc.ToArray();
                            double totalAllocationHour = 0.0;
                            totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;

                            if (newRow == null)
                            {
                                newRow = formatTable.NewRow();
                                newRow[DatabaseObjects.Columns.CategoryName] = userSkillItem.CategoryName;
                                newRow["Skill"] = userSkillItem.Title;
                            }
                            newRow[UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false)] = totalAllocationHour;
                        }

                        if (newRow != null)
                            formatTable.Rows.Add(newRow);
                    }
                }
            }
            return formatTable;
        }

        private DataTable CreateWeeklyFormatTableForSkillWithPeopleDrillDown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn CategoryName = new DataColumn(DatabaseObjects.Columns.CategoryName, typeof(string));
            formatTable.Columns.Add(CategoryName);

            DataColumn Skill = new DataColumn("Skill", typeof(string));
            formatTable.Columns.Add(Skill);

            DataColumn Resource = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(Resource);

            DataColumn ResourceId = new DataColumn(DatabaseObjects.Columns.ResourceId, typeof(string));
            formatTable.Columns.Add(ResourceId);

            Dictionary<DateTime, double> workHoursList = new Dictionary<DateTime, double>();

            if (columnList != null && columnList.Count > 0)
            {
                foreach (string column in columnList)
                {
                    DataColumn colName = new DataColumn(column, typeof(double));
                    formatTable.Columns.Add(colName);
                }
            }


            //List<UserProfile> userProfilelist = uGITCache.UserProfileCache.GetAllUsers(oWeb);
            foreach (UserSkills userSkillItem in userSkillList)
            {
                List<UserProfile> filterUserProfileList = userProfilelist.Where(x => !string.IsNullOrEmpty(x.Skills) && x.Skills.Contains(userSkillItem.ID.ToString())).ToList();

                foreach (UserProfile userProfileItem in filterUserProfileList)
                {
                    DataTable dttemptable = sourceTable;
                    string strExp = string.Format("{0}='{1}'",DatabaseObjects.Columns.ResourceName, userProfileItem.Name.Replace("'", "''"));
                    DataRow[] dr = dttemptable.Select(strExp);
                    if (dr.Length > 0)
                    {
                        dttemptable = dr.CopyToDataTable();

                        var allocationByWorkTypes = dttemptable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);

                        foreach (var workType in allocationByWorkTypes)
                        {
                            DataRow[] workTypeRows = workType.ToArray();
                            var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);

                            foreach (var workItem in allocationByWorkItem)
                            {
                                DataRow[] workItemRows = workItem.ToArray();
                                var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));

                                DataRow newRow = null;

                                foreach (var monthAlloc in allocationByMonth)
                                {
                                    DataRow[] monthAllocRows = monthAlloc.ToArray();
                                    double totalAllocationHour = 0.0;
                                    totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;

                                    if (newRow == null)
                                    {
                                        newRow = formatTable.NewRow();
                                        newRow[DatabaseObjects.Columns.CategoryName] = userSkillItem.CategoryName;
                                        newRow["Skill"] = userSkillItem.Title;
                                        newRow[DatabaseObjects.Columns.ResourceName] = userProfileItem.Name;
                                        newRow[DatabaseObjects.Columns.ResourceId] = userProfileItem.Id;
                                    }
                                    newRow[UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false)] = totalAllocationHour;
                                }

                                if (newRow != null)
                                    formatTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }

            return formatTable;
        }
        private DataTable CreateMonthlyFormatTableForSkillWithPeopleDrillDown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn CategoryName = new DataColumn(DatabaseObjects.Columns.CategoryName, typeof(string));
            formatTable.Columns.Add(CategoryName);

            DataColumn Skill = new DataColumn("Skill", typeof(string));
            formatTable.Columns.Add(Skill);

            DataColumn Resource = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(Resource);

            DataColumn ResourceId = new DataColumn(DatabaseObjects.Columns.ResourceId, typeof(string));
            formatTable.Columns.Add(ResourceId);

            Dictionary<int, double> workHoursList = new Dictionary<int, double>();

            if (columnList != null && columnList.Count > 0)
            {
                foreach (string column in columnList)
                {
                    DataColumn colName = new DataColumn(column, typeof(double));
                    formatTable.Columns.Add(colName);
                }
            }

            foreach (UserSkills userSkillItem in userSkillList)
            {
                List<UserProfile> filterUserProfileList = userProfilelist.Where(x => !string.IsNullOrEmpty(x.Skills) && x.Skills.Contains(userSkillItem.ID.ToString())).ToList();

                foreach (UserProfile userProfileItem in filterUserProfileList)
                {
                    DataTable dttemptable = sourceTable;
                    string strExp = string.Format("{0}='{1}'",DatabaseObjects.Columns.ResourceName, userProfileItem.Name.Replace("'", "''"));
                    DataRow[] dr = dttemptable.Select(strExp);
                    if (dr.Length > 0)
                    {
                        dttemptable = dr.CopyToDataTable();

                        var allocationByWorkTypes = dttemptable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);

                        foreach (var workType in allocationByWorkTypes)
                        {
                            DataRow[] workTypeRows = workType.ToArray();
                            var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);

                            foreach (var workItem in allocationByWorkItem)
                            {
                                DataRow[] workItemRows = workItem.ToArray();
                                var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month);

                                DataRow newRow = null;

                                foreach (var monthAlloc in allocationByMonth)
                                {
                                    DataRow[] monthAllocRows = monthAlloc.ToArray();
                                    double totalAllocationHour = 0.0;
                                    totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;

                                    if (newRow == null)
                                    {
                                        newRow = formatTable.NewRow();
                                        newRow[DatabaseObjects.Columns.CategoryName] = userSkillItem.CategoryName;
                                        newRow["Skill"] = userSkillItem.Title;
                                        newRow[DatabaseObjects.Columns.ResourceName] = userProfileItem.Name;
                                        newRow[DatabaseObjects.Columns.ResourceId] = userProfileItem.Id;
                                    }
                                    newRow[Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy")] = totalAllocationHour;
                                }
                                if (newRow != null)
                                    formatTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }

            return formatTable;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // If request is for export the report in given form like pdf,excel etc.
            if (Request["ExportReport"] != null)
            {
                string exportType = Request["exportType"];

                if (exportType == "excel")
                {
                    // Filter && Copy the budget table in temporary table.
                    DataTable excelTable = GetDataSource();

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
                // In case of print is called from report
                else if (exportType == "print")
                {
                    btnClose.Visible = false;
                    exportPanel.Visible = false;
                    printReport = true;
                }
            }

            base.Render(writer);
        }

        protected void gvResourceReport_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            ColorGroupRow(e, Color.FromArgb(227, 226, 228), 0);
            ColorGroupRow(e, Color.FromArgb(234, 234, 235), 1);
            if (e.RowType == GridViewRowType.Footer)
            {
                if (e.Row.Cells.Count > 1)
                    e.Row.Cells[1].Text = "<b>GRAND TOTAL: </b>";
            }

            if (e.RowType == GridViewRowType.Data)
            {
                DataRow currentRow = gvResourceReport.GetDataRow(e.VisibleIndex);
                string func = string.Empty;

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ResourceName) && Convert.ToString(currentRow[DatabaseObjects.Columns.ResourceName]) != string.Empty)
                {
                    func = string.Format("openResourceProfileInfo('{0}{1}','','{2}','{3}','{4}', 0)", viewUrl, currentRow[DatabaseObjects.Columns.ResourceId], "User Detail:" + currentRow[DatabaseObjects.Columns.ResourceName], 60, 75);
                    e.Row.Attributes.Add("onClick", func);
                    e.Row.Attributes.Add("cursor", "pointer");
                }
            }
        }

        private void ColorGroupRow(ASPxGridViewTableRowEventArgs e, Color backColor, int rowIndex)
        {
            if ((e.RowType == GridViewRowType.Group || e.RowType == GridViewRowType.GroupFooter) && gvResourceReport.GetRowLevel(e.VisibleIndex) == rowIndex)
            {
                e.Row.BackColor = backColor;
                if (e.RowType == GridViewRowType.GroupFooter)
                {
                    e.Row.Cells[gvResourceReport.GroupCount].Text = string.Format("<b>{0} Total: </b>", gvResourceReport.GetGroupedColumns().FirstOrDefault(m => m.GroupIndex == rowIndex).Caption);
                }
            }
        }

        protected void gvResourceReport_HeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewHeaderFilterEventArgs e)
        {

        }
        protected void gvResourceReport_DataBinding(object sender, EventArgs e)
        {
            gvResourceReport.DataSource = GetDataSource();
        }

        protected void gvResourceReport_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (viewType == "Weekly")
            {
                DateTime weekDate = Convert.ToDateTime(((DevExpress.Web.ASPxSummaryItem)(e.Item)).FieldName);
                string strExp = string.Format("{0}='{1}'",DatabaseObjects.Columns.WeekStartDate, weekDate);
                DataRow[] dr = resourceSummaryTable.Select(strExp);
                double totalAllocationHour = 0.0;
                if (dr.Length > 0)
                {
                    //var allocationByWeek = resourceSummaryTable.AsEnumerable().ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));
                    var allocationByWeek = dr.AsEnumerable().ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));
                    foreach (var monthAlloc in allocationByWeek)
                    {
                        DataRow[] monthAllocRows = monthAlloc.ToArray();
                        totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;

                    }
                }
                e.TotalValue = (Math.Round(totalAllocationHour, 2)).ToString();
                ((DevExpress.Web.ASPxSummaryItem)(e.Item)).Tag = "GRAND TOTAL:" + (Math.Round(totalAllocationHour, 2)).ToString();
            }
            else
            {
                //DateTime monthDate = Convert.ToDateTime(((DevExpress.Web.ASPxSummaryItem)(e.Item)).FieldName);
                //DateTime startOfMonth = new DateTime(monthDate.Year, monthDate.Month, 1);
                string strdate = Convert.ToString(((DevExpress.Web.ASPxSummaryItem)(e.Item)).FieldName);
                string[] dateArray = strdate.Split('-');
                int month = Convert.ToDateTime("01-" + dateArray[0] + "-2011").Month;
                int year = Convert.ToDateTime("01-01-" + dateArray[1]).Year;
                DateTime monthDate = new DateTime(year, month, 1);
                string strExp = string.Format("MonthStartDate='{0}'", monthDate);
                DataRow[] dr = resourceSummaryTable.Select(strExp);
                double totalAllocationHour = 0.0;
                if (dr.Length > 0)
                {
                    var allocationByMonth = dr.AsEnumerable().ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate));
                    foreach (var monthAlloc in allocationByMonth)
                    {
                        DataRow[] monthAllocRows = monthAlloc.ToArray();
                        totalAllocationHour = monthAllocRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation)) / 100;
                    }
                }
                e.TotalValue = (Math.Round(totalAllocationHour, 2)).ToString();
                ((DevExpress.Web.ASPxSummaryItem)(e.Item)).Tag = "GRAND TOTAL:" + (Math.Round(totalAllocationHour, 2)).ToString();
            }
        }




    }
    //public class CustomFooterRowTemplate : ITemplate
    //{
    //    public string Text { get; set; }
    //    public CustomFooterRowTemplate(string text)
    //    {
    //        this.Text = text;
    //    }

    //    void ITemplate.InstantiateIn(Control container)
    //    {
    //        Label lblTotal = new Label();
    //        lblTotal.Text = Text;
    //        lblTotal.Font.Bold = true;
    //        container.Controls.Add(lblTotal);
    //    }
    //}

}