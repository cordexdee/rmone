using DevExpress.CodeParser;
using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using DevExPrinting = DevExpress.XtraPrinting;

namespace uGovernIT.Web
{
    public partial class BenchTab : System.Web.UI.UserControl
    {
        #region variables

        public string FromDateCheck { get; set; }
        public string ToDateCheck { get; set; }
        public string ControlId { get; set; }
        public string TicketId { get; set; }
        public bool FilterMode { get; set; }
        public string FrameId;
        public bool ReadOnly;
        private string peopleGlobalRoleID=null;
        double green = 80, lightgreen = 15, yellow = 99, red = 100, gray = 0, orange = 120;
        private string formTitle = "Potential Allocations";
        //private string editParam = "CustomResourceAllocation";
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        DataTable resultTable;
        public bool btnexport;
        DataSet ds = new DataSet();
        public string ResourceCapacity { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        List<string> sActionType = new List<string>();
        protected DataTable resultedTable;

        protected bool isResourceAdmin = false;
        private string allowAllocationForSelf;
        protected List<UserProfile> userEditPermisionList = null;
        public DateTime currentStartDate;
        public DateTime currentEndDate;
        protected List<ResourceAllocationMonthly> dtMonthlyAllocation;
        protected DataTable dtRawDataAllResource;
        DataTable allocationData = null;
        protected List<ResourceWorkItems> allResourceWorkItemsSPList;
        
        DataSet dsData = new DataSet();
        protected bool enableDivision;
        protected bool enableFunctionalArea = false;
        protected DataSet dsFooter = null;
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        
        ResourceWorkItemsManager workitemManager = null;
        GlobalRoleManager roleManager = null;

        DepartmentManager departmentManagerLoad = null;
        UserProfileManager ObjUserProfileManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        
        private List<UserProfile> userProfiles = null;
        private List<Department> departmentsLoad = null;
        string hndYear = string.Empty;
        private DateTime dateFrom;
        private DateTime dateTo;
        private ResourceAllocationManager ResourceAllocationManager = null;
        private ResourceWorkItemsManager ResourceWorkItemsManager = null;
        ConfigurationVariableManager ConfigVariableMGR = null;
        DataTable dtFilterTypeData;
        private string selectedCategory = string.Empty;
        public string AllocationGanttURL = "/layouts/ugovernit/DelegateControl.aspx?control=allocationgantt";
        List<string> selectedTypes = new List<string>();
        private DataTable dataTable = null;
        List<UserProfile> lstActiveUsersIds = null;
        public bool IsCPRModuleEnabled
        {
            get
            {
                return uHelper.IsCPRModuleEnabled(applicationContext);
            }
        }

        public bool ShowClearFilter
        {
            get
            {
                return showClearFilter;
            }

            set
            {
                showClearFilter = value;
            }
        }

        public bool showClearFilter;

        public string AddCombineMultiAllocationUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&refreshpage=0";
        private int MonthColWidth = 60;
        List<string> lstEstimateColorsAndFontColors = null;
        List<string> lstAssignColorsAndFontColors = null;
        bool stopToRegerateColumns = false;
        private string selectedManager = string.Empty;
        public string SelectedUser = string.Empty;
        private string selecteddepartment = string.Empty;
        public string selectedUsers;
        private ModuleViewManager _moduleViewManager = null;
        private DataRow moduleRow;
        public string selectedDivision = string.Empty;
        public bool CalledFromDirectorView = false;
        public bool generateColumns = false;
        public bool isFilterApplying = false;
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(applicationContext);
                }
                return _moduleViewManager;
            }
        }

        List<GlobalRole> globalRoleData = new List<GlobalRole>();

        // Variables that store summary values.  
        //double ResourceFTE;
        //double ResourceTotalFTE;
        #endregion

        #region pageEvents
        protected override void OnInit(EventArgs e)
        {
            ObjUserProfileManager = new UserProfileManager(applicationContext);
            departmentManagerLoad = new DepartmentManager(applicationContext);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(applicationContext);
            ResourceAllocationManager = new ResourceAllocationManager(applicationContext);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(applicationContext);
            ConfigVariableMGR = new ConfigurationVariableManager(applicationContext);
            enableDivision = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableDivision);
            roleManager = new GlobalRoleManager(applicationContext);
            globalRoleData = uHelper.GetGlobalRoles(applicationContext, false);
            EditPermisionList();
            FillDropDownType();
            userProfiles = ObjUserProfileManager.GetUsersProfile().Where(x => !x.UserName.EqualsIgnoreCase("SuperAdmin")).ToList();
            //CurrentUser = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id));
            if (enableDivision)
            {
                departmentsLoad = departmentManagerLoad.GetDepartmentData();
            }
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
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (glType.DataSource == null)
            {
                glType.DataSource = dtFilterTypeData;
                glType.DataBind();
            }
            AddCombineMultiAllocationUrl = UGITUtility.GetAbsoluteURL(string.Format(AddCombineMultiAllocationUrl, "combinedmultiallocationjs", "Add Allocation", "ResourceAllocation"));

            UserProfile currentUserProfile = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id)); //HttpContext.Current.CurrentUser();


            ResourceCapacity = Request["IsResourceDrillDown"];

            
            dataTable = (DataTable)glType.DataSource;

            List<string> selectedActionType = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string sau in selectedActionType)
            {
                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sau);
                if (row != null)
                    sActionType.Add(Convert.ToString(row["LevelName"]));
                else
                    sActionType.Add(sau);
            }

            showClearFilter = IsFilterOn();

            string ControlName = uHelper.GetPostBackControlId(this.Page);

            if (ControlName == "gv")
                UGITUtility.CreateCookie(Response, "filter", string.Format("type${0}", glType.Text));

            if (ControlName == "ddlDepartment")
            {
                UGITUtility.CreateCookie(Response, "filterDepartment", string.Format("department${0}", ddlDepartment.GetValues()));
                UGITUtility.CreateCookie(Response, "filterFunctionArea", string.Format("functionarea${0}", "0"));
            }
            else if (ControlName == "userGroupGridLookup")
            {
                // UGITUtility.CreateCookie(Response, "filter", string.Format("group${0}~#type${1}", ddlUserGroup.SelectedValue, selectedCategory));
                hdnGenerateColumns.Value = "1";
                generateColumns = true;
            }

            if (ControlName == "ddlResourceManager")
            {
                //UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", ddlResourceManager.SelectedValue));
                UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value))));
            }

            if (!Page.IsPostBack)
            {
                if (currentUserProfile != null && !string.IsNullOrEmpty(currentUserProfile.Department))
                {
                    if (enableDivision)
                    {
                        long deptartmentID = 0;
                        deptartmentID = Convert.ToInt64(currentUserProfile.Department);
                        var div = departmentsLoad.FirstOrDefault(x => x.TenantID == currentUserProfile.TenantID && x.ID == deptartmentID).DivisionIdLookup;

                        currentUserProfile.Division = Convert.ToString(div);
                        if (currentUserProfile.Division != null)
                        {
                            UGITUtility.DeleteCookie(Request, Response, "division");
                            if (!currentUserProfile.IsManager)
                            {
                                SetCookie("division", Convert.ToString(currentUserProfile.Division));
                                hdnaspDepartment.Value = currentUserProfile.Department;
                                ddlDepartment.SetValues(currentUserProfile.Department);
                            }
                            else
                            {
                                UGITUtility.DeleteCookie(Request, Response, "filterResource");
                                UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", currentUserProfile.Id));
                                hdnaspDepartment.Value = "";
                                ddlDepartment.SetValues("");
                                ddlDepartment.SetText("All");
                            }
                        }
                    }
                    else
                    {
                        ddlDepartment.Value = currentUserProfile.Department;
                    }
                    LoadRolesOnDepartmentAndFunctional(currentUserProfile.Department);
                    LoadDdlResourceManager("", currentUserProfile.IsManager ? currentUserProfile.Id : "");
                }
                else
                {
                    if (enableDivision)
                    {
                        UGITUtility.DeleteCookie(Request, Response, "division");
                        SetCookie("division", "");
                    }
                    UGITUtility.DeleteCookie(Request, Response, "filterResource");
                    UGITUtility.CreateCookie(Response, "filterResource", string.Format("user${0}", currentUserProfile.Id));
                    hdnaspDepartment.Value = "";

                    ddlDepartment.SetText("All");
                    LoadRolesOnDepartmentAndFunctional("");
                    LoadDdlResourceManager("", currentUserProfile?.IsManager ?? false ? currentUserProfile.Id : "");
                    if (currentUserProfile != null && currentUserProfile.IsManager && currentUserProfile.TenantID == applicationContext.TenantID)
                    {
                        ddlDepartment.SetValues("");
                    }
                }

            }

            if (!IsPostBack)
            {
                divProject.Visible = false;
                divFilter.Visible = true;
                string defaultDisplayMode = ConfigVariableMGR.GetValue(ConfigConstants.DefaultRMMDisplayMode);
                
                if (defaultDisplayMode == "Monthly")
                {
                   
                    hdndtfrom.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 1, 1));
                    FromDateCheck = Convert.ToString(new DateTime(DateTime.Now.Year, 1, 1));
                    hdndtto.Value = Convert.ToString(new DateTime(DateTime.Now.Year, 12, 31));
                    ToDateCheck = Convert.ToString(new DateTime(DateTime.Now.Year, 12, 31));
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
                    hdndtfrom.Value = FromDateCheck = firstMonday.ToShortDateString();
                    hdndtto.Value = ToDateCheck = lastDayOfNextMonth.ToShortDateString();
                    hdndisplayMode.Value = DisplayMode.Weekly.ToString();
                    hdnSelectedDate.Value = firstMonday.ToShortDateString();
                }


                LoadDepartment();
                loadFunctions();

                if (Request["IsResourceDrillDown"] == "true")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["pDepartmentName"])))
                    {
                        ddlDepartment.SetValues(Convert.ToString(Request["pDepartmentName"]));
                        hdnaspDepartment.Value = Convert.ToString(Request["pDepartmentName"]);
                    }

                    //LoadFunctionalArea();
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
                            //ddlResourceManager.SelectedValue = Vals[1];
                            if (cmbResourceManager.Items.FindByValue(Vals[1]) != null)
                                cmbResourceManager.Items.FindByValue(Vals[1]).Selected = true;
                            //cmbResourceManager.Value = Vals[1];
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

                    

                    #endregion
                }

                if (UGITUtility.ObjectToString(Request["IsChartDrillDown"]) == "true")
                {
                    hdnaspDepartment.Value = "";
                }

                string filterCountPercentageFTE = UGITUtility.GetCookieValue(Request, "filtercountpercentagefte");
                if (string.IsNullOrWhiteSpace(filterCountPercentageFTE) && Request["AllocationViewType"] == "ProjectAllocation")
                {
                    filterCountPercentageFTE = "fte";
                }

                if (filterCountPercentageFTE == "hrs")
                    rbtnHrs.Checked = true;
                else if (filterCountPercentageFTE == "percentage")
                    rbtnPercentage.Checked = true;
                else if (filterCountPercentageFTE == "availability")
                    rbtnAvailability.Checked = true;
                else if (filterCountPercentageFTE == "count")
                    rbtnItemCount.Checked = true;
                else
                    rbtnFTE.Checked = true;

                string allfilter = UGITUtility.GetCookieValue(Request, "filterAll");

                if (allfilter == "all")
                {
                    chkAll.Checked = true;
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

                if (userGroupGridLookup.DataSource == null)
                {
                    LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value);
                }
                if (!string.IsNullOrWhiteSpace(Request["GlobalRoleId"]))
                {
                    userGroupGridLookup.Value = UGITUtility.ObjectToString(Request["GlobalRoleId"]);
                    hdnSelectedGroup.Value = UGITUtility.ObjectToString(Request["GlobalRoleId"]);
                    UGITUtility.CreateCookie(Response, "filter", string.Format("group-{0}#type-{1}", UGITUtility.ObjectToString(Request["GlobalRoleId"]), glType.Text));
                }
            }

            hdnTypeLoader.Value = glType.Text;


            

            HttpCookie cookie = Request.Cookies["roleid"];
            if (cookie != null)
            {
                //if (ddlUserGroup.Items.FindByValue(cookie.Value) != null && cookie.Value != "")
                //    ddlUserGroup.Items.FindByValue(cookie.Value).Selected = true;

                //if (cookie.Value == "0")
                //    ddlUserGroup.ClearSelection();
            }
            if (!IsPostBack)
            {
                UGITUtility.CreateCookie(Response, "IsMultiUserSelected", "0");
                UGITUtility.CreateCookie(Response, "IsMulti", "0");
                PrepareAllocationGrid();
            }
            if (UGITUtility.GetCookieValue(Request, "IsMulti") == "1")
            {
                PrepareAllocationGrid();
                UGITUtility.CreateCookie(Response, "IsMulti", "0");
            }
            if (Request["IsResourceDrillDown"] == "true")
                chkAll.Checked = true;

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
        }

        private void EditPermisionList()
        {
            isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || ObjUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            //allowAllocationForSelf = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
            allowAllocationForSelf = ObjConfigurationVariableManager.GetValue(ConfigConstants.AllowAllocationForSelf);
            //allowAllocationViewAll = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);

            if (!isResourceAdmin)
            {
                if (allowAllocationForSelf.EqualsIgnoreCase("Edit") && HttpContext.Current.CurrentUser().IsManager)
                    userEditPermisionList = ObjUserProfileManager.LoadAuthorizedUsers(true);
                else if (allowAllocationForSelf.EqualsIgnoreCase("Edit") || allowAllocationForSelf.EqualsIgnoreCase("View"))
                    userEditPermisionList = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf);

                //if (userEditPermisionList != null)
                //    UserList = string.Join(",", userEditPermisionList.Select(x => x.Id).ToList());

                //if (allowAllocationForSelf.EqualsIgnoreCase("NoView"))
                //    UserList = "000";
            }
            //else
            //    UserList = string.Empty;
        }

        protected override void OnPreRender(EventArgs e)
        {
            gvResourceAvailablity.DataBind();
            base.OnPreRender(e);
        }

        #endregion

        #region gridevent
        protected void gvResourceAvailablity_DataBinding(object sender, EventArgs e)
        {
            if (gvResourceAvailablity.DataSource == null)
            {
                DataTable dtNewTable = GetAllocationData();
                if (gvResourceAvailablity.Columns.Count > 0)
                {
                    string ordercolumn = Convert.ToString(((GridViewDataColumn)gvResourceAvailablity.Columns[DatabaseObjects.Columns.RResource]).SortOrder);

                    if (ordercolumn.Contains("Asc"))
                    {
                        dtNewTable.DefaultView.Sort = "ResourceUser ASC";
                    }
                    else if (ordercolumn.Contains("Des"))
                    {
                        dtNewTable.DefaultView.Sort = "ResourceUser DESC";
                    }

                    dtNewTable = dtNewTable.DefaultView.ToTable();
                }

                if (rbtnOverstaffedonly.Checked)
                {
                    dtNewTable.DefaultView.Sort = " AvgerageUtil DESC";
                }
                else
                {
                    dtNewTable.DefaultView.Sort = "AvgerageUtil ASC";
                }

                dtNewTable = dtNewTable.DefaultView.ToTable();

                gvResourceAvailablity.DataSource = dtNewTable;
            }

            if (generateColumns)
            {
                PrepareAllocationGrid();
                hdnGenerateColumns.Value = "0";
                generateColumns = false;
            }
            //SetGridSorting();
        }

        #endregion

        #region helper method

        public void SetGridSorting()
        {
            if ((!IsPostBack || rbtnBenchonly.Checked) && gvResourceAvailablity?.Columns?.Count > 0)
            {
                gvResourceAvailablity.ClearSort();
                gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Ascending);
                rbtnBenchonly.Checked = true;
            }
        }
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

        private DataTable SummarizeDataTable(DataTable dtStaffSheet, string ColToSum)
        {
            var rows = dtStaffSheet.AsEnumerable();
            var columns = dtStaffSheet.Columns.Cast<DataColumn>();
            string columnToGroup = ColToSum;
            DataColumn colToGroup = columns.First(c => c.ColumnName.Equals(columnToGroup, StringComparison.OrdinalIgnoreCase));
            var colsToSum = columns.Where(c => c != colToGroup && c.Caption != "ItemOrder" && c.Caption != "Id" && c.Caption != "FullAllocation" && c.ColumnName != "ProjectCapacity" && c.ColumnName != "RevenueCapacity");
            var columnsToSum = new HashSet<DataColumn>(colsToSum);

            resultTable = dtStaffSheet.Clone(); // empty table, same schema
            foreach (var group in rows.GroupBy(r => r[colToGroup]))
            {
                DataRow row = resultTable.Rows.Add();
                foreach (var col in columns)
                {
                    if (columnsToSum.Contains(col))
                    {
                        if (col.ColumnName != "Id" && col.ColumnName != "ItemOrder" && col.ColumnName != "Resource" && col.ColumnName != "ProjectCapacity" && col.ColumnName != "RevenueCapacity")
                        {
                            double sum = group.Sum(r => r.Field<string>(col) == null ? 0 : Convert.ToDouble(r.Field<string>(col)));
                            row[col.ColumnName] = Convert.ToDouble(sum); //sum.ToString("N2");
                        }
                    }
                    else
                        row[col.ColumnName] = group.First()[col];
                }
            }
            return resultTable;
        }
        #endregion

        #region GetData
        private DataTable GetAllocationData()
        {
            UserProfile currentUserProfile = HttpContext.Current.CurrentUser();
            string type = string.Empty;
            DataTable data = new DataTable();
            string AllocationType = string.Empty;
            List<ResourceWorkItems> lstRWorkItem = null;
            List<UserProfile> lstUProfile;

            // Code to check for empty hidden values, when switching between #, % ,FTE, Availability radio buttons & assigning values from cookies
            if (hdndisplayMode.Value == "")
            {
                hdndisplayMode.Value = UGITUtility.GetCookieValue(Request, "RAdisplayMode");
                if (string.IsNullOrEmpty(hdndisplayMode.Value))
                    hdndisplayMode.Value = DisplayMode.Weekly.ToString();
            }
            if (hdndtfrom.Value == "")
            {
                hdndtfrom.Value = UGITUtility.GetCookieValue(Request, "RAdtfrom");
                if (string.IsNullOrEmpty(hdndtfrom.Value))
                    hdndtfrom.Value = new DateTime(DateTime.Now.Year, 1, 1).ToString();
                FromDateCheck = new DateTime(DateTime.Now.Year, 1, 1).ToString();

            }
            if (hdndtto.Value == "")
            {
                hdndtto.Value = UGITUtility.GetCookieValue(Request, "RAdtto");
                if (string.IsNullOrEmpty(hdndtto.Value))
                    hdndtto.Value = new DateTime(DateTime.Now.Year, 12, 31).ToString();
                ToDateCheck = new DateTime(DateTime.Now.Year, 12, 31).ToString();
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
                type = "HRS";
            else
                type = "COUNT";

            string managerUser = (!string.IsNullOrEmpty(Convert.ToString(cmbResourceManager.Value))) ? Convert.ToString(cmbResourceManager.Value) : "";

            List<string> selUsers = new List<string>();

            string roleID = "";// ddlUserGroup.SelectedValue != "0" && ddlUserGroup.SelectedValue != "" ? ddlUserGroup.SelectedValue : "";
            if (userGroupGridLookup?.GridView?.GetSelectedFieldValues("Id")?.Count > 0)
            {
                roleID = string.Join(",", userGroupGridLookup.GridView.GetSelectedFieldValues("Id"));
            }

            string functionID = "";
            if (functionGridLookup?.GridView?.GetSelectedFieldValues("ID")?.Count > 0)
            {
                functionID = string.Join(",", functionGridLookup.GridView.GetSelectedFieldValues("ID"));
            }

            if (hdndisplayMode.Value == "Monthly")
            {
                if (managerUser == "")
                {
                    cmbResourceManager.Value = string.Empty; cmbResourceManager.SelectedIndex = 0;
                    cmbResourceManager.Items.FindByValue("0").Selected = true;
                }

                string allResources = string.Empty;
                string allClosed = string.Empty;
                if ((Convert.ToString(chkAll.Checked) == "True") || (Convert.ToString(chkAll.Checked) == "1"))
                {
                    allResources = "True";
                }
                else
                {
                    allResources = "False";
                }

                if ((Convert.ToString(chkIncludeClosed.Checked) == "True") || (Convert.ToString(chkAll.Checked) == "1"))
                {
                    allClosed = "True";
                }
                else
                {
                    allClosed = "False";
                }

                string levelName = string.Join(",", sActionType);
                string yearMonthly = string.Empty;

                if (!string.IsNullOrEmpty(hdndtfrom.Value))
                {
                    yearMonthly = Convert.ToDateTime(hdndtfrom.Value).ToString("yyyy");
                }

                Dictionary<string, object> RUValues = new Dictionary<string, object>();
                RUValues.Add("@TenantID", applicationContext.TenantID);
                RUValues.Add("@IncludeAllResources", "1"); //allResources
                RUValues.Add("@IncludeClosedProject", allClosed);
                RUValues.Add("@DisplayMode", type);  //FTE, COUNT, PERCENT, AVAILABILITY
                RUValues.Add("@Department", hdnaspDepartment.Value == "undefined" ? string.Empty : hdnaspDepartment.Value.TrimEnd(','));
                RUValues.Add("@StartDate", Convert.ToDateTime(hdndtfrom.Value).ToString("MM-dd-yyyy"));
                RUValues.Add("@EndDate", Convert.ToDateTime(hdndtto.Value).ToString("MM-dd-yyyy"));
                RUValues.Add("@ResourceManager", string.IsNullOrEmpty(managerUser) ? "0" : managerUser);
                RUValues.Add("@AllocationType", AllocationType);
                RUValues.Add("@LevelName", levelName);
                RUValues.Add("@GlobalRoleId", roleID);
                RUValues.Add("@Mode", UGITUtility.ObjectToString(hdndisplayMode.Value));
                RUValues.Add("@ShowAvgColumns", CalledFromDirectorView? "1" : "0");
                RUValues.Add("@Function", functionID);
                RUValues.Add("@SoftAllocation", chkIncludeSoftAllocations.Checked);
                RUValues.Add("@OnlyNCO", chkOnlyNCO.Checked);
                DataTable dataTable = new DataTable();
                dsData = GetTableDataManager.GetDataSet("ResourceUtilzation_BenchTab", RUValues);
                dataTable = dsData.Tables[0];
                data = dataTable;
            }
            else
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", applicationContext.TenantID);
                values.Add("@Mode", hdndisplayMode.Value);
                values.Add("@StartDate", Convert.ToDateTime(hdndtfrom.Value).ToString("MM-dd-yyyy"));
                values.Add("@EndDate", Convert.ToDateTime(hdndtto.Value).ToString("MM-dd-yyyy"));
                values.Add("@Department", hdnaspDepartment.Value == "undefined" ? string.Empty : hdnaspDepartment.Value.TrimEnd(','));
                values.Add("@ResourceManager", managerUser == string.Empty ? "0" : managerUser);
                values.Add("@DisplayMode", type);
                values.Add("@IncludeAllResources", "1"); // chkAll.Checked
                values.Add("@IncludeClosedProject", chkIncludeClosed.Checked);
                values.Add("@AllocationType", AllocationType);
                values.Add("@LevelName", string.Join(",", sActionType));
                values.Add("@Function", functionID);
                values.Add("@GlobalRoleId", roleID);
                values.Add("@ShowAvgColumns", CalledFromDirectorView ? "1" : "0");
                values.Add("@SoftAllocation", chkIncludeSoftAllocations.Checked);
                values.Add("@OnlyNCO", chkOnlyNCO.Checked);
                //values.Add("@Function", functionID);

                if (btnexport)
                {
                    ds = GetTableDataManager.GetDataSet("ResourceUtilzationweekly_AllocationHrs", values);
                    dsData = ds;
                    //string[] resourceNameHtml;
                    //string[] resourceName;
                    //string resource;
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    resourceNameHtml = ds.Tables[0].Rows[i]["ResourceUser"].ToString().Split('>');
                    //    resourceName = resourceNameHtml[1].Split('<');
                    //    resource = resourceName[0];
                    //    ds.Tables[0].Rows[i]["ResourceUser"] = resource;
                    //    ds.AcceptChanges();
                    //}
                }
                else
                {
                    dsData = GetTableDataManager.GetDataSet("ResourceUtilzationweekly_AllocationHrs", values);
                }

                if (dsData.Tables.Count > 0)
                    data = dsData.Tables[0];

                if (!string.IsNullOrEmpty(hdnaspDepartment.Value) || !string.IsNullOrEmpty(roleID) || managerUser != "0")
                {
                    if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsData.Tables[0].Rows.Count; j++)
                        {
                            selUsers.Add(Convert.ToString(dsData.Tables[0].Rows[j]["Id"]));
                        }
                    }
                }

                if (selUsers.Count > 0)
                {
                    string[] selusers = selUsers.ToArray();
                    selectedUsers = string.Join(", ", selusers);
                    SetCookie("GridUsers", selectedUsers);
                }
                else
                {
                    SetCookie("GridUsers", "");
                }
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Request["pGlobalRoleID"]))) // load group users
            {
                lstUProfile = ObjUserProfileManager.GetUsersByGlobalRoleID(peopleGlobalRoleID);
            }
            else if (!string.IsNullOrEmpty(Convert.ToString(Request["ticketId"])))
            {
                lstUProfile = new List<UserProfile>();
                string typeName = uHelper.getModuleNameByTicketId(Request["ticketId"]);
                lstRWorkItem = workitemManager.LoadWorkItemsById(typeName, Request["ticketId"], null);
                if (lstRWorkItem != null)
                {
                    UserProfile newlstitem = null;
                    List<string> lstIDs = lstRWorkItem.Select(x => x.Resource).Distinct().ToList();
                    foreach (string id in lstIDs)
                    {
                        newlstitem = ObjUserProfileManager.LoadById(id);
                        if (newlstitem != null)
                            lstUProfile.Add(newlstitem);
                    }
                }
            }
            else
            {
                if (Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)) != "0" && Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)) != "")
                {
                    lstUProfile = ObjUserProfileManager.GetUserByManager(Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)));

                    UserProfile newlstitem = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id == Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)));
                    if (newlstitem != null)
                        lstUProfile.Add(newlstitem);
                }
                else
                {
                    lstUProfile = ObjUserProfileManager.GetUsersProfile();
                }

                if (userGroupGridLookup?.GridView?.GetSelectedFieldValues("Id")?.Count > 0)
                {
                    List<string> gIds =  userGroupGridLookup.GridView.GetSelectedFieldValues("Id").Select(x =>(string)x).ToList();
                    List<UserProfile> lstGroupUsers = ObjUserProfileManager.GetUsersProfile().Where(x => gIds.Contains(x.GlobalRoleId)).ToList();
                    lstUProfile = lstGroupUsers.FindAll(x => lstUProfile.Any(y => y.Id == x.Id));
                }
            }

            //GetExcessCapacityRow(dsData);
            GetCountRows(dsData);
            return data;
        }

        private void GetCountRows(DataSet dsBenchTab)
        {
            string displaymode = rbtnHrs.Checked == true ? "H" : rbtnItemCount.Checked == true ? "C" : rbtnPercentage.Checked == true ? "P" : rbtnFTE.Checked == true ? "F" : "A";

            DataTable dtBenchCostTbl = new DataTable();
            dtBenchCostTbl.Columns.Add("", typeof(string));
            dtBenchCostTbl.Columns.Add("EC", typeof(string));
            dtBenchCostTbl.Columns.Add("BenchCost", typeof(string));
            dtBenchCostTbl.Rows.Add();

            DataTable dtRedCountTbl = new DataTable();
            dtRedCountTbl.Columns.Add("", typeof(string));
            dtRedCountTbl.Columns.Add("EC", typeof(string));
            dtRedCountTbl.Columns.Add("RedCount", typeof(string));
            dtRedCountTbl.Rows.Add();

            DataTable dtYellowCountTbl = new DataTable();
            dtYellowCountTbl.Columns.Add("", typeof(string));
            dtYellowCountTbl.Columns.Add("EC", typeof(string));
            dtYellowCountTbl.Columns.Add("YellowCount", typeof(string));
            dtYellowCountTbl.Rows.Add();

            DataTable dtGreenCountTbl = new DataTable();
            dtGreenCountTbl.Columns.Add("", typeof(string));
            dtGreenCountTbl.Columns.Add("EC", typeof(string));
            dtGreenCountTbl.Columns.Add("GreenCount", typeof(string));
            dtGreenCountTbl.Rows.Add();

            DataTable dtSilverCountTbl = new DataTable();
            dtSilverCountTbl.Columns.Add("", typeof(string));
            dtSilverCountTbl.Columns.Add("EC", typeof(string));
            dtSilverCountTbl.Columns.Add("SilverCount", typeof(string));
            dtSilverCountTbl.Rows.Add();

            double totalHoursInDay = uHelper.GetWorkingHoursInADay(applicationContext);
            if (dsBenchTab != null && dsBenchTab.Tables.Count == 2)
            {
                if (hdndisplayMode.Value == "Monthly")
                {
                    foreach (DataRow dr in dsBenchTab.Tables[0].Rows)
                    {
                        int monthCount = 1;
                        UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Id])));
                        double hourlyRate = UGITUtility.StringToDouble(dsBenchTab.Tables[1].AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.Id) == UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Id]))?["EmpCostRate"] ?? 0);
                        while (monthCount < 13)
                        {
                            DateTime AllocationMonthStartDate = new DateTime(Convert.ToDateTime(hdndtfrom.Value).Year, monthCount, 1);
                            double totalhours = totalHoursInDay * uHelper.GetTotalWorkingDaysBetween(applicationContext, AllocationMonthStartDate, AllocationMonthStartDate.AddMonths(1).AddDays(-1));

                            if (dsBenchTab.Tables[0].Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                            {
                                double cellvalue = UGITUtility.StringToDouble(GetValueFromInput(dr[AllocationMonthStartDate.ToString("MMM-dd-yy")].ToString(), "P"));
                                double allocHour = UGITUtility.StringToDouble(GetValueFromInput(dr[AllocationMonthStartDate.ToString("MMM-dd-yy")].ToString(), "H"));
                                if (!dtBenchCostTbl.Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtBenchCostTbl.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtBenchCostTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (totalhours - allocHour > 0)
                                {
                                    dtBenchCostTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtBenchCostTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + ((totalhours - allocHour) * hourlyRate);
                                }

                                if (!dtYellowCountTbl.Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtYellowCountTbl.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtYellowCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (!dtGreenCountTbl.Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtGreenCountTbl.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtGreenCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (!dtRedCountTbl.Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtRedCountTbl.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtRedCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (!dtSilverCountTbl.Columns.Contains(AllocationMonthStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtSilverCountTbl.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtSilverCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = 0;
                                }

                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(dr[AllocationMonthStartDate.ToString("MMM-dd-yy")])))
                                {
                                    if (cellvalue >= orange)
                                    {
                                        dtYellowCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtYellowCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Orange
                                    }
                                    else if (cellvalue >= green && cellvalue < orange)
                                    {
                                        dtGreenCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtGreenCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Green
                                    }
                                    else if (cellvalue >= red && cellvalue < gray)
                                    {
                                        dtRedCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtRedCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Red
                                    }
                                    else if(cellvalue >= 0)
                                    {
                                        dtSilverCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtSilverCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + 1;
                                    }
                                }
                                else
                                {
                                    dtRedCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtRedCountTbl.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) + 1;
                                    //Red
                                }
                            }

                            monthCount++;
                        }
                    }
                }

                if (hdndisplayMode.Value == "Weekly")
                {
                    foreach (DataRow dr in dsBenchTab.Tables[0].Rows)
                    {
                        DateTime startDate = Convert.ToDateTime(hdndtfrom.Value); // Assuming this is the start date
                        DateTime endDate = startDate.AddMonths(3); // Assuming we want to iterate over three months
                        UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Id])));
                        double hourlyRate = UGITUtility.StringToDouble(dsBenchTab.Tables[1].AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.Id) == UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Id]))?["EmpCostRate"] ?? 0);
                        while (startDate < endDate)
                        {
                            DateTime weekStartDate = startDate;
                            DateTime weekEndDate = startDate.AddDays(7); // Assuming weeks start on Sunday and end on Saturday
                            double totalhours = totalHoursInDay * uHelper.GetTotalWorkingDaysBetween(applicationContext, weekStartDate, weekEndDate.AddDays(-1));

                            if (dsBenchTab.Tables[0].Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                            {
                                double allocHour = UGITUtility.StringToDouble(GetValueFromInput(dr[weekStartDate.ToString("MMM-dd-yy")].ToString(), "H"));
                                double cellvalue = UGITUtility.StringToDouble(GetValueFromInput(dr[weekStartDate.ToString("MMM-dd-yy")].ToString(), "P"));
                                if (!dtBenchCostTbl.Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtBenchCostTbl.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtBenchCostTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (totalhours - allocHour > 0)
                                {
                                    dtBenchCostTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtBenchCostTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + ((totalhours - allocHour) * hourlyRate);
                                }

                                if (!dtYellowCountTbl.Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtYellowCountTbl.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtYellowCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (!dtGreenCountTbl.Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtGreenCountTbl.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtGreenCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = 0;
                                }
                                if (!dtRedCountTbl.Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtRedCountTbl.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtRedCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = 0;
                                }

                                if (!dtSilverCountTbl.Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                                {
                                    dtSilverCountTbl.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                    dtSilverCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = 0;
                                }

                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(dr[weekStartDate.ToString("MMM-dd-yy")])))
                                {
                                    
                                    if (cellvalue >= orange)
                                    {
                                        dtYellowCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtYellowCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Orange
                                    }
                                    else if (cellvalue >= green && cellvalue < orange)
                                    {
                                        
                                        dtGreenCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtGreenCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Green
                                    }
                                    else if (cellvalue >= red && cellvalue < gray)
                                    {
                                        dtRedCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtRedCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + 1;
                                        //Red
                                    }
                                    else if (cellvalue >= gray && cellvalue < green)
                                    {
                                        dtSilverCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtSilverCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + 1;
                                    }
                                    
                                }
                                else
                                {
                                    dtRedCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(dtRedCountTbl.Rows[0][weekStartDate.ToString("MMM-dd-yy")]) + 1;
                                    //Red
                                }
                            }
                            // Move to the next week
                            startDate = startDate.AddDays(7);
                        }
                    }
                }
                dtRedCountTbl.Rows[0]["RedCount"] = "RedCount";
                dtYellowCountTbl.Rows[0]["YellowCount"] = "YellowCount";
                dtGreenCountTbl.Rows[0]["GreenCount"] = "GreenCount";
                dtSilverCountTbl.Rows[0]["SilverCount"] = "SilverCount";
            }

            dsData.Tables.Add(dtRedCountTbl);
            dsData.Tables.Add(dtYellowCountTbl);
            dsData.Tables.Add(dtGreenCountTbl);
            dsData.Tables.Add(dtBenchCostTbl);
            dsData.Tables.Add(dtSilverCountTbl);
        }
        
        private void GetExcessCapacityRow(DataSet dsBenchTab)
        {
            DataTable dtExecessCapacity = new DataTable();
            try
            {
                dtExecessCapacity.Columns.Add("", typeof(string));
                dtExecessCapacity.Columns.Add("EC", typeof(string));
                dtExecessCapacity.Columns.Add("ExcessCapacity", typeof(string));
                dtExecessCapacity.Rows.Add();
                if (dsBenchTab != null && dsBenchTab.Tables.Count == 5)
                {
                    if (hdndisplayMode.Value == "Monthly")
                    {
                        int monthCount = 1;
                        
                        while (monthCount < 13)
                        {
                            DateTime AllocationMonthStartDate = new DateTime(Convert.ToDateTime(hdndtfrom.Value).Year, monthCount, 1);
                            double totalEccessCapacity = UGITUtility.StringToInt(dsBenchTab.Tables[2].Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]) 
                                                                - (UGITUtility.StringToInt(dsBenchTab.Tables[1].Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")])
                                                                + UGITUtility.StringToInt(dsBenchTab.Tables[3].Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]));
                                                                
                            dtExecessCapacity.Columns.Add(AllocationMonthStartDate.ToString("MMM-dd-yy"), typeof(double));
                            dtExecessCapacity.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(totalEccessCapacity);

                            monthCount++;
                        }
                        dtExecessCapacity.Rows[0]["ExcessCapacity"] = "Excess Capacity";

                    }
                    if (hdndisplayMode.Value == "Weekly")
                    {
                        DateTime startDate = Convert.ToDateTime(hdndtfrom.Value); // Assuming this is the start date
                        DateTime endDate = startDate.AddMonths(3); // Assuming we want to iterate over three months

                        while (startDate < endDate)
                        {
                            DateTime weekStartDate = startDate;
                            DateTime weekEndDate = startDate.AddDays(7); // Assuming weeks start on Sunday and end on Saturday

                            if (dsBenchTab.Tables[2].Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                            {
                                double totalEccessCapacity = UGITUtility.StringToInt(dsBenchTab.Tables[2].Rows[0][weekStartDate.ToString("MMM-dd-yy")])
                                                                    - (UGITUtility.StringToInt(dsBenchTab.Tables[1].Rows[0][weekStartDate.ToString("MMM-dd-yy")])
                                                                    + UGITUtility.StringToInt(dsBenchTab.Tables[3].Rows[0][weekStartDate.ToString("MMM-dd-yy")]));

                                dtExecessCapacity.Columns.Add(weekStartDate.ToString("MMM-dd-yy"), typeof(double));
                                dtExecessCapacity.Rows[0][weekStartDate.ToString("MMM-dd-yy")] = UGITUtility.StringToDouble(totalEccessCapacity);
                            }
                            // Move to the next week
                            startDate = startDate.AddDays(7);
                        }

                        dtExecessCapacity.Rows[0]["ExcessCapacity"] = "Excess Capacity";
                    }
                }
            }catch(Exception ex)
            {
                ULog.WriteException("Method GetExcessCapacity: " + ex.Message);
            }
            dsData.Tables.Add(dtExecessCapacity);
        }
        private void PrepareAllocationGrid()
        {
            if (isFilterApplying == false)
            {
                //string dept = Convert.ToString(hdnaspDepartment.Value);
                //string managerUser = (!string.IsNullOrEmpty(Convert.ToString(cmbResourceManager.Value))) ? Convert.ToString(cmbResourceManager.Value) : "";
                //LoadDdlResourceManager(dept, managerUser);

                gvResourceAvailablity.Columns.Clear();
                gvResourceAvailablity.TotalSummary.Clear();
                if (gvResourceAvailablity.Columns.Count <= 0)
                {
                    GridViewCommandColumn gridViewCommandColumn = new GridViewCommandColumn();
                    gridViewCommandColumn.ShowSelectCheckbox = true;
                    gridViewCommandColumn.Caption = " ";
                    gridViewCommandColumn.Width = new Unit("20px");
                    gvResourceAvailablity.Columns.Add(gridViewCommandColumn);

                    GridViewDataTextColumn colId = new GridViewDataTextColumn();
                    colId.Caption = "#";
                    colId.FieldName = DatabaseObjects.Columns.ItemOrder;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Width = new Unit("20px");
                    colId.ExportWidth = 70;
                    gvResourceAvailablity.Columns.Add(colId);


                    colId = new GridViewDataTextColumn();
                    colId.FieldName = DatabaseObjects.Columns.Id;
                    colId.Caption = DatabaseObjects.Columns.Id;
                    colId.Visible = false;
                    colId.VisibleIndex = -1;
                    gvResourceAvailablity.Columns.Add(colId);

                    colId = new GridViewDataTextColumn();
                    colId.FieldName = DatabaseObjects.Columns.Resource;
                    colId.Caption = DatabaseObjects.Columns.RResource; //DatabaseObjects.Columns.Resource;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colId.Width = new Unit("120px");
                    //
                    if (ResourceCapacity != "true")//Request["ResourceCapacity"] == "true")
                    {
                        if (Request["pStartDate"] == null && Request["pEndDate"] == null)
                            colId.DataItemTemplate = new HoverMenuDataTemplate();
                    }
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    //colId.SortOrder = ColumnSortOrder.Ascending;
                    colId.ExportWidth = 200;
                    gvResourceAvailablity.Columns.Add(colId);

                    ASPxSummaryItem item4 = new ASPxSummaryItem(DatabaseObjects.Columns.Resource, SummaryItemType.Custom);
                    item4.Tag = "BenchCost";
                    item4.ShowInColumn = DatabaseObjects.Columns.Resource;
                    gvResourceAvailablity.TotalSummary.Add(item4);

                    ASPxSummaryItem item5 = new ASPxSummaryItem(DatabaseObjects.Columns.Resource, SummaryItemType.Custom);
                    item5.Tag = "RedCount";
                    item5.ShowInColumn = DatabaseObjects.Columns.Resource;
                    gvResourceAvailablity.TotalSummary.Add(item5);

                    ASPxSummaryItem item6 = new ASPxSummaryItem(DatabaseObjects.Columns.Resource, SummaryItemType.Custom);
                    item6.Tag = "YellowCount";
                    item6.ShowInColumn = DatabaseObjects.Columns.Resource;
                    gvResourceAvailablity.TotalSummary.Add(item6);

                    ASPxSummaryItem item7 = new ASPxSummaryItem(DatabaseObjects.Columns.Resource, SummaryItemType.Custom);
                    item7.Tag = "GreenCount";
                    item7.ShowInColumn = DatabaseObjects.Columns.Resource;
                    gvResourceAvailablity.TotalSummary.Add(item7);

                    ASPxSummaryItem item8 = new ASPxSummaryItem(DatabaseObjects.Columns.Resource, SummaryItemType.Custom);
                    item8.Tag = "SilverCount";
                    item8.ShowInColumn = DatabaseObjects.Columns.Resource;
                    gvResourceAvailablity.TotalSummary.Add(item8);

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
                    colId.ExportWidth = 90;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Visible = !CalledFromDirectorView;
                    gvResourceAvailablity.Columns.Add(colId);


                    colId = new GridViewDataTextColumn();
                    colId.FieldName = DatabaseObjects.Columns.RevenueCapacity;
                    if (chkIncludeClosed.Checked == false)
                        colId.Caption = "$ Active";
                    else
                        colId.Caption = "$ Lifetime";
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.Width = new Unit("40px");
                    colId.ExportWidth = 90;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Visible = !CalledFromDirectorView;
                    gvResourceAvailablity.Columns.Add(colId);
                    if (CalledFromDirectorView)
                    {
                        colId = new GridViewDataTextColumn();
                        colId.FieldName = "AvgerageUtil";
                        colId.Caption = "Avrg Utilization";
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.Width = new Unit("45px");
                        colId.ExportWidth = 90;
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.PropertiesEdit.DisplayFormatString = "{0}%";
                        colId.Visible = CalledFromDirectorView;
                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;

                        if (rbtnBenchonly.Checked)
                        {
                            colId.SortOrder = ColumnSortOrder.Ascending ;
                        }
                        else
                        {
                            colId.SortOrder = ColumnSortOrder.Descending;
                        }

                        gvResourceAvailablity.Columns.Add(colId);

                        colId = new GridViewDataTextColumn();
                        colId.FieldName = "AvgerageChargeableUtil";
                        colId.Caption = "Avrg Chargeable";
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.Width = new Unit("50px");
                        colId.ExportWidth = 90;
                        colId.HeaderStyle.Font.Bold = true;
                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.PropertiesEdit.DisplayFormatString = "{0}%";
                        colId.Visible = CalledFromDirectorView;
                        gvResourceAvailablity.Columns.Add(colId);
                    }
                    GridViewBandColumn bdCol = new GridViewBandColumn();
                    string currentDate = string.Empty;
                    for (DateTime dt = Convert.ToDateTime(hdndtfrom.Value); Convert.ToDateTime(hdndtto.Value) >= dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        FromDateCheck = Convert.ToDateTime(hdndtfrom.Value).ToString();
                        ToDateCheck = Convert.ToDateTime(hdndtto.Value).ToString();

                        int weeks = Convert.ToInt32((Convert.ToDateTime(hdndtto.Value) - Convert.ToDateTime(hdndtfrom.Value)).TotalDays / 7);

                        if (hdndisplayMode.Value == "Weekly")
                        {
                            if (dt.ToString("MMM-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                            {
                                gvResourceAvailablity.Columns.Add(bdCol);
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
                                gvResourceAvailablity.Columns.Add(bdCol);
                                bdCol = new GridViewBandColumn();
                            }

                            if (dt.ToString("yyyy") != currentDate)
                            {
                                bdCol.Caption = dt.ToString("yyyy");
                                SetCookie("year", dt.ToString("yyyy"));
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
                                    colId.HeaderTemplate = new CommandColumnHeaderTemplate(colId);
                                }
                            }
                            if (weeks == 4)
                            {
                                colId.ExportWidth = 180;
                            }
                            else
                            {
                                colId.ExportWidth = 140;
                            }

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
                        colId.HeaderStyle.Border.BorderStyle = BorderStyle.None;
                        colId.CellStyle.Border.BorderStyle = BorderStyle.None;
                        if (hdndisplayMode.Value == "Monthly")
                        {
                            if (Request["pStartDate"] != null && Request["pEndDate"] != null)
                            {
                                colId.Width = new Unit("80px");
                            }
                            else
                            {
                                colId.Width = new Unit("40px");
                                colId.ExportWidth = 70;
                            }
                        }
                        else if (hdndisplayMode.Value == "Daily")
                        {
                            colId.Width = new Unit("100px");
                        }
                        else
                        {
                            colId.Width = new Unit("35px");
                        }

                        bdCol.Columns.Add(colId);

                        ASPxSummaryItem itemBenchCost = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                        itemBenchCost.Tag = "TBenchCost";
                        itemBenchCost.DisplayFormat = "N2";
                        gvResourceAvailablity.TotalSummary.Add(itemBenchCost);

                        ASPxSummaryItem itemRedCount = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                        itemRedCount.Tag = "TRedCount";
                        itemRedCount.DisplayFormat = "N2";
                        gvResourceAvailablity.TotalSummary.Add(itemRedCount);

                        ASPxSummaryItem itemYelloCount = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                        itemYelloCount.Tag = "TYellowCount";
                        itemYelloCount.DisplayFormat = "N2";
                        gvResourceAvailablity.TotalSummary.Add(itemYelloCount);

                        ASPxSummaryItem itemGreenCount = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                        itemGreenCount.Tag = "TGreenCount";
                        itemGreenCount.DisplayFormat = "N2";
                        gvResourceAvailablity.TotalSummary.Add(itemGreenCount);

                        ASPxSummaryItem itemSilverCount = new ASPxSummaryItem(dt.ToString("MMM-dd-yy"), SummaryItemType.Custom);
                        itemSilverCount.Tag = "TSilverCount";
                        itemSilverCount.DisplayFormat = "N2";
                        gvResourceAvailablity.TotalSummary.Add(itemSilverCount);
                    }
                    if (currentDate == bdCol.Caption)
                    {
                        gvResourceAvailablity.Columns.Add(bdCol);
                    }
                }
            }
            //SetGridSorting();
        }

        private void loadFunctions()
        {
            FunctionRoleManager _functionRoleManager = new FunctionRoleManager(HttpContext.Current.GetManagerContext());
            List<FunctionRole> functionRoles = _functionRoleManager.Load()?.OrderBy(y => y.Title).ToList() ?? null;
            if (functionRoles?.Count > 0)
            {
                cmbFunctionRole.ValueField = DatabaseObjects.Columns.ID;
                cmbFunctionRole.TextField = DatabaseObjects.Columns.Title;
                cmbFunctionRole.DataSource = functionRoles;
                cmbFunctionRole.DataBind();

                functionGridLookup.KeyFieldName = DatabaseObjects.Columns.ID;
                functionGridLookup.DataSource = functionRoles;
                functionGridLookup.DataBind();
            }
        }
        protected void LoadDdlResourceManager(string values = "", string selectedMgr = "")
        {

            List<UserProfile> lstUserProfile = new List<UserProfile>();
            List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
            if (values == "undefined" || values == "0" || string.IsNullOrEmpty(values))
                lstUserProfile = ObjUserProfileManager.GetUsersProfile().Where(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            else
                lstUserProfile = ObjUserProfileManager.GetUsersProfile().Where(x => x.IsManager == true && x.Enabled == true && lstDepartments.Contains(x.Department)).OrderBy(x => x.Name).ToList();

            cmbResourceManager.Items.Clear();

            if (lstUserProfile != null)
            {
                foreach (UserProfile userProfileItem in lstUserProfile)
                {
                    cmbResourceManager.Items.Add(new ListEditItem(userProfileItem.Name, userProfileItem.Id.ToString()));
                }

                cmbResourceManager.Items.Insert(0, new ListEditItem("All Users", "0"));
            }
            else
            {
                cmbResourceManager.Items.Insert(0, new ListEditItem("All Users", "0"));
            }


            var formKeys = Request.Form?.AllKeys?.ToList();
            if (formKeys?.Any(x => x.EndsWith("ddlResourceManager")) == true)
            {
                selectedMgr = Request[formKeys.First(x => x.EndsWith("ddlResourceManager"))];
            }

            if (!string.IsNullOrEmpty(selectedMgr) && cmbResourceManager.Items.FindByValue(selectedMgr) != null)
            {
                cmbResourceManager.Items.FindByValue(selectedMgr).Selected = true;
            }

        }

        private void LoadDepartment()
        {
            CompanyManager companyManager = new CompanyManager(applicationContext);
            List<Company> companies = new List<Company>();
            companies = companyManager.Load();
            DepartmentManager departmentManager = new DepartmentManager(applicationContext);

        }

        private void LoadFunctionalArea()
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(applicationContext);
            List<FunctionalArea> funcationalArealst = functionalAreasManager.LoadFunctionalAreas();

            List<FunctionalArea> filterFuncationalArealst = new List<FunctionalArea>();
            if (ddlDepartment.GetValues() != null && ddlDepartment.GetValues() != string.Empty)
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted && x.DepartmentLookup != null && x.DepartmentLookup.Value == UGITUtility.StringToInt(ddlDepartment.GetValues())).ToList();
            else
                filterFuncationalArealst = funcationalArealst.Where(x => !x.Deleted).ToList();

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



                //DataRow dtrow = dtTypeDate.NewRow();
                //dtrow["LevelTitle"] = "ALL";
                //dtrow["LevelName"] = "ALL";
                //dtTypeDate.Rows.InsertAt(dtrow, 0);

                dtFilterTypeData = dtTypeDate;
                glType.DataSource = dtFilterTypeData;
                glType.DataBind();

            }
        }

        #endregion


        #region Events
        protected void btnClose_Click(object sender, EventArgs e)
        {
            var objSelected = gvResourceAvailablity.GetSelectedFieldValues(DatabaseObjects.Columns.Id);
            if (objSelected != null && objSelected.Count > 0)
            {
                string sourceUrl = string.Empty;
                if (Context.Request["source"] != null && Context.Request["source"].Trim() != string.Empty)
                {
                    sourceUrl = Context.Request["source"].Trim();
                }
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("ControlId", this.ControlId);
                dict.Add("LookupId", Convert.ToString(objSelected[0]));
                dict.Add("frameUrl", sourceUrl);
                var vals = UGITUtility.GetJsonForDictionary(dict);
                uHelper.ClosePopUpAndEndResponse(Context, false, vals);
            }
        }

        protected void ddlIntervalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareAllocationGrid();
        }

        //protected void chkPercentage_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkPercentage.Checked)
        //    {
        //        chkItemCount.Checked = false;
        //    }
        //    SetCookiesCheckboxFilters();
        //}

        private void SetCookiesCheckboxFilters()
        {

            if (chkAll.Checked)
            {
                UGITUtility.CreateCookie(Response, "filterAll", "all");
            }
            else
            {
                UGITUtility.CreateCookie(Response, "filterAll", "");
            }
            if (chkIncludeClosed.Checked)
            {
                UGITUtility.CreateCookie(Response, "IncludeClosed", "true");
            }
            else
            {
                UGITUtility.CreateCookie(Response, "IncludeClosed", "");
            }
        }

        protected void chkColor_CheckedChanged(object sender, EventArgs e)
        {
            SetCookiesCheckboxFilters();
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
            SetCookiesCheckboxFilters();
            PrepareAllocationGrid();
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
            //else if (hdndisplayMode.Value == DisplayMode.Daily.ToString())
            //{
            //    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddDays(-7));
            //    hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddDays(-7));
            //}
            else
            {
                //FromDateCheck = Convert.ToString(Convert.ToDateTime(FromDateCheck).AddYears(-1));
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
                //hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddYears(-1));
                //hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddYears(-1));
                //ToDateCheck = Convert.ToString(Convert.ToDateTime(ToDateCheck).AddYears(-1));
            }
            PrepareAllocationGrid();
            dtMonthlyAllocation = null;
            dtRawDataAllResource = null;
            gvResourceAvailablity.DataSource = GetAllocationData();
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
            //else if (hdndisplayMode.Value == DisplayMode.Daily.ToString())
            //{
            //    hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddDays(7));
            //    hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddDays(7));
            //}
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
                //FromDateCheck = Convert.ToString(Convert.ToDateTime(FromDateCheck).AddYears(1));
                //hdndtfrom.Value = Convert.ToString(Convert.ToDateTime(hdndtfrom.Value).AddYears(1));
                //hdndtto.Value = Convert.ToString(Convert.ToDateTime(hdndtto.Value).AddYears(1));
                //ToDateCheck = Convert.ToString(Convert.ToDateTime(ToDateCheck).AddYears(1));
            }
            PrepareAllocationGrid();
            dtMonthlyAllocation = null;
            dtRawDataAllResource = null;
            gvResourceAvailablity.DataSource = GetAllocationData();
        }

        public DateTime GetLastMondayOfMonth(DateTime date)
        {
            // First, determine the last day of the month for the given date
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return lastDayOfMonth;
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
            string selectedValue = string.Join(",", userGroupGridLookup.GridView.GetSelectedFieldValues("Id"));
            UGITUtility.CreateCookie(Response, "filter", string.Format("group-{0}#type-{1}", selectedValue, glType.Text));
            hdnSelectedGroup.Value = selectedValue;

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
        }

        protected void rbtnProject_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            SetCookiesCheckboxFilters();
            PrepareAllocationGrid();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void rbtnItemCount_CheckedChanged(object sender, EventArgs e)
        {
            //UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "count");
            SetCookie("filtercountpercentagefte", "count");
            PrepareAllocationGrid();
        }

        //protected void rbtnPercentage_CheckedChanged(object sender, EventArgs e)
        //{
        //    //UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "percentage");
        //    SetCookie("filtercountpercentagefte", "percentage");
        //    PrepareAllocationGrid();
        //}
        //protected void rbtnHrs_CheckedChanged(object sender, EventArgs e)
        //{
        //    SetCookie("filtercountpercentagefte", "Hrs");
        //    PrepareAllocationGrid();
        //}

        //protected void rbtnFTE_CheckedChanged(object sender, EventArgs e)
        //{
        //    //UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "fte");
        //    SetCookie("filtercountpercentagefte", "fte");
        //    PrepareAllocationGrid();
        //}
        ////change by Hareram
        //protected void rbtnAvailability_CheckedChanged(object sender, EventArgs e)
        //{
        //    //UGITUtility.CreateCookie(Response, "filtercountpercentagefte", "availability");
        //    SetCookie("filtercountpercentagefte", "availability");
        //    PrepareAllocationGrid();
        //}
        #endregion

        private void SetCookie(string Name, string Value)
        {
            UGITUtility.CreateCookie(Response, Name, Value);
        }

        
        protected void gvResourceAvailablity_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                // Finalization.  
                string strResourceAllocationColorPalete = ObjConfigurationVariableManager.GetValue(ConfigConstants.ResourceAllocationColorPalete);
                bool HideBenchCost = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.HideBenchCost);
                if (!string.IsNullOrEmpty(strResourceAllocationColorPalete))
                {
                    Dictionary<string, string> cpResourceAllocationColorPalete = UGITUtility.GetCustomProperties(strResourceAllocationColorPalete, Constants.Separator);


                    if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                    {
                        DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                        if (item.FieldName == DatabaseObjects.Columns.Resource)
                        {

                            if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "BenchCost" && HideBenchCost == false)
                                e.TotalValue = "<div class='square1'><span style='margin-left:15px;'>Bench Cost</span></div>";
                            else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "RedCount")
                                e.TotalValue = "<div class='square' style='background-color:" + cpResourceAllocationColorPalete[Constants.Red] + "'><span style='margin-left:15px;'>Fully Available (FTE)</span></div>";
                            else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "YellowCount")
                                e.TotalValue = "<div class='square' style='background-color:" + cpResourceAllocationColorPalete[Constants.Orange] + "'><span style='margin-left:15px;'>Over Allocated (FTE)</span></div>";
                            else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "GreenCount")
                                e.TotalValue = "<div class='square' style='background-color:" + cpResourceAllocationColorPalete[Constants.Green] + "'><span style='margin-left:15px;'>Fully Engaged (FTE)</span></div>";
                            else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag.Contains("SilverCount"))
                                e.TotalValue = "<div class='square' style='background-color:" + cpResourceAllocationColorPalete[Constants.Gray] + "'><span style='margin-left:15px;'>Partially Engaged (FTE)</span></div>";

                        }
                        if (item.FieldName != DatabaseObjects.Columns.Resource)
                        {
                            if (hdndisplayMode.Value == "Monthly")
                            {
                                int currentYear = Convert.ToInt32(DateTime.Now.Year);
                                int selectedYear = Convert.ToInt32(Convert.ToDateTime(hdndtfrom.Value).ToString("yyyy"));
                                if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TBenchCost" && dsData?.Tables?.Count > 5)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[5]))
                                    {
                                        object value = dsData.Tables[5].Rows[0][item.FieldName];
                                        if (UGITUtility.StringToDouble(value) < 0)
                                            e.TotalValue = "(" + Convert.ToString(value) + ")";
                                        else
                                            e.TotalValue = "$" + UGITUtility.ObjectToString(Math.Round(UGITUtility.StringToDouble(value) / 1000, 0)) + " (K)";
                                    }
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TRedCount" && dsData?.Tables?.Count > 2)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[2]))
                                        e.TotalValue = dsData.Tables[2].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TYellowCount" && dsData?.Tables?.Count > 3)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[3]))
                                        e.TotalValue = dsData.Tables[3].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TGreenCount" && dsData?.Tables?.Count > 4)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[4]))
                                        e.TotalValue = dsData.Tables[4].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TSilverCount" && dsData?.Tables?.Count > 6)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[6]))
                                        e.TotalValue = dsData.Tables[6].Rows[0][item.FieldName];
                                }

                            }
                            else if (hdndisplayMode.Value == "Weekly")
                            {
                                if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TBenchCost" && dsData?.Tables?.Count > 5 && HideBenchCost == false)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[5]))
                                    {
                                        object value = dsData.Tables[5].Rows[0][item.FieldName];
                                        if (UGITUtility.StringToDouble(value) < 0)
                                            e.TotalValue = "(" + Convert.ToString(value) + ")";
                                        else
                                            e.TotalValue = "$" + UGITUtility.ObjectToString(Math.Round(UGITUtility.StringToDouble(value) / 1000, 0)) + " (K)";
                                    }
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TRedCount" && dsData?.Tables?.Count > 2)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[2]))
                                        e.TotalValue = dsData.Tables[2].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TYellowCount" && dsData?.Tables?.Count > 3)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[3]))
                                        e.TotalValue = dsData.Tables[3].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TGreenCount" && dsData?.Tables?.Count > 4)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[4]))
                                        e.TotalValue = dsData.Tables[4].Rows[0][item.FieldName];
                                }
                                else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TSilverCount" && dsData?.Tables?.Count > 6)
                                {
                                    if (UGITUtility.IfColumnExists(item.FieldName, dsData.Tables[6]))
                                        e.TotalValue = dsData.Tables[6].Rows[0][item.FieldName];
                                }
                            }
                        }

                    }
                }
            }
        }

        protected void gvResourceAvailablity_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            List<Department> departments = new List<Department>();
            if (!string.IsNullOrEmpty(selectedDivision))
            {
                SetCookie("division", selectedDivision);
                departments = departmentsLoad.Where(d => d.DivisionIdLookup == UGITUtility.StringToLong(selectedDivision)).ToList();
                hdnaspDepartment.Value = string.Join(",", departments.Select(d => d.ID));
                ddlDepartment.SetValues(hdnaspDepartment.Value);
            }

            PrepareAllocationGrid();

            //SetGridSorting();
        }

        protected void cbpManagers_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] values = UGITUtility.SplitString(parameters, Constants.Separator2);
            if (values.Count() >= 1)
            {
                if (cmbResourceManager.SelectedItem != null)
                    LoadDdlResourceManager(values[0], UGITUtility.ObjectToString(cmbResourceManager.SelectedItem.Value));
                else
                    LoadDdlResourceManager(values[0]);
            }
            else
            {
                LoadDdlResourceManager();
            }
            if (cmbResourceManager.SelectedItem == null)
                cmbResourceManager.Items.FindByValue("0").Selected = true;
        }

        protected void cbpResourceAvailability_Callback(object sender, CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] arrParams = UGITUtility.SplitString(parameters, Constants.CommentSeparator);
            if (parameters.Contains("LoadRoles"))
            {
                if (arrParams.Count() > 0)
                {
                    string[] values = UGITUtility.SplitString(arrParams[0], Constants.Separator2);
                    if (values.Count() >= 1)
                    {
                        LoadRolesOnDepartmentAndFunctional(values[1], hdnaspFunction.Value);
                        if (values[1].EqualsIgnoreCase("undefined"))
                            hdnaspDepartment.Value = "";
                        else
                            hdnaspDepartment.Value = values[1];
                    }
                }
            }
            else if (parameters.Contains("FunctionalRole"))
            {
                if (arrParams.Count() > 1 && !string.IsNullOrWhiteSpace(arrParams[1]))
                {
                    LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value, arrParams[1]);
                }
                else
                {
                    LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value, "");
                }
            }
            ASPxCallbackPanel gv = (ASPxCallbackPanel)sender;
            gv.JSProperties["cpResourceAvailabilityCallback"] = hdnaspDepartment.Value;
        }

        private void LoadRolesOnDepartmentAndFunctional(string departmentValues = "", string functionValues = "")
        {
            List<GlobalRole> globalRoles = new List<GlobalRole>();
            globalRoles = globalRoleData;

            if (globalRoles?.Count() > 0 && !string.IsNullOrWhiteSpace(functionValues))
            {
                FunctionRoleMappingManager _functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
                List<long> ids = UGITUtility.SplitString(functionValues, Constants.Separator)?.Select(long.Parse)?.ToList() ?? null;
                List<FunctionRoleMapping> lstFunctionRoleMapping = _functionRoleMappingManager.Load(x => ids.Contains(x.FunctionId));
                if (lstFunctionRoleMapping != null && lstFunctionRoleMapping.Count > 0)
                {
                    List<string> lstRoleids = lstFunctionRoleMapping.Select(x => x.RoleId).ToList();
                    globalRoles = globalRoles.Where(x => lstRoleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
                }
            }

            if (globalRoles?.Count() > 0)
            {
                JobTitleManager jobTitleManager = new JobTitleManager(applicationContext);
                List<JobTitle> jobTitles = new List<JobTitle>();
                List<string> lstDepartments = UGITUtility.ConvertStringToList(departmentValues, Constants.Separator6);
                if (string.IsNullOrEmpty(departmentValues) || departmentValues == "undefined")
                    jobTitles = jobTitleManager.Load();
                else
                    jobTitles = jobTitleManager.Load(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.DepartmentId)));
                List<string> jobtitleids = jobTitles.Select(x => x.RoleId).ToList();
                globalRoles = globalRoles.Where(x => jobtitleids.Contains(x.Id))?.OrderBy(y => y.Name)?.ToList();
            }

            if (globalRoles != null)
            {
                userGroupGridLookup.KeyFieldName = "Id";
                userGroupGridLookup.DataSource = globalRoles;
                userGroupGridLookup.DataBind();
            }
        }
        //private void LoadGlobalRolesOnDepartment(string values)
        //{
        //    JobTitleManager jobTitleManager = new JobTitleManager(applicationContext);
        //    List<JobTitle> jobTitles = new List<JobTitle>();
        //    List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
        //    if (string.IsNullOrEmpty(values) || values == "undefined")
        //        jobTitles = jobTitleManager.Load();
        //    else
        //        jobTitles = jobTitleManager.Load(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.DepartmentId)));

        //    List<string> jobtitleids = jobTitles.Select(x => x.RoleId).ToList();

        //    GlobalRoleManager roleManager = new GlobalRoleManager(applicationContext);
        //    List<GlobalRole> globalRoles = new List<GlobalRole>();
        //    globalRoles = uHelper.GetGlobalRoles(applicationContext, false).Where(x => jobtitleids.Contains(x.Id)).OrderBy(y => y.Name).ToList(); //roleManager.Load(x => jobtitleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
        //    if (globalRoles != null)
        //    {
        //        userGroupGridLookup.KeyFieldName = "Id";
        //        userGroupGridLookup.DataSource = globalRoles;
        //        userGroupGridLookup.DataBind();
        //    }
        //}

        protected void ddlUserGroup_Init(object sender, EventArgs e)
        {
            string hdndept = UGITUtility.ObjectToString(Request[hdnaspDepartment.UniqueID]);
            if (string.IsNullOrEmpty(hdndept))
            {
                GlobalRoleManager globalRoleManager = new GlobalRoleManager(applicationContext);
                List<GlobalRole> roles = globalRoleData; //globalRoleManager.Load(x => !x.Deleted).OrderBy(x => x.Name).ToList();
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
                LoadRolesOnDepartmentAndFunctional(hdndept);
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

                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;\" src=\"/content/images/back-arrowBlue.png\" onclick=\""+ string.Format("ClickOnPrevious(this,'{0}')", colBDC.Caption) + "\" class=\"resource-img-gantt\"  />"));
                HContainer.Controls.Add(hnkBDButton);
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;\" src=\"/content/images/next-arrowBlue.png\" onclick=\""+ string.Format("ClickOnNext(this,'{0}')", colBDC.Caption) + "\" class=\"resource-img-gantt\"  />"));
                container.Controls.Add(HContainer);
            }

            #endregion
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
                container.Controls.Add(divContainer);



            }
        }

        protected void cmbResourceManager_Callback(object sender, CallbackEventArgsBase e)
        {
            UGITUtility.GetCookieValue(Request, "filterResource");
            PrepareAllocationGrid();
        }

        protected void cmbResourceManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.GetCookieValue(Request, "filterResource");
            PrepareAllocationGrid();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            btnexport = true;
            allocationData = GetAllocationData();
            gvResourceAvailablity.DataSource = allocationData;
            DevExpress.XtraPrinting.XlsExportOptionsEx options = new DevExpress.XtraPrinting.XlsExportOptionsEx();
            options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
            gridExporter.WriteXlsToResponse("Resource Utilization", options);
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ResourceAvailabilityloadingPanel.Visible = true;
            btnexport = true;
            string pageHeaderInfo;
            string deptDetailInfo;
            string deptShortInfo;

            if (ddlDepartment.dropBox.Text.Contains(",") || ddlDepartment.dropBox.Text == "<Various>")
            {
                deptShortInfo = "Various Departments";
                deptDetailInfo = RMMSummaryHelper.GetSelectedDepartmentsInfo(applicationContext, Convert.ToString(hdnaspDepartment.Value), enableDivision);
            }
            else if (ddlDepartment.dropBox.Text == "All")
            {
                deptShortInfo = "All Departments";
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            else
            {
                deptShortInfo = ddlDepartment.dropBox.Text;
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            DateTime fromDate = Convert.ToDateTime(hdndtfrom.Value);
            DateTime toDate = Convert.ToDateTime(hdndtto.Value);
            //string PageNamePDF = "<span style='font-weight: bold'>Resource Utilization:</span>";

            pageHeaderInfo = string.Format("Resource Utilization: {0}; {1} to {2}", deptShortInfo, uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));
            //pageHeaderInfo = string.Format("{0} {1}; {2} to {3}", PageNamePDF, deptShortInfo, uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));
            string reportFooterInfo = string.Format("\nSelection Criteria");//\n   {0}: {1}\n   {2}: {3} to {4}", "Departments", deptDetailInfo, "Date Range", uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));
            if (!string.IsNullOrEmpty(selecteddepartment))
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1}", "Departments", deptDetailInfo);
            }
            if (userGroupGridLookup != null && !string.IsNullOrWhiteSpace(userGroupGridLookup.Text))
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1}", "Role", userGroupGridLookup.Text); //ObjUserProfileManager.GetUserNameById(ddlUserGroup.SelectedValue));
            }
            if (!string.IsNullOrEmpty(selectedManager))
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1}", "Manager", ObjUserProfileManager.GetUserNameById(selectedManager));
            }
            if(sActionType.Count() > 0)
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1}", "Type", UGITUtility.ConvertListToString(sActionType, Constants.Separator6));
            }
            if (chkAll.Checked)
                reportFooterInfo = reportFooterInfo + string.Format("\n All Resources Included.");
            if (!chkIncludeClosed.Checked)
                reportFooterInfo = reportFooterInfo + string.Format("\n Closed Projects Included.");

            reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1} to {2}", "Date Range", uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));

            allocationData = GetAllocationData();

            gvResourceAvailablity.DataSource = allocationData;
            
            gridExporter.Landscape = true;
            gridExporter.PaperKind = System.Drawing.Printing.PaperKind.A4;
            
            gridExporter.LeftMargin = 1;
            gridExporter.RightMargin = 1;
            gridExporter.TopMargin = -1;
            gridExporter.BottomMargin = -1;
            
            gridExporter.PageHeader.Font.Size = 11;
            gridExporter.PageHeader.Font.Name = "Arial";
            gridExporter.PageHeader.Center = pageHeaderInfo;
            gridExporter.PageHeader.Font.Bold = true;

            gridExporter.PageFooter.Center = "Page [Page # of Pages #]";
            gridExporter.PageFooter.Left = "[Date Printed]";
            gridExporter.ReportFooter = reportFooterInfo;

            gridExporter.WritePdfToResponse("Resource Utilization");
            
            Thread.Sleep(1000);

            ResourceAvailabilityloadingPanel.Visible = false;
        }

        protected void gridExporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Header)
                return;
            GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;

            DateTime fromDate = Convert.ToDateTime(hdndtfrom.Value);
            DateTime toDate = Convert.ToDateTime(hdndtto.Value);

            if (e.RowType == GridViewRowType.Data && dataColumn != null)
            {
                e.BrickStyle.Sides = DevExPrinting.BorderSide.All;
                e.BrickStyle.BorderColor = System.Drawing.Color.Black;
                e.BrickStyle.BorderWidth = 1;
                //e.BrickStyle.BorderStyle = DevExpress.XtraPrinting.BrickBorderStyle.Inset;
                e.Column.CellStyle.Height = new Unit("10px");
                if (dataColumn.FieldName == "ResourceUser")
                {
                    //if (hdndisplayMode.Value == "Weekly")
                    //{
                    //    string[] resourceNameHtml = e.Text.Split('>');
                    //    string[] resourceName = resourceNameHtml[1].Split('<');
                    //    string resource = resourceName[0];
                    //    e.Text = resource;
                    //}
                    e.BrickStyle.TextAlignment = DevExPrinting.TextAlignment.MiddleLeft;
                }
                else
                {
                    e.BrickStyle.TextAlignment = DevExPrinting.TextAlignment.MiddleCenter;
                }
                
                if (dataColumn.FieldName != "ResourceUser" && dataColumn.FieldName != "ItemOrder" && dataColumn.FieldName != "ProjectCapacity" && dataColumn.FieldName != "RevenueCapacity")
                {
                    for (DateTime dt = Convert.ToDateTime(fromDate); Convert.ToDateTime(toDate) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        string estAlloc = string.Empty;
                        string backColor = string.Empty;
                        string foreColor = string.Empty;

                        if (dataColumn.FieldName == dt.ToString("MMM-dd-yy"))
                        {
                            object estAllocValue = e.GetValue(dt.ToString("MMM-dd-yy")); //#90ee90>1
                            if (estAllocValue is null)
                            {
                                estAlloc = "";

                            }
                            else
                            {
                                var obj = gvResourceAvailablity.GetRow(e.VisibleIndex);
                                DateTime dtStart = DateTime.MinValue;
                                if (dataColumn.Caption == "#")
                                    e.Text = Convert.ToString(e.VisibleIndex + 1);
                                
                                var dataRowView = obj as DataRowView;
                                DateTime.TryParse(dataColumn.FieldName, out dtStart);
                                UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.Id])));
                                string displaymode = rbtnHrs.Checked == true ? "H" : rbtnItemCount.Checked == true ? "C" : rbtnPercentage.Checked == true ? "P" : rbtnFTE.Checked == true ? "F" : "A";
                                string strResourceAllocationColorPalete = ObjConfigurationVariableManager.GetValue(ConfigConstants.ResourceAllocationColorPalete);
                                if (!string.IsNullOrEmpty(strResourceAllocationColorPalete))
                                {
                                    Dictionary<string, string> cpResourceAllocationColorPalete = UGITUtility.GetCustomProperties(strResourceAllocationColorPalete, Constants.Separator);

                                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(estAllocValue)))
                                    {
                                        double cellvalue = UGITUtility.StringToDouble(GetValueFromInput(UGITUtility.ObjectToString(estAllocValue), "P"));
                                        e.Text = GetValueFromInput(UGITUtility.ObjectToString(estAllocValue), displaymode);
                                        if (displaymode == "P" || displaymode == "A")
                                            e.Text = e.Text + "%";
                                        if (cellvalue >= orange)
                                        {
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Orange]));
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                        }
                                        else if (cellvalue >= green && cellvalue < orange)
                                        {
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Green]));//Green
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                        }
                                        else if (cellvalue >= gray && cellvalue < green)
                                        {
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Gray]));//Gray
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                        }
                                        else if (cellvalue >= red && cellvalue < gray)
                                        {
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]));//Red
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                        }
                                        else
                                            e.Text = e.Text;  //no color
                                    }
                                    else
                                    {
                                        if (userProfile.UGITStartDate < dtStart)
                                        {
                                            if (displaymode == "P")
                                            {
                                                e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]));
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                                e.Text = "0%";
                                            }
                                            else if (displaymode == "A")
                                            {
                                                e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Green]));
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                                e.Text = "100%";
                                            }
                                            else
                                            {
                                                e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]));
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fff");
                                                e.Text = "0";
                                            }
                                        }
                                        else
                                            e.Text = "";
                                    }
                                }
                                if (userProfile.UGITStartDate > dtStart || userProfile.UGITEndDate < dtStart)
                                {
                                    e.Text = "";
                                }
                            }
                            //e.BrickStyle.Sides = DevExPrinting.BorderSide.All;
                        }
                    }
                }
            }
        }

        protected void imgNewGanttAllocation_Click(object sender, EventArgs e)
        {

            GetAllocationData();

            hndYear = UGITUtility.GetCookieValue(Request, "year");

            string pageHeaderInfo;
            string deptDetailInfo;
            string deptShortInfo;

            if (ddlDepartment.dropBox.Text.Contains(",") || ddlDepartment.dropBox.Text == "<Various>")
            {
                deptShortInfo = "Various Departments";
                deptDetailInfo = RMMSummaryHelper.GetSelectedDepartmentsInfo(applicationContext, Convert.ToString(hdnaspDepartment.Value), enableDivision);
            }
            else if (ddlDepartment.dropBox.Text == "All")
            {
                deptShortInfo = "All Departments";
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            else
            {
                deptShortInfo = ddlDepartment.dropBox.Text;
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            DateTime fromDate = Convert.ToDateTime(hdndtfrom.Value);
            DateTime toDate = Convert.ToDateTime(hdndtto.Value);
            pageHeaderInfo = string.Format("Resource Utilization: {0}; {1} to {2}", deptShortInfo, uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));
            string reportFooterInfo = string.Format("\nSelection Criteria\n   {0}: {1}\n   {2}: {3} to {4}", "Departments", deptDetailInfo, "Date Range", uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));

            DataTable dtGanttExport = new DataTable();
            dtGanttExport = GetAllocationDataGantt();
            gvPreview.DataSource = dtGanttExport;
            gvPreview.DataBind();

            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A4;
            ASPxGridViewExporter1.LeftMargin = 1;
            ASPxGridViewExporter1.RightMargin = 1;
            ASPxGridViewExporter1.TopMargin = -1;
            ASPxGridViewExporter1.BottomMargin = -1;
            ASPxGridViewExporter1.PageHeader.Font.Size = 11;
            ASPxGridViewExporter1.PageHeader.Font.Name = "Arial";
            ASPxGridViewExporter1.PageHeader.Center = pageHeaderInfo;
            ASPxGridViewExporter1.PageFooter.Center = "Page [Page # of Pages #]";
            ASPxGridViewExporter1.PageFooter.Left = "[Date Printed]";
            ASPxGridViewExporter1.ReportFooter = reportFooterInfo;
            ASPxGridViewExporter1.MaxColumnWidth = 400;
            ASPxGridViewExporter1.Styles.GroupRow.Font.Name = "Arial";
            ASPxGridViewExporter1.Styles.GroupRow.Font.Size = 10;
            ASPxGridViewExporter1.Styles.GroupRow.ForeColor = Color.Black;
            ASPxGridViewExporter1.Styles.Footer.Font.Name = "Arial";
            ASPxGridViewExporter1.Styles.Footer.Font.Size = 10;
            ASPxGridViewExporter1.Styles.Footer.Font.Bold = true;
            ASPxGridViewExporter1.Styles.Footer.ForeColor = Color.White;
            ASPxGridViewExporter1.Styles.Footer.BackColor = System.Drawing.ColorTranslator.FromHtml("#808080");
            ASPxGridViewExporter1.WritePdfToResponse("Resource Utilization Gantt");

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Export", "alert('File Exported')", true);

        }

        #region GanttData
        public void GetAllocationFooter()
        {
            Dictionary<string, object> RUValues = new Dictionary<string, object>();
            string AllocationType = string.Empty;
            string allResources = string.Empty;
            string allClosed = string.Empty;
            string type = string.Empty;
            if ((Convert.ToString(chkAll.Checked) == "True") || (Convert.ToString(chkAll.Checked) == "1"))
            {
                allResources = "True";
            }
            else
            {
                allResources = "False";
            }

            if ((Convert.ToString(chkIncludeClosed.Checked) == "True") || (Convert.ToString(chkAll.Checked) == "1"))
            {
                allClosed = "True";
            }
            else
            {
                allClosed = "False";
            }
            
            string managerUser = (!string.IsNullOrEmpty(Convert.ToString(cmbResourceManager.Value))) ? Convert.ToString(cmbResourceManager.Value) : "";
            if (rbtnFTE.Checked)
                type = "FTE";
            else if (rbtnPercentage.Checked)
                type = "PERCENT";
            else if (rbtnAvailability.Checked)
                type = "AVAILABILITY";
            else if (rbtnHrs.Checked)
                type = "HRS";
            else
                type = "COUNT";
            string levelName = string.Join(",", sActionType);
            string roleID = ddlUserGroup.SelectedValue != "0" && ddlUserGroup.SelectedValue != "" ? ddlUserGroup.SelectedValue : "";

            RUValues.Add("@TenantID", applicationContext.TenantID);
            RUValues.Add("@IncludeAllResources", allResources);
            RUValues.Add("@IncludeClosedProject", allClosed);
            RUValues.Add("@DisplayMode", type);  //FTE, COUNT, PERCENT, AVAILABILITY
            RUValues.Add("@Department", hdnaspDepartment.Value == "undefined" ? string.Empty : hdnaspDepartment.Value.TrimEnd(','));
            RUValues.Add("@StartDate", Convert.ToDateTime(hdndtfrom.Value).ToString("MM-dd-yyyy"));
            RUValues.Add("@EndDate", Convert.ToDateTime(hdndtto.Value).ToString("MM-dd-yyyy"));
            RUValues.Add("@ResourceManager", string.IsNullOrEmpty(managerUser) ? "0" : managerUser);
            RUValues.Add("@AllocationType", AllocationType);
            RUValues.Add("@LevelName", levelName);
            RUValues.Add("@GlobalRoleId", roleID);
            RUValues.Add("@Mode", UGITUtility.ObjectToString(hdndisplayMode.Value)); 

            dsFooter = GetTableDataManager.GetDataSet("ResourceUtlizationFooter", RUValues);
        }
        public DataTable GetAllocationDataGantt()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Name, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Project, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PctEstimatedAllocation, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));
            data.Columns.Add("AllocationType", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationID, typeof(int));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleRelativePagePath, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ERPJobID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ContactLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.PreconStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PreconEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionStart, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.CloseoutDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.SoftAllocation, typeof(bool));
            data.Columns.Add("NCO", typeof(string));
            data.Columns.Add("AllStartDates", typeof(string));
            data.Columns.Add("AllEndDates", typeof(string));
            DataTable dtlist = new DataTable();

            GetAllocationFooter();

            if (hdndisplayMode.Value == "Weekly")
            {
                dateFrom = Convert.ToDateTime(hdndtfrom.Value);
                dateTo = Convert.ToDateTime(hdndtto.Value);

            }
            else
            {

                int selectedYear = UGITUtility.StringToInt(hndYear);
                dateFrom = new DateTime(selectedYear, 1, 1);
                dateTo = new DateTime(selectedYear + 1, 1, 1);

            }
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode.Value, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear))
                {
                    lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    hndYear = lblSelectedYear.Text;
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }
            if (!string.IsNullOrEmpty(ddlDepartment.GetValues()))
            {
                selecteddepartment = ddlDepartment.GetValues();
            }
            else if (!string.IsNullOrEmpty(hdnaspDepartment.Value))
            {
                selecteddepartment = hdnaspDepartment.Value;
            }

            if (hdndisplayMode.Value == "Weekly")
            {
                dateFrom = Convert.ToDateTime(hdndtfrom.Value);
                dateTo = Convert.ToDateTime(hdndtto.Value);

            }
            else
            {
                int selectedYear = UGITUtility.StringToInt(hndYear);
                dateFrom = new DateTime(selectedYear, 1, 1);
                dateTo = new DateTime(selectedYear + 1, 1, 1);
            }
            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear))
                {
                    lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    hndYear = lblSelectedYear.Text;
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }
            if (Request["selectedManager"] != null && Request["selectedManager"] != "0")
            {
                selectedManager = Convert.ToString(Request["selectedManager"]);
            }
            else if (cmbResourceManager.SelectedIndex != -1)
            {
                selectedManager = Convert.ToString(cmbResourceManager.Value);
            }
            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.StripHTML(Request["SelectedCategory"]);
            else if (!string.IsNullOrEmpty(glType.Text))
            {
                if (dataTable == null)
                    dataTable = (DataTable)glType.DataSource;

                selectedTypes = glType.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> selectModules = new List<string>();

                foreach (string sType in selectedTypes)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("LevelTitle") == sType);
                    if (row != null)
                    {
                        selectModules.Add(Convert.ToString(row["LevelName"]));
                        //if (Convert.ToString(row["ColumnType"]) == "Module")
                        //{
                        //    if (!containsModules)
                        //        containsModules = true;
                        //}
                    }
                    else
                    {
                        selectModules.Add(sType);
                    }
                }

                selectedCategory = string.Join("#", selectModules.ToArray());
            }
            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;

            //lstUProfile = ObjUserProfileManager.GetUsersProfile();
            if (Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)) != "0" && Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)) != "")
            {
                lstUProfile = ObjUserProfileManager.GetUserByManager(Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)), true);

                UserProfile newlstitem = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id == Convert.ToString(UGITUtility.ObjectToString(cmbResourceManager.Value)));
                if (newlstitem != null)
                    lstUProfile.Add(newlstitem);
            }
            else
            {
                lstUProfile = ObjUserProfileManager.GetUsersProfile();
            }

            if (userGroupGridLookup?.GridView?.GetSelectedFieldValues("Id")?.Count > 0)
            {
                List<string> gIds = userGroupGridLookup.GridView.GetSelectedFieldValues("Id").Select(x => (string)x).ToList();
                List<UserProfile> lstGroupUsers = ObjUserProfileManager.GetUsersProfile().Where(x => gIds.Contains(x.GlobalRoleId)).ToList();
                lstUProfile = lstGroupUsers.FindAll(x => lstUProfile.Any(y => y.Id == x.Id));
            }


            string filteredUsers = string.Empty; // UGITUtility.GetCookieValue(Request, "GridUsers");
            string SelectedUsers = string.Empty;
            if (!string.IsNullOrEmpty(filteredUsers))
            {
                SelectedUsers = filteredUsers;
                limitedUsers = true;
            }
            else
            {
                SelectedUsers = string.Empty;
                limitedUsers = false;
            }

            if (!string.IsNullOrEmpty(TicketId))
            {
                limitedUsers = false;
            }

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();

            //SelectedUsers = "ce015a93-a2b6-41e1-ac8b-0dfba3b24661";   //Benny Zang: testing code
            //limitedUsers = true;
            if (limitedUsers)
            {
                if (!string.IsNullOrEmpty(SelectedUsers) && SelectedUsers != "null")
                {
                    userIds = UGITUtility.ConvertStringToList(SelectedUsers, Constants.Separator6);
                }
                else
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();
                }
                dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);

            }
            else
            {
                dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 4, dateFrom, dateTo);
                workitems = RMMSummaryHelper.GetOpenworkitems(applicationContext, chkIncludeClosed.Checked);
            }


            if (!string.IsNullOrEmpty(TicketId))
            {
                DataRow[] ticketRows = dtResult.Select("TicketID = " + TicketId);
                dtResult = ticketRows.CopyToDataTable();

                DataRow[] workitemRows = workitems.Select($"{DatabaseObjects.Columns.WorkItem} = {TicketId}");
                workitems = workitemRows.CopyToDataTable();
            }

            //filter data based on closed check
            if (!chkIncludeClosed.Checked)
            {
                // Filter by Open Tickets.
                List<string> LstClosedTicketIds = new List<string>();
                //get closed ticket instead of open ticket and then filter all except closed ticket
                DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(applicationContext);
                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                }
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow[] dr = dtResult.AsEnumerable().Where(x => !LstClosedTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (dr != null && dr.Length > 0)
                        dtResult = dr.CopyToDataTable();
                    else
                        dtResult.Rows.Clear();
                }
            }

            //Load Tickets object before loop to improve perfromance
            TicketManager objTicketManager = new TicketManager(applicationContext);
            DataTable dtAllModuleTickets = objTicketManager.GetAllProjectTickets();


            if (dtResult == null)
                return data;

            var allocationGroupData = dtResult.AsEnumerable()
                .GroupBy(row => new
                {
                    Id = row.Field<string>("WorkItem"),
                    ResourceId = row.Field<string>("ResourceId"),
                })
                .Select(group => new AllocationData()
                {
                    ResourceId = group.Key.ResourceId,
                    WorkItem = group.Key.Id,
                    SubWorkItem = string.Join(",", group.Select(row => row.Field<string>("SubWorkItem"))),
                    // Add other aggregated properties here
                    AllocationID = UGITUtility.ObjectToString(group.First().Field<long>("ID")),
                    ResourceUser = group.First().Field<string>("ResourceUser"),
                    WorkItemType = group.First().Field<string>("WorkItemType"),
                    WorkItemLink = group.First().Field<string>("WorkItemLink"),
                    AllocationStartDate = group.Min(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue),
                    AllocationEndDate = group.Max(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue),
                    PctAllocation = group.Sum(row => row.Field<double>("PctAllocation")),
                    WorkItemID = string.Join(",", group.Select(row => row.Field<long>("WorkItemID")).Distinct()),
                    ShowEditButton = false,
                    ShowPartialEdit = false,
                    PlannedStartDate = group.Min(row => row.Field<DateTime?>("PlannedStartDate") ?? DateTime.MinValue),
                    PlannedEndDate = group.Max(row => row.Field<DateTime?>("PlannedEndDate") ?? DateTime.MinValue),
                    PctPlannedAllocation = group.Sum(row => row.Field<double?>("PctPlannedAllocation") ?? 0),
                    EstStartDate = group.Min(row => row.Field<DateTime?>("EstStartDate") ?? DateTime.MinValue),
                    EstEndDate = group.Max(row => row.Field<DateTime?>("EstEndDate") ?? DateTime.MinValue),
                    PctEstimatedAllocation = group.Sum(row => row.Field<double?>("PctEstimatedAllocation") ?? 0),
                    SoftAllocation = string.Join(",", group.Select(row=>row.Field<bool>("SoftAllocation").ToString())),
                    AllStartDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue)),
                    AllEndDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue))
                }).ToArray();



            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (hdndisplayMode.Value == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView();
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView();


            ILookup<object, DataRow> dtAllocLookups = null;
            ILookup<object, DataRow> dtWeeklyLookups = null;
            //Grouping on AllocationMonthly datatable based on ResourceWorkItemLookup
            if (dtAllocationMonthly != null && dtAllocationMonthly.Rows.Count > 0)
                dtAllocLookups = dtAllocationMonthly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.ResourceWorkItemLookup]);
            //Grouping on AllocationWeekly datatable based on WorkItemID
            if (dtAllocationWeekly != null && dtAllocationWeekly.Rows.Count > 0)
                dtWeeklyLookups = dtAllocationWeekly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.WorkItemID]);

            Dictionary<string, DataRow> tempTicketCollection = new Dictionary<string, DataRow>();
            #region data creating
            UserProfile userDetails = null;
            List<string> lstSelectedDepartment = UGITUtility.ConvertStringToList(selecteddepartment, Constants.Separator6);
            List<string> departmentTempUserIds = lstUProfile.Where(a => lstSelectedDepartment.Exists(d => d == a.Department)).Select(x => x.Id).ToList();
            List<string> managerTempUserIds = lstUProfile.Where(a => a.ManagerID == selectedManager).Select(x => x.Id).ToList();
            List<UserProfile> lstUsersHavingAllocations = new List<UserProfile>();
            if (!string.IsNullOrEmpty(selecteddepartment))
            {
                lstActiveUsersIds = lstUProfile.Where(a => lstSelectedDepartment.Exists(d => d == a.Department)
                && a.UGITStartDate <= dateTo && a.UGITEndDate >= dateFrom).ToList();
            }
            else
                lstActiveUsersIds = lstUProfile.Where(a => a.UGITStartDate <= dateTo && a.UGITEndDate >= dateFrom).ToList();

            if (!string.IsNullOrEmpty(selectedManager) && selectedManager != "0")
                lstActiveUsersIds = lstActiveUsersIds.Where(a => a.ManagerID == selectedManager).ToList();

            //            var allocationData = lstActiveUsersIds.AsEnumerable()
            //.GroupBy(row => new
            //{
            //Id = "None",
            //ResourceId = row.Id,
            //})
            //.Select(group => new AllocationData()
            //{
            //ResourceId = group.Key.ResourceId,
            //WorkItem = "None",
            //SubWorkItem = "",
            //        // Add other aggregated properties here
            //        AllocationID = "",
            //ResourceUser = group.Key.ResourceId,
            //WorkItemType = "",
            //WorkItemLink = "",
            //AllocationStartDate = group.First().UGITStartDate,
            //AllocationEndDate = group.First().UGITEndDate,
            //PctAllocation = 0,
            //WorkItemID = "",
            //ShowEditButton = false,
            //ShowPartialEdit = false,
            //PlannedStartDate = DateTime.MinValue,
            //PlannedEndDate = DateTime.MinValue,
            //PctPlannedAllocation = 0,
            //EstStartDate = DateTime.MinValue,
            //EstEndDate = DateTime.MinValue,
            //PctEstimatedAllocation = 0,
            //SoftAllocation = "false",
            //AllStartDates = UGITUtility.ObjectToString(group.First().UGITStartDate),
            //AllEndDates = UGITUtility.ObjectToString(group.First().UGITEndDate)
            //}).ToArray();
            //            var alldata = allocationGroupData.Concat(allocationData);

            var allocationData = dtResult.AsEnumerable()
            .GroupBy(row => new
            {
                ResourceId = row.Field<string>("ResourceId"),
            })
            .Select(group => new AllocationData()
            {
                ResourceId = group.Key.ResourceId,
                SubWorkItem = "",
                AllocationID = "",
                ResourceUser = group.First().Field<string>("ResourceUser"),
                WorkItemType = "",
                WorkItemLink = "",
                AllocationStartDate = group.Min(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue),
                AllocationEndDate = group.Max(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue),
                PctAllocation = group.Sum(row => row.Field<double>("PctAllocation")),
                WorkItemID = "",
                ShowEditButton = false,
                ShowPartialEdit = false,
                PlannedStartDate = DateTime.MinValue,
                PlannedEndDate = DateTime.MinValue,
                PctPlannedAllocation = 0,
                EstStartDate = DateTime.MinValue,
                EstEndDate = DateTime.MinValue,
                PctEstimatedAllocation = 0,
                SoftAllocation = "false",
                AllStartDates = "",
                AllEndDates = ""
            }).ToArray();


            TicketManager ticketManager = new TicketManager(applicationContext);
            ModuleViewManager moduleManager = new ModuleViewManager(applicationContext);

            foreach (var dr in allocationGroupData)
            {
                string userid = Convert.ToString(dr.ResourceId);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = lstActiveUsersIds.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                if (userDetails == null || (!chkAll.Checked && !userDetails.Enabled))
                    continue;
                lstUsersHavingAllocations.Add(userDetails);

                List<string> lstWorkItemIds = UGITUtility.ConvertStringToList(dr.WorkItemID, Constants.Separator6);
                DataRow newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr.ResourceUser);
                }
                newRow[DatabaseObjects.Columns.Name] = userDetails.Name;
                newRow[DatabaseObjects.Columns.SoftAllocation] = UGITUtility.StringToBoolean(dr.SoftAllocation);
                newRow[DatabaseObjects.Columns.SubWorkItem] = UGITUtility.ObjectToString(dr.SubWorkItem);
                newRow["AllStartDates"] = UGITUtility.ObjectToString(dr.AllStartDates);
                newRow["AllEndDates"] = UGITUtility.ObjectToString(dr.AllEndDates);

                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    DataRow[] filterworkitemrow = workitems.AsEnumerable().Where(x => lstWorkItemIds.Contains(UGITUtility.ObjectToString(x.Field<long>("ID")))).ToArray();
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }

                if (drWorkItem != null && drWorkItem[DatabaseObjects.Columns.WorkItem] != null)
                {
                    string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                    string[] arrayModule = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    string moduleName = string.Empty;
                    if (UGITUtility.IsValidTicketID(workItem))
                        moduleName = uHelper.getModuleNameByTicketId(workItem);

                    UGITModule module = ModuleViewManager.LoadByName(moduleName, true);
                    Page.Title = module != null ? module.ModuleName + "Tickets" : "";
                    DataTable moduledt = UGITUtility.ObjectToData(module);
                    if (moduledt.Rows.Count > 0)
                        moduleRow = UGITUtility.ObjectToData(module).Rows[0];

                    DataRow moduleDetail = moduleRow;

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        if (arrayModule.Contains(moduleName) || arrayModule.Length == 0)
                        {

                            module = moduleManager.GetByName(moduleName);
                            if (module == null)
                                continue;

                            //check for active modules.
                            if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                                continue;
                            DataRow dataRow = null;
                            if (tempTicketCollection.ContainsKey(workItem))
                                dataRow = tempTicketCollection[workItem];
                            else
                            {
                                dataRow = dtAllModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == workItem);
                                if (dataRow != null)
                                {
                                    tempTicketCollection.Add(workItem, dataRow);
                                }
                            }
                            if (dataRow != null)
                            {
                                string ticketID = workItem;
                                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 50);
                                if (!string.IsNullOrEmpty(ticketID))
                                {
                                    string prefixcolumn = uHelper.getAltTicketIdField(applicationContext, moduleName);
                                    string prefixTitle = string.Empty;
                                    if (UGITUtility.IfColumnExists(dataRow, prefixcolumn))
                                        prefixTitle = Convert.ToString(dataRow[prefixcolumn]);
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", string.IsNullOrEmpty(prefixTitle) ? ticketID : prefixTitle, title);
                                    newRow[DatabaseObjects.Columns.ERPJobID] = UGITUtility.ObjectToString(dataRow[DatabaseObjects.Columns.ERPJobID]);
                                    newRow[DatabaseObjects.Columns.CRMCompanyLookup] = UGITUtility.ObjectToString(dataRow[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                    if (UGITUtility.StringToBoolean(dataRow[DatabaseObjects.Columns.Closed]))
                                        newRow[DatabaseObjects.Columns.Closed] = true;
                                    newRow[DatabaseObjects.Columns.PreconStartDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.PreconStartDate]);
                                    newRow[DatabaseObjects.Columns.PreconEndDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.PreconEndDate]);
                                    newRow[DatabaseObjects.Columns.EstimatedConstructionStart] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    newRow[DatabaseObjects.Columns.EstimatedConstructionEnd] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                                    newRow[DatabaseObjects.Columns.CloseoutDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.CloseoutDate]);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;// title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);

                                //condition for add new column for breakup gantt chart...
                                string plannedStartDate = Convert.ToString(dr.PlannedStartDate);
                                string plannedEndDate = Convert.ToString(dr.PlannedEndDate);

                                string estStartDate = Convert.ToString(dr.EstStartDate);
                                string estEndDate = Convert.ToString(dr.EstEndDate);
                                string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}' AND {DatabaseObjects.Columns.Id}='{userid}'";
                                if (data != null && data.Rows.Count > 0)
                                {
                                    DataRow[] row = data.Select(expression);

                                    if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr.PlannedStartDate;
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr.PlannedEndDate;
                                        newRow["ExtendedDateAssign"] = dr.PlannedStartDate + Constants.Separator1 + dr.PlannedEndDate;

                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr.PctPlannedAllocation);
                                    }

                                    if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate) && estEndDate != "01-01-1753 00:00:00")
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                        newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;

                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' style='color:black;font-weight:800px !important;'>{2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;


                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp);
                                        }
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            foreach (string s in lstWorkItemIds)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                if (dttemp != null && dttemp.Length > 0)
                                                    ViewTypeAllocation(data, newRow, dttemp);
                                            }
                                        }
                                    }

                                    data.Rows.Add(newRow);
                                    //}
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr.PlannedStartDate;
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr.PlannedEndDate;
                                        newRow["ExtendedDateAssign"] = dr.PlannedStartDate + Constants.Separator1 + dr.PlannedEndDate;
                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr.PctPlannedAllocation);
                                    }

                                    if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                        newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' title='{2}' style='color:black;font-weight:800px !important;'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, UGITUtility.TruncateWithEllipsis(title, 40), title ?? string.Empty);

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp);

                                        }
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            foreach (string s in lstWorkItemIds)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                if (dttemp != null && dttemp.Length > 0)
                                                {
                                                    ViewTypeAllocation(data, newRow, dttemp);
                                                }
                                            }

                                        }
                                    }
                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    else
                    {
                        ////code commented so that resource utlization screen data and pdf data match
                        ////This condition set to remove PTO types but core only use PTO type other then module type so return for all non module types
                        //if (ConfigVariableMGR.GetValueAsBool(ConfigConstants.HidePTOonGantt))
                        //    continue;

                        if (arrayModule.Length > 0 && !arrayModule.Contains(Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType])))
                            continue;

                        if (data != null && data.Rows.Count > 0)
                        {
                            string expression = string.Format("{0}= '{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            DataRow[] row = data.Select(expression);

                            if (row != null && row.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                                {
                                    row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr.EstStartDate)))
                                        row[0][DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr.EstEndDate)))
                                        row[0][DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, row[0], dttemp);
                                    }
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, row[0], dttemp, false);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                                if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                                }

                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow["ProjectNameLink"] = workItem;
                                newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                                newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;

                                    newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp);
                                    }
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, newRow, dttemp, false);
                                            }
                                        }
                                    }
                                }
                                data.Rows.Add(newRow);
                            }
                        }
                        else
                        {
                            string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                            if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                            }
                            else
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            }

                            newRow[DatabaseObjects.Columns.TicketId] = workItem;
                            newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                            newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                            newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);
                            newRow["ProjectNameLink"] = workItem;

                            if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;

                                newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                            }

                            if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                            {
                                foreach (string s in lstWorkItemIds)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, newRow, dttemp);
                                }
                            }
                            else
                            {
                                if (dtAllocLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, newRow, dttemp, false);

                                        }
                                    }
                                }
                            }

                            data.Rows.Add(newRow);
                        }
                    }
                }
            }


            DateTime dateF = Convert.ToDateTime(dateFrom);
            DateTime dateT = Convert.ToDateTime(dateTo);
            DateTime allocationMonthStartDate = DateTime.MinValue;
            DateTime allocationMonthEndDate = DateTime.MinValue;

            foreach (UserProfile user in lstActiveUsersIds)
            {
                DataRow newRow = data.NewRow();
                bool rowHasData = false;
                newRow[DatabaseObjects.Columns.Id] = user.Id;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                newRow[DatabaseObjects.Columns.Resource] = user.Name;
                newRow[DatabaseObjects.Columns.Name] = user.Name;

                var allocGroupRow = allocationData.FirstOrDefault(x => x.ResourceId == user.Id);
                if (allocGroupRow != null)
                { 
                    allocationMonthStartDate = new DateTime(allocGroupRow.AllocationStartDate.Value.Year, allocGroupRow.AllocationStartDate.Value.Month, 1);
                    allocationMonthEndDate = new DateTime(allocGroupRow.AllocationEndDate.Value.Year, allocGroupRow.AllocationEndDate.Value.Month, 1);
                }

                for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    if ((user.UGITStartDate <= dt && dt < user.UGITEndDate ) )
                    {
                        if (allocGroupRow != null && (dt >= allocationMonthStartDate && dt <= allocationMonthEndDate))
                            continue;
                        if (data.Columns.Contains(dt.ToString("MMM-dd-yy") + "E"))
                        {
                            newRow[dt.ToString("MMM-dd-yy") + "E"] = 0;
                            rowHasData = true;
                        }
                    }

                }
                if(rowHasData)
                    data.Rows.Add(newRow);
            }

            #endregion
            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC, {2} ASC, {3} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.Project, DatabaseObjects.Columns.Title);
            return data;
        }

        private void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, bool Assigned = true)
        {
            //double yearquaAllocE = 0;
            //double yearquaAllocA = 0;

            //double halfyearquaAllocE1 = 0;
            //double halfyearquaAllocE2 = 0;
            //double halfyearquaAllocA1 = 0;
            //double halfyearquaAllocA2 = 0;

            //double quaterquaAllocE1 = 0;
            //double quaterquaAllocE2 = 0;
            //double quaterquaAllocE3 = 0;
            //double quaterquaAllocE4 = 0;
            //double quaterquaAllocA1 = 0;
            //double quaterquaAllocA2 = 0;
            //double quaterquaAllocA3 = 0;
            //double quaterquaAllocA4 = 0;

            foreach (DataRow rowitem in dttemp)
            {
                if (hdndisplayMode.Value == "Weekly")
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"])
                            + Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A"] = UGITUtility.StringToDouble(newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A"]) 
                                + Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        DateTime AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        DateTime allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) >= AllocationMonthStartDate && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) <= allocationEndDate)
                        {
                            if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"]) +
                                    Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                            }
                        }
                    }
                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A"] = UGITUtility.StringToDouble(newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A"]) 
                                + Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                        }
                    }

                }
            }

        }

        private DataTable LoadAllocationMonthlyView()
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(applicationContext);
            return allocationMonthlyManager.LoadAllocationMonthlyView(Convert.ToDateTime(dateFrom),
                Convert.ToDateTime(dateTo), chkIncludeClosed.Checked);

        }

        private DataTable LoadAllocationWeeklySummaryView()
        {
            try
            {
                DateTime dtFrom = dateFrom;
                DateTime dtTo = dateTo;

                string commQuery = string.Empty;
                ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(applicationContext);
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo));

                DataTable dtAllocationWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'" + "AND " + commQuery);
                return dtAllocationWeekWise;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return null;
        }

        //private void LoadAllocationTimeline()
        //{
        //    if (pnlAllocationTimeline.Controls.Count < 1)
        //    {
        //        ResourceAllocationGridNew _allocationGantt = Page.LoadControl("~/ControlTemplates/RMONE/ResourceAllocationGridNew.ascx") as ResourceAllocationGridNew;

        //        _allocationGantt.UserAll = UGITUtility.ObjectToString(Request["userall"]);
        //        //_allocationGantt.IncludeClosed = UGITUtility.ObjectToString(Request["includeClosed"]);
        //        //if (selectedUsersList != null && selectedUsersList.Count > 0)
        //        //{
        //        //    List<string> userids = selectedUsersList.Select(x => x.Id).ToList();
        //        //    _allocationGantt.SelectedUsers = UGITUtility.ConvertListToString(userids, Constants.Separator6);
        //        //    _allocationGantt.SelectedUser = _allocationGantt.SelectedUsers;
        //        //}
        //        //else if (!string.IsNullOrEmpty(selectedUsersId))
        //        //    _allocationGantt.SelectedUser = selectedUsersId;
        //        //else
        //        _allocationGantt.SelectedUser = UGITUtility.ObjectToString(cmbResourceManager.SelectedItem?.Value);

        //        if (!string.IsNullOrEmpty(_allocationGantt.SelectedUser))
        //        {
        //            if (_allocationGantt.SelectedUser != "0")
        //            {
        //                UGITUtility.CreateCookie(Response, "SelectedUser", _allocationGantt.SelectedUser);
        //            }
        //            else
        //            {
        //                UGITUtility.CreateCookie(Response, "SelectedUser", string.Empty);
        //            }
        //        }
        //        else
        //        {
        //            UGITUtility.CreateCookie(Response, "SelectedUser", string.Empty);
        //        }
        //        //UGITUtility.CreateCookie(Response, "SelectedUser", "");
        //        _allocationGantt.SelectedYear = "2023";
        //        _allocationGantt.Height = 550;
        //        pnlAllocationTimeline.Controls.Add(_allocationGantt);
        //    }
        //    pnlAllocationTimeline.ClientVisible = true;
        //}

        protected void gvPreview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string foreColor = "#000000";
            string Estimatecolor = "#24b6fe";
            string Assigncolor = "#24b6fe";
            string moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
            if (lstEstimateColorsAndFontColors != null && lstEstimateColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                    foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                }
            }

            if (lstAssignColorsAndFontColors != null && lstAssignColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstAssignColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                Assigncolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                //foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
            }
            if (e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != "AllocationType" && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && e.DataColumn.FieldName != DatabaseObjects.Columns.Title)
            {
                int defaultBarH = 12;
                
                string html;
                DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                string type = UGITUtility.ObjectToString(e.GetValue("ProjectNameLink"));

                string tickId = string.Empty;
                string allStartDates = string.Empty;
                string allEndDates = string.Empty;
                //DateTime oPreconStart = DateTime.MinValue;
                //DateTime oPreconEnd = DateTime.MinValue;
                //DateTime oConstStart = DateTime.MinValue;
                //DateTime oConstEnd = DateTime.MinValue;
                //DateTime oCloseout = DateTime.MinValue;

                DateTime aStartDate = DateTime.MinValue;
                DateTime aEndDate = DateTime.MinValue;
                DateTime aPreconStart = DateTime.MinValue;
                DateTime aPreconEnd = DateTime.MinValue;
                DateTime aConstStart = DateTime.MinValue;
                DateTime aConstEnd = DateTime.MinValue;
                DateTime aCloseout = DateTime.MinValue;
                string id = string.Empty;
                bool aSoftAllocation = true;
                string project = string.Empty;
                string allocId = string.Empty;
                if (e.VisibleIndex > 0)
                {
                    DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                    tickId = row[DatabaseObjects.Columns.TicketId].ToString();
                    aStartDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]);
                    aEndDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationEndDate]);
                    aPreconStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconStartDate]);
                    aPreconEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconEndDate]);
                    aConstStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    aConstEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    aCloseout = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.CloseoutDate]);
                    aSoftAllocation = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.SoftAllocation]);
                    project = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                    id = row[DatabaseObjects.Columns.Id].ToString();
                    allocId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.AllocationID]);

                    allStartDates = UGITUtility.ObjectToString(row["AllStartDates"]);
                    allEndDates = UGITUtility.ObjectToString(row["allEndDates"]);
                }

                //if (tickId != "CPR-23-000628")
                //    return;
                DateTime dateF = Convert.ToDateTime(dateFrom);
                DateTime dateT = Convert.ToDateTime(dateTo);
                for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    html = "";
                    if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                    {
                        string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                        string roundleftbordercls = string.Empty;
                        string roundrightbordercls = string.Empty;
                        string backgroundColor = string.Empty;
                        if (aStartDate == DateTime.MinValue || aEndDate == DateTime.MinValue)
                            return;
                        // classed to set round corners on bar end points
                        if (hdndisplayMode.Value != "Weekly")
                        {
                            if (aStartDate.Year < dt.Year && dt.Month == 1)
                            {
                                roundleftbordercls = "RoundLeftSideCorner";
                            }
                            if (aEndDate.Year > dt.Year && dt.Month == 12)
                            {
                                roundrightbordercls = "RoundRightSideCorner";
                            }
                            if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                            {
                                roundleftbordercls = "RoundLeftSideCorner";
                            }
                            if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                            {
                                roundrightbordercls = "RoundRightSideCorner";
                            }
                        }
                        else if (hdndisplayMode.Value == "Weekly")
                        {

                            if (aStartDate >= dt && aStartDate.AddDays(-6) < dt)
                            {
                                roundleftbordercls = "RoundLeftSideCorner";
                            }
                            if (aEndDate >= dt && aEndDate <= dt.AddDays(6))
                            {
                                roundrightbordercls = "RoundRightSideCorner";
                            }
                        }

                        backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt);

                        if (dt.Month == aConstStart.Month && dt.Month == aPreconEnd.Month)
                        {
                            if (aEndDate <= aPreconEnd)
                            {
                                backgroundColor = "preconbgcolor";
                            }
                        }

                        if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                        {
                            if (aSoftAllocation)
                                backgroundColor = "softallocationbgcolor";
                            if (moduleName != null && moduleName.Equals("Time Off"))
                            {
                                //backgroundColor = "ptobgcolor";
                                html = GeneratePtoCard(dt, id, defaultBarH, type);
                                e.Cell.CssClass += " ptoAlignmentClass";
                            }
                            else
                            {
                                estAlloc = estAlloc + "% <br>";

                                int NoOfDays = 0;
                                int remainingDays = 0;
                                string widthEstAlloc = string.Empty;

                                if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                {
                                    NoOfDays = aStartDate.Day;
                                    remainingDays = 30 - NoOfDays;
                                    widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                                    roundleftbordercls = "RoundLeftSideCorner";
                                    backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, aStartDate);
                                }
                                else if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                                {
                                    NoOfDays = aEndDate.Day;
                                    widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                                    roundrightbordercls = "RoundRightSideCorner";
                                    roundleftbordercls = "";
                                    backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, aEndDate);
                                }


                                if (dt.Month == aConstStart.Month && dt.Month == aPreconEnd.Month)
                                {
                                    if (aEndDate <= aPreconEnd)
                                    {
                                        NoOfDays = aEndDate.Day;
                                        widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                                    }
                                }

                                if (!string.IsNullOrEmpty(widthEstAlloc))
                                {
                                    if (Convert.ToInt32(widthEstAlloc) >= 50)
                                    {
                                        if (roundleftbordercls == "RoundLeftSideCorner")
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:right;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                        }
                                        else if (roundrightbordercls == "RoundRightSideCorner")
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                        }
                                        else
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                        }
                                    }
                                    else
                                    {
                                        if (roundleftbordercls == "RoundLeftSideCorner")
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:right;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'></div>";
                                        }
                                        else if (roundrightbordercls == "RoundRightSideCorner")
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'></div>";
                                        }
                                        else
                                        {
                                            html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                        }
                                    }
                                }
                                else
                                {
                                    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                }
                            }
                        }
                        else
                        {
                            html = $" <div class='{roundleftbordercls} {roundrightbordercls}' style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                        }
                        e.Cell.Text = html;
                        //sets alternate color to columns
                        if (e.DataColumn.VisibleIndex % 2 == 0)
                            e.Cell.BackColor = Color.WhiteSmoke;
                    }
                }
            }

        }

        protected void gvPreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                // Retrieve updated data from the data source
                DataTable data = GetAllocationData();
                PrepareAllocationGridGantt();
                // Bind the updated data to the grid

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, ex.Message);
            }
        }

        private void PrepareAllocationGridGantt()
        {
            gvPreview.Columns.Clear();
            gvPreview.GroupSummary.Clear();
            gvPreview.TotalSummary.Clear();

            int fullScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth"));
            int noOfColumns = 14;
            if (hdndisplayMode.Value == "Weekly")
                noOfColumns = 16;
            if (fullScreenWidth > 0)
            {
                int remainingwidthforcols = fullScreenWidth - 300;
                MonthColWidth = UGITUtility.StringToInt(remainingwidthforcols / noOfColumns);
            }

            gvPreview.Templates.GroupRowContent = new GridGroupRowContentTemplateNew(userEditPermisionList, isResourceAdmin, rbtnPercentage.Checked, chkIncludeClosed.Checked, selectedCategory, MonthColWidth);
            gvPreview.SettingsPager.AlwaysShowPager = false;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;


            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Resource;
            colId.Caption = DatabaseObjects.Columns.Resource;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            //colId.Width = new Unit("200px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.GroupIndex = 0;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.ExportCellStyle.Font.Names = new string[] { "Arial" };
            colId.ExportCellStyle.Font.Bold = true;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Name;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.Visible = false;
            //colId.SortOrder = ColumnSortOrder.Ascending;
            colId.Settings.SortMode = ColumnSortMode.Custom;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            colId.ExportCellStyle.Font.Names = new string[] { "Arial" };
            colId.ExportCellStyle.Font.Bold = true;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Id;
            colId.Caption = DatabaseObjects.Columns.Id;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("300px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.Visible = false;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            colId.ExportCellStyle.Font.Names = new string[] { "Arial" };
            colId.ExportCellStyle.Font.Bold = true;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Title;
            colId.HeaderCaptionTemplate = new GridTitleHeaderTemplate(UGITUtility.ObjectToString(Request["SelectedUser"]));
            colId.Caption = "";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            //colId.Width = new Unit("400px");
            colId.ExportWidth = 400;
            colId.ExportCellStyle.Font.Names = new string[] { "Arial" };
            colId.ExportCellStyle.Font.Size = 9;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            gvPreview.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.Title);

            var IsShowTotalCapicityFTE = true; // ConfigVariableMGR.GetValueAsBool(ConfigConstants.ShowTotalCapicityFTE);
            if (IsShowTotalCapicityFTE)
            {
                ASPxSummaryItem item = new ASPxSummaryItem(DatabaseObjects.Columns.Title, SummaryItemType.Custom);
                item.Tag = "ResourceItem";

                gvPreview.TotalSummary.Add(item);
            }

            GridViewBandColumn bdCol = new GridViewBandColumn();
            bdCol.ExportCellStyle.Font.Names = new string[] { "Arial" };
            bdCol.ExportCellStyle.Font.Bold = true;
            string currentDate = string.Empty;


            for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                int weeks = Convert.ToInt32((Convert.ToDateTime(hdndtto.Value) - Convert.ToDateTime(hdndtfrom.Value)).TotalDays / 7);

                if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                {
                    bdCol.Name = dt.ToString("MMM-dd-yy") + "E";
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumn(bdCol, hdndisplayMode.Value);
                    gvPreview.Columns.Add(bdCol);
                    bdCol = new GridViewBandColumn();
                }

                if (dt.ToString("yyyy") != currentDate)
                {
                    bdCol.Caption = dt.ToString("yyyy");
                    bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    bdCol.HeaderStyle.Font.Bold = true;
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumn(bdCol, hdndisplayMode.Value);
                    currentDate = dt.ToString("yyyy");
                }

                GridViewDataTextColumn ColIdData = new GridViewDataTextColumn();
                if (hdndisplayMode.Value == "Weekly")
                {
                    if (dt.DayOfWeek != DayOfWeek.Monday)
                        continue;
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("dd-MMM");
                    if (weeks == 4)
                    {
                        ColIdData.ExportWidth = 130;
                    }
                    else
                    {
                        ColIdData.ExportWidth = 110;
                    }
                }
                else if (hdndisplayMode.Value == "Quarterly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "HalfYearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "Yearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                    ColIdData.ExportWidth = 56;
                }
                ColIdData.CellStyle.CssClass = "timeline-td";
                ColIdData.UnboundType = DevExpress.Data.UnboundColumnType.String;
                ColIdData.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.HeaderStyle.Font.Bold = true;

                ColIdData.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.HeaderTemplate = new MonthDownHeaderTemplate(ColIdData, hdndisplayMode.Value);

                //ColIdData.Width = new Unit("160" + "px");
                //ColIdData.ExportWidth = 56;
                ColIdData.ExportCellStyle.Font.Names = new string[] { "Arial" };
                ColIdData.ExportCellStyle.Font.Bold = true;

                CreateGridSummaryColumn(gvPreview, dt.ToString("MMM-dd-yy") + "E");

                bdCol.Columns.Add(ColIdData);


                ASPxSummaryItem itemCFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                itemCFTE.DisplayFormat = "N2";
                gvPreview.TotalSummary.Add(itemCFTE);

                if (IsShowTotalCapicityFTE)
                {
                    ASPxSummaryItem itemTFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                    itemTFTE.Tag = "TFTE";
                    itemTFTE.DisplayFormat = "N2";
                    gvPreview.TotalSummary.Add(itemTFTE);
                }
            }
            gvPreview.Columns.Add(bdCol);

            if (Height != null && Height.Value > 0)
            {
                gvPreview.Settings.VerticalScrollableHeight = Convert.ToInt32(Height.Value - 260);
                gvPreview.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
        }

        private void CreateGridSummaryColumn(DevExpress.Web.ASPxGridView gvPreview, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
            summary.ShowInGroupFooterColumn = column;
            summary.DisplayFormat = "{0}";
            gvPreview.GroupSummary.Add(summary);

            if (column == DatabaseObjects.Columns.AllocationStartDate)
            {
                ASPxSummaryItem summaryStartDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryStartDate.ShowInGroupFooterColumn = column;
                summaryStartDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryStartDate);
            }
            if (column == DatabaseObjects.Columns.AllocationEndDate)
            {
                ASPxSummaryItem summaryEndDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryEndDate.ShowInGroupFooterColumn = column;
                summaryEndDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryEndDate);
            }

        }

        public string GeneratePtoCard(DateTime currentDate, string userId, int defaultBarH, string workItem)
        {
            string html = string.Empty;
            string marginLeft = string.Empty;
            DateTime startDate = currentDate;
            DateTime endDate = DateTime.MinValue;
            Tuple<int, int> addDays = new Tuple<int, int>(10, 20);
            if (hdndisplayMode.Value == "Weekly")
            {
                endDate = currentDate.AddDays(6);
                addDays = new Tuple<int, int>(2, 4);
            }
            else if (hdndisplayMode.Value == "Quarterly")
            {
                endDate = startDate.AddMonths(3);
                int firstInterval = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                int secondInterval = firstInterval + DateTime.DaysInMonth(startDate.Year, startDate.Month + 1);
                addDays = new Tuple<int, int>(firstInterval, secondInterval);
            }
            else
            {
                int lastDayOfMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                endDate = new DateTime(startDate.Year, startDate.Month, lastDayOfMonth);
            }
            DataTable dt = ResourceAllocationManager.LoadRawTableByResource(userId.Split(',').ToList(), 4, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] monthlyData = dt.Select($"{DatabaseObjects.Columns.AllocationStartDate}<='{endDate}' and {DatabaseObjects.Columns.AllocationEndDate} >= '{startDate}'" +
                                            $"and {DatabaseObjects.Columns.WorkItemType}='Time Off' and {DatabaseObjects.Columns.WorkItemLink}='{workItem}'");

                if (monthlyData.Count() > 0)
                {
                    foreach (DataRow data in monthlyData)
                    {
                        string taskId = data.Field<long>(DatabaseObjects.Columns.Id).ToString();
                        string resourceName = data.Field<string>(DatabaseObjects.Columns.Resource).ToString().Replace("'", "`");
                        DateTime fromDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate);
                        DateTime toDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate);
                        if ((fromDate >= startDate && fromDate <= startDate.AddDays(addDays.Item1)) || fromDate < startDate)
                        {
                            marginLeft = "0%";
                        }
                        else if (fromDate > startDate.AddDays(addDays.Item1) && fromDate <= startDate.AddDays(addDays.Item2))
                        {
                            marginLeft = "30%";
                        }
                        else
                        {
                            marginLeft = "60%";
                        }
                        if (fromDate == toDate)
                        {
                            //html += $"<a href=\"#\" onclick=\"OpenTimeOffAllocation('{taskId}', '{resourceName}')\"  >";
                            //html += $"<div class='RoundLeftSideCorner RoundRightSideCorner ptobgcolor jqtooltip' title = '{fromDate.ToString("dd MMM, yyyy")}' style='width:40%;margin-bottom:5%;color:white;margin-left:{marginLeft};height:{defaultBarH}px;padding-top:5px;font-weight:500;'>1</div>";
                            //html += "</a>";
                            html = "1";
                        }
                        else
                        {
                            double dayDiff = 0;
                            dayDiff = GetOverlappingDays(startDate, endDate, fromDate, toDate);
                            dayDiff += 1;
                            //html += $"<a href=\"#\" onclick=\"OpenTimeOffAllocation('{taskId}', '{resourceName}')\"  >";
                            //html += $"<div class='RoundLeftSideCorner RoundRightSideCorner ptobgcolor jqtooltip' title = '{fromDate.ToString("dd MMM, yyyy")} to {toDate.ToString("dd MMM, yyyy")}' style='width:40%;margin-bottom:5%;color:white;margin-left:{marginLeft};height:{defaultBarH}px;padding-top:5px;font-weight:500;'>{dayDiff}</div>";
                            //html += "</a>";
                            html = Convert.ToString(dayDiff);
                        }
                    }
                }
            }
            return html;
        }

        public static double GetOverlappingDays(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            DateTime maxStart = firstStart > secondStart ? firstStart : secondStart;
            DateTime minEnd = firstEnd < secondEnd ? firstEnd : secondEnd;
            TimeSpan interval = minEnd - maxStart;
            double returnValue = interval > TimeSpan.FromSeconds(0) ? interval.TotalDays : 0;
            return returnValue;
        }
        protected void ExportCallback_Callback(object source, CallbackEventArgs e)
        {
            hndYear = UGITUtility.GetCookieValue(Request, "year");
            string deptDetailInfo;
            string deptShortInfo;

            if (ddlDepartment.dropBox.Text.Contains(",") || ddlDepartment.dropBox.Text.Contains(";")|| ddlDepartment.dropBox.Text == "<Various>")
            {
                deptShortInfo = "Various Departments";
                deptDetailInfo = RMMSummaryHelper.GetSelectedDepartmentsInfo(applicationContext, Convert.ToString(hdnaspDepartment.Value), enableDivision);
            }
            else if (ddlDepartment.dropBox.Text == "All")
            {
                deptShortInfo = "All Departments";
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            else
            {
                deptShortInfo = ddlDepartment.dropBox.Text;
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            DateTime fromDate = Convert.ToDateTime(hdndtfrom.Value);
            DateTime toDate = Convert.ToDateTime(hdndtto.Value);
            string pageHeaderInfo = string.Format("Resource Utilization: {0}; {1} to {2}", deptShortInfo, uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));

            string reportFooterInfo = string.Format("\nSelection Criteria \n   {0}: {1}\n   {2}: {3} to {4}", "Departments", deptDetailInfo, "Date Range", uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));
            
            if (userGroupGridLookup?.GridView?.GetSelectedFieldValues("Id")?.Count > 0)
            {
                List<string> gIds = userGroupGridLookup.GridView.GetSelectedFieldValues("Id").Select(x => (string)x).ToList();
                string roleName = roleManager.Load(x => gIds.Contains(x.Id))?.Select(x => x.Name)?.Aggregate((x, y) => x + ", " + y) ?? string.Empty;
                reportFooterInfo = reportFooterInfo + string.Format("\n {0}: {1}", "Role", roleName);
            }
            if (UGITUtility.ObjectToString(cmbResourceManager.Value) != "0")
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n   {0}: {1}", "Manager", ObjUserProfileManager.GetUserNameById(UGITUtility.ObjectToString(cmbResourceManager.Value)));
            }
            if (!string.IsNullOrEmpty(glType.Text))
            {
                reportFooterInfo = reportFooterInfo + string.Format("\n   {0}: {1}", "Type", glType.Text);
            }
            if (chkAll.Checked)
                reportFooterInfo = reportFooterInfo + string.Format("\n   All Resources Included.");
            if (chkIncludeClosed.Checked)
                reportFooterInfo = reportFooterInfo + string.Format("\n   Closed Projects Included.");

            //reportFooterInfo = reportFooterInfo + string.Format("\n   {0}: {1} to {2}", "Date Range", uHelper.GetDateStringInFormat(applicationContext, fromDate, false), uHelper.GetDateStringInFormat(applicationContext, toDate, false));

            DataTable dtGanttExport = new DataTable();
            dtGanttExport = GetAllocationDataGantt();
            gvPreview.DataSource = dtGanttExport;
            gvPreview.DataBind();

            ASPxGridViewExporter1.Landscape = true;

            PrintingSystemBase ps = new PrintingSystemBase();
            ps.PageSettings.Landscape = true;
            ps.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            ps.PageSettings.LeftMargin = 5;
            ps.PageSettings.RightMargin = 5;
            ps.PageSettings.TopMargin = 5;
            ps.PageSettings.BottomMargin = 5;

            PrintableComponentLinkBase lnk = new PrintableComponentLinkBase(ps);
            lnk.Component = ASPxGridViewExporter1;
            lnk.PrintingSystemBase.Document.AutoFitToPagesWidth = 1;
            lnk.PrintingSystemBase.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;

            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { lnk });
            compositeLink.Landscape = true;

            // Handle the CreateMarginalHeaderArea event to add the header text on every page
            compositeLink.CreateMarginalHeaderArea += (sender, _headerArgs) =>
            {
                TextBrick headerBrick = new TextBrick();
                headerBrick.BackColor = Color.Transparent;
                headerBrick.BorderColor = Color.Transparent;
                headerBrick.Text = pageHeaderInfo;
                headerBrick.Font = new Font("Arial", 10); // Customize font and size as needed

                SizeF textSize = _headerArgs.Graph.MeasureString(pageHeaderInfo, headerBrick.Font);
                float headerTextHeight = textSize.Height;
                float headerTextWidth = textSize.Width + 5;
                float headerTextX = (_headerArgs.Graph.ClientPageSize.Width - headerTextWidth) / 2;
                float headerTextY = 10;
                headerBrick.Rect = new RectangleF(headerTextX, headerTextY, headerTextWidth, headerTextHeight);
                _headerArgs.Graph.DrawBrick(headerBrick);
            };

            // Create TextBrick objects for each line in the footer
            List<TextBrick> footerBricks = new List<TextBrick>();

            string[] footerLines = reportFooterInfo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            float lineHeight = 20;

            foreach (string line in footerLines)
            {
                TextBrick textBrick = new TextBrick();
                textBrick.BackColor = Color.Transparent;
                textBrick.BorderColor = Color.Transparent;
                textBrick.Text = line;
                textBrick.Font = new Font("Arial", 15);
                footerBricks.Add(textBrick);
            }

            // Handle the CreateReportFooterArea event to add the footer
            lnk.CreateReportFooterArea += (sender, footerArgs) =>
            {
                float y = 0; // Initial Y position
                foreach (TextBrick textBrick in footerBricks)
                {
                    footerArgs.Graph.DrawBrick(textBrick, new RectangleF(0, y, footerArgs.Graph.ClientPageSize.Width, lineHeight));
                    y += lineHeight; // Adjust Y position for the next line
                }
            };

            compositeLink.CreateDocument();

            // Handle the CreateMarginalFooterArea event to add the date and page number in the footer
            compositeLink.CreateMarginalFooterArea += (sender, _footerArgs) =>
            {
                // Calculate the total page count after document creation
                int totalPageCount = compositeLink.PrintingSystemBase.PageCount;

                float footerWidth = _footerArgs.Graph.ClientPageSize.Width;

                // Draw page number on the right side
                PageInfoBrick pageInfoBrick = new PageInfoBrick();
                pageInfoBrick.BackColor = Color.Transparent;
                pageInfoBrick.BorderColor = Color.Transparent;
                pageInfoBrick.PageInfo = PageInfo.NumberOfTotal;
                pageInfoBrick.Format = "Page {0} of {1}";
                pageInfoBrick.Font = new Font("Arial", 10); // Customize font and size as needed
                pageInfoBrick.Alignment = BrickAlignment.Center;
                float pageInfoWidth = 100; // Adjust the width as needed
                pageInfoBrick.Rect = new RectangleF(footerWidth - pageInfoWidth, 10, pageInfoWidth, 20); // Adjust the position as needed
                _footerArgs.Graph.DrawBrick(pageInfoBrick);

                // Calculate the remaining width for the date brick
                float dateWidth = footerWidth - pageInfoWidth;

                // Draw custom date on the left side
                TextBrick dateBrick = new TextBrick();
                dateBrick.BackColor = Color.Transparent;
                dateBrick.BorderColor = Color.Transparent;
                dateBrick.Text = DateTime.Now.ToString("yyyy-MM-dd"); // Customize date format as needed
                dateBrick.Font = new Font("Arial", 10); // Customize font and size as needed
                dateBrick.Rect = new RectangleF(10, 10, dateWidth, 20); // Adjust the position as needed
                _footerArgs.Graph.DrawBrick(dateBrick);
            };
            compositeLink.CreateDocument();
            MemoryStream _mstream = new MemoryStream();
            compositeLink.PrintingSystemBase.ExportToPdf(_mstream);
            Session["_mstream"] = _mstream;
            Session["exporttype"] = e.Parameter.ToString();
        }

        protected void response_btn_Click(object sender, EventArgs e)
        {
            MemoryStream stream = Session["_mstream"] as MemoryStream;
            string exporttype = Session["exporttype"].ToString();
            WriteToResponse("Resource Utilization Gantt", true, exporttype, stream);
        }
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            Page.Response.BinaryWrite(stream.ToArray());
            try
            {
                Page.Response.Flush();
                Page.Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {

            }
        }
        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            if (allocationData == null)
            {
                try
                {
                    allocationData = GetAllocationDataGantt();
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, ex.Message);
                }

                if (!stopToRegerateColumns)
                    PrepareAllocationGridGantt();
            }
            gvPreview.DataSource = allocationData;

        }
        DateTime dtstartDate;
        DateTime dtEndDate;
        double ResourceFTE=0;
        //double ResourceTotalFTE=0;
        double pctSum;
        string datePattern = @"^[A-Za-z]{3}-\d{2}-\d{2}[A-Za-z]$";

        protected void gvResourceAvailablity_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                DataRow currentRow = gvResourceAvailablity.GetDataRow(e.VisibleIndex);
                UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                if (userProfile == null)
                    return;
                string absoluteRowUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}";
                absoluteRowUrlView = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "addworkitem", "Add Allocation", "ResourceAllocation", userProfile.Id, chkIncludeClosed.Checked));
                string userid = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Id]);
                string username = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Resource]);
                string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteRowUrlView, "Resource Utilization", Server.UrlEncode(Request.Url.AbsolutePath));
                if (e.Row.Cells.Count > 1)
                {
                    bool isUserEnabled = userProfile.Enabled;
                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&showuserresume=true&selecteddepartment=-1&SelectedResource={0}", userid));
                    string TitleLink = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"Timeline for User : {1}\", \"90\",\"90\", false,\"{2}\")'>{3}</a>",
                                                    userLinkUrl, username, Uri.EscapeDataString(Request.Path), isUserEnabled ? username : $"<span style =\"color:red;\">{username}</span>");

                    if ((userEditPermisionList != null && userEditPermisionList.Exists(x => x.Id == userProfile.Id)) || isResourceAdmin)
                    {
                        if (isUserEnabled)
                            e.Row.Cells[1].Text = string.Format("<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' >{0} {1}</div>", TitleLink, "<image style=\"padding-right:5px;visibility:hidden; width:20px\" src=\"/content/images//plus-blue.png\" onclick=\"javascript:" + func + "  \"  />");
                        else
                            e.Row.Cells[1].Text = string.Format("<div>{0} {1}</div>", TitleLink, "<image style=\"padding-right:5px;visibility:hidden; width:20px\" src=\"/content/images//plus-blue.png\" onclick=\"javascript:" + func + "  \"  />");
                    }
                }
            }
        }

        protected void gvResourceAvailablity_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                var obj = gvResourceAvailablity.GetRow(e.VisibleIndex);
                DateTime dtStart = DateTime.MinValue;
                if (e.DataColumn.Caption == "#")
                    e.Cell.Text = Convert.ToString(e.VisibleIndex + 1);
                var dataRowView = obj as DataRowView;
                UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(UGITUtility.ObjectToString(dataRowView[DatabaseObjects.Columns.Id])));
                if (e.DataColumn.FieldName != "ItemOrder" && e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != DatabaseObjects.Columns.ProjectCapacity && e.DataColumn.FieldName != DatabaseObjects.Columns.RevenueCapacity && e.DataColumn.Caption != "Avrg Utilization" && e.DataColumn.Caption != "Avrg Chargeable")
                {
                    DateTime.TryParse(e.DataColumn.FieldName, out dtStart);
                    string displaymode = rbtnHrs.Checked == true ? "H" : rbtnItemCount.Checked == true ? "C" : rbtnPercentage.Checked == true ? "P" : rbtnFTE.Checked == true ? "F" : "A";
                    string strResourceAllocationColorPalete = ObjConfigurationVariableManager.GetValue(ConfigConstants.ResourceAllocationColorPalete);

                    string absoluteUrlEdit = string.Empty;
                    string roleName = globalRoleData?.Where(x => x.Id == userProfile.GlobalRoleId)?.FirstOrDefault()?.Name ?? string.Empty;
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL($"/Layouts/ugovernit/DelegateControl.aspx?control=potentialallocations&userId={userProfile.Id}&StartDate={dtStart.ToShortDateString()}&EndDate={dtStart.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dtStart) - 1).ToShortDateString()}");
                    string func = string.Format("window.parent.UgitOpenPopupDialog('{0}','','{1} for {3} {4}','95','95',0,'{2}')", absoluteUrlEdit, formTitle, Server.UrlEncode(Request.Url.AbsolutePath), userProfile.Name, !string.IsNullOrWhiteSpace(roleName) ? "(" + roleName + ")" : "");
                    e.Cell.Attributes.Add("style", "cursor: pointer;");
                    e.Cell.Attributes.Add("onclick", func);
                    if (!string.IsNullOrEmpty(strResourceAllocationColorPalete))
                    {
                        Dictionary<string, string> cpResourceAllocationColorPalete = UGITUtility.GetCustomProperties(strResourceAllocationColorPalete, Constants.Separator);

                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(e.CellValue)))
                        {
                            double cellvalue = UGITUtility.StringToDouble(GetValueFromInput(UGITUtility.ObjectToString(e.CellValue), "P"));
                            e.Cell.Text = GetValueFromInput(UGITUtility.ObjectToString(e.CellValue), displaymode);
                            if (displaymode == "P" || displaymode == "A")
                                e.Cell.Text = e.Cell.Text + "%";


                            if (cellvalue >= orange)
                            {
                                e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Orange]) + ";color:#fff>" + e.Cell.Text + "</div>"; //Orange
                            }
                            else if (cellvalue >= green && cellvalue < orange)
                            {
                                e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Green]) + ";color:#fff>" + e.Cell.Text + "</div>";//Green
                            }
                            else if (cellvalue >= gray && cellvalue < green)
                            {
                                e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Gray]) + ";color:#fff;>" + e.Cell.Text + "</div>";//Gray
                            }
                            else if (cellvalue >= red && cellvalue < gray)
                            {
                                e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]) + ";color:#fff;>" + e.Cell.Text + "</div>";//Red
                            }
                            else
                            {
                                e.Cell.Text = "<div>" + e.Cell.Text + "</div>";  //no color
                            }
                        }
                        else
                        {
                            if (userProfile.UGITStartDate < dtStart)
                            {
                                if (displaymode == "P")
                                    e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]) + ";color:#fff;>0%</div>";
                                else if (displaymode == "A")
                                    e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Green]) + ";color:#fff;>100%</div>";
                                else
                                    e.Cell.Text = "<div style=background-color:" + UGITUtility.ObjectToString(cpResourceAllocationColorPalete[Constants.Red]) + ";color:#fff;>0</div>";
                            }
                            else
                                e.Cell.Text = "";
                        }
                    }
                    if (userProfile.UGITStartDate > dtStart || userProfile.UGITEndDate < dtStart)
                    {
                        e.Cell.Attributes.Remove("style");
                        e.Cell.Attributes.Remove("onclick");
                        e.Cell.Text = "";
                    }

                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }

        }

        //protected void chkBenchonly_ValueChanged(object sender, EventArgs e)
        //{
        //    gvResourceAvailablity.ClearSort();
        //    if (chkBenchonly.Checked)
        //    {
        //        gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Ascending);
        //        chkOverstaffedonly.Checked = false;
        //    }
        //    else
        //    {
        //        gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Descending);
        //        chkOverstaffedonly.Checked = true;
        //    }
        //}

        protected void rbtnBenchonly_ValueChanged(object sender, EventArgs e)
        {
            PrepareAllocationGrid();
            gvResourceAvailablity.ClearSort();
            if (rbtnBenchonly.Checked)
            {
                gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Ascending);
                rbtnOverstaffedonly.Checked = false;
            }
            else
            {
                gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Descending);
                rbtnOverstaffedonly.Checked = true;
            }
        }

        protected void gvResourceAvailablity_BeforeHeaderFilterFillItems(object sender, ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
            isFilterApplying = true;
            PrepareAllocationGrid();
        }

        protected void gvResourceAvailablity_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            isFilterApplying = true;
            PrepareAllocationGrid();
        }
        private bool IsFilterOn()
        {
            return (gvResourceAvailablity.FilterEnabled && !string.IsNullOrEmpty(gvResourceAvailablity.FilterExpression)); // (grid.SortCount > 0) ||
        }

        protected void gvResourceAvailablity_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            isFilterApplying = false;
            PrepareAllocationGrid();
            if (e.CallbackName == "APPLYHEADERCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
            {
                ShowClearFilter = IsFilterOn();  //!IsPreview && 
                if (gvResourceAvailablity.JSProperties.ContainsKey("cpShowClearFilter"))
                {
                    gvResourceAvailablity.JSProperties["cpShowClearFilter"] = IsFilterOn(); //!IsPreview && 
                }
                else
                {
                    gvResourceAvailablity.JSProperties.Add("cpShowClearFilter", IsFilterOn()); // !IsPreview && 
                }

                if (gvResourceAvailablity.JSProperties.ContainsKey("cpClientID"))
                {
                    gvResourceAvailablity.JSProperties["cpClientID"] = customMessageContainer.ClientID; //!IsPreview && 
                }
                else
                {
                    gvResourceAvailablity.JSProperties.Add("cpClientID", customMessageContainer.ClientID); // !IsPreview && 
                }
            }
        }



        protected void rbtnOverstaffedonly_ValueChanged(object sender, EventArgs e)
        {
            PrepareAllocationGrid();
            gvResourceAvailablity.ClearSort();
            if (rbtnOverstaffedonly.Checked)
            {
                gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Descending);
                rbtnBenchonly.Checked = false;
            }
            else
            {
                gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Ascending);
                rbtnBenchonly.Checked = true;
            }
        }

        //protected void chkOverstaffedonly_ValueChanged(object sender, EventArgs e)
        //{
        //    gvResourceAvailablity.ClearSort();
        //    if (chkOverstaffedonly.Checked)
        //    {
        //        gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Descending);
        //        chkBenchonly.Checked = false;
        //    }
        //    else
        //    {
        //        gvResourceAvailablity.SortBy(gvResourceAvailablity.Columns["AvgerageUtil"], ColumnSortOrder.Ascending);
        //        chkBenchonly.Checked = true;
        //    }
        //}

        public string GetValueFromInput(string input, string condition)
        {
            string[] parts = input.Split('#');

            foreach (string part in parts)
            {
                string[] keyValue = part.Split(':');

                if (keyValue.Length == 2 && keyValue[0] == condition)
                {
                    return keyValue[1];
                }
            }

            // Return an appropriate value if the condition is not found
            return "Not found";
        }
        protected void gvPreview_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            #region group summary
            if (e.IsGroupSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    dtstartDate = DateTime.MinValue;
                    dtEndDate = DateTime.MinValue;
                    pctSum = 0;
                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        }
                    }

                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        }
                    }

                    if (Regex.IsMatch(item.FieldName, datePattern))
                    {
                        bool softAllocationValue = UGITUtility.StringToBoolean(e.GetValue("SoftAllocation"));
                        if (!softAllocationValue)
                            pctSum += UGITUtility.StringToDouble(e.FieldValue);
                    }
                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {

                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate")
                    {
                        if (dtstartDate != DateTime.MinValue)
                            e.TotalValue = dtstartDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";

                    }
                    if (item.FieldName == "AllocationEndDate")
                    {
                        if (dtEndDate != DateTime.MinValue)
                            e.TotalValue = dtEndDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";
                    }

                    if (Regex.IsMatch(item.FieldName, datePattern))
                    {
                        e.TotalValue = pctSum;
                    }
                }
            }
            #endregion

            #region total summary
            if (e.IsTotalSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    ResourceFTE = 0.0;
                    //ResourceTotalFTE = 0.0;
                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id
                        && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType"
                        && item.FieldName != DatabaseObjects.Columns.AllocationStartDate
                        && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "")
                            ResourceFTE += UGITUtility.StringToDouble(Convert.ToString(e.FieldValue));
                    }

                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == DatabaseObjects.Columns.Title)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceItem")
                            e.TotalValue = "Total Capacity";
                        else if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ExCap")
                            e.TotalValue = "Excess Capacity";
                        else
                            e.TotalValue = "Allocated Demand";
                    }

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id 
                        && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" 
                        && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        string columnName = item.FieldName.Remove(item.FieldName.Length - 1);
                        if (hdndisplayMode.Value == "Monthly")
                        {
                            int currentYear = Convert.ToInt32(DateTime.Now.Year);
                            int selectedYear = Convert.ToInt32(Convert.ToDateTime(hdndtfrom.Value).ToString("yyyy"));

                            if ((currentYear - selectedYear < -2) || (currentYear - selectedYear > 3))
                            {
                                if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE" && ds?.Tables?.Count > 2)
                                {
                                    if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[2]))
                                        e.TotalValue = dsFooter.Tables[1].Rows[0][columnName];
                                }
                                else if(((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ExCap" && ds?.Tables?.Count > 3)
                                {
                                    if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[3]))
                                        e.TotalValue = dsFooter.Tables[3].Rows[0][columnName];
                                }
                                else if (dsFooter?.Tables?.Count > 1)
                                {
                                    if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[1]))
                                        e.TotalValue = dsFooter.Tables[0].Rows[0][columnName];
                                }
                            }
                            else
                            {
                                if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE" && dsFooter != null)
                                {
                                    if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[1]))
                                        e.TotalValue = dsFooter.Tables[1].Rows[0][columnName];
                                }
                                else if (dsFooter.Tables[0].Rows.Count > 0)
                                {
                                    if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[0]))
                                        e.TotalValue = dsFooter.Tables[0].Rows[0][columnName];
                                }
                            }

                        }
                        else if (hdndisplayMode.Value == "Weekly")
                        {
                            if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE" && dsFooter?.Tables?.Count > 1)
                            {
                                if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[1]))
                                    e.TotalValue = dsFooter.Tables[1].Rows[0][columnName];
                            }
                            else
                            {
                                if (UGITUtility.IfColumnExists(columnName, dsFooter.Tables[0]))
                                    e.TotalValue = dsFooter.Tables[0].Rows[0][columnName];
                            }
                        }

                    }
                }
            }
            #endregion
        }
        #endregion
        protected void ASPxGridViewExporter1_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            #region GanttMnager

            if (e.RowType == GridViewRowType.Header)
            {
                e.BrickStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                return;
            }

            GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;

            DateTime fromDate = Convert.ToDateTime(hdndtfrom.Value);
            DateTime toDate = Convert.ToDateTime(hdndtto.Value);

            string foreColor = "#000000";
            string Estimatecolor = "#24b6fe";
            string Assigncolor = "#24b6fe";
            string moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
            if (lstEstimateColorsAndFontColors != null && lstEstimateColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                    foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                }
            }

            if (lstAssignColorsAndFontColors != null && lstAssignColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstAssignColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                Assigncolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                //foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
            }

            if (dataColumn != null)
            {
                e.BrickStyle.BorderWidth = 1;
                e.Column.ExportCellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                if (dataColumn.FieldName != DatabaseObjects.Columns.Resource && dataColumn.FieldName != "AllocationType" && dataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && dataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && dataColumn.FieldName != DatabaseObjects.Columns.Title)
                {
                    e.BrickStyle.TextAlignment = TextAlignment.MiddleCenter;
                    int defaultBarH = 12;
                   
                    string html;
                    DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                    DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                    string type = UGITUtility.ObjectToString(e.GetValue("ProjectNameLink"));

                    string tickId = string.Empty;
                    string allStartDates = string.Empty;
                    string allEndDates = string.Empty;
                    //DateTime oPreconStart = DateTime.MinValue;
                    //DateTime oPreconEnd = DateTime.MinValue;
                    //DateTime oConstStart = DateTime.MinValue;
                    //DateTime oConstEnd = DateTime.MinValue;
                    //DateTime oCloseout = DateTime.MinValue;

                    DateTime aStartDate = DateTime.MinValue;
                    DateTime aEndDate = DateTime.MinValue;
                    DateTime aPreconStart = DateTime.MinValue;
                    DateTime aPreconEnd = DateTime.MinValue;
                    DateTime aConstStart = DateTime.MinValue;
                    DateTime aConstEnd = DateTime.MinValue;
                    DateTime aCloseout = DateTime.MinValue;
                    string id = string.Empty;
                    bool aSoftAllocation = true;
                    string project = string.Empty;
                    string allocId = string.Empty;
                    if (e.VisibleIndex > 0)
                    {
                        DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                        tickId = row[DatabaseObjects.Columns.TicketId].ToString();
                        aStartDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]);
                        aEndDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationEndDate]);
                        aPreconStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconStartDate]);
                        aPreconEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconEndDate]);
                        aConstStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        aConstEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        aCloseout = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.CloseoutDate]);
                        aSoftAllocation = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.SoftAllocation]);
                        project = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                        id = row[DatabaseObjects.Columns.Id].ToString();
                        allocId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.AllocationID]);

                        allStartDates = UGITUtility.ObjectToString(row["AllStartDates"]);
                        allEndDates = UGITUtility.ObjectToString(row["allEndDates"]);
                    }

                    //if (tickId != "CPR-23-000628")
                    //    return;
                    DateTime dateF = Convert.ToDateTime(dateFrom);
                    DateTime dateT = Convert.ToDateTime(dateTo);
                    for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        html = "";
                        if (dataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                        {
                            string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                            string roundleftbordercls = string.Empty;
                            string roundrightbordercls = string.Empty;
                            string backgroundColor = string.Empty;
                            if (aStartDate == DateTime.MinValue || aEndDate == DateTime.MinValue)
                            {
                                if (e.Text == "0")
                                    e.Text = "0%";
                                return;
                            }
                            // classed to set round corners on bar end points
                            if (hdndisplayMode.Value != "Weekly")
                            {
                                if (aStartDate.Year < dt.Year && dt.Month == 1)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate.Year > dt.Year && dt.Month == 12)
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                                if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                            }
                            else if (hdndisplayMode.Value == "Weekly")
                            {

                                if (aStartDate >= dt && aStartDate.AddDays(-6) < dt)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate >= dt && aEndDate <= dt.AddDays(6))
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                            }

                            backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt);

                            if (dt.Month == aConstStart.Month && dt.Month == aPreconEnd.Month)
                            {
                                if (aEndDate <= aPreconEnd)
                                {
                                    backgroundColor = "preconbgcolor";
                                }
                            }

                            if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                            {
                                if (aSoftAllocation)
                                    backgroundColor = "#ecf1f9";
                                if (moduleName != null && moduleName.Equals("Time Off"))
                                {
                                    backgroundColor = "#909090"; //ptobgcolor
                                    html = GeneratePtoCard(dt, id, defaultBarH, type);
                                    //e.     CssClass += " ptoAlignmentClass";
                                    e.Text = estAlloc + "%"; //html;
                                    e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(backgroundColor);
                                    e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                                }
                                else
                                {
                                    estAlloc = estAlloc + "%";
                                    e.Column.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    int NoOfDays = 0;
                                    int remainingDays = 0;
                                    string widthEstAlloc = string.Empty;

                                    if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                    {
                                        NoOfDays = aStartDate.Day;
                                        remainingDays = 30 - NoOfDays;
                                        widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                                        roundleftbordercls = "RoundLeftSideCorner";
                                        backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, aStartDate);
                                    }
                                    else if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                                    {
                                        NoOfDays = aEndDate.Day;
                                        widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                                        roundrightbordercls = "RoundRightSideCorner";
                                        roundleftbordercls = "";
                                        backgroundColor = GetGanttCellBackGroundColor(aPreconStart, aPreconEnd, aConstStart, aConstEnd, aEndDate);
                                    }


                                    if (dt.Month == aConstStart.Month && dt.Month == aPreconEnd.Month)
                                    {
                                        if (aEndDate <= aPreconEnd)
                                        {
                                            NoOfDays = aEndDate.Day;
                                            widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(widthEstAlloc))
                                    {
                                        if (Convert.ToInt32(widthEstAlloc) >= 50)
                                        {
                                            //if (roundleftbordercls == "RoundLeftSideCorner")
                                            //{
                                            //    //html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:right;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";

                                            //    e.Text = estAlloc;
                                            //    e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(Estimatecolor);
                                            //    e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(foreColor);
                                            //}
                                            //else if (roundrightbordercls == "RoundRightSideCorner")
                                            //{
                                            //    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                            //}
                                            //else
                                            //{
                                            //    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                            //}

                                            e.Text = estAlloc;
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(backgroundColor);
                                            if (backgroundColor == "#005C9B"
                                                || backgroundColor == "#351B82"
                                                || backgroundColor == "#52BED9"
                                                || backgroundColor == "#3F6DB8"
                                                || backgroundColor == "#3E6CB6"
                                                )
                                            {
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                                            }
                                            else
                                            {
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(foreColor);
                                            }
                                            //e.BrickStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(50, 0, 0, 0);
                                        }
                                        else
                                        {
                                            //if (roundleftbordercls == "RoundLeftSideCorner")
                                            //{
                                            //    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:right;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'></div>";
                                            //}
                                            //else if (roundrightbordercls == "RoundRightSideCorner")
                                            //{
                                            //    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'></div>";
                                            //}
                                            //else
                                            //{
                                            //    html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                            //}

                                            e.Text = estAlloc;
                                            e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(backgroundColor);
                                            if (backgroundColor == "#005C9B"
                                                || backgroundColor == "#351B82"
                                                || backgroundColor == "#52BED9"
                                                || backgroundColor == "#3F6DB8"
                                                || backgroundColor == "#3E6CB6"
                                                )
                                            {
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                                            }
                                            else
                                            {
                                                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(foreColor);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //html = $"<div class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;'>{estAlloc}</div>";
                                        e.Text = estAlloc;
                                        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(backgroundColor);

                                        if (backgroundColor == "#005C9B"
                                                || backgroundColor == "#351B82"
                                                || backgroundColor == "#52BED9"
                                                || backgroundColor == "#3F6DB8"
                                                || backgroundColor == "#3E6CB6"
                                                )
                                        {
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                                        }
                                        else
                                        {
                                            e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(foreColor);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //UserProfile userProfile = ObjUserProfileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(id));
                                //if (userProfile != null && userProfile.UGITStartDate < dt && userProfile.UGITEndDate >= dt)
                                //    e.Text = "0%";
                                //else
                                    e.Text = "";
                            }
                            //e.Text = html;
                            //e.Value = html;
                            //sets alternate color to columns
                            //if (dataColumn.VisibleIndex % 2 == 0)
                            //    e.BrickStyle.BackColor = Color.WhiteSmoke;
                        }


                    }
                }
            }
            #endregion

        }

        public static string GetGanttCellBackGroundColor(DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd, DateTime dt)
        {
            string backgroundColor;
            DateTime dtEnd = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
            ////classes to set color based on dates
            if ((dt >= aPreconStart && dt <= aPreconEnd) && (dtEnd >= aConstStart && dtEnd <= aConstEnd))
                backgroundColor = "#3E6CB6"; //preconbgcolor-constbgcolor
            else if ((dt >= aConstStart && dt <= aConstEnd) && (aConstEnd != DateTime.MinValue && dtEnd >= aConstEnd))
                backgroundColor = "#3F6DB8"; //constbgcolor-closeoutbgcolor
            else if (dt >= aPreconStart && dt <= aPreconEnd)
                backgroundColor = "#52BED9"; //preconbgcolor
            else if (dt >= aConstStart && dt <= aConstEnd)
                backgroundColor = "#005C9B"; //constbgcolor
            else if (aConstEnd != DateTime.MinValue && dt >= aConstEnd)
                backgroundColor = "#351B82"; //closeoutbgcolor
            else
                backgroundColor = "#D6DAD9"; // if allocation does not falls in any stage consider it as precon stage
            return backgroundColor;
        }

    }

}
