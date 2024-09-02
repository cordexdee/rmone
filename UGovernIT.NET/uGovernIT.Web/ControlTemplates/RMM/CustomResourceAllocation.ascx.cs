using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using DevExpress.Web.ASPxGantt;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using DevExpress.Web;
using System.Text.RegularExpressions;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Helpers;
using uGovernIT.Manager.RMM;
using DevExpress.XtraScheduler.Drawing;
using System.Web.UI.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using uGovernIT.Web.Controllers;
using uGovernIT.Web.ControlTemplates.RMONE;
using DevExpress.Web.Internal;
using uGovernIT.Util.Log;
using uGovernIT.Web.ControlTemplates.RMM;
using DevExpress.Utils.Extensions;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class CustomResourceAllocation : UserControl
    {
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        public string FrameId { get; set; }
        public bool IncludeClosed { get; set; }
        public string managerFrom { get; set; }
        private DataTable resultedTable;
        private string selectedUsers = string.Empty;
        private string selectedUsersId = string.Empty;
        List<UserProfile> selectedUsersList = new List<UserProfile>();
        UserProfile manager = null;
        string resourceFromCookies = "selectedResource";
        bool isResourceAdmin = false;

        private const string absoluteUrlView = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        public string absoluteCalendarUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=projectcalendarview");
        public string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=addworkitem&Type=ResourceAllocation");
        public string viewTimesheetPath = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=customresourcetimeSheet");
        public string timesheetPendingAprvlPath = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=timesheetpendingapprovals");
        public string allocationGanttUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=AllocationGantt");
        public string AddOpenAllocationUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        public string AddMultiAllocationUrlNew = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        public string AddCombineMultiAllocationUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&refreshpage=0";
        public string OpenUserResumeUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&refreshpage=0";
        public string GanttPopupUrl = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&SelectedUsers={0}";
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}&refreshpage=0";
        public string timeOffAllocationLink = string.Empty;
        public string GanttPopupLink { get; set; }
        public string MultiAddUrl { get; set; }
        public string AddNonProjectUrl { get; set; }
        public bool ShowTimeOffOnly {
            get {
                return Request["ShowTimeOffOnly"] != null && Request["ShowTimeOffOnly"].ToString() == "true" ? true : false;
            }
        }
        public bool IsRedirectFromUtilization
        {
            get
            {
                return UGITUtility.ObjectToString(Request["IsRedirectFromUtilization"]) == "true" ? true : false;
            }
        }
        private string newParam = "addworkitem";
        private string formTitle = "Add Allocation for ";
        HttpCookie myCookie = null;
        protected string delegateUrl = string.Empty;

        string hdnSelectedUserListVal = string.Empty;
        string hdnParentOfVal = string.Empty;
        string hdnChildOfVal = string.Empty;
        string hdnCmbResourceManagerVal = string.Empty;
        public DateTime startDateRange = DateTime.MinValue;
        public DateTime endDateRange = DateTime.MinValue;
        //private bool allowAllocationForSelf;
        private string allowAllocationForSelf;
        private bool viewself = false;
        public bool FromAllocationGrid = false;

        public string ActiveView = string.Empty;
        public int MgrViewAllocDays = 0;
        public string DefaultView = "";

        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        ResourceAllocationManager allocManager = null;
        ResourceAllocationMonthlyManager allocMManager = null;
        ResourceWorkItemsManager workitemManager = null;
        UserProfileManager ObjUserProfileManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        ProjectEstimatedAllocationManager projectEstimatedAllocationManager = null;
        ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = null;
        List<ResourceTimeSheetSignOff> resourceTimeSheetSignOffs = null;
        GlobalRoleManager roleManager = null;
        UserProjectExperienceManager ObjUserProjectExperienceManager = null;
        TicketManager ticketManager = null;


        public DataTable TimeOffAllocatin { get; set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }
        public void SetLayout()
        {
            if (resultedTable == null || resultedTable.Rows.Count <= 0)
            {
                searchViewPanel.Visible = false;
                searchViewPanel.Width = 0;
            }
            else
            {
                searchViewPanel.Visible = true;
                searchViewPanel.Width = 250;
            }

            trAllocationDistribution.Visible = false;
            if (!string.IsNullOrEmpty(hdnSelectedAllocation.Value) && rAllocationDistForm.Items.Count > 0)
            {
                trAllocationDistribution.Visible = true;
            }
            else
            {
                trAllocationDistribution.Visible = false;
                //gridAllocation.FocusedRowIndex = -1;  //commented these lines to fix expand/collapse feature
            }

            trTaskList.Visible = false;
            if (!string.IsNullOrEmpty(hdnSelectedAllocation.Value))
            {
                if (TasklistControl.Controls.Count > 0)
                    trTaskList.Visible = true;
                else
                    trTaskList.Visible = false;
            }
            else
            {
                trTaskList.Visible = false;
                //gridAllocation.FocusedRowIndex = -1;
            }

            lbAllocationDistributionMsg.Text = string.Empty;
            lblProjectTaskAllocationMsg.Text = string.Empty;

            if (!string.IsNullOrEmpty(hdnSelectedAllocation.Value))
            {
                string selectVal = Convert.ToString(gridAllocation.GetRowValuesByKeyValue(hdnSelectedAllocation.Value, DatabaseObjects.Columns.WorkItemLink));
                lbAllocationDistributionMsg.Text = string.Format("<b>{0}:</b> Monthly Allocation Breakdown", selectVal);
                lblProjectTaskAllocationMsg.Text = string.Format("<b>{0}:</b> Project Task Allocation", selectVal);
            }

            if (manager != null && !string.IsNullOrWhiteSpace(manager.Id))
            {
                //ddlResourceManager.ClearSelection();
                //ddlResourceManager.SelectedIndex = ddlResourceManager.Items.IndexOf(ddlResourceManager.Items.FindByValue(manager.ID.ToString()));
                cmbResourceManager.SelectedIndex = cmbResourceManager.Items.IndexOf(cmbResourceManager.Items.FindByValue(manager.Id.ToString()));
                // Load users details
                lbDetail3.Visible = false;
                lbDetail2.Visible = false;
                lbDetail2Val.Visible = false;
                //lbDetail1.Visible = false;
                if (!string.IsNullOrEmpty(manager.Location))
                {
                    LocationManager locationManager = new LocationManager(applicationContext);
                    Location location = locationManager.LoadByID(Convert.ToInt64(manager.Location));
                    if (location != null)
                    {
                        lbDetail2.Visible = true;
                        lbDetail2Val.Visible = true;
                        lbDetail2Val.Text = location.Title;
                    }
                }
                if (!string.IsNullOrEmpty(manager.Department))
                {
                    DepartmentManager departmentManager = new DepartmentManager(applicationContext);
                    CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(applicationContext);
                    Department department = departmentManager.LoadByID(Convert.ToInt64(manager.Department));
                    CompanyDivision companyDivision = null;
                    if (department != null)
                    {
                        lbDetail3.Visible = true;
                        lbDetail3Val.Text = department.Title;

                        companyDivision = companyDivisionManager.Load(x => x.ID == department.DivisionIdLookup).FirstOrDefault();
                        if (companyDivision != null)
                        {
                            lbDivision.Visible = true;
                            lbDivisionVal.Text = companyDivision.Title;
                        }
                        else
                        {
                            lbDivision.Visible = false;
                        }
                    }
                }

            }

            string selectedVals = string.Empty;
            if (string.IsNullOrEmpty(hdnSelectedUserListVal))
            {
                // selectedVals = ddlResourceManager.SelectedValue;
                selectedVals = Convert.ToString(cmbResourceManager.Value);
            }
            else
            {
                selectedVals = hdnSelectedUserListVal;
            }
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["ID"])))
                selectedVals = UGITUtility.ObjectToString(Request["ID"]);
            UserProfile selectedUserObj = null;
            if (!string.IsNullOrEmpty(selectedVals))
            {
                selectedUserObj = applicationContext.UserManager.GetUserById(selectedVals);
            }
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "ResourceAllocation", selectedVals, IncludeClosed));
            if (!string.IsNullOrEmpty(Request["isRedirectFromCardView"]) && Convert.ToString(Request["isRedirectFromCardView"]) == "true")
                aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','680px','500px',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle + selectedUserObj?.Name));
            else
                aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','680px','500px',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle + selectedUserObj?.Name));

            string selectedUserForGantt = string.Empty;
            selectedUserForGantt = UGITUtility.ObjectToString(Request["userall"]);
            //_allocationGantt.IncludeClosed = UGITUtility.ObjectToString(Request["includeClosed"]);
            if (selectedUsersList != null && selectedUsersList.Count > 0)
            {
                List<string> userids = selectedUsersList.Select(x => x.Id).ToList();
                if (!resourceChk.Checked)
                {
                    string managerId = UGITUtility.ObjectToString(cmbResourceManager.SelectedItem?.Value);
                    userids.Remove(managerId);
                }
                selectedUserForGantt = UGITUtility.ConvertListToString(userids, Constants.Separator6);
            }
            else if (!string.IsNullOrEmpty(selectedUsersId))
                selectedUserForGantt = selectedUsersId;
            else
                selectedUserForGantt = UGITUtility.ObjectToString(cmbResourceManager.SelectedItem?.Value);
            hdnSelectedUsersForGantt.Value = selectedUserForGantt;
            GanttPopupLink = UGITUtility.GetAbsoluteURL(string.Format(GanttPopupUrl, ""));
            MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "multiallocationjs", "Add Multiple Allocations", UGITUtility.ObjectToString(hdnSelectedWorkItem.Value), manager?.Id, IncludeClosed));
            AddNonProjectUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "ptomultiallocationjs", "Add Non Project Allocations", UGITUtility.ObjectToString(hdnSelectedWorkItem.Value), manager?.Id, IncludeClosed));
            btnAddMultiAllocation.ClientSideEvents.Click = "function(s,e){ window.parent.UgitOpenPopupDialog('" + MultiAddUrl +"', '', 'Add Allocation for " + manager?.Name +"', '1100px', '90', 0, ''); }";
            btnAddNonProjectAllocation.ClientSideEvents.Click = $"function(s,e){{ window.parent.UgitOpenPopupDialog('{AddNonProjectUrl}', '', 'Add Non Project Allocation for {manager?.Name}', '680px', '90', 0, ''); }}";
            
            btnAddMultiAllocation.Visible = false;
            btnAddNonProjectAllocation.Visible = false;
            //Show chart icon to show resource dashboards
            string rmmDashboard = ObjConfigurationVariableManager.GetValue(ConfigConstants.RMMDashboardView);
            if (!string.IsNullOrEmpty(rmmDashboard))
            {
                List<string> dataArray = UGITUtility.ConvertStringToList(rmmDashboard, Constants.Separator);
                string dashboardView = string.Empty;
                if (dataArray.Count > 0)
                    dashboardView = dataArray[0];

                string popupWidth = "90";
                string popupHeight = "90";
                if (dataArray.Count > 1)
                {
                    dataArray = UGITUtility.ConvertStringToList(dataArray[1], "*");
                    if (dataArray.Count > 0)
                        popupWidth = dataArray[0];
                    if (dataArray.Count > 1)
                        popupHeight = dataArray[1];
                }

                resourceChart.Visible = true;
                int viewID = 0;
                List<string> viewParams = new List<string>();
                if (int.TryParse(dashboardView, out viewID))
                {
                    viewParams.Add(string.Format("viewID={0}", viewID));
                }
                else
                {
                    viewParams.Add(string.Format("view={0}", dashboardView));
                }

                if (cmbResourceManager.SelectedItem != null)
                {
                    string selectedUser = cmbResourceManager.SelectedItem.Text;
                    viewParams.Add(string.Format("externalfilter={0}", Uri.EscapeDataString(string.Format("ManagerLookup='{0}'", selectedUser))));
                }

                if (manager != null)
                {
                    resourceChart.Attributes.Add("onClick", string.Format("window.parent.UgitOpenPopupDialog(\"{0}\", \"{1}\", 'Resource Dashboard for {4}', '{2}', '{3}', 0)",
                        UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ShowDashboardView), string.Join("&", viewParams.ToArray()), popupWidth, popupHeight, manager.Name));
                }
            }

            if ((allowAllocationForSelf.EqualsIgnoreCase("Edit") || HttpContext.Current.CurrentUser().IsManager || isResourceAdmin) && !uHelper.IsCPRModuleEnabled(applicationContext))
                aAddItem.Visible = true;
            else
                aAddItem.Visible = false;
            //ddlResourceManager.Attributes.Add("onChange", "MoveDown(this.value)");
            //cmbResourceManager.ClientSideEvents.SelectedIndexChanged = string.Format("function(s, e) {0} MoveDown(); {1}", "{", "}");
            // new line of code...
            BindAllocation();
            gridAllocation.DataBind();
            if (hndAllocation.Value == "ExpandAll")
            {
                gridAllocation.ExpandAll();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            SetLayout();
            
            base.OnPreRender(e);
        }

        #region Resources
        protected void LoadDdlResourceManager()
        {
            if (cmbResourceManager.Items.Count <= 0)
            {
                UserProfile currentUser = HttpContext.Current.CurrentUser();
                if (isResourceAdmin)
                {
                    List<UserProfile> userCollection = ObjUserProfileManager.Load(x => x.Enabled == true && !x.UserName.EqualsIgnoreCase("SuperAdmin")).OrderBy(x => x.Name).ToList(); // ObjUserProfileManager.GetUsersProfile().OrderBy(x => x.Name).ToList();
                    if (userCollection != null)
                    {
                        //foreach (UserProfile user in userCollection)
                        //{
                        //    cmbResourceManager.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        //}
                        cmbResourceManager.DataSource = userCollection;
                        cmbResourceManager.ValueField = "Id";
                        cmbResourceManager.TextField = "Name";
                        cmbResourceManager.DataBind();
                    }
                }
                else
                {
                    List<UserProfile> userCollection = new List<UserProfile>();

                    if (allowAllocationForSelf.EqualsIgnoreCase("Edit") || currentUser.IsManager)
                    {
                        userCollection = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf.EqualsIgnoreCase("Edit") || currentUser.IsManager || isResourceAdmin); // ObjUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id);
                    }
                    else
                    {
                        userCollection.Add(currentUser);
                    }

                    userCollection = userCollection.OrderBy(x => x.Name).ToList();

                    if (userCollection != null)
                    {
                        //foreach (UserProfile user in userCollection)
                        //{
                        //    cmbResourceManager.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        //}
                        cmbResourceManager.DataSource = userCollection;
                        cmbResourceManager.ValueField = "Id";
                        cmbResourceManager.TextField = "Name";
                        cmbResourceManager.DataBind();
                    }
                    if (cmbResourceManager.Items.Count == 0 && viewself)
                    {
                        cmbResourceManager.Items.Add(new ListEditItem(currentUser.Name, currentUser.Id));
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            allocManager = new ResourceAllocationManager(applicationContext);
            allocMManager = new ResourceAllocationMonthlyManager(applicationContext);
            workitemManager = new ResourceWorkItemsManager(applicationContext);
            ObjUserProfileManager = new UserProfileManager(applicationContext);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(applicationContext);
            projectEstimatedAllocationManager = new ProjectEstimatedAllocationManager(applicationContext);
            resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(applicationContext);
            roleManager = new GlobalRoleManager(applicationContext);
            ObjUserProjectExperienceManager  = new UserProjectExperienceManager(applicationContext);
            ticketManager = new TicketManager(applicationContext);

            
            HttpCookie  cookieYear = UGITUtility.GetCookie(Request, "SelectedYear");
            if (cookieYear == null || !int.TryParse(cookieYear.Value, out int year))
                lblSelectedYear.Text = DateTime.Now.Year.ToString();
            else
                lblSelectedYear.Text = UGITUtility.ObjectToString(cookieYear.Value);
            if (!IsPostBack)
            {
                chkIncludeClosed.Checked = IncludeClosed;
            }

            lbDivision.Text = ObjConfigurationVariableManager.GetValue(ConfigConstants.DivisionLabel) + ":&nbsp;";
            
            List<int> listState = new List<int>();
            gridAllocation.FocusedRowIndex = -1;
            gridAllocation.StylesPopup.HeaderFilter.Content.CssClass = "SearchFilter_content";
            gridAllocation.StylesPopup.HeaderFilter.ButtonPanel.CancelButton.CssClass = "Filter_cancelBtn resourceAll-gridFilterBtn";
            gridAllocation.StylesPopup.HeaderFilter.ButtonPanel.OkButton.CssClass = "Filter_okBtn resourceAll-gridFilterBtn";
            gridAllocation.StylesPopup.HeaderFilter.Footer.CssClass = "FilterFooter_btnWrap";
            //Checks current user is super admin or not
            isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || ObjUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());

            allowAllocationForSelf = ObjConfigurationVariableManager.GetValue(ConfigConstants.AllowAllocationForSelf);
            MgrViewAllocDays = UGITUtility.StringToInt(ObjConfigurationVariableManager.GetValue(ConfigConstants.MgrViewAllocDays));
            hdnSelectedUserListVal = Request[hdnSelectedUserList.UniqueID];
            hdnParentOfVal = Request[hdnParentOf.UniqueID];
            hdnChildOfVal = Request[hdnChildOf.UniqueID];
            hdnCmbResourceManagerVal = Request[hdnCmbResourceManager.UniqueID];
            LoadDdlResourceManager();
            if (!IsPostBack)
            {
                //Load Selected Users from cookie
                myCookie = UGITUtility.GetCookie(Request, "SelectedUsers");
                if(myCookie == null)
                    myCookie = UGITUtility.GetCookie(Request, "userall");
                if (myCookie != null && !string.IsNullOrEmpty(myCookie.Value))
                {
                    hdnSelectedUserList.Value = hdnSelectedUserListVal = myCookie.Value;
                    UGITUtility.DeleteCookie(Request, Response, "SelectedUsers"); // Delete myCookie
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "DeleteCookies('SelectedUsers','"+VirtualPathUtility.MakeRelative("~",HttpContext.Current.Request.Url.PathAndQuery)+"')", true);
                }
                //Load manager from cookie
                string resource = null;
                if (Request.QueryString["ID"] != null)
                {
                    resource = Request.QueryString["ID"];
                }
                else
                {
                    resource = UGITUtility.GetCookieValue(Request, resourceFromCookies).Trim();
                }
                if (!string.IsNullOrEmpty(resource))
                {
                    hdnChildOfVal = resource;
                    //if resource is not exist in resource dropdown then select first resource from dropdown
                    if (cmbResourceManager.Items.Count > 0)
                    {
                        int index = cmbResourceManager.Items.IndexOf(cmbResourceManager.Items.FindByValue(resource));
                        if (index == -1)
                        {
                            hdnChildOfVal = Convert.ToString(cmbResourceManager.Items[0].Value);
                        }
                        //else
                        //{
                        //    hdnChildOfVal = applicationContext.CurrentUser.Id;
                        //}
                    }
                }

                ActiveView = UGITUtility.GetCookieValue(Request, "activeview").Trim();

            }
            //Get subordinate of manager if any are selected
            if (!string.IsNullOrEmpty(hdnSelectedUserListVal))
            {
                string selecteduserIds = HttpUtility.UrlDecode(hdnSelectedUserListVal);
                selectedUsersList.Clear();
                selectedUsersList = ObjUserProfileManager.GetUserInfosById(selecteduserIds);
            }
            else if (!string.IsNullOrWhiteSpace(hdnCmbResourceManagerVal))
            {
                if (resourceChk.Checked)
                {
                    selectedUsersList.Clear();
                    selectedUsersList = ObjUserProfileManager.GetUserInfosById(hdnCmbResourceManagerVal);
                }
            }
            PrepareAllocationGrid();
            BindAllocation();
            gridAllocation.DataBind();
            //BindMonthAllocation();
            //rAllocationDistForm.DataBind();

            if (gridAllocation.DataSource != null && Session["ExpandedGridViewRows"] != null)
            {
                var expandedRowsList = (List<int>)Session["ExpandedGridViewRows"];

                foreach (var rowIndex in expandedRowsList)
                {
                    if (gridAllocation.IsGroupRow(rowIndex) && gridAllocation.IsRowExpanded(rowIndex))
                    {
                        //expand this group row
                        gridAllocation.ExpandRow(rowIndex);
                    }
                }
            }
            else
            {
                gridAllocation.ExpandAll();
            }


            if (ObjConfigurationVariableManager.GetValue(ConfigConstants.AllowAllocationForSelf).EqualsIgnoreCase("View"))
                viewself = true;

            resourceTimeSheetSignOffs = resourceTimeSheetSignOffManager.Load(x => x.Deleted == false).ToList();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbDistributionError.Text = string.Empty;
            lbAllocationError.Text = string.Empty;
            hdnChildOf.Value = hdnChildOfVal;
            hdnParentOf.Value = hdnParentOfVal;
            hdnSelectedUserList.Value = hdnSelectedUserListVal;
            if (this.ShowTimeOffOnly)
            {
                hdnChildOf.Value = Request["hdnChildOfVal"] != null ? Request["hdnChildOfVal"].ToString() : "";
                hdnParentOf.Value = Request["hdnParentOfVal"] != null ? Request["hdnParentOfVal"].ToString() : "";
                hdnSelectedUserList.Value = Request["hdnSelectedUserListVal"] != null ? Request["hdnSelectedUserListVal"].ToString() : "";
                resourceChk.Checked = Request["resourceChecked"] != null ? Request["resourceChecked"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false : false;
            }
            AddOpenAllocationUrl = UGITUtility.GetAbsoluteURL(string.Format(AddOpenAllocationUrl, "addworkitem", "Add Allocation for "+ hdnSelectedUserListVal, "ResourceAllocation"));
            AddMultiAllocationUrlNew = UGITUtility.GetAbsoluteURL(string.Format(AddMultiAllocationUrlNew, "multiallocationjs", "Add Allocation for " + hdnSelectedUserListVal, "ResourceAllocation"));
            AddCombineMultiAllocationUrl = UGITUtility.GetAbsoluteURL(string.Format(AddCombineMultiAllocationUrl, "combinedmultiallocationjs", "Add Allocation for " + hdnSelectedUserListVal, "ResourceAllocation"));
            OpenUserResumeUrl = UGITUtility.GetAbsoluteURL(string.Format(OpenUserResumeUrl, "openuserresume", "User Resume", "ResourceAllocation"));
            if (UGITUtility.ObjectToString(Request.QueryString["tabname"]) == "allocationtimeline")
                btnclose.Visible = true;
            else
                btnclose.Visible = false;

            //Read Manager information 
            LoadAsistantsAndAllocation();


            //Get subordinate of manager if any are selected
            if (!string.IsNullOrEmpty(hdnSelectedUserList.Value))
            {
                string selecteduserIds = HttpUtility.UrlDecode(hdnSelectedUserList.Value);
                selectedUsersList.Clear();
                selectedUsersList = ObjUserProfileManager.GetUserInfosById(selecteduserIds);
            }
            
            if (!IsPostBack)
            {
                DataTable table = GetMonthAllocationData();
                rAllocationDistForm.DataSource = table;
                rAllocationDistForm.DataBind();
                DefaultView = UGITUtility.ObjectToString(ObjConfigurationVariableManager.GetValue(ConfigConstants.DefaultView));
                UGITUtility.CreateCookie(Response, "activeview", DefaultView.ToLower());
            }
            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                resultedTable.DefaultView.Sort = "Title asc";
            }
            else
            {
                if ((!IsPostBack || !string.IsNullOrWhiteSpace(UGITUtility.GetCookieValue(Request, "managerchanged"))) && !this.ShowTimeOffOnly)
                {
                    UGITUtility.DeleteCookie(Request, Response, "managerchanged");
                    resourceChk.Checked = true;
                }
            }
            timeOffAllocationLink = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&hdnChildOfVal={1}&hdnParentOfVal={2}" +
                "&workItemType={3}&hdnSelectedUserListVal={4}&ShowTimeOffOnly=true&resourceChecked={5}&usermanagers={6}",
                       "CustomResourceAllocation", hdnChildOfVal, hdnParentOfVal, "Time Off", hdnSelectedUserListVal, resourceChk.Checked.ToString(), UGITUtility.GetCookieValue(Request, "usermanagers").ToString()));
            if (ShowTimeOffOnly)
            {
                gvFilteredList.Visible = false;
                dvAssociateCardView.Visible = false;
                pnlAllocationTimeline.Visible = false;
                ResourceListTD.Visible = false;
            }
            gvFilteredList.DataSource = resultedTable;
            gvFilteredList.DataBind();
            if (gridAllocation.EditingRowVisibleIndex == -1)
            {
                BindAllocation();
                gridAllocation.DataBind();
            }
            LoadTileView();
            LoadAllocationTimeline();
            btnExpandAll.Visible = false;
            btnCollapseAll.Visible = false;
            btCollapseAllAllocation.Style.Remove("display");
            btExpandAllAllocation.Style.Remove("display");
            if (ShowTimeOffOnly && TimeOffAllocatin != null)
            {
                LoadTimeOffCardView();
            }
            //string renderMode = UGITUtility.GetCookieValue(Request, "activeview");
            //if (renderMode == "gantt")
            //{
            //    LoadAllocationTimeline();
            //}
            //else
            //{
            //    btnExpandAll.Visible = false;
            //    btnCollapseAll.Visible = false;
            //    btCollapseAllAllocation.Style.Remove("display");
            //    btExpandAllAllocation.Style.Remove("display");
            //}

            //resourceChk.Checked = string.IsNullOrWhiteSpace(UGITUtility.GetCookieValue(Request, "userall")) ? true : false;
        }

        private void LoadAsistantsAndAllocation()
        {

            ResourceColumnsSetting();
            moveUp.Visible = false;
            if (hdnParentOf.Value.Trim() == string.Empty && hdnChildOf.Value.Trim() == string.Empty)
            {
                UserProfile userInfo = ObjUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
                selectedUsersList.Clear();
                if (userInfo != null)
                {
                    selectedUsersList.Add(userInfo);
                    manager = userInfo;
                    moveUp.Visible = false;
                    CreateResourceTable(HttpContext.Current.CurrentUser().Id);
                }
            }
            else if (hdnParentOf.Value.Trim() != string.Empty)
            {
                string userId = hdnParentOf.Value;
                UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);

                if (userInfo != null)
                {
                    if (string.IsNullOrEmpty(userInfo.ManagerID) || userInfo.Id == userInfo.ManagerID || (!isResourceAdmin && userInfo.ManagerID == HttpContext.Current.CurrentUser().Id))
                    {
                        moveUp.Visible = false;
                    }
                    else
                    {
                        UserProfile currentuserManager = ObjUserProfileManager.LoadById(userInfo.ManagerID);
                        if (currentuserManager != null && !string.IsNullOrWhiteSpace(currentuserManager.ManagerID))
                        {
                            moveUp.Visible = true;
                            moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", currentuserManager.ManagerID));
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(userInfo.ManagerID))
                    {
                        UserProfile managerInfo = ObjUserProfileManager.LoadById(userInfo.ManagerID);
                        if (managerInfo != null)
                        {
                            selectedUsersList.Clear();
                            selectedUsersList.Add(managerInfo);
                            manager = managerInfo;
                            CreateResourceTable(userInfo.ManagerID);
                        }
                    }
                }
            }
            else if (hdnChildOf.Value.Trim() != string.Empty)
            {
                moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", hdnChildOf.Value));
                string userId = hdnChildOf.Value;
                UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);

                if (userInfo != null)
                {

                    if ((!isResourceAdmin && userInfo.Id == HttpContext.Current.CurrentUser().Id) || string.IsNullOrEmpty(userInfo.ManagerID) || userInfo.Id == userInfo.ManagerID)
                    {
                        moveUp.Visible = false;
                    }
                    else
                    {
                        moveUp.Visible = true;
                        moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", userInfo.ManagerID));
                    }

                    selectedUsersList.Clear();
                    selectedUsersList.Add(userInfo);
                    manager = userInfo;
                    CreateResourceTable(userId);
                }
            }

            //Set selected resource id in cockies
            if (manager != null && !string.IsNullOrEmpty(manager.Id))
            {
                UGITUtility.CreateCookie(Response, resourceFromCookies, manager.Id.ToString());
                hdnChildOfVal = UGITUtility.ObjectToString(manager.Id);
            }
        }
        protected void GvFilteredList_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            GridViewRow gvRow = e.Row;

            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                bool isSelected = false;
                if (selectedUsersList.Exists(delegate (UserProfile ev) { return ev.Id.ToString() == Convert.ToString(rowView[DatabaseObjects.Columns.Id]); }))
                {
                    // gvRow.CssClass = "ugitsellinkbg";
                    gvRow.BackColor = System.Drawing.Color.AliceBlue;
                    isSelected = true;
                }
                gvRow.Cells[0].Text = string.Format("<input type='checkbox' id='{0}-{1}' {2} onclick='event.cancelBubble=true;CheckUser(\"{1}\", this.id)' class='usercheck11' name='userCheck' />", gvRow.RowIndex, rowView[DatabaseObjects.Columns.Id], isSelected ? "checked='checked'" : string.Empty);
                List<UsersEmail> userInfo = new List<UsersEmail>();
                //string userLookup = new SPFieldUserValue(SPContext.Current.Web, string.Format("{0}{1}{2}", rowView[DatabaseObjects.Columns.Id], Constants.Separator, rowView[DatabaseObjects.Columns.Title]));
                //ObjUserProfileManager.AddUsersFromFieldUserValue(ref userInfo, userLookup, true, false);
                List<string> toolTip = userInfo.Select(x => x.userToolTip).ToList();
                string sourceUrl = Request.Url.PathAndQuery;
                string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/userinfo.aspx?uID={0}&RMMCardView=1", rowView[DatabaseObjects.Columns.Id]));
                string userName = Convert.ToString(rowView[DatabaseObjects.Columns.Title]);
                string usrLinkUr = string.Format("<a title='{4}' class='jqtooltip' href='javascript:window.parent.UgitOpenPopupDialog(\"{2}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{3}\")'>{5}</a>{0}",
                                                bool.Parse(rowView["IsAssistantExist"].ToString()) ? string.Format("&nbsp;<img onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' style='width:20px;' src='/Content/images/downarrow_new.png' alt='Down' title='Move down'/>", rowView[DatabaseObjects.Columns.Id]) : "&nbsp;&nbsp;&nbsp;",
                                                userName.Replace("'", string.Empty), userLinkUrl, sourceUrl, string.Join(" ", toolTip).Replace("'", string.Empty), userName);

                UserProfile userCheck = ObjUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
                if (!ObjUserProfileManager.IsAdmin(userCheck) && !ObjUserProfileManager.IsResourceAdmin(userCheck))
                {
                    usrLinkUr = string.Format("<div title='{1}' class='jqtooltip' >{2}</a>{0}</div>",
                                                bool.Parse(rowView["IsAssistantExist"].ToString()) ? string.Format("&nbsp;<img onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' style='width:20px;' src='/Content/images/downarrow_new.png' alt='Down' title='Move down'/>", rowView[DatabaseObjects.Columns.Id]) : "&nbsp;&nbsp;&nbsp;",
                                                 string.Join(" ", toolTip).Replace("'", string.Empty), userName);
                }
                IEnumerable<string> columns = gvFilteredList.Columns.Cast<TemplateField>().Select(x => x.HeaderText);
                var allocaionHead = columns.First(x => x == "Alloc%");
                if (allocaionHead != null)
                {
                    int colIndex = columns.ToList().IndexOf(allocaionHead);
                    string allocationBar = GetAllocatedPercentage(Convert.ToString(rowView[DatabaseObjects.Columns.Id]), Convert.ToString(lblSelectedYear.Text));
                    gvRow.Cells[colIndex].Text = string.Format("<div style='float:left;width:50px;'>{0}</div><div style='float:left;padding-left:5px;'>{1}</div>", allocationBar, usrLinkUr);
                }
            }

        }
        private string CreateAllocationBar(string userId)
        {
            StringBuilder bar = new StringBuilder();
            double pctAllocation = allocManager.AllocationPercentage(userId, 1, false);
            double percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "rmmEmpty-ProgressBar emptyProgressBar";
            percentage = (long)Math.Round(pctAllocation);
            if (percentage > 100)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:5px;z-index:1; color:#FFF;'>{2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:5px;z-index:1;'>{2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }

            return bar.ToString();
        }

        private string CreateAllocationBar(string userId, string year)
        {
            StringBuilder bar = new StringBuilder();

            DateTime startDate = new DateTime(UGITUtility.StringToInt(year), 1, 1);
            DateTime endDate = new DateTime(UGITUtility.StringToInt(year), 12, 31);
            double pctAllocation = allocManager.AllocationPercentage(userId, startDate, endDate, false);
            double percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "rmmEmpty-ProgressBar emptyProgressBar";
            percentage = (long)Math.Round(pctAllocation);
            if (percentage > 100)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:8px;z-index:1; color:#FFF;'>{2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:8px;z-index:1;'>{2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }

            return bar.ToString();
        }

        private string GetAllocatedPercentage(string userId, string year)
        {
            StringBuilder bar = new StringBuilder();

            DateTime startDate = new DateTime(UGITUtility.StringToInt(year), 1, 1);
            DateTime endDate = new DateTime(UGITUtility.StringToInt(year), 12, 31);
            double pctAllocation = allocManager.AllocationPercentage(userId, startDate, endDate, false);
            double percentage = 0;
            //string progressBarClass = "progressbar";
            //string empltyProgressBarClass = "rmmEmpty-ProgressBar emptyProgressBar";
            percentage = (long)Math.Round(pctAllocation);
            
            //progressBarClass = "progressbarhold";
            bar.AppendFormat("<div style='position:relative;'><strong style='font-size:14px;width:100%;text-align:center;'>{0}%</strong></div>", percentage);
            
            return bar.ToString();
        }

        private void ResourceColumnsSetting()
        {
            // gvFilteredList.AllowFiltering = false;
            //gvFilteredList.IsAllowSort = false;
            //gvFilteredList.AllowPaging = false;
        }

        private void CreateResourceTable(string userId)
        {
            List<UserProfile> userProfiles = ObjUserProfileManager.GetUserByManager(userId).Where(x => x.Enabled).ToList();
            if (userProfiles != null && userProfiles.Count > 0)
            {
                DataTable result = new DataTable();

                result.Columns.Add("ID", typeof(string));
                result.Columns.Add("Title", typeof(string));
                result.Columns.Add("BudgetLookup", typeof(string));
                result.Columns.Add("ResourceHourlyRate", typeof(int));
                result.Columns.Add("DepartmentLookup", typeof(string));
                result.Columns.Add("LocationLookup", typeof(string));
                result.Columns.Add("Allocation", typeof(string));
                result.Columns.Add("IsAssistantExist", typeof(bool));
                result.Columns.Add("Consultant");
                List<UserProfile> userInfoList = ObjUserProfileManager.GetUsersProfile();
                foreach (UserProfile user in userProfiles)
                {
                    StringBuilder consultantText = new StringBuilder();

                    if (user.IsIT)
                        consultantText.Append("IT");

                    if (user.IsConsultant)
                        consultantText.Append(" Consultant");
                    else
                        consultantText.Append(" Staff");

                    if (user.IsManager)
                        consultantText.Append(" (Manager)");

                    string budgetCategory = Convert.ToString(Convert.ToInt32(user.BudgetCategory) > 0 ? user.BudgetCategory : 0);

                    bool isAssistantExist = false;

                    //Check whether assistant of user exist or not
                    List<UserProfile> userCollection = userInfoList.Where(x => x.ManagerID == user.Id).ToList();
                    if (userCollection != null && userCollection.Count > 0)
                        isAssistantExist = true;

                    if (user != null && (user.ManagerID == null || user.Id != ObjUserProfileManager.LoadById(user.Id).ManagerID))
                        result.Rows.Add(user.Id, user.Name, Convert.ToString(budgetCategory), user.HourlyRate, Convert.ToString(user.Department), Convert.ToString(user.Location), "", isAssistantExist, Convert.ToString(consultantText).TrimStart());
                }
                resultedTable = result;
            }

            if ((resultedTable == null || resultedTable.Rows.Count <= 0) && manager != null)
            {
                gvFilteredList.SettingsText.EmptyDataRow = string.Format("No resources available under \"{0}\"", manager.Name);
            }
        }

        protected void btShowAllocation_Click(object sender, EventArgs e)
        {
            gridAllocation.FocusedRowIndex = -1;
            gridAllocation.ExpandAll();
        }
        #endregion

        #region Allocation Grid

        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if (gridAllocation.DataSource == null)
            {
                BindAllocation();
            }
            if (ShowTimeOffOnly && TimeOffAllocatin != null)
            {
                LoadTimeOffCardView();
            }
        }

        private void PrepareAllocationGrid()
        {
            if (gridAllocation.Columns.Count <= 0)
            {
                bool ShowERPJobID = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                string ERPJobIDName = ObjConfigurationVariableManager.GetValue(ConfigConstants.ERPJobIDName);

                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Resource;
                colId.Caption = "Resource";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.GroupIndex = 0;
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.WorkItemType;
                colId.Caption = ObjConfigurationVariableManager.GetValue("RMMLevel1Name");
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.GroupIndex = 1;
                gridAllocation.Columns.Add(colId);

                //Edit
                if (allowAllocationForSelf.EqualsIgnoreCase("Edit") || HttpContext.Current.CurrentUser().IsManager || isResourceAdmin)
                {
                    //Delete
                    //GridViewDataColumn gridViewDelete = new GridViewDataColumn();
                    //gridViewDelete.Name = "Delete";
                    //gridViewDelete.Width = Unit.Percentage(5);
                    //gridAllocation.Columns.Add(gridViewDelete);
                    //edit
                    GridViewDataColumn gridViewData = new GridViewDataColumn();
                    gridViewData.Name = "Edit";
                    gridViewData.Width = Unit.Percentage(7);
                    gridViewData.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    gridAllocation.Columns.Add(gridViewData);

                }

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.WorkItemLink;
                colId.Caption = ObjConfigurationVariableManager.GetValue("RMMLevel2Name");
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.HeaderStyle.CssClass += " rmm-project-title-workitem-header";
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = Unit.Percentage(26);
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                if (ShowERPJobID)
                {
                    colId.FieldName = DatabaseObjects.Columns.ERPJobID;
                    colId.Caption = ERPJobIDName;
                    colId.Width = Unit.Percentage(13);
                }
                else
                {
                    colId.FieldName = DatabaseObjects.Columns.WorkItem;
                    colId.Caption = "Item ID";
                    colId.Width = Unit.Percentage(13);
                }

                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);


                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.SubWorkItem;
                colId.Caption = ObjConfigurationVariableManager.GetValue("RMMLevel3Name");
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.Width = Unit.Percentage(15);
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.PctAllocation;
                colId.Caption = "Alloc %";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.PropertiesEdit.DisplayFormatString = "{0}%";
                colId.PropertiesEdit.EncodeHtml = false;
                colId.Width = Unit.Percentage(7);
                gridAllocation.Columns.Add(colId);

                GridViewDataDateColumn colIdDate = new GridViewDataDateColumn();
                colIdDate.FieldName = DatabaseObjects.Columns.AllocationStartDate;
                colIdDate.Caption = "Start Date";
                colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colIdDate.PropertiesDateEdit.EditFormatString = "MMM-dd-yyy";
                colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
                colIdDate.PropertiesEdit.EncodeHtml = false;
                colIdDate.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.Width = Unit.Percentage(13);
                gridAllocation.Columns.Add(colIdDate);

                colIdDate = new GridViewDataDateColumn();
                colIdDate.FieldName = DatabaseObjects.Columns.AllocationEndDate;
                colIdDate.Caption = "End Date";
                colIdDate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colIdDate.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colIdDate.PropertiesDateEdit.EditFormatString = "MMM-dd-yyy";
                colIdDate.PropertiesEdit.DisplayFormatString = "MMM-dd-yyy";
                colIdDate.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colIdDate.PropertiesEdit.EncodeHtml = false;
                colIdDate.Width = Unit.Percentage(13);
                gridAllocation.Columns.Add(colIdDate);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.SoftAllocation;
                colId.Caption = "Type";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                colId.Width = Unit.Percentage(8);
                gridAllocation.Columns.Add(colId);
            }

            (gridAllocation.Columns[1] as GridViewDataColumn).EditItemTemplate = new ReadOnlyTemplate();
            (gridAllocation.Columns[2] as GridViewDataColumn).EditItemTemplate = new ReadOnlyTemplate();
            (gridAllocation.Columns[3] as GridViewDataColumn).EditItemTemplate = new ReadOnlyTemplate();
            if (Height != null && Height.Value > 0)
            {
                gridAllocation.Settings.VerticalScrollableHeight = Convert.ToInt32(Height.Value - 180);
                gridAllocation.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
        }

        DataTable GetAllocationData(DateTime sDate, DateTime eDate)
        {
            DataTable allocations = null;
            if (Request["startDate"] != null && Request["endDate"] != null)
            {
                DateTime startDate;
                DateTime endDate;
                string userID = UGITUtility.ObjectToString(Request["ID"]);
                FromAllocationGrid = true;
                DateTime.TryParse(Request["startDate"], out startDate);
                DateTime.TryParse(Request["endDate"], out endDate);

                dvAssociateCardView.Visible = false;
                dvAllocationHeader.Visible = false;
                //trAllocationDistribution.Visible = false;
                ResourceListGrid.Visible = true;
                trTaskList.Visible = false;
                ResourceListTD.Visible = false;
                aAddItem.Visible = false;

                if (Request["monthlyAllocationEdit"] == "false")
                {
                    btnSaveAllocationDistribution.Visible = false;
                    btnEditDistribution.Visible = false;
                }

                ResourceAllocationType allocationType = ResourceAllocationType.Allocation;
                Enum.TryParse(Request["allocationType"], out allocationType);
                List<string> userId = new List<string> { Request["ID"] };
                allocations = allocManager.LoadWorkAllocationByDate(userId, 4, sDate: startDate, eDate: endDate);

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

                    DataRow[] allocationRows = allocations.AsEnumerable()
                                        .Where(row => !LstClosedTicketIds.Contains(row.Field<string>("WorkItem"))).ToArray();
                    //.CopyToDataTable();
                    if (allocationRows != null && allocationRows.Count() > 0)
                    {
                        allocations = allocationRows.CopyToDataTable();
                    }
                }

                //for particular project data..
                if (!string.IsNullOrEmpty(Request["ticketId"]))
                {
                    DataRow[] drs;
                    if (!string.IsNullOrEmpty(Request["subworkitem"]))
                        drs = allocations.Select(string.Format("{0} = '{3}' AND {1} = '{4}' AND {2} = '{5}'", DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem, DatabaseObjects.Columns.SubWorkItem, Request["workItemType"], Request["ticketId"], Request["subworkitem"]));   // .AsEnumerable().Where(dr => Request["ticketId"] == dr.Field<string>(DatabaseObjects.Columns.WorkItem));
                    else
                        drs = allocations.Select(string.Format("{0} like '%{2}%' AND {1} = '{3}'", DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem, Request["workItemType"], Request["ticketId"]));

                    if (drs != null && drs.Count() > 0)
                    {
                        allocations = drs.CopyToDataTable();
                    }
                    else
                        allocations = null;
                }

                if (!string.IsNullOrEmpty(Request["capacityreport"]) && Convert.ToString(Request["capacityreport"]) == "true")
                {
                    List<UserProfile> lstUProfile = null;
                    string roleId = string.Empty;
                    if (Request["jobTitleLookup"] != null)
                    {
                        string jobtitle = Convert.ToString(Request["jobTitleLookup"]);
                        lstUProfile = ObjUserProfileManager.GetUsersProfile().Where(x => x.JobProfile == jobtitle && x.Enabled == true).ToList();
                    }
                    else if (Request["roleLookup"] != null)
                    {
                        roleId = Convert.ToString(Request["roleLookup"]);
                        lstUProfile = ObjUserProfileManager.GetUsersProfile().Where(x => x.GlobalRoleId == roleId && x.Enabled == true).ToList();
                    }
                    else if (Request["unfilledRolesLookup"] != null)
                    {
                        roleId = Convert.ToString(Request["unfilledRolesLookup"]);
                    }

                    if (Request["SelectedDepts"] != null)
                    {
                        List<string> selectedDepartments = UGITUtility.ConvertStringToList(Convert.ToString(Request["SelectedDepts"]), Constants.Separator6);
                        if (selectedDepartments != null && selectedDepartments.Count > 0)
                            lstUProfile = lstUProfile.Where(x => selectedDepartments.Any(y => y == x.Department)).ToList();
                    }

                    if (Request["unfilledRolesLookup"] != null)
                    {
                        lstUProfile = new List<UserProfile>();
                        UserProfile unassignedUser = new UserProfile() { Id = Guid.Empty.ToString(), Name = "Unfilled Roles", UserName = "UnfilledRoles", GlobalRoleId = roleId, isRole = false, TenantID = applicationContext.TenantID };
                        lstUProfile.Add(unassignedUser);
                    }

                    allocations = null;
                    selectedUsersList.Clear();
                    DataTable dr = ticketManager.GetAllProjectTickets(IncludeClosed);
                    foreach (UserProfile userProfile in lstUProfile)
                    {
                        if (allocations == null)
                        {
                            allocations = allocManager.LoadDatatableByResource(Convert.ToString(userProfile.Id), startDate, endDate, allocationType, dr, IncludeClosed, roleId);    // ResourceAllocation.LoadDatatableByResource(Convert.ToString(userID), startDate, endDate, allocationType);
                        }
                        else
                        {
                            allocations.Merge(allocManager.LoadDatatableByResource(Convert.ToString(userProfile.Id), startDate, endDate, allocationType, dr, IncludeClosed, roleId));    // ResourceAllocation.LoadDatatableByResource(Convert.ToString(userID), startDate, endDate, allocationType);
                        }
                        selectedUsersList.Add(userProfile);
                    }

                    if (allocations != null && allocations.Rows.Count > 0)
                    {
                        for (int i = 0; i < allocations.Rows.Count; i++)
                        {
                            allocations.Rows[i][DatabaseObjects.Columns.Resource] = lstUProfile.FirstOrDefault(x => x.Id == Convert.ToString(allocations.Rows[i][DatabaseObjects.Columns.ResourceId])).Name;
                        }
                    }
                }

            }
            else
            {

                if (!string.IsNullOrEmpty(Request["singleselection"]) && Convert.ToString(Request["singleselection"]) == "true")
                {
                    List<string> userId = new List<string> { Request["ID"] };
                    allocations = allocManager.LoadWorkAllocationByDate(userId, 4, sDate: sDate, eDate: eDate);
                }
                else if (selectedUsersList.Count > 0)
                {
                    allocations = allocManager.LoadWorkAllocationByDate(selectedUsersList.Select(x => x.Id).ToList(), 4, sDate: sDate, eDate: eDate);
                }

                // only if includeClosed is unchecked then filter allocation based on config variable 'MgrViewAllocDays' to filter out past allocations. 
                if (!chkIncludeClosed.Checked && allocations?.Rows?.Count > 0)
                {
                    allocations = allocations.AsEnumerable().Where(row => row.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate).AddDays(MgrViewAllocDays) >= DateTime.Now)?.Count() > 0 
                        ? allocations.AsEnumerable().Where(row => row.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate).AddDays(MgrViewAllocDays) >= DateTime.Now)?.CopyToDataTable() 
                        : null;
                }
            }

            if (allocations != null && allocations.Rows.Count > 0)
            {
                DataView dvAllocations = new DataView(allocations);
                dvAllocations.Sort = string.Format("{0} asc, {1} asc, {2} asc, {3} asc, {4} asc", DatabaseObjects.Columns.Resource, DatabaseObjects.Columns.Closed,
                                    DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate);
                allocations = dvAllocations.ToTable();
                if (allocations != null)
                {
                    if (!resourceChk.Checked)
                    {
                        if (hdnParentOf.Value.Trim() != string.Empty)
                        {
                            string userId = hdnParentOf.Value;
                            allocations = allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId) != null
                                            && allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId).Count() > 0
                                         ? allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId).CopyToDataTable()
                                         : null;
                        }
                        else if (hdnChildOf.Value.Trim() != string.Empty)
                        {
                            string userId = hdnChildOf.Value;
                            allocations = allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId) != null
                                            && allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId).Count() > 0
                                         ? allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.ResourceId) != userId).CopyToDataTable()
                                         : null;
                        }
                    }
                    if (allocations != null)
                    {
                        var tempAllocations = allocations.AsEnumerable();

                        if (this.ShowTimeOffOnly)
                        {
                            tempAllocations = allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.WorkItemType) == "Time Off");
                        }
                        else if ((Request["showtimeoff"] != null && UGITUtility.StringToBoolean(Request["showtimeoff"]) == false)
                            || Request["showtimeoff"] == null && ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.HidePTOonMgrView))
                        {
                            tempAllocations = allocations.AsEnumerable()?.Where(r => r.Field<string>(DatabaseObjects.Columns.WorkItemType) != "Time Off");
                        }

                        if (tempAllocations != null && tempAllocations.Count() > 0)
                            allocations = tempAllocations.CopyToDataTable();
                        else
                            allocations = null;
                    }
                }
            }
            return allocations;
        }

        private double CalculateProjectAllocation(DataRow dRow, DataTable table)
        {
            double pctAllocation = 0;
            DateTime pStartDate;
            DateTime pEndDate;
            DataRow[] dr = table.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.Id, dRow[DatabaseObjects.Columns.Id]));

            if (dr != null && dr.Length == 1)
            {
                string user = Convert.ToString(dr[0][DatabaseObjects.Columns.ResourceId]);
                string module = uHelper.getModuleNameByTicketId(Convert.ToString(dRow[DatabaseObjects.Columns.WorkItem]));
                if (!string.IsNullOrEmpty(module))
                {
                    List<string> viewFields = new List<string>();
                    viewFields.Add(DatabaseObjects.Columns.TicketActualStartDate);
                    viewFields.Add(DatabaseObjects.Columns.TicketActualCompletionDate);
                    DataRow projectItem = Ticket.GetCurrentTicket(applicationContext, module, Convert.ToString(dRow[DatabaseObjects.Columns.WorkItem]));

                    pStartDate = DateTime.MinValue;
                    if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualStartDate) && projectItem[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                        pStartDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualStartDate]);

                    pEndDate = DateTime.MinValue;
                    if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualCompletionDate) && projectItem[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                        pEndDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);


                    int workingHrs = uHelper.GetWorkingHoursInADay(applicationContext);

                    string level1Type = module == "PMM" ? Constants.RMMLevel1PMMProjectsType : Constants.RMMLevel1TSKProjectsType;
                    List<RResourceAllocation> resAlloctions = allocManager.LoadByWorkItem(level1Type, Convert.ToString(dRow[DatabaseObjects.Columns.WorkItem]), null, 4, false, true);
                    if (resAlloctions == null || resAlloctions.Count <= 0)
                        return pctAllocation;

                    List<RResourceAllocation> projectResources = new List<RResourceAllocation>();
                    List<string> userIds = resAlloctions.Select(x => x.Resource).Distinct().ToList();
                    //foreach (int ur in userIds)
                    //{
                    RResourceAllocation allc = null;

                    List<RResourceAllocation> multiAllc = resAlloctions.Where(x => x.Resource == user).ToList();
                    if (multiAllc != null && multiAllc.Count >= 0)
                    {
                        allc = multiAllc.First();
                        projectResources.Add(allc);
                    }

                    // Old calculation
                    /* Combine multiple entries and calculate %age allocated using A/B where
                      B = total working days for all allocations that fall within the range
                      A = total allocation for the total of that time period
                   */
                    if (multiAllc.Count > 1)
                    {
                        allc.AllocationStartDate = multiAllc.Min(x => x.AllocationStartDate);
                        allc.AllocationEndDate = multiAllc.Max(x => x.AllocationEndDate);
                        double projectWrkHrs = 0;
                        int totalWrkHrs = 0;
                        foreach (RResourceAllocation sAllc in multiAllc)
                        {
                            DateTime sDate = Convert.ToDateTime(sAllc.AllocationStartDate);
                            DateTime eDate = Convert.ToDateTime(sAllc.AllocationEndDate);
                            if (pStartDate.Date > sDate.Date)
                                sDate = pStartDate.Date;
                            if (pEndDate.Date < eDate.Date)
                                eDate = pEndDate.Date;

                            int workDays = uHelper.GetTotalWorkingDaysBetween(applicationContext, sDate, eDate);
                            projectWrkHrs += Convert.ToDouble((workDays * workingHrs * sAllc.PctAllocation) / 100);
                            totalWrkHrs += workDays * workingHrs;
                        }

                        if (totalWrkHrs == 0)
                            pctAllocation = 0;
                        else
                            pctAllocation = (projectWrkHrs * 100) / totalWrkHrs;
                    }
                    else
                    {
                        pctAllocation = Convert.ToDouble(allc.PctAllocation);
                    }
                }
            }

            return pctAllocation;
        }

        void BindAllocation()
        {
            if (Request["selectedYear"] != null && int.TryParse(Request["selectedYear"].ToString(), out int year))
            {
                lblSelectedYear.Text = year.ToString();
            }
            else {
                lblSelectedYear.Text = DateTime.Now.Year.ToString();
            }
            if (!string.IsNullOrWhiteSpace(lblSelectedYear.Text))
            {
                startDateRange = new DateTime(UGITUtility.StringToInt(lblSelectedYear.Text.Trim()), 1, 1);
                endDateRange = new DateTime(UGITUtility.StringToInt(lblSelectedYear.Text.Trim()), 12, 31);
            }
            DataTable table = GetAllocationData(startDateRange, endDateRange);

            if (table != null && table.Rows.Count > 0 && chkIncludeClosed.Checked == false)
            {
                DataView view = table.AsDataView();
                view.RowFilter = $"{DatabaseObjects.Columns.Closed} = 'False' OR {DatabaseObjects.Columns.Closed} is NULL";
                table = view.ToTable();
            }

            if (table != null && table.Rows.Count > 0 && chkIncludeClosed.Checked)
            {
                table = table.AsEnumerable().Any(x => !(x[DatabaseObjects.Columns.Closed] != DBNull.Value && x.Field<bool>(DatabaseObjects.Columns.Closed) == true && x.Field<string>(DatabaseObjects.Columns.WorkItemType) == ModuleNames.OPM) && x.Field<string>(DatabaseObjects.Columns.Status) != "Cancelled")
                    ? table.AsEnumerable().Where(x => !(x[DatabaseObjects.Columns.Closed] != DBNull.Value && x.Field<bool>(DatabaseObjects.Columns.Closed) == true && x.Field<string>(DatabaseObjects.Columns.WorkItemType) == ModuleNames.OPM) && x.Field<string>(DatabaseObjects.Columns.Status) != "Cancelled").CopyToDataTable() : null;
            }

            if (table != null && table.Columns.Count > 0)
            {
                System.Data.DataColumn newColumn = new System.Data.DataColumn(DatabaseObjects.Columns.IsSummaryRow, typeof(System.Boolean));
                newColumn.DefaultValue = true;
                table.Columns.Add(newColumn);
                newColumn = new System.Data.DataColumn(DatabaseObjects.Columns.IsSoftAllocInPrecon, typeof(System.Boolean));
                newColumn.DefaultValue = true;
                table.Columns.Add(newColumn);
                newColumn = new System.Data.DataColumn(DatabaseObjects.Columns.IsSoftAllocInConst, typeof(System.Boolean));
                newColumn.DefaultValue = true;
                table.Columns.Add(newColumn);
                newColumn = new System.Data.DataColumn(DatabaseObjects.Columns.IsSoftAllocInCloseOut, typeof(System.Boolean));
                newColumn.DefaultValue = true;
                table.Columns.Add(newColumn);
            }

            if (ShowTimeOffOnly)
            {
                if (TimeOffAllocatin != null)
                {
                    TimeOffAllocatin.Clear();
                }
                TimeOffAllocatin = table;
            }

            AddSummaryRow(table);

            gridAllocation.DataSource = table;
        }

        public void AddSummaryRow(DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                var duplicateData = table.AsEnumerable().GroupBy(o => (o.Field<string>(DatabaseObjects.Columns.ResourceId),
                        o.Field<string>(DatabaseObjects.Columns.WorkItem), o.Field<string>(DatabaseObjects.Columns.WorkItemType)))
                        .Where(gr => gr.Count() > 1).ToList();
                if (duplicateData != null && duplicateData.Count > 0)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr[DatabaseObjects.Columns.Id].ToString() == "0")
                            dr.Delete();
                    }
                    foreach (var item in duplicateData)
                    {
                        string erpJobID = string.Empty;
                        var data = table.AsEnumerable().Where(o => item.Key.Item1 == o.Field<string>(DatabaseObjects.Columns.ResourceId)
                            && item.Key.Item2 == o.Field<string>(DatabaseObjects.Columns.WorkItem)
                            && item.Key.Item3 == o.Field<string>(DatabaseObjects.Columns.WorkItemType)).ToList();
                        if (data.Any(o => o.Field<string>(DatabaseObjects.Columns.WorkItemType) == "Time Off"))
                        {
                            continue;
                        }
                        foreach (var dataItem in data)
                        {
                            dataItem[DatabaseObjects.Columns.IsSummaryRow] = false;
                            erpJobID = dataItem[DatabaseObjects.Columns.ERPJobID].ToString();
                            if(UGITUtility.IfColumnExists(dataItem, DatabaseObjects.Columns.NonChargeable))
                                dataItem[DatabaseObjects.Columns.ERPJobID] = UGITUtility.StringToBoolean(dataItem[DatabaseObjects.Columns.NonChargeable]) ? "NCO" : "";
                        }
                        DateTime startDate = data.Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate)).Min();
                        DateTime endDate = data.Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate)).Max();
                        double pctAllocation = GetPctAllocation(data);
                        string userRole = string.Empty;
                        UserProfile userProfile = ObjUserProfileManager.GetUserById(item.Key.Item1);
                        GlobalRole uRole = roleManager.Get(x => x.Id == userProfile.GlobalRoleId);
                        if (uRole != null)
                        {
                            userRole = uRole.Name;
                        }
                        DataRow dr = table.NewRow();
                        var sourceRow = data[0];
                        dr.ItemArray = sourceRow.ItemArray.Clone() as object[];
                        dr[DatabaseObjects.Columns.Id] = 0;
                        dr[DatabaseObjects.Columns.ResourceId] = item.Key.Item1;
                        dr[DatabaseObjects.Columns.Resource] = userProfile.Name;
                        dr[DatabaseObjects.Columns.WorkItem] = item.Key.Item2;
                        dr[DatabaseObjects.Columns.WorkItemType] = item.Key.Item3;
                        dr[DatabaseObjects.Columns.ERPJobID] = erpJobID;
                        dr[DatabaseObjects.Columns.SubWorkItem] = userRole;
                        dr[DatabaseObjects.Columns.AllocationStartDate] = startDate;
                        dr[DatabaseObjects.Columns.AllocationEndDate] = endDate;
                        dr[DatabaseObjects.Columns.EstStartDate] = startDate;
                        dr[DatabaseObjects.Columns.EstEndDate] = endDate;
                        dr[DatabaseObjects.Columns.PctAllocation] = pctAllocation;
                        dr[DatabaseObjects.Columns.PctEstimatedAllocation] = pctAllocation;
                        dr[DatabaseObjects.Columns.IsSummaryRow] = true;
                        if (data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInPrecon]) != true
                            && UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInConst]) != true
                            && UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInCloseOut]) != true))
                        {
                            //dr[DatabaseObjects.Columns.IsAllocInPrecon] = true;
                            dr[DatabaseObjects.Columns.IsStartDateBeforePrecon] = true;
                        }
                        else
                        {
                            dr[DatabaseObjects.Columns.IsStartDateBeforePrecon] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsStartDateBeforePrecon]) == true);
                        }
                        dr[DatabaseObjects.Columns.IsAllocInPrecon] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInPrecon]) == true);
                        dr[DatabaseObjects.Columns.IsAllocInConst] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInConst]) == true);
                        dr[DatabaseObjects.Columns.IsAllocInCloseOut] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInCloseOut]) == true);
                        dr[DatabaseObjects.Columns.IsSoftAllocInPrecon] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInPrecon]) 
                            && UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.SoftAllocation]));
                        dr[DatabaseObjects.Columns.IsSoftAllocInConst] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInConst]) 
                            && UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.SoftAllocation]));
                        dr[DatabaseObjects.Columns.IsSoftAllocInCloseOut] = data.Any(o => UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.IsAllocInCloseOut]) 
                            && UGITUtility.StringToBoolean(o[DatabaseObjects.Columns.SoftAllocation]));
                        dr[DatabaseObjects.Columns.OnHold] = data.All(o => o.Field<int?>(DatabaseObjects.Columns.OnHold) == 1) ? 1 : 0;
                        table.Rows.Add(dr);
                    }
                }

                table.DefaultView.Sort = string.Format("{0} asc, {1} asc, {2} asc,{5} desc, {3} asc, {4} asc", DatabaseObjects.Columns.Resource,
                    DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem, DatabaseObjects.Columns.AllocationStartDate,
                    DatabaseObjects.Columns.AllocationEndDate, DatabaseObjects.Columns.IsSummaryRow);
                table.DefaultView.ToTable();
            }
        }

        public double GetPctAllocation(List<DataRow> data)
        {
            double totalDays = 0;
            double totalPercentage = 0;
            foreach (DataRow dr in data)
            {
                DateTime allocStartDate = dr.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate);
                DateTime allocEndDate = dr.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate);
                double daysDiff = (allocEndDate - allocStartDate).TotalDays;
                double pctAlloc = dr.Field<double>(DatabaseObjects.Columns.PctAllocation);
                totalPercentage += pctAlloc * daysDiff;
            }
            totalDays = (data.Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate)).Max()
                - data.Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate)).Min()).TotalDays;
            return totalDays > 0 ? Math.Round(totalPercentage / totalDays, 1) : 0.0;
        }

        protected void gridAllocation_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            List<string> users = new List<string>();
            long workItemID = 0;
            long allocationID = UGITUtility.StringToLong(e.Keys[0]);
            RResourceAllocation allocation = allocManager.LoadByID(allocationID);

            if (allocation != null)
            {
                workItemID = allocation.ResourceWorkItemLookup;
                ResourceWorkItemsManager ObjWorkItemsManager = new ResourceWorkItemsManager(applicationContext);
                allocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(allocation.ResourceWorkItemLookup);
                if (allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("PMM")
                    || allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("NPR")
                    || allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("TSK"))
                {
                    RMMSummaryHelper.CleanAllocation(applicationContext, allocation.ResourceWorkItems, cleanEstimated: true, cleanPlanned: false);
                }
                else
                {
                    allocManager.Delete(allocation);
                    allocManager.UpdateIntoCache(allocation, false);

                    users.Add(allocation.Resource);
                    //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                    ThreadStart threadStartMethod = delegate () { 
                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(applicationContext, workItemID); 
                        projectEstimatedAllocationManager.UpdateProjectGroups(uHelper.getModuleNameByTicketId(allocation.TicketID), allocation.TicketID); 
                        //projectEstimatedAllocationManager.RefreshProjectComplexity(uHelper.getModuleNameByTicketId(allocation.TicketID), users); 
                    };
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }

                // Rebind the details
                //BindAllocation();
                //gridAllocation.DataBind();

                hdnSelectedAllocation.Value = string.Empty;
                hdnSelectedWorkItem.Value = string.Empty;
            }
            e.Cancel = true;
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void gridAllocation_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            int pctComplete = 0;
            int.TryParse(Convert.ToString(e.NewValues[DatabaseObjects.Columns.PctAllocation]), out pctComplete);
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            DateTime.TryParse(Convert.ToString(e.NewValues[DatabaseObjects.Columns.AllocationStartDate]), out startDate);
            DateTime.TryParse(Convert.ToString(e.NewValues[DatabaseObjects.Columns.AllocationEndDate]), out endDate);


            if (startDate == DateTime.MinValue)
            {
                startDate = DateTime.Now.Date;
                e.NewValues[DatabaseObjects.Columns.AllocationStartDate] = startDate;
            }
            if (endDate == DateTime.MinValue)
            {
                endDate = new DateTime(DateTime.Now.Year, 12, 31);
                e.NewValues[DatabaseObjects.Columns.AllocationEndDate] = endDate;
            }


            double pctAllocation = UGITUtility.StringToDouble(e.NewValues[DatabaseObjects.Columns.PctAllocation]);
            long ID = Convert.ToInt32(e.Keys[0]);
            RResourceAllocation allocation = allocManager.LoadByID(ID);
            string message = string.Empty;
            if (allocation != null)
            {
                allocation.PctAllocation = Convert.ToInt32(pctAllocation);
                allocation.AllocationStartDate = startDate;
                allocation.AllocationEndDate = endDate;
                allocation.Resource = selectedUsersList[0].Id;
                //allocation.ResourceName = selectedUsersList[0].Name;
                message = allocManager.Save(allocation);
            }

            //if (!string.IsNullOrEmpty(message))
            //{
            //    lbAllocationError.Text = message;

            //    e.Cancel = true;
            //    gridAllocation.CancelEdit();
            //    BindAllocation();
            //    gridAllocation.DataBind();
            //}
            //else
            //{
            if (allocation.ResourceWorkItemLookup > 0)
            {
                string webUrl = HttpContext.Current.Request.Url.ToString();

                //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                ThreadStart threadStartMethod = delegate ()
                {
                    RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(applicationContext, allocation.ResourceWorkItemLookup);
                };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }

            e.Cancel = true;
            gridAllocation.CancelEdit();
            BindAllocation();
            gridAllocation.DataBind();
            gvFilteredList.DataBind();
            if (ShowTimeOffOnly && TimeOffAllocatin != null)
            {
                LoadTimeOffCardView();
            }
        }

        protected void gridAllocation_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

        }

        protected void gridAllocation_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Group)
            {
                if (e.Row.Cells.Count == 2)
                {
                    Control outerGroup = gridAllocation.FindGroupRowTemplateControl(e.VisibleIndex, "outerGroup");
                    outerGroup.Visible = true;
                    ASPxLabel label = outerGroup.FindControl("lblGroupLabel") as ASPxLabel;
                    label.Text = label.Text.Replace("&#39;", "'");
                    e.Row.Cells[1].Controls.Add(outerGroup);
                }
                if (e.Row.Cells.Count == 3)
                {
                    Control innerGroup = gridAllocation.FindGroupRowTemplateControl(e.VisibleIndex, "innerGroup");
                    innerGroup.Visible = true;
                    e.Row.Cells[2].Controls.Add(innerGroup);
                }
            }
            int startIndex = (allowAllocationForSelf.EqualsIgnoreCase("Edit") || HttpContext.Current.CurrentUser().IsManager || isResourceAdmin) ? 3 : 2;
            
                if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = gridAllocation.GetDataRow(e.VisibleIndex);
                if (gridAllocation.Columns[DatabaseObjects.Columns.WorkItemLink].Visible == true)
                {
                    if (e.Row.Cells.Count > startIndex && e.Row.Cells[startIndex].GetType().FullName == "DevExpress.Web.Rendering.GridViewTableDataCell")
                    {
                        if (gvFilteredList.Visible)
                        {
                            e.Row.Cells[startIndex].Text = string.Format("<div class='rmm-project-title-workitem' IsAllocInPrecon={1} IsAllocInConst={2} IsAllocInCloseOut={3} IsSummaryRow={4} " +
                                                                "IsStartDateBeforePrecon={5} IsStartDateBetweenPreconAndConst={6} IsStartDateBetweenConstAndCloseOut={7} IsOnHold={8}>{0}</div>",
                                                                Convert.ToString(row[DatabaseObjects.Columns.WorkItemLink]), 
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInPrecon]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInConst]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInCloseOut]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsSummaryRow]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBeforePrecon]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBetweenPreconAndConst]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBetweenConstAndCloseOut]),
                                                                row[DatabaseObjects.Columns.OnHold]);
                        }
                        else
                        {
                            e.Row.Cells[startIndex].Text = string.Format("<div class='rmm-project-title-workitem-1' IsAllocInPrecon={1} IsAllocInConst={2} IsAllocInCloseOut={3} IsSummaryRow={4} " +
                                                                "IsStartDateBeforePrecon={5} IsStartDateBetweenPreconAndConst={6} IsStartDateBetweenConstAndCloseOut={7} IsOnHold={8}>{0}</div>",
                                                                Convert.ToString(row[DatabaseObjects.Columns.WorkItemLink]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInPrecon]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInConst]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsAllocInCloseOut]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsSummaryRow]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBeforePrecon]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBetweenPreconAndConst]),
                                                                UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsStartDateBetweenConstAndCloseOut]),
                                                                row[DatabaseObjects.Columns.OnHold]);
                        }
                    }
                }
                if (gridAllocation.Columns[DatabaseObjects.Columns.ERPJobID]?.Visible == true)
                {
                    if (e.Row.Cells.Count > startIndex + 1 && e.Row.Cells[startIndex + 1].GetType().FullName == "DevExpress.Web.Rendering.GridViewTableDataCell")
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(row[DatabaseObjects.Columns.ERPJobID])))
                        {
                            string innerText = ExtractHtmlInnerText(Convert.ToString(row[DatabaseObjects.Columns.ERPJobID]));
                            if (innerText.Length > 0 && !innerText.Equals("0"))
                            {
                                bool isSummaryRow = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.IsSummaryRow]);
                                e.Row.Cells[startIndex + 1].Text = string.Format("<div class={1}>{0}</div>",
                                                                    Convert.ToString(row[DatabaseObjects.Columns.ERPJobID])
                                                                    ,isSummaryRow ? "rmm-project-title-cmic" : "rmm-project-common");
                            }
                            else
                            {
                                e.Row.Cells[startIndex + 1].Text = "";
                            }
                        }
                    }
                }
                
                    if (e.Row.Cells.Count > startIndex +2 && e.Row.Cells[startIndex +2 ].GetType().FullName == "DevExpress.Web.Rendering.GridViewTableDataCell")
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem])))
                        {
                            e.Row.Cells[startIndex + 2].Text = string.Format("<div class='rmm-project-common'>{0}</div>",
                                                                Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]));
                        }
                    }
            }
        }
        public static string ExtractHtmlInnerText(string htmlText)
        {
            //Match any Html tag (opening or closing tags) 
            // followed by any successive whitespaces
            //consider the Html text as a single line
            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);
            string resultText = regex.Replace(htmlText, " ").Trim();
            return resultText;
        }


        protected void btExpandAllAllocation_Click(object sender, ImageClickEventArgs e)
        {
            hndAllocation.Value = "ExpandAll";
            gridAllocation.ExpandAll();

        }

        protected void btCollapseAllAllocation_Click(object sender, ImageClickEventArgs e)
        {
            hndAllocation.Value = "CollapseAll";
            gridAllocation.CollapseAll();
        }

        protected void gridAllocation_FocusedRowChanged(object sender, EventArgs e)
        {
            if (gridAllocation.EditingRowVisibleIndex > 0 && gridAllocation.FocusedRowIndex > 0)
                gridAllocation.CancelEdit();
            if (gridAllocation.Styles.FocusedRow.CssClass.Contains("dxgvDataRowAlt_UGITBlackDevEx"))
            {
                string cssclass = gridAllocation.Styles.FocusedRow.CssClass.Replace("ms-alternatingstrong", "");
                gridAllocation.Styles.FocusedRow.CssClass = cssclass;
            }
            int oldAllocation = 0;
            int.TryParse(hdnSelectedAllocation.Value, out oldAllocation);

            if (gridAllocation.GetChildRowCount(gridAllocation.FocusedRowIndex) > 0)
                gridAllocation.FocusedRowIndex = gridAllocation.FocusedRowIndex + 1;

            hdnSelectedAllocation.Value = string.Empty;
            hdnSelectedWorkItem.Value = string.Empty;

            if (gridAllocation.EditingRowVisibleIndex == gridAllocation.FocusedRowIndex)
            {
                gridAllocation.FocusedRowIndex = -1;
                gridAllocation.ExpandAll();
                return;
            }
            DataRow rowView = (DataRow)gridAllocation.GetDataRow(gridAllocation.FocusedRowIndex);
            if (rowView == null)
                return;
            int selectedID = 0;
            int.TryParse(Convert.ToString(rowView[DatabaseObjects.Columns.Id]), out selectedID);
            gridAllocation.DataBind();
            int workItemID = 0;
            //int.TryParse(Convert.ToString(gridAllocation.GetRowValuesByKeyValue(selectedID, DatabaseObjects.Columns.WorkItemID)), out workItemID);
            int.TryParse(Convert.ToString(rowView[DatabaseObjects.Columns.WorkItemID]), out workItemID);
            if (selectedID > 0)
            {
                hdnSelectedAllocation.Value = selectedID.ToString();
                hdnSelectedWorkItem.Value = workItemID.ToString();
                if (oldAllocation != selectedID)
                {
                    BindMonthAllocation();
                    rAllocationDistForm.DataBind();
                }
            }
            ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
            List<UGITModule> drModules = moduleViewManager.Load(x => x.ModuleName == Convert.ToString(rowView[1]) || x.Title == Convert.ToString(rowView[1]));
            if (drModules != null && drModules.Count > 0)
            {
                trTaskList.Visible = true;
                TaskList importControl = (TaskList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/TaskList.ascx");
                importControl.ModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(rowView[2])); // "PMM";
                importControl.TicketPublicId = Convert.ToString(rowView[2]);
                importControl.ShowBaseline = false;
                importControl.BaselineNum = 0;
                importControl.IsReadOnly = true;
                TasklistControl.Controls.Add(importControl);
            }

        }

        protected void gridAllocation_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindAllocation();
            gridAllocation.DataBind();
        }

        protected void gridAllocation_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name.Equals("Edit"))
            {
                string value = Convert.ToString(e.KeyValue);
                string username = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Resource));
                //e.Cell.Text = "<img style='transform:scale(2.3);' class='myClass' src='/content/images/editIcon-new.png' onclick=\"openworkitem('" + value+"','" + username +")\" width=30px; />";
                if (value != "0")
                {
                    e.Cell.Text = $"<img class='myClass' src='/content/images/editIcon-new.png' onclick=\"openworkitemingrid('{value}','{username}')\" width='24px' />";
                    e.Cell.Text += $"<img style='transform: scale(1.2);margin-right:27%;' class='myClass' src='/content/images/deleteIcon-new.png' onclick=\"deleteworkitem('{value}')\"  width=27px; />";
                }
                else
                {
                    e.Cell.Text = "";
                }

                e.DataColumn.ReadOnly = true;               
            }
            //if (e.DataColumn.Name.EqualsIgnoreCase("Delete"))
            //{
            //    string value = Convert.ToString(e.KeyValue);
            //    //e.Cell.Text = "<img src='/content/images/redNew_delete.png' alt='" + value + "' class='btnDeleteCss' width=16px; />";
            //    // above line of code will not work after page load, so added below line 
            //    e.Cell.Text = "<img class='myClass' src='/content/images/deleteIcon-new.png' onclick=\"deleteworkitem('" + value+"')\"  width=27px; />";
            //    e.DataColumn.ReadOnly = true;
            //}
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.PctAllocation || e.DataColumn.FieldName == DatabaseObjects.Columns.PctEstimatedAllocation || e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationStartDate ||
                e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationEndDate || e.DataColumn.FieldName == DatabaseObjects.Columns.SoftAllocation)
            {
                string type = Convert.ToString(e.GetValue(DatabaseObjects.Columns.WorkItemType));
                bool isAllocInPrecon = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsAllocInPrecon));
                bool IsStartDateBeforePrecon = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsStartDateBeforePrecon));
                bool isAllocInConst = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsAllocInConst));
                bool isAllocInCloseOut = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsAllocInCloseOut));
                bool isSoftAllocInPrecon = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsSoftAllocInPrecon));
                bool isSoftAllocInConst = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsSoftAllocInConst));
                bool isSoftAllocInCloseOut = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsSoftAllocInCloseOut));
                bool isSoftAllocation = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.SoftAllocation));
                bool isSummaryRow = Convert.ToBoolean(e.GetValue(DatabaseObjects.Columns.IsSummaryRow));

                if (type == "Current Projects (PMM)" || type == "Project Management Module (PMM)")
                    type = "PMM";

                if (type == "Opportunity Management (OPM)" || type == "Opportunities (OPM)")
                    type = ModuleNames.OPM;

                if (type == "Project Management (CPR)" || type == "Construction Projects (CPR)")
                    type = ModuleNames.CPR;

                if (type == "Service Projects (CNS)")
                    type = ModuleNames.CNS;

                if (type == "Task Lists (TSK)")
                    type = ModuleNames.TSK;


                bool enableRMMAssignment = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableRMMAssignment);
                if (type == ModuleNames.PMM || type == uHelper.GetModuleTitle("NPR") || type == uHelper.GetModuleTitle("TSK") || type == ModuleNames.TSK || type == ModuleNames.NPR
                  || type == ModuleNames.OPM || type == ModuleNames.CPR || type == ModuleNames.CNS || type.Equals("Time Off"))
                {
                    if (e.DataColumn.FieldName == DatabaseObjects.Columns.PctAllocation)
                    {
                        if (enableRMMAssignment)
                        {
                            double plnd = UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.PctPlannedAllocation));
                            double est = UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.PctEstimatedAllocation));
                            e.Cell.Text = string.Format("<div class='rmm-project-common'><div>Alloc: {0:F0}%</div><div>Asgn: {1:F0}%</div></div>", est, plnd);
                        }
                        else
                        {
                            double est = UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.PctEstimatedAllocation));
                            e.Cell.Text = string.Format("<div class='rmm-project-common'><div>{0:F0}%</div></div>", est);
                        }
                    }
                    else if (e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationStartDate)
                    {
                        DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                        DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.EstStartDate));
                        string plnd = "-";
                        string est = "-";
                        if (plndD != DateTime.MinValue && plndD != Convert.ToDateTime("01-01-1753 00:00:00"))
                        {
                            plnd = UGITUtility.GetDateStringInFormat(plndD, false);
                        }
                        if (estD != DateTime.MinValue && estD != Convert.ToDateTime("01-01-1753 00:00:00"))
                        {
                            est = UGITUtility.GetDateStringInFormat(estD, false);
                        }
                        string txt = string.Empty;
                        if (plndD != DateTime.MinValue && estD != DateTime.MinValue)
                        {
                            if (plndD.Date == estD.Date)
                                txt = string.Format("<div>{0}</div>", est);
                            else
                                txt = string.Format("<div>{0}</div><div>{1}</div>", est, plnd);
                        }
                        else if (plndD != DateTime.MinValue)
                        {
                            txt = string.Format("<div>Asgn: {0}</div>", plnd);
                        }
                        else if (estD != DateTime.MinValue)
                        {
                            if (enableRMMAssignment)
                                txt = string.Format("<div>Alloc: {0}</div>", est);
                            else
                                txt = string.Format("<div>{0}</div>", est);
                        }
                        string startDateCssClass = IsStartDateBeforePrecon ? "rmm-project-common" : isAllocInPrecon ? "datePreconClass" : isAllocInConst ? "dateConstClass" : isAllocInCloseOut ? "dateCloseoutClass" : "rmm-project-common";
                        e.Cell.Text = string.Format("<div class='{1}'>{0}</div>", txt, startDateCssClass);
                    }
                    else if (e.DataColumn.FieldName == DatabaseObjects.Columns.AllocationEndDate)
                    {
                        DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedEndDate));
                        DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.EstEndDate));
                        string plnd = "-";
                        string est = "-";
                        if (plndD != DateTime.MinValue && plndD != Convert.ToDateTime("01-01-1753 00:00:00"))
                        {
                            plnd = UGITUtility.GetDateStringInFormat(plndD, false);
                        }
                        if (estD != DateTime.MinValue && estD != Convert.ToDateTime("01-01-1753 00:00:00"))
                        {
                            est = UGITUtility.GetDateStringInFormat(estD, false);
                        }
                        string txt = string.Empty;
                        if (plndD != DateTime.MinValue && estD != DateTime.MinValue)
                        {
                            if (plndD.Date == estD.Date)
                                txt = string.Format("<div>{0}</div>", est);
                            else
                                txt = string.Format("<div>{0}</div><div>{1}</div>", est, plnd);
                        }
                        else if (plndD != DateTime.MinValue)
                        {
                            txt = string.Format("<div>Asgn: {0}</div>", plnd);
                        }
                        else if (estD != DateTime.MinValue)
                        {
                            if (enableRMMAssignment)
                                txt = string.Format("<div>Alloc: {0}</div>", est);
                            else
                                txt = string.Format("<div>{0}</div>", est);
                        }
                        string endDateCssClass = isAllocInCloseOut ? "dateCloseoutClass" : isAllocInConst ? "dateConstClass" : isAllocInPrecon ? "datePreconClass" : "rmm-project-common";
                        e.Cell.Text = string.Format("<div class='{1}'>{0}</div>", txt, endDateCssClass);
                    }
                    else if (e.DataColumn.FieldName == DatabaseObjects.Columns.SoftAllocation)
                    {
                        string txt = string.Empty;
                        if (!isSummaryRow)
                        {
                            txt = isSoftAllocation ? "Soft" : "Hard";
                            //txt = Convert.ToString(e.GetValue(DatabaseObjects.Columns.SoftAllocation));
                            e.Cell.Text = string.Format("<div class='rmm-project-common'>{0}</div>", txt);
                        }
                        else
                        {
                            
                            string id = e.GetValue(DatabaseObjects.Columns.Id).ToString();
                            string oText = string.Empty;
                            if (id == "0")
                            {
                                oText = string.Format("<div class='alloctype'><i class='{0}' style='font-size: 24px; color:#52BED9'></i>" +
                                "<i class='{1}' style='font-size: 24px; color:#005C9B'></i>" +
                                "<i class='{2}' style='font-size: 24px; color:#351B82'></i></div>",
                                isAllocInPrecon && !isSoftAllocation ? "fa fa-circle" : "far fa-circle",
                                isAllocInConst && !isSoftAllocation ? "fa fa-circle" : "far fa-circle",
                                isAllocInCloseOut && !isSoftAllocation ? "fa fa-circle" : "far fa-circle");
                            }
                            else
                            {
                                oText = string.Format("<div class='alloctype'><i class='{0}' style='font-size: 24px; color:#52BED9'></i>" +
                                "<i class='{1}' style='font-size: 24px; color:#005C9B'></i>" +
                                "<i class='{2}' style='font-size: 24px; color:#351B82'></i></div>",
                                isAllocInPrecon && !isSoftAllocation ? "fa fa-circle" : "far fa-circle",
                                isAllocInConst && !isSoftAllocation ? "fa fa-circle" : "far fa-circle",
                                isAllocInCloseOut && !isSoftAllocation ? "fa fa-circle" : "far fa-circle");
                            }
                            e.Cell.Text = oText;
                        }
                    }
                }
                else if (e.DataColumn.FieldName == DatabaseObjects.Columns.SoftAllocation)
                {
                    string txt = string.Empty;
                    if (!isSummaryRow)
                    {
                        txt = isSoftAllocation ? "Soft" : "Hard";
                        //txt = Convert.ToString(e.GetValue(DatabaseObjects.Columns.SoftAllocation));
                        e.Cell.Text = string.Format("<div class='rmm-project-common'>{0}</div>", txt);
                    }
                    else
                    {
                        string oText = string.Format("<div class='alloctype'><i class='{0} fa-circle' style='font-size: 22px; color:#52BED9'></i>" +
                            "<i class='{1} fa-circle' style='font-size: 22px; color:#005C9B'></i>" +
                            "<i class='{2} fa-circle' style='font-size: 22px; color:#351B82'></i></div>",
                            isAllocInPrecon ? "fa" : "far",
                            isAllocInConst ? "fa" : "far",
                            isAllocInCloseOut ? "fa" : "far");
                        e.Cell.Text = oText;
                    }
                }
            }
        }
        protected void gridAllocation_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
        {
            if (e.Item.Name == "Sync")
            {
                DataRow row = gridAllocation.GetDataRow(e.ElementIndex) as DataRow;
                long ID = UGITUtility.StringToLong(row[DatabaseObjects.Columns.Id]);
                RResourceAllocation allocation = allocManager.Get(ID);
                string message = string.Empty;
                if (allocation != null)
                {
                    if (allocation.PlannedStartDate.HasValue)
                        allocation.AllocationStartDate = allocation.PlannedStartDate;
                    if (allocation.PlannedEndDate.HasValue)
                        allocation.AllocationEndDate = allocation.PlannedEndDate;

                    allocation.PctAllocation = Convert.ToInt32(Math.Round(Convert.ToDouble(allocation.PctPlannedAllocation), 2));
                    message = allocManager.Save(allocation);


                    //Update Monthly distribution when user do sync
                    long workItemID = allocation.ResourceWorkItemLookup;
                    //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                    ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(applicationContext, workItemID); };
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }
                if (string.IsNullOrWhiteSpace(message))
                {
                    lbAllocationError.Text = "Sync successfully";
                    BindAllocation();
                    gridAllocation.DataBind();
                }
            }
        }
        protected void gridAllocation_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            e.Items.Clear();
            if (e.MenuType == GridViewContextMenuType.Rows)
            {
                var item = e.CreateItem("Sync Planned to Estimated", "Sync");
                item.Image.Url = "/content/Images/refresh-icon.png";
                e.Items.Insert(0, item);
            }
        }
        #endregion

        #region Grid Month Allocation

        DataTable GetMonthAllocationData()
        {

            DataTable allocationDist = CreateAllocationMonthlySchema();
            DateTime allocStartDate = DateTime.MinValue;
            DateTime allocEndDate = DateTime.MinValue;
            DataTable allocations = null;
            long workItemID = 0;
            long allocationID = 0;
            if (!string.IsNullOrEmpty(hdnSelectedWorkItem.Value))
            {
                long.TryParse(hdnSelectedAllocation.Value, out allocationID);
                long.TryParse(hdnSelectedWorkItem.Value, out workItemID);
            }
            else
            {
                if (Request[hdnSelectedWorkItem.UniqueID] != null)
                {
                    long.TryParse(Request[hdnSelectedAllocation.UniqueID], out allocationID);
                    long.TryParse(Request[hdnSelectedWorkItem.UniqueID], out workItemID);
                }
                else
                {
                    long.TryParse(Request[hdnSelectedAllocation.UniqueID], out allocationID);
                    DataTable allocationTable = (DataTable)gridAllocation.DataSource;
                    if (allocationTable != null && allocationTable.Rows.Count > 0)
                    {
                        DataRow sRow = allocationTable.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.Id) == allocationID);
                        if (sRow != null)
                        {
                            long.TryParse(Convert.ToString(sRow[DatabaseObjects.Columns.WorkItemID]), out workItemID);
                        }
                    }
                }
            }

            // DataRowView selectedAlloc = (DataRowView)gridAllocation.GetRow(gridAllocation.FindVisibleIndexByKeyValue(allocationID));
            DataRowView selectedAlloc = (DataRowView)gridAllocation.GetRow(gridAllocation.FocusedRowIndex);

            if (workItemID > 0 && selectedAlloc != null)
            {
                allocStartDate = Convert.ToDateTime(selectedAlloc[DatabaseObjects.Columns.AllocationStartDate]);
                allocEndDate = Convert.ToDateTime(selectedAlloc[DatabaseObjects.Columns.AllocationEndDate]);

                allocStartDate = allocStartDate.AddDays(-allocStartDate.Day + 1);
                allocEndDate = allocEndDate.AddDays(-allocEndDate.Day + 1);

                allocations = RMMSummaryHelper.GetMonthlyDistributedAllocations(applicationContext, new List<long>() { workItemID });
                if (allocations != null)
                {
                    DataRow allocationDistRow = allocations.Rows[0];
                    List<DateTime> distributionDates = allocations.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "ID" && (Convert.ToDateTime(x.ColumnName).Date >= allocStartDate.Date && Convert.ToDateTime(x.ColumnName).Date <= allocEndDate.Date)).Select(x => Convert.ToDateTime(x.ColumnName)).ToList();
                    var distributionYears = distributionDates.ToLookup(x => x.Year);
                    foreach (var dYear in distributionYears)
                    {
                        DataRow row = allocationDist.NewRow();
                        allocationDist.Rows.Add(row);
                        row["Year"] = dYear.Key;
                        List<DateTime> dateRanges = dYear.ToList();
                        foreach (DateTime rDate in dateRanges)
                        {
                            row[rDate.ToString("MMM")] = allocationDistRow[rDate.ToString("MMM/dd/yyyy")];
                        }
                    }
                }
            }
            return allocationDist;
        }

        void BindMonthAllocation()
        {
            DataTable table = GetMonthAllocationData();

            //if (table != null && table.Rows.Count > 0)
            //{                
            //    trAllocationDistribution.Visible = true;
            //}

            rAllocationDistForm.DataSource = table;
            btnSaveAllocationDistribution.Visible = false;
            btnEditDistribution.Visible = true;
        }

        DataTable CreateAllocationMonthlySchema()
        {
            DataTable table = new DataTable();
            DataColumn column = table.Columns.Add("Year", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Jan", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Feb", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Mar", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Apr", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("May", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Jun", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Jul", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Aug", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Sep", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Oct", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Nov", typeof(double));
            column.DefaultValue = 0;
            column = table.Columns.Add("Dec", typeof(double));
            column.DefaultValue = 0;
            return table;
        }

        protected void btnSaveAllocationDistribution_Click(object sender, EventArgs e)
        {
            List<ResourceDistributionItem> distributions = new List<ResourceDistributionItem>();
            DataTable table = GetMonthAllocationData();
            if (table != null)
            {
                foreach (RepeaterItem rItem in rAllocationDistForm.Items)
                {
                    DataRow data = table.Rows[rItem.ItemIndex];
                    int year = Convert.ToInt32(data["Year"]);
                    for (int i = 1; i <= 12; i++)
                    {
                        ASPxTextBox txtBox = (ASPxTextBox)rItem.FindControl("txt" + i);
                        double pctAlloction = 0;
                        double.TryParse(txtBox.Text.Replace("%", "").Trim(), out pctAlloction);

                        if (pctAlloction > 0)
                            distributions.Add(new ResourceDistributionItem()
                            {
                                Date = new DateTime(year, i, 1),
                                PctAllocation = pctAlloction
                            });

                        ASPxLabel lb = (ASPxLabel)rItem.FindControl(string.Format("lb{0}", i));
                        lb.Text = string.Format("{0}%", pctAlloction);
                    }
                }
            }

            gridAllocation.DataBind();

            //Trim monthDist list
            DateTime startDate = startDateRange.Date;
            DateTime endDate = startDateRange.Date;
            if (distributions.Count > 0)
            {
                startDate = distributions.Min(x => x.Date).Date;
                endDate = distributions.Max(x => x.Date).Date;
            }
            DateTime tempDate = startDate;
            double totalWorkingHrs = 0;
            double allocatedHrs = 0;

            int workindHrsInDay = uHelper.GetWorkingHoursInADay(applicationContext);

            //Save monthly allocation pct against workitem item of resource
            int workItemID = 0;
            int.TryParse(hdnSelectedWorkItem.Value, out workItemID);
            if (workItemID > 0)
            {
                long allocationID = UGITUtility.StringToLong(hdnSelectedAllocation.Value);
                RResourceAllocation resAllocation = allocManager.Get(allocationID);
                DateTime allocStartDate = Convert.ToDateTime(resAllocation.AllocationStartDate).AddDays(-Convert.ToDateTime(resAllocation.AllocationStartDate).Day + 1);
                DateTime allocEndDate = Convert.ToDateTime(resAllocation.AllocationEndDate).AddDays(-Convert.ToDateTime(resAllocation.AllocationEndDate).Day + 1);


                string resourceID = resAllocation.Resource;

                while (tempDate <= endDate)
                {
                    DateTime tempEndDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                    double pctAlloction = 0;
                    ResourceDistributionItem dstItem = distributions.FirstOrDefault(x => x.Date == tempDate.Date);
                    if (dstItem != null)
                        pctAlloction = dstItem.PctAllocation;

                    int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(applicationContext, tempDate.Date, tempEndDate.Date);
                    int totalHrsInMonth = workindHrsInDay * workingDaysInMonth;
                    totalWorkingHrs += totalHrsInMonth;
                    double allocatedhrsInMonth = (pctAlloction * totalHrsInMonth) / 100;
                    allocatedHrs += allocatedhrsInMonth;
                    tempDate = tempDate.AddMonths(1);
                }


                if (Convert.ToDateTime(resAllocation.AllocationStartDate).Month != startDate.Month || Convert.ToDateTime(resAllocation.AllocationStartDate).Year != startDate.Year)
                    resAllocation.AllocationStartDate = startDate;

                if (Convert.ToDateTime(resAllocation.AllocationEndDate).Month != endDate.Month || Convert.ToDateTime(resAllocation.AllocationEndDate).Year != endDate.Year)
                    resAllocation.AllocationEndDate = endDate.AddDays(DateTime.DaysInMonth(endDate.Year, endDate.Month) - 1);
                resAllocation.PctAllocation = Math.Round((allocatedHrs * 100) / totalWorkingHrs, 2);
                string msg = allocManager.Save(resAllocation);
                if (!string.IsNullOrEmpty(msg))
                {
                    lbDistributionError.Text = msg;
                    return;
                }

                //if allocation pct is zero then save function do all the cleanup 
                if (resAllocation.PctAllocation <= 0)
                    return;

                RMMSummaryHelper.DistributeAllocationByMonth(applicationContext, resAllocation.ResourceWorkItems, distributions);

                if (resAllocation.ResourceWorkItemLookup > 0)
                {
                    //Start Thread to update rmm summary list w.r.t current workitem
                    ULog.WriteException("Method UpdateRMMAllocationSummary Called Inside Thread In Event btnSaveAllocationDistribution_Click on Page CustomResourceAllocation.ascx");
                    ThreadStart threadStartMethod = delegate ()
                    {
                        RMMSummaryHelper.UpdateRMMAllocationSummary(applicationContext, resAllocation.ResourceWorkItemLookup);
                    };
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }

                BindAllocation();
                gridAllocation.DataBind();

                if (rAllocationDistForm.Items.Count > 0)
                {
                    foreach (RepeaterItem item in rAllocationDistForm.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                ASPxLabel lb = (ASPxLabel)item.FindControl(string.Format("lb{0}", i));
                                ASPxTextBox txt = (ASPxTextBox)item.FindControl(string.Format("txt{0}", i));
                                lb.Visible = true;
                                txt.Visible = false;
                            }
                        }
                    }
                    btnSaveAllocationDistribution.Visible = false;
                    btnEditDistribution.Visible = true;
                }
            }
        }
        protected void btnEditDistribution_Click(object sender, EventArgs e)
        {
            btnSaveAllocationDistribution.Visible = true;
            btnEditDistribution.Visible = false;
            if (rAllocationDistForm.Items.Count > 0)
            {
                foreach (RepeaterItem item in rAllocationDistForm.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            ASPxLabel lb = (ASPxLabel)item.FindControl(string.Format("lb{0}", i));
                            ASPxTextBox txt = (ASPxTextBox)item.FindControl(string.Format("txt{0}", i));
                            lb.Visible = false;
                            txt.Visible = true;
                            txt.Width = new Unit(75, UnitType.Pixel);
                        }
                    }
                }
            }
        }
        #endregion
        protected void gridAllocation_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < gridAllocation.VisibleRowCount; i++)
            {
                string workItemType = Convert.ToString(gridAllocation.GetRowValues(i, DatabaseObjects.Columns.WorkItemType));
                if (!string.IsNullOrEmpty(workItemType))
                    list.Add((string)gridAllocation.GetRowValues(i, DatabaseObjects.Columns.WorkItemType));
            }
            e.Properties["cpWorkItemType"] = list;
        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {
            //lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) + 1);
            //UGITUtility.CreateCookie(Response, "SelectedYear", lblSelectedYear.Text);
            hndAllocation.Value = "ExpandAll";
        }

        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {
            //lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) - 1);
            //UGITUtility.CreateCookie(Response, "SelectedYear", lblSelectedYear.Text);
            hndAllocation.Value = "ExpandAll";
        }

        protected void gridAllocation_CustomGroupDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //if (e.Column.DataItemTemplate != null)
            //    return;
            //if (string.IsNullOrWhiteSpace(e.Column.FieldName))
            //    return;

            //try
            //{
            //    string values = Convert.ToString(e.GetFieldValue(e.Column.FieldName == DatabaseObjects.Columns.Resource ? DatabaseObjects.Columns.ResourceId: e.Column.FieldName));
            //    if (values != "00000000-0000-0000-0000-000000000000")
            //    {
            //        FieldConfigurationManager fmanger = new FieldConfigurationManager(applicationContext);
            //        if (fmanger.GetFieldByFieldName(e.Column.FieldName) != null)
            //        {
            //            string value = fmanger.GetFieldConfigurationData(e.Column.FieldName, values);
            //            e.DisplayText = value;
            //        }
            //    }
            //    else
            //    {
            //        e.DisplayText = "Unfilled Roles";
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected void gridAllocation_SelectionChanged(object sender, EventArgs e)
        {
            if (gridAllocation.Styles.FocusedRow.CssClass.Contains("dxgvDataRowAlt_UGITBlackDevEx"))
            {
                string cssclass = gridAllocation.Styles.FocusedRow.CssClass.Replace("ms-alternatingstrong", "");
                gridAllocation.Styles.FocusedRow.CssClass = cssclass + "ms-alternatingstrong";
            }
        }
        protected void gvFilteredList_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow rowView = (sender as ASPxGridView).GetDataRow(e.VisibleIndex);
            TableRow gvRow = e.Row;
            int dataSourceIndex = e.VisibleIndex;
            int pendingApprovalCount = resourceTimeSheetSignOffs.Where(x => x.Resource == Convert.ToString(rowView[DatabaseObjects.Columns.Id]) && x.SignOffStatus == Constants.PendingApproval).Count();
            string imageURL = "/Content/Images/userNew.png";
            List<UserProfile> selectUsers = new List<UserProfile>();
            if (gvRow != null && e.RowType == GridViewRowType.Data)
            {
                bool isSelected = false;
                UserProfile userProfile = ObjUserProfileManager.GetUserById(Convert.ToString(rowView[DatabaseObjects.Columns.Id]));
                imageURL = userProfile?.Picture ?? "/Content/Images/userNew.png";
                if (!string.IsNullOrWhiteSpace(UGITUtility.GetCookieValue(Request, "userall")))
                {
                    selectUsers = ObjUserProfileManager.GetUserInfosById(Server.UrlDecode(UGITUtility.GetCookieValue(Request, "userall")));
                }
                if (selectUsers.Exists(delegate (UserProfile ev) { return ev.Id.ToString() == Convert.ToString(rowView[DatabaseObjects.Columns.Id]); }))
                {
                    if (!selectedUsersList.Contains(userProfile))
                        selectedUsersList.Add(userProfile);
                    //gvRow.CssClass = "ugitsellinkbg";
                    gvRow.BackColor = System.Drawing.Color.AliceBlue;
                    isSelected = true;
                }
                if (selectedUsersList != null && selectedUsersList.Count > 0)
                {
                    if (selectedUsersList.Exists(delegate (UserProfile ev) { return ev.Id.ToString() == Convert.ToString(rowView[DatabaseObjects.Columns.Id]); }))
                    {
                        // gvRow.CssClass = "ugitsellinkbg";
                        gvRow.BackColor = System.Drawing.Color.AliceBlue;
                        isSelected = true;
                    }
                }
                gvRow.Cells[0].Text = string.Format("<input type='checkbox' id='{0}-{1}' {2} onclick='event.cancelBubble=true;CheckUser(\"{1}\", this.id, true)' class='usercheck11' name='userCheck'  />", e.VisibleIndex, rowView[DatabaseObjects.Columns.Id], isSelected ? "checked='checked'" : string.Empty);
                List<UsersEmail> userInfo = new List<UsersEmail>();

                List<string> toolTip = userInfo.Select(x => x.userToolTip).ToList();
                string sourceUrl = Request.Url.PathAndQuery;
                string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&RMMCardView=1", rowView[DatabaseObjects.Columns.Id]));

                string userName = Convert.ToString(rowView[DatabaseObjects.Columns.Title]);
                string usrLinkUr = string.Empty;
                if (!uHelper.IsCPRModuleEnabled(applicationContext))
                {
                    usrLinkUr = string.Format("<a title='{4}' id='{7}-{6}' class='jqtooltip' style='cursor:pointer;' onclick='event.cancelBubble=true;CheckUser(\"{6}\", this.id, false)'>{5}</a>{0}",
                                    string.Format("&nbsp;&nbsp;<image style=\"padding-right:4px; width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue-new.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + rowView[DatabaseObjects.Columns.Id] + "', '" + userName.Replace("'", "`") + "', '" + string.Empty + "')\"  />"),
                                    userName.Replace("'", string.Empty), userLinkUrl, Server.UrlEncode(Request.Url.AbsolutePath), string.Join(" ", toolTip).Replace("'", string.Empty), userName, rowView[DatabaseObjects.Columns.Id], e.VisibleIndex);
                }
                else
                {
                    usrLinkUr = string.Format("<img height='30' width='30' style='margin-right:3px;border-radius:50px;' src='{8}' /><a title='{4}' id='{7}-{6}' class='jqtooltip' style='cursor:pointer;' onclick='event.cancelBubble=true;CheckUser(\"{6}\", this.id, false)'>{5}</a>{0}",
                                    string.Format("&nbsp;&nbsp;<image style=\"padding-right:4px; width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue-new.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenMultiAllocationPopup('" + rowView[DatabaseObjects.Columns.Id] + "', '" + userName.Replace("'", "`") + "', '" + string.Empty + "')\"  />"),
                                    userName.Replace("'", string.Empty), userLinkUrl, Server.UrlEncode(Request.Url.AbsolutePath), string.Join(" ", toolTip).Replace("'", string.Empty), userName, rowView[DatabaseObjects.Columns.Id], e.VisibleIndex, imageURL);

                }
                string addDownLink = bool.Parse(rowView["IsAssistantExist"].ToString()) ? string.Format("&nbsp;<img style='width:16px;' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow_new.png' alt='Down' title='Move down'/>", rowView[DatabaseObjects.Columns.Id]) : "&nbsp;&nbsp;&nbsp;";
                usrLinkUr += addDownLink;
                UserProfile userCheck = ObjUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
                if (!ObjUserProfileManager.IsAdmin(userCheck) && !ObjUserProfileManager.IsResourceAdmin(userCheck))
                {
                    usrLinkUr = string.Format("<div title='{1}' class='jqtooltip' >{2}</a>{0}</div>",
                                                bool.Parse(rowView["IsAssistantExist"].ToString()) ? string.Format("&nbsp;<img style='width:16px;' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow_new.png' alt='Down' title='Move down'/>", rowView[DatabaseObjects.Columns.Id]) : "&nbsp;&nbsp;&nbsp;",
                                                 string.Join(" ", toolTip).Replace("'", string.Empty), userName);
                }
                IEnumerable<string> columns = gvFilteredList.Columns.Cast<GridViewDataColumn>().Select(x => x.Caption);
                var allocaionHead = columns.First(x => x == "Alloc%");
                if (allocaionHead != null)
                {
                    int colIndex = columns.ToList().IndexOf(allocaionHead);
                    string allocationBar = GetAllocatedPercentage(Convert.ToString(rowView[DatabaseObjects.Columns.Id]), Convert.ToString(lblSelectedYear.Text));
                    if (pendingApprovalCount == 0)
                        gvRow.Cells[0].Text = string.Format("{1}<span style='padding-left:10px;'>{0}</span>", usrLinkUr, gvRow.Cells[0].Text);
                    else
                        gvRow.Cells[0].Text = string.Format("{4}<span style='padding-left:10px;'>{0}</div><div onclick='event.cancelBubble=true;PendingTimesheetApprovals(\"{2}\",\"{3}\")' class='timeSheetPndAppvl-count' title='Timesheet Pending Approvals'>{1}</span>", usrLinkUr, pendingApprovalCount, Convert.ToString(rowView[DatabaseObjects.Columns.Id]), userName.Replace("'", string.Empty), gvRow.Cells[0].Text);
                    gvRow.Cells[colIndex].Text = string.Format("<div>{0}</div>", allocationBar);

                }
            }
        }

        protected void gvFilteredList_DataBound(object sender, EventArgs e)
        {
            Control ctrl = gvFilteredList.FindHeaderTemplateControl(gvFilteredList.Columns[0], "allCheck");
            if (ctrl != null)
            {
                if (hdnParentOf.Value.Trim() == string.Empty && hdnChildOf.Value.Trim() == string.Empty && !resourceChk.Checked)
                {
                    selectedUsersList.Clear();
                }

                //Fetch header checkbox and check it if all user are selected
                //ResourceListGrid.Attributes.Remove("class");
                // ResourceListGrid.Attributes.Remove("style");
                ResourceListGrid.Attributes.Add("class", "col-md-10 noSidePadding");
                CheckBox chBox = ctrl as CheckBox;
                chBox.Attributes.Add("onclick", " CheckUser('all', this.id, true)");
                if (gvFilteredList.DataSource != null)
                {
                    ResourceListGrid.Attributes.Remove("class");
                    ResourceListGrid.Attributes.Add("class", "col-md-10 col-sm-9 col-xs-12 noSidePadding");
                    //ResourceListGrid.Attributes.Add("style", "width:81%;");
                    DataTable table = (DataTable)gvFilteredList.DataSource;
                    bool isSelected = true;
                    List<UserProfile> selectedUser = new List<UserProfile>();
                    List<UserProfile> UserlistbyManager = new List<UserProfile>();

                    if (!string.IsNullOrWhiteSpace(UGITUtility.GetCookieValue(Request, "userall")))
                        selectedUser = ObjUserProfileManager.GetUserInfosById(HttpUtility.UrlDecode(UGITUtility.GetCookieValue(Request, "userall")));
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(cmbResourceManager.Value)) && cmbResourceManager.SelectedIndex > 0)
                        UserlistbyManager = ObjUserProfileManager.GetUserByManager(Server.UrlDecode(UGITUtility.GetCookieValue(Request, "selectedResource")));
                    if (!string.IsNullOrEmpty(hdnParentOf.Value))
                    {
                        UserProfile uProfile = ObjUserProfileManager.GetUserById(hdnParentOf.Value);
                        if (uProfile != null && UGITUtility.GetCookie(Request, "newresource") != null)
                        {
                            UGITUtility.GetCookie(Request, "newresource").Value = uProfile.ManagerID;
                        }
                    }
                    if ((!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "usermanagers")) && UGITUtility.GetCookieValue(Request, "usermanagers") == UGITUtility.GetCookieValue(Request, "newresource")))
                    {
                        selectedUsersList = selectedUser;
                        if (resourceChk.Checked)
                        {
                            string resourceManager = UGITUtility.GetCookieValue(Request, "usermanagers");
                            UserProfile userProfile = ObjUserProfileManager.GetUserById(resourceManager);
                            selectedUsersList.Add(userProfile);
                        }
                    }
                    if (resourceChk.Checked && this.ShowTimeOffOnly && Request["usermanagers"] != null && !string.IsNullOrWhiteSpace(Request["usermanagers"].ToString()))
                    {
                        string resourceManager = Request["usermanagers"].ToString();
                        UserProfile userProfile = ObjUserProfileManager.GetUserById(resourceManager);
                        selectedUsersList.Add(userProfile);
                    }
                    if (selectedUser.Count == 0 && UGITUtility.GetCookie(Request, "newresource") != null && resourceChk.Checked && !this.ShowTimeOffOnly)
                    {
                        selectedUsersList = ObjUserProfileManager.GetUserInfosById(UGITUtility.GetCookie(Request, "newresource").Value);
                    }
                    foreach (DataRow row in table.Rows)
                    {
                        if (!selectedUser.Exists(delegate (UserProfile ev) { return ev.Id == Convert.ToString(row[DatabaseObjects.Columns.Id]); }))
                        {
                            isSelected = false;
                            break;
                        }
                    }
                    if ((!IsPostBack || !string.IsNullOrWhiteSpace(UGITUtility.GetCookieValue(Request, "managerchanged"))) && managerFrom == null)
                    {
                        UGITUtility.DeleteCookie(Request, Response, "managerchanged");
                        foreach (DataRow row in table.Rows)
                        {

                            UserProfile userProfile = ObjUserProfileManager.GetUserById(row[DatabaseObjects.Columns.Id].ToString());
                            selectedUsersList.Add(userProfile);
                            if (!this.ShowTimeOffOnly)
                            {
                                resourceChk.Checked = false;
                                isSelected = true;
                            }
                        }
                    }
                    
                    if (isSelected)
                        chBox.Checked = true;
                }

            }
        }

        protected void LoadTileView()
        {
            CustomResourceAllocationCard customResourceAllocationCard = (CustomResourceAllocationCard)Page.LoadControl("~/CONTROLTEMPLATES/RMM/CustomResourceAllocationCard.ascx");
            customResourceAllocationCard.hdnChildOf = hdnChildOfVal;
            customResourceAllocationCard.hdnParentOf = hdnParentOfVal;
            customResourceAllocationCard.Height = this.Height;
            associateTileView.Controls.Add(customResourceAllocationCard);
        }

        protected void btnChangeView_Click(object sender, EventArgs e)
        {

        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void pcAllocationGantt_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblSelectedYear.Text))
            {
                startDateRange = new DateTime(UGITUtility.StringToInt(lblSelectedYear.Text.Trim()), 1, 1);
                endDateRange = new DateTime(UGITUtility.StringToInt(lblSelectedYear.Text.Trim()), 12, 31);
            }
            DataTable table = GetAllocationData(startDateRange, endDateRange);

            if (table != null && table.Rows.Count > 0 && chkIncludeClosed.Checked == false)
            {
                DataView view = table.AsDataView();
                view.RowFilter = $"{DatabaseObjects.Columns.Closed} = 'False' OR {DatabaseObjects.Columns.Closed} is NULL";
                table = view.ToTable();
            }

            allocationGantt.TasksDataSource = table;
            allocationGantt.ResourcesDataSource = table;
            allocationGantt.ResourceAssignmentsDataSource = table;

        }

        protected void imgOpenAddAllocation_Init(object sender, EventArgs e)
        {
            ASPxImage cb = sender as ASPxImage;
            if (allowAllocationForSelf.EqualsIgnoreCase("Edit") || HttpContext.Current.CurrentUser().IsManager || isResourceAdmin)
            {
                GridViewGroupRowTemplateContainer container = cb.NamingContainer as GridViewGroupRowTemplateContainer;
                DataRow currentRow = gridAllocation.GetDataRow(container.VisibleIndex);

                string userid = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.ResourceId]);
                string username = UGITUtility.ObjectToString(currentRow["ResourceUser"]);

                if (!string.IsNullOrEmpty(username))
                    username = username.Replace("'", "&#39;");

                string workitemtype = UGITUtility.ObjectToString(currentRow["WorkItemType"]);
                //cb.ClientSideEvents.Click = string.Format("function (s, e) {2}event.cancelBubble=true; OpenAddAllocationPopup('{0}','{1}','{4}'){3}", userid, username, "{", "}", workitemtype);
            }
            else
            {
                cb.Visible = false;
            }
        }

        protected void callbackControl_Callback(object source, CallbackEventArgs e)
        {
            try
            {
                uHelper._semaphore.WaitAsync();
                {
                    List<string> users = new List<string>();
                    long workItemID = 0;
                    long allocationID = UGITUtility.StringToLong(e.Parameter);
                    RResourceAllocation allocation = allocManager.LoadByID(allocationID);

                    if (allocation != null)
                    {
                        workItemID = allocation.ResourceWorkItemLookup;
                        ResourceWorkItemsManager ObjWorkItemsManager = new ResourceWorkItemsManager(applicationContext);
                        allocation.ResourceWorkItems = ObjWorkItemsManager.LoadByID(allocation.ResourceWorkItemLookup);
                        if (allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("PMM")
                            || allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("NPR")
                            || allocation.ResourceWorkItems.WorkItemType == uHelper.GetModuleTitle("TSK"))
                        {
                            RMMSummaryHelper.CleanAllocation(applicationContext, allocation.ResourceWorkItems, cleanEstimated: true, cleanPlanned: false);
                        }
                        else
                        {
                            allocManager.Delete(allocation);
                            allocManager.UpdateIntoCache(allocation, true);
                            if (!string.IsNullOrWhiteSpace(allocation.ProjectEstimatedAllocationId))
                            {
                                long id = UGITUtility.StringToLong(allocation.ProjectEstimatedAllocationId);
                                ProjectEstimatedAllocation projEstObj = projectEstimatedAllocationManager.LoadByID(id);
                                if (projEstObj != null)
                                {
                                    projectEstimatedAllocationManager.Delete(projEstObj);
                                }

                                UserProfile user = ObjUserProfileManager.GetUserById(allocation.Resource);
                                GlobalRole role = roleManager.Load(x => x.Id == allocation.RoleId)?.FirstOrDefault() ?? null;
                                if (user != null && role != null)
                                {
                                    string historyDesc = string.Format("Allocation removed for user: {0} - {1} {2}% {3}-{4}", user.Name, role.Name, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate));
                                    ResourceAllocationManager.UpdateHistory(applicationContext, historyDesc, allocation.TicketID);
                                    ULog.WriteLog("MV >> " + applicationContext.CurrentUser.Name + historyDesc);
                                }
                            }

                            users.Add(allocation.Resource);
                            //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                            ThreadStart threadStartMethod = delegate () { 
                                RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(applicationContext, workItemID); 
                                projectEstimatedAllocationManager.UpdateProjectGroups(uHelper.getModuleNameByTicketId(allocation.TicketID), allocation.TicketID); 
                                //projectEstimatedAllocationManager.RefreshProjectComplexity(uHelper.getModuleNameByTicketId(allocation.TicketID), users); 
                            };
                            Thread sThread = new Thread(threadStartMethod);
                            sThread.IsBackground = true;
                            sThread.Start();
                        }

                        if (!string.IsNullOrWhiteSpace(allocation.TicketID) && UGITUtility.IsValidTicketID(allocation.TicketID))
                        {
                            List<string> tagLookup = ObjUserProjectExperienceManager.GetProjectExperienceTags(allocation.TicketID, false)?.Select(x => x.TagId)?.ToList() ?? null;
                            ObjUserProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, allocation.TicketID);
                        }

                        // Rebind the details
                        BindAllocation();
                        gridAllocation.DataBind();

                        //hdnSelectedAllocation.Value = string.Empty;
                        //hdnSelectedWorkItem.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException("DeleteAllocations_semaphore Lock:" + ex.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnAllocationtimeline_Click(object sender, EventArgs e)
        {
            LoadAllocationTimeline();
        }

        private void LoadAllocationTimeline()
        {
            if (pnlAllocationTimeline.Controls.Count < 1)
            {
                ResourceAllocationGridNew _allocationGantt = Page.LoadControl("~/ControlTemplates/RMONE/ResourceAllocationGridNew.ascx") as ResourceAllocationGridNew;
                
                _allocationGantt.UserAll = UGITUtility.ObjectToString(Request["userall"]);
                //_allocationGantt.IncludeClosed = UGITUtility.ObjectToString(Request["includeClosed"]);
                if (selectedUsersList != null && selectedUsersList.Count > 0)
                {
                    List<string> userids = selectedUsersList.Select(x => x.Id).ToList();
                    if (!resourceChk.Checked)
                    {
                        string managerId = UGITUtility.ObjectToString(cmbResourceManager.SelectedItem?.Value);
                        userids.Remove(managerId);
                    }
                    _allocationGantt.SelectedUsers = UGITUtility.ConvertListToString(userids, Constants.Separator6);
                    _allocationGantt.SelectedUser = _allocationGantt.SelectedUsers;
                }
                else if (!string.IsNullOrEmpty(selectedUsersId))
                    _allocationGantt.SelectedUser = selectedUsersId;
                else
                    _allocationGantt.SelectedUser = UGITUtility.ObjectToString(cmbResourceManager.SelectedItem?.Value);

                //if(resourceChk.Checked)
                //{
                //    _allocationGantt.SelectedUser += Constants.Separator6 + UGITUtility.ObjectToString(Request["usermanagers"]);
                //}

                UGITUtility.CreateCookie(Response, "SelectedUser", _allocationGantt.SelectedUser);
                _allocationGantt.SelectedYear = UGITUtility.ObjectToString(Request["selectedYear"]);
                _allocationGantt.Height = this.Height;
                pnlAllocationTimeline.Controls.Add(_allocationGantt);
            }
                pnlAllocationTimeline.ClientVisible = true;
                //btnChangeView.ImageUrl = "/content/Images/cardViewBlack-new.png";
                //btnChangeView.ToolTip = "Show Card View";
                //ResourceListGrid.Style.Add("display", "none");
                //dvAssociateCardView.Style.Add("display", "none");
                //ResourceListTD.Style.Add("display", "none");
                ////btnExpandAll.Visible = true;
                ////btnCollapseAll.Visible = true;
                //btCollapseAllAllocation.Style.Add("display", "none");
                //btExpandAllAllocation.Style.Add("display", "none");

                //btnAllocationtimeline.ImageUrl = "/content/Images/gridBlackNew.png";
                //btnAllocationtimeline.ToolTip = "Show Grid View";
            
        }
        private void LoadTimeOffCardView()
        {
            TimeOffCardView timeOffCardView = Page.LoadControl("~/ControlTemplates/RMONE/TimeOffCardView.ascx") as TimeOffCardView;
            timeOffCardView.TimeOffAllocationData = TimeOffAllocatin;
            pnlTimeOffCardView.Controls.Add(timeOffCardView);
        }
        private void LoadGridView()
        {
            btnAllocationtimeline.ClientVisible = false;
            btnAllocationtimeline.ToolTip = "Show Gantt View";
            btnAllocationtimeline.ImageUrl = "/Content/Images/ganttBlackNew.png";
            dvAssociateCardView.Style.Add("display", "none");
            ResourceListGrid.Style.Add("display", "block");
            ResourceListTD.Style.Add("display", "block");
            pnlAllocationTimeline.ClientVisible = false;
            //UGITUtility.CreateCookie(Response, "activeview", "grid");
            UGITUtility.CreateCookie(Response, "activeview", "cards");
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
            hndAllocation.Value = "ExpandAll";
            gridAllocation.ExpandAll();
        }
    }

}
