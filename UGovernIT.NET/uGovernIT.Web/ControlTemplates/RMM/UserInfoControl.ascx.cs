using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DevExpress.Web;
using System.Linq;
using uGovernIT.Manager;
using System.DirectoryServices;
using System.IO;
using uGovernIT.Utility;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web.UI;
using System.Globalization;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Web.ControlTemplates.GlobalPage;


namespace uGovernIT.Web
{
    public partial class UserInfoControl : System.Web.UI.UserControl
    {
        DataTable userInfoList;
        // SPList userInfoList;
        UserRoleManager rolemanagers = new UserRoleManager(HttpContext.Current.GetManagerContext());
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

        protected string userID;
        string newADUser;
        public UserProfile userInfo = null;
        string groupID;
        //bool newUser = false;

        bool isSiteAdmin = false;
        bool isResourceAdmin = false;
        bool isResourceManager = false;

        //private SPGroup userGroup = null;
        private List<Role> userGroup = null;
        // DepartmentDropdownList departmentCtr = null;
        string defaultUserRole = string.Empty;
        public string jsonSkillData = string.Empty;
        public string ajaxHelperURL = string.Empty;

        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&listName={1}&Module=CMDB&UserId={2}";
        private string newParam = "userprofilerelatedassets";
        public string assetUrl = string.Empty;
        public string departmentLabel;
        public bool olderOutOfOfficeValue;
        public UserLookupValue delegateUser;
        public DateTime leaveFrom;
        public DateTime leaveTo;
        public bool sendNotification;
        bool disableChangePassword;
        public string openCloseTicketsForRequestorUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=openandcloseforrequestor");
        public bool IsEnable { get; set; }
        public string absoluteURL(string s)
        {
            string url = "";
            return url = UGITUtility.GetAbsoluteURL(s);
        }
        public string SetValueCheck { get; set; }
        ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        private void showControlsForNewUsers()
        {
            if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateNewUser) && objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateFBAUser))
            {
                trTypeofUser.Visible = true;
            }
            tr0.Visible = false;
            trReenterPassword.Visible = true;
            trPasword.Visible = true;
            trLoginName.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;
            //tr4.Visible = true;
            tr5.Visible = true;
            tr6.Visible = true;
            tr7.Visible = true;
            tr8.Visible = true;
            tr9.Visible = true;
            tr10.Visible = true;
            tr11.Visible = true;
            tr12.Visible = true;
            tr13.Visible = true;
            tr14.Visible = true;
            tr15.Visible = true;
            tr16.Visible = true;
            tr17.Visible = false;
            tr18.Visible = true;
            tr21.Visible = true;
            tr51.Visible = true;
            txtName.Visible = true;
            txtEmail.Visible = true;
            txtMobileNumber.Visible = true;
            departmentCtr.Visible = true;
            // ctDepartment.Visible = true;
            txtJobTitle.Visible = true;
            txtHourlyRate.Visible = true;
            //peAssignedTo.Visible = true;
            cbIT.Visible = true;
            cbIsConsultant.Visible = true;
            cbIsManager.Visible = true;
            ddlBudgetCategory.Visible = true;
            ddlSubBudgetCategory.Visible = true;
            glLocation.Visible = true;
            ddlRole.Visible = true;
            ddlFunctionalArea.Visible = true;
            // glSkills.Visible = true;
            chkEnable.Visible = true;
            txtDeskLocation.Visible = true;
            //dtcStartDate.Visible = true;
            //dtcEndDate.Visible = true;

            btnResetPassword.Visible = false;

            chkEnablePwdExpiration.Visible = true;
            lbEnablePwdExpiration.Visible = false;
            lbPwdExpiryDate.Visible = false;
            //dtcPwdExpiryDate.Visible = true;

            chkDisableWorkflowNotifications.Visible = true;
            lblDisableWorkflowNotifications.Visible = false;

            tr24.Visible = true;
            txtNotificationEmail.Visible = true;

            tr25.Visible = true;
            lbApproveLevelAmount.Visible = false;
            string sysUIFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            dtWorkingHoursStart.Value = Convert.ToDateTime(DateTime.Today.ToString(sysUIFormat).ToString() + " 09:00:00");
            dtWorkingHoursEnd.Value = Convert.ToDateTime(DateTime.Today.ToString(sysUIFormat).ToString() + " 18:00:00");

        }

        protected override void OnInit(EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/api/Account/");
            // departmentCtr = (DepartmentDropdownList)ctDepartment;
            //departmentCtr.IsMulti = false;
            //departmentCtr.EnablePostBack = false;
            userInfoList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            isSiteAdmin = umanager.IsRole(RoleType.Admin, Page.User.Identity.Name.ToString());
            isResourceAdmin = isSiteAdmin || umanager.IsRole(RoleType.ResourceAdmin, Page.User.Identity.Name.ToString());
            DDLLocation_Load();
            DDLBudgetCategories_Load();
            disableChangePassword = true;
            // Case when user id comes in request to update the user's property.
            if (Request.QueryString.AllKeys.Contains("uID"))
            {
                userID = Request["uID"];
            }
            if (!IsPostBack)
            {
                if (Request["uID"] != null)
                {
                    userID = Request["uID"];
                    newADUser = Request["newUser"];
                    if (newADUser == "1")
                    {

                        dtcStartDate.Value = DateTime.Today;
                        dtcEndDate.Value = DateTime.Today.AddYears(1);
                        jsonSkillData = "[]";
                        viewPanel.Visible = true;
                        showControlsForNewUsers();
                        return;
                    }
                    userInfo = umanager.GetUserById(userID);
                    if (!string.IsNullOrEmpty(userID) || userInfo == null)
                    {

                        viewPanel.Visible = true;
                        OpenUserProfile(e);
                        return;
                    }
                }
                // Case when group id comes in request to add user in the group.
                else if (Request["gID"] != null)
                {
                    groupID = Request["gID"];
                    {
                        userGroup = rolemanagers.GetRoleList().Where(x => x.Id == groupID).ToList();
                        if (string.IsNullOrEmpty(groupID) || userGroup == null)
                        {
                            viewPanel.Visible = false;
                            return;
                        }
                        //newUser = true;
                        OpenNewForm();
                    }
                }
            }
            assetUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, DatabaseObjects.Tables.Assets, userID));
            //lnkAssets.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", assetUrl, Server.UrlEncode(Request.Url.AbsolutePath), "Assets Details");
            //base.OnPreInit(e);
        }

       

        private void LoadAssetsForParticularUser()
        {
            if (userID != null)
            {


            }
        }

        private void OpenUserProfile(EventArgs e)
        {
            tr24.Visible = true;
            tr25.Visible = true;

            if (!isResourceAdmin)
            {
                List<UserProfile> userProfile = umanager.GetUsersProfile();
                if (userProfile.Exists(x => x.Id == userID))
                {
                    isResourceManager = true;
                }
            }

            if (!(isResourceAdmin || isResourceManager))
            {
                // Read-only mode
                UserProfile userProfile = umanager.GetUserInfoById(userID);
                if (userProfile != null && userProfile.Id == userID)
                {
                    btnResetPassword.Visible = !disableChangePassword;
                }
                else
                {
                    btnResetPassword.Visible = !disableChangePassword && (isResourceManager || isResourceAdmin);
                    btnDelete.Visible = isResourceManager || isResourceAdmin;
                }

                tr6.Visible = false;

                btSave.Visible = false;
                btCancel.Text = string.Format("<span class='button-bg'><b style='float: left; font-weight: normal;'>Close</b><i style='float: left; position: relative; top: -3px;left:2px'><img src='/Content/images/uGovernIT/ButtonImages/cancelwhite.png'  style='border:none;' title='' alt=''/></i> </span>");

                // lbAccount.Text = userInfo.UserName;
                lbName.Text = userInfo.Name;
                lbEmail.Text = userInfo.Email;
                lbNotificationEmail.Text = userInfo.NotificationEmail;
                lbJobTitle.Text = userInfo.JobProfile;
                lbMobileNumber.Text = userInfo.MobilePhone;
                lbHourlyRate.Text = string.Format("${0}", userInfo.HourlyRate);

                txtApproveLevelAmount.Visible = false;
                lbApproveLevelAmount.Visible = true;
                lbApproveLevelAmount.Text = string.Format("${0}", userInfo.ApproveLevelAmount);
                lbLocation.Text = userInfo.Location;

                if (!string.IsNullOrEmpty(Convert.ToString(userInfo.Department)))
                {
                    departmentCtr.Value = userInfo.Department;
                    // departmentCtr.ControlMode = ControlMode.Display;
                    departmentCtr.SetValues(departmentCtr.Value);
                }
                // lblEnable.Text = UGITUtility.StringToBoolean(userInfo.Enabled).ToString();
                //Keep old value
                IsEnable = userInfo.Enabled;
                //set DisableWorkflowNotifications.
                lblDisableWorkflowNotifications.Text = UGITUtility.StringToBoolean(userInfo.DisableWorkflowNotifications).ToString();

                tr43.Visible = true;

                dtWorkingHoursStart.Visible = false;
                dtWorkingHoursEnd.Visible = false;
                lblWorwkingHoursStart.Visible = true;
                lblWorkingHoursEnd.Visible = true;
                tospan.Visible = false;

                if (string.IsNullOrEmpty(Convert.ToString(userInfo.WorkingHoursStart)))
                {
                    lblWorwkingHoursStart.Text = Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayStartTime")).ToString("hh:mm:tt");
                    lblWorkingHoursEnd.Text = "To " + Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayEndTime")).ToString("hh:mm:tt");
                }
                else
                {
                    lblWorwkingHoursStart.Text = userInfo.WorkingHoursStart.ToString();
                    lblWorkingHoursEnd.Text = "To " + userInfo.WorkingHoursEnd.ToString();
                }
                if (userInfo.FunctionalArea != null)
                    lblFunctionalArea.Text = GetTableDataManager.GetTableData(DatabaseObjects.Tables.FunctionalAreas, "id=" + userInfo.FunctionalArea).Rows[0][DatabaseObjects.Columns.Title].ToString();

                //if (userInfo.Skills.Count > 0)
                //    lblSkills.Text = string.Join("; ", userInfo.Skills.Select(x => x.Value).ToArray());

                skilldiv.Visible = false;
                if (userInfo.BudgetCategory != null)
                {
                    lbBudgetCategory.Text = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, "id=" + userInfo.BudgetCategory).Rows[0][DatabaseObjects.Columns.BudgetCategory].ToString();
                }

                if (userInfo.ManagerID != null)
                {
                    lbManager.Text = umanager.GetUserById(userInfo.ManagerID).Name;
                }
                lbManager.Visible = true;
                //peAssignedTo.Visible = false;
                lbIsConsultant.Text = "No";
                if (userInfo.IsConsultant)
                {
                    lbIsConsultant.Text = "Yes";
                }

                lbIsManager.Text = "No";
                if (userInfo.IsManager)
                {
                    lbIsManager.Text = "Yes";
                }

                lbIT.Text = "No";
                if (userInfo.IsIT)
                {
                    lbIT.Text = "Yes";
                }
                lblDeskLocation.Text = userInfo.DeskLocation;
                lblStartDate.Text = (userInfo.UGITStartDate).ToShortDateString();
                lblEndDate.Text = (userInfo.UGITEndDate).ToShortDateString();

                lbEnablePwdExpiration.Visible = true;
                chkEnablePwdExpiration.Visible = false;
                chkEnablePwdExpiration.Checked = userInfo.EnablePasswordExpiration;
                chkEnablePwdExpiration_CheckedChanged(chkEnablePwdExpiration, new EventArgs());
                lbEnablePwdExpiration.Text = "Off";
                if (chkEnablePwdExpiration.Checked)
                {
                    lbEnablePwdExpiration.Text = "On";
                    int passwordExpirationPeriod = 0; ;
                    int.TryParse(objConfigurationVariableHelper.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
                    if (passwordExpirationPeriod > 0)
                        passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;
                    lbExpirationPeriod.Text = string.Format("Expiration Period: {0} days", passwordExpirationPeriod);
                }

                lbPwdExpiryDate.Visible = true;
                dtcPwdExpiryDate.Visible = false;
                if (userInfo.PasswordExpiryDate != DateTime.MinValue)
                {
                    dtcPwdExpiryDate.Value = userInfo.PasswordExpiryDate;
                }

                //new entries for "out of office calender" to show data start
                tr66.Visible = true;
                tr60.Visible = true;
                lblEnableOutOfOffice.Visible = true;
                chkOutOfOffice.Visible = false;
                outOfOfficePanelEdit.Visible = false;
                lblEnableOutOfOffice.Text = "Off";

                //To show user dalegated for
                if (userInfo.DelegateUserFor != null)
                {
                    //lblDelegatedTaskFor.Text = string.Join("; ", userInfo.DelegateUserFor.Select(x => x.Name));
                    delgateUserForPanelRead.CssClass = "readdegatefor";
                }

                if (!string.IsNullOrEmpty(userInfo.Id))
                {
                    lblDisableWorkflowNotifications.Visible = false;
                    chkDisableWorkflowNotifications.Visible = true;
                    chkDisableWorkflowNotifications.Checked = userInfo.DisableWorkflowNotifications;

                    chkOutOfOffice.Visible = true;
                    outOfOfficePanelEdit.Visible = true;
                    outOfOfficePanelRead.Visible = false;
                    lblEnableOutOfOffice.Visible = false;
                    chkOutOfOffice.Checked = userInfo.EnableOutofOffice;
                    //Keep old value of out of office
                    //delegateUser = userInfo.DelegateUserOnLeave;
                    olderOutOfOfficeValue = userInfo.EnableOutofOffice;
                    leaveFrom = userInfo.LeaveFromDate;
                    leaveTo = userInfo.LeaveToDate;
                    btSaveOwnProfile.Visible = true;
                    if (chkOutOfOffice.Checked)
                        outOfOfficePanelEdit.Style.Add("display", "block");

                    if (!string.IsNullOrEmpty(Convert.ToString(userInfo.LeaveFromDate)))
                    {
                        LeaveFromDate.Value = userInfo.LeaveFromDate;
                    }
                    else
                    {
                        //LeaveFromDate.Value = DateTime.Now;
                    }

                    //LeavetoDate.SelectedDate = userInfo.LeaveToDate;
                    if (userInfo.DelegateUserOnLeave != null)
                    {
                        //DelegateUserOnLeave.CommaSeparatedAccounts = userInfo.DelegateUserOnLeave.LoginName;
                    }
                }
                else
                {
                    if (userInfo.EnableOutofOffice)
                    {
                        lblEnableOutOfOffice.Text = "On";
                        lblEnableOutOfOffice.CssClass = "on";
                        outOfOfficePanelRead.Visible = true;
                        //lblLeaveDate.Text = string.Format("From {0} to {1}", uHelper.GetDateStringInFormat(userInfo.LeaveFromDate, false), uHelper.GetDateStringInFormat(userInfo.LeaveToDate, false));
                        if (userInfo.DelegateUserOnLeave != null)
                            lblDelegateUserOnLeave.Text = string.Format("Delegated task to: {0}", userInfo.DelegateUserOnLeave);
                    }
                }

                //new entries for "out of office calender" to show data start

                tr51.Visible = true;
            }
            else // either resource admin or manager of this resource, allow editing
            {
                bool enablePasswordReset = !disableChangePassword && objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableADPasswordReset);

                // Only resource admins can add users or reset password
                btnResetPassword.Visible = enablePasswordReset && isResourceAdmin;
                btnDelete.Visible = isResourceAdmin;

                //lbAccount.Text = userInfo.Name;

                lbName.Visible = false;
                if (userInfo.BudgetCategory.HasValue)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(userInfo.BudgetCategory)))
                    {
                        DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, "id=" + userInfo.BudgetCategory);
                        if (dt != null)
                        {
                            ddlSubBudgetCategory.Items.Clear();
                            DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories);
                            if (budgets != null && budgets.Rows.Count > 0)
                            {
                                DataRow[] budgetSubCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategory, dt.Rows[0]["BudgetCategory"]).ToString());
                                foreach (DataRow row in budgetSubCategories)
                                {
                                    ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                                }
                                ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                                ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(userInfo.BudgetCategory)).Selected = true;
                                DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                                foreach (DataRow row in budgetCategories.Rows)
                                {
                                    ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategory]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategory])));
                                }
                                ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                                ddlBudgetCategory.Items.FindByText(dt.Rows[0]["BudgetCategory"].ToString()).Selected = true;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Convert.ToString(userInfo.FunctionalArea)))
                {
                    ddlFunctionalArea.SetValues(Convert.ToString(userInfo.FunctionalArea));
                }
                txtName.Visible = true;
                txtName.Text = userInfo.Name;
                lbEmail.Visible = false;
                txtEmail.Visible = true;
                txtEmail.Text = userInfo.Email;
                lbNotificationEmail.Visible = false;
                txtNotificationEmail.Visible = true;
                txtNotificationEmail.Text = userInfo.NotificationEmail;
                lbApproveLevelAmount.Visible = false;
                txtApproveLevelAmount.Visible = true;
                txtApproveLevelAmount.Text = userInfo.ApproveLevelAmount.ToString();
                lbMobileNumber.Visible = false;
                txtMobileNumber.Visible = true;
                txtMobileNumber.Text = userInfo.PhoneNumber;
                lbLocation.Visible = false;
                glLocation.Visible = true;
                if (userInfo.Location != null)
                {
                    glLocation.Value = userInfo.Location;
                    glLocation.GridView.Selection.SelectRowByKey(userInfo.Location);
                }

                managerUser.SetValues(userInfo.ManagerID);

                if (!IsPostBack)
                    if (!string.IsNullOrEmpty(userInfo.Department))
                    {

                        departmentCtr.SetValues(Convert.ToString(userInfo.Department));
                        // departmentCtr.ControlMode = ControlMode.Edit;
                    }
                lblRole.Visible = false;
                ddlRole.Visible = true;
                lbJobTitle.Visible = false;
                txtJobTitle.Visible = true;
                txtJobTitle.Text = userInfo.JobProfile;
                //new entries for "out of office calender" to show data start
                tr66.Visible = true;
                if (userInfo.DelegateUserFor != null)
                {
                    //lblDelegatedTaskFor.Text = string.Join("; ", userInfo.DelegateUserFor.Select(x => x.Name));
                    delgateUserForPanelRead.CssClass = "readdegatefor";
                }
                tr60.Visible = true;
                lblEnableOutOfOffice.Visible = false;
                chkOutOfOffice.Visible = true;
                outOfOfficePanelEdit.Visible = true;
                outOfOfficePanelRead.Visible = false;
                chkOutOfOffice.Checked = userInfo.EnableOutofOffice;
                tr43.Visible = true;
                dtWorkingHoursStart.Visible = true;
                dtWorkingHoursEnd.Visible = true;
                lblWorwkingHoursStart.Visible = false;
                lblWorkingHoursEnd.Visible = false;
                if (string.IsNullOrEmpty(Convert.ToString(userInfo.WorkingHoursStart)))
                {
                    dtWorkingHoursStart.NullText = objConfigurationVariableHelper.GetValue("WorkdayStartTime");
                    dtWorkingHoursEnd.NullText = objConfigurationVariableHelper.GetValue("WorkdayEndTime");
                    dtWorkingHoursStart.Value = Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayStartTime"));
                    dtWorkingHoursEnd.Value = Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayEndTime"));
                }
                else
                {
                    dtWorkingHoursStart.Value = userInfo.WorkingHoursStart;
                    dtWorkingHoursEnd.Value = userInfo.WorkingHoursEnd;
                }
                //Keep old value of out of office
                olderOutOfOfficeValue = userInfo.EnableOutofOffice;
                //delegateUser = userInfo.DelegateUserOnLeave;
                leaveFrom = userInfo.LeaveFromDate;
                leaveTo = userInfo.LeaveToDate;
                if (userInfo.LeaveFromDate != DateTime.MinValue)
                {
                    LeaveFromDate.Value = !string.IsNullOrEmpty(Convert.ToString(userInfo.LeaveFromDate)) ? userInfo.LeaveFromDate : DateTime.MinValue;
                }
                else
                {
                    LeaveFromDate.Value = DateTime.Now;
                }

                if (chkOutOfOffice.Checked)
                {
                    outOfOfficePanelEdit.Style.Add("display", "block");
                    LeaveFromDate.Value = userInfo.LeaveFromDate;
                    LeavetoDate.Value = userInfo.LeaveToDate;
                    if (userInfo.DelegateUserOnLeave != null)
                    {
                        // DelegateUserOnLeave.CommaSeparatedAccounts = userInfo.DelegateUserOnLeave.LoginName;
                    }
                }

                //new entries for "out of office calender" to show data end
                lbHourlyRate.Visible = false;
                txtHourlyRate.Visible = true;
                txtHourlyRate.Text = userInfo.HourlyRate.ToString();
                txtLoginName.Visible = false;
                lblUserName.Text = userInfo.UserName;
                lbManager.Visible = false;

                string uRoleValue = "";
                List<string> iRoleList = umanager.GetRoles(userID).ToList();
                if (iRoleList.Count > 0)
                {
                    foreach (string listname in iRoleList)
                    {
                        uRoleValue += rolemanagers.GetRoleByName(listname).Id + ",";
                    }
                }
                if (!string.IsNullOrEmpty(uRoleValue))
                {
                    string[] val = uRoleValue.Split(',');
                    if (val.Length > 1)
                    {
                        ddlRole.SetValues(uRoleValue.Substring(0, uRoleValue.Length - 1).ToString());
                    }
                    else
                    {
                        ddlRole.SetValues(uRoleValue);
                    }
                }

                lbBudgetCategory.Visible = false;
                ddlBudgetCategory.Visible = true;
                ddlSubBudgetCategory.Visible = true;
                if (userInfo.BudgetCategory != null && !IsPostBack)
                {
                }
                lbIsConsultant.Visible = false;
                cbIsConsultant.Visible = true;
                cbIsConsultant.Checked = userInfo.IsConsultant;
                lbIsManager.Visible = false;
                cbIsManager.Visible = true;
                cbIsManager.Checked = userInfo.IsManager;
                lbIT.Visible = false;
                cbIT.Visible = true;
                cbIT.Checked = userInfo.IsIT;
                lblEnable.Visible = false;
                chkEnable.Visible = true;
                chkEnable.Checked = UGITUtility.StringToBoolean(userInfo.Enabled);
                lblFunctionalArea.Visible = false;
                ddlFunctionalArea.Visible = true;
                lblDisableWorkflowNotifications.Visible = false;
                chkDisableWorkflowNotifications.Visible = true;
                chkDisableWorkflowNotifications.Checked = userInfo.DisableWorkflowNotifications;
                txtDeskLocation.Visible = true;
                txtDeskLocation.Text = userInfo.DeskLocation;
                dtcEndDate.Visible = true;
                dtcStartDate.Visible = true;
                dtcStartDate.Value = userInfo.UGITStartDate;
                dtcEndDate.Value = userInfo.UGITEndDate;
                glLocation.GridView.Selection.SetSelectionByKey(userInfo.Location, true);

                //new lines for modification of skill code..
                List<UserSkill> listofuserskill = new List<UserSkill>();
                if (userInfo.Skills != null)
                {
                    //foreach (LookupValue lVal in userInfo.Skills)
                    //{
                    //    //listofuserskill.Add(new UserSkill() { id = lVal.ID, name = lVal.Value });
                    //}
                    jsonSkillData = Newtonsoft.Json.JsonConvert.SerializeObject(listofuserskill);
                }
                else
                {
                    jsonSkillData = "[]";
                }
                lbEnablePwdExpiration.Visible = false;
                chkEnablePwdExpiration.Visible = true;
                chkEnablePwdExpiration.Checked = userInfo.EnablePasswordExpiration;
                chkEnablePwdExpiration_CheckedChanged(chkEnablePwdExpiration, new EventArgs());
                lbEnablePwdExpiration.Text = "Off";
                if (chkEnablePwdExpiration.Checked)
                {
                    lbEnablePwdExpiration.Text = "On";
                    int passwordExpirationPeriod = 0; ;
                    int.TryParse(objConfigurationVariableHelper.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
                    if (passwordExpirationPeriod > 0)
                        passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;
                    lbExpirationPeriod.Text = string.Format("Expiration Period: {0} days", passwordExpirationPeriod);
                }
                lbPwdExpiryDate.Visible = false;
                dtcPwdExpiryDate.Visible = true;
                if (userInfo.PasswordExpiryDate != DateTime.MinValue)
                {
                    dtcPwdExpiryDate.Value = userInfo.PasswordExpiryDate;
                    lbPwdExpiryDate.Text = Convert.ToString(userInfo.PasswordExpiryDate);
                }
                tr51.Visible = true;
            }

            // lblUserGroups.Text = string.Join("; ", UserProfile.GetUserGroups( userInfo.ID).Select(x => x.Name).ToArray());
        }

        private void OpenNewForm()
        {
            // New Form
            // pplUserAccount.Visible = true;
            tr1.Visible = false;
            tr2.Visible = false;
            tr3.Visible = false;
            //tr4.Visible = false;
            tr5.Visible = false;
            tr6.Visible = false;
            tr7.Visible = false;
            tr8.Visible = false;
            divassetsTickets.Visible = false;
            tr10.Visible = false;
            tr11.Visible = false;
            tr12.Visible = false;
            tr13.Visible = true;
            tr16.Visible = false;
            tr15.Visible = false;
            tr14.Visible = false;
            tr17.Visible = false;
            tr18.Visible = false;
            //tr19.Visible = false;
            tr20.Visible = false;
            tr43.Visible = false;

            tr23.Visible = false;

            lblRole.Visible = false;
            ddlRole.Visible = true;

            tr9.Visible = false;
            tr21.Visible = false;
            btnResetPassword.Visible = false;

        }
        protected void BtSave_Click(object sender, EventArgs e)
        {
            string path = "";
            if (Request.QueryString.AllKeys.Contains("newUser"))
            {
                UserProfile user = new UserProfile();
                if (FileUploadUserPics.HasFile)
                {
                    path = UGITUtility.GetAbsoluteURL("Content/ProfileImages/") + Guid.NewGuid().ToString().Substring(0, 5) + FileUploadUserPics.FileName;
                    string extension = Path.GetExtension(path);
                    FileUploadUserPics.SaveAs(path);
                    user.Picture = path;
                }

                user.UserName = txtLoginName.Text.Trim();
                user.Name = txtName.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                if (!string.IsNullOrEmpty(ddlSubBudgetCategory.SelectedValue.ToString()))
                {
                    user.BudgetCategory = Convert.ToInt32(ddlSubBudgetCategory.SelectedValue.ToString());
                }
                if (!string.IsNullOrEmpty(ddlFunctionalArea.GetValues().ToString())) { user.FunctionalArea = Convert.ToInt32(ddlFunctionalArea.GetValues().ToString()); }

                user.NotificationEmail = txtNotificationEmail.Text.Trim();
                user.PhoneNumber = txtMobileNumber.Text.Trim();
                user.JobProfile = txtJobTitle.Text.Trim();
                user.HourlyRate = Convert.ToInt32(!string.IsNullOrEmpty(txtHourlyRate.Text.Trim()) ? txtHourlyRate.Text.ToString() : "0");
                user.ApproveLevelAmount = !string.IsNullOrEmpty(txtApproveLevelAmount.Text.Trim()) ? double.Parse(txtApproveLevelAmount.Text.Trim()) : 0.0;
                user.ManagerID = managerUser.GetValues();
                user.IsIT = cbIT.Checked;
                user.isRole = false;
                user.IsConsultant = cbIsConsultant.Checked;
                user.IsManager = cbIsManager.Checked;
                if (!string.IsNullOrEmpty(Convert.ToString(glLocation.Value))) { user.Location = glLocation.Value.ToString(); }
                if (!string.IsNullOrEmpty(Convert.ToString(dtcStartDate.Value))) { user.UGITStartDate = (DateTime)dtcStartDate.Value; }
                else { user.UGITStartDate = DateTime.Today; }
                if (!string.IsNullOrEmpty(Convert.ToString(dtcEndDate.Value))) { user.UGITEndDate = (DateTime)dtcEndDate.Value; }
                else { user.UGITEndDate = DateTime.Today.AddYears(1); }
                user.Enabled = chkEnable.Checked;
                user.EnablePasswordExpiration = chkEnablePwdExpiration.Checked;
                user.DisableWorkflowNotifications = chkDisableWorkflowNotifications.Checked;
                user.PasswordExpiryDate = !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)) ? (DateTime)dtcPwdExpiryDate.Value : DateTime.Today.AddYears(1);
                if (!string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursEnd.Value)))
                {
                    string date = DateTime.Today.ToShortDateString();
                    string time = Convert.ToDateTime(dtWorkingHoursEnd.Value.ToString()).ToString("hh:mm tt");
                    user.WorkingHoursEnd = Convert.ToDateTime(date + " " + time);
                }
                else
                {
                    string date = DateTime.Today.ToShortDateString();
                    string time = Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayEndTime")).ToString("hh:mm tt");
                    user.WorkingHoursStart = Convert.ToDateTime(date + " " + time);

                }
                if (!string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursStart.Value)))
                {
                    string date = DateTime.Today.ToShortDateString();
                    string time = Convert.ToDateTime(dtWorkingHoursStart.Value.ToString()).ToString("hh:mm tt");
                    user.WorkingHoursStart = Convert.ToDateTime(date + " " + time);
                }
                else
                {
                    string date = DateTime.Today.ToShortDateString();
                    string time = Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayStartTime")).ToString("hh:mm tt");
                    user.WorkingHoursStart = Convert.ToDateTime(date + " " + time);
                }

                if (umanager.FindById(txtLoginName.Text.Trim()) == null)
                {
                    if (!string.IsNullOrEmpty(ddlRole.GetValues().ToString()))
                    {
                        //string[] valuesList = ddlRole.GetValues().Split(',');
                        //foreach (string roleID in valuesList)
                        //{
                        //    user.Roles.Add(new IdentityUserRole() { RoleId = roleID, UserId = user.Id });
                        //}
                    }
                    IdentityResult result = umanager.Create(user, txtPassword.Text.Trim());
                    if (result.Succeeded)
                    {
                        umanager.AddClaim(user.Id, new Claim("AuthProvider", "Forms"));
                        uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('User Name already Exists.)", true);
                }
            }
            if (Request.QueryString.AllKeys.Contains("UpdateUser"))
            {
                UserProfile user = umanager.FindById(userID);
                if (FileUploadUserPics.HasFile)
                {
                    path = UGITUtility.GetAbsoluteURL("Content/ProfileImages/") + Guid.NewGuid().ToString().Substring(0, 5) + FileUploadUserPics.FileName;
                    if (!string.IsNullOrEmpty(user.Picture))
                    {
                        if (File.Exists(user.Picture))
                        {
                            File.Delete(user.Picture);
                        }
                    }
                    string extension = Path.GetExtension(path);
                    FileUploadUserPics.SaveAs(path);
                    user.Picture = path;
                }
                user.Name = txtName.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.NotificationEmail = txtNotificationEmail.Text.Trim();
                if (!string.IsNullOrEmpty(ddlSubBudgetCategory.SelectedValue))
                {
                    user.BudgetCategory = Convert.ToInt32(ddlSubBudgetCategory.SelectedValue.ToString());
                }
                user.PhoneNumber = txtMobileNumber.Text.Trim();
                user.Department = departmentCtr.GetValues();
                user.JobProfile = txtJobTitle.Text.Trim();
                user.HourlyRate = Convert.ToInt32(txtHourlyRate.Text.Trim());
                user.ApproveLevelAmount = !string.IsNullOrEmpty(Convert.ToString(txtApproveLevelAmount.Text.Trim())) ? double.Parse(txtApproveLevelAmount.Text.Trim()) : 0.0;
                user.ManagerID = managerUser.GetValues();
                user.IsIT = cbIT.Checked;
                user.isRole = false;
                if (!string.IsNullOrEmpty(Convert.ToString(glLocation.Value))) { user.Location = glLocation.Value.ToString(); }
                if (!string.IsNullOrEmpty(Convert.ToString(ddlFunctionalArea.GetValues()))) { user.FunctionalArea = Convert.ToInt32(ddlFunctionalArea.GetValues().ToString()); }
                user.IsConsultant = cbIsConsultant.Checked;
                user.EnableOutofOffice = chkOutOfOffice.Checked;
                user.IsManager = cbIsManager.Checked;
                if (!string.IsNullOrEmpty(Convert.ToString(dtcStartDate.Value))) { user.UGITStartDate = (DateTime)dtcStartDate.Value; }
                else { user.UGITStartDate = DateTime.Today; }
                if (!string.IsNullOrEmpty(Convert.ToString(dtcEndDate.Value))) { user.UGITEndDate = (DateTime)dtcEndDate.Value; }
                else { user.UGITEndDate = DateTime.Today.AddYears(1); }
                user.Enabled = chkEnable.Checked;
                user.EnablePasswordExpiration = chkEnablePwdExpiration.Checked;
                user.DisableWorkflowNotifications = chkDisableWorkflowNotifications.Checked;
                user.PasswordExpiryDate = !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)) ? (DateTime)dtcPwdExpiryDate.Value : DateTime.Today.AddYears(1);
                user.WorkingHoursEnd = !string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursEnd.Value)) ? DateTime.Parse(dtWorkingHoursEnd.Value.ToString()) : Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayEndTime"));
                user.WorkingHoursStart = !string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursStart.Value)) ? DateTime.Parse(dtWorkingHoursStart.Value.ToString()) : Convert.ToDateTime(objConfigurationVariableHelper.GetValue("WorkdayStartTime"));
                if (!string.IsNullOrEmpty(ddlRole.GetText()))
                {

                    List<string> existingRoles = umanager.GetRoles(user.Id).ToList();
                    List<string> valueList = rolemanagers.GetRoleList().Where(x => ddlRole.GetValuesAsList().Contains(x.Id)).Select(c => c.Name).ToList();
                    List<string> deleteList = existingRoles.Except(valueList).ToList();
                    List<string> addList = valueList.Except(existingRoles).ToList();
                    if (deleteList.Count > 0)
                        deleteList.ForEach(x => { umanager.DeleteUserRole(user, x); });
                    if (addList.Count > 0)
                        addList.ForEach(x => { umanager.AddUserRole(user, x); });
                }
                IdentityResult result = umanager.Update(user);
                if (result.Succeeded)
                {
                    //confirmDisablePopup.ShowOnPageLoad = true;
                    //lblinformativeMsg.Text = "Save Successfully";

                    uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
                }
            }


        }

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void RemoveUserFromSite()
        {
            //using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            //using ( = spSite.OpenWeb())
            //{
            //    spweb.AllowUnsafeUpdates = true;
            //    spweb.SiteUsers.RemoveByID(userID);
            //    spweb.AllowUnsafeUpdates = false;
            //}
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                UserProfile u = umanager.FindById(userID);
                if (!isSiteAdmin)
                {

                }
                else
                {
                    if (!umanager.isCurrentUser(u.Id)) { umanager.Delete(u); }
                    else { umanager.Delete(u); }
                    umanager.UpdateIntoCache(u);
                }
                string logMessage = string.Format("Removed user: {0} from site collection", userInfo.Name);
                //Log.WriteUGITLog( logMessage, UGITLogSeverity.Information, UGITLogCategory.UserProfile);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void DDLLocation_Load()
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DataTable locations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (locations != null && locations.Rows.Count > 0)
            {
                glLocation.GridView.HtmlRowPrepared += GlLocation_HtmlRowPrepared;
                if (locations != null)
                {
                    int groupIndex = 0;
                    if (locations.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.UGITRegion)).Length > 0)
                    {
                        glLocation.Columns[0].Visible = true;
                        ((GridViewDataTextColumn)glLocation.Columns[0]).GroupIndex = groupIndex;
                        groupIndex++;
                    }
                    if (locations.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.UGITCountry)).Length > 0)
                    {
                        glLocation.Columns[1].Visible = true;
                        ((GridViewDataTextColumn)glLocation.Columns[1]).GroupIndex = groupIndex;
                        groupIndex++;
                    }
                    if (locations.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.UGITState)).Length > 0)
                    {
                        glLocation.Columns[2].Visible = true;
                        ((GridViewDataTextColumn)glLocation.Columns[2]).GroupIndex = groupIndex;
                        groupIndex++;
                    }
                }

                glLocation.DataSource = locations;
                glLocation.DataBind();
                glLocation.GridView.Width = 350;
            }
        }

        void GlLocation_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Group)
            {
                int level = glLocation.GridView.GetRowLevel(e.VisibleIndex);
                if (level == 0)
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFD4");
                    e.Row.Style.Add(System.Web.UI.HtmlTextWriterStyle.FontWeight, "bold");
                }
                else if (level == 1)
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD4AA");
                    e.Row.Style.Add(System.Web.UI.HtmlTextWriterStyle.FontWeight, "bold");
                }
                else if (level == 2)
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4D4FF");
                    e.Row.Style.Add(System.Web.UI.HtmlTextWriterStyle.FontWeight, "bold");
                }
            }
        }
        protected void DDLBudgetCategories_Load()
        {
            if (ddlBudgetCategory.Items.Count <= 0)
            {
                DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories);
                if (budgets != null && budgets.Rows.Count > 0)
                {
                    DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                    foreach (DataRow row in budgetCategories.Rows)
                    {
                        ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName])));
                    }
                    ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                }
            }
        }

        protected void DDLBudgetCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubBudgetCategory.Items.Clear();
            DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories);
            if (budgets != null && budgets.Rows.Count > 0)
            {
                DataRow[] budgetCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategory, ddlBudgetCategory.SelectedValue));
                foreach (DataRow row in budgetCategories)
                {
                    ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                }
                ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        //The method to obtain the distringuised Name of the manager
        public static string GetManager(string Domain, string ADLogin, string ADPswd, string Login)
        {
            //Declare variables to use for storing the distringuised name 
            //and setup the filter variables
            string dn = "";
            string trace = "";
            string filter = string.Format("(&(ObjectClass={0})(ObjectCategory={1})(sAMAccountName={2}))", "user", "person", Login);

            try
            {
                //Setup the directory root using the LDAP search string and connection information
                DirectoryEntry adRoot = new DirectoryEntry("LDAP://" + Domain, ADLogin, ADPswd, System.DirectoryServices.AuthenticationTypes.Secure);
                //Create the searcher variable and set it to the root variable
                DirectorySearcher searcher = new DirectorySearcher(adRoot);
                searcher.PropertiesToLoad.Add("DistinguishedName");
                searcher.SearchScope = SearchScope.Subtree;
                searcher.ReferralChasing = ReferralChasingOption.All;
                //Set the search variable filter to the filter variable
                searcher.Filter = filter;

                //Find only the first user based upon the search string
                SearchResult result = searcher.FindOne();

                //Create the manager AD record and set it equal to the search result
                DirectoryEntry manager = result.GetDirectoryEntry();

                //Get the manager's distringuised name and set it to the dn variable
                dn = (string)manager.Properties["DistinguishedName"][0];

            }
            catch (Exception ex)
            {
                //Create the error trace variable and write it out to the log file
                trace = ex.Message + " - " + ex.StackTrace.ToString() + "\n";
                trace += DateTime.Now.Date.ToString("yyyyMMdd") + " " +
                 DateTime.Now.TimeOfDay.ToString() + "\n";

                //Log.WriteLog("ADUserUpdate.log", trace);
            }

            return dn;
        }

        string ExtractUserName(string path)
        {
            string[] userPath = path.Split(new char[] { '\\' });
            return userPath[userPath.Length - 1];
        }

        protected void chkEnablePwdExpiration_CheckedChanged(object sender, EventArgs e)
        {
            tr22.Visible = false;
            if (chkEnablePwdExpiration.Checked)
                tr22.Visible = true;
        }

        protected void btSaveOwnProfile_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            uHelper.ClosePopUpAndEndResponse(Context);

        }


        protected void btnDeleteFromGroup_Click(object sender, EventArgs e)
        {
            if (Request["uID"] != null)
            {

            }
        }
    }
}