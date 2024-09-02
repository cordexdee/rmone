using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Helpers;
using DevExpress.XtraReports.UI;

namespace uGovernIT.Web
{
    public partial class ResourceReport : System.Web.UI.UserControl
    {
        #region Member Variables
        protected int year = System.DateTime.Now.Year;

        //private Boolean useAllocation = true;
        public DateTime dateFrom;
        public DateTime dateTo;
        public string startWith;
        public string drillDownTo;
        public string viewType { get; set; }
        private DataTable resourceSummaryTable = null;
        private string manager;
        private string functionalArea;
        Dictionary<string, double> subTotal = new Dictionary<string, double>();
        Dictionary<string, double> grandTotal = new Dictionary<string, double>();
        private List<string> columnList = new List<string>();
        private string dateFieldColumnName = string.Empty;
        public string reportType { get; set; }
        private string reportTypeColumnName = string.Empty;
        public string selectedCategory { get; set; }
        protected StringBuilder urlBuilder = new StringBuilder();
        protected bool printReport = false;

        string title = string.Empty;
        protected bool enableDivision;
        private bool showFunctionalArea = true;
        string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
        string RMMLevel1NPRProjects = uHelper.GetModuleTitle("NPR");
        string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");
        DataTable resultentTable;
        public string unitAllocAct { get; set; }
        private string department;
        private bool GroupByDepartment;
        private bool GroupByFunctionalArea;
        private Dictionary<string, int> lstOfGroupedColumns = new Dictionary<string, int>();
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        ModuleViewManager ModuleManagerObj = null;
        ConfigurationVariableManager ConfigVariableManagerObj = null;
        TicketManager TicketManagerObj = null;
        FieldConfigurationManager configurationManager = null;
        ConfigurationVariableManager ConfigVariableMGR = null;

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            ModuleManagerObj = new ModuleViewManager(AppContext);
            ConfigVariableManagerObj = new ConfigurationVariableManager(AppContext);
            configurationManager = new FieldConfigurationManager(AppContext);
            TicketManagerObj = new TicketManager(AppContext);
            ConfigVariableMGR = new ConfigurationVariableManager(AppContext);
            if (!IsPostBack)
            {
                hdnYear.Value = year.ToString();
                lblProjectYear.Text = year.ToString();
            }
            else
            {
                if (Request[hdnYear.UniqueID] != null)
                    int.TryParse(Request[hdnYear.UniqueID], out year);
            }
            loadPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblReportDateRange.Text = "<b>Report From: </b>" + UGITUtility.GetDateStringInFormat(dateFrom, false) + "<b> To </b>" + UGITUtility.GetDateStringInFormat(dateTo, false);

            if (reportType == "Actuals")
                lblRightHeading.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;View: </b>" + viewType;
            else
                lblRightHeading.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;View: </b>" + viewType;

            if (!string.IsNullOrEmpty(manager))
            {
                lblRightHeading.Text += "<b>&nbsp;&nbsp;&nbsp;&nbsp;Manager: </b>" + AppContext.UserManager.GetUserNameById(manager);
            }

            enableDivision = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableDivision);

            if (!string.IsNullOrEmpty(department))
            {
                string departments;
                //department.Split(',').ToList().ForEach(x =>
                //{
                    //Manager.Department depp = uHelper.GetDepartment(Convert.ToInt32(x));
                    //if (depp != null)
                    //    departments.Add(depp.Name);
                ////});
                departments = RMMSummaryHelper.GetSelectedDepartmentsInfo(AppContext, department, enableDivision);
                lblRightHeading.Text += "<b>&nbsp;&nbsp;&nbsp;&nbsp;Department: </b>" + string.Join(", ", departments);
            }
            gvResourceReport.SettingsExport.ReportFooter = "Report From: " + UGITUtility.GetDateStringInFormat(dateFrom, false) + " To " + UGITUtility.GetDateStringInFormat(dateTo, false); ;
            if (!string.IsNullOrEmpty(functionalArea) && showFunctionalArea)
            {
                if (functionalArea.IndexOf(',') != -1)
                {
                    lblRightHeading.Text += "<b>&nbsp;&nbsp;&nbsp;&nbsp;Functional Area: </b>" + "All";
                }
                else
                    lblRightHeading.Text += "<b>&nbsp;&nbsp;&nbsp;&nbsp;Functional Area: </b>" + functionalArea;
            }

            lblProjectYear.Text = year.ToString();


            gvResourceReport.DataBind();
        }

        #region Member Functions
        private void loadPage()
        {
            trYearSelection.Visible = false;

            //Get the selected categories from request.
            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.ObjectToString(Request["SelectedCategory"]);

            //Get the view type from request.
            if (!string.IsNullOrEmpty(Request["ViewType"]))
                viewType = UGITUtility.ObjectToString(Request["ViewType"]);

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

            if (!string.IsNullOrEmpty(Request["DrillDownTo"]))
            {
                drillDownTo = Convert.ToString(Request["DrillDownTo"]);
            }

            if (!string.IsNullOrEmpty(Request["StartWith"]))
            {
                startWith = Convert.ToString(Request["StartWith"]);
            }

            if (!string.IsNullOrEmpty(Request["FuncArea"]))
            {
                functionalArea = Convert.ToString(Request["FuncArea"]);
                if (functionalArea == "hide")
                    showFunctionalArea = false;
            }

            if (!string.IsNullOrEmpty(Request["Manager"]))
            {
                manager = Convert.ToString(Request["Manager"]);
            }

            if (!string.IsNullOrEmpty(Request["Department"]))
            {
                department = Convert.ToString(Request["Department"]);
            }
            //Get the view type from request.
            if (!string.IsNullOrEmpty(Request["ReportType"]))
                reportType = Request["ReportType"];

            //Get the view type from request.
            if (!string.IsNullOrEmpty(Request["isCallBack"]))
                btnClose.Visible = false;

            //Get unit 
            if (!string.IsNullOrEmpty(Request["unitAllocAct"]))
                unitAllocAct = Convert.ToString(Request["unitAllocAct"]).ToLower();

            if (!string.IsNullOrEmpty(Request["GroupByDepartment"]))
                GroupByDepartment = true;
            if (!string.IsNullOrEmpty(Request["GroupByFunctionalArea"]))
                GroupByFunctionalArea = true;

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
            gvResourceReport.Settings.ShowGroupPanel = true;
        }

        private void GenerateColumns()
        {
            if (lstOfGroupedColumns.Count > 0)
                lstOfGroupedColumns.Clear();

            if (gvResourceReport.Columns.Count <= 0)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                GridViewBandColumn bandCol = new GridViewBandColumn();

                bool showDeptFun = false;
                if (startWith != "Manager/People" && drillDownTo == "People")
                    showDeptFun = true;
                else if (startWith == "Manager/People" && drillDownTo != "Category")
                    showDeptFun = true;
                else if (startWith == "Manager/People" && (GroupByDepartment || GroupByDepartment))
                    showDeptFun = true;

                if (showDeptFun)
                {
                    #region department & functional area
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = DatabaseObjects.Columns.DepartmentNameLookup;
                    colId.Caption = "Department";
                    colId.Name = DatabaseObjects.Columns.DepartmentNameLookup;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    gvResourceReport.Columns.Add(colId);

                    if (showFunctionalArea)
                    {
                        colId = new GridViewDataTextColumn();
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.PropertiesTextEdit.EncodeHtml = true;
                        colId.FieldName = DatabaseObjects.Columns.FunctionalAreaTitleLookup;
                        colId.Caption = "Functional Area";
                        colId.Name = DatabaseObjects.Columns.FunctionalAreaTitleLookup;
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        gvResourceReport.Columns.Add(colId);
                    }
                    #endregion
                }
                if (startWith == "Work Category")
                {
                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = "WorkItemType";
                    colId.Caption = "Category";
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.GroupIndex = 0;

                    gvResourceReport.Columns.Add(colId);
                    if (!lstOfGroupedColumns.ContainsKey(colId.FieldName))
                        lstOfGroupedColumns.Add(colId.FieldName, colId.GroupIndex);

                    if (drillDownTo == "People")
                    {


                        colId = new GridViewDataTextColumn();
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.PropertiesTextEdit.EncodeHtml = true;
                        colId.FieldName = DatabaseObjects.Columns.ResourceName;
                        colId.Caption = "Person";
                        colId.VisibleIndex = 0;
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        colId.FooterTemplate = new CustomFooterRowTemplate("Grand Total:");
                        gvResourceReport.Columns.Add(colId);

                    }
                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = "SubItem";
                    colId.Caption = "Sub Category";
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.GroupFooterTemplate = new CustomFooterRowTemplate("Sub Category Total:");
                    colId.FooterTemplate = new CustomFooterRowTemplate("Grand Total:");
                    if (drillDownTo == "People")
                    {
                        colId.GroupIndex = 1;
                        if (!lstOfGroupedColumns.ContainsKey(colId.FieldName))
                            lstOfGroupedColumns.Add(colId.FieldName, colId.GroupIndex);
                    }
                    gvResourceReport.Columns.Add(colId);
                }
                else
                {


                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = DatabaseObjects.Columns.ManagerName;
                    colId.Caption = "Manager";
                    colId.Name = DatabaseObjects.Columns.ManagerName;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.GroupIndex = 0;

                    gvResourceReport.Columns.Add(colId);
                    if (!lstOfGroupedColumns.ContainsKey(colId.FieldName))
                        lstOfGroupedColumns.Add(colId.FieldName, colId.GroupIndex);

                    colId = new GridViewDataTextColumn();
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                    colId.PropertiesTextEdit.EncodeHtml = true;
                    colId.FieldName = DatabaseObjects.Columns.ResourceName;
                    colId.Caption = "Person";
                    colId.VisibleIndex = 0;
                    colId.Name = DatabaseObjects.Columns.ResourceName;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    colId.GroupFooterTemplate = new CustomFooterRowTemplate("Person Total:");
                    colId.FooterTemplate = new CustomFooterRowTemplate("Grand Total:");
                    if (drillDownTo == "Category")
                    {
                        colId.GroupIndex = 1;
                        if (!lstOfGroupedColumns.ContainsKey(colId.FieldName))
                            lstOfGroupedColumns.Add(colId.FieldName, colId.GroupIndex);

                    }
                    gvResourceReport.Columns.Add(colId);

                    if (drillDownTo == "Category")
                    {
                        colId = new GridViewDataTextColumn();
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.PropertiesTextEdit.EncodeHtml = true;
                        colId.FieldName = "WorkItemType";
                        colId.Caption = "Category";
                        colId.Name = "WorkItemType";
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        colId.GroupFooterTemplate = new CustomFooterRowTemplate("Category Total:");
                        colId.FooterTemplate = new CustomFooterRowTemplate("Grand Total:");
                        colId.GroupIndex = 2;
                        gvResourceReport.Columns.Add(colId);
                        if (!lstOfGroupedColumns.ContainsKey(colId.FieldName))
                            lstOfGroupedColumns.Add(colId.FieldName, colId.GroupIndex);

                        colId = new GridViewDataTextColumn();
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        colId.FooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Right;
                        colId.PropertiesTextEdit.EncodeHtml = true;
                        colId.FieldName = "SubItem";
                        colId.Caption = "Sub Category";
                        colId.Name = "SubItem";
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        gvResourceReport.Columns.Add(colId);
                    }
                }

                if (GroupByFunctionalArea || GroupByDepartment)
                    ReOrderGroupingIndex(lstOfGroupedColumns);


                // Week/Month data columns
                if (columnList != null && columnList.Count > 0)
                {
                    gvResourceReport.GroupSummary.Clear();
                    gvResourceReport.TotalSummary.Clear();
                    bool allowBandColumn = false;
                    if (reportType == "AllocationsActuals")
                        allowBandColumn = true;
                    List<string> lstSummaryColumn = null;
                    foreach (string column in columnList)
                    {
                        lstSummaryColumn = new List<string>();
                        string caption = column;

                        //Get grid column
                        if (!allowBandColumn)
                        {
                            lstSummaryColumn.Add(column);
                            colId = GetGridColumn(column, caption);
                        }

                        if (allowBandColumn)
                        {
                            bandCol = new GridViewBandColumn();
                            bandCol.Caption = column;
                            bandCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bandCol.HeaderStyle.Font.Bold = true;
                            //Add allocation column
                            caption = "Alloc";
                            colId = GetGridColumn(string.Format("Alloc_{0}", column), caption);
                            lstSummaryColumn.Add(string.Format("Alloc_{0}", column));
                            bandCol.Columns.Add(colId);
                            caption = "Act";
                            colId = GetGridColumn(string.Format("Act_{0}", column), caption);
                            lstSummaryColumn.Add(string.Format("Act_{0}", column));
                            bandCol.Columns.Add(colId);

                            gvResourceReport.Columns.Add(bandCol);
                        }
                        else
                        {
                            gvResourceReport.Columns.Add(colId);
                        }

                        if (lstSummaryColumn != null && lstSummaryColumn.Count > 0)
                        {
                            lstSummaryColumn.ForEach(x => { CreateGridSummaryColumn(gvResourceReport, x); });

                        }

                    }

                    gvResourceReport.Settings.GroupFormat = "{1}";
                }
            }
        }

        public DataTable GetDataSource()
        {
            DataTable reportTable = new DataTable();

            DataTable ResourceUsageSummary = null;

            string yearStartDateStr = UGITUtility.ObjectToString(dateFrom);
            string yearEndDateStr = UGITUtility.ObjectToString(dateTo);

            // Set the data of the year.
            if (viewType == "Weekly")
            {
                dateFieldColumnName = DatabaseObjects.Columns.WeekStartDate;
                ResourceUsageSummaryWeekWiseManager weekwiseManager = new ResourceUsageSummaryWeekWiseManager(AppContext);
                ResourceUsageSummary = weekwiseManager.GetDataTable(); // oWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryWeekWise];
            }
            else
            {
                dateFieldColumnName = DatabaseObjects.Columns.MonthStartDate;
                ResourceAllocationMonthlyManager monthlyManager = new ResourceAllocationMonthlyManager(AppContext);
                ResourceUsageSummaryMonthWiseManager monthwiseManager = new ResourceUsageSummaryMonthWiseManager(AppContext);
                ResourceUsageSummary = monthlyManager.GetDataTable();   // oWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryMonthWise];

                //Set the dateFrom as start date of month and dateTo as last day of the month. because the report is dependent on the month.
                dateTo = new DateTime(dateTo.Year, dateTo.Month, DateTime.DaysInMonth(dateTo.Year, dateTo.Month));
                dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, 1);
            }

            // Set the column for report type as "Allocation" or "Actual".
            if (reportType == "Actuals" && (string.IsNullOrEmpty(unitAllocAct) || unitAllocAct.Equals("hours")))
                reportTypeColumnName = DatabaseObjects.Columns.ActualHour;
            else if (reportType == "Actuals")
                reportTypeColumnName = DatabaseObjects.Columns.PctActual;
            else if (reportType != "AllocationsActuals" && (string.IsNullOrEmpty(unitAllocAct) || unitAllocAct.Equals("ftes")))
                reportTypeColumnName = DatabaseObjects.Columns.PctAllocation;
            else
                reportTypeColumnName = DatabaseObjects.Columns.AllocationHour;


            //Get all the between selected date.
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><And><Geq><FieldRef Name='{0}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{1}</Value></Geq><Leq><FieldRef Name='{0}' /><Value Type='DateTime' IncludeTimeValue='FALSE'>{2}</Value></Leq></And></Where>", dateFieldColumnName, yearStartDateStr, yearEndDateStr);
            if (ResourceUsageSummary != null)
            {
                foreach (DataRow dr in ResourceUsageSummary.Rows)
                {
                    if (UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.FunctionalAreaTitleLookup))
                        dr[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = GetFieldByID(dr[DatabaseObjects.Columns.FunctionalAreaTitleLookup].ToString());
                }
                DataRow[] coll = ResourceUsageSummary.Select($"{dateFieldColumnName}>='{ yearStartDateStr}' And {dateFieldColumnName} <= '{yearEndDateStr}' ");

                //Filter the data for selected category.
                if (coll.Count() > 0)
                {
                    resourceSummaryTable = coll.CopyToDataTable();
                    for (int i = 0; i < resourceSummaryTable.Rows.Count; i++)
                    {
                        resourceSummaryTable.Columns[DatabaseObjects.Columns.Resource].DataType = typeof(string);
                        var values = configurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.Resource, Convert.ToString(resourceSummaryTable.Rows[i][DatabaseObjects.Columns.Resource]), null);
                        if (!string.IsNullOrWhiteSpace(values))
                        {

                            resourceSummaryTable.Rows[i][DatabaseObjects.Columns.Resource] = values;
                        }
                    }
                    #region Update user department & functional area
                    //Update user department & functional area 
                    if (resourceSummaryTable != null && resourceSummaryTable.Rows.Count > 0)
                    {
                        if (!uHelper.IfColumnExists(DatabaseObjects.Columns.DepartmentNameLookup, resourceSummaryTable))
                            resourceSummaryTable.Columns.Add(DatabaseObjects.Columns.DepartmentNameLookup);
                        if (!uHelper.IfColumnExists(DatabaseObjects.Columns.DepartmentID, resourceSummaryTable))
                            resourceSummaryTable.Columns.Add(DatabaseObjects.Columns.DepartmentID);

                        if (!uHelper.IfColumnExists(DatabaseObjects.Columns.FunctionalAreaTitleLookup, resourceSummaryTable))
                            resourceSummaryTable.Columns.Add(DatabaseObjects.Columns.FunctionalAreaTitleLookup);

                        List<string> lstResource = resourceSummaryTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.Resource)).Select(x => x.Field<string>(DatabaseObjects.Columns.Resource)).Distinct().ToList();
                        List<UserProfile> lstOfUserProfile = new List<UserProfile>();
                        if (lstResource != null && lstResource.Count > 0)
                        {
                            UserProfile userProfile = null;
                            lstResource.ForEach(x =>
                            {
                                userProfile = AppContext.UserManager.GetUserInfoByIdOrName(x);
                                // userProfile = AppContext.UserManager.GetUsersProfile();
                                if (userProfile != null)
                                    lstOfUserProfile.Add(userProfile);
                            });
                        }

                        if (lstOfUserProfile != null && lstOfUserProfile.Count > 0)
                        {
                            DepartmentManager departmentManager = new DepartmentManager(AppContext);
                            foreach (UserProfile profile in lstOfUserProfile)
                            {

                                var profileName = profile.Name;
                                if (profileName.Contains('\''))
                                {
                                    profileName = profileName.Replace("'", "''");

                                }
                                var resourceExpression = string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, profileName);
                                var resourceExpression1 = string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, profile.Id);

                                DataRow[] drColl = resourceSummaryTable.Select(resourceExpression);
                                if (drColl == null || drColl.Length == 0)
                                    continue;
                                drColl.ToList<DataRow>().ForEach(x =>
                                {
                                    x[DatabaseObjects.Columns.DepartmentID] = profile.DepartmentId;
                                    x[DatabaseObjects.Columns.DepartmentNameLookup] = departmentManager.LoadByID(Convert.ToInt64(profile.Department))?.Title;
                                    x[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = GetFieldByID(Convert.ToString(profile.FunctionalArea)); //Convert.ToString(configurationManager.GetFieldByID(Convert.ToString(profile.FunctionalArea)));
                                    x[DatabaseObjects.Columns.Resource] = profile.Name;
                                });
                            }

                            resourceSummaryTable.AcceptChanges();
                        }

                    }
                    #endregion

                    //In case of multiple functional area against department

                    string funcAreaExpression = string.Empty;
                    string departmentExpression = string.Empty;

                    if (!string.IsNullOrWhiteSpace(department))
                    {
                        var LastCharOfString = department[department.Length - 1];
                        if (LastCharOfString == ',')
                        {
                            department = department.Remove(department.LastIndexOf(','), 1);
                        }
                        departmentExpression = string.Format("{0} in ({1})", DatabaseObjects.Columns.DepartmentNameLookup, string.Join(",", department));

                    }

                    funcAreaExpression = !string.IsNullOrEmpty(functionalArea) ? string.Format("{0} ='{1}'", DatabaseObjects.Columns.FunctionalAreaTitleLookup, functionalArea.Replace("'", "''")) : string.Empty;

                    if (!string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(funcAreaExpression) && funcAreaExpression.IndexOf(',') != -1)
                    {
                        string[] express = UGITUtility.SplitString(functionalArea, Constants.Separator6);
                        string tempStr = string.Empty;

                        express.ToList().ForEach(x =>
                        {
                            if (tempStr == string.Empty)
                                tempStr = string.Format("'{0}'", x);
                            else
                                tempStr = string.Format("{0},'{1}'", tempStr, x);
                        });
                        funcAreaExpression = string.Format("{0} In ({1})", DatabaseObjects.Columns.FunctionalAreaTitleLookup, tempStr);
                    }

                    string[] filter = new string[] {
                                                 !string.IsNullOrEmpty(manager) ? string.Format("{0} ='{1}'", DatabaseObjects.Columns.ManagerLookup, manager.Replace("'","''")) : string.Empty,departmentExpression,
                                                 funcAreaExpression
                                               };

                    string qry = string.Join(" And ", filter.Where(x => !string.IsNullOrEmpty(x)).ToArray());

                    if (!string.IsNullOrEmpty(qry))
                    {
                        DataRow[] drs = resourceSummaryTable.Select(qry);
                        if (drs != null && drs.Length > 0)
                        {
                            resourceSummaryTable = drs.CopyToDataTable();
                        }
                        else
                            resourceSummaryTable = resourceSummaryTable.Clone();// show blank if no data found 
                    }

                    if (resourceSummaryTable != null)
                    {
                        if (resourceSummaryTable.Rows.Count > 0 && !string.IsNullOrEmpty(reportType))
                        {
                            DataRow[] excludeZeroHours = null;
                            switch (reportType)
                            {
                                case "Allocation":
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AllocationHour, resourceSummaryTable))
                                        excludeZeroHours = resourceSummaryTable.Select($"{DatabaseObjects.Columns.AllocationHour} > 0");// resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.AllocationHour) > 0).ToArray();
                                    break;
                                case "Actuals":
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ActualHour, resourceSummaryTable))
                                        excludeZeroHours = resourceSummaryTable.Select($"{DatabaseObjects.Columns.ActualHour} > 0");   // resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.ActualHour) > 0).ToArray();
                                    break;
                                case "AllocationsActuals":
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AllocationHour, resourceSummaryTable) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ActualHour, resourceSummaryTable))
                                        excludeZeroHours = resourceSummaryTable.Select($"{DatabaseObjects.Columns.AllocationHour} > 0 OR {DatabaseObjects.Columns.ActualHour} > 0");  //resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.AllocationHour) > 0 || x.Field<double>(DatabaseObjects.Columns.ActualHour) > 0).ToArray();
                                    break;
                                default:
                                    break;

                            }

                            if (excludeZeroHours == null || excludeZeroHours.Length == 0)
                                resourceSummaryTable = resourceSummaryTable.Clone();
                            else
                                resourceSummaryTable = excludeZeroHours.CopyToDataTable();
                        }

                        DataTable teampResourceTable = resourceSummaryTable.Clone();

                        Dictionary<string, string> moduleTitles = new Dictionary<string, string>();
                        DataTable modules = ModuleManagerObj.GetDataTable();  // uGITCache.GetModuleList(ModuleType.All);
                        foreach (DataRow row in modules.Rows)
                        {
                            moduleTitles.Add(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]), Convert.ToString(row[DatabaseObjects.Columns.Title]));
                        }

                        if (!string.IsNullOrEmpty(selectedCategory))
                        {
                            string[] allcategory = selectedCategory.Split('#');

                            if (allcategory.Length >= 1)
                            {
                                string categoryInternalName = string.Empty;
                                foreach (string category in allcategory)
                                {

                                    categoryInternalName = category;
                                    if (moduleTitles.FirstOrDefault(x => x.Value.ToLower() == category.ToLower()).Key != null)
                                        categoryInternalName = moduleTitles.FirstOrDefault(x => x.Value.ToLower() == category.ToLower()).Key;

                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.WorkItemType, resourceSummaryTable))
                                    {
                                        DataRow[] selectedRows = resourceSummaryTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemType, categoryInternalName.Replace("'", "''")));

                                        if (selectedRows.Length > 0)
                                        {
                                            foreach (DataRow row in selectedRows)
                                            {
                                                teampResourceTable.ImportRow(row);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Create a list of column for the table we will use to bind the List view.
                        teampResourceTable.DefaultView.Sort = dateFieldColumnName;
                        teampResourceTable = teampResourceTable.DefaultView.ToTable();
                        var StartDays = teampResourceTable.AsEnumerable().ToLookup(x => x.Field<DateTime>(dateFieldColumnName));

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

                        columnList = columnList.Distinct().ToList();

                        if (viewType == "Weekly")
                        {
                            string timesheetMode = ConfigVariableManagerObj.GetValue(ConfigConstants.TimesheetMode); // uGITCache.GetConfigVariableValue(ConfigConstants.TimesheetMode, SPContext.Current.Web);
                            if ((timesheetMode.ToLower() == Constants.SignOffMode.ToLower() || timesheetMode.ToLower() == Constants.ApprovalMode.ToLower()) && UGITUtility.StringToBoolean(Request["ApprovedHoursOnly"]))
                            {
                                DataRow[] itemColl = GetResourceTimeSheetForRange(dateFrom, dateTo);
                                if (itemColl != null && itemColl.Count() > 0)
                                {

                                    List<string> approvedSheetUsers = itemColl.CopyToDataTable().AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Resource])).Distinct().ToList();
                                    if (approvedSheetUsers != null && approvedSheetUsers.Count > 0)
                                    {
                                        DataRow[] existUsersData = teampResourceTable.AsEnumerable().Where(x => approvedSheetUsers.Exists(y => y == Convert.ToString(x[DatabaseObjects.Columns.Resource]))).ToArray();
                                        if (existUsersData != null && existUsersData.Length > 0)
                                            teampResourceTable = existUsersData.CopyToDataTable();
                                        else
                                            teampResourceTable = teampResourceTable.Clone();
                                    }

                                    var signOffStartDays = itemColl.CopyToDataTable().AsEnumerable().ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.UGITStartDate));

                                    if (columnList != null && columnList.Count > 0)
                                        columnList.Clear();
                                    foreach (var startday in signOffStartDays)
                                    {
                                        columnList.Add(UGITUtility.GetDateStringInFormat(Convert.ToDateTime(startday.Key), false));
                                    }

                                    columnList = columnList.Distinct().ToList();

                                }
                                else
                                    teampResourceTable = teampResourceTable.Clone();
                            }

                            if (startWith == "Work Category")
                            {
                                if (drillDownTo == "People")
                                    reportTable = CreateWeeklyFormatTableWithPeopleDrilldown(teampResourceTable);
                                else
                                    reportTable = CreateWeeklyFormatTable(teampResourceTable);
                            }
                            else
                            {
                                if (drillDownTo == "Category")
                                    reportTable = CreateWeeklyFormatTableWithCategoryDrillDown(teampResourceTable);
                                else
                                    reportTable = CreateWeeklyFormatTableForManager(teampResourceTable);
                            }
                        }
                        else
                        {
                            if (startWith == "Work Category")
                            {
                                if (drillDownTo == "People")
                                    reportTable = CreateMonthlyFormatTableWithPeopleDrillDown(teampResourceTable);
                                else
                                    reportTable = CreateMonthlyFormatTable(teampResourceTable);
                            }
                            else
                            {
                                if (drillDownTo == "Category")
                                    reportTable = CreateMonthlyFormatTableWithCategoryDrillDown(teampResourceTable);
                                else
                                    reportTable = CreateMonthlyFormatTableForManager(teampResourceTable);
                            }
                        }
                    }
                    GenerateColumns();
                }
            }
            resultentTable = reportTable;
            return reportTable;
        }

        private DataTable CreateWeeklyFormatTable(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);
            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);
            DataColumn SubType = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(SubType);
            DataColumn title = new DataColumn("Title", typeof(string));
            formatTable.Columns.Add(title);
            DataColumn department = new DataColumn("DepartmentNameLookup", typeof(string));
            formatTable.Columns.Add(department);
            DataColumn functionalarea = new DataColumn("FunctionalAreaTitleLookup", typeof(string));
            formatTable.Columns.Add(functionalarea);

            //Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);


            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateWeeklyFormatTableWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateWeeklyFormatTableWithExtraGroup(sourceTable, formatTable);
            }



            formatTable.DefaultView.Sort = "WorkItemType ASC, WorkItem ASC, SubItem ASC";
            return formatTable.DefaultView.ToTable();
        }
        private DataTable CreateWeeklyFormatTableWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType));

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] workTypeRows = workType.ToArray();
                var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);

                foreach (var workItem in allocationByWorkItem)
                {
                    DataRow[] workItemRows = workItem.ToArray();
                    var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));

                    DataRow newRow = null;
                    double totalAllocationHours = 0.0;
                    double totalActualsHours = 0.0;
                    string titleVal = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);
                    foreach (var monthAlloc in allocationByMonth)
                    {
                        DataRow[] monthAllocRows = monthAlloc.ToArray();
                        double allocationHours = 0.0;
                        double actualsHours = 0.0;

                        GetActualAllocationHours(unitAllocAct, monthAllocRows, out actualsHours, out allocationHours);

                        totalAllocationHours += allocationHours;
                        totalActualsHours += actualsHours;
                        if (newRow == null)
                        {
                            newRow = formatTable.NewRow();
                            newRow["WorkItem"] = Convert.ToString(workType.Key);
                            newRow["SubItem"] = workItem.Key;
                            if (!string.IsNullOrWhiteSpace(titleVal))
                                newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);

                            newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(department) ? "-- No Department --" : department;
                            newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(functionalarea) ? "-- No Functional Area --" : functionalarea;

                        }

                        string columnTitle = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false);
                        if (uHelper.IfColumnExists(columnTitle, newRow.Table))
                            newRow[columnTitle] = reportType == "AllocationsActuals" ? actualsHours : allocationHours;
                        if (reportType == "AllocationsActuals")
                        {
                            string columnName = string.Format("Alloc_{0}", columnTitle);
                            if (uHelper.IfColumnExists(columnName, newRow.Table))
                                newRow[columnName] = allocationHours;

                            string columnNameAct = string.Format("Act_{0}", columnTitle);
                            if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                newRow[columnNameAct] = actualsHours;

                        }
                    }

                    if (newRow != null && (totalAllocationHours > 0 || totalActualsHours > 0))
                    {
                        if (!string.IsNullOrWhiteSpace(titleVal))
                        {
                            newRow[DatabaseObjects.Columns.Title] = titleVal;
                            newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                        }

                        formatTable.Rows.Add(newRow);
                    }
                }
            }
            return formatTable;
        }
        private DataTable CreateWeeklyFormatTableForManager(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn ResourceManager = new DataColumn(DatabaseObjects.Columns.ManagerName, typeof(string));
            formatTable.Columns.Add(ResourceManager);

            DataColumn Resource = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(Resource);

            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);

            DataColumn functionalarealookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionalarealookup);

            Dictionary<DateTime, double> workHoursList = new Dictionary<DateTime, double>();

            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateWeeklyFormatTableForManagerWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableForManagerWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableForManagerWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateWeeklyFormatTableForManagerWithExtraGroup(sourceTable, formatTable);
            }


            return formatTable;
        }
        private DataTable CreateWeeklyFormatTableWithPeopleDrilldown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);
            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);
            DataColumn SubType = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(SubType);
            DataColumn title = new DataColumn("Title", typeof(string));
            formatTable.Columns.Add(title);

            DataColumn resourceName = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(resourceName);

            DataColumn ERPJobID = new DataColumn(DatabaseObjects.Columns.ERPJobID, typeof(string));
            formatTable.Columns.Add(ERPJobID);

            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);
            DataColumn functionalarea = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionalarea);



            //Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);

            CreateTableSchema(columnList, formatTable);
            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateWeeklyFormatTableWithPeopleDrilldownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithPeopleDrilldownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithPeopleDrilldownWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateWeeklyFormatTableWithPeopleDrilldownWithExtraGroup(sourceTable, formatTable);
            }

            formatTable.DefaultView.Sort = "WorkItemType ASC, WorkItem ASC, SubItem ASC, ResourceNameUser ASC";
            return formatTable.DefaultView.ToTable();
        }
        private DataTable CreateWeeklyFormatTableWithPeopleDrilldownWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType));

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] workTypeRows = workType.ToArray();
                var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);
                foreach (var workItem in allocationByWorkItem)
                {

                    DataRow[] workItemRows = workItem.ToArray();
                    var allocationByResource = workItemRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);
                    string titleVal = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);

                    foreach (var resource in allocationByResource)
                    {

                        DataRow[] resourceRows = resource.ToArray();
                        var allocationByMonth = resourceRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));

                        DataRow newRow = null;
                        double totalAllocationHours = 0.0;
                        double totalActualsHours = 0.0;
                        foreach (var monthAlloc in allocationByMonth)
                        {
                            DataRow[] monthAllocRows = monthAlloc.ToArray();
                            double allocationHours = 0.0;
                            double actualsHours = 0.0;
                            GetActualAllocationHours(unitAllocAct, monthAllocRows, out actualsHours, out allocationHours);

                            totalAllocationHours += allocationHours;
                            totalActualsHours += actualsHours;

                            if (newRow == null)
                            {
                                newRow = formatTable.NewRow();
                                newRow["WorkItem"] = Convert.ToString(workType.Key);
                                newRow["SubItem"] = workItem.Key; 
                                newRow[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.ERPJobID]);
                                if (UGITUtility.IsValidTicketID(workItem.Key))
                                {
                                    var dRow = Ticket.GetCurrentTicket(AppContext, uHelper.getModuleNameByTicketId(workItem.Key), workItem.Key);
                                    if (!string.IsNullOrWhiteSpace(titleVal))
                                        if (dRow != null)
                                            newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, dRow[DatabaseObjects.Columns.Title]);
                                }

                                newRow[DatabaseObjects.Columns.ResourceName] = AppContext.UserManager.GetUserNameById(resource.Key);
                                newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(resourceRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Department --" : Convert.ToString(resourceRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                                newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(resourceRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(resourceRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                            }

                            string columnTitle = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false);
                            if (uHelper.IfColumnExists(columnTitle, newRow.Table))
                                newRow[columnTitle] = reportType == "AllocationsActuals" ? actualsHours : allocationHours;

                            if (reportType == "AllocationsActuals")
                            {
                                string columnName = string.Format("Alloc_{0}", columnTitle);
                                if (uHelper.IfColumnExists(columnName, newRow.Table))
                                    newRow[columnName] = allocationHours;

                                string columnNameAct = string.Format("Act_{0}", columnTitle);
                                if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                    newRow[columnNameAct] = actualsHours;

                            }
                        }

                        if (newRow != null && (totalAllocationHours > 0 || totalActualsHours > 0))
                        {
                                if (!string.IsNullOrWhiteSpace(titleVal))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = titleVal;
                                //newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                                if (!string.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.ERPJobID])))
                                    newRow["SubItem"] = string.Format("{0} ({1}, {2})", newRow[DatabaseObjects.Columns.Title], newRow[DatabaseObjects.Columns.ERPJobID], workItem.Key);
                                else
                                    newRow["SubItem"] = string.Format("{0} ({1})", newRow[DatabaseObjects.Columns.Title], workItem.Key);
                            }
                            formatTable.Rows.Add(newRow);
                        }
                    }
                }
            }
            return formatTable;
        }
        private DataTable CreateWeeklyFormatTableForManagerWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);

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
                        double totalActualsHours = 0.0;

                        GetActualAllocationHours(unitAllocAct, monthAllocRows, out totalActualsHours, out totalAllocationHour);

                        if (newRow == null)
                        {
                            newRow = formatTable.NewRow();
                            //string managerName = string.Empty;
                            //if (string.IsNullOrEmpty(workType.Key))
                            //    managerName = !string.IsNullOrWhiteSpace(workType.Key) ? workType.Key : "-- No Manager --";
                            //else
                            //{
                            //    List<UserProfile> lstOfUserProfile = new List<UserProfile>();
                            //    UserProfile userProfile = null;
                            //    userProfile = AppContext.UserManager.GetUserById(workType.Key);
                            //    managerName = userProfile.Name;
                            //}
                            newRow[DatabaseObjects.Columns.ManagerName] = !string.IsNullOrWhiteSpace(workType.Key) ? workType.Key : "-- No Manager --"; 
                            newRow[DatabaseObjects.Columns.ResourceName] = Convert.ToString(workItemRows[0][DatabaseObjects.Columns.ResourceName]);
                            newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Department --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                            newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                        }

                        string columnTitle = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false);
                        if (uHelper.IfColumnExists(columnTitle, newRow.Table))
                            newRow[columnTitle] = reportType == "AllocationsActuals" ? totalActualsHours : totalAllocationHour;
                        if (reportType == "AllocationsActuals")
                        {
                            string columnName = string.Format("Alloc_{0}", columnTitle);
                            if (uHelper.IfColumnExists(columnName, newRow.Table))
                                newRow[columnName] = totalAllocationHour;

                            string columnNameAct = string.Format("Act_{0}", columnTitle);
                            if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                newRow[columnNameAct] = totalActualsHours;
                        }
                    }

                    if (newRow != null)
                        formatTable.Rows.Add(newRow);
                }
            }
            return formatTable;
        }
        private DataTable CreateWeeklyFormatTableWithCategoryDrillDownWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] managerRows = workType.ToArray();
                var allocationByResource = managerRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);

                foreach (var resource in allocationByResource)
                {
                    DataRow[] resourceRows = resource.ToArray();
                    var allocationByWorkItem = resource.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType)).OrderBy(x => x.Key);
                    foreach (var workItem in allocationByWorkItem)
                    {
                        DataRow[] subItemArray = workItem.ToArray();
                        var subItemArrayLookup = subItemArray.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);
                        foreach (var subItem in subItemArrayLookup)
                        {
                            DataRow[] workItemRows = subItem.ToArray();
                            var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate));
                            string titleVal = Convert.ToString(subItem.ToList()[0][DatabaseObjects.Columns.Title]);

                            DataRow newRow = null;

                            foreach (var monthAlloc in allocationByMonth)
                            {
                                DataRow[] monthAllocRows = monthAlloc.ToArray();
                                double totalAllocationHour = 0.0;
                                double totalActualsHours = 0.0;
                                GetActualAllocationHours(unitAllocAct, monthAllocRows, out totalActualsHours, out totalAllocationHour);

                                if (newRow == null)
                                {
                                    newRow = formatTable.NewRow();
                                    string managerName = !string.IsNullOrWhiteSpace(workType.Key) ? workType.Key : "-- No Manager --";
                                    newRow[DatabaseObjects.Columns.ManagerName] = managerName;
                                    newRow[DatabaseObjects.Columns.ResourceName] = AppContext.UserManager.GetUserNameById(resource.Key);
                                    newRow["WorkItem"] = workItem.Key;
                                    newRow["SubItem"] = subItem.Key;
                                    newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(resourceRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Department --" : Convert.ToString(resourceRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                                    newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(resourceRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(resourceRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                                }

                                string columnTitle = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.WeekStartDate]), false);
                                if (uHelper.IfColumnExists(columnTitle, newRow.Table))
                                    newRow[columnTitle] = reportType == "AllocationsActuals" ? totalActualsHours : totalAllocationHour;
                                if (reportType == "AllocationsActuals")
                                {
                                    string columnName = string.Format("Alloc_{0}", columnTitle);
                                    if (uHelper.IfColumnExists(columnName, newRow.Table))
                                        newRow[columnName] = totalAllocationHour;

                                    string columnNameAct = string.Format("Act_{0}", columnTitle);
                                    if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                        newRow[columnNameAct] = totalActualsHours;
                                }
                            }

                            if (newRow != null)
                            {
                                if (!string.IsNullOrEmpty(titleVal))
                                    newRow["SubItem"] = string.Format("{0}: {1}", subItem.Key, titleVal);
                                formatTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }
            return formatTable;
        }
        private DataTable CreateMonthlyFormatTable(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);
            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);
            DataColumn SubType = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(SubType);
            DataColumn title = new DataColumn("Title", typeof(string));
            formatTable.Columns.Add(title);
            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);
            DataColumn FunctionalAreaTitleLookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(FunctionalAreaTitleLookup);
            //Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);

            CreateTableSchema(columnList, formatTable);


            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateMonthlyFormatTableWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateMonthlyFormatTableWithExtraGroup(sourceTable, formatTable);
            }

            formatTable.DefaultView.Sort = "WorkItemType ASC, WorkItem ASC, SubItem ASC";
            return formatTable.DefaultView.ToTable();
        }
        private DataTable CreateMonthlyFormatTableWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType));

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] workTypeRows = workType.ToArray();
                var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);

                foreach (var workItem in allocationByWorkItem)
                {
                    DataRow[] workItemRows = workItem.ToArray();
                    var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month);
                    string titleVal = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);

                    DataRow newRow = null;
                    double totalAllocationHours = 0.0;
                    double totalActualsHours = 0.0;
                    foreach (var monthAlloc in allocationByMonth)
                    {
                        DataRow[] monthAllocRows = monthAlloc.ToArray();
                        double allocationHours = 0.0;
                        double actualsHours = 0.0;
                        GetActualAllocationHours(unitAllocAct, monthAllocRows, out actualsHours, out allocationHours);

                        totalAllocationHours += allocationHours;
                        totalActualsHours += actualsHours;

                        if (newRow == null)
                        {
                            newRow = formatTable.NewRow();
                            newRow["WorkItem"] = Convert.ToString(workType.Key);
                            newRow["SubItem"] = workItem.Key;
                            if (!string.IsNullOrWhiteSpace(titleVal))
                                newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                            newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(department) ? "-- No Department --" : department;
                            newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(functionalArea) ? "-- No Functional Area --" : functionalArea;
                        }

                        string colTitle = Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy");
                        if (uHelper.IfColumnExists(colTitle, newRow.Table))
                            newRow[colTitle] = reportType == "AllocationsActuals" ? actualsHours : allocationHours;

                        if (reportType == "AllocationsActuals")
                        {
                            string columnName = string.Format("Alloc_{0}", colTitle);
                            if (uHelper.IfColumnExists(columnName, newRow.Table))
                                newRow[columnName] = allocationHours;

                            string columnNameAct = string.Format("Act_{0}", colTitle);
                            if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                newRow[columnNameAct] = actualsHours;

                        }
                    }

                    if (newRow != null && (totalAllocationHours > 0 || totalActualsHours > 0))
                    {
                        if (!string.IsNullOrEmpty(titleVal))
                        {
                            newRow[DatabaseObjects.Columns.Title] = titleVal;
                            newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                        }
                        formatTable.Rows.Add(newRow);
                    }
                }
            }
            return formatTable;
        }
        private DataTable CreateMonthlyFormatTableWithPeopleDrillDownWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {

            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType));

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] workTypeRows = workType.ToArray();
                var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);

                foreach (var workItem in allocationByWorkItem)
                {
                    DataRow[] resourceRows = workItem.ToArray();
                    var allocationByResource = resourceRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);
                    string titleVal = string.Empty;

                    foreach (var resouce in allocationByResource)
                    {
                        DataRow[] workItemRows = resouce.ToArray();
                        var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month);

                        DataRow newRow = null;
                        double totalAllocationHours = 0.0;
                        double totalActualsHours = 0.0;
                        foreach (var monthAlloc in allocationByMonth)
                        {
                            DataRow[] monthAllocRows = monthAlloc.ToArray();
                            double allocationHours = 0.0;
                            double actualsHours = 0.0;

                            GetActualAllocationHours(unitAllocAct, monthAllocRows, out actualsHours, out allocationHours);

                            totalAllocationHours += allocationHours;
                            totalActualsHours += actualsHours;
                            if (newRow == null)
                            {
                                newRow = formatTable.NewRow();
                                newRow["WorkItem"] = Convert.ToString(workType.Key);
                                titleVal = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);
                                newRow[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.ERPJobID]);
                                if (!string.IsNullOrEmpty(titleVal.Trim()))
                                {
                                    titleVal = string.Format(": {0}", titleVal);
                                }
                                newRow["SubItem"] = Convert.ToString(workItem.Key) + titleVal;
                                newRow[DatabaseObjects.Columns.ResourceName] = AppContext.UserManager.GetUserNameById(resouce.Key);
                                newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Deparment --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                                newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                            }

                            string colNameMonth = Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM");
                            string colNameYear = Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy");
                            string colName = colNameMonth + "-" + colNameYear;
                            if (uHelper.IfColumnExists(colName, newRow.Table))
                                newRow[colName] = reportType == "AllocationsActuals" ? actualsHours : allocationHours;

                            if (reportType == "AllocationsActuals")
                            {
                                string columnName = string.Format("Alloc_{0}", colName);
                                if (uHelper.IfColumnExists(columnName, newRow.Table))
                                    newRow[columnName] = allocationHours;

                                string columnNameAct = string.Format("Act_{0}", colName);
                                if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                    newRow[columnNameAct] = actualsHours;

                            }
                        }

                        if (newRow != null && (totalAllocationHours > 0 || totalActualsHours > 0))
                        {
                            if (!string.IsNullOrEmpty(titleVal))
                            {
                                newRow[DatabaseObjects.Columns.Title] = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);
                                //newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, newRow[DatabaseObjects.Columns.Title]);

                                if (!string.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.ERPJobID])))
                                    newRow["SubItem"] = string.Format("{0} ({1}, {2})", newRow[DatabaseObjects.Columns.Title], newRow[DatabaseObjects.Columns.ERPJobID], workItem.Key);
                                else
                                    newRow["SubItem"] = string.Format("{0} ({1})", newRow[DatabaseObjects.Columns.Title], workItem.Key);
                            }
                            formatTable.Rows.Add(newRow);
                        }
                    }
                }
            }
            return formatTable;
        }
        private DataTable CreateMonthlyFormatTableWithCategoryDrillDownWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType));

            foreach (var workType in allocationByWorkTypes)
            {
                DataRow[] workTypeRows = workType.ToArray();
                var allocationByWorkItem = workTypeRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).OrderBy(x => x.Key);

                foreach (var workItem in allocationByWorkItem)
                {

                    DataRow[] ManagerRows = workItem.ToArray();
                    var allocationByManager = ManagerRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);
                    string titleVal = Convert.ToString(workItem.ToList()[0][DatabaseObjects.Columns.Title]);

                    foreach (var manager in allocationByManager)
                    {
                        DataRow[] resourceRows = manager.ToArray();
                        var allocationByResource = resourceRows.ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource)).OrderBy(x => x.Key);

                        foreach (var resouce in allocationByResource)
                        {
                            DataRow[] workItemRows = resouce.ToArray();
                            var allocationByMonth = workItemRows.ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Month);

                            DataRow newRow = null;
                            double totalAllocationHours = 0.0;
                            double totalActualsHours = 0.0;
                            foreach (var monthAlloc in allocationByMonth)
                            {
                                DataRow[] monthAllocRows = monthAlloc.ToArray();
                                double allocationHours = 0.0;
                                double actualsHours = 0.0;
                                GetActualAllocationHours(unitAllocAct, monthAllocRows, out actualsHours, out allocationHours);

                                totalAllocationHours += allocationHours;
                                totalActualsHours += actualsHours;
                                if (newRow == null)
                                {
                                    newRow = formatTable.NewRow();
                                    newRow["WorkItem"] = Convert.ToString(workType.Key);
                                    string mgrName = !string.IsNullOrWhiteSpace(manager.Key) ? manager.Key : "-- No Manager --";
                                    newRow[DatabaseObjects.Columns.ManagerName] = mgrName;
                                    newRow[DatabaseObjects.Columns.ResourceName] = Convert.ToString(workItemRows[0][DatabaseObjects.Columns.ResourceName]);
                                    newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Deparment --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                                    newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                                    newRow["SubItem"] = workItem.Key;
                                    if (!string.IsNullOrWhiteSpace(titleVal))
                                        newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                                }

                                string colTitle = Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy");
                                if (uHelper.IfColumnExists(colTitle, newRow.Table))
                                    newRow[colTitle] = reportType == "AllocationsActuals" ? actualsHours : allocationHours;
                                if (reportType == "AllocationsActuals")
                                {
                                    string columnName = string.Format("Alloc_{0}", colTitle);
                                    if (uHelper.IfColumnExists(columnName, newRow.Table))
                                        newRow[columnName] = allocationHours;

                                    string columnNameAct = string.Format("Act_{0}", colTitle);
                                    if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                        newRow[columnNameAct] = actualsHours;
                                }
                            }

                            if (newRow != null && (totalAllocationHours > 0 || totalActualsHours > 0))
                            {
                                if (!string.IsNullOrEmpty(titleVal))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = titleVal;
                                    newRow["SubItem"] = string.Format("{0}: {1}", workItem.Key, titleVal);
                                }
                                formatTable.Rows.Add(newRow);
                            }
                        }
                    }
                }

            }
            return formatTable;
        }
        private DataTable CreateMonthlyFormatTableForManagerWithExtraGroup(DataTable sourceTable, DataTable formatTable, string department = "", string functionalarea = "")
        {
            var allocationByWorkTypes = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ManagerLookup)).OrderBy(x => x.Key);

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
                        double totalActualsHours = 0.0;
                        string unitToShow = unitAllocAct.ToLower();

                        GetActualAllocationHours(unitToShow, monthAllocRows, out totalActualsHours, out totalAllocationHour);

                        if (newRow == null)
                        {
                            newRow = formatTable.NewRow();
                            string managerName = !string.IsNullOrWhiteSpace(workType.Key) ? workType.Key : "-- No Manager --";
                            if (managerName != "-- No Manager --")
                            {
                                List<UserProfile> lstOfUserProfile = new List<UserProfile>();
                                UserProfile userProfile = null;
                                userProfile = AppContext.UserManager.GetUserById(workType.Key);
                                managerName = userProfile.Name;
                            }
                            newRow[DatabaseObjects.Columns.ManagerName] = managerName;
                            newRow[DatabaseObjects.Columns.ResourceName] = Convert.ToString(workItemRows[0][DatabaseObjects.Columns.ResourceName]);
                            newRow[DatabaseObjects.Columns.DepartmentNameLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup])) ? "-- No Department --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.DepartmentNameLookup]);
                            newRow[DatabaseObjects.Columns.FunctionalAreaTitleLookup] = string.IsNullOrEmpty(Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup])) ? "-- No Functional Area --" : Convert.ToString(workItemRows[0][DatabaseObjects.Columns.FunctionalAreaTitleLookup]);
                        }

                        string colTitle = Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(monthAllocRows[0][DatabaseObjects.Columns.MonthStartDate]).ToString("yy");
                        if (uHelper.IfColumnExists(colTitle, newRow.Table))
                            newRow[colTitle] = reportType == "AllocationsActuals" ? totalActualsHours : totalAllocationHour;
                        if (reportType == "AllocationsActuals")
                        {
                            string columnName = string.Format("Alloc_{0}", colTitle);
                            if (uHelper.IfColumnExists(columnName, newRow.Table))
                                newRow[columnName] = totalAllocationHour;

                            string columnNameAct = string.Format("Act_{0}", colTitle);
                            if (uHelper.IfColumnExists(columnNameAct, newRow.Table))
                                newRow[columnNameAct] = totalActualsHours;

                        }

                    }
                    if (newRow != null)
                        formatTable.Rows.Add(newRow);
                }
            }
            return formatTable;
        }
        private DataTable CreateWeeklyFormatTableWithCategoryDrillDown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);

            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);

            DataColumn ResourceManager = new DataColumn(DatabaseObjects.Columns.ManagerName, typeof(string));
            formatTable.Columns.Add(ResourceManager);

            DataColumn Resource = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(Resource);

            DataColumn subItemCol = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(subItemCol);

            DataColumn deparmentlookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(deparmentlookup);

            DataColumn functionallookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionallookup);

            DataColumn ERPJobID = new DataColumn(DatabaseObjects.Columns.ERPJobID, typeof(string));
            formatTable.Columns.Add(ERPJobID);

            ///Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);

            Dictionary<DateTime, double> workHoursList = new Dictionary<DateTime, double>();

            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateWeeklyFormatTableWithCategoryDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithCategoryDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateWeeklyFormatTableWithCategoryDrillDownWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateWeeklyFormatTableWithCategoryDrillDownWithExtraGroup(sourceTable, formatTable);
            }

            return formatTable;
        }
        private DataTable CreateMonthlyFormatTableWithCategoryDrillDown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);
            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);
            DataColumn SubType = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(SubType);
            DataColumn title = new DataColumn("Title", typeof(string));
            formatTable.Columns.Add(title);

            DataColumn managerName = new DataColumn(DatabaseObjects.Columns.ManagerName, typeof(string));
            formatTable.Columns.Add(managerName);

            DataColumn resourceName = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(resourceName);
            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);

            DataColumn functionalAreaLookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionalAreaLookup);

            DataColumn ERPJobID = new DataColumn(DatabaseObjects.Columns.ERPJobID, typeof(string));
            formatTable.Columns.Add(ERPJobID);

            //Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);

            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateMonthlyFormatTableWithCategoryDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithCategoryDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithCategoryDrillDownWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateMonthlyFormatTableWithCategoryDrillDownWithExtraGroup(sourceTable, formatTable);
            }

            formatTable.DefaultView.Sort = "WorkItemType ASC, WorkItem ASC, SubItem ASC";
            return formatTable.DefaultView.ToTable();
        }
        private DataTable CreateMonthlyFormatTableForManager(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn ResourceManager = new DataColumn(DatabaseObjects.Columns.ManagerName, typeof(string));
            formatTable.Columns.Add(ResourceManager);

            DataColumn Resource = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(Resource);

            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);

            DataColumn functionalArealookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionalArealookup);

            DataColumn ERPJobID = new DataColumn(DatabaseObjects.Columns.ERPJobID, typeof(string));
            formatTable.Columns.Add(ERPJobID);

            Dictionary<int, double> workHoursList = new Dictionary<int, double>();
            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateMonthlyFormatTableForManagerWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableForManagerWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableForManagerWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateMonthlyFormatTableForManagerWithExtraGroup(sourceTable, formatTable);
            }

            return formatTable;
        }
        private void CreateTableSchema(List<string> columnColl, DataTable formatTable)
        {
            if (reportType == "AllocationsActuals")
            {
                if (columnList != null && columnList.Count > 0)
                {
                    foreach (string column in columnList)
                    {
                        DataColumn colName = new DataColumn(column, typeof(double));
                        formatTable.Columns.Add(colName);
                    }
                }

                if (columnList != null && columnList.Count > 0)
                {
                    foreach (string column in columnList)
                    {
                        DataColumn colName = new DataColumn("Alloc_" + column, typeof(double));
                        formatTable.Columns.Add(colName);

                        DataColumn colNameAct = new DataColumn("Act_" + column, typeof(double));
                        formatTable.Columns.Add(colNameAct);
                    }
                }
            }
            else if (columnList != null && columnList.Count > 0)
            {
                foreach (string column in columnList)
                {
                    DataColumn colName = new DataColumn(column, typeof(double));
                    formatTable.Columns.Add(colName);
                }
            }
        }
        private DataTable CreateMonthlyFormatTableWithPeopleDrillDown(DataTable sourceTable)
        {
            DataTable formatTable = new DataTable();

            DataColumn WorkItemType = new DataColumn("WorkItemType", typeof(string));
            formatTable.Columns.Add(WorkItemType);
            DataColumn Type = new DataColumn("WorkItem", typeof(string));
            formatTable.Columns.Add(Type);
            DataColumn SubType = new DataColumn("SubItem", typeof(string));
            formatTable.Columns.Add(SubType);
            DataColumn title = new DataColumn("Title", typeof(string));
            formatTable.Columns.Add(title);


            DataColumn resourceName = new DataColumn(DatabaseObjects.Columns.ResourceName, typeof(string));
            formatTable.Columns.Add(resourceName);
            DataColumn DepartmentNameLookup = new DataColumn(DatabaseObjects.Columns.DepartmentNameLookup, typeof(string));
            formatTable.Columns.Add(DepartmentNameLookup);
            DataColumn functionalLookup = new DataColumn(DatabaseObjects.Columns.FunctionalAreaTitleLookup, typeof(string));
            formatTable.Columns.Add(functionalLookup);

            DataColumn ERPJobID = new DataColumn(DatabaseObjects.Columns.ERPJobID, typeof(string));
            formatTable.Columns.Add(ERPJobID);

            //Set The value according to the defined constants for RMM module.
            WorkItemType.Expression = string.Format("IIF({0}='{4}','{1}', IIF({0}='{5}','{2}', IIF({0}='{6}','{3}', {0})))", "WorkItem",
                                                    RMMLevel1NPRProjects, RMMLevel1PMMProjects, RMMLevel1TSKProjects,
                                                    Constants.RMMLevel1NPRProjectsType, Constants.RMMLevel1PMMProjectsType, Constants.RMMLevel1TSKProjectsType);
            CreateTableSchema(columnList, formatTable);

            if (GroupByDepartment && GroupByFunctionalArea)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    var functionalAreaTitleLookup = departmentRows.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                    foreach (var fArea in functionalAreaTitleLookup)
                    {
                        DataRow[] functionalAreaRows = fArea.ToArray();
                        if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                            continue;
                        CreateMonthlyFormatTableWithPeopleDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, dPartment.Key, fArea.Key);
                    }
                }
            }
            else if (GroupByFunctionalArea)
            {
                var functionalAreaTitleLookup = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.FunctionalAreaTitleLookup));
                foreach (var fArea in functionalAreaTitleLookup)
                {
                    DataRow[] functionalAreaRows = fArea.ToArray();
                    if (functionalAreaRows == null || functionalAreaRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithPeopleDrillDownWithExtraGroup(functionalAreaRows.CopyToDataTable(), formatTable, functionalarea: fArea.Key);
                }
            }
            else if (GroupByDepartment)
            {
                var allocationByDepartment = sourceTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.DepartmentNameLookup));
                foreach (var dPartment in allocationByDepartment)
                {
                    DataRow[] departmentRows = dPartment.ToArray();
                    if (departmentRows == null || departmentRows.Length == 0)
                        continue;
                    CreateMonthlyFormatTableWithPeopleDrillDownWithExtraGroup(departmentRows.CopyToDataTable(), formatTable, department: dPartment.Key);
                }
            }
            else
            {
                CreateMonthlyFormatTableWithPeopleDrillDownWithExtraGroup(sourceTable, formatTable);
            }

            formatTable.DefaultView.Sort = "WorkItemType ASC, WorkItem ASC, SubItem ASC, ResourceNameUser ASC";
            return formatTable.DefaultView.ToTable();
        }
        private GridViewDataTextColumn GetGridColumn(string columnName, string caption)
        {
            GridViewDataTextColumn column = new GridViewDataTextColumn();
            column.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            column.FooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
            column.GroupFooterCellStyle.HorizontalAlign = HorizontalAlign.Center;
            column.GroupFooterCellStyle.Font.Bold = true;
            column.FooterCellStyle.Font.Bold = true;
            column.PropertiesTextEdit.EncodeHtml = true;
            column.FieldName = columnName;
            column.Caption = caption;
            column.Name = columnName;
            column.PropertiesTextEdit.DisplayFormatString = "{0:F2}";
            column.HeaderStyle.Font.Bold = true;
            column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            column.Width = new Unit(100);
            return column;
        }
        private void GetActualAllocationHours(string unitToShow, DataRow[] monthAllocRows, out double totalActualsHours, out double totalAllocationHour)
        {
            double totalActH = 0.0;
            double totalAllocH = 0.0;
            if (reportType == "AllocationsActuals")
            {

                if (!unitToShow.Equals("ftes"))
                {
                    totalActH = monthAllocRows.Sum(x => Convert.ToDouble(x[DatabaseObjects.Columns.ActualHour]));
                    totalAllocH = monthAllocRows.Sum(x => Convert.ToDouble(x[DatabaseObjects.Columns.AllocationHour]));
                }
                else
                {
                    totalActH = monthAllocRows.Sum(x => Convert.ToDouble(x[DatabaseObjects.Columns.PctActual])) / 100;
                    totalAllocH = monthAllocRows.Sum(x => Convert.ToDouble(x[DatabaseObjects.Columns.PctAllocation])) / 100;
                }
            }
            else if (reportType == "Actuals")
                totalAllocH = unitToShow.Equals("ftes") ? monthAllocRows.Sum(x => Convert.ToDouble(x[reportTypeColumnName])) / 100 : monthAllocRows.Sum(x => Convert.ToDouble(x[reportTypeColumnName]));
            else
                totalAllocH = unitToShow.Equals("hours") ? monthAllocRows.Sum(x => Convert.ToDouble(x[reportTypeColumnName])) : monthAllocRows.Sum(x => Convert.ToDouble(x[reportTypeColumnName])) / 100;

            totalActualsHours = totalActH;
            totalAllocationHour = totalAllocH;
        }
        private void CreateGridSummaryColumn(ASPxGridView gvResourceReport, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
            summary.ShowInGroupFooterColumn = column;

            summary.DisplayFormat = "{0:F2}";
            gvResourceReport.GroupSummary.Add(summary);

            ASPxSummaryItem totalSummary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);

            totalSummary.DisplayFormat = "{0:F2}";
            gvResourceReport.TotalSummary.Add(totalSummary);

        }

        private void ReOrderGroupingIndex(Dictionary<string, int> lstOfGroupedColumns)
        {
            if (lstOfGroupedColumns == null || lstOfGroupedColumns.Count == 0)
                return;

            if (GroupByFunctionalArea)
            {
                lstOfGroupedColumns.ToList().ForEach(x => { lstOfGroupedColumns[x.Key] += 1; });
                lstOfGroupedColumns.Add(DatabaseObjects.Columns.FunctionalAreaTitleLookup, 0);
            }

            if (GroupByDepartment)
            {
                lstOfGroupedColumns.ToList().ForEach(x => { lstOfGroupedColumns[x.Key] += 1; });
                lstOfGroupedColumns.Add(DatabaseObjects.Columns.DepartmentNameLookup, 0);
            }

            UpdateGroupIndex(lstOfGroupedColumns);
        }

        private void UpdateGroupIndex(Dictionary<string, int> lstOfGroupedColumns)
        {
            ReadOnlyCollection<GridViewDataColumn> groupedColumns = gvResourceReport.GetGroupedColumns();
            foreach (GridViewDataColumn currentCol in groupedColumns)
            {
                currentCol.GroupIndex = -1;
            }
            var orderedList = (from pair in lstOfGroupedColumns
                               orderby pair.Value ascending
                               select pair);

            foreach (var dic in orderedList)
            {

                GridViewDataColumn col = (GridViewDataColumn)gvResourceReport.Columns[dic.Key];
                if (col != null)
                {
                    col.GroupIndex = dic.Value;
                }

            }
        }

        private DataRow[] GetResourceTimeSheetForRange(DateTime dateFrom, DateTime dateTo)
        {

            DataTable dt = new DataTable();
            ResourceTimeSheetManager timesheetManager = new ResourceTimeSheetManager(AppContext);
            dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheetSignOff, $"{DatabaseObjects.Columns.TenantID}='{AppContext.TenantID}'");
            //SPQuery query = new SPQuery();
            //List<string> expression = new List<string>();
            //expression.Add(string.Format("<Geq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Geq>", DatabaseObjects.Columns.UGITStartDate, SPUtility.CreateISO8601DateTimeFromSystemDateTime(dateFrom.Date)));
            //expression.Add(string.Format("<Leq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Leq>", DatabaseObjects.Columns.UGITEndDate, SPUtility.CreateISO8601DateTimeFromSystemDateTime(dateTo.Date)));
            //expression.Add(string.Format("<Or><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{0}'/><Value Type='Text'>{2}</Value></Eq></Or>", DatabaseObjects.Columns.SignOffStatus, "Sign Off", "Approved"));

            //query.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(expression, expression.Count - 1, true));

            DataRow[] coll = dt.Select($"{DatabaseObjects.Columns.SignOffStatus} in ('Sign Off','Approved') And {DatabaseObjects.Columns.StartDate} >= '{dateFrom}' And {DatabaseObjects.Columns.EndDate} <= '{dateTo}' ");
            return coll;
            //if (coll == null || coll.Count == 0)
            //    return dt;

            //dt = coll.GetDataTable();
            //return dt;
        }
        private string getTicketTitle(string moduleName, string ticketId)
        {
            string _title = string.Empty;
            UGITModule module = ModuleManagerObj.GetByName(moduleName);
            DataRow datarow = TicketManagerObj.GetByTicketID(module, ticketId);
            if (datarow != null)
                _title = Convert.ToString(datarow[DatabaseObjects.Columns.Title]);

            return _title;
        }

        #endregion Member Functions
        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnActual_Click(object sender, EventArgs e)
        {
            if (btnActual.Text == "Show Actuals")
            {
                btnActual.Text = "Show Allocation";
            }
            else
            {
                btnActual.Text = "Show Actuals";
            }
        }

        protected void gvResourceReport_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ColorGroupRow(e, Color.FromArgb(227, 226, 228), 0);
            ColorGroupRow(e, Color.FromArgb(234, 234, 235), 1);
            ColorGroupRow(e, Color.FromArgb(234, 234, 235), 2);
            if (e.RowType == GridViewRowType.Footer)
            {
                if (e.Row.Cells.Count > 1)
                    e.Row.Cells[1].Text = "<b>Grand Total: </b>";
            }
        }

        private void ColorGroupRow(ASPxGridViewTableRowEventArgs e, Color backColor, int rowIndex)
        {
            if ((e.RowType == GridViewRowType.Group || e.RowType == GridViewRowType.GroupFooter) && gvResourceReport.GetRowLevel(e.VisibleIndex) == rowIndex)
            {
                e.Row.BackColor = backColor;
                if (e.RowType == GridViewRowType.GroupFooter)
                {
                    int index = e.Row.Cells.Count - gvResourceReport.DataColumns.Count + gvResourceReport.GroupCount;
                    e.Row.Cells[index].Text = string.Format("<b>{0} Total: </b>", gvResourceReport.GetGroupedColumns().FirstOrDefault(m => m.GroupIndex == rowIndex).Caption);
                }
            }
        }

        protected void gvResourceReport_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

        }

        protected void gvResourceReport_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {

        }

        protected void ASPxButton4_Click(object sender, EventArgs e)
        {
            DataTable ResultDataForCSV = GetReportData();
            if (startWith == "Work Category")
            {
                if (drillDownTo == "People")
                    ResultDataForCSV.DefaultView.Sort = "WorkItem ASC, SubItem ASC, ResourceNameUser ASC";
                else
                    ResultDataForCSV.DefaultView.Sort = "WorkItem ASC, SubItem ASC";
            }
            else
            {
                if (drillDownTo == "Category")
                    ResultDataForCSV.DefaultView.Sort = "ManagerName ASC, ResourceNameUser ASC, WorkItem ASC, SubItem ASC";
                else
                    ResultDataForCSV.DefaultView.Sort = "ManagerName ASC, ResourceNameUser ASC";
            }
            ResultDataForCSV = ResultDataForCSV.DefaultView.ToTable();

            string csvData = UGITUtility.ConvertTableToCSV(ResultDataForCSV);
            string attachment = string.Format("attachment; filename={0}.csv", "ResourceReportCSV");
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.Write(csvData.ToString());
            Response.Flush();
            Response.End();

            //gvResourceReportExporter.WriteCsvToResponse("ResourceReportCSV-" + DateTime.Now.ToString("MM-dd-yyyy"));
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.GroupHeadingSufix = "Total: ";
            reportHelper.GrandGroupHeading = "Grand Total: ";
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            DataTable ResultData = GetReportData();
            XtraReport report = reportHelper.GenerateReport(gvResourceReport, ResultData, lblReportTitle.Text, 8F, "xls");
            reportHelper.WriteXlsToResponse(Response, lblReportTitle.Text + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

//            gvResourceReportExporter.WriteXlsxToResponse("ResourceReportExcel-" + DateTime.Now.ToString("MM-dd-yyyy"));
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
        void reportHelper_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {

        }

        protected void ASPxButton2_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.GroupHeadingSufix = "Total: ";
            reportHelper.GrandGroupHeading = "Grand Total: ";
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            DataTable ResultData = GetReportData();
            XtraReport report = reportHelper.GenerateReport(gvResourceReport, ResultData, lblReportTitle.Text, 6.75F);
            reportHelper.WritePdfToResponse(Response, lblReportTitle.Text + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            //gvResourceReportExporter.WritePdfToResponse("ResourceReportPDF-" + DateTime.Now.ToString("MM-dd-yyyy"));
        }

        protected void ASPxButton3_Click(object sender, EventArgs e)
        {

        }

        protected void gvResourceReport_DataBinding(object sender, EventArgs e)
        {
            if (resultentTable == null || resultentTable.Rows.Count == 0)
                gvResourceReport.DataSource = GetReportData();// GetDataSource(); //();f
            else
                gvResourceReport.DataSource = resultentTable;
        }
        public string GetFieldByID(string value)
        {
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.FunctionalAreas, $"{DatabaseObjects.Columns.TenantID}='{AppContext.TenantID}' AND {DatabaseObjects.Columns.ID}='{value}' ");
            if (dt != null && dt.Rows.Count > 0)

                return dt.Rows[0]["Title"].ToString();
            else
            {
                return string.Empty;
            }
        }

        public DataTable GetReportData()
        {
            DataTable reportTable = new DataTable();
            DataTable ResourceUsageSummary = null;
            DataTable tempResourceTable = null;
            string categoryInternalNames = "";
            string yearStartDateStr = UGITUtility.ObjectToString(dateFrom);
            string yearEndDateStr = UGITUtility.ObjectToString(dateTo);
            Dictionary<string, object> values = new Dictionary<string, object>();

            string prms = string.Format("{0}='{1}' AND {2}='{3}' AND {4} IN ('{5}')", DatabaseObjects.Columns.EnableModule, 1,
                DatabaseObjects.Columns.TenantID, AppContext.TenantID,
                DatabaseObjects.Columns.Title, selectedCategory.Replace("#", "','").Replace("&amp;", "&"));
            var allCategories = selectedCategory.Split('#');
            DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, prms, 
                DatabaseObjects.Columns.Title + "," + DatabaseObjects.Columns.ModuleName, null);
            if (dtModules.Rows.Count > 0)
            {
                var modulelist = dtModules.AsEnumerable().Select(r => r[DatabaseObjects.Columns.ModuleName].ToString());
                var moduleTitlelist = dtModules.AsEnumerable().Select(r => r[DatabaseObjects.Columns.Title].ToString());
                var addOnOtherCategories = allCategories.Except(moduleTitlelist);
                allCategories = modulelist.Concat(addOnOtherCategories).ToArray();
                categoryInternalNames = string.Join(",", allCategories);
            }
            else
                categoryInternalNames = string.Join(",", allCategories);

            // Set the column for report type as "Allocation" or "Actual".
            if (reportType == "Actuals" && (string.IsNullOrEmpty(unitAllocAct) || unitAllocAct.Equals("hours")))
                reportTypeColumnName = DatabaseObjects.Columns.ActualHour;
            else if (reportType == "Actuals")
                reportTypeColumnName = DatabaseObjects.Columns.PctActual;
            else if (reportType != "AllocationsActuals" && (string.IsNullOrEmpty(unitAllocAct) || unitAllocAct.Equals("ftes")))
                reportTypeColumnName = DatabaseObjects.Columns.PctAllocation;
            else
                reportTypeColumnName = DatabaseObjects.Columns.AllocationHour;

            values.Add("@TenantID", AppContext.TenantID);
            values.Add("@WorkItemType", categoryInternalNames);
            if (showFunctionalArea)
                values.Add("@FunctionalArea", functionalArea);
            values.Add("@ReportType", reportType);
            values.Add("@ViewType", viewType);
            if (!string.IsNullOrEmpty(department))
                values.Add("@Department", department.Remove(department.Length - 1));
            values.Add("@unitAllocAct", unitAllocAct);
            values.Add("@ManagerID", manager);
         
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
            ResourceUsageSummary = GetTableDataManager.GetData("ResourceSummaryReportData", values);


            if (ResourceUsageSummary != null)
            {
                if (ResourceUsageSummary.Rows.Count > 0 && !string.IsNullOrEmpty(reportType))
                {
                    DataRow[] excludeZeroHours = null;
                    switch (reportType)
                    {
                        case "Allocation":
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AllocationHour, ResourceUsageSummary))
                                excludeZeroHours = ResourceUsageSummary.Select($"{DatabaseObjects.Columns.AllocationHour} > 0");// resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.AllocationHour) > 0).ToArray();
                            break;
                        case "Actuals":
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ActualHour, ResourceUsageSummary))
                                excludeZeroHours = ResourceUsageSummary.Select($"{DatabaseObjects.Columns.ActualHour} > 0");   // resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.ActualHour) > 0).ToArray();
                            break;
                        case "AllocationsActuals":
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AllocationHour, ResourceUsageSummary) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ActualHour, ResourceUsageSummary))
                                excludeZeroHours = ResourceUsageSummary.Select($"{DatabaseObjects.Columns.AllocationHour} > 0 OR {DatabaseObjects.Columns.ActualHour} > 0");  //resourceSummaryTable.AsEnumerable().Where(x => x.Field<double>(DatabaseObjects.Columns.AllocationHour) > 0 || x.Field<double>(DatabaseObjects.Columns.ActualHour) > 0).ToArray();
                            break;
                        default:
                            break;

                    }

                    if (excludeZeroHours == null || excludeZeroHours.Length == 0)
                        tempResourceTable = ResourceUsageSummary.Clone();
                    else
                    {
                        tempResourceTable = excludeZeroHours.CopyToDataTable();
                        tempResourceTable.DefaultView.Sort = dateFieldColumnName;
                        tempResourceTable = tempResourceTable.DefaultView.ToTable();
                    }
                }
                else
                {
                    tempResourceTable = ResourceUsageSummary.Clone();
                }



                //DataTable tempResourceTable = ResourceUsageSummary; //.Clone()

                //if (ResourceUsageSummary.Rows.Count > 0)


                var StartDays = tempResourceTable.AsEnumerable().ToLookup(x => x.Field<DateTime>(dateFieldColumnName));

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
                columnList = columnList.Distinct().ToList();

                if (viewType == "Weekly")
                {
                    string timesheetMode = ConfigVariableManagerObj.GetValue(ConfigConstants.TimesheetMode); // uGITCache.GetConfigVariableValue(ConfigConstants.TimesheetMode, SPContext.Current.Web);
                    if ((timesheetMode.ToLower() == Constants.SignOffMode.ToLower() || timesheetMode.ToLower() == Constants.ApprovalMode.ToLower()) && UGITUtility.StringToBoolean(Request["ApprovedHoursOnly"]))
                    {
                        DataRow[] itemColl = GetResourceTimeSheetForRange(dateFrom, dateTo);
                        if (itemColl != null && itemColl.Count() > 0)
                        {

                            List<string> approvedSheetUsers = itemColl.CopyToDataTable().AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Resource])).Distinct().ToList();
                            if (approvedSheetUsers != null && approvedSheetUsers.Count > 0)
                            {
                                DataRow[] existUsersData = tempResourceTable.AsEnumerable().Where(x => approvedSheetUsers.Exists(y => y == Convert.ToString(x[DatabaseObjects.Columns.Resource]))).ToArray();
                                if (existUsersData != null && existUsersData.Length > 0)
                                    tempResourceTable = existUsersData.CopyToDataTable();
                                else
                                    tempResourceTable = tempResourceTable.Clone();
                            }

                            var signOffStartDays = itemColl.CopyToDataTable().AsEnumerable().ToLookup(x => x.Field<DateTime>(DatabaseObjects.Columns.UGITStartDate));

                            if (columnList != null && columnList.Count > 0)
                                columnList.Clear();
                            foreach (var startday in signOffStartDays)
                            {
                                columnList.Add(UGITUtility.GetDateStringInFormat(Convert.ToDateTime(startday.Key), false));
                            }

                            columnList = columnList.Distinct().ToList();

                        }
                        else
                            tempResourceTable = tempResourceTable.Clone();
                    }

                    if (startWith == "Work Category")
                    {
                        if (drillDownTo == "People")
                            reportTable = CreateWeeklyFormatTableWithPeopleDrilldown(tempResourceTable);
                        else
                            reportTable = CreateWeeklyFormatTable(tempResourceTable);
                    }
                    else
                    {
                        if (drillDownTo == "Category")
                            reportTable = CreateWeeklyFormatTableWithCategoryDrillDown(tempResourceTable);
                        else
                            reportTable = CreateWeeklyFormatTableForManager(tempResourceTable);
                    }
                }
                else
                {
                    if (startWith == "Work Category")
                    {
                        if (drillDownTo == "People")
                            reportTable = CreateMonthlyFormatTableWithPeopleDrillDown(tempResourceTable);
                        else
                            reportTable = CreateMonthlyFormatTable(tempResourceTable);
                    }
                    else
                    {
                        if (drillDownTo == "Category")
                            reportTable = CreateMonthlyFormatTableWithCategoryDrillDown(tempResourceTable);
                        else
                            reportTable = CreateMonthlyFormatTableForManager(tempResourceTable);
                    }
                }
            }
            GenerateColumns();
            resultentTable = reportTable;
            return reportTable;

        }

    }

    public class CustomFooterRowTemplate : ITemplate
    {
        public string Text { get; set; }
        public CustomFooterRowTemplate(string text)
        {
            this.Text = text;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            Label lblTotal = new Label();
            lblTotal.Text = Text;
            lblTotal.Font.Bold = true;
            container.Controls.Add(lblTotal);
        }
    }
}