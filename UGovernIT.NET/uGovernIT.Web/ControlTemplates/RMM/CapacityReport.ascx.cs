using DevExpress.Data;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using DevExpress.XtraGrid;
using DevExPrinting = DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using uGovernIT.Util.Log;
using System.Web.WebPages;
using DevExpress.XtraPrinting;
using DevExpress.Charts.Native;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Text.RegularExpressions;

namespace uGovernIT.Web
{
    public partial class CapacityReport : System.Web.UI.UserControl
    {
        #region variables
        public string ControlId { get; set; }
        public string TicketId { get; set; }
        public string FrameId;
        public bool ReadOnly;
        private string peopleGlobalRoleID;
        double green = 80, lightgreen = 15, yellow = 99, red = 100, gray = 0, orange = 120;
        private string formTitle = "Resource Utilization";
        private string editParam = "CustomResourceAllocation";
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        string varUseCalendar = string.Empty;
        DataTable resultTable;
        public string ResourceCapacity { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        List<string> sActionType = new List<string>();
        protected DataTable resultedTable;
        DataSet ds = new DataSet();

        protected bool isResourceAdmin = false;
        private bool allowAllocationForSelf;
        private bool allowAllocationViewAll;
        protected List<UserProfile> userEditPermisionList = null;
        public DateTime currentStartDate;
        public DateTime currentEndDate;
        protected List<ResourceAllocationMonthly> dtMonthlyAllocation;
        protected DataTable dtRawDataAllResource;
        protected List<ResourceWorkItems> allResourceWorkItemsSPList;
        List<ResourceAllocationMonthly> currentSelectedRows = new List<ResourceAllocationMonthly>();
        string currenSelectedUserID = string.Empty;

        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        ResourceAllocationManager ObjResourceAllocationManager = null;
        ResourceAllocationMonthlyManager allcMManager = null;
        ResourceWorkItemsManager workitemManager = null;

        UserProfileManager profileManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        JobTitleManager jobTitleManager = null;
        GlobalRoleManager roleManager = null;

        JobTitle jobTitle = null;
        //GlobalRole role = null;
        UserProfile userP = null;
        private List<UserProfile> userProfiles = null;
        private List<GlobalRole> userRoles = null;

        List<UserProfile> filteredUserProfiles = null;
        List<string> selectedDepartments = null;

        DateTime dateFrom;
        DateTime dateTo;

        public bool AllowGroupFilterOnExpTags
        {
            get
            {
                return uHelper.IsExperienceTagAllowGroupFilter(applicationContext);
            }
        }
        #endregion

        #region pageEvents
        protected override void OnInit(EventArgs e)
        {
            dateFrom = new DateTime(UGITUtility.StringToInt(DateTime.Today.Year), 1, 1);
            dateTo = dateFrom.AddMonths(12);

            ObjResourceAllocationManager = new ResourceAllocationManager(applicationContext);
            allcMManager = new ResourceAllocationMonthlyManager(applicationContext);
            workitemManager = new ResourceWorkItemsManager(applicationContext);

            profileManager = new UserProfileManager(applicationContext);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(applicationContext);
            varUseCalendar = ObjConfigurationVariableManager.GetValue(DatabaseObjects.Columns.UseCalendar);
            jobTitleManager = new JobTitleManager(applicationContext);
            roleManager = new GlobalRoleManager(applicationContext);

            EditPermisionList();
            FillDropDownType();

            if (allResourceWorkItemsSPList == null)
            {
                allResourceWorkItemsSPList = workitemManager.Load();
            }

            userProfiles = profileManager.GetUsersProfile();

            userRoles = roleManager.Load();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedItem.Value == "UnfilledRoles")
            {
                userProfiles.Clear();
                DateTime dtfrom = Convert.ToDateTime(hdndtfrom.Value);
                DateTime dtto = Convert.ToDateTime(hdndtto.Value);
                var blankResources = ObjResourceAllocationManager.Load(x => x.Resource.Equals(Guid.Empty.ToString()) && x.AllocationStartDate.Value.Year <= dtfrom.Year && x.AllocationEndDate.Value.Year >= dtto.Year).Select(x => x.RoleId).Distinct().ToList();
                foreach (var role in blankResources)
                {
                    UserProfile user = new UserProfile() { Id = Guid.Empty.ToString(), Name = "Unfilled Roles", UserName = "UnfilledRoles", GlobalRoleId = role, isRole = false, Enabled = true, TenantID = applicationContext.TenantID };
                    userProfiles.Add(user);
                }
            }

            UserProfile currentUserProfile = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id)); //HttpContext.Current.CurrentUser();
            if (Request["AllocationViewType"] == "RMMAllocation")
            {
                divAllocation.Visible = true;   //temporally false for all view types
            }
            else
            {
                divAllocation.Visible = false;
            }

            ResourceCapacity = Request["IsResourceDrillDown"];

            bool EnableRMMAssignment = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableRMMAssignment);
            if (!EnableRMMAssignment)
                rbtnAssigned.Visible = false;

            #region color setting
            string strResourceAllocationColor = ObjConfigurationVariableManager.GetValue(ConfigConstants.ResourceAllocationColor);
            if (!string.IsNullOrEmpty(strResourceAllocationColor))
            {
                Dictionary<string, string> cpResourceAllocationColor = UGITUtility.GetCustomProperties(strResourceAllocationColor, Constants.Separator);

                if (cpResourceAllocationColor.ContainsKey(Constants.Green))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.Green], out green);
                }
                if (cpResourceAllocationColor.ContainsKey(Constants.LightGreen))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.LightGreen], out lightgreen);
                }
                if (cpResourceAllocationColor.ContainsKey(Constants.Yellow))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.Yellow], out yellow);
                }
                if (cpResourceAllocationColor.ContainsKey(Constants.Red))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.Red], out red);
                }

                if (cpResourceAllocationColor.ContainsKey(Constants.Gray))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.Gray], out gray);
                }
                if (cpResourceAllocationColor.ContainsKey(Constants.Orange))
                {
                    Double.TryParse(cpResourceAllocationColor[Constants.Orange], out orange);
                }
            }
            #endregion

            DataTable dataTable = (DataTable)glType.DataSource;
            //List<string> sActionUrs = new List<string>();

            List<string> selectedActionType = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string sau in selectedActionType)
            {
                //DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Role") == sau);
                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sau);
                if (row != null)
                    sActionType.Add(Convert.ToString(row["LevelName"]));
                else
                    sActionType.Add(sau);
            }

            string ControlName = uHelper.GetPostBackControlId(this.Page);

            if (ControlName == "gv")
                UGITUtility.CreateCookie(Response, "filter", string.Format("type${0}", glType.Text));

            if (ControlName == "ddlDepartment")
            {
                UGITUtility.CreateCookie(Response, "filterDepartment", string.Format("department${0}", ddlDepartment.GetValues()));
                UGITUtility.CreateCookie(Response, "filterFunctionArea", string.Format("functionarea${0}", "0"));
            }
            else if (ControlName == "ddlUserGroup")
            {
                // UGITUtility.CreateCookie(Response, "filter", string.Format("group${0}~#type${1}", ddlUserGroup.SelectedValue, selectedCategory));
            }
            if (ControlName == "ddlFunctionalArea")
            {
                UGITUtility.CreateCookie(Response, "filterFunctionArea", string.Format("functionarea${0}", ddlFunctionalArea.SelectedValue));
            }
            if (ControlName == "ddlResourceManager")
            {
                UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", ddlResourceManager.SelectedValue));
            }
            if (!Page.IsPostBack)
            {
                if (currentUserProfile != null && !string.IsNullOrEmpty(currentUserProfile.Department))
                {
                    //ddlDepartment.Value = currentUserProfile.Department;
                    hdnaspDepartment.Value = currentUserProfile.Department;
                    ddlDepartment.SetValues(currentUserProfile.Department);
                    LoadGlobalRolesOnDepartment(currentUserProfile.Department);
                    LoadDdlResourceManager(currentUserProfile.Department, "");
                }
                else
                {
                    hdnaspDepartment.Value = "";
                    LoadGlobalRolesOnDepartment("");
                    LoadDdlResourceManager();
                }
            }
            if (!IsPostBack)
            {
                if (UGITUtility.ObjectToString(Request["AllocationViewType"]) == "ProjectAllocation")
                {
                    divProject.Visible = true;
                    if (!string.IsNullOrEmpty(Request["pStartDate"]) && !string.IsNullOrEmpty(Request["pEndDate"]))
                    {
                        hdndtfrom.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pStartDate"]).Year, Convert.ToDateTime(Request["pStartDate"]).Month, 1));
                        hdndtto.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month, System.DateTime.DaysInMonth(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month)));
                    }
                    else
                    {
                        hdndtfrom.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 1, 1));
                        hdndtto.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 12, 31));
                    }
                    divFilter.Visible = false;
                    chkAll.Visible = false;
                    //chkItemCount.Visible = false;
                    rbtnItemCount.Visible = false;

                }
                else if (UGITUtility.ObjectToString(Request["AllocationViewType"]) == "CRMProjectAuto")
                {
                    divProject.Visible = false;

                    if (Request["pStartDate"] != null)
                        hdndtfrom.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pStartDate"]).Year, Convert.ToDateTime(Request["pStartDate"]).Month, 1));
                    if (Request["pEndDate"] != null)
                        hdndtto.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month, System.DateTime.DaysInMonth(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month)));
                    // hdndtto.Value = Request["pEndDate"];

                    peopleGlobalRoleID = Request["pGlobalRoleID"];
                    divFilter.Visible = false;
                }
                else if (UGITUtility.ObjectToString(Request["IsResourceDrillDown"]) == "true")
                {
                    divProject.Visible = false;

                    if (Request["pStartDate"] != null)
                        hdndtfrom.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pStartDate"]).Year, Convert.ToDateTime(Request["pStartDate"]).Month, 1));
                    if (Request["pEndDate"] != null)
                        hdndtto.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month, System.DateTime.DaysInMonth(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month)));
                    // hdndtto.Value = Request["pEndDate"];

                    peopleGlobalRoleID = Request["pGlobalRoleID"];
                    divFilter.Attributes.Add("style", "display:none");
                    chkAll.Checked = true;

                    divCheckbox.Visible = false;
                    divProject.Visible = false;
                    tdManager.Visible = false;
                    tdManagerDropDown.Visible = false;
                    tdType.Visible = false;
                    tdTypeGridLookup.Visible = false;

                    hdndisplayMode.Value = DisplayMode.Monthly.ToString();

                    //LoadDepartment();
                    //DdlUserGroups();
                    LoadFunctionalArea();
                }
                else
                {

                    divProject.Visible = false;
                    divFilter.Visible = true;
                    string defaultDisplayMode = ObjConfigurationVariableManager.GetValue(ConfigConstants.DefaultRMMDisplayMode);
                    if (defaultDisplayMode == "Monthly" || Request["AllocationViewType"] == "CapacityReport")
                    {

                        hdndtfrom.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 1, 1));
                        hdndtto.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 12, 31));
                        hdndisplayMode.Value = DisplayMode.Monthly.ToString();
                    }
                    else
                    {
                        DateTime currentDate = DateTime.Now;

                        // First day of the month before the current month
                        DateTime firstDayOfPreviousMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                        // Last day of the month after the current month
                        DateTime lastDayOfNextMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(3).AddDays(-1);
                        // Find the first Monday by iterating through the days of the week
                        // and checking if it's Monday
                        DateTime firstMonday = firstDayOfPreviousMonth;
                        while (firstMonday.DayOfWeek != DayOfWeek.Monday)
                        {
                            firstMonday = firstMonday.AddDays(1);
                        }
                        hdndtfrom.Value = firstMonday.ToShortDateString();
                        hdndtto.Value = lastDayOfNextMonth.ToShortDateString();
                        hdndisplayMode.Value = DisplayMode.Weekly.ToString();
                        hdnSelectedDate.Value = firstMonday.ToShortDateString();
                    }
                }

                //hdndisplayMode.Value = DisplayMode.Monthly.ToString();
                //LoadDepartment();
                //LoadFunctionalArea();
                //              LoadDdlResourceManager();

                if (UGITUtility.ObjectToString(Request["IsResourceDrillDown"]) == "true")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["pDepartmentName"])))
                    {
                        ddlDepartment.SetValues(Convert.ToString(Request["pDepartmentName"]));
                        hdnaspDepartment.Value = Convert.ToString(Request["pDepartmentName"]);
                    }
                    
                    LoadFunctionalArea();
                }
                else
                {
                    #region cookies

                    string afilter = UGITUtility.GetCookieValue(Request, "filter");
                    string afilterResource = UGITUtility.GetCookieValue(Request, "filterResource");
                    string afilterFunctionalArea = UGITUtility.GetCookieValue(Request, "filterFunctionArea");
                    string afilterDepartment = UGITUtility.GetCookieValue(Request, "filterDepartment");


                    if (!string.IsNullOrEmpty(afilter))
                    {
                        string[] TypeVals = afilter.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);

                        sActionType.Clear();
                        //List<string> sActionUrs = new List<string>();
                        if (TypeVals.Length > 1)
                        {
                            List<string> cookieselectedTypes = TypeVals[1].Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (string sau in cookieselectedTypes)
                            {
                                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelName") == sau);
                                if (row != null)
                                {
                                    sActionType.Add(Convert.ToString(row["LevelName"]));
                                }
                                else
                                {
                                    sActionType.Add(sau);
                                }
                            }
                        }

                        glType.Text = string.Join("; ", sActionType.ToArray());
                    }

                    if (!string.IsNullOrEmpty(afilterResource))
                    {
                        string[] Vals = afilterResource.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlResourceManager.SelectedValue = Vals[1];
                        }
                    }

                    if (!string.IsNullOrEmpty(afilterDepartment))
                    {
                        string[] Vals = afilterDepartment.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlDepartment.SetValues(Vals[1]);
                        }
                    }
                    else
                    {
                        //if (currentUserProfile != null && Convert.ToInt32(currentUserProfile.Department) > 0)
                        if (currentUserProfile != null && UGITUtility.StringToInt(currentUserProfile.Department) > 0)
                        {
                            ddlDepartment.SetValues(currentUserProfile.Department);
                            ddlDepartment.Value = currentUserProfile.Department;
                        }
                    }

                    LoadFunctionalArea();

                    if (!string.IsNullOrEmpty(afilterFunctionalArea))
                    {
                        string[] Vals = afilterFunctionalArea.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Vals.Count() > 0)
                        {
                            ddlFunctionalArea.SelectedValue = Vals[1];
                        }
                    }

                    #endregion
                }

                if(UGITUtility.ObjectToString(Request["IsChartDrillDown"]) == "true")
                {
                    hdnaspDepartment.Value = "";
                }

                    string filterCountPercentageFTE = UGITUtility.GetCookieValue(Request, "filtercountpercentagefte");
                if (string.IsNullOrWhiteSpace(filterCountPercentageFTE) && Request["AllocationViewType"] == "ProjectAllocation")
                {
                    filterCountPercentageFTE = "percentage";
                }

                if (filterCountPercentageFTE == "fte")
                    rbtnFTE.Checked = true;
                else if (filterCountPercentageFTE == "percentage")
                    rbtnPercentage.Checked = true;
                else if (filterCountPercentageFTE == "availability")
                    rbtnAvailability.Checked = true;
                else if (filterCountPercentageFTE == "hrs")
                    rbtnHrs.Checked = true;
                else
                    rbtnItemCount.Checked = true;
                

                string allfilter = UGITUtility.GetCookieValue(Request, "filterAll");

                if (allfilter == "all")
                {
                    chkAll.Checked = true;
                    //isFilterApplied = true;
                }
                else
                {
                    chkAll.Checked = false;
                }

                string closedfilter = UGITUtility.GetCookieValue(Request, "IncludeClosed");

                if (closedfilter == "true")
                {
                    chkIncludeClosed.Checked = true;
                }
                else
                {
                    chkIncludeClosed.Checked = false;
                }
                //if (!isFilterApplied)
                //{
                //    chkColor.Checked = true;
                //    chkItemCount.Checked = true;
                //}
            }


            hdnTypeLoader.Value = glType.Text;
            //}
            if (!IsPostBack)
            {
                PrepareAllocationGrid();
            }
            if (Request["IsResourceDrillDown"] == "true")
                chkAll.Checked = true;

            selectedDepartments = new List<string>();
        }

        private void EditPermisionList()
        {
            isResourceAdmin = profileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || profileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            allowAllocationForSelf = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
            allowAllocationViewAll = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);

            if (!isResourceAdmin)
                userEditPermisionList = profileManager.LoadAuthorizedUsers(allowAllocationForSelf);
        }

        protected override void OnPreRender(EventArgs e)
        {
            gvCapacityReport.DataBind();
            base.OnPreRender(e);
        }

        #endregion
        
        private DataTable GetAllocationData(bool isExport = false)
        {          
            //check it to show assigned one or estimated one
            //bool IsAssignedallocation = false;
            DataTable data = new DataTable();

            string type = string.Empty;
            string AllocationType = string.Empty;

            //if (Request["AllocationViewType"] == "RMMAllocation")
            //{
            //    if (rbtnAssigned.Checked)
            //        IsAssignedallocation = true;
            //    else
            //        IsAssignedallocation = false;
            //}


            // Code to check for empty hidden values, when switching between #, % ,FTE, Availability radio buttons & assigning values from cookies
            if (hdndisplayMode.Value == "")
            {
                hdndisplayMode.Value = UGITUtility.GetCookieValue(Request, "RAdisplayMode");
                if (string.IsNullOrEmpty(hdndisplayMode.Value))
                    hdndisplayMode.Value = DisplayMode.Monthly.ToString();
            }
            if (hdndtfrom.Value == "")
            {
                hdndtfrom.Value = UGITUtility.GetCookieValue(Request, "RAdtfrom");
                if (string.IsNullOrEmpty(hdndtfrom.Value))
                    hdndtfrom.Value = new DateTime(DateTime.Now.Year, 1, 1).ToString();
            }
            if (hdndtto.Value == "")
            {
                hdndtto.Value = UGITUtility.GetCookieValue(Request, "RAdtto");
                if (string.IsNullOrEmpty(hdndtto.Value))
                    hdndtto.Value = new DateTime(DateTime.Now.Year, 12, 31).ToString();
            }
            UGITUtility.CreateCookie(Response, "RAdisplayMode", hdndisplayMode.Value);
            UGITUtility.CreateCookie(Response, "RAdtfrom", hdndtfrom.Value);
            UGITUtility.CreateCookie(Response, "RAdtto", hdndtto.Value);


            if (rbtnFTE.Checked)
                type = "FTE";
            else if (rbtnPercentage.Checked)
                type = "PERCENT";
            else if (rbtnAvailability.Checked)
                type = "AVAILABILITY";
            else if (rbtnHrs.Checked)
                type = "HOURS";
            else
                type = "COUNT";

            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", applicationContext.TenantID);
            values.Add("@Mode", hdndisplayMode.Value);
            values.Add("@Fromdate", UGITUtility.ChangeDateFormat(Convert.ToDateTime(hdndtfrom.Value)));
            values.Add("@Todate", UGITUtility.ChangeDateFormat(Convert.ToDateTime(hdndtto.Value)));
            values.Add("@Department", hdnaspDepartment.Value == "undefined" ? string.Empty : hdnaspDepartment.Value.TrimEnd(','));
            //values.Add("@ResourceManager", ddlResourceManager.SelectedValue);
            values.Add("@type", type);
            values.Add("@IncludeAllResources", chkAll.Checked);
            //values.Add("@IsAssignedallocation", IsAssignedallocation);
            //values.Add("@AllocationType", AllocationType);
            values.Add("@LevelName", string.Join(",", sActionType));
            values.Add("@Category", ddlCategory.SelectedItem.Value);
            values.Add("@Closed", chkIncludeClosed.Checked);
            values.Add("@url", UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx"));
            values.Add("@Filter", UGITUtility.ObjectToString(Request["Filter"]));
            values.Add("@Studio", UGITUtility.ObjectToString(Request["Studio"]));
            values.Add("@Division", UGITUtility.StringToLong(Request["Division"]));
            values.Add("@Sector", UGITUtility.ObjectToString(Request["Sector"]));
            values.Add("@SoftAllocation", chkIncludeSoftAllocation.Checked);
            values.Add("@OnlyNCO", chkOnlyNCOs.Checked);
            if (hdndisplayMode.Value == "Monthly")
            {
                if (isExport == false)                
                    ds = GetTableDataManager.GetDataSet("CapacityReport", values);
                else
                    ds = GetTableDataManager.GetDataSet("CapacityReportForExport", values);

                if (ds.Tables.Count > 0)
                    data = ds.Tables[0];                
            }
            else
            {
                ds = GetTableDataManager.GetDataSet("CapacityReportWeekly", values);
                if (ds.Tables.Count > 0)
                    data = ds.Tables[0];
            }

            return data;
        }

        private void PrepareAllocationGrid()
        {
            gvCapacityReport.Columns.Clear();
            gvCapacityReport.TotalSummary.Clear();
            if (gvCapacityReport.Columns.Count <= 0)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.Caption = "#";
                colId.FieldName = DatabaseObjects.Columns.ItemOrder;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.HeaderStyle.Font.Bold = true;
                colId.Width = new Unit("50px");
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.SortOrder = ColumnSortOrder.Ascending;
                gvCapacityReport.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Id;
                colId.Caption = DatabaseObjects.Columns.Id;
                colId.Visible = false;
                colId.VisibleIndex = -1;
                gvCapacityReport.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Resource;
                colId.Caption = DatabaseObjects.Columns.RResource; //DatabaseObjects.Columns.Resource;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                //
                if (ResourceCapacity != "true")//Request["ResourceCapacity"] == "true")
                {
                    if (Request["pStartDate"] == null && Request["pEndDate"] == null)
                        colId.DataItemTemplate = new HoverMenuDataTemplate();
                }
                colId.Width = new Unit("200px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.SortOrder = ColumnSortOrder.Ascending;
                colId.Visible = false;
                gvCapacityReport.Columns.Add(colId);

                //jobtitle
                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.JobTitle;
                colId.Caption = "Job Title"; //DatabaseObjects.Columns.Resource;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Width = new Unit("200px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //colId.SortOrder = ColumnSortOrder.Ascending;

                if (ddlCategory.SelectedItem.Value == "JobTitle")
                    colId.Visible = true;
                else
                    colId.Visible = false;

                gvCapacityReport.Columns.Add(colId);
                
                //Role
                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Role;
                colId.Caption = DatabaseObjects.Columns.Role; 
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Width = new Unit("200px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.SortOrder = ColumnSortOrder.Ascending;
                if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                    colId.Visible = true;
                else
                    colId.Visible = false;

                gvCapacityReport.Columns.Add(colId);

                //no of resource
                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ResourceQuantity;
                colId.Caption = "# Resources"; //DatabaseObjects.Columns.Resource;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;                
                colId.Width = new Unit("40px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "JobTitle")
                    colId.Visible = true;
                else
                    colId.Visible = false;
                gvCapacityReport.Columns.Add(colId);
                
                gvCapacityReport.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.JobTitle);
                ASPxSummaryItem item = new ASPxSummaryItem(DatabaseObjects.Columns.JobTitle, SummaryItemType.Custom);
                item.Tag = "ResourceItem";
                gvCapacityReport.TotalSummary.Add(item);

                gvCapacityReport.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.Role);
                ASPxSummaryItem roleItem = new ASPxSummaryItem(DatabaseObjects.Columns.Role, SummaryItemType.Custom);
                roleItem.Tag = "RoleItem";
                gvCapacityReport.TotalSummary.Add(roleItem);

                gvCapacityReport.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.ResourceQuantity);
                ASPxSummaryItem itemResourceCount = new ASPxSummaryItem(DatabaseObjects.Columns.ResourceQuantity, SummaryItemType.Custom);
                itemResourceCount.Tag = "ResourceCount";
                gvCapacityReport.TotalSummary.Add(itemResourceCount);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ProjectCapacity;

                if (chkIncludeClosed.Checked == false)
                    colId.Caption = "# Active";
                else
                    colId.Caption = "# Lifetime";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("40px");
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "JobTitle")
                    colId.Visible = true;
                else
                    colId.Visible = false;
                gvCapacityReport.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.RevenueCapacity;
                if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "JobTitle")
                    colId.Visible = true;
                else
                    colId.Visible = false;
                if (chkIncludeClosed.Checked == false)
                    colId.Caption = "$ Active";
                else
                    colId.Caption = "$ Lifetime";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("40px");
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                gvCapacityReport.Columns.Add(colId);

                GridViewBandColumn bdCol = new GridViewBandColumn();
                string currentDate = string.Empty;
                for (DateTime dt = Convert.ToDateTime(hdndtfrom.Value); Convert.ToDateTime(hdndtto.Value) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    int weeks = Convert.ToInt32((Convert.ToDateTime(hdndtto.Value) - Convert.ToDateTime(hdndtfrom.Value)).TotalDays / 7);
                    //if (hdndisplayMode.Value == "Daily")
                    //{
                    //    if (FirstDayOfWeek(dt).ToString("MMM-dd-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                    //    {
                    //        gvCapacityReport.Columns.Add(bdCol);
                    //        bdCol = new GridViewBandColumn();
                    //    }
                    //    if (FirstDayOfWeek(dt).ToString("MMM-dd-yy") != currentDate)
                    //    {
                    //        if (FirstDayOfWeek(dt).Month == dt.Month)
                    //        {
                    //            bdCol.Caption = FirstDayOfWeek(dt).ToString("MMM-dd-yy");
                    //            currentDate = FirstDayOfWeek(dt).ToString("MMM-dd-yy");
                    //        }
                    //        else
                    //        {
                    //            bdCol.Caption = dt.ToString("MMM-dd-yy");
                    //            currentDate = dt.ToString("MMM-dd-yy");
                    //        }
                    //        bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    //        bdCol.HeaderStyle.Font.Bold = true;
                    //        //if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                    //        //{
                    //        //    bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                    //        //}
                    //    }

                    //}
                    if (hdndisplayMode.Value == "Weekly")
                    {
                        if (dt.ToString("MMM-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                        {
                            gvCapacityReport.Columns.Add(bdCol);
                            bdCol = new GridViewBandColumn();
                        }

                        if (dt.ToString("MMM-yy") != currentDate)
                        {
                            bdCol.Caption = dt.ToString("MMM-yy");
                            bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bdCol.HeaderStyle.Font.Bold = true;
                            if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                            {
                                bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                            }
                            currentDate = dt.ToString("MMM-yy");
                        }
                    }
                    else
                    {
                        if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                        {
                            gvCapacityReport.Columns.Add(bdCol);
                            bdCol = new GridViewBandColumn();
                        }

                        if (dt.ToString("yyyy") != currentDate)
                        {
                            bdCol.Caption = dt.ToString("yyyy");
                            //SetCookie("year", dt.ToString("yyyy"));
                            bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bdCol.HeaderStyle.Font.Bold = true;
                            if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                            {
                                bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                            }
                            currentDate = dt.ToString("yyyy");
                        }
                    }

                    colId = new GridViewDataTextColumn();
                    if (hdndisplayMode.Value == "Monthly")
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("MMM");
                        if (ResourceCapacity != "true")//Request["ResourceCapacity"] == "true")
                        {
                            if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                            {
                                colId.HeaderTemplate = new CommandColumnHeaderTemplate(colId);
                            }
                        }
                    }
                    else if (hdndisplayMode.Value == "Weekly")
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("MMM-dd");
                        if (ResourceCapacity != "true")//Request["ResourceCapacity"] == "true")
                        {
                            if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                            {
                                //colId.HeaderTemplate = new CommandColumnHeaderTemplate(colId);
                            }
                        }
                        //if (weeks == 4)
                        //{
                        //    colId.ExportWidth = 180;
                        //}
                        //else
                        //{
                        //    colId.ExportWidth = 80;
                        //}
                    }
                    else
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("dd");
                    }

                    colId.UnboundType = DevExpress.Data.UnboundColumnType.String;

                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    if (hdndisplayMode.Value == "Monthly")
                    {
                        if (Request["pStartDate"] != null && Request["pEndDate"] != null)
                            colId.Width = new Unit("80px");
                        else
                            colId.Width = new Unit("50px");
                    }
                    else if (hdndisplayMode.Value == "Daily")
                    {
                        colId.Width = new Unit("100px");
                    }
                    else
                    {
                        colId.Width = new Unit("60px");
                    }

                    bdCol.Columns.Add(colId);

                    ASPxSummaryItem itemDFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                    itemDFTE.DisplayFormat = "N2";
                    gvCapacityReport.TotalSummary.Add(itemDFTE);

                    ASPxSummaryItem itemTFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                    itemTFTE.Tag = "TFTE";
                    itemTFTE.DisplayFormat = "N2";
                    //gvCapacityReport.TotalSummary.Add(itemTFTE);
                }

                if (currentDate == bdCol.Caption)
                {
                    gvCapacityReport.Columns.Add(bdCol);
                }
            }
           // string dept = Convert.ToString(hdnaspDepartment.Value); //!string.IsNullOrEmpty(ddlDepartment.Value) ? ddlDepartment.Value : ddlDepartment.GetValues();            
           // LoadDdlResourceManager(dept, ddlResourceManager.SelectedItem.Value);
        }

        private List<ResourceAllocationMonthly> LoadAllocationMonthlyView()
        {
            try
            {
                string commQuery = string.Empty;
                string dtfrom = Convert.ToDateTime(hdndtfrom.Value).ToString("yyyy-MM-dd");
                string dtto = Convert.ToDateTime(hdndtto.Value).ToString("yyyy-MM-dd");
                //if (Request["ticketId"] == null && divProject.Visible == false)
                //{
                //    commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.MonthStartDate, dtfrom, dtto);
                //}
                //else
                //{
                //    if (rbtnProject.Checked)
 //                       commQuery = string.Format("{3} = '{4}' AND {0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.MonthStartDate, dtfrom, dtto, DatabaseObjects.Columns.ResourceWorkItem, Request["ticketId"]);
                    //else
                        commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.MonthStartDate, dtfrom, dtto);
                //}

                if (chkIncludeClosed.Checked)
                    return allcMManager.Load(commQuery);
                else
                    return allcMManager.LoadOpenItems(commQuery);
            }
            catch (Exception ex)
            { ULog.WriteException(ex); }
            return null;
        }


        #region helper method
        private int GetDaysForDisplayMode(string dMode, DateTime dt)
        {
            int days = 30;
            switch (dMode)
            {
                case "Daily":
                    days = 1;
                    break;
                case "Weekly":
                    {
                        if (dt.ToString("ddd") == "Mon")
                            days = 7;
                        else if (dt.ToString("ddd") == "Tue")
                            days = 6;
                        else if (dt.ToString("ddd") == "Wed")
                            days = 5;
                        else if (dt.ToString("ddd") == "Thu")
                            days = 4;
                        else if (dt.ToString("ddd") == "Fri")
                            days = 3;
                        else if (dt.ToString("ddd") == "Sat")
                            days = 2;
                        else if (dt.ToString("ddd") == "Sun")
                            days = 1;

                        break;
                    }
                case "Monthly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(1)) - dt).Days;
                    break;
                default:
                    break;
            }
            return days;
        }
        public class HoverMenuDataTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;

                HtmlGenericControl divContainer = new HtmlGenericControl("Div");
                divContainer.ID = string.Format("div_title_{0}", gridContainer.KeyValue);
                divContainer.Style.Add("float", "left");
                divContainer.Style.Add("width", "100%");
                divContainer.Style.Add("position", "relative");

                HtmlGenericControl innerDivContainer = new HtmlGenericControl("Div");
                innerDivContainer.ID = string.Format("actionButtons{0}", gridContainer.KeyValue);
                innerDivContainer.Style.Add("display", "none");
                innerDivContainer.Style.Add("width", "15px");
                innerDivContainer.Style.Add("position", "absolute");
                innerDivContainer.Style.Add("right", "0px");
                innerDivContainer.Style.Add("float", "right");
                string userName = DataBinder.Eval(container, string.Format("DataItem.{0}", DatabaseObjects.Columns.Resource)).ToString();

                innerDivContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px; width:20px;\" src=\"/content/images/plus-blue.png\" onclick=\"OpenAddAllocationPopup('" + gridContainer.KeyValue + "', '" + userName + "')\"  />"));
                divContainer.Controls.Add(new LiteralControl(string.Format("<span style='float:left;'>{0}</span>", userName)));
                divContainer.Controls.Add(innerDivContainer);
                //container.Controls.Add(divContainer);



            }
        }

        public DateTime FirstDayOfWeek(DateTime date)
        {
            var candidateDate = date;
            while (candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                candidateDate = candidateDate.AddDays(-1);
            }
            return candidateDate;
        }

        private int GetISOWeek(DateTime d)
        {
            return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private DataTable SummarizeDataTable(DataTable dtStaffSheet, string columnToGroup)
        {
            var rows = dtStaffSheet.AsEnumerable();
            var columns = dtStaffSheet.Columns.Cast<DataColumn>();
            //string columnToGroup = ColToSum;
            DataColumn colToGroup = columns.First(c => c.ColumnName.Equals(columnToGroup, StringComparison.OrdinalIgnoreCase));
            var colsToSum = columns.Where(c => c != colToGroup && c.Caption != "ItemOrder" && c.Caption != "Id" && c.Caption != "FullAllocation"  && c.ColumnName != "JobTitle" && c.ColumnName != "Role" && c.ColumnName != DatabaseObjects.Columns.ResourceQuantity && c.Caption != DatabaseObjects.Columns.ProjectCapacity && c.Caption != DatabaseObjects.Columns.RevenueCapacity);
            var columnsToSum = new HashSet<DataColumn>(colsToSum);

            var colsToCount = columns.Where(c => c.Caption == DatabaseObjects.Columns.ProjectCapacity || c.Caption == DatabaseObjects.Columns.RevenueCapacity);
            var columnsToCount = new HashSet<DataColumn>(colsToCount);

            resultTable = dtStaffSheet.Clone(); // empty table, same schema
            int ResourceCount = 0;
            foreach (var groupItem in rows.GroupBy(r => r[colToGroup]))
            {
                ResourceCount = 0;

                if (groupItem.Key == DBNull.Value)
                    ResourceCount = groupItem.CopyToDataTable().AsEnumerable().Select(x => x.Field<int?>("ResourceQuantity")).Max() ?? 0;
                else if (groupItem.ToArray().Length > 1 && groupItem.ToArray()[0]["ResourceQuantity"] != DBNull.Value)
                    ResourceCount = Convert.ToInt32(groupItem.ToArray()[0]["ResourceQuantity"]);
                else
                    ResourceCount = 0;

                DataRow row = resultTable.Rows.Add();
                foreach (var col in columns)
                {
                    try
                    {
                        if (columnsToSum.Contains(col))
                        {
                            if (col.ColumnName != "Id" && col.ColumnName != "ItemOrder" && col.ColumnName != "Resource" && col.ColumnName != "JobTitle" && col.ColumnName != "Role" && col.ColumnName != "ResourceUser")
                            {
                                double sum = groupItem.Sum(r => r.Field<string>(col) == null ? 0 : Convert.ToDouble(r.Field<string>(col)));
                                row[col.ColumnName] = Convert.ToInt64(sum);
                            }
                        }
                        else if (columnsToCount.Contains(col))
                        {
                            var lst = (from n in groupItem
                                       select n.Field<string>(col)).Distinct();

                            double sum = lst.Sum(r => r == null ? 0 : Convert.ToDouble(r));
                            row[col.ColumnName] = Convert.ToInt64(sum); //sum.ToString("N2");
                        }
                        else if (col.ColumnName == "ResourceQuantity")
                            row[col.ColumnName] = ResourceCount;
                        else
                            row[col.ColumnName] = groupItem.First()[col];
                    }
                    catch(Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                    
                }
            }
            return resultTable;
        }
        #endregion

        protected void LoadDdlResourceManager(string values = "", string selectedMgr = "")
        {
            //List<UserProfile> lstUserProfile = profileManager.Load(x => x.IsManager).OrderBy(x=>x.Name).ToList();  // commented to filter disabled Managers
            //List<UserProfile> lstUserProfile = profileManager.Load(x => x.IsManager && x.Enabled == true).OrderBy(x => x.Name).ToList();

            List<UserProfile> lstUserProfile = new List<UserProfile>();

            if (values == "undefined" || string.IsNullOrEmpty(values))
                lstUserProfile = profileManager.Load(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            else
            {
                List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                lstUserProfile = profileManager.Load(x => x.IsManager == true && x.Enabled == true && lstDepartments.Contains(x.Department)).OrderBy(x => x.Name).ToList();
            }

            ddlResourceManager.Items.Clear();

            if (lstUserProfile != null)
            {
                foreach (UserProfile userProfileItem in lstUserProfile)
                {
                    ddlResourceManager.Items.Add(new ListItem(userProfileItem.Name, userProfileItem.Id.ToString()));
                }
                //if (lstUserProfile.Count == 0)
                ddlResourceManager.Items.Insert(0, new ListItem("All Users", "0"));

                UserProfile currentUserProfile = HttpContext.Current.CurrentUser();
                if (currentUserProfile != null && currentUserProfile.IsManager)
                {
                    ddlResourceManager.SelectedIndex = ddlResourceManager.Items.IndexOf(ddlResourceManager.Items.FindByValue(currentUserProfile.Id));
                    //                   ddlDepartment.SetValues(currentUserProfile.Department);
                }
                else if (ddlResourceManager.SelectedIndex <= 0 && !string.IsNullOrWhiteSpace(currentUserProfile.ManagerID))
                {
                    ddlResourceManager.SelectedIndex = ddlResourceManager.Items.IndexOf(ddlResourceManager.Items.FindByValue(currentUserProfile.ManagerID));
                    //                    UserProfile currentUserManager = HttpContext.Current.GetUserManager().GetUserById(currentUserProfile.ManagerID);
                    //                    if (currentUserManager != null)
                    //                        ddlDepartment.SetValues(currentUserManager.Department);
                }
                else
                    ddlResourceManager.SelectedIndex = 0;
            }
            else
            {
                ddlResourceManager.Items.Insert(0, new ListItem("All Users", "0"));
            }

            if (!string.IsNullOrEmpty(selectedMgr) && ddlResourceManager.Items.FindByValue(selectedMgr) != null)
            {
                ddlResourceManager.ClearSelection();
                ddlResourceManager.Items.FindByValue(selectedMgr).Selected = true;
            }
        }

        private void LoadDepartment()
        {
            CompanyManager companyManager = new CompanyManager(applicationContext);
            List<Company> companies = new List<Company>();
            companies = companyManager.Load();  // uGITCache.LoadCompanies(SPContext.Current.Web);
            DepartmentManager departmentManager = new DepartmentManager(applicationContext);
            //List<Department> activeDepartments = departmentManager.Load();   // companies.First().Departments.Where(x => !x.IsDeleted).ToList();
            //ddlDepartment.DataValueField = DatabaseObjects.Columns.ID;
            //ddlDepartment.DataTextField = DatabaseObjects.Columns.Title;
            //ddlDepartment.DataSource = activeDepartments;
            //ddlDepartment.DataBind();
            //ddlDepartment.Items.Insert(0, new ListItem(Constants.AllDepartments, "0"));

        }

        private void LoadFunctionalArea()
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(applicationContext);
            List<FunctionalArea> funcationalArealst = functionalAreasManager.LoadFunctionalAreas();     /// uGITCache.LoadFunctionalAreas(SPContext.Current.Web);

            List<FunctionalArea> filterFuncationalArealst = new List<FunctionalArea>();
            if (ddlDepartment.GetValues() != null && ddlDepartment.GetValues() != string.Empty)
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted && x.DepartmentLookup != null && x.DepartmentLookup.Value == UGITUtility.StringToInt(ddlDepartment.GetValues())).ToList();
            else
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted).ToList();

            ddlFunctionalArea.DataValueField = DatabaseObjects.Columns.ID;
            ddlFunctionalArea.DataTextField = DatabaseObjects.Columns.Title;

            ddlFunctionalArea.DataSource = filterFuncationalArealst;
            ddlFunctionalArea.DataBind();
            ddlFunctionalArea.Items.Insert(0, new ListItem("None", "0"));
        }

        protected void FillDropDownType()
        {
            DataTable dtTypeDate = AllocationTypeManager.LoadLevel1(applicationContext);
            if (dtTypeDate != null)
            {
                for (int i = dtTypeDate.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtTypeDate.Rows[i]["LevelTitle"] == DBNull.Value)
                    {
                        dtTypeDate.Rows[i].Delete();
                    }
                }
                dtTypeDate.AcceptChanges();



                DataRow dtrow = dtTypeDate.NewRow();
                dtrow["LevelTitle"] = "ALL";
                dtrow["LevelName"] = "ALL";
                dtTypeDate.Rows.InsertAt(dtrow, 0);

                glType.DataSource = dtTypeDate;
                glType.DataBind();
            }
        }

        protected void chkColor_CheckedChanged(object sender, EventArgs e)
        {
            //SetCookiesCheckboxFilters();
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
            //SetCookiesCheckboxFilters();
        }

        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            {
                DateTime selectedMonth = UGITUtility.StringToDateTime(hdnSelectedDate.Value);
                DateTime nextMonth = selectedMonth.AddMonths(1);
                DateTime previousMonth = selectedMonth.AddMonths(-1);
                hdnSelectedDate.Value = selectedMonth.AddMonths(-1).ToString("MMM-dd-yy");
                hdndtfrom.Value = UGITUtility.ObjectToString(GetFirstMondayOfMonth(previousMonth));
                hdndtto.Value = UGITUtility.ObjectToString(GetLastMondayOfMonth(nextMonth));
            }
            else
            {
                if (string.IsNullOrEmpty(hdndtfrom.Value) || (string.IsNullOrEmpty(hdndtto.Value)))
                {
                    string year = UGITUtility.GetCookieValue(Request, "year");
                    string fromDate = year + "-01-01 00:00:00.000";
                    string endDate = year + "-12-31 00:00:00.000";
                    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(fromDate).AddYears(-1));
                    hdndtto.Value = Convert.ToString(Convert.ToDateTime(endDate).AddYears(-1));
                }
                else
                {
                    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddYears(-1));
                    hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddYears(-1));
                }
            }
            PrepareAllocationGrid();
            dtMonthlyAllocation = null;
            dtRawDataAllResource = null;
            gvCapacityReport.DataSource = GetAllocationData();
        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            {
                DateTime selectedMonth = UGITUtility.StringToDateTime(hdnSelectedDate.Value);
                DateTime nextMonth = selectedMonth.AddMonths(3);
                DateTime previousMonth = selectedMonth.AddMonths(1);
                hdnSelectedDate.Value = selectedMonth.AddMonths(1).ToString("MMM-dd-yy");
                hdndtfrom.Value = UGITUtility.ObjectToString(GetFirstMondayOfMonth(previousMonth));
                hdndtto.Value = UGITUtility.ObjectToString(GetLastMondayOfMonth(nextMonth));
            }
            else
            {
                if (string.IsNullOrEmpty(hdndtfrom.Value) || (string.IsNullOrEmpty(hdndtto.Value)))
                {
                    string year = UGITUtility.GetCookieValue(Request, "year");
                    string fromDate = year + "-01-01 00:00:00.000";
                    string endDate = year + "-12-31 00:00:00.000";
                    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(fromDate).AddYears(1));
                    hdndtto.Value = Convert.ToString(Convert.ToDateTime(endDate).AddYears(1));
                }
                else
                {
                    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddYears(1));
                    hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddYears(1));
                }
            }
            PrepareAllocationGrid();
            dtMonthlyAllocation = null;
            dtRawDataAllResource = null;
            gvCapacityReport.DataSource = GetAllocationData();
        }
        public DateTime GetFirstMondayOfMonth(DateTime date)
        {
            // Determine the first day of the month for the given date
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            // Calculate the number of days to add to reach the first Monday
            int daysToAdd = (DayOfWeek.Monday - firstDayOfMonth.DayOfWeek + 7) % 7;

            // Calculate the date of the first Monday by adding the days to the first day of the month
            DateTime firstMondayOfMonth = firstDayOfMonth.AddDays(daysToAdd);

            return firstMondayOfMonth;
        }
        protected void ddlUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //uHelper.CreateCookie(Response, "filter", string.Format("group-{0}", ddlUserGroup.SelectedValue));
            UGITUtility.CreateCookie(Response, "filter", string.Format("group-{0}#type-{1}", ddlUserGroup.SelectedValue, glType.Text));
            hdnSelectedGroup.Value = ddlUserGroup.SelectedValue;
            
            PrepareAllocationGrid();
        }

        protected void ddlResourceManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterResource");
            PrepareAllocationGrid();
        }

        protected void ddlFunctionalArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterFunctionArea");
            PrepareAllocationGrid();
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            //SetCookiesCheckboxFilters();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void rbtnEstimate_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rbtnAssigned_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rbtnItemCount_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "count");
        }

        protected void rbtnPercentage_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "percentage");
        }

        protected void rbtnFTE_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "fte");
        }
        //change by Hareram
        protected void rbtnAvailability_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "availability");
        }
        protected void rbtnHrs_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "hrs");
        }
        protected void btnDrilDown_Click(object sender, EventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Weekly.ToString();

                //modification related to grid header.
                if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Mon")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime());
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Tue")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(6));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Wed")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(5));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Thu")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(4));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Fri")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(3));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sat")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(2));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sun")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(1));

                DateTime enddateweekly = hdnSelectedDate.Value.ToDateTime().AddMonths(1).AddDays(-1);
                if (enddateweekly.ToString("ddd") == "Mon")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(6));
                else if (enddateweekly.ToString("ddd") == "Tue")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(5));
                else if (enddateweekly.ToString("ddd") == "Wed")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(4));
                else if (enddateweekly.ToString("ddd") == "Thu")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(3));
                else if (enddateweekly.ToString("ddd") == "Fri")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(2));
                else if (enddateweekly.ToString("ddd") == "Sat")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(1));
                else if (enddateweekly.ToString("ddd") == "Sun")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(0));

                DateTime firstDayOfPreviousMonth = Convert.ToDateTime(hdndtfrom.Value).AddDays(1 - Convert.ToDateTime(hdndtfrom.Value).Day);

                // Find the day of the week for the first day of the previous month
                DayOfWeek dayOfWeek = firstDayOfPreviousMonth.DayOfWeek;

                // Calculate the number of days to add to reach the first Monday
                int daysToAdd = (DayOfWeek.Monday - dayOfWeek + 7) % 7;

                // Calculate the date of the previous month's first Monday
                DateTime previousMonthsFirstMonday = firstDayOfPreviousMonth.AddDays(daysToAdd);

                DateTime nextMonth = Convert.ToDateTime(hdndtto.Value).AddMonths(2);
                if (nextMonth.Month - firstDayOfPreviousMonth.Month > 2)
                    nextMonth = nextMonth.AddMonths(-1);

                // Calculate the last Monday of the next month
                DateTime lastMondayOfNextMonth = GetLastMondayOfMonth(nextMonth);
                while (lastMondayOfNextMonth.DayOfWeek != DayOfWeek.Monday)
                {
                    lastMondayOfNextMonth = lastMondayOfNextMonth.AddDays(-1);
                }


                hdndtfrom.Value = Convert.ToString(previousMonthsFirstMonday);
                // Calculate the date 4 weeks after endDate

                hdndtto.Value = Convert.ToString(lastMondayOfNextMonth);
            }
            //else if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            //{
            //    hdndisplayMode.Value = DisplayMode.Daily.ToString();
            //    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime());

            //    if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Mon")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(7));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Tue")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(6));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Wed")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(5));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Thu")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(4));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Fri")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(3));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sat")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(2));
            //    else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sun")
            //        hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(1));
            //}

            PrepareAllocationGrid();
            gvCapacityReport.DataSource = GetAllocationData();
        }
        public DateTime GetLastMondayOfMonth(DateTime date)
        {
            // First, determine the last day of the month for the given date
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return lastDayOfMonth;
        }
        protected void btnDrilUp_Click(object sender, EventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Monthly.ToString();

                if (Request["pStartDate"] != null && Request["pEndDate"] != null)
                {

                    DateTime tempstartDate = Convert.ToDateTime(Request["pStartDate"]);
                    DateTime tempendDate = Convert.ToDateTime(Request["pEndDate"]);
                    DateTime tempDate = DateTime.ParseExact(hdnSelectedDate.Value, "MMM-yy", null);

                    if (tempstartDate.Year == tempDate.Year)
                    {
                        hdndtfrom.Value = Convert.ToString(tempstartDate);
                        hdndtto.Value = Convert.ToString(tempendDate);
                    }
                    else
                    {
                        hdndtfrom.Value = Convert.ToString(new DateTime(tempDate.Year, tempstartDate.Month, tempstartDate.Day));
                        hdndtto.Value = Convert.ToString(new DateTime(tempDate.Year, tempendDate.Month, tempendDate.Day));
                    }

                }
                else
                {
                    DateTime tempDate = DateTime.ParseExact(hdnSelectedDate.Value, "MMM-yy", null);
                    hdndtfrom.Value = Convert.ToString(new DateTime(tempDate.Year, 1, 1));
                    hdndtto.Value = Convert.ToString(new DateTime(tempDate.Year, 12, 31));
                }
            }
            //else if (hdndisplayMode.Value == DisplayMode.Daily.ToString())
            //{
            //    hdndisplayMode.Value = DisplayMode.Weekly.ToString();

            //    DateTime tempfromDate = new DateTime(Convert.ToDateTime(hdnSelectedDate.Value).Year, Convert.ToDateTime(hdnSelectedDate.Value).Month, 1);

            //    if (tempfromDate.ToString("ddd") == "Mon")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate);
            //    else if (tempfromDate.ToString("ddd") == "Tue")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(6));
            //    else if (tempfromDate.ToString("ddd") == "Wed")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(5));
            //    else if (tempfromDate.ToString("ddd") == "Thu")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(4));
            //    else if (tempfromDate.ToString("ddd") == "Fri")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(3));
            //    else if (tempfromDate.ToString("ddd") == "Sat")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(2));
            //    else if (tempfromDate.ToString("ddd") == "Sun")
            //        hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(1));

            //    DateTime enddateweekly = tempfromDate.AddMonths(1).AddDays(-1);

            //    if (enddateweekly.ToString("ddd") == "Mon")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(6));
            //    else if (enddateweekly.ToString("ddd") == "Tue")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(5));
            //    else if (enddateweekly.ToString("ddd") == "Wed")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(4));
            //    else if (enddateweekly.ToString("ddd") == "Thu")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(3));
            //    else if (enddateweekly.ToString("ddd") == "Fri")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(2));
            //    else if (enddateweekly.ToString("ddd") == "Sat")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(1));
            //    else if (enddateweekly.ToString("ddd") == "Sun")
            //        hdndtto.Value = Convert.ToString(enddateweekly.AddDays(0));
            //}

            PrepareAllocationGrid();
            gvCapacityReport.DataSource = GetAllocationData();
        }

        protected void rbtnProject_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void cbpManagers_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] values = UGITUtility.SplitString(parameters, Constants.Separator2);
            if (values.Count() >= 1)
            {
                LoadDdlResourceManager(values[0]);
            }
            else
            {
                LoadDdlResourceManager();
            }
        }

        protected void cbpResourceAvailability_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] arrParams = UGITUtility.SplitString(parameters, Constants.CommentSeparator);
            if (arrParams.Count() > 0)
            {
                string[] values = UGITUtility.SplitString(arrParams[0], Constants.Separator2);
                if (values.Count() >= 1)
                {
                    LoadGlobalRolesOnDepartment(values[1]);
                    if (values[1].EqualsIgnoreCase("undefined"))
                        hdnaspDepartment.Value = "";
                    else
                        hdnaspDepartment.Value = values[1];
                }
            }

            ASPxCallbackPanel gv = (ASPxCallbackPanel)sender;
            gv.JSProperties["cpResourceAvailabilityCallback"] = hdnaspDepartment.Value;
        }

        private void LoadGlobalRolesOnDepartment(string values)
        {
            JobTitleManager jobTitleManager = new JobTitleManager(applicationContext);
            List<JobTitle> jobTitles = new List<JobTitle>();
            if (string.IsNullOrEmpty(values) || values == "undefined")
                jobTitles = jobTitleManager.Load();
            else
            {
                List<string> lstDepartments = UGITUtility.ConvertStringToList(ddlDepartment.GetValues(), Constants.Separator6);
                jobTitles = jobTitleManager.Load(x => lstDepartments.Contains(Convert.ToString(x.DepartmentId)));
            }

            List<string> jobtitleids = jobTitles.Select(x => x.RoleId).ToList();

            List<GlobalRole> globalRoles = new List<GlobalRole>();
            globalRoles = roleManager.Load(x => jobtitleids.Contains(x.Id));
            if (globalRoles != null)
            {
                ddlUserGroup.DataSource = globalRoles;
                ddlUserGroup.DataTextField = "Name";
                ddlUserGroup.DataValueField = "ID";
                ddlUserGroup.DataBind();

            }
            ddlUserGroup.Items.Insert(0, new ListItem("All Roles", "0"));
        }

        protected void ddlUserGroup_Init(object sender, EventArgs e)
        {
            string hdndept = UGITUtility.ObjectToString(Request[hdnaspDepartment.UniqueID]);
            if (string.IsNullOrEmpty(hdndept))
            {
                List<GlobalRole> roles = uHelper.GetGlobalRoles(applicationContext, false); //roleManager.Load(x => !x.Deleted).OrderBy(x => x.Name).ToList();
                if (roles != null)
                {
                    ddlUserGroup.DataSource = roles.OrderBy(x => x.Name);
                    ddlUserGroup.DataTextField = "Name";
                    ddlUserGroup.DataValueField = "Id";
                    ddlUserGroup.DataBind();
                }

                ddlUserGroup.Items.Insert(0, new ListItem("All Roles", ""));
            }
            else
            {
                LoadGlobalRolesOnDepartment(hdndept);
            }
        }

        public class CommandColumnHeaderTemplate : ITemplate
        {
            GridViewDataTextColumn colID = null;
            public CommandColumnHeaderTemplate(GridViewDataTextColumn coID)
            {
                this.colID = coID;
            }

            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                HyperLink hnkButton = new HyperLink();
                hnkButton.Text = this.colID.Caption;
                container.Controls.Add(hnkButton);
                //if (!string.IsNullOrEmpty(HttpContext.Current.Request["pGroupName"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["ticketId"]))
                ////if (HttpContext.Current.Request["rbtnProject"] == "checked")
                ////{
                string func = string.Format("ClickOnDrillDown(this,'{0}','{1}')", colID.FieldName, colID.Caption);
                hnkButton.Attributes.Add("onclick", func);
                //}
            }

            #endregion
        }

        public class CommandGridViewBandColumn : ITemplate
        {
            GridViewBandColumn colBDC = null;

            public CommandGridViewBandColumn(GridViewBandColumn coID)
            {
                this.colBDC = coID;
            }

            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                HtmlGenericControl HContainer = new HtmlGenericControl("Div");
                HyperLink hnkBDButton = new HyperLink();
                hnkBDButton.Style.Add("vertical-align", "top");
                hnkBDButton.Text = this.colBDC.Caption;
                //container.Controls.Add(hnkBDButton);
                string func = string.Format("ClickOnDrillUP(this,'{0}')", colBDC.Caption);
                hnkBDButton.Attributes.Add("onclick", func);

                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;\" src=\"/content/images/back-arrowBlue.png\" onclick=\"ClickOnPrevious()\" class=\"resource-img\"  />"));
                HContainer.Controls.Add(hnkBDButton);
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;\" src=\"/content/images/next-arrowBlue.png\" onclick=\"ClickOnNext()\" class=\"resource-img\"  />"));
                container.Controls.Add(HContainer);
            }

            #endregion
        }
        protected void gvResourceAvailablity_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadFunctionalArea();
            PrepareAllocationGrid();
        }

        protected void gvCapacityReport_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (Request["IsResourceDrillDown"] != "true")//Request["ResourceCapacity"] == "true")
            {
                var obj = gvCapacityReport.GetRow(e.VisibleIndex);
                DateTime dtStart = DateTime.MinValue;
                if (e.DataColumn.Caption == "#")
                    e.Cell.Text = Convert.ToString(e.VisibleIndex + 1);
                var dataRowView = obj as DataRowView;
                string userID = UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.Id]);
                /*
                string userID = UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.Id]);
                profile = HttpContext.Current.GetManagerContext().UserManager.GetUserById(userID);
                //JobTitleManager jobTitleManager = new JobTitleManager(applicationContext);
                if (profile != null)
                    jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                */
                //dataRowView[DatabaseObjects.Columns.JobTitle] 

                if (ddlDepartment.GetValues() != null && ddlDepartment.GetValues() != string.Empty)
                {
                    selectedDepartments = UGITUtility.ConvertStringToList(ddlDepartment.GetValues(), Constants.Separator6);
                    filteredUserProfiles = userProfiles.Where(x => selectedDepartments.Any(y => y == x.Department)).ToList();
                }
                else if(!string.IsNullOrEmpty(ddlDepartment.Value))
                {
                    selectedDepartments = UGITUtility.ConvertStringToList(ddlDepartment.Value, Constants.Separator6);
                    filteredUserProfiles = userProfiles.Where(x => selectedDepartments.Any(y => y == x.Department)).ToList();
                }
                else if (!string.IsNullOrEmpty(hdnaspDepartment.Value))
                {
                    selectedDepartments = UGITUtility.ConvertStringToList(hdnaspDepartment.Value, Constants.Separator6);
                    filteredUserProfiles = userProfiles.Where(x => selectedDepartments.Any(y => y == x.Department)).ToList();
                }
                else
                {
                    filteredUserProfiles = userProfiles;
                }

                List<string> strUserIds = new List<string>();
                string JobTitle = string.Empty, JobTitleText = string.Empty;
                if (ddlCategory.SelectedItem.Value == "JobTitle")
                {
                     JobTitle = UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.JobTitle]);
                    //List<string> strUserIds = profileManager.GetUsersProfile().Where(x => x.JobTitleLookup == profile.JobTitleLookup && x.Enabled == true).Select(x=>x.Id).ToList();
                    
                    if (rbtnAll.Checked)
                        strUserIds = filteredUserProfiles.Where(x => x.JobProfile == JobTitle).Select(x => x.Id).ToList();
                    else
                        strUserIds = filteredUserProfiles.Where(x => x.JobProfile == JobTitle && x.Enabled == true).Select(x => x.Id).ToList();
                }
                else if(ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                {
                    JobTitle = UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.Role]);
                    JobTitleText = JobTitle;
                    if (!string.IsNullOrEmpty(JobTitle))
                    {
                        var role = userRoles.Where(x => x.Name == JobTitle).FirstOrDefault();
                        if (role != null)
                        {
                            if (rbtnAll.Checked)
                                strUserIds = filteredUserProfiles.Where(x => x.GlobalRoleId == role.Id).Select(x => x.Id).ToList();
                            else
                                strUserIds = filteredUserProfiles.Where(x => x.GlobalRoleId == role.Id && x.Enabled == true).Select(x => x.Id).ToList();

                            JobTitle = role.Id;
                        }
                    }
                }


                if (e.DataColumn.FieldName != "ItemOrder" && e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != DatabaseObjects.Columns.JobTitle && e.DataColumn.FieldName != DatabaseObjects.Columns.ResourceQuantity && Request["pStartDate"] == null && e.DataColumn.Caption != "# Active Projects" && e.DataColumn.Caption != "$ Active" && e.DataColumn.Caption != "# Lifetime Projects" && e.DataColumn.Caption != "$ Lifetime" && e.DataColumn.FieldName != DatabaseObjects.Columns.Role)
                {
                    string CategoryType = string.Empty;
                    if (ddlCategory.SelectedItem.Value == "JobTitle")
                        CategoryType = "jobTitleLookup";
                    else if (ddlCategory.SelectedItem.Value == "Role")
                        CategoryType = "roleLookup";
                    else if (ddlCategory.SelectedItem.Value == "UnfilledRoles")
                        CategoryType = "unfilledRolesLookup";

                    DateTime.TryParse(e.DataColumn.FieldName, out dtStart);
                    if ((!divProject.Visible && rbtnProject.Checked) || (divProject.Visible && rbtnAll.Checked))
                    {
                        string absoluteUrlEdit = string.Empty;
                        if (sActionType != null && sActionType.Count > 0 && !sActionType.Contains("ALL"))
                        {
                            string selectedType = string.Join(",", sActionType.ToArray());
                            if (rbtnEstimate.Checked)
                            {
                                absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&selectedType={4}&allocationType={5}&{8}={6}&isRedirectFromCardView=true&capacityreport=true&IncludeClosed={7}&SelectedDepts={9}", editParam, userID, dtStart.ToShortDateString(), dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart) - 1).ToShortDateString(), selectedType, ResourceAllocationType.Estimated, JobTitle, chkIncludeClosed.Checked, CategoryType, string.Join(Constants.Separator6, selectedDepartments)));
                            }
                            else
                                absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&selectedType={4}&allocationType={5}&isRedirectFromCardView=true&capacityreport=true&{8}={6}&IncludeClosed={7}&SelectedDepts={9}", editParam, userID, dtStart.ToShortDateString(), dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart) - 1).ToShortDateString(), selectedType, ResourceAllocationType.Planned, JobTitle, chkIncludeClosed.Checked, CategoryType, string.Join(Constants.Separator6, selectedDepartments)));
                        }
                        else
                        {
                            if (rbtnEstimate.Checked)
                            {
                                absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&allocationType={4}&monthlyAllocationEdit=false&isRedirectFromCardView=true&capacityreport=true&{7}={5}&IncludeClosed={6}&SelectedDepts={8}", editParam, userID, dtStart.ToShortDateString(), dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart) - 1).ToShortDateString(), ResourceAllocationType.Estimated, JobTitle, chkIncludeClosed.Checked, CategoryType, string.Join(Constants.Separator6, selectedDepartments)));
                            }
                            else
                            {
                                absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&allocationType={4}&monthlyAllocationEdit=false&isRedirectFromCardView=true&capacityreport=true&{7}={5}&IncludeClosed={6}&SelectedDepts={8}", editParam, userID, dtStart.ToShortDateString(), dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart) - 1).ToShortDateString(), ResourceAllocationType.Planned, JobTitle, chkIncludeClosed.Checked, CategoryType, string.Join(Constants.Separator6, selectedDepartments)));
                            }
                        }

                        string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteUrlEdit, formTitle, Server.UrlEncode(Request.Url.AbsolutePath));
                        e.Cell.Attributes.Add("onclick", func);
                    }

                }

                if (e.DataColumn.FieldName == "Resource")
                {
                    //e.Cell.BackColor = ColorTranslator.FromHtml("#F2F0F0");
                    if (!string.IsNullOrEmpty(userID))
                    {
                        //UserProfile user = ObjUserProfileManager.GetUserInfoById(userID);
                        //if (user != null)
                        //    e.Cell.Text = user.Name;
                    }
                }

                if (e.DataColumn.FieldName == DatabaseObjects.Columns.ResourceQuantity)
                {
                    if (ddlCategory.SelectedItem.Value == "JobTitle")
                    {
                        if (rbtnAll.Checked)
                        {
                            if (chkAll.Checked)
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => x.JobProfile == JobTitle).Select(x => x.Name).OrderBy(y => y).ToList());
                            else
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => x.JobProfile == JobTitle && x.Enabled == true).Select(x => x.Name).OrderBy(y => y).ToList());
                        }
                        else
                        {
                            if (chkAll.Checked)
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => x.JobProfile == (JobTitle ?? "")).Select(x => x.Name).OrderBy(y => y).ToList());
                            else
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => x.JobProfile == (JobTitle ?? "") && x.Enabled == true).Select(x => x.Name).OrderBy(y => y).ToList());

                        }
                    }
                    else if(ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                    {
                        if (rbtnAll.Checked)
                        {
                            if (chkAll.Checked)
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => strUserIds.Contains(x.Id)).Select(x => x.Name).Distinct().OrderBy(y => y).ToList());
                            else
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => strUserIds.Contains(x.Id) && x.Enabled == true).Select(x => x.Name).Distinct().OrderBy(y => y).ToList());
                        }
                        else
                        {
                            if (chkAll.Checked)
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => strUserIds.Contains(x.Id)).Select(x => x.Name).Distinct().OrderBy(y => y).ToList());
                            else
                                e.Cell.ToolTip = string.Join("\n", filteredUserProfiles.Where(x => strUserIds.Contains(x.Id) && x.Enabled == true).Select(x => x.Name).Distinct().OrderBy(y => y).ToList());
                        }
                    }
                }

                if (e.DataColumn.Caption != "#" && e.DataColumn.Caption != DatabaseObjects.Columns.RResource && e.DataColumn.Caption != "# Active Projects" && e.DataColumn.Caption != "$ Active" && e.DataColumn.Caption != "# Lifetime Projects" && e.DataColumn.Caption != "$ Lifetime" && e.DataColumn.Caption != "Job Title" && e.DataColumn.Caption != "# Of Resources" && e.DataColumn.Caption != DatabaseObjects.Columns.Role)
                {
                    e.Cell.Text = string.Empty;
                    if (rbtnPercentage.Checked)
                    {
                        int ResourceQuantity = UGITUtility.StringToInt(UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.ResourceQuantity]));
                        double cellFTE = UGITUtility.StringToDouble(Convert.ToString(e.CellValue));

                        if (ResourceQuantity > 0)
                        {
                            var result = cellFTE / ResourceQuantity;

                            if (result <= gray)
                            {
                                e.Cell.Text = string.Empty;
                            }
                            else
                            {
                                e.Cell.Text = string.Format("{0:0}%", Math.Round(result, 0));
                            }
                        }
                        else
                            e.Cell.Text = string.Empty;


                        /*
                        if (Convert.ToDouble(e.CellValue) <= gray)
                            e.Cell.Text = string.Empty;
                        else if (Convert.ToDouble(e.CellValue) >= red)
                            e.Cell.Text = e.CellValue.ToString().Trim() + "%";
                        else if (Convert.ToDouble(e.CellValue) >= green && Convert.ToDouble(e.CellValue) <= yellow)
                            e.Cell.Text = e.CellValue.ToString().Trim() + "%";
                        else
                            e.Cell.Text = e.CellValue.ToString().Trim() + "%";
                        */

                    }

                    if (rbtnFTE.Checked && Convert.ToDouble(e.CellValue) > 0)
                    {
                        double cellFTE = UGITUtility.StringToDouble(Convert.ToString(e.CellValue)) / 100;
                        e.Cell.Text = string.Format("{0:0.00}", Math.Round(cellFTE, 2));
                    }

                    //if (chkItemCount.Checked && Convert.ToDouble(e.CellValue) > 0)
                    if (rbtnItemCount.Checked && Convert.ToDouble(e.CellValue) > 0)
                    {
                        int count = 0;
                        if (hdndisplayMode.Value == "Monthly")
                        {
                            string exp = string.Empty;

                            if (currenSelectedUserID != userID || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                            {
                                currenSelectedUserID = userID;

                                if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                                {
                                    currentSelectedRows = (from monthly in dtMonthlyAllocation.Where(x => x.ResourceSubWorkItem.EqualsIgnoreCase(JobTitleText)).ToList()
                                                           where (strUserIds.Contains(monthly.Resource))
                                                           select monthly).ToList();
                                }
                                else
                                {
                                    currentSelectedRows = (from monthly in dtMonthlyAllocation
                                                           where (strUserIds.Contains(monthly.Resource))
                                                           select monthly).ToList();
                                }
                            }
                            if (sActionType != null && sActionType.Count > 0 && !sActionType.Contains("ALL"))
                            {
                                if (rbtnAssigned.Checked)
                                {
                                    var rowcount = from monthly in currentSelectedRows
                                                   where (monthly.MonthStartDate == dtStart
                                                          && sActionType.Contains(monthly.ResourceWorkItemType) && monthly.PctPlannedAllocation.Value != 0)
                                                   group monthly by monthly.ResourceWorkItem into monthgroup
                                                   select new
                                                   {
                                                       ResourceWorkItem = monthgroup.Key,
                                                       ProjectCount = monthgroup.Count()
                                                   };

                                    //if (rowcount != null)
                                    //    count = rowcount.Select(x => x.ProjectCount).Count();

                                    if (rowcount != null)
                                        count = rowcount.Sum(x => x.ProjectCount);
                                }
                                else
                                {
                                    try
                                    {
                                        var rowcount = from monthly in currentSelectedRows
                                                       where (monthly.MonthStartDate == dtStart
                                                              && sActionType.Contains(monthly.ResourceWorkItemType) && monthly.PctAllocation.Value != 0)
                                                       group monthly by monthly.ResourceWorkItem into monthgroup
                                                       select new
                                                       {
                                                           ResourceWorkItem = monthgroup.Key,
                                                           ProjectCount = monthgroup.Count()
                                                       };

                                        //if (rowcount != null)
                                        //    count = rowcount.Select(x => x.ProjectCount).Count();

                                        if (rowcount != null)
                                            count = rowcount.Sum(x => x.ProjectCount);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }
                                }
                            }
                            else
                            {
                                if (rbtnAssigned.Checked)
                                {
                                    try
                                    {
                                        var rowcount = from monthly in currentSelectedRows
                                                       where (monthly.MonthStartDate == dtStart && UGITUtility.StringToDouble(monthly.PctPlannedAllocation) != 0)
                                                       group monthly by monthly.ResourceWorkItem into monthgroup
                                                       select new
                                                       {
                                                           ResourceWorkItem = monthgroup.Key,
                                                           ProjectCount = monthgroup.Count()
                                                       };


                                        //if (rowcount != null)
                                        //    count = rowcount.Select(x => x.ProjectCount).Count();

                                        if (rowcount != null)
                                            count = rowcount.Sum(x => x.ProjectCount);
                                    }
                                    catch(Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }

                                }
                                else
                                {
                                    var rowcount = (from monthly in currentSelectedRows
                                                   where (monthly.MonthStartDate == dtStart
                                                     && UGITUtility.StringToDouble(monthly.PctAllocation) != 0)
                                                   group monthly by monthly.ResourceWorkItem into monthgroup
                                                   select new
                                                   {
                                                       ResourceWorkItem = monthgroup.Key,
                                                       ProjectCount = monthgroup.Count()
                                                   }).ToList();


                                    if (rowcount != null)
                                        count = rowcount.Count(); //count = rowcount.Sum(x => x.ProjectCount);
                                }
                            }
                        }
                        else
                        {
                            bool IsAssignedallocation = false;
                            if (Request["AllocationViewType"] == "RMMAllocation")
                            {
                                if (rbtnAssigned.Checked)
                                    IsAssignedallocation = true;
                                else
                                    IsAssignedallocation = false;
                            }

                            count = ObjResourceAllocationManager.CountAllocationPercentageWithProjectType(dtRawDataAllResource, Convert.ToString(((DataRowView)obj).Row[DatabaseObjects.Columns.Id]), 4, dtStart, dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart)).AddDays(-1), false, sActionType, resultedTable, UGITUtility.StringToBoolean(Request["IsRunScenario"]), UGITUtility.StringToInt(Request["viewid"]), IsAssignedallocation, workItemList: allResourceWorkItemsSPList);
                        }

                        if (count > 0)
                            e.Cell.Text = Convert.ToString(count);
                        else
                            e.Cell.Text = string.Empty;

                    }

                    if (rbtnAvailability.Checked && !string.IsNullOrEmpty(UGITUtility.ObjectToString(e.CellValue)))
                    {
                        //string value = UGITUtility.ObjectToString(e.CellValue);
                        //var Availability = Convert.ToDouble(value);
                        double ResourceQuantity = UGITUtility.StringToDouble(UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.ResourceQuantity]));

                        double cellFTE = UGITUtility.StringToDouble(Convert.ToString(e.CellValue)) / 100;
                        var result = ResourceQuantity - cellFTE;

                        if (result == 0)
                        {
                            e.Cell.Text = "" ;
                            e.Cell.BackColor = Color.Red;
                        }
                        else if (result < 0)
                        {
                            e.Cell.Text = "" ;
                            //e.Cell.BackColor = ColorTranslator.FromHtml("#afe9fe");
                            //if(result <= -1.2 && result >= -1)
                            e.Cell.BackColor = System.Drawing.Color.Red;
                        }
                        else if (result > 0)
                        {
                            e.Cell.Text = string.Format("{0:0.00}", Math.Round(result, 2)); //Convert.ToString(result);
                            //e.Cell.BackColor = ColorTranslator.FromHtml("#FFBA62");
                            if (result > 0 && result <= 0.3)
                                e.Cell.BackColor = ColorTranslator.FromHtml("#afe9fe"); //System.Drawing.Color.Yellow;
                            else
                                e.Cell.BackColor = ColorTranslator.FromHtml("#fcf7b5"); //System.Drawing.Color.Green;
                        }
                        else
                        {
                            e.Cell.Text = string.Format("{0:0.00}", Math.Round(result, 2)); //Convert.ToString(result);
                            e.Cell.BackColor = ColorTranslator.FromHtml("#fcf7b5");

                        }
                        

                        //e.Cell.Text = string.Format("{0:0.00}", Math.Round(cellFTE, 2));
                        //var result = (Availability >= 100) ? Availability : (Availability == 0) ? 0 : 100 - Availability; //: Availability = 0;

                        //if (result == 0)
                        //{
                        //    e.Cell.Text = string.Empty;
                        //    e.Cell.BackColor = Color.Transparent;
                        //}
                        //else if (result <= 30)
                        //{
                        //    e.Cell.Text = result + "%";
                        //    e.Cell.BackColor = ColorTranslator.FromHtml("#afe9fe");
                        //}
                        //else if (result >= 100)
                        //{
                        //    e.Cell.Text = result + "%";
                        //    e.Cell.BackColor = ColorTranslator.FromHtml("#FFBA62");
                        //}
                        //else
                        //{
                        //    e.Cell.Text = result + "%";
                        //    e.Cell.BackColor = ColorTranslator.FromHtml("#fcf7b5");

                        //}
                    }

                    // Commented to show Colours, by default
                    //if (chkColor.Checked)
                    {
                        // if (chkPercentage.Checked || chkItemCount.Checked)
                        if (rbtnItemCount.Checked || rbtnPercentage.Checked || rbtnFTE.Checked || rbtnAvailability.Checked)
                        {
                            e.Cell.BackColor = Color.White;
                            if (Convert.ToDouble(e.CellValue) <= gray)
                            {
                                //e.Cell.BackColor = ColorTranslator.FromHtml("#EAE7E7"); //System.Drawing.Color.LightGray;
                            }
                            else if (Convert.ToDouble(e.CellValue) >= red)
                            {
                                e.Cell.BackColor = ColorTranslator.FromHtml("#FF765E");
                                e.Cell.ForeColor = Color.White;
                            }
                            else if (Convert.ToDouble(e.CellValue) >= green && Convert.ToDouble(e.CellValue) <= yellow)
                                e.Cell.BackColor = ColorTranslator.FromHtml("#FFFD78");
                            else
                                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                        }
                        
                        else if(!rbtnAvailability.Checked)
                        {
                            if (Convert.ToDouble(e.CellValue) <= gray)
                            {
                                //e.Cell.BackColor = ColorTranslator.FromHtml("#EAE7E7"); //System.Drawing.Color.LightGray;
                            }
                            else if (Convert.ToDouble(e.CellValue) >= red)
                            {
                                e.Cell.BackColor = System.Drawing.Color.Red;
                                e.Cell.ForeColor = Color.White;
                            }
                            else if (Convert.ToDouble(e.CellValue) >= green && Convert.ToDouble(e.CellValue) <= yellow)
                                e.Cell.BackColor = System.Drawing.Color.Yellow;
                            else
                                e.Cell.BackColor = System.Drawing.Color.Green;
                        }
                    }

                    if (hdndisplayMode.Value == DisplayMode.Daily.ToString())
                    {
                        if (Convert.ToDateTime(e.DataColumn.FieldName).ToString("ddd") == "Sat")
                        {
                            e.Cell.BackColor = ColorTranslator.FromHtml("#F2F0F0");
                            e.Cell.Text = "";
                        }
                        else if (Convert.ToDateTime(e.DataColumn.FieldName).ToString("ddd") == "Sun")
                        {
                            e.Cell.BackColor = ColorTranslator.FromHtml("#F2F0F0");
                            e.Cell.Text = "";
                        }
                    }
                }

                if (e.DataColumn.Caption == "# Active Projects" || e.DataColumn.Caption == "# Lifetime Projects")
                {
                    string value = UGITUtility.ObjectToString(e.CellValue);
                    var result = Convert.ToDouble(value);
                    if (rbtnAvailability.Checked && !string.IsNullOrEmpty(UGITUtility.ObjectToString(e.CellValue)))
                    {
                        if (jobTitle != null)
                        {
                            if (result <= jobTitle.LowProjectCapacity)
                            {
                                e.Cell.BackColor = ColorTranslator.FromHtml("#afe9fe");
                            }
                            else if (result > jobTitle.HighProjectCapacity)
                            {
                                e.Cell.BackColor = ColorTranslator.FromHtml("#fcf7b5");
                            }
                            else
                            {
                                e.Cell.BackColor = ColorTranslator.FromHtml("#FFBA62");
                            }
                        }
                    }
                }
                if (e.DataColumn.Caption == "$ Active" || e.DataColumn.Caption == "$ Lifetime")
                {
                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(e.CellValue)))
                    {
                        var result = Convert.ToDouble(UGITUtility.ObjectToString(e.CellValue));
                        e.Cell.Text = UGITUtility.FormatNumber(result, "currency");
                        if (rbtnAvailability.Checked)
                        {
                            string value = UGITUtility.ObjectToString(e.CellValue).Replace("$", "").Replace("M", "").Replace("K", "");

                            if (jobTitle != null)
                            {
                                if (result <= jobTitle.LowRevenueCapacity)
                                {
                                    e.Cell.BackColor = ColorTranslator.FromHtml("#afe9fe");
                                }
                                else if (result > jobTitle.HighRevenueCapacity)
                                {
                                    e.Cell.BackColor = ColorTranslator.FromHtml("#fcf7b5");
                                }
                                else
                                {
                                    e.Cell.BackColor = ColorTranslator.FromHtml("#FFBA62");
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                if (e.DataColumn.Caption != "#" && e.DataColumn.Caption != DatabaseObjects.Columns.RResource && e.DataColumn.Caption != "# Active Projects" && e.DataColumn.Caption != "$ Active" && e.DataColumn.Caption != "# Lifetime Projects" && e.DataColumn.Caption != "$ Lifetime")
                {
                    e.Cell.Text = string.Empty;

                    if (100 - Convert.ToDouble(e.CellValue) > 0)
                        e.Cell.Text = Convert.ToString(100 - Convert.ToDouble(e.CellValue));
                    else
                        e.Cell.Text = "0";

                }
            }

        }

        

        // Variables that store summary values.  
        //double ResourceFTE;
        //double ResourceTotalFTE;
        //int ResourceCount, tmpResourceCount = 0;

        protected void btnExportToPdf_Click(object sender, EventArgs e)
        {
            DataTable allocationData = GetAllocationData(true);

            for (int i = 0; i < allocationData.Columns.Count; i++)
            {
                if (allocationData.Columns[i].ColumnName != "ItemOrder"
                    && allocationData.Columns[i].ColumnName != "Role"
                    && allocationData.Columns[i].ColumnName != "ResourceQuantity"
                    && allocationData.Columns[i].ColumnName != "RevenueCapacity"
                    && allocationData.Columns[i].ColumnName != "ProjectCapacity")
                {
                    for (int rows = 0; rows < allocationData.Rows.Count; rows++)
                    {
                        string newValue = allocationData.Rows[rows][allocationData.Columns[i].ColumnName].ToString();

                        if (!string.IsNullOrWhiteSpace(newValue))
                        {
                            string colorValue = newValue.Split(new string[] { "background-color:" }, StringSplitOptions.None)[1].Split(';')[0];

                            newValue = Regex.Replace(newValue, "<.*?>|&.*?;", string.Empty);


                            allocationData.Rows[rows][allocationData.Columns[i].ColumnName] = newValue + ":" + colorValue;
                        }
                    }
                }
            }
            gvCapacityReport.DataSource = allocationData;
            gvCapacityReport.DataBind();
            gvCapacityReport.Columns["ItemOrder"].Visible = false;
            
            gridExporter.Landscape = true;
            gridExporter.PaperKind = System.Drawing.Printing.PaperKind.A4Plus;

            gridExporter.LeftMargin = 1;
            gridExporter.RightMargin = 0;
            gridExporter.TopMargin = -1;
            gridExporter.BottomMargin = -1;

            gridExporter.PageHeader.Font.Size = 15;
            gridExporter.PageHeader.Font.Name = "Arial";
            gridExporter.PageHeader.Center = "Capacity Planning";

            gridExporter.PageFooter.Center = "Page [Page # of Pages #]";
            gridExporter.PageFooter.Left = "[Date Printed]";

            gridExporter.WritePdfToResponse("Capacity Planning");
        }

        protected void gridExporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.Font = new Font("Calibri", 11f);
            if (e.VisibleIndex == -1)
                return;

            GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;
            var obj = gvCapacityReport.GetRow(e.VisibleIndex);

            if (e.RowType == GridViewRowType.Footer)
            {
                e.BrickStyle.TextAlignment = DevExPrinting.TextAlignment.MiddleCenter;
            }

            string strResourceAllocationColorPalete = ObjConfigurationVariableManager.GetValue(ConfigConstants.ResourceAllocationColorPalete);

            if (e.RowType == GridViewRowType.Data && dataColumn != null)
            {
                if (dataColumn.FieldName == DatabaseObjects.Columns.ResourceQuantity || dataColumn.FieldName == DatabaseObjects.Columns.ProjectCapacity)
                {
                    e.BrickStyle.TextAlignment = DevExPrinting.TextAlignment.MiddleCenter;
                }

                if (dataColumn.FieldName != DatabaseObjects.Columns.Resource && dataColumn.FieldName != DatabaseObjects.Columns.ProjectCapacity && dataColumn.FieldName != DatabaseObjects.Columns.RevenueCapacity && dataColumn.FieldName != DatabaseObjects.Columns.Role && dataColumn.FieldName != DatabaseObjects.Columns.ResourceQuantity && dataColumn.FieldName != DatabaseObjects.Columns.JobTitle && dataColumn.FieldName != DatabaseObjects.Columns.ItemOrder)
                {
                    for (DateTime dt = Convert.ToDateTime(dateFrom); Convert.ToDateTime(dateTo) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        if (dataColumn.FieldName == dt.ToString("MMM-dd-yy"))
                        {
                            string columValue = Convert.ToString(ds.Tables[0].Rows[e.VisibleIndex][dataColumn.FieldName]);

                            if (!string.IsNullOrWhiteSpace(columValue))
                            {
                                e.Text = columValue.Split(':')[0];
                                e.BrickStyle.TextAlignment = DevExPrinting.TextAlignment.MiddleCenter;

                                string backColor = columValue.Split(':')[1];

                                e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(backColor);
                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            }

                            //double cellValue = UGITUtility.StringToDouble(Convert.ToString(ds.Tables[0].Rows[e.VisibleIndex][dataColumn.FieldName]));
                            //if (!string.IsNullOrEmpty(strResourceAllocationColorPalete))
                            //{
                            //    Dictionary<string, string> cpResourceAllocationColorPalete = UGITUtility.GetCustomProperties(strResourceAllocationColorPalete, Constants.Separator);
                            //    if (cellValue >= orange)
                            //    {
                            //        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Orange]));
                            //        e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            //    }
                            //    else if (cellValue >= green && cellValue < orange)
                            //    {
                            //        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Green]));//Green
                            //        e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            //    }
                            //    else if (cellValue >= gray && cellValue < green)
                            //    {
                            //        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Gray]));//Gray
                            //        e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            //    }
                            //    else if (cellValue >= red && cellValue < gray)
                            //    {
                            //        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]));//Red
                            //        e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                            //    }
                            //    else
                            //        e.Text = e.Text;  //no color
                            //}
                            //    if (Convert.ToDouble(cellValue) <= gray)
                            //{
                            //    //e.Cell.BackColor = ColorTranslator.FromHtml("#EAE7E7"); //System.Drawing.Color.LightGray;
                            //}
                            //else if (Convert.ToDouble(cellValue) >= red)
                            //{
                            //    e.BrickStyle.BackColor = ColorTranslator.FromHtml("#FF765E");
                            //    e.BrickStyle.ForeColor = Color.White;
                            //}
                            //else if (Convert.ToDouble(cellValue) >= green && Convert.ToDouble(cellValue) <= yellow)
                            //    e.BrickStyle.BackColor = Color.Yellow; //ColorTranslator.FromHtml("#FFFD78");
                            //else
                            //    e.BrickStyle.BackColor = ColorTranslator.FromHtml("#90ee90"); //System.Drawing.Color.LightGreen;
                        }
                    }
                }
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvCapacityReport.DataSource = GetAllocationData(true);
            gvCapacityReport.DataBind();
            DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
            options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
            //gvPreview.ExportRenderBrick += GvPreview_ExportRenderBrick;

            gvCapacityReport.Columns["ItemOrder"].Visible = false;
            gridExporter.WriteXlsToResponse("Capacity Planning", options);
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                DataTable ResultData = GetAllocationData();
                gvCapacityReport.DataSource = ResultData;
                gvCapacityReport.DataBind();
                //gvCapacityReport.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.WYSIWYG;
                gvCapacityReport.ExportRenderBrick += GvCapacityReport_ExportRenderBrick;
                DevExPrinting.XlsxExportOptionsEx options = new DevExPrinting.XlsxExportOptionsEx();
                options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                ReportGenerationHelper reportHelper = new ReportGenerationHelper();

                //DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                //options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                //options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                XtraReport report = reportHelper.GenerateReport(gvCapacityReport, ResultData, "Capacity Planning", 6.75F);

                string fileName = string.Format("Capacity_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/content/images/ugovernit/upload/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail&type=queryReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }

        private void GvCapacityReport_ExportRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            
        }

        protected void mnuExportOptions_ItemClick(object source, MenuItemEventArgs e)
        {
            string strCommand = e.Item.ToString();

            DataTable allocationData = GetAllocationData(true);

            for (int i = 0; i < allocationData.Columns.Count; i++)
            {
                if (allocationData.Columns[i].ColumnName != "ItemOrder"
                    && allocationData.Columns[i].ColumnName != "Role"
                    && allocationData.Columns[i].ColumnName != "ResourceQuantity"
                    && allocationData.Columns[i].ColumnName != "RevenueCapacity"
                    && allocationData.Columns[i].ColumnName != "ProjectCapacity")
                {
                    for (int rows = 0; rows < allocationData.Rows.Count; rows++)
                    {
                        string newValue = allocationData.Rows[rows][allocationData.Columns[i].ColumnName].ToString();

                        if (!string.IsNullOrWhiteSpace(newValue))
                        {
                            string colorValue = newValue.Split(new string[] { "background-color:" }, StringSplitOptions.None)[1].Split(';')[0];

                            newValue = Regex.Replace(newValue, "<.*?>|&.*?;", string.Empty);


                            allocationData.Rows[rows][allocationData.Columns[i].ColumnName] = newValue + ":" + colorValue;
                        }
                    }
                }
            }

             gvCapacityReport.DataSource = allocationData;
            gvCapacityReport.DataBind();

            if (strCommand == "CSV")
            {
                CsvExportOptionsEx optionsEx = new CsvExportOptionsEx();
                optionsEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                optionsEx.TextExportMode = TextExportMode.Text;

                gvCapacityReport.Columns["ItemOrder"].Visible = false;
                gridExporter.WriteCsvToResponse("Capacity Planning", true, optionsEx);
            }
            else
            {
                DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                options.TextExportMode = TextExportMode.Text;

                gvCapacityReport.Columns["ItemOrder"].Visible = false;
                gridExporter.WriteXlsToResponse("Capacity Planning", options);
            }
        }

        protected void gvCapacityReport_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                //if (e.SummaryProcess == CustomSummaryProcess.Start)
                //{
                //    ResourceFTE = 0.0;
                //    ResourceTotalFTE = 0.0;
                //    ResourceCount = 0;
                //}

                //// Calculation. 
                //if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                //{
                //    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                //    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.JobTitle && item.FieldName != DatabaseObjects.Columns.Role && item.FieldName != DatabaseObjects.Columns.ResourceQuantity)
                //    {
                //        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "")
                //            ResourceFTE += UGITUtility.StringToDouble(Convert.ToString(e.FieldValue));
                //    }

                //    if (item.FieldName == DatabaseObjects.Columns.ResourceQuantity)
                //    {
                //        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceCount")
                //            ResourceCount += UGITUtility.StringToInt(Convert.ToString(e.FieldValue));
                //    }

                //}
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == DatabaseObjects.Columns.Resource || item.FieldName == DatabaseObjects.Columns.JobTitle || item.FieldName == DatabaseObjects.Columns.Role)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceItem" || ((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "RoleItem")
                            e.TotalValue = ""; //e.TotalValue = "Total Capacity (FTE)";
                        else
                        {
                            if (ddlCategory.SelectedItem.Value == "UnfilledRoles")
                            {
                                e.TotalValue = "No. of FTE Needed";
                            }
                            else
                            {
                                if (rbtnAvailability.Checked)
                                    e.TotalValue = "No. of FTE Available";
                                else
                                    e.TotalValue = "No. of FTE Allocated";
                            }
                        }
                    }

                    if (item.FieldName == DatabaseObjects.Columns.ResourceQuantity)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceCount")
                        {
                            if(ds.Tables.Count>0)
                            if (UGITUtility.IfColumnExists(item.FieldName, ds.Tables[1]))
                                e.TotalValue = ds.Tables[1].Rows[0][item.FieldName];
                        }
                        else
                            e.TotalValue = "";
                    }

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.JobTitle && item.FieldName != DatabaseObjects.Columns.ResourceQuantity && item.FieldName != DatabaseObjects.Columns.Role)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE")
                        {
                            if(ds.Tables.Count>0)
                            if (UGITUtility.IfColumnExists(item.FieldName, ds.Tables[1]))
                                e.TotalValue = ds.Tables[1].Rows[0][item.FieldName];
                        }
                        else
                        {
                            if(ds.Tables.Count>0)
                            if (UGITUtility.IfColumnExists(item.FieldName, ds.Tables[1]))
                                e.TotalValue = ds.Tables[1].Rows[0][item.FieldName];
                        }
                    }

                    //if (item.FieldName != DatabaseObjects.Columns.Resource &&  item.FieldName != DatabaseObjects.Columns.JobTitle && item.FieldName != DatabaseObjects.Columns.ResourceQuantity && item.FieldName != DatabaseObjects.Columns.Role)
                    //{
                    //    List<UserProfile> lstUProfile;
                    //    if (!string.IsNullOrEmpty(Convert.ToString(Request["pGlobalRoleID"])))
                    //        lstUProfile = profileManager.GetUsersByGlobalRoleID(peopleGlobalRoleID);   // UserProfile.GetGroupUsers(peopleGroupName, SPContext.Current.Web);
                    //    else if (!string.IsNullOrEmpty(Convert.ToString(Request["ticketId"])))
                    //    {
                    //        lstUProfile = new List<UserProfile>();
                    //        List<string> lstIDs = allResourceWorkItemsSPList.Where(x => x.WorkItemType == uHelper.getModuleNameByTicketId(Request["ticketId"]) && x.WorkItem == Request["ticketId"]).Select(x => x.Resource).ToList();
                    //        if (lstIDs != null)
                    //        {
                    //            lstUProfile.AddRange(userProfiles.Join(lstIDs, x => x.Id, y => y, (x, y) => new { profile = x }).Select(x => x.profile));
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (ddlResourceManager.SelectedValue != "0" && ddlResourceManager.SelectedValue != "")
                    //        {
                    //            lstUProfile = profileManager.GetUserByManager(Convert.ToString(ddlResourceManager.SelectedValue));  // UserProfile.LoadUsersByManagerId(Convert.ToInt32(ddlResourceManager.SelectedValue));
                    //            lstUProfile.Add(userProfiles.FirstOrDefault(x => x.Id == ddlResourceManager.SelectedValue));
                    //        }
                    //        else
                    //        {
                    //            lstUProfile = userProfiles;
                    //        }
                    //    }

                    //    foreach (UserProfile userProfile in lstUProfile)
                    //    {
                    //        if (!userProfile.Enabled)
                    //            continue;

                    //        //filter code.. for dropdowns.
                    //        if (divFilter.Visible)
                    //        {
                    //            if (ddlDepartment.GetValues() != null && ddlDepartment.GetValues() != string.Empty)
                    //            {
                    //                List<string> lstDepartments = UGITUtility.ConvertStringToList(ddlDepartment.GetValues(), Constants.Separator6);
                    //                if (!lstDepartments.Contains(Convert.ToString(userProfile.DepartmentId)))
                    //                    continue;

                    //                //if (userProfile.DepartmentId != Convert.ToInt32(ddlDepartment.GetValues()))
                    //                //    continue;
                    //            }

                    //            if (ddlFunctionalArea.SelectedValue != "0")
                    //            {
                    //                if (userProfile.FunctionalArea == null)
                    //                {
                    //                    continue;
                    //                }
                    //                else
                    //                {
                    //                    if (userProfile.FunctionalArea != Convert.ToInt32(ddlFunctionalArea.SelectedValue))
                    //                        continue;
                    //                }

                    //            }
                    //        }

                    //        if (userProfile.UGITStartDate < UGITUtility.StringToDateTime(item.FieldName) && userProfile.UGITEndDate > UGITUtility.StringToDateTime(item.FieldName))
                    //            ResourceTotalFTE++;

                    //    }

                    //    if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE")
                    //    {
                    //        e.TotalValue = Math.Round(ResourceTotalFTE, 2);
                    //    }
                    //    else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceCount")
                    //    {
                    //        e.TotalValue = ResourceCount;
                    //    }
                    //    else
                    //    {
                    //        if (rbtnAvailability.Checked)
                    //        {
                    //            e.TotalValue = (tmpResourceCount - Math.Round(ResourceFTE / 100, 2));
                    //        }
                    //        else
                    //            e.TotalValue = Math.Round(ResourceFTE / 100, 2);
                    //    }
                    //}
                }
            }
        }

        protected void gvCapacityReport_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void gvCapacityReport_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (ResourceCapacity != "true")//Request["ResourceCapacity"] == "true")
            {
                if (e.RowType == DevExpress.Web.GridViewRowType.Data)
                {
                    DataRow currentRow = gvCapacityReport.GetDataRow(e.VisibleIndex);
                    userP = profileManager.GetUserById(Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                    if (userP == null)
                        return;
                    //string absoluteRowUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}";
                    //absoluteRowUrlView = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "addworkitem", "Add Allocation", "ResourceAllocation", userP.Id, chkIncludeClosed.Checked));
                    //string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteRowUrlView, "Resource Utilization", Server.UrlEncode(Request.Url.AbsolutePath));

                    string label = string.Empty;
                    if (ddlCategory.SelectedItem.Value == "JobTitle")
                        label = string.Format("<div>{0}</div>", Convert.ToString(currentRow[DatabaseObjects.Columns.JobTitle]));
                    else if (ddlCategory.SelectedItem.Value == "Role" || ddlCategory.SelectedItem.Value == "UnfilledRoles")
                        label = string.Format("<div>{0}</div>", Convert.ToString(currentRow[DatabaseObjects.Columns.Role]));

                    if (currentRow != null && e.Row.Cells.Count > 1)
                    {
                        if (isResourceAdmin)
                        {
                            e.Row.Cells[1].Text = label;
                        }
                        else
                        {
                            string userId = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Id]);
                            if (userEditPermisionList != null && userEditPermisionList.Exists(x => x.Id == userId))
                            {

                                e.Row.Cells[1].Text = label;
                            }
                        }
                    }
                }
            }
        }

        protected void gvCapacityReport_DataBinding(object sender, EventArgs e)
        {
            if (gvCapacityReport.DataSource == null)
            {
                gvCapacityReport.DataSource = GetAllocationData();
            }
        }

        enum DisplayMode
        {
            Daily,
            Weekly,
            Monthly,
            HalfYearly,
            Yearly
        }
    }
}