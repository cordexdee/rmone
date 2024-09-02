using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Helpers;
using System.Text;
using System.Web.Script.Serialization;
using uGovernIT.Manager.Helper;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class CustomResourceTimeSheet : UserControl
    {
        public string FrameId { get; set; }
        public string ResourceId { get; set; }
        public string WeekStartDt { get; set; }

        public int signOffItemId = 0;
        private UserProfile currentSelectedUser = null;
        private DateTime startDate = DateTime.MaxValue;
        protected string viewTicketsPath = string.Empty;
        private bool allowEditing;
        string resourceFromCookies = "selectedResource";
        bool isSuperAdmin;
        protected string ajaxPageURL;
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&StartDate={4}&WorkItemType={5}&WorkItem={6}";
        private const string rustColorCode = "#d96125";
        private string newParam = "addworkitem";
        private string formTitle = "Add Work Item";
        string tempType = string.Empty;
        string TimesheetMode = "";
        DateTime signOffWeekStartDate;
        public string signOffHistoryUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=resourcetimesheethistroy");

        bool isResourceAdmin;
        private bool isManager=false;

        string status = "";
        string tempWorkItem = string.Empty;
        string tempPrintType = string.Empty;
        string tempPrintWorkItem = string.Empty;
        string weekEndDays;
        bool isApprovedCalled;
        bool isSendForApprovalCalled;
        bool isReturnCalled;
        bool isSignOffCalled;
        private bool disableEmails= false;
        bool isTimeEntryCalled;
        bool addTimeEntry;
        DataTable spSignOffList;
        protected bool hideDeleteButton;
        private string timeSheetAbsoluteUrl = "";
        protected List<UserProfile> userEditPermisionList = null;
        List<TimeSheetStateMaintain> rTimeSheetState;
        bool collapseGroups = false;
        public UserProfile currentLoggedInuser = null;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public string currentUserID = HttpContext.Current.CurrentUser().Id;
        UserProfileManager objUserProfileManager = HttpContext.Current.GetUserManager();
        ConfigurationVariableManager objConfigurationVariableManager = null;
        ResourceWorkItemsManager ObjResourceWorkItemManager = null;
        ResourceTimeSheetManager ObjResourceTimeSheetManager = null;
        ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = null;
        ModuleViewManager moduleManager = null;

        DataTable dtResourceTimeSheet;
        Dictionary<string, object> values = new Dictionary<string, object>();
        bool enableProjStdWorkItems =false;

        protected override void OnInit(EventArgs e)
        {
            objConfigurationVariableManager = new ConfigurationVariableManager(context);
            ObjResourceWorkItemManager = new ResourceWorkItemsManager(context);
            ObjResourceTimeSheetManager = new ResourceTimeSheetManager(context);
            resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(context);
            moduleManager = new ModuleViewManager(context);

            weekEndDays = objConfigurationVariableManager.GetValue(ConfigConstants.WeekendDays);
            enableProjStdWorkItems = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);

            // Get list of users that report to logged-in user as manager or Dept/Functional Area Owner
            // userEditPermisionList = UserProfile.LoadAuthorizedUsers(false); // Don't include current user here!!
            userEditPermisionList = objUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id).ToList();

            if (!IsPostBack)
                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "rTimeSheetState");

            try
            {
                HttpCookie rTimeSheetStateCookie = UGITUtility.GetCookie(HttpContext.Current.Request, "rTimeSheetState");
                if (rTimeSheetStateCookie != null && rTimeSheetStateCookie.Value != null)
                {
                    string cookValues = Server.UrlDecode(rTimeSheetStateCookie.Value);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    rTimeSheetState = js.Deserialize<List<TimeSheetStateMaintain>>(cookValues);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            //// Config variable to decide whether to send emails or not.
            //disableEmails = uGITCache.GetConfigVariableValueAsBool(ConfigConstants.RMMTimesheetDisableEmails, SPContext.Current.Web);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the detailPage Url for the ugitModule
            UGITModule ugitModule = moduleManager.GetByName(ModuleNames.RMM);  // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, "CMDB");

            // UGITModule ugitModule = uGITCache.ModuleConfigCache.GetCachedModule(spweb, ModuleNames.RMM);
            if (ugitModule != null)
                timeSheetAbsoluteUrl = ugitModule.StaticModulePagePath;

            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/rmmapi/");
            //Checks current user is in super admin group or not
            isSuperAdmin = objUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser());
            isResourceAdmin = objUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            currentLoggedInuser = objUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);

            TimesheetMode = objConfigurationVariableManager.GetValue(ConfigConstants.TimesheetMode).Replace(" ", string.Empty).ToLower();
            spSignOffList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheetSignOff, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            lbMessage.Text = string.Empty;
            // Get filtered tickets page url
            ConfigurationVariable callLogVariable = objConfigurationVariableManager.LoadVaribale("FilterTicketsPageUrl");
            if (callLogVariable != null)
            {
                viewTicketsPath = UGITUtility.GetAbsoluteURL(callLogVariable.KeyValue.Trim());
            }

            if (cmbCurrentUser.Items.Count > 0 && cmbCurrentUser.SelectedItem != null)
            {
                currentSelectedUser = objUserProfileManager.LoadById(Convert.ToString(cmbCurrentUser.SelectedItem.Value));
            }

            // Create Control to add in popup
            cmbCurrentUser_Load(cmbCurrentUser, null);
            List<UserProfile> selectedUsers = new List<UserProfile>();
            if (!string.IsNullOrEmpty(Convert.ToString(cmbCurrentUser.Value)))
            {
                //List<string> selecteduserIds = UGITUtility.ConvertStringToList(Convert.ToString(cmbCurrentUser.Value), new string[] { "," }, true);
                List<string> selecteduserIds = UGITUtility.ConvertStringToList(Convert.ToString(cmbCurrentUser.Value), Constants.Separator6);
                foreach (var item in selecteduserIds)
                {
                    selectedUsers.Add(objUserProfileManager.GetUserProfile(item));
                }
                //selectedUsers = objUserProfileManager.GetUsersProfile();
            }

            AddWorkItems addWorkItems = (AddWorkItems)Page.LoadControl("~/CONTROLTEMPLATES/RMM/AddWorkItems.ascx");
            addWorkItems.Type = "Actuals";
            addWorkItems.ID = this.ID + "callBackAllocation";
            addWorkItems.StartDate = startDate;
            addWorkItems.SelectedUsersListString = Convert.ToString(cmbCurrentUser.Value);
            addWorkItems.SelectedUsersList = selectedUsers;
            addWorkItems.IsInPopupCallback = true;
            popAllocation.Controls.Add(addWorkItems);
        }

        protected void SetSelectedUser()
        {
            if (IsPostBack)
            {
                if (cmbCurrentUser.Items.Count > 0 && cmbCurrentUser.SelectedItem != null)
                    currentSelectedUser = objUserProfileManager.LoadById(Convert.ToString(cmbCurrentUser.Value));

                //Set currentSelectedUser if user has selected a row from status grid to load the time sheet of selected user
                else if (Request.Form["__CALLBACKPARAM"] != null)
                {
                    string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (Request.Form["__CALLBACKPARAM"].Contains("ReloadTimeSheet"))
                    {
                        string selectedUserId = val.AsEnumerable().FirstOrDefault(x => x.StartsWith("ReloadTimeSheet")).Split(':')[1];
                        currentSelectedUser = objUserProfileManager.LoadById(selectedUserId);
                    }
                }
                else
                    currentSelectedUser = objUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
            }
            else
            {
                //Check whether cookies has selected resource id or not
                string selectedUserID = HttpContext.Current.CurrentUser().Id;
                string resource = UGITUtility.GetCookieValue(Request, resourceFromCookies).Trim();

                if (ResourceId != null)
                {
                    selectedUserID = ResourceId;
                }
                else if (!string.IsNullOrEmpty(resource) && cmbCurrentUser.Items.FindByValue(resource) != null)
                {
                    //int resourceID = 0;
                    //int.TryParse(resource, out resourceID);
                    if (!string.IsNullOrEmpty(resource))
                    {
                        selectedUserID = resource;
                    }
                }
                currentSelectedUser = objUserProfileManager.LoadById(selectedUserID);
            }
            lblRescourceManagerName.Text = currentSelectedUser.Name;
        }

        protected void DetailViewPanel_Load(object sender, EventArgs e)
        {

        }

        private DataTable GetTimeSheet()
        {
            allowEditing = true;
            DayOfWeek selectedStartDay = dtcStartdate.SelectedDate.DayOfWeek;
            int diff = selectedStartDay - DayOfWeek.Monday;
            int pastWeeksAllowed = 0;
            int.TryParse(objConfigurationVariableManager.GetValue("RMMPastWeeksAllowed"), out pastWeeksAllowed);
            int futureWeeksAllowed = 0;
            int.TryParse(objConfigurationVariableManager.GetValue("RMMFutureWeeksAllowed"), out futureWeeksAllowed);

            if ((pastWeeksAllowed > 0 && (startDate <= DateTime.Now.AddDays(-(pastWeeksAllowed * 7) - diff)))
               || (futureWeeksAllowed > 0 && (startDate >= DateTime.Now.AddDays((futureWeeksAllowed * 7) + diff))))
            {
                allowEditing = false;
            }

            if (currentSelectedUser == null)
                return null;

            return ObjResourceTimeSheetManager.LoadWorkSheetByDate(currentSelectedUser.Id, startDate);
        }

        private void BindTimeSheetDetails()
        {
            DataTable timeSheet = GetTimeSheet();

            //collapse all child elements at project level
            if ((!IsPostBack || collapseGroups) && timeSheet != null)
            {
                DataRow[] totalRows = timeSheet.Select(string.Format("{0} = 'Total'", DatabaseObjects.Columns.SubWorkItem));

                rTimeSheetState = new List<TimeSheetStateMaintain>();
                foreach (DataRow row in totalRows)
                    rTimeSheetState.Add(new TimeSheetStateMaintain() { user = Convert.ToString(cmbCurrentUser.Value), workItem = "Child_" + Convert.ToString(row[DatabaseObjects.Columns.WorkItem]) });

                if (rTimeSheetState != null && rTimeSheetState.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonVal = js.Serialize(rTimeSheetState);
                    UGITUtility.CreateCookie(Response, "rTimeSheetState", jsonVal);
                }
            }

            //Remove those rows that has total hour 0;
            DataTable timeSheetPrint = timeSheet;
            if (timeSheet != null && timeSheet.Rows.Count > 0)
            {
                DataRow[] drs = timeSheet.AsEnumerable().Where(x => (x.Field<double>("WeekDay1") + x.Field<double>("WeekDay2") + x.Field<double>("WeekDay3") + x.Field<double>("WeekDay4") + x.Field<double>("WeekDay5") + x.Field<double>("WeekDay6") + x.Field<double>("WeekDay7")) > 0).ToArray();
                if (drs != null && drs.Length > 0)
                {
                    timeSheetPrint = drs.CopyToDataTable();
                }
                else
                {
                    timeSheetPrint = timeSheet.Clone();
                }
            }

            timeSheetActions.Visible = (allowEditing && timeSheet != null && timeSheet.Rows.Count > 0);

            if (!allowEditing)
                rTimeSheet.EditIndex = -1;

            aAddItem.Visible = allowEditing;
            btCopyTimeSheet.Visible = allowEditing;

            if (timeSheet != null && timeSheet.Rows.Count > 0)
            {
                // Hide delete icon and add New WorkItem button if the Time Sheet is locked for this user
                //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ShowDeleteButton, timeSheet) && (hideDeleteButton || !allowEditing))
                //{
                //    timeSheet.AsEnumerable().ToList().ForEach(x =>
                //    {
                //        x[DatabaseObjects.Columns.ShowDeleteButton] = false;
                //    });
                //    aAddItem.Visible = false;
                //    btCopyTimeSheet.Visible = false;
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), Constants.CallLockEditing, Constants.FunctionLockEditing, true);
                //}

                // IF we have data, sort it and label the columns correctly
                if (uHelper.IfColumnExists(DatabaseObjects.Columns.ItemOrder, timeSheet))
                    timeSheet.DefaultView.Sort = "Type, WorkItem, ItemOrder, SubWorkItem, SubSubWorkItem";
                else
                    timeSheet.DefaultView.Sort = "Type, WorkItem, SubWorkItem, SubSubWorkItem";
                if (rTimeSheet.DataSource == null)
                {
                    rTimeSheet.DataSource = timeSheet.DefaultView;
                    rTimeSheet.DataBind();
                }
                dtResourceTimeSheet = timeSheet;

                ((LinkButton)rTimeSheet.FindControl("lbType")).Text = objConfigurationVariableManager.GetValue("RMMLevel1Name");
                ((LinkButton)rTimeSheet.FindControl("lbWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel2Name");
                ((LinkButton)rTimeSheet.FindControl("lbSubWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel3Name");
                ((LinkButton)rTimeSheet.FindControl("lbSubSubWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel4Name");

                HtmlTableCell subWorkItemHead = (HtmlTableCell)rTimeSheet.FindControl("subWorkItemHead");

                //HtmlControl trSubWorkItemTotal1 = (HtmlControl)rTimeSheet.FindControl("trSubWorkItemTotal1");
                //HtmlControl trSubWorkItemTotal2 = (HtmlControl)rTimeSheet.FindControl("trSubWorkItemTotal2");
                //HtmlTableCell subWorkItemTotal1 = (HtmlTableCell)rTimeSheet.FindControl("subWorkItemTotal1");
                //HtmlTableCell subWorkItemTotal2 = (HtmlTableCell)rTimeSheet.FindControl("subWorkItemTotal2");
                HtmlTableCell subWorkItemTotal3 = (HtmlTableCell)rTimeSheet.FindControl("subWorkItemTotal3");
                HtmlTableCell subWorkItemEdit = (HtmlTableCell)rTimeSheet.FindControl("subWorkItemEdit");

                DataRow[] rows = timeSheet.Select(string.Format("{0} <> '' and {0} is not null and {0} <> '0'", DatabaseObjects.Columns.SubWorkItem));
                if (rows.Length > 0)
                {
                    if (subWorkItemHead != null)
                        subWorkItemHead.Visible = true;

                    // Doesn't seem to be used anymore, so keep hidden to avoid empty rows
                    //if (trSubWorkItemTotal1 != null)
                    //    trSubWorkItemTotal1.Visible = true;
                    //if (subWorkItemTotal1 != null)
                    //    subWorkItemTotal1.Visible = true;
                    //if (trSubWorkItemTotal2 != null)
                    //    trSubWorkItemTotal2.Visible = true;
                    //if (subWorkItemTotal2 != null)
                    //    subWorkItemTotal2.Visible = true;

                    if (subWorkItemTotal3 != null)
                        subWorkItemTotal3.Visible = true;
                    if (subWorkItemEdit != null)
                        subWorkItemEdit.Visible = true;

                    foreach (ListViewDataItem dItem in rTimeSheet.Items)
                    {
                        HtmlTableCell subWorkItemItem = (HtmlTableCell)dItem.FindControl("subWorkItemItem");
                        if (subWorkItemItem != null)
                            subWorkItemItem.Visible = true;
                    }
                }
                else
                {
                    if (subWorkItemHead != null)
                        subWorkItemHead.Visible = false;

                    //if (trSubWorkItemTotal1 != null)
                    //    trSubWorkItemTotal1.Visible = false;
                    //if (subWorkItemTotal1 != null)
                    //    subWorkItemTotal1.Visible = false;
                    //if (trSubWorkItemTotal2 != null)
                    //    trSubWorkItemTotal2.Visible = false;
                    //if (subWorkItemTotal2 != null)
                    //    subWorkItemTotal2.Visible = false;

                    if (subWorkItemTotal3 != null)
                        subWorkItemTotal3.Visible = false;
                    if (subWorkItemEdit != null)
                        subWorkItemEdit.Visible = false;

                    foreach (ListViewDataItem dItem in rTimeSheet.Items)
                    {
                        HtmlTableCell subWorkItemItem = (HtmlTableCell)dItem.FindControl("subWorkItemItem");
                        if (subWorkItemItem != null)
                            subWorkItemItem.Visible = false;
                    }
                }

                if (enableProjStdWorkItems)
                {
                    HtmlTableCell subSubWorkItemHead = (HtmlTableCell)rTimeSheet.FindControl("subSubWorkItemHead");
                    HtmlTableCell subSubWorkItemTotal = (HtmlTableCell)rTimeSheet.FindControl("subSubWorkItemTotal");

                    //if (subSubWorkItemTotal != null)
                    //    subSubWorkItemTotal.Visible = true;

                    rows = timeSheet.Select(string.Format("{0} <> '' and {0} is not null and {0} <> '0'", DatabaseObjects.Columns.SubSubWorkItem));
                    if (rows.Length > 0)
                    {
                        if (subSubWorkItemTotal != null)
                            subSubWorkItemTotal.Visible = true;

                        if (subSubWorkItemHead != null)
                            subSubWorkItemHead.Visible = true;

                        if (subWorkItemTotal3 != null)
                            subWorkItemTotal3.Visible = true;
                        if (subWorkItemEdit != null)
                            subWorkItemEdit.Visible = true;

                        foreach (ListViewDataItem dItem in rTimeSheet.Items)
                        {
                            HtmlTableCell subSubWorkItemItem = (HtmlTableCell)dItem.FindControl("subSubWorkItemItem");
                            if (subSubWorkItemItem != null)
                                subSubWorkItemItem.Visible = true;
                        }

                        subSubWorkItemTotal.InnerText = "Total";
                    }
                    else
                    {
                        foreach (ListViewDataItem dItem in rTimeSheet.Items)
                        {
                            HtmlTableCell subSubWorkItemItem = (HtmlTableCell)dItem.FindControl("subSubWorkItemItem");
                            if (subSubWorkItemItem != null)
                                subSubWorkItemItem.Visible = false;
                        }
                        subWorkItemTotal3.InnerText = "Total";
                    } 
                }
                else
                {
                    subWorkItemTotal3.InnerText = "Total";
                }
            }
            else
            {
                // else show empty timesheet
                rTimeSheet.DataSource = timeSheet;
                rTimeSheet.DataBind();
                dtResourceTimeSheet = timeSheet;
            }

            //if (timeSheet != null && timeSheet.Rows.Count > 0)
            //{
            //    // IF we have data, sort it and label the columns correctly
            //    timeSheet.DefaultView.Sort = "Type, WorkItem, SubWorkItem";
            //    rTimeSheet.DataSource = timeSheet.DefaultView;
            //    rTimeSheet.DataBind();

            //    ((LinkButton)rTimeSheet.FindControl("lbType")).Text = objConfigurationVariableManager.GetValue("RMMLevel1Name");
            //    ((LinkButton)rTimeSheet.FindControl("lbWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel2Name");
            //    ((LinkButton)rTimeSheet.FindControl("lbSubWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel3Name");
            //}
            //else
            //{
            //    // else show empty timesheet
            //    rTimeSheet.DataSource = timeSheet;
            //    rTimeSheet.DataBind();
            //}


            if (timeSheetPrint != null && timeSheetPrint.Rows.Count > 0)
            {
                if (uHelper.IfColumnExists(DatabaseObjects.Columns.ItemOrder, timeSheetPrint))
                    timeSheetPrint.DefaultView.Sort = "Type, WorkItem, ItemOrder, SubWorkItem, SubSubWorkItem";
                else
                    timeSheetPrint.DefaultView.Sort = "Type, WorkItem, SubWorkItem, SubSubWorkItem";

                ListViewPrint.DataSource = timeSheetPrint.DefaultView;
                ListViewPrint.DataBind();

                ((Label)ListViewPrint.FindControl("lbType")).Text = objConfigurationVariableManager.GetValue("RMMLevel1Name");
                ((Label)ListViewPrint.FindControl("lbWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel2Name");
                ((Label)ListViewPrint.FindControl("lbSubWorkItem")).Text = objConfigurationVariableManager.GetValue("RMMLevel3Name");

                HtmlTableCell subWorkItemHead_print = (HtmlTableCell)ListViewPrint.FindControl("subWorkItemHead");
                //HtmlControl trSubWorkItemTotal1_print = (HtmlControl)ListViewPrint.FindControl("trSubWorkItemTotal1");
                //HtmlControl trSubWorkItemTotal2_print = (HtmlControl)ListViewPrint.FindControl("trSubWorkItemTotal2");
                //HtmlTableCell subWorkItemTotal1_print = (HtmlTableCell)ListViewPrint.FindControl("subWorkItemTotal1");
                //HtmlTableCell subWorkItemTotal2_print = (HtmlTableCell)ListViewPrint.FindControl("subWorkItemTotal2");
                HtmlTableCell subWorkItemTotal3_print = (HtmlTableCell)ListViewPrint.FindControl("subWorkItemTotal3");
                HtmlTableCell subWorkItemEdit_print = (HtmlTableCell)ListViewPrint.FindControl("subWorkItemEdit");

                DataRow[] rows = timeSheet.Select(string.Format("{0} <> '' and {0} is not null and {0} <> '0'", DatabaseObjects.Columns.SubWorkItem));
                if (rows.Length > 0)
                {
                    if (subWorkItemHead_print != null)
                        subWorkItemHead_print.Visible = true;

                    //if (trSubWorkItemTotal1_print != null)
                    //    trSubWorkItemTotal1_print.Visible = true;
                    //if (subWorkItemTotal1_print != null)
                    //    subWorkItemTotal1_print.Visible = true;
                    //if (trSubWorkItemTotal2_print != null)
                    //    trSubWorkItemTotal2_print.Visible = true;
                    //if (subWorkItemTotal2_print != null)
                    //    subWorkItemTotal2_print.Visible = true;

                    if (subWorkItemTotal3_print != null)
                        subWorkItemTotal3_print.Visible = true;
                    if (subWorkItemEdit_print != null)
                        subWorkItemEdit_print.Visible = true;
                    foreach (ListViewDataItem dItem in ListViewPrint.Items)
                    {
                        HtmlTableCell subWorkItemItem = (HtmlTableCell)dItem.FindControl("subWorkItemItem");
                        if (subWorkItemItem != null)
                            subWorkItemItem.Visible = true;
                    }
                }
                else
                {
                    if (subWorkItemHead_print != null)
                        subWorkItemHead_print.Visible = false;

                    //if (trSubWorkItemTotal1_print != null)
                    //    trSubWorkItemTotal1_print.Visible = false;
                    //if (subWorkItemTotal1_print != null)
                    //    subWorkItemTotal1_print.Visible = false;
                    //if (trSubWorkItemTotal2_print != null)
                    //    trSubWorkItemTotal2_print.Visible = false;
                    //if (subWorkItemTotal2_print != null)
                    //    subWorkItemTotal2_print.Visible = false;

                    if (subWorkItemTotal3_print != null)
                        subWorkItemTotal3_print.Visible = false;
                    if (subWorkItemEdit_print != null)
                        subWorkItemEdit_print.Visible = false;
                    foreach (ListViewDataItem dItem in ListViewPrint.Items)
                    {
                        HtmlTableCell subWorkItemItem = (HtmlTableCell)dItem.FindControl("subWorkItemItem");
                        if (subWorkItemItem != null)
                            subWorkItemItem.Visible = false;
                    }
                }
            }
            else
            {
                // else show empty timesheet
                ListViewPrint.DataSource = timeSheetPrint;
                ListViewPrint.DataBind();
            }


            // MUST be after rTimeSheet.DataBind
            ShowDataDynamically(timeSheet);
            ShowPrintDataDynamically(timeSheet);
        }

        protected void PreviousWeek_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedStartDay = dtcStartdate.SelectedDate.DayOfWeek;
            int diff = selectedStartDay - DayOfWeek.Monday;
            DateTime firstDate = dtcStartdate.SelectedDate.AddDays(-diff);
            startDate = firstDate.AddDays(-7);
            int pastWeeksAllowed = int.Parse(objConfigurationVariableManager.GetValue("RMMPastWeeksAllowed"));
            if (startDate <= DateTime.Now.AddDays(-(pastWeeksAllowed * 7) - diff))
                startDate = firstDate;
            dtcStartdate.SelectedDate = startDate;
            StartWeekDate.Value = dtcStartdate.SelectedDate.ToString();
            lbWeekDuration.Text = string.Format("{0}-{1}", startDate.ToString("MMM dd,yyyy"), startDate.AddDays(6).ToString("MMM dd,yyyy"));
            startWeekDateForEdit.Value = startDate.ToString("MMM dd yyyy");
            rTimeSheet.EditIndex = -1;
            BindTimeSheetDetails();

            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("startDate", startDate.ToString("MM dd yyyy"));
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void NextWeek_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedStartDay = dtcStartdate.SelectedDate.DayOfWeek;
            int diff = selectedStartDay - DayOfWeek.Monday;
            DateTime firstDate = dtcStartdate.SelectedDate.AddDays(-diff);
            startDate = firstDate.AddDays(7);
            int futureWeeksAllowed = int.Parse(objConfigurationVariableManager.GetValue("RMMFutureWeeksAllowed"));
            if (startDate >= DateTime.Now.AddDays((futureWeeksAllowed * 7) + diff))
                startDate = firstDate;
            dtcStartdate.SelectedDate = startDate;
            StartWeekDate.Value = dtcStartdate.SelectedDate.ToString();
            lbWeekDuration.Text = string.Format("{0}-{1}", startDate.ToString("MMM dd,yyyy"), startDate.AddDays(6).ToString("MMM dd,yyyy"));
            startWeekDateForEdit.Value = startDate.ToString("MMM dd yyyy");
            rTimeSheet.EditIndex = -1;
            BindTimeSheetDetails();

            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("startDate", startDate.ToString("MM dd yyyy"));
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        private void ShowDataDynamically(DataTable timeSheet)
        {
            DateTime weekDate = startDate;
            double total = 0;
            for (int i = 1; i <= 7; i++)
            {
                LinkButton headerLink = (LinkButton)rTimeSheet.FindControl(string.Format("lbWeekDay{0}", i));
                if (headerLink != null)
                {
                    //highlight current date header with red border
                    if (weekDate.Date == DateTime.Today && headerLink.CssClass.IndexOf("clshighlightheadercell") == -1)
                        headerLink.CssClass += " clshighlightheadercell";

                    //highlight weekend header
                    string dayOfWeek = weekDate.DayOfWeek.ToString();
                    if (!string.IsNullOrEmpty(weekEndDays) && weekEndDays.IndexOf(dayOfWeek) != -1)
                    {
                        headerLink.CssClass += " clshighlightweekendheader";
                        headerLink.Attributes.Add("color", "white !important");
                    }

                    headerLink.Text = weekDate.ToString("ddd dd");
                }
                Label lbTotal = (Label)rTimeSheet.FindControl(string.Format("lbWeekDay{0}VTotal", i));

                if (lbTotal != null)
                {
                    //double sum = timeSheet.AsEnumerable().Sum(x => x.Field<double>("WeekDay" + i));
                    double sum = timeSheet.AsEnumerable().Where(y => y.Field<string>(DatabaseObjects.Columns.SubWorkItem) != "Total").Sum(x => x.Field<double>("WeekDay" + i));
                    lbTotal.Text = sum.ToString();
                    lbTotal.Attributes.Add("oldTotal", sum.ToString());
                    total += sum;
                }
                weekDate = weekDate.AddDays(1);
            }
            Label lbVTotal = (Label)rTimeSheet.FindControl("lbVTotal");
            if (lbVTotal != null)
                lbVTotal.Text = total.ToString();
        }

        private void ShowPrintDataDynamically(DataTable timeSheet)
        {
            DateTime weekDate = startDate;
            double total = 0;
            for (int i = 1; i <= 7; i++)
            {
                // LinkButton headerLink = (LinkButton)ListViewPrint.FindControl(string.Format("lbWeekDay{0}", i));
                Label headerLabel = (Label)ListViewPrint.FindControl(string.Format("lbWeekDay{0}", i));

                if (headerLabel != null)
                {
                    headerLabel.Text = weekDate.ToString("ddd dd");
                }

                Label lbTotal = (Label)ListViewPrint.FindControl(string.Format("lbWeekDay{0}VTotal", i));

                if (lbTotal != null)
                {
                    double sum = timeSheet.AsEnumerable().Where(y => y.Field<string>(DatabaseObjects.Columns.SubWorkItem) != "Total").Sum(x => x.Field<double>("WeekDay" + i));
                    lbTotal.Text = sum.ToString();
                    lbTotal.Attributes.Add("oldTotal", sum.ToString());
                    total += sum;
                }
                weekDate = weekDate.AddDays(1);
            }
            Label lbVTotal = (Label)ListViewPrint.FindControl("lbVTotal");
            if (lbVTotal != null)
                lbVTotal.Text = total.ToString();
        }

        protected void RTimeSheet_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            if (rTimeSheet.EditIndex >= 0)
            {
                UpdateTimeSheet();
            }

            rTimeSheet.EditIndex = e.NewEditIndex;
            BindTimeSheetDetails();
        }

        protected void RTimeSheet_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = rTimeSheet.Items[e.ItemIndex];
            UpdateTimeSheet();
            BindTimeSheetDetails();
        }

        private bool UpdateTimeSheet()
        {
            if (currentSelectedUser != null)
            {
                int workItemId = (int)rTimeSheet.DataKeys[rTimeSheet.EditIndex].Value;
                DateTime sDate = startDate;
                if (ValidateTimeSheet(rTimeSheet.EditItem, workItemId))
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        TextBox txtWeekDay = (TextBox)rTimeSheet.EditItem.FindControl("txtWeekDay" + i);

                        double hours = 0;
                        try
                        {
                            hours = double.Parse(txtWeekDay.Text.Trim());
                            int wholeHours = (int)((hours * 60) / 60);
                            int remMin = (int)((hours * 60) % 60);
                            if (remMin > 45)
                            {
                                remMin = 60;
                            }
                            else if (remMin > 30)
                            {
                                remMin = 45;
                            }
                            else if (remMin > 15)
                            {
                                remMin = 30;
                            }
                            else if (remMin > 0)
                            {
                                remMin = 15;
                            }

                            double actualHours = Math.Round((double)(((double)(wholeHours * 60) + remMin) / 60), 2);
                            ResourceTimeSheet work = new ResourceTimeSheet(currentSelectedUser.Id, sDate);
                            work.HoursTaken = Convert.ToInt32(actualHours);
                            work.ResourceWorkItem = ObjResourceWorkItemManager.LoadByID(workItemId);
                            ObjResourceTimeSheetManager.Save(work);
                        }
                        catch { }
                        sDate = sDate.AddDays(1);
                    }
                    rTimeSheet.EditIndex = -1;
                }
            }

            return true;
        }

        private bool ValidateTimeSheet(ListViewItem item, int workItemID)
        {
            bool valid = true;
            string message = string.Empty;
            DataTable timeSheetTable = ObjResourceTimeSheetManager.Load(currentSelectedUser.Id.ToString(), startDate, startDate.AddDays(6));
            DateTime startDateTemp = startDate;
            if (timeSheetTable != null)
            {
                for (int i = 1; i <= 7; i++)
                {
                    TextBox txtWeekDay = (TextBox)rTimeSheet.EditItem.FindControl("txtWeekDay" + i);
                    DataRow[] selectedDateSheet = timeSheetTable.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == startDateTemp.Date && x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) != workItemID.ToString()).ToArray();
                    double hours = 0;
                    try
                    {
                        hours = double.Parse(txtWeekDay.Text.Trim());
                        double minutes = selectedDateSheet.Sum(x => x.Field<double>(DatabaseObjects.Columns.HoursTaken) * 60);
                        minutes += (hours * 60);
                        hours = minutes / 60;
                        if (hours > 24)
                        {
                            valid = false;
                            message = "You cannot add more than 24 hours in a day";
                            break;
                        }
                    }
                    catch
                    {
                        //Invalid non integer entry
                    }
                    startDateTemp = startDateTemp.AddDays(1);
                }
            }

            if (!valid)
            {
                lbMessage.Text = message;
                lbMessage.ForeColor = System.Drawing.Color.Red;
            }
            return valid;
        }

        protected void RTimeSheet_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            rTimeSheet.EditIndex = -1;
            BindTimeSheetDetails();
        }

        protected void RTimeSheet_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem item = rTimeSheet.Items[e.ItemIndex];
            //ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true);
            int workItemId = (int)rTimeSheet.DataKeys[e.ItemIndex].Value;
            ResourceWorkItems workItem = null;
            //var value= hdnworkitemtype.Value;
            TicketHoursManager tHelper = new TicketHoursManager(context);

            HiddenField hdnworkitemtype = item.FindControl("hdnworkitemtype") as HiddenField;
            HiddenField hdnworkitemname = item.FindControl("hdnworkitemname") as HiddenField;
            HiddenField hdnsubworkitem = item.FindControl("hdnsubworkitem") as HiddenField;
            HiddenField hdnsubSubworkitem = item.FindControl("hdnsubSubworkitem") as HiddenField;
            HtmlTableRow workSheetRow = item.FindControl("workSheetRow") as HtmlTableRow;
            HiddenField tr = workSheetRow.FindControl("hdnworkitemtype") as HiddenField;
            int taskId = 0;

            if (workItemId == 0)
            {
                if (!string.IsNullOrWhiteSpace(hdnworkitemtype.Value) && !string.IsNullOrWhiteSpace(hdnworkitemname.Value) && !string.IsNullOrWhiteSpace(hdnsubworkitem.Value))
                {
                    if (!string.IsNullOrWhiteSpace(hdnsubworkitem.Value))
                    {
                        string[] entries = UGITUtility.SplitString(hdnsubworkitem.Value, Constants.Separator);
                        string[] subsubWorkItemEntries = UGITUtility.SplitString(hdnsubSubworkitem.Value, Constants.Separator);
                        string subWorkItem = entries.ElementAtOrDefault(1) != null && entries.ElementAtOrDefault(2) != null ? $"{entries[1]};#{entries[2]}" : entries[0];
                        string subsubWorkItem = subsubWorkItemEntries.ElementAtOrDefault(1) != null && subsubWorkItemEntries.ElementAtOrDefault(2) != null ? $"{subsubWorkItemEntries[1]};#{subsubWorkItemEntries[2]}" : subsubWorkItemEntries[0];
                        workItem = ObjResourceWorkItemManager.LoadByWorkItem(currentSelectedUser.Id, hdnworkitemtype.Value, hdnworkitemname.Value, subWorkItem, subsubWorkItem, Convert.ToString(startDate), Convert.ToString(startDate.AddDays(6)));
                    }
                    else
                        workItem = ObjResourceWorkItemManager.LoadByWorkItem(currentSelectedUser.Id, hdnworkitemtype.Value, hdnworkitemname.Value, string.Empty, Convert.ToString(startDate), Convert.ToString(startDate.AddDays(6)));
                }

                if (workItem != null)
                {
                    if(enableProjStdWorkItems)
                    {
                        workItem.StartDate = startDate;
                        workItem.EndDate = startDate.AddDays(6);
                        workItem.Deleted = true;
                        ObjResourceWorkItemManager.Save(workItem);
                    }
                    else 
                    { 
                        //Delete task based entry from ticket hours for selected week
                        //SPFieldLookupValue tLookup = new SPFieldLookupValue(hdnsubworkitem.Value);

                        string SubWorkItem = hdnsubworkitem.Value;
                        if (!string.IsNullOrEmpty(SubWorkItem))
                        //  continue;
                        {
                            string dataseprator = ";#";
                            string[] entries = UGITUtility.SplitString(SubWorkItem, dataseprator);
                            string subWorkItemId = entries[0];
                            taskId = Convert.ToInt32(subWorkItemId);
                            string subWorkItemTitle = entries[1];
                        }

                        if (SubWorkItem != null && taskId > 0)
                        {
                            tHelper.DeleteWeekEntries(context, hdnworkitemtype.Value, hdnworkitemname.Value, currentSelectedUser.Id, taskId, startDate);
                        }
                    }
                }
            }
            else
            {
                // Remove all childs entry for current week from Ticket Hours list and update RMMSummary list in thread for current sheet entries
                if ((hdnworkitemtype.Value == "PMM" || hdnworkitemtype.Value == "TSK" || hdnworkitemtype.Value == "NPR")
                    && hdnsubworkitem.Value.Contains(Constants.Separator))
                {
                    // SPFieldLookupValueCollection subWorkItemValues = new SPFieldLookupValueCollection(hdnsubworkitem.Value);
                    string subWorkItemValues = hdnsubworkitem.Value;
                    string dataseprator = ";#";
                    string[] subWorkItemIds = UGITUtility.SplitString(subWorkItemValues, dataseprator);
                    string subWorkItemId = subWorkItemIds[0];
                    taskId = Convert.ToInt32(subWorkItemId);

                    //string[] subWorkItemIds = subWorkItemValues.Split();
                    if (subWorkItemIds.Count() > 0)
                    {
                        //TicketHourHelper tHelper = new TicketHourHelper(contxt);
                        tHelper.DeleteWeekEntries(context, hdnworkitemtype.Value, hdnworkitemname.Value, currentSelectedUser.Id, taskId, startDate);
                    }
                }

                workItem = ObjResourceWorkItemManager.LoadByID(workItemId);

                if (workItem != null)
                {
                    //Set the current week range shown at front end.
                    workItem.StartDate = startDate;
                    workItem.EndDate = startDate.AddDays(6);
                    workItem.Deleted = true;
                    ObjResourceWorkItemManager.Save(workItem);
                }
            }

            #region old code for Delete
            //ResourceWorkItems workItem = ObjResourceWorkItemManager.LoadByID(workItemId);
            //if (workItem != null)
            //{
            //    //Set the current week range shown at front end.
            //    workItem.StartDate = startDate;
            //    workItem.EndDate = startDate.AddDays(6);
            //    workItem.Deleted = true;
            //  ObjResourceWorkItemManager.Save(workItem);
            //}
            #endregion End of Old Code
            List<long> changedWorkItemsID = new List<long>();
            changedWorkItemsID.Add(workItemId);

            if (changedWorkItemsID.Count > 0)
            {
                string resourceID = currentSelectedUser.Id;
                string webUrl = HttpContext.Current.Request.Url.ToString();
                //Start Thread to update rmm summary list for current sheet entries
                ThreadStart threadStartMethod = delegate ()
                {
                    RMMSummaryHelper.UpdateActualInRMMSummary(context, changedWorkItemsID, resourceID, startDate);
                };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();

                if (workItem != null)
                {
                    workItem.Deleted = true;
                    ObjResourceWorkItemManager.Save(workItem);
                }
            }

            BindTimeSheetDetails();
        }

        protected void RTimeSheet_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void RTimeSheet_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        protected void BtNewWorkItem_Click(object sender, EventArgs e)
        {
            BindTimeSheetDetails();
        }

        protected void DdlCurrentUsers_Load(object sender, EventArgs e)
        {
            if (cmbCurrentUser.Items.Count <= 0)
            {

                if (isSuperAdmin)
                {
                    List<UserProfile> userCollection = objUserProfileManager.RMMUserList();
                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            cmbCurrentUser.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        }
                    }
                }
                else
                {
                    List<UserProfile> userCollection = new List<UserProfile>();
                    userCollection = objUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id).ToList();
                    userCollection = userCollection.OrderBy(x => x.Name).ToList();
                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            cmbCurrentUser.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        }
                    }
                }
            }

            SetSelectedUser();

            if (currentSelectedUser != null)
            {
                //cmbCurrentUser.ClearSelection();
                cmbCurrentUser.SelectedIndex = cmbCurrentUser.Items.IndexOf(cmbCurrentUser.Items.FindByValue(currentSelectedUser.Id.ToString()));
            }

            if (!IsPostBack)
            {
                DateTime cDate = DateTime.Now;
                if (Request["startDate"] != null)
                {
                    DateTime.TryParse(Request["startDate"], out cDate);
                }

                DayOfWeek selectedStartDay = cDate.DayOfWeek;
                int diff = selectedStartDay - DayOfWeek.Monday;
                DateTime firstDate = cDate.AddDays(-diff);
                dtcStartdate.SelectedDate = firstDate.Date;
                startDate = firstDate;

                lbWeekDuration.Text = string.Format("{0}-{1}", firstDate.ToString("MMM dd,yyyy"), firstDate.AddDays(6).ToString("MMM dd,yyyy"));
                startWeekDateForEdit.Value = firstDate.ToString("MMM dd yyyy");
                BindTimeSheetDetails();
                StartWeekDate.Value = startDate.ToString();
            }
            else // PostBack
            {
                DayOfWeek selectedStartDay = dtcStartdate.SelectedDate.DayOfWeek;
                int diff = selectedStartDay - DayOfWeek.Monday;
                DateTime firstDate = dtcStartdate.SelectedDate.AddDays(-diff);
                dtcStartdate.SelectedDate = firstDate.Date;
                startDate = firstDate;

                lbWeekDuration.Text = string.Format("{0}-{1}", firstDate.ToString("MMM dd,yyyy"), firstDate.AddDays(6).ToString("MMM dd,yyyy"));
                startWeekDateForEdit.Value = firstDate.ToString("MMM dd yyyy");
                if (StartWeekDate.Value.Trim() != string.Empty && DateTime.Parse(StartWeekDate.Value.Trim()).Date != dtcStartdate.SelectedDate.Date)
                {
                    rTimeSheet.EditIndex = -1;
                    BindTimeSheetDetails();
                }
                StartWeekDate.Value = startDate.ToString();
            }
        }

        protected void DdlCurrentUsers_PreRender(object sender, EventArgs e)
        {

        }

        protected void DtcStartdate_DateChanged(object sender, EventArgs e)
        {
            rTimeSheet.EditIndex = -1;
            BindTimeSheetDetails();
        }

        protected void DdlCurrentUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSelectedUser = objUserProfileManager.GetUserById(Convert.ToString(cmbCurrentUser.SelectedItem.Value));

            if (currentSelectedUser != null)
            {
                //Set selected resource id in cockies
                UGITUtility.CreateCookie(Response, resourceFromCookies, currentSelectedUser.Id.ToString());
            }

            rTimeSheet.EditIndex = -1;
            BindTimeSheetDetails();
        }

        protected override void OnPreRender(EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "Actuals", Convert.ToString(cmbCurrentUser.Value), startDate,"",""));
            aAddItem.Attributes.Add("href", "javascript:void(0);");
            aAddItem.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','680px','350px',0,'{1}')", url, Uri.EscapeDataString(Request.Url.AbsoluteUri), formTitle));

            base.OnPreRender(e);
        }

        protected void rTimeSheet_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem item = (ListViewDataItem)e.Item;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableRow row = ((HtmlTableRow)e.Item.FindControl("workSheetRow"));

                string rowClass = setClassForTr((DataRowView)e.Item.DataItem);

                DataRowView rView = e.Item.DataItem as DataRowView;
                foreach (HtmlTableCell cell in row.Cells)
                {
                    string attvalue = Convert.ToString(cell.Attributes["daynum"]);
                    if (!string.IsNullOrEmpty(attvalue) && attvalue.Contains("day"))
                    {
                        cell.Attributes["class"] = Convert.ToString(cell.Attributes["class"]) + (UGITUtility.StringToBoolean(rView["ShowEditButtons"]) ? "week" : string.Empty);
                        //To highlight weekend column 
                        string[] arr = attvalue.Split(new string[] { "day" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr != null && arr.Length > 0)
                        {
                            DateTime weekDate = startDate.AddDays(Convert.ToInt32(arr[0]) - 1);
                            string dayOfWeek = weekDate.DayOfWeek.ToString();
                            if (!string.IsNullOrEmpty(weekEndDays) && weekEndDays.IndexOf(dayOfWeek) != -1)
                                if (weekDate.DayOfWeek == DayOfWeek.Saturday || weekDate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    cell.Attributes["class"] = Convert.ToString(cell.Attributes["class"]) + " clsweekendhighlight";
                                }
                        }
                    }
                }

                #region updated one
                Label lblWorkItem = (Label)item.FindControl("lblWorkItem");

                Label lblType = (Label)item.FindControl("lblType");
                HiddenField hdnWorkItem = (HiddenField)item.FindControl("hdnworkitemname");
                HiddenField hdnSubworkItemTotal = (HiddenField)item.FindControl("hdnSubworkItemTotal");
                HiddenField hdnListName = (HiddenField)item.FindControl("hdnListName");

                // Is logged-in user direct or indirect manager of user or manager of user's department or functional area
                bool isUserManager = (userEditPermisionList != null && userEditPermisionList.Count > 0 && userEditPermisionList.Exists(x => x.Id == currentSelectedUser.Id));

                // Allow user to edit if current user or admin or user's manager (direct or indirect)
                bool isAllowEditing = (currentLoggedInuser.Id == currentSelectedUser.Id || isResourceAdmin || IsManager(currentLoggedInuser, currentSelectedUser) || isUserManager);

                HtmlGenericControl divCommentDay1 = e.Item.FindControl("divCommentDay1") as HtmlGenericControl;
                HtmlGenericControl divCommentDay2 = e.Item.FindControl("divCommentDay2") as HtmlGenericControl;
                HtmlGenericControl divCommentDay3 = e.Item.FindControl("divCommentDay3") as HtmlGenericControl;
                HtmlGenericControl divCommentDay4 = e.Item.FindControl("divCommentDay4") as HtmlGenericControl;
                HtmlGenericControl divCommentDay5 = e.Item.FindControl("divCommentDay5") as HtmlGenericControl;
                HtmlGenericControl divCommentDay6 = e.Item.FindControl("divCommentDay6") as HtmlGenericControl;
                HtmlGenericControl divCommentDay7 = e.Item.FindControl("divCommentDay7") as HtmlGenericControl;


                if (isAllowEditing)
                {
                    HiddenField hdnWeekDays1 = (HiddenField)item.FindControl("hdnWeekDays1");
                    HiddenField hdnWeekDays2 = (HiddenField)item.FindControl("hdnWeekDays2");
                    HiddenField hdnWeekDays3 = (HiddenField)item.FindControl("hdnWeekDays3");
                    HiddenField hdnWeekDays4 = (HiddenField)item.FindControl("hdnWeekDays4");
                    HiddenField hdnWeekDays5 = (HiddenField)item.FindControl("hdnWeekDays5");
                    HiddenField hdnWeekDays6 = (HiddenField)item.FindControl("hdnWeekDays6");
                    HiddenField hdnWeekDays7 = (HiddenField)item.FindControl("hdnWeekDays7");

                    HiddenField hdnComment1 = (HiddenField)item.FindControl("hdnComment1");
                    HiddenField hdnComment2 = (HiddenField)item.FindControl("hdnComment2");
                    HiddenField hdnComment3 = (HiddenField)item.FindControl("hdnComment3");
                    HiddenField hdnComment4 = (HiddenField)item.FindControl("hdnComment4");
                    HiddenField hdnComment5 = (HiddenField)item.FindControl("hdnComment5");
                    HiddenField hdnComment6 = (HiddenField)item.FindControl("hdnComment6");
                    HiddenField hdnComment7 = (HiddenField)item.FindControl("hdnComment7");

                    HiddenField hdbID1 = (HiddenField)item.FindControl("hdbID1");
                    HiddenField hdbID2 = (HiddenField)item.FindControl("hdbID2");
                    HiddenField hdbID3 = (HiddenField)item.FindControl("hdbID3");
                    HiddenField hdbID4 = (HiddenField)item.FindControl("hdbID4");
                    HiddenField hdbID5 = (HiddenField)item.FindControl("hdbID5");
                    HiddenField hdbID6 = (HiddenField)item.FindControl("hdbID6");
                    HiddenField hdbID7 = (HiddenField)item.FindControl("hdbID7");

                    HtmlTableCell tdday1 = (HtmlTableCell)e.Item.FindControl("tdday1");
                    HtmlTableCell tdday2 = (HtmlTableCell)e.Item.FindControl("tdday2");
                    HtmlTableCell tdday3 = (HtmlTableCell)e.Item.FindControl("tdday3");
                    HtmlTableCell tdday4 = (HtmlTableCell)e.Item.FindControl("tdday4");
                    HtmlTableCell tdday5 = (HtmlTableCell)e.Item.FindControl("tdday5");
                    HtmlTableCell tdday6 = (HtmlTableCell)e.Item.FindControl("tdday6");
                    HtmlTableCell tdday7 = (HtmlTableCell)e.Item.FindControl("tdday7");

                    if (hdnSubworkItemTotal.Value == "Total")
                    {
                        divCommentDay1.Visible = false;
                        divCommentDay2.Visible = false;
                        divCommentDay3.Visible = false;
                        divCommentDay4.Visible = false;
                        divCommentDay5.Visible = false;
                        divCommentDay6.Visible = false;
                        divCommentDay7.Visible = false;
                    }

                    if (hdnWeekDays1.Value == "0" || hdnWeekDays1.Value == "")
                    {
                        divCommentDay1.Visible = false;
                    }
                    else
                    {
                        divCommentDay1.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID1.Value }',' { hdnComment1.Value.Replace("\n","<br/>") }','{ hdnListName.Value }')");  // string.Format("AddTicketHoursComment(" + hdbID1.Value + ",'" + hdnComment1.Value + "','" + hdnListName.Value + "')"));

                        if (!string.IsNullOrEmpty(hdnComment1.Value))
                        {
                            divCommentDay1.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay1.Style.Add("visibility", "hidden");
                            tdday1.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday1.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }

                    if (hdnWeekDays2.Value == "0" || hdnWeekDays2.Value == "")
                    {
                        divCommentDay2.Visible = false;
                    }
                    else
                    {
                        divCommentDay2.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID2.Value }',' { hdnComment2.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");

                        if (!string.IsNullOrEmpty(hdnComment2.Value))
                        {
                            divCommentDay2.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay2.Style.Add("visibility", "hidden");
                            tdday2.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday2.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }

                    if (hdnWeekDays3.Value == "0" || hdnWeekDays3.Value == "")
                    {
                        divCommentDay3.Visible = false;
                    }
                    else
                    {
                        divCommentDay3.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID3.Value }',' { hdnComment3.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");

                        if (!string.IsNullOrEmpty(hdnComment3.Value))
                        {
                            divCommentDay3.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay3.Style.Add("visibility", "hidden");
                            tdday3.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday3.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }

                    if (hdnWeekDays4.Value == "0" || hdnWeekDays4.Value == "")
                    {
                        divCommentDay4.Visible = false;
                    }
                    else
                    {
                        divCommentDay4.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID4.Value }',' { hdnComment4.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");

                        if (!string.IsNullOrEmpty(hdnComment4.Value))
                        {
                            divCommentDay4.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay4.Style.Add("visibility", "hidden");
                            tdday4.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday4.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }

                    if (hdnWeekDays5.Value == "0" || hdnWeekDays5.Value == "")
                    {
                        divCommentDay5.Visible = false;
                    }
                    else
                    {
                        divCommentDay5.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID5.Value }',' { hdnComment5.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");

                        if (!string.IsNullOrEmpty(hdnComment5.Value))
                        {
                            divCommentDay5.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay5.Style.Add("visibility", "hidden");
                            tdday5.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday5.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }

                    if (hdnWeekDays6.Value == "0" || hdnWeekDays6.Value == "")
                    {
                        divCommentDay6.Visible = false;
                    }
                    else
                    {
                        divCommentDay6.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID6.Value }',' { hdnComment6.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");

                        if (!string.IsNullOrEmpty(hdnComment6.Value))
                        {
                            divCommentDay6.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay6.Style.Add("visibility", "hidden");
                            tdday6.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday6.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }
                    if (hdnWeekDays7.Value == "0" || hdnWeekDays7.Value == "")
                    {
                        divCommentDay7.Visible = false;
                    }
                    else
                    {
                        divCommentDay7.Attributes.Add("onclick", $"AddTicketHoursComment('{ hdbID7.Value }',' { hdnComment7.Value.Replace("\n", "<br/>") }','{ hdnListName.Value }')");
                        if (!string.IsNullOrEmpty(hdnComment7.Value))
                        {
                            divCommentDay7.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            divCommentDay7.Style.Add("visibility", "hidden");
                            tdday7.Attributes.Add("onmouseover", string.Format("ShowCommentIcon(this)"));
                            tdday7.Attributes.Add("onmouseout", string.Format("HideCommentIcon(this)"));
                        }
                    }
                }
                else
                {
                    divCommentDay1.Visible = false;
                    divCommentDay2.Visible = false;
                    divCommentDay3.Visible = false;
                    divCommentDay4.Visible = false;
                    divCommentDay5.Visible = false;
                    divCommentDay6.Visible = false;
                    divCommentDay7.Visible = false;
                }

                //SubworkItem PMM-11-000057  
                if (tempWorkItem == hdnWorkItem.Value)
                {
                    if (lblWorkItem != null)
                        lblWorkItem.Text = "";
                }
                else
                    tempWorkItem = hdnWorkItem.Value;
                //WorkItem Current Projects (PMM) 
                if (tempType == lblType.Text)
                    lblType.Text = "";
                else
                {
                    tempType = lblType.Text;
                    rowClass += " typegroupstart-row";
                }

                row.Attributes.Add("class", string.Format("{0} {1}", row.Attributes["class"], rowClass));

                #endregion
                #region old code
                //ListViewDataItem item = (ListViewDataItem)e.Item;
                //HiddenField hdnWorkItem = (HiddenField)item.FindControl("hdnworkitemname");
                //Label lblWorkItem = (Label)item.FindControl("lblWorkItem");
                //Label lblType = (Label)item.FindControl("lblType");

                //if (e.Item.ItemType == ListViewItemType.DataItem)
                //{
                //    if (tempType == lblType.Text)
                //    {

                //        lblType.Text = "";
                //    }
                //    else
                //    {
                //        tempType = lblType.Text;
                //    }

                //}

                ////SubworkItem PMM-11-000057  
                //if (!string.IsNullOrEmpty(hdnWorkItem.Value))
                //{
                //    if (tempWorkItem == hdnWorkItem.Value && lblWorkItem != null)
                //        lblWorkItem.Text = "";
                //    else
                //        tempWorkItem = hdnWorkItem.Value;

                //}


                ////WorkItem Current Projects (PMM) 
                //if (tempType == lblType.Text)
                //    lblType.Text = "";
                //else
                //{
                //    tempType = lblType.Text;
                //    rowClass += " typegroupstart-row";
                //}

                //row.Attributes.Add("class", string.Format("{0} {1}", row.Attributes["class"], rowClass));
                #endregion End of old code
            }
        }

        protected void cmbCurrentUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSelectedUser = objUserProfileManager.LoadById(Convert.ToString(cmbCurrentUser.SelectedItem.Value));

            if (currentSelectedUser != null)
            {
                //Set selected resource id in cockies
                UGITUtility.CreateCookie(Response, resourceFromCookies, currentSelectedUser.Id.ToString());
            }

            rTimeSheet.EditIndex = -1;
            rTimeSheetState = new List<TimeSheetStateMaintain>();
            collapseGroups = true;
            BindTimeSheetDetails();
        }

        protected void cmbCurrentUser_Load(object sender, EventArgs e)
        {
            if (cmbCurrentUser.Items.Count <= 0)
            {

                if (isSuperAdmin)
                {
                    List<UserProfile> userCollection = objUserProfileManager.RMMUserList();
                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            cmbCurrentUser.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        }
                    }
                }
                else if (objUserProfileManager.IsRole("Admin", HttpContext.Current.CurrentUser().UserName) || objUserProfileManager.IsRole("ResourceAdmin", HttpContext.Current.CurrentUser().UserName))
                {
                    List<UserProfile> userCollection = new List<UserProfile>();
                    userCollection = objUserProfileManager.Load(x => x.Enabled == true && !x.UserName.EqualsIgnoreCase("SuperAdmin")).OrderBy(x => x.Name).ToList();  // objUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id).ToList();

                    if (userCollection != null)
                    {
                        userCollection = userCollection.OrderBy(x => x.Name).ToList();
                        foreach (UserProfile user in userCollection)
                        {
                            cmbCurrentUser.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                        }
                    }
                }
                else
                {
                    cmbCurrentUser.Items.Add(new ListEditItem(HttpContext.Current.CurrentUser().Name, HttpContext.Current.CurrentUser().Id));
                }
            }

            SetSelectedUser();

            if (currentSelectedUser != null)
            {
                //cmbCurrentUser.ClearSelection();
                cmbCurrentUser.SelectedIndex = cmbCurrentUser.Items.IndexOf(cmbCurrentUser.Items.FindByValue(currentSelectedUser.Id.ToString()));
            }

            if (!IsPostBack)
            {
                DateTime cDate = DateTime.Now;
                if (WeekStartDt != null)
                {
                    //DateTime.TryParse(Request["startDate"], out cDate);
                    cDate = DateTime.ParseExact(string.Format("{0:MM dd yyyy}", WeekStartDt.ToDateTime()), "MM dd yyyy", null);
                }
                else if (Request["startDate"] != null)
                {
                    //DateTime.TryParse(Request["startDate"], out cDate);
                    cDate = DateTime.ParseExact(Request["startDate"], "MM dd yyyy", null);
                }
                if (cDate == DateTime.MinValue)
                    cDate = DateTime.Now;

                if (!string.IsNullOrEmpty(Request["UId"]))
                {
                    currentSelectedUser = objUserProfileManager.LoadById(Convert.ToString(Request["UId"])); 
                    cmbCurrentUser.SelectedIndex = cmbCurrentUser.Items.IndexOf(cmbCurrentUser.Items.FindByValue(Request["UId"]));
                }

                DayOfWeek selectedStartDay = cDate.DayOfWeek;
                int diff = selectedStartDay - DayOfWeek.Monday;
                DateTime firstDate = cDate.AddDays(-diff);
                dtcStartdate.SelectedDate = firstDate.Date;
                startDate = firstDate;

                lbWeekDuration.Text = string.Format("{0}-{1}", firstDate.ToString("MMM dd,yyyy"), firstDate.AddDays(6).ToString("MMM dd,yyyy"));
                startWeekDateForEdit.Value = firstDate.ToString("MMM dd yyyy");

                //Set accessibility of time sheet controls on the basis of Time-Sheet Status
                SetButtonVisibility();

                BindTimeSheetDetails();
                StartWeekDate.Value = startDate.ToString();
            }

            else // PostBack
            {
                DayOfWeek selectedStartDay = dtcStartdate.SelectedDate.DayOfWeek;
                int diff = selectedStartDay - DayOfWeek.Monday;
                DateTime firstDate = dtcStartdate.SelectedDate.AddDays(-diff);
                dtcStartdate.SelectedDate = firstDate.Date;
                startDate = firstDate;

                lbWeekDuration.Text = string.Format("{0}-{1}", firstDate.ToString("MMM dd,yyyy"), firstDate.AddDays(6).ToString("MMM dd,yyyy"));
                startWeekDateForEdit.Value = firstDate.ToString("MMM dd yyyy");

                //Set accessibility of time sheet controls on the basis of Time-Sheet Status
                SetButtonVisibility();

                if (StartWeekDate.Value.Trim() != string.Empty && DateTime.Parse(StartWeekDate.Value.Trim()).Date != dtcStartdate.SelectedDate.Date)
                {
                    rTimeSheet.EditIndex = -1;
                    BindTimeSheetDetails();
                }
                StartWeekDate.Value = startDate.ToString();
            }
        }


        protected void SetButtonVisibility()
        {
            // Is logged-in user direct or indirect manager of user or manager of user's department or functional area
            bool isUserManager = (userEditPermisionList != null && userEditPermisionList.Count > 0 && userEditPermisionList.Exists(x => x.Id == currentSelectedUser.Id));

            //bool isUserManager = true; hardcodecheck
            // Allow user to edit if current user or admin or user's manager (direct or indirect)
            string username = HttpContext.Current.User.Identity.Name.ToString();

            bool isAllowEditing = (HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id || isResourceAdmin || IsManager(currentLoggedInuser, currentSelectedUser) || isUserManager);
            //bool isAllowEditing = true; hardcode check

            btEditTimesheetLI.Visible = btEditTimesheet.Visible = aAddItem.Visible = btCopyTimeSheet.Visible = isAllowEditing;
            hideDeleteButton = !isAllowEditing;
            if (hideDeleteButton)
                Page.ClientScript.RegisterStartupScript(this.GetType(), Constants.CallLockEditing, Constants.FunctionLockEditing, true);
            //TimesheetMode = "normal";
            //check if Configuration variable TimesheetMode is either 'Approval' or 'Sign Off' otherwise disable the timesheet workflow and set it to Normal TimesheetMode
            if (TimesheetMode != Constants.ApprovalMode.ToLower() && TimesheetMode != Constants.SignOffMode.ToLower())
            {
                btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = btnSendApprovalLI.Visible = btnSendApproval.Visible = btnApporvedLI.Visible
                = btnApporved.Visible = aAddItem.Visible = lblStatus.Visible = btnSignOffLI.Visible = btSignOff.Visible = statusPicker.Visible=btCopyTimeSheet.Visible
                = btnApporved.Visible = lblStatus.Visible = btnSignOffLI.Visible = btSignOff.Visible = statusPicker.Visible
                = pendingStatusPicker.Visible = false;
                return;
            }

            signOffWeekStartDate = UGITUtility.StringToDateTime(startWeekDateForEdit.Value);
            status = GetSignOffStatus(signOffWeekStartDate, currentSelectedUser); //if required add GetSignOffStatus()
            ////status = GetSignOffStatus(signOffWeekStartDate, currentSelectedUser);
            //status = Constants.ApprovalMode;
            if (string.IsNullOrEmpty(status))
            {
                btEditTimesheetLI.Visible = btEditTimesheet.Visible = btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = btnSendApprovalLI.Visible
                    = btnSendApproval.Visible = btnApporvedLI.Visible = btnApporved.Visible = aAddItem.Visible = lblStatus.Visible = btnSignOffLI.Visible
                    = btSignOff.Visible = statusPicker.Visible = pendingStatusPicker.Visible = btCopyTimeSheet.Visible = false;
                hideDeleteButton = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), Constants.CallLockEditing, Constants.FunctionLockEditing, true);
                return;
                //}
            }
            //// Managers can approve their subordinate's timesheet, and resource admins can also approve their own timesheet as well
            bool allowApprove = (TimesheetMode == Constants.ApprovalMode.ToLower() && status != Constants.Approved && (isUserManager || isResourceAdmin));

            //bool allowApprove = true;

            btnApporvedLI.Visible = btnApporved.Visible = allowApprove;

            //// Return allowed for managers and resource admins if timesheet approved or in pending approval mode
            bool allowReturnTimeSheet = (TimesheetMode == Constants.ApprovalMode.ToLower() && (status == Constants.PendingApproval || status == Constants.Approved) && (isUserManager || isResourceAdmin));

            //bool allowReturnTimeSheet = true;

            btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = allowReturnTimeSheet;

            //// Users can only send their own timesheets for approval, and only if not already sent for approval or approved
            bool allowSendForApproval = TimesheetMode == Constants.ApprovalMode.ToLower() && (status == Constants.TimeEntry || status == Constants.Returned) &&
                         HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id;
            btnSendApprovalLI.Visible = btnSendApproval.Visible = allowSendForApproval;

            //// Users can only signoff on their own timesheets, and only if not already signed off
            bool allowSignOff = TimesheetMode == Constants.SignOffMode.ToLower() && status == Constants.TimeEntry && HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id;
            btnSignOffLI.Visible = btSignOff.Visible = allowSignOff;

            ////set visibility of subordinate's status and pendingStatus icons
            statusPicker.Visible = isManager || isResourceAdmin;

            pendingStatusPicker.Visible = (TimesheetMode == Constants.ApprovalMode.ToLower() && (isManager || isResourceAdmin));

            timeSheetStatus.Text = status;
            lblPrintStatus.Text = status;

            ////Set color for TimeSheet status label on the basis of it's value
            if (status == Constants.TimeEntry)
            {
                timeSheetStatus.ForeColor = System.Drawing.Color.DarkBlue;
                lblPrintStatus.ForeColor = System.Drawing.Color.DarkBlue;
            }
            else if (status == Constants.Returned)
            {
                timeSheetStatus.ForeColor = System.Drawing.Color.Red;
                lblPrintStatus.ForeColor = System.Drawing.Color.Red;
            }
            else if (status == Constants.PendingApproval)
            {
                //    // timeSheetStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml(rustColorCode);
                timeSheetStatus.ForeColor = System.Drawing.Color.Green;

                //    // lblPrintStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml(rustColorCode);
                lblPrintStatus.ForeColor = System.Drawing.Color.Green;
            }
            //}
            else if (status == Constants.Approved || status == Constants.SignOff)
            {
                timeSheetStatus.ForeColor = System.Drawing.Color.Green;
                lblPrintStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        #region Method to Check if the LoggedIn user is the Manager of current Selected User
        /// <summary>
        /// This method is Check if the LoggedIn user is the Manager of current Selected User
        /// </summary>
        private bool IsManager(UserProfile userloggedIn, UserProfile userSelected)
        {
            if (userloggedIn.IsManager && userSelected.ManagerID != null
                  && userloggedIn.Id == userSelected.ManagerID)
                return true;

            return false;
        }
        #endregion Method to Check if the LoggedIn user is the Manager of current Selected User


        #region Method to change the Time Sheet SignOff Status
        /// <summary>
        /// This method is used to Method to change the Time Sheet SignOff Status
        /// </summary>
        protected void btnSignOffTimeSheet_Click(object sender, EventArgs e)
        {
            string actionValue = hdnActionValue.Value;

            if (!string.IsNullOrEmpty(actionValue))
            {
                if (actionValue == Constants.Return)
                {
                    ReturnTimeSheet();
                }
                else if (actionValue == Constants.SendForApproval)
                {
                    SendTimeSheetForApproval();
                }
                else if (actionValue == Constants.Approved)
                {
                    ApproveTimeSheet();
                }
                else if (actionValue == Constants.SignOff)
                {
                    SignOffTimesheet();
                }
                hdnActionValue.Value = "";
            }
            else
            {
                return;
            }

        }
        #endregion Method to change the Time Sheet SignOff Status

        #region Method to Sending the Time Sheet for Approval
        /// <summary>
        /// This method is used to Sign Off the time sheet by marking time sheet status as Pending Approval.
        /// </summary>
        protected void SendTimeSheetForApproval()
        {
            isSendForApprovalCalled = true;
            DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

            //Check for existing record in signoff list
            status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);
            // add tables for sign off;
            if (!string.IsNullOrEmpty(status) && status == Constants.TimeEntry && isSendForApprovalCalled)
            {
                if (spSignOffList == null || spSignOffList.Rows.Count == 0)
                {
                    DataRow spSignOffListItems = spSignOffList.NewRow();
                    //spSignOffListItems[DatabaseObjects.Columns.Title] = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    //spSignOffListItems[DatabaseObjects.Columns.UGITStartDate] = signOffWeekStartDate;
                    //spSignOffListItems[DatabaseObjects.Columns.UGITEndDate] = signOffWeekEndDate;
                    //spSignOffListItems[DatabaseObjects.Columns.Resource] = currentSelectedUser.Id.ToString();
                    //spSignOffListItems[DatabaseObjects.Columns.History] = currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                    //spSignOffListItems[DatabaseObjects.Columns.SignOffStatus] = Constants.PendingApproval;
                    /*
                    values.Clear();
                    values.Add(DatabaseObjects.Columns.Title, currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd"));
                    values.Add(DatabaseObjects.Columns.UGITStartDate, signOffWeekStartDate);
                    values.Add(DatabaseObjects.Columns.UGITEndDate, signOffWeekEndDate);
                    values.Add(DatabaseObjects.Columns.Resource, currentSelectedUser.Id);
                    values.Add(DatabaseObjects.Columns.History, currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus);
                    values.Add(DatabaseObjects.Columns.SignOffStatus, Constants.PendingApproval);
                    values.Add(DatabaseObjects.Columns.TenantID, context.TenantID);

                    GetTableDataManager.AddItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, values);
                    
                    // spSignOffListItems.Update();
                    */

                    ResourceTimeSheetSignOff resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();
                    resourceTimeSheetSignOff.Title = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    resourceTimeSheetSignOff.StartDate = signOffWeekStartDate;
                    resourceTimeSheetSignOff.EndDate = signOffWeekEndDate;
                    resourceTimeSheetSignOff.Resource = currentSelectedUser.Id;
                    resourceTimeSheetSignOff.History = currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                    resourceTimeSheetSignOff.SignOffStatus = Constants.PendingApproval;

                    resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);
                }
                isSendForApprovalCalled = false;
            }
            if (currentLoggedInuser.Id == currentSelectedUser.Id)
            {
                btnSendApprovalLI.Visible = btnSendApproval.Visible = false;
                timeSheetStatus.Text = Constants.PendingApproval;
                lblPrintStatus.Text = Constants.PendingApproval;
                timeSheetStatus.ForeColor = System.Drawing.Color.Green;
                lblPrintStatus.ForeColor = System.Drawing.Color.Green;
                btnApporvedLI.Visible = btnApporved.Visible = isResourceAdmin;
                btnApporvedLI.Visible = btnApporved.Visible = true; ;

                BindTimeSheetDetails();

                if (!disableEmails && currentLoggedInuser.ManagerID != null && !string.IsNullOrEmpty(currentLoggedInuser.Email))
                {
                    // take manager details of selected user or logged in user as both are same in this case
                    //string informTo = currentLoggedInUser.Manager.Name;
                    var managerId = currentSelectedUser.ManagerID;
                    var managerForCurrentSelectedUser = objUserProfileManager.LoadById(Convert.ToString(managerId));

                    string informTo = managerForCurrentSelectedUser.Name;

                    //string mailTo = currentLoggedInUser.Manager.Email;
                    string mailTo = managerForCurrentSelectedUser.Email;
                    SendEmail(informTo, mailTo, Constants.TimesheetPendingApprovalStatus, signOffWeekStartDate);
                }
            }
            else
            {
                return;
            }
        }
        #endregion Method to Sending the Time Sheet for Approval


        #region Method to Approve the TimeSheet
        /// <summary>
        /// This method is used to Approve the Timesheet for a particular week for a user by changing its SignOffStatus to Approved.
        /// </summary>
        protected void ApproveTimeSheet()
        {
            isApprovedCalled = true;
            DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

            status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);
            //insert
            if (!string.IsNullOrEmpty(status) && isApprovedCalled)
            {
                if (spSignOffList != null)
                {
                    /*
                    DataRow spSignOffListItems = spSignOffList.NewRow();
                    //spSignOffListItems[DatabaseObjects.Columns.Title] = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    //spSignOffListItems[DatabaseObjects.Columns.UGITStartDate] = signOffWeekStartDate;
                    //spSignOffListItems[DatabaseObjects.Columns.UGITEndDate] = signOffWeekEndDate;
                    //spSignOffListItems[DatabaseObjects.Columns.Resource] =  currentSelectedUser.Id;
                    //spSignOffListItems[DatabaseObjects.Columns.History] = currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetApprovedStatus;
                    //spSignOffListItems[DatabaseObjects.Columns.SignOffStatus] = Constants.Approved;

                    values.Clear();

                    values.Add(DatabaseObjects.Columns.Title, currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd"));
                    values.Add(DatabaseObjects.Columns.UGITStartDate, signOffWeekStartDate);
                    values.Add(DatabaseObjects.Columns.UGITEndDate, signOffWeekEndDate);
                    values.Add(DatabaseObjects.Columns.Resource, currentSelectedUser.Id);
                    values.Add(DatabaseObjects.Columns.History, currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetApprovedStatus);
                    values.Add(DatabaseObjects.Columns.SignOffStatus, Constants.Approved);
                    values.Add(DatabaseObjects.Columns.TenantID, context.TenantID);

                    GetTableDataManager.AddItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, values);

                    // spSignOffListItems.Update();
                    */
                    isApprovedCalled = false;
                }
            }

            if (!string.IsNullOrEmpty(status) && !isApprovedCalled)
            {
                btnSendApprovalLI.Visible = btnSendApproval.Visible = btnApporvedLI.Visible = btnApporved.Visible = false;
                timeSheetStatus.Text = Constants.Approved;
                lblPrintStatus.Text = Constants.Approved;
                timeSheetStatus.ForeColor = System.Drawing.Color.Green;
                lblPrintStatus.ForeColor = System.Drawing.Color.Green;
                BindTimeSheetDetails();

                if (!disableEmails && currentLoggedInuser != currentSelectedUser && (isResourceAdmin || IsManager(currentLoggedInuser, currentSelectedUser)))
                {
                    btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = true;
                    string informTo = currentSelectedUser.Name;
                    string mailTo = currentSelectedUser.Email;
                    SendEmail(informTo, mailTo, Constants.TimesheetApprovedStatus, signOffWeekStartDate);
                }
            }
            else
            {
                isApprovedCalled = false;
                return;
            }
        }
        #endregion Method to Approve the TimeSheet

        #region Method to Sign Off the TimeSheet
        /// <summary>
        /// This method is Sign Off the Timesheet for a particular week for a user by changing its SignOffStatus to Sign Off.
        /// </summary>
        protected void SignOffTimesheet()
        {
            isSignOffCalled = true;
            DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

            status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);

            if (!string.IsNullOrEmpty(status) && isSignOffCalled)
            {
                if (spSignOffList != null)
                {
                    DataRow spSignOffListItems = spSignOffList.NewRow();
                    //spSignOffListItems[DatabaseObjects.Columns.Title] = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    //spSignOffListItems[DatabaseObjects.Columns.UGITStartDate] = signOffWeekStartDate;
                    //spSignOffListItems[DatabaseObjects.Columns.UGITEndDate] = signOffWeekEndDate;
                    //spSignOffListItems[DatabaseObjects.Columns.Resource] = currentSelectedUser.Id;
                    //spSignOffListItems[DatabaseObjects.Columns.History] = currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.SignOff;
                    //spSignOffListItems[DatabaseObjects.Columns.SignOffStatus] = Constants.SignOff;
                    values.Clear();
                    values.Add(DatabaseObjects.Columns.Title, currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd"));
                    values.Add(DatabaseObjects.Columns.UGITStartDate, signOffWeekStartDate);
                    values.Add(DatabaseObjects.Columns.UGITEndDate, signOffWeekEndDate);
                    values.Add(DatabaseObjects.Columns.Resource, currentSelectedUser.Id);
                    values.Add(DatabaseObjects.Columns.History, currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.SignOff);
                    values.Add(DatabaseObjects.Columns.SignOffStatus, Constants.SignOff);
                    values.Add(DatabaseObjects.Columns.TenantID, context.TenantID);

                    GetTableDataManager.AddItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, values);

                    //spSignOffListItems.Update();
                    isSignOffCalled = false;
                }
            }

            if (!string.IsNullOrEmpty(status) && !isSignOffCalled)
            {
                btnSignOffLI.Visible = btSignOff.Visible = false;
                timeSheetStatus.Text = Constants.SignOff;
                lblPrintStatus.Text = Constants.SignOff;
                timeSheetStatus.ForeColor = System.Drawing.Color.Green;
                lblPrintStatus.ForeColor = System.Drawing.Color.Green;
                BindTimeSheetDetails();

                var currentLoggedInUser = objUserProfileManager.LoadById(Convert.ToString(currentUserID));
                var managerForCurrentLoggedInUser = objUserProfileManager.LoadById(Convert.ToString(currentLoggedInUser.ManagerID));

                if (!disableEmails && currentLoggedInUser.ManagerID != null && !string.IsNullOrEmpty(managerForCurrentLoggedInUser.Email))
                {

                    string informTo = managerForCurrentLoggedInUser.Name;
                    string mailTo = currentLoggedInUser.Email;
                    SendEmail(informTo, mailTo, "Signed Off", signOffWeekStartDate);
                }
            }
            else
            {
                isSignOffCalled = false;
                return;
            }
        }
        #endregion Method to Approve the TimeSheet

        #region Method to Return the TimeSheet
        /// <summary>
        /// This method is used to UnLock the TimeSheet and to change its SignOffStatus to Return for a particular week for a user.
        /// </summary>
        protected void ReturnTimeSheet()
        {
            isReturnCalled = true;
            status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);
            if (!string.IsNullOrEmpty(status) && !isReturnCalled)
            {
                btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = btnApporvedLI.Visible = btnApporved.Visible = false;
                timeSheetStatus.Text = Constants.Returned;
                lblPrintStatus.Text = Constants.Returned;
                timeSheetStatus.ForeColor = System.Drawing.Color.Red;
                lblPrintStatus.ForeColor = System.Drawing.Color.Red;
                BindTimeSheetDetails();

                if (!disableEmails)
                {
                    string informTo = currentSelectedUser.Name;
                    string mailTo = currentSelectedUser.Email;
                    SendEmail(informTo, mailTo, Constants.TimeSheetReturnStatus, signOffWeekStartDate);
                }
            }
            else
            {
                isReturnCalled = false;
                return;
            }
        }
        #endregion Method to Return the Time Sheet 

        #region Method to Get the Current Sign Off Status of the Time Sheet
        /// <summary>
        /// This method is used to get the current Status of the time sheet for a particular week for a user
        /// </summary>
        protected string GetCurretSignOffStatus(DateTime weekStartDate, UserProfile selectedUser)
        {
            string timesheetStatus = string.Empty;
            DateTime weekEndDate = weekStartDate.EndOfWeek(DayOfWeek.Sunday);
            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, selectedUser.Id));
            if (spSignOffList != null && spSignOffList.Rows.Count > 0)
            {
                string query = string.Format("{0}", UGITUtility.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
                DataRow[] resultCollection = spSignOffList.Select(query);
                if (resultCollection != null && resultCollection.Length > 0)
                {
                    DataRow item = resultCollection[0];
                    timesheetStatus = Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]);
                }
            }
            return timesheetStatus;
        }
        #endregion Method to Get the Current Sign Off Status of the Time Sheet

        #region Method to Update SignOffStatus of the Timesheet
        /// <summary>
        /// This method is used to update the Status of the timesheet for a particular week for a user
        /// </summary>
        protected string UpdateSignOffStatus(DateTime weekStartDate, UserProfile selectedUser)
        {
            string timesheetStatus = Constants.TimeEntry; // Status defaults to TimeEntry
            DateTime weekEndDate = weekStartDate.EndOfWeek(DayOfWeek.Sunday);
            var currentLoggedInUser = objUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, selectedUser.Id));
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.UGITStartDate, weekStartDate.ToString("yyyy-MM-dd")));
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.UGITEndDate, weekEndDate.ToString("yyyy-MM-dd")));

            if (spSignOffList != null && spSignOffList.Rows.Count > 0)
            {
                values.Clear();
                string query = string.Empty;
                if (requiredQuery.Count > 0)
                {
                    query = string.Join("AND ", requiredQuery);
                }
                DataRow[] resultCollection = spSignOffList.Select(query);
                if (resultCollection != null && resultCollection.Length > 0)
                {
                    ResourceTimeSheetSignOff resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();

                    DataRow item = resultCollection[0];
                    resourceTimeSheetSignOff = resourceTimeSheetSignOffManager.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.Id]));

                    timesheetStatus = Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]);
                    signOffItemId = Convert.ToInt16(item[DatabaseObjects.Columns.Id]);
                    string history = Convert.ToString(item[DatabaseObjects.Columns.History]).Trim();
                    if (!string.IsNullOrWhiteSpace(history))
                        history += Constants.SeparatorForVersions;
                    //updating timesheet status when Return button is clicked
                    if (isReturnCalled)
                    {
                        item[DatabaseObjects.Columns.SignOffStatus] = Constants.Returned;
                        history += currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimeSheetReturnStatus + Constants.Separator + txtAddComment.Text;
                        isReturnCalled = false;
                        timesheetStatus = Constants.Returned;
                    }
                    //upating the timesheet status when Send For Approval button is clicked
                    else if (isSendForApprovalCalled)
                    {
                        item[DatabaseObjects.Columns.SignOffStatus] = Constants.PendingApproval;
                        history += currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                        isSendForApprovalCalled = false;
                        timesheetStatus = Constants.PendingApproval;
                    }
                    //upating the timesheet status when Approved button is clicked
                    else if (isApprovedCalled)
                    {
                        item[DatabaseObjects.Columns.SignOffStatus] = Constants.Approved;
                        history += currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetApprovedStatus;
                        isApprovedCalled = false;
                        timesheetStatus = Constants.Approved;
                    }
                    //upating the timesheet status when Sign Off button is clicked
                    else if (isSignOffCalled)
                    {
                        item[DatabaseObjects.Columns.SignOffStatus] = Constants.SignOff;
                        history += currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.SignOff;
                        isSignOffCalled = false;
                        timesheetStatus = Constants.SignOff;
                    }
                    //upating the timesheet status to Time Entry when status is Sign Off and user edit and save his timesheet
                    else if (isTimeEntryCalled)
                    {
                        item[DatabaseObjects.Columns.SignOffStatus] = Constants.TimeEntry;
                        history += currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimeEntry;
                        isTimeEntryCalled = false;
                        timesheetStatus = Constants.TimeEntry;
                    }
                    else
                    {
                        return timesheetStatus;
                    }
                    item[DatabaseObjects.Columns.History] = history;

                    resourceTimeSheetSignOff.History = history;
                    resourceTimeSheetSignOff.SignOffStatus = timesheetStatus;
                    resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);

                    /*
                    values.Add(DatabaseObjects.Columns.History, history);
                    values.Add(DatabaseObjects.Columns.SignOffStatus, timesheetStatus);
                    GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]), values);
                    */
                }
                else if (resultCollection != null && resultCollection.Length == 0)
                {
                    addTimeEntry = true; // shubhangi done change
                                         //upating the timesheet status to Time Entry when there is no entry in ResourceTimeSheetSignOff list and user edit & save his timesheet
                    if (addTimeEntry)
                    {
                        /*
                        values.Clear();
                        DataRow spSignOffListItems = spSignOffList.Rows.Add();
                        spSignOffListItems[DatabaseObjects.Columns.Title] = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + weekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + weekEndDate.ToString("yyyy-MM-dd");
                        spSignOffListItems[DatabaseObjects.Columns.UGITStartDate] = weekStartDate;
                        spSignOffListItems[DatabaseObjects.Columns.UGITEndDate] = weekEndDate;
                        spSignOffListItems[DatabaseObjects.Columns.Resource] = currentSelectedUser.Id;
                        spSignOffListItems[DatabaseObjects.Columns.History] = currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimeEntry;
                        spSignOffListItems[DatabaseObjects.Columns.SignOffStatus] = Constants.TimeEntry;
                        values.Add(DatabaseObjects.Columns.Title, currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + weekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + weekEndDate.ToString("yyyy-MM-dd"));
                        // values.Add(DatabaseObjects.Columns.UGITStartDate, weekStartDate);
                        // values.Add(DatabaseObjects.Columns.UGITEndDate, weekEndDate);
                        values.Add(DatabaseObjects.Columns.Resource, currentSelectedUser.Id);
                        values.Add(DatabaseObjects.Columns.History, currentLoggedInUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimeEntry);
                        values.Add(DatabaseObjects.Columns.SignOffStatus, Constants.TimeEntry);
                        // spSignOffListItems.update();
                        //GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, UGITUtility.StringToLong(spSignOffListItems[DatabaseObjects.Columns.ID]), values);
                        GetTableDataManager.AddItem<int>(DatabaseObjects.Tables.ResourceTimeSheetSignOff, values);
                        */

                        ResourceTimeSheetSignOff resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();
                        resourceTimeSheetSignOff.Title = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + weekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + weekEndDate.ToString("yyyy-MM-dd");
                        resourceTimeSheetSignOff.StartDate = weekStartDate;
                        resourceTimeSheetSignOff.Resource = currentSelectedUser.Id;
                        resourceTimeSheetSignOff.EndDate = weekEndDate;
                        resourceTimeSheetSignOff.History = currentLoggedInuser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                        resourceTimeSheetSignOff.SignOffStatus = Constants.PendingApproval;
                        resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);

                        addTimeEntry = false;
                        timesheetStatus = Constants.TimeEntry;
                    }
                }
            }
            return timesheetStatus;
        }
        #endregion Method to Update SignOffStatus of the Timesheet

        #region Method to Get Status of the Time Sheet
        /// <summary>
        /// This method is used to get the Status of the time sheet for a particular week for a user
        /// </summary>
        protected string GetSignOffStatus(DateTime weekStartDate, UserProfile selectedUser)
        {
            string timesheetStatus = Constants.TimeEntry; // Status defaults to TimeEntry
            DateTime weekEndDate = weekStartDate.EndOfWeek(DayOfWeek.Sunday);
            List<string> requiredQuery = new List<string>();

            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, selectedUser.Id));
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.StartDate, weekStartDate.ToString("yyyy-MM-dd")));
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.EndDate, weekEndDate.ToString("yyyy-MM-dd")));

            if (spSignOffList != null && spSignOffList.Rows.Count > 0)
            {
                string query = string.Format("{0}", UGITUtility.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
                DataRow[] resultCollection = spSignOffList.Select(query);
                if (resultCollection != null && resultCollection.Length > 0)
                {
                    DataRow item = resultCollection[0];
                    timesheetStatus = Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]);
                    signOffItemId = Convert.ToInt32(item[DatabaseObjects.Columns.ID]);
                }
            }
            return timesheetStatus;
        }
        #endregion Method to Get Sign Off Status of the Time Sheet

        #region Method to Send Email for Time Sheet Status changed
        /// <summary>
        /// This method is used to Send Email when the Manager or the Resrouce Admin return the Time Sheet Status to a user.
        /// </summary>
        private void SendEmail(string infromTo, string mailTo, string statusText, DateTime weekStartDate)
        {
            DateTime weekEndDate = Extensions.EndOfWeek(weekStartDate, DayOfWeek.Sunday);
            timeSheetAbsoluteUrl = string.Format("{0}?TabN=Actuals&startDate={1}&UId={2}", timeSheetAbsoluteUrl, weekStartDate, currentSelectedUser.Id);

            string weekStartDateString = weekStartDate.ToString("MMM dd, yyyy");
            string weekEndDateString = weekEndDate.ToString("MMM dd, yyyy");

            string subject = string.Format("Timesheet for {0}: {1} {2}", currentSelectedUser.Name, weekStartDateString, statusText);
            StringBuilder bodyText = new StringBuilder();
            bodyText.AppendFormat("<div>");
            bodyText.AppendFormat("<span>Hi <b>{0}</b>,</span>", infromTo);
            bodyText.AppendFormat("<span><br /><br /><b>{0}'s</b> timesheet for the week of <b>{1}</b> to <b>{2}</b> has been <b>{3}</b>.</span>",
                                  currentSelectedUser.Name, weekStartDateString, weekEndDateString, statusText);
            if (statusText == Constants.TimeSheetReturnStatus)
                bodyText.AppendFormat("<span><br /><br /><b>Manager Comment:</b> {0}</span>", txtAddComment.Text);

            bodyText.AppendFormat("<span><br /><br />Please <a href=\"{0}\">click here</a> to view the timesheet.</span>",
                                   UGITUtility.GetAbsoluteURL(timeSheetAbsoluteUrl));
            bodyText.AppendFormat("</div>");
            MailMessenger mail = new MailMessenger(context);

            mail.SendMail(mailTo, subject, string.Empty, bodyText.ToString(), true);
        }
        #endregion Method to Send Email for Time Sheet Status changed

        #region Method to bind TimeSheet data to Printable view list
        /// <summary>
        /// This method to used to bind TimeSheet data to Printable view list
        /// </summary>
        protected void ListViewPrint_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem item = (ListViewDataItem)e.Item;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView rView = e.Item.DataItem as DataRowView;
                Label lblType = (Label)item.FindControl("lblType");

                if (tempPrintType == lblType.Text)
                    lblType.Text = "";
                else
                    tempPrintType = lblType.Text;

                HiddenField hdnWorkItem = (HiddenField)item.FindControl("hdnworkitemname");
                if (tempPrintWorkItem == hdnWorkItem.Value)
                {
                    Label lblWorkItem = (Label)item.FindControl("lblWorkItem");
                    lblWorkItem.Text = "";
                }
                else
                {
                    tempPrintWorkItem = hdnWorkItem.Value;
                }
            }
        }
        #endregion MMethod to bind TimeSheet data to Printable view list

        #region Callback Method of statusGridContainer
        protected void statusGridContainer_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            statusGrid.DataBind();
        }
        #endregion Callback Method of statusGridContainer

        #region Callback Method of pendingStatusGridContainer
        protected void pendingStatusGridContainer_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            pendingStatusGrid.DataBind();
        }
        #endregion Callback Method of pendingStatusGridContainer

        #region Method to Get Timesheet Pending Approval popup Grid Data
        /// <summary>
        /// This method is used to get Team Member's Data for all the weeks if status is Pending For Approval
        /// </summary>
        private DataTable GetPendingStatusData()
        {
            string query = string.Empty;
            DataTable pendingApprovalData = new DataTable();
            pendingApprovalData.Columns.Add(DatabaseObjects.Columns.ResourceId);
            //pendingApprovalData.Columns.Add(DatabaseObjects.Columns.ResourceName);
            pendingApprovalData.Columns.Add(DatabaseObjects.Columns.Resource);

            pendingApprovalData.Columns.Add(DatabaseObjects.Columns.Status);
            pendingApprovalData.Columns.Add(DatabaseObjects.Columns.WeekStartDate);

            if (!isResourceAdmin && !isManager)
                return pendingApprovalData; // Only admins and managers can see data

            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}= '{1}'", DatabaseObjects.Columns.SignOffStatus, Constants.PendingApproval));

            if (spSignOffList != null && spSignOffList.Rows.Count > 0)
            {

                if (requiredQuery.Count > 0)
                {
                    query = string.Join("AND ", requiredQuery);
                }

                DataRow[] resultCollection = spSignOffList.Select(query);

                if (resultCollection != null && resultCollection.Count() > 0)
                {
                    foreach (DataRow item in resultCollection)
                    {
                        string resourceId = Convert.ToString(item[DatabaseObjects.Columns.Resource]);
                        var resourceName = objUserProfileManager.LoadById(resourceId);

                        pendingApprovalData.Rows.Add(resourceId, resourceName.Name, Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]), UGITUtility.GetDateStringInFormat(Convert.ToString(item[DatabaseObjects.Columns.UGITStartDate]), false));
                    }
                }
            }

            // Return all the pending status data if loggedin user is Resource Admin
            if (isResourceAdmin || pendingApprovalData.Rows.Count == 0)
                return pendingApprovalData;

            // If we got here, then user is a manager but not a resource admin, so need to return only subordinates' data
            // Filter data for subordinates only
            DataRow[] rowColl = pendingApprovalData.AsEnumerable().Where(x => userEditPermisionList.Any(y => y.Id == Convert.ToString(x[DatabaseObjects.Columns.Resource]))).ToArray();

            if (rowColl != null && rowColl.Length > 0)
                pendingApprovalData = rowColl.CopyToDataTable();
            else // If no data for subordinates, then clear the data table
                pendingApprovalData.Rows.Clear();

            return pendingApprovalData;
        }
        #endregion Method to Get Timesheet Pending Approval popup Grid Data

        #region Data Binding Method for Timesheet Pending Approval popup Grid
        protected void pendingStatusGrid_DataBinding(object sender, EventArgs e)
        {
            pendingStatusGrid.DataSource = GetPendingStatusData();

            //Enable checkbox on filter popup
            GridHeaderFilterMode headerFilterMode = GridHeaderFilterMode.CheckedList;
            foreach (GridViewDataColumn column in pendingStatusGrid.Columns)
            {
                column.SettingsHeaderFilter.Mode = headerFilterMode;
            }
        }
        #endregion Data Binding Method for Timesheet Pending Approval popup Grid


        #region Data Binding Method for Timesheet for My Team popup Grid
        protected void statusGrid_DataBinding(object sender, EventArgs e)
        {
            statusGrid.DataSource = GetMyTeamStatusData();

            //Enable checkbox on filter popup
            GridHeaderFilterMode headerFilterMode = GridHeaderFilterMode.CheckedList;
            foreach (GridViewDataColumn column in statusGrid.Columns)
            {
                column.SettingsHeaderFilter.Mode = headerFilterMode;
            }
        }
        #endregion Data Binding Method for Timesheet for My Team popup Grid

        #region Method to Get Timesheet for My Team popup Grid Data
        /// <summary>
        /// This method is used to get My Team Status Popup Data for the selected week
        /// </summary>
        private DataTable GetMyTeamStatusData()
        {
            var currentLoggedInUser = objUserProfileManager.LoadById(Convert.ToString(currentUserID));

            DataTable myTeamStatusData = new DataTable();
            myTeamStatusData.Columns.Add(DatabaseObjects.Columns.ResourceId);
            //myTeamStatusData.Columns.Add(DatabaseObjects.Columns.ResourceName);
            myTeamStatusData.Columns.Add(DatabaseObjects.Columns.Resource);
            myTeamStatusData.Columns.Add(DatabaseObjects.Columns.Status);
            myTeamStatusData.Columns.Add(DatabaseObjects.Columns.WorkDate);
            myTeamStatusData.Columns.Add(DatabaseObjects.Columns.Modified);

            DataTable signOffListData = myTeamStatusData.Clone();

            //Adding all subordinates to datatable with the default status as TimeEntry
            foreach (UserProfile item in userEditPermisionList)
            {
                if (item.Id != currentLoggedInUser.Id)
                {
                    DataRow row = myTeamStatusData.NewRow();
                    row[DatabaseObjects.Columns.ResourceId] = item.Id;
                    //row[DatabaseObjects.Columns.ResourceName] = item.Name;
                    row[DatabaseObjects.Columns.Resource] = item.Name;
                    row[DatabaseObjects.Columns.Status] = Constants.TimeEntry;
                    row[DatabaseObjects.Columns.WorkDate] = "";
                    row[DatabaseObjects.Columns.Modified] = "";
                    myTeamStatusData.Rows.Add(row);
                }
            }

            //Return datatable if user don't have any subordinates
            if (myTeamStatusData == null || myTeamStatusData.Rows.Count == 0)
                return myTeamStatusData;

            DateTime weekStartDate = DateTime.Now;

            //Get week start date from hidden field startWeekDateForEdit
            if (!string.IsNullOrEmpty(startWeekDateForEdit.Value))
                weekStartDate = UGITUtility.StringToDateTime(startWeekDateForEdit.Value);

            DateTime weekEndDate = weekStartDate.EndOfWeek(DayOfWeek.Sunday);

            //Fetch last updated Resource work item data from ResourceTimeSheet list where 'Hours Taken' is greater than 0 for the selected week
            ILookup<string, DataRow> lastUpdatedWorkItemLookup = GetLastUpdateUserdata(weekStartDate, weekEndDate);

            //Updating workdate and created date for those users which have record in ResourceTimeSheet list for the selected week
            if (lastUpdatedWorkItemLookup != null && lastUpdatedWorkItemLookup.Count > 0)
            {
                foreach (DataRow drMyTeam in myTeamStatusData.Rows)
                {
                    //string resource = Convert.ToString(drMyTeam[DatabaseObjects.Columns.ResourceId]) + Constants.Separator + Convert.ToString(drMyTeam[DatabaseObjects.Columns.ResourceName]);
                    string resource = Convert.ToString(drMyTeam[DatabaseObjects.Columns.ResourceId]);

                    var dataRowGroup = lastUpdatedWorkItemLookup.FirstOrDefault(x => x.Key == resource);

                    if (dataRowGroup == null)
                        continue;

                    DataRow[] rowSet = dataRowGroup.ToArray();

                    if (rowSet == null && rowSet.Length == 0)
                        continue;

                    //fecthing record with Max workdate
                    DataRow row = rowSet.CopyToDataTable().Select("WorkDate = MAX(WorkDate)").FirstOrDefault();

                    drMyTeam[DatabaseObjects.Columns.WorkDate] = UGITUtility.GetDateStringInFormat(Convert.ToString(row[DatabaseObjects.Columns.WorkDate]), false);
                    drMyTeam[DatabaseObjects.Columns.Modified] = UGITUtility.GetDateStringInFormat(Convert.ToString(row[DatabaseObjects.Columns.Modified]), true);
                }
            }

            //Fetch records from ResourceTimeSheetSignOff list for the selected week
            List<string> requiredQuery = new List<string>();

            requiredQuery.Add(string.Format("{0}= '{1}' ", DatabaseObjects.Columns.UGITStartDate, weekStartDate.ToString("yyyy-MM-dd")));
            requiredQuery.Add(string.Format("{0}= '{1}'", DatabaseObjects.Columns.UGITEndDate, weekEndDate.ToString("yyyy-MM-dd")));


            if (spSignOffList != null && spSignOffList.Rows.Count > 0)
            {

                string query = string.Empty;
                if (requiredQuery.Count > 0)
                {
                    query = string.Join("AND ", requiredQuery);
                }

                DataRow[] resultCollection = spSignOffList.Select(query);

                if (resultCollection != null && resultCollection.Count() > 0)
                {
                    foreach (var item in resultCollection)
                    {
                        //SPFieldLookupValue resourceLookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.Resource]));

                        var resourceID = Convert.ToString(item[DatabaseObjects.Columns.Resource]);
                        if (resourceID != currentLoggedInUser.Id)
                            signOffListData.Rows.Add(resourceID, resourceID, Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]));
                    }
                }
            }

            //If there is no record in ResourceTimeSheetSignoff list for selected week than return myTeamStatusData
            if (signOffListData.Rows.Count == 0)
                return myTeamStatusData;

            //updating status for those users which have record in ResourceTimeSheetSignoff list for the selected week
            else if (signOffListData.Rows.Count > 0)
            {
                foreach (DataRow drList in signOffListData.Rows)
                {
                    myTeamStatusData.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.ResourceId]) == Convert.ToString(drList[DatabaseObjects.Columns.ResourceId])).ToList().ForEach(x =>
                    {
                        x[DatabaseObjects.Columns.Status] = Convert.ToString(drList[DatabaseObjects.Columns.Status]);
                    });
                }
            }
            return myTeamStatusData;
        }
        #endregion Method to Get Timesheet for My Team popup Grid Data


        #region Method to Get Last Updated Time Entry Records from ResourceTimeSheet list for current week
        /// <summary>
        /// This method is used to get Last Updated Time Entry records from ResourceTimeSheet list for current week
        /// </summary>
        private ILookup<string, DataRow> GetLastUpdateUserdata(DateTime weekStartDate, DateTime weekEndDate)
        {
            ILookup<string, DataRow> lastUpdatedWorkItemLookup = null;
            DataTable timesheetData = new DataTable();

            List<string> requiredQuery = new List<string>();

            requiredQuery.Add(string.Format("{0}= '{1}'", DatabaseObjects.Columns.WorkDate, weekStartDate.ToString("yyyy-MM-dd"), weekEndDate.ToString("yyyy-MM-dd")));
            requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.HoursTaken, 0));

            DataTable spResourceTimesheetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (spResourceTimesheetList != null && spResourceTimesheetList.Rows.Count > 0)
            {

                if (requiredQuery.Count > 0)
                {
                    string query = string.Join("AND ", requiredQuery);

                    // DataRow[] resourceTimeSheetData = spResourceTimesheetList.Select(query);
                    DataRow[] resourceTimeSheetData = spResourceTimesheetList.Select();

                    if (resourceTimeSheetData.Count() > 0)
                        timesheetData = resourceTimeSheetData.CopyToDataTable();

                }
            }

            if (timesheetData != null && timesheetData.Rows.Count > 0)
                lastUpdatedWorkItemLookup = timesheetData.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource));

            return lastUpdatedWorkItemLookup;
        }
        #endregion Method to Get Last Updated Time Entry Records from ResourceTimeSheet list for current week

        //#region Method to set accessibility of Time Sheet action control buttons
        ///// <summary>
        ///// This method is used to set the accessibilty of the time-sheet buttons on the basis of time-sheet status
        ///// </summary>
        //protected void SetButtonVisibility()
        //{
        //    // Is logged-in user direct or indirect manager of user or manager of user's department or functional area
        //    // bool isUserManager = (userEditPermisionList != null && userEditPermisionList.Count > 0 && userEditPermisionList.Exists(x => x.ID == currentSelectedUser.ID));
        //    bool isUserManager = true;
        //    // Allow user to edit if current user or admin or user's manager (direct or indirect)

        //    // bool isAllowEditing = (HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id || isResourceAdmin || IsManager(currentLoggedInUser, currentSelectedUser) || isUserManager);
        //    bool isAllowEditing = true;

        //    btEditTimesheetLI.Visible = btEditTimesheet.Visible = aAddItem.Visible = isAllowEditing;
        //    //hideDeleteButton = !isAllowEditing;
        //    //if (hideDeleteButton)
        //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), Constants.CallLockEditing, Constants.FunctionLockEditing, true);

        //    //check if Configuration variable TimesheetMode is either 'Approval' or 'Sign Off' otherwise disable the timesheet workflow and set it to Normal TimesheetMode
        //    if (TimesheetMode != Constants.ApprovalMode.ToLower() && TimesheetMode != Constants.SignOffMode.ToLower())
        //    {
        //        btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = btnSendApprovalLI.Visible = btnSendApproval.Visible = btnApporvedLI.Visible
        //            = btnApporved.Visible = aAddItem.Visible = lblStatus.Visible = btnSignOffLI.Visible = btSignOff.Visible = statusPicker.Visible
        //            = pendingStatusPicker.Visible = false;
        //        return;
        //    }

        //    signOffWeekStartDate = UGITUtility.StringToDateTime(startWeekDateForEdit.Value);
        //    //status = GetSignOffStatus(signOffWeekStartDate, currentSelectedUser);
        //    status = Constants.ApprovalMode;
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        btEditTimesheetLI.Visible = btEditTimesheet.Visible = btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = btnSendApprovalLI.Visible
        //            = btnSendApproval.Visible = btnApporvedLI.Visible = btnApporved.Visible = aAddItem.Visible = lblStatus.Visible = btnSignOffLI.Visible
        //            = btSignOff.Visible = statusPicker.Visible = pendingStatusPicker.Visible = false;
        //        //hideDeleteButton = true;
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), Constants.CallLockEditing, Constants.FunctionLockEditing, true);
        //        return;
        //    }

        //    // Managers can approve their subordinate's timesheet, and resource admins can also approve their own timesheet as well
        //    // bool allowApprove = (TimesheetMode == Constants.ApprovalMode.ToLower() && status != Constants.Approved && (isUserManager || isResourceAdmin));

        //    bool allowApprove = true;

        //    btnApporvedLI.Visible = btnApporved.Visible = allowApprove;

        //    // Return allowed for managers and resource admins if timesheet approved or in pending approval mode
        //    // bool allowReturnTimeSheet = (TimesheetMode == Constants.ApprovalMode.ToLower() && (status == Constants.PendingApproval || status == Constants.Approved) && (isUserManager || isResourceAdmin));

        //    bool allowReturnTimeSheet = true;

        //    btnReturnTimeSheetLI.Visible = btnReturnTimeSheet.Visible = allowReturnTimeSheet;

        //    // Users can only send their own timesheets for approval, and only if not already sent for approval or approved
        //    bool allowSendForApproval = TimesheetMode == Constants.ApprovalMode.ToLower() && (status == Constants.TimeEntry || status == Constants.Returned) &&
        //                                HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id;
        //    btnSendApprovalLI.Visible = btnSendApproval.Visible = allowSendForApproval;

        //    // Users can only signoff on their own timesheets, and only if not already signed off
        //    bool allowSignOff = TimesheetMode == Constants.SignOffMode.ToLower() && status == Constants.TimeEntry && HttpContext.Current.CurrentUser().Id == currentSelectedUser.Id;
        //    btnSignOffLI.Visible = btSignOff.Visible = allowSignOff;

        //    //set visibility of subordinate's status and pendingStatus icons
        //    // statusPicker.Visible = isManager || isResourceAdmin;
        //    statusPicker.Visible = true;

        //    //   pendingStatusPicker.Visible = (TimesheetMode == Constants.ApprovalMode.ToLower() && (isManager || isResourceAdmin));
        //    pendingStatusPicker.Visible = (TimesheetMode == Constants.ApprovalMode.ToLower());

        //    timeSheetStatus.Text = status;
        //    lblPrintStatus.Text = status;

        //    //Set color for TimeSheet status label on the basis of it's value
        //    if (status == Constants.TimeEntry)
        //    {
        //        timeSheetStatus.ForeColor = System.Drawing.Color.DarkBlue;
        //        lblPrintStatus.ForeColor = System.Drawing.Color.DarkBlue;
        //    }
        //    else if (status == Constants.Returned)
        //    {
        //        timeSheetStatus.ForeColor = System.Drawing.Color.Red;
        //        lblPrintStatus.ForeColor = System.Drawing.Color.Red;
        //    }
        //    else if (status == Constants.PendingApproval)
        //    {
        //        // timeSheetStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml(rustColorCode);
        //        timeSheetStatus.ForeColor = System.Drawing.Color.Green;

        //        // lblPrintStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml(rustColorCode);
        //        lblPrintStatus.ForeColor = System.Drawing.Color.Green;

        //    }
        //    else if (status == Constants.Approved || status == Constants.SignOff)
        //    {
        //        timeSheetStatus.ForeColor = System.Drawing.Color.Green;
        //        lblPrintStatus.ForeColor = System.Drawing.Color.Green;
        //    }
        //}
        //#endregion Method to set accessibility of Time Sheet action control buttons

        #region Method to Get Status of the Time Sheet
        /// <summary>
        ///// This method is used to get the Status of the time sheet for a particular week for a user
        ///// </summary>
        //protected string GetSignOffStatus(DateTime weekStartDate, UserProfile selectedUser)
        //{
        //    string timesheetStatus = Constants.TimeEntry; // Status defaults to TimeEntry
        //    DateTime weekEndDate = weekStartDate.EndOfWeek(DayOfWeek.Sunday);

        //    List<string> requiredQuery = new List<string>();
        //    requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value IncludeTimeValue='FALSE' Type='DateTime'>{1}</Value></Eq>", DatabaseObjects.Columns.UGITStartDate, weekStartDate.ToString("yyyy-MM-dd")));
        //    requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value IncludeTimeValue='FALSE' Type='DateTime'>{1}</Value></Eq>", DatabaseObjects.Columns.UGITEndDate, weekEndDate.ToString("yyyy-MM-dd")));
        //    requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'  /><Value Type='User'>{1}</Value></Eq>", DatabaseObjects.Columns.Resource, selectedUser.ID));

        //    if (spSignOffList != null && spSignOffList.ItemCount > 0)
        //    {
        //        SPQuery sQuery = new SPQuery();
        //        sQuery.ViewFields = string.Format("<FieldRef Name= '{0}' /><FieldRef Name= '{1}' />", DatabaseObjects.Columns.SignOffStatus, DatabaseObjects.Columns.History);
        //        sQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
        //        SPListItemCollection resultCollection = spSignOffList.GetItems(sQuery);
        //        if (resultCollection != null && resultCollection.Count > 0)
        //        {
        //            SPListItem item = resultCollection[0];
        //            timesheetStatus = Convert.ToString(item[DatabaseObjects.Columns.SignOffStatus]);
        //            signOffItemId = item.ID;
        //        }
        //    }
        //    return timesheetStatus;
        //}
        //#endregion Method to Get Sign Off Status of the Time Sheet


        //#region Method to change the Time Sheet SignOff Status
        ///// <summary>
        ///// This method is used to Method to change the Time Sheet SignOff Status
        ///// </summary>
        //protected void btnSignOffTimeSheet_Click(object sender, EventArgs e)
        //{
        //    string actionValue = hdnActionValue.Value;

        //    if (!string.IsNullOrEmpty(actionValue))
        //    {
        //        if (actionValue == Constants.Return)
        //        {
        //            ReturnTimeSheet();
        //        }
        //        else if (actionValue == Constants.SendForApproval)
        //        {
        //            SendTimeSheetForApproval();
        //        }
        //        else if (actionValue == Constants.Approved)
        //        {
        //            ApproveTimeSheet();
        //        }
        //        else if (actionValue == Constants.SignOff)
        //        {
        //            SignOffTimesheet();
        //        }
        //        hdnActionValue.Value = "";
        //    }
        //    else
        //    {
        //        return;
        //    }

        //}
        //#endregion Method to change the Time Sheet SignOff Status
        #endregion

        #region Maintain State of Expand and Collapse After Postback  
        public class TimeSheetStateMaintain
        {
            public string user { get; set; }
            public string workItem { get; set; }
        }

        public string setClassForTr(DataRowView row)
        {
            string subWorkItem = Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]);
            string workItem = Convert.ToString(row[DatabaseObjects.Columns.WorkItem]);
            //subWorkItem = "Total";
            string css = string.Empty;
            if (subWorkItem == "Total")
            {
                css += "Total Parent_" + workItem;

            }
            else
            {
                css += "Child_" + workItem;
                /*
                if (rTimeSheetState != null && rTimeSheetState.Count > 0)
                {
                    if (rTimeSheetState.Where(x => x.workItem == css && x.user == Convert.ToString(cmbCurrentUser.Value)).ToList().Count > 0)
                        css += " hideRow";
                }
                */
            }

            return css;

        }

        public string SetExpandCollapseIcon()
        {
            string subWorkItem = Convert.ToString(Eval(DatabaseObjects.Columns.SubWorkItem));
            string workItem = Convert.ToString(Eval(DatabaseObjects.Columns.WorkItem));

            bool isExpanded = true;
            // further have to remove;
            //subWorkItem = "Total";
            if (subWorkItem == "Total")
            {
                if (rTimeSheetState != null && rTimeSheetState.Count > 0)
                {
                    if (rTimeSheetState.Where(x => x.workItem == "Child_" + workItem && x.user == Convert.ToString(cmbCurrentUser.Value)).ToList().Count > 0)
                        isExpanded = true;
                }

                string icon = "plus_16x16.png";
                if (isExpanded)
                    icon = "minus_16x16.png";

                return string.Format("<img style='float:left; width:14px; ' onclick='childClick(this)' src='/Content/Images/{0}'/>", icon);
            }
            return string.Empty;
        }

        public string SetWorkItemIcon()
        {
            string subWorkItem = Convert.ToString(Eval(DatabaseObjects.Columns.SubWorkItem));
            string workItemType = Convert.ToString(Eval(DatabaseObjects.Columns.Type));
            string workItem = Convert.ToString(Eval(DatabaseObjects.Columns.WorkItem));
            if (subWorkItem == "Total")
            {
                string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "Actuals", Convert.ToString(cmbCurrentUser.Value), startDate, workItemType, workItem));
                string path = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','600px','350px',0,'{1}')", url, Uri.EscapeDataString(Request.Url.AbsoluteUri), formTitle);
                return $"<img style='width:14px;cursor:pointer;' onclick=\"{path}\" src='/Content/Images/new_task.png' title='Add WorkItem' />";
            }
            return string.Empty;
        }

        #endregion

        /// <summary>
        /// This method is used to save the comment for each work item for each value
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void pcAddComment_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter) && !string.IsNullOrEmpty(hdnCommentListName.Value))
            {
                if (hdnCommentListName.Value == DatabaseObjects.Tables.TicketHours)
                {
                    TicketHoursManager ticketHourMGR = new TicketHoursManager(context);
                    ActualHour ticketHourObj = ticketHourMGR.LoadByID(Convert.ToInt64(e.Parameter));
                    if (ticketHourObj != null)
                    {
                        if (hdnCommentListName.Value == DatabaseObjects.Tables.TicketHours)
                            ticketHourObj.Comment = txtComment.Text.Trim();
                        ticketHourMGR.Update(ticketHourObj);
                    }
                }
                else if (hdnCommentListName.Value == DatabaseObjects.Tables.ResourceTimeSheet)
                {
                    ResourceTimeSheet work = ObjResourceTimeSheetManager.LoadByID(Convert.ToInt64(e.Parameter));
                    if (work != null)
                    {
                        work.WorkDescription = txtComment.Text.Trim();
                        ObjResourceTimeSheetManager.Save(work);
                    }
                }
                //SPListItem item = SPListHelper.GetSPListItem(hdnCommentListName.Value, UGITUtility.StringToInt(Convert.ToString(e.Parameter)));

                //if (item != null)
                //{
                //    if (hdnCommentListName.Value == DatabaseObjects.Tables.TicketHours)
                //        item[DatabaseObjects.Columns.TicketComment] = txtComment.Text.Trim();
                //    else
                //        item[DatabaseObjects.Columns.WorkDescription] = txtComment.Text.Trim();
                //    item.UpdateOverwriteVersion();
                //}
            }

            pcAddComment.ShowOnPageLoad = false;

            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("startDate", startDate.ToString("MM dd yyyy"));
            queryCollection.Set("UId", Convert.ToString(cmbCurrentUser.SelectedItem.Value));
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            ASPxWebControl.RedirectOnCallback(listUrl);

        }

        #region Method to Notify Manager if TimeSheet is updated after Approved
        /// <summary>
        /// This method to notify Manager if TimeSheet is updated after Approved
        /// </summary>
        protected void btnNotifySave_Click(object sender, EventArgs e)
        {
            BindTimeSheetDetails();
            DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

            if (!string.IsNullOrEmpty(status) && (status == Constants.Approved || status == Constants.SignOff) && currentLoggedInuser.Id == currentSelectedUser.Id)
            {
                if (status == Constants.Approved)
                    isSendForApprovalCalled = true;
                else if (status == Constants.SignOff)
                    isTimeEntryCalled = true;

                //Update TimeSheetSignOff Status
                status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);

                if (status == Constants.PendingApproval && !isSendForApprovalCalled)
                {
                    btnSendApprovalLI.Visible = btnSendApproval.Visible = false;
                    timeSheetStatus.Text = lblPrintStatus.Text = Constants.PendingApproval;
                    timeSheetStatus.ForeColor = lblPrintStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml(rustColorCode);
                    btnApporvedLI.Visible = btnApporved.Visible = isResourceAdmin;
                }
                else if (status == Constants.TimeEntry && !isTimeEntryCalled)
                {
                    btnSignOffLI.Visible = btSignOff.Visible = true;
                    timeSheetStatus.Text = lblPrintStatus.Text = Constants.TimeEntry;
                    timeSheetStatus.ForeColor = lblPrintStatus.ForeColor = System.Drawing.Color.DarkBlue;
                }

                if (!isSendForApprovalCalled && !isTimeEntryCalled && currentLoggedInuser.ManagerID != null /*&& !string.IsNullOrEmpty(currentLoggedInuser.Manager.Email)*/)
                {
                    // Prasad: Updated email not needed since another email will go out anyway after they SignOff on the edit
                    //string informTo = currentLoggedInUser.Manager.Name;
                    //string mailTo = currentLoggedInUser.Manager.Email;
                    //SendEmail(informTo, mailTo, Constants.Updated, signOffWeekStartDate);
                }
                else
                {
                    isSendForApprovalCalled = isTimeEntryCalled = false;
                }
            }
            else if (TimesheetMode == Constants.SignOffMode.ToLower() && status == Constants.TimeEntry && currentLoggedInuser == currentSelectedUser)
            {
                string currentStatus = GetCurretSignOffStatus(signOffWeekStartDate, currentSelectedUser);

                if (!string.IsNullOrEmpty(currentStatus) && currentStatus == status)
                    return;

                addTimeEntry = true;

                // Update TimeSheetSignOff Status
                status = UpdateSignOffStatus(signOffWeekStartDate, currentSelectedUser);

                if (status == Constants.TimeEntry && !addTimeEntry)
                {
                    btnSignOffLI.Visible = btSignOff.Visible = true;
                    timeSheetStatus.Text = lblPrintStatus.Text = Constants.TimeEntry;
                    timeSheetStatus.ForeColor = lblPrintStatus.ForeColor = System.Drawing.Color.DarkBlue;

                    // Prasad: Updated email not needed since another email will go out anyway after they SignOff on the edit
                    //if (currentLoggedInUser.Manager != null && !string.IsNullOrEmpty(currentLoggedInUser.Manager.Email))
                    //{
                    //    string informTo = currentLoggedInUser.Manager.Name;
                    //    string mailTo = currentLoggedInUser.Manager.Email;
                    //    SendEmail(informTo, mailTo, Constants.Updated, signOffWeekStartDate);
                    //}
                }
                else
                {
                    addTimeEntry = false;
                }
            }
        }
        #endregion Method to Notify Manager/Subordinate if TimeSheet is updated after SendForApproval or Approved

        protected void btRefreshPage_Click(object sender, EventArgs e)
        {
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("startDate", UGITUtility.StringToDateTime(startDate).ToString("MM dd yyyy"));
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void btnLoadTimeSheet_Click(object sender, EventArgs e)
        {
            if (!hdnAllowCallBack.Contains("Result"))
                return;

            string[] resultSet = hdnAllowCallBack["Result"].ToString().Split(';');

            hdnAllowCallBack.Remove("Result");

            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("startDate", UGITUtility.StringToDateTime(resultSet[1]).ToString("MM dd yyyy"));
            queryCollection.Set("UId", resultSet[0]);
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }
    }
}
