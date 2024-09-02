using DevExpress.Web;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class _UserInfo : UPage
    {
        string newADUser;
        string groupID;

        string[] arrayDelegateUserFor;
        string defaultUserRole = string.Empty;
        string JobTitleSelectedVal=string.Empty;
        private List<Role> userGroup = null;
        //private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&listName={1}&Module=CMDB&UserId={2}";
        //int count = 0;

        protected string editGroupUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=editspgroup"));
        protected string userHomePage = UGITUtility.GetAbsoluteURL("/Pages/userhomepage");

        //private string newParam = "userprofilerelatedassets";


        //bool newUser = false;
        bool isSiteAdmin = false;
        bool isResourceAdmin = false;
        bool isResourceManager = false;
        bool isInAdminGroup = false;
        bool disableChangePassword;
        bool resultdata = true;
        bool fromRMMCards = false;

        public int passwordExpirationPeriod = 0;

        public UserProfile userInfo = null;
        public UserProfile user = null;
        public UserProfile userDelegateFor = null;
        List<UserCertificates> spUserCertificateList;
        List<UserSkills> spUserSkillList;
        List<ExperiencedTag> spExperiencedTagList;

        public string jsonSkillData = string.Empty;
        public string ajaxHelperURL = string.Empty;
        public string assetUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=userprofilerelatedassets");
        public string departmentLabel;
        public string openCloseTicketsForRequestorUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=openandcloseforrequestor");
        public string absoluteURL(string s)
        {
            string url = "";
            return url = UGITUtility.GetAbsoluteURL(s);
        }
        public string SetValueCheck { get; set; }

        public bool olderOutOfOfficeValue;
        public bool sendNotification;
        public bool IsEnable { get; set; }
        public bool limitExceed = false;




        public UserLookupValue delegateUser;
        public DateTime leaveFrom;
        public DateTime leaveTo;


        protected ApplicationContext dbContext = HttpContext.Current.GetManagerContext();
        protected string userID;

        //    DataTable userInfoList;
        FieldConfiguration field = null;
        // SPList userInfoList;
        UserRoleManager rolemanagers = null;
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        BudgetCategoryViewManager budgetCataegoryManager = null;
        FieldConfigurationManager fmanger = null;
        LocationManager ObjLocationManager = null;
        DepartmentManager objDepartmentManager = null;
        FunctionalAreasManager objFunctionalAreasManager = null;
        //UserRolesManager userRoles = null;
        LandingPagesManager userPages = null;
        JobTitleManager jobTitleMGR = null;
        GlobalRoleManager roleManager = null;
        UserCertificateManager userCertificateManager = null;
        UserSkillManager userSkillManager = null;
        ExperiencedTagManager experiencedTagManager = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        StudioManager objStudioManager = null;
        Department selectedDepartment = null;
        UserProjectExperienceManager userProjectExperienceManager = null;
        ProjectEstimatedAllocationManager allocationManager = null;
        //private TenantValidation tenantValidation = null;

        //ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                }
                return _configurationVariableManager;
            }

        }

        private void showControlsForNewUsers()
        {
            if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateNewUser) && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateFBAUser))
            {
                trTypeofUser.Visible = true;
            }
            //btSave.Visible = false;
            btSave.Text = "Invite";
            txtPassword.Visible = false;
            txtReenterPassword.Visible = false;
            btnDelete.Visible = false;
            tr0.Visible = false;
            trLoginName.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;
            tr26.Visible = true;
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
            tr17.Visible = true;
            tr18.Visible = true;
            tr21.Visible = true;
            tr51.Visible = true;
            txtName.Visible = true;
            txtEmail.Visible = true;
            txtMobileNumber.Visible = true;
            txtEmployeeId.Visible = true;
            departmentCtr.Visible = true;
            // ctDepartment.Visible = true;
            cmbJobTitle.Visible = true;
            //divRole.Visible = false;
            txtHourlyRate.Visible = true;
            //peAssignedTo.Visible = true;
            cbIT.Visible = true;
            cbIsConsultant.Visible = true;
            cbIsManager.Visible = true;
            cbIsServiceAccount.Visible = true;
            ddlBudgetCategory.Visible = true;
            ddlSubBudgetCategory.Visible = true;
            glLocation.Visible = true;
            ddlRole.Visible = true;
            ddlFunctionalArea.Visible = true;
            ddlStudio.Visible = true;
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
            rolemanagers = new UserRoleManager(dbContext);
            budgetCataegoryManager = new BudgetCategoryViewManager(dbContext);
            fmanger = new FieldConfigurationManager(dbContext);
            ObjLocationManager = new LocationManager(dbContext);
            objDepartmentManager = new DepartmentManager(dbContext);
            objFunctionalAreasManager = new FunctionalAreasManager(dbContext);
            userPages = new LandingPagesManager(dbContext);
            jobTitleMGR = new JobTitleManager(dbContext);
            TenantValidation tenantValidation = new TenantValidation(dbContext);
            roleManager = new GlobalRoleManager(dbContext);
            userCertificateManager = new UserCertificateManager(dbContext);
            userSkillManager = new UserSkillManager(dbContext);
            spUserCertificateList = userCertificateManager.Load();
            spUserSkillList = userSkillManager.Load();
            experiencedTagManager = new ExperiencedTagManager(dbContext);
            userProjectExperienceManager = new UserProjectExperienceManager(dbContext);
            objStudioManager = new StudioManager(dbContext);
            allocationManager = new ProjectEstimatedAllocationManager(dbContext);
            spExperiencedTagList = experiencedTagManager.Load();
            ddlFunctionalArea.devexListBox.GridViewStyles.Row.CssClass = "lookUpValBox-listRow";
            ddlStudio.devexListBox.GridViewStyles.Row.CssClass = "lookUpValBox-listRow";
            ddlRole.devexListBox.Columns["Title"].Caption = "Select All";
            LeaveFromDate.Date = LeavetoDate.Date = DateTime.Today.Date;
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/api/Account/");

            isSiteAdmin = umanager.IsRole(RoleType.Admin, Page.User.Identity.Name.ToString());
            isResourceAdmin = isSiteAdmin || umanager.IsRole(RoleType.ResourceAdmin, Page.User.Identity.Name.ToString());
            isInAdminGroup = umanager.IsinAdminGroup(dbContext.CurrentUser);
            fromRMMCards = Request.QueryString.AllKeys.Contains("RMMCardView"); // variable used to check if request is from RMM CardsView & disable few fields.
            DDLLocation_Load();
            BindEmployeeType();
            DDLBudgetCategories_Load();
            tbSkills_Load();
            tbExperiencedTags_Load();
            tbCertificate_Load();
            if (!IsPostBack)
            {
                //cmbLoadJobLookup();
                BindRoles();
            }
            //disableChangePassword = true;
            disableChangePassword = false;
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
                        dtcEndDate.Value = new DateTime(8900, 12, 31);
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
                Util.Log.ULog.WriteLog($"{dbContext.TenantAccountId}|{user.Name}: Visited Page: UserInfo.aspx");
            }
            //assetUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, DatabaseObjects.Tables.Assets, userID));
            //lnkassets.PostBackUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", assetUrl, Server.UrlEncode(Request.Url.AbsolutePath), "Assets Details");
            
            if (IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "moreFunction", "ShowMoreFunction()", true);
            }

            base.OnPreInit(e);
        }

        protected void chkimidiate_change(object sender, EventArgs e)
        {
            if (chkimidiate.Checked)
            {
                btSave.Text = "Create";
                trTypeofUser.Visible = false;
                trPasword.Visible = true;
                trReenterPassword.Visible = true;
                txtPassword.Visible = true;
                txtReenterPassword.Visible = true;
            }
            else
            {
                btSave.Text = "Invite";
                trTypeofUser.Visible = false;
                trPasword.Visible = false;
                trReenterPassword.Visible = false;
                txtPassword.Visible = false;
                txtReenterPassword.Visible = false;
            }
        }

        protected void CmbJobTitles_Callback(object source, CallbackEventArgsBase e)
        {
            var val = e.Parameter;
            if (val != null)
            {
                int number;
                bool isnumber = int.TryParse(val, out number);

                if(isnumber)
                cmbLoadJobLookup(Convert.ToInt64(val));
            }
        }

        private void LoadAssetsForParticularUser()
        {
            if (userID != null)
            {


            }
        }

        private void OpenUserProfile(EventArgs e)
        {

            bool enableDisableflag = false;
            var enableDisableControl = false;
            lblUserGroups.Visible = true;
            //string fieldName = null;
            tr24.Visible = true;
            tr25.Visible = true;
            //userGroupList.Visible = false;
            chkimidiate.Visible = false;
            //btnInvite.Visible = false;
            btSave.Text = "Save";


            if (!isResourceAdmin)
            {
                List<UserProfile> userProfile = umanager.GetUsersProfile();
                if (userProfile.Exists(x => x.Id == userID))
                {
                    isResourceManager = true;
                }
            }

            if (!isResourceAdmin)
            {
                // Read-only mode
                UserProfile userProfile = umanager.GetUserInfoById(userID);
                if (userProfile != null && userProfile.Id == userID)
                {
                    //btnResetPassword.Visible = !disableChangePassword;
                    enableDisableflag = (userProfile.Id == HttpContext.Current.CurrentUser().Id || isResourceAdmin) ? true : false;
                    btnResetPassword.Visible = enableDisableflag;
                }
                else
                {
                    btnResetPassword.Visible = !disableChangePassword && (isResourceManager || isResourceAdmin);
                    //btnDelete.Visible = isResourceManager || isResourceAdmin;
                    btnDelete.Visible = isInAdminGroup;
                }

                //FileUploadUserPics.SetImageUrl(userInfo.Picture);

                tr6.Visible = false;

                btSave.Visible = false;
                // lbAccount.Text = userInfo.UserName;

                if (!string.IsNullOrEmpty(userInfo.UserRoleId))
                {
                    LandingPages userDetails = umanager.GetUserRoleById(userInfo.UserRoleId);
                    lblUserRole.Text = userDetails.Name;
                }

                lblUserName.Text = userInfo.UserName;
                ProfileImg.ImageUrl = userInfo.Picture;
                if (userInfo.Picture != null)
                    lblProfilePic.Text = userInfo.Picture.Substring(userInfo.Picture.LastIndexOf("/") + 1);
                lbName.Text = userInfo.Name;
                lbEmail.Text = userInfo.Email;
                lbNotificationEmail.Text = userInfo.NotificationEmail;
                lbJobTitle.Text = userInfo.JobProfile;
                lbMobileNumber.Text = userInfo.MobilePhone;
                lbEmployeeId.Text = userInfo.EmployeeId;

                if (cmbJobTitle.Value != null)
                {
                    lbJobTitle.Text = jobTitleMGR.LoadByID(Convert.ToInt64(cmbJobTitle.Value)).Title;
                }

                if (userInfo != null && !string.IsNullOrEmpty(userInfo.GlobalRoleId))
                {
                    GlobalRole role = roleManager.LoadById(userInfo.GlobalRoleId);
                    if (role != null)
                        txtUsersRole.Text = role.Name;
                    else
                        txtUsersRole.Text = string.Empty;
                }

                lbHourlyRate.Text = string.Format("${0}", userInfo.HourlyRate);

                txtApproveLevelAmount.Visible = false;
                lbApproveLevelAmount.Visible = true;
                lbApproveLevelAmount.Text = string.Format("${0}", userInfo.ApproveLevelAmount);

                if (!string.IsNullOrEmpty(userInfo.Location))
                    lbLocation.Text = ObjLocationManager.LoadByID(Convert.ToInt64(userInfo.Location)).Title;

                if (!string.IsNullOrEmpty(Convert.ToString(userInfo.Department)))
                {
                    departmentCtr.Value = userInfo.Department;
                    selectedDepartment = objDepartmentManager.LoadByID(UGITUtility.StringToLong(userInfo.Department)); 
                    departmentCtr.SetValues(departmentCtr.Value);
                    lbDepartmentCtr.Text = objDepartmentManager.LoadByID(Convert.ToInt64(userInfo.Department)).Title;

                }
                departmentCtr.Visible = false;
                ddlRole.Visible = false;
                ddlUserRole.Visible = false;
                lblUserRole.Visible = true;

                lblEnable.Text = UGITUtility.StringToBoolean(userInfo.Enabled).ToString();
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
                lbStudio.Visible = true;
                ddlStudio.Visible = false;
                if (string.IsNullOrEmpty(Convert.ToString(userInfo.WorkingHoursStart)))
                {
                    lblWorwkingHoursStart.Text = Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayStartTime")).ToString("hh:mm:tt");
                    lblWorkingHoursEnd.Text = "To " + Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayEndTime")).ToString("hh:mm:tt");
                }
                else
                {
                    lblWorwkingHoursStart.Text = userInfo.WorkingHoursStart.ToString();
                    lblWorkingHoursEnd.Text = "To " + userInfo.WorkingHoursEnd.ToString();
                }
                if (userInfo.FunctionalArea != null)
                    lblFunctionalArea.Text = objFunctionalAreasManager.LoadByID(Convert.ToInt32(userInfo.FunctionalArea)).Title; //GetTableDataManager.GetTableData(DatabaseObjects.Tables.FunctionalAreas, "id=" + userInfo.FunctionalArea).Rows[0][DatabaseObjects.Columns.Title].ToString();
                ddlFunctionalArea.Visible = false;

                if (userInfo.StudioLookup > 0)
                    lbStudio.Text = objStudioManager.LoadByID(Convert.ToInt32(userInfo.StudioLookup)).Title; 

                field = fmanger.GetFieldByFieldName(DatabaseObjects.Columns.UserSkillLookup);
                if (field != null && field.Datatype == "Lookup")
                {
                    string value = fmanger.GetFieldConfigurationData(field, string.Join("; ", userInfo.Skills));
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        lblSkills.Text = value;
                    }
                    else
                    {
                        lblSkills.Text = string.Empty;
                    }
                }
                skilldiv.Visible = false;
                UserGroups();//Multiple user groupd

                if (userInfo.BudgetCategory != null)
                {
                    lbBudgetCategory.Text = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, "id=" + userInfo.BudgetCategory).Rows[0][DatabaseObjects.Columns.BudgetCategoryName].ToString();
                }

                if (!string.IsNullOrEmpty(userInfo.ManagerID))
                {
                    lbManager.Text = umanager.GetUserById(userInfo.ManagerID).Name;

                }
                managerUser.Visible = false;
                lbManager.Visible = true;
                lblIsServiceAccount.Visible = true;
                lblIsServiceAccount.Text = "No";
                if (userInfo.IsServiceAccount)
                {
                    lblIsServiceAccount.Text = "Yes";
                }

                //peAssignedTo.Visible = false;
                lbIsConsultant.Visible = true;
                lbIsConsultant.Text = "No";

                if (userInfo.IsConsultant)
                {
                    lbIsConsultant.Text = "Yes";
                }
                lblRmmConsultant.Visible = false;

                lbIsManager.Text = "No";
                if (userInfo.IsManager)
                {
                    lbIsManager.Text = "Yes";
                }
                lbIsManager.Visible = true;
                lblRmmManager.Visible = false;

                lbIT.Visible = true;
                lbIT.Text = "No";
                if (userInfo.IsIT)
                {
                    lbIT.Text = "Yes";
                }

                h3It.Visible = true;
                h3Consultant.Visible = true;
                h3Manager.Visible = true;
                h3ServiceAccount.Visible = true;
                h3Enable.Visible = true;
                h3EPE.Visible = true;



                lblRmmIT.Visible = false;
                lblRmmenable.Visible = false;
                lblRmmworkFlowDis.Visible = false;
                lblRmmpwdExp.Visible = false;



                lblDeskLocation.Text = userInfo.DeskLocation;
                lblStartDate.Text = (userInfo.UGITStartDate).ToShortDateString();
                dtcStartDate.Visible = false;
                lblEndDate.Text = (userInfo.UGITEndDate).ToShortDateString();
                dtcEndDate.Visible = false;
                lbEnablePwdExpiration.Visible = true;
                chkEnablePwdExpiration.Visible = false;
                chkEnablePwdExpiration.Checked = userInfo.EnablePasswordExpiration;
                chkEnablePwdExpiration_CheckedChanged(chkEnablePwdExpiration, new EventArgs());
                lbEnablePwdExpiration.Text = "Off";
                if (chkEnablePwdExpiration.Checked)
                {
                    lbEnablePwdExpiration.Text = "On";
                    //int passwordExpirationPeriod = 0;
                    int.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
                    if (passwordExpirationPeriod == 0)
                        passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;
                    lbExpirationPeriod.Text = GetExpirationPeriodMessage(userInfo, passwordExpirationPeriod); //string.Format("Expiration Period: {0} days", passwordExpirationPeriod);
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
                enableDisableflag = (userProfile.Id == HttpContext.Current.CurrentUser().Id) ? false : true;
                lblEnableOutOfOffice.Visible = enableDisableflag;
                // lblEnableOutOfOffice.Visible = true;
                enableDisableControl = (userProfile.Id == HttpContext.Current.CurrentUser().Id) ? true : false;
                chkOutOfOffice.Visible = enableDisableControl;
                outOfOfficePanelEdit.Visible = false;
                lblEnableOutOfOffice.Text = lblEnableOutOfOffice.Visible == true ? "Off" : string.Empty;

                //lblEnableOutOfOffice.Text = "Off";

                //To show user dalegated for
                if (!string.IsNullOrEmpty(userInfo.DelegateUserFor))
                {
                    ShowMultiDelegateUsers();
                }

                if (!string.IsNullOrEmpty(userInfo.Id))
                {
                    //lblDisableWorkflowNotifications.Visible = false;

                    chkDisableWorkflowNotifications.Visible = enableDisableControl;
                    lblDisableWorkflowNotifications.Visible = enableDisableflag;

                    //if (userProfile.Id== userID ||isResourceAdmin)
                    //{
                    //    chkDisableWorkflowNotifications.Visible = true;
                    //}
                    chkDisableWorkflowNotifications.Checked = userInfo.DisableWorkflowNotifications;

                    //chkOutOfOffice.Visible = true;
                    outOfOfficePanelEdit.Visible = true;
                    outOfOfficePanelRead.Visible = false;
                    //lblEnableOutOfOffice.Visible = false;
                    //chkOutOfOffice.Checked = userInfo.EnableOutofOffice;
                    //Keep old value of out of office
                    //delegateUser = userInfo.DelegateUserOnLeave;
                    olderOutOfOfficeValue = userInfo.EnableOutofOffice;
                    leaveFrom = userInfo.LeaveFromDate;
                    leaveTo = userInfo.LeaveToDate;
                    btSaveOwnProfile.Visible = (userProfile.Id == HttpContext.Current.CurrentUser().Id || isResourceAdmin) ? true : false;
                    btSave.Visible = (isResourceAdmin) ? true : false;

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
                if (!IsPostBack)
                {
                    if (userInfo != null && !string.IsNullOrEmpty(userInfo.Department))
                    {
                        departmentCtr.SetValues(Convert.ToString(userInfo.Department));
                        selectedDepartment = objDepartmentManager.LoadByID(UGITUtility.StringToLong(userInfo.Department));
                        cmbLoadJobLookup(Convert.ToInt64(userInfo.Department));
                        
                    }
                }

                bool enablePasswordReset = !disableChangePassword && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableADPasswordReset);

                // Only resource admins can add users or reset password
                btnResetPassword.Visible = enablePasswordReset && isResourceAdmin;
                //btnDelete.Visible = isResourceAdmin;
                btnDelete.Visible = isInAdminGroup;

                //lbAccount.Text = userInfo.Name;

                lbName.Visible = false;
                if (userInfo != null && userInfo.BudgetCategory.HasValue)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(userInfo.BudgetCategory)))
                    {
                        #region Old Code
                        /*
                            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, "id=" + userInfo.BudgetCategory);
                            if (dt != null)
                            {
                                ddlSubBudgetCategory.Items.Clear();
                                DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories, $"{DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");
                                if (budgets != null && budgets.Rows.Count > 0)
                                {
                                    DataRow[] budgetSubCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategoryName, dt.Rows[0]["BudgetCategoryName"]).ToString());
                                    foreach (DataRow row in budgetSubCategories)
                                    {
                                        ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                                    }
                                    ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                                    ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(userInfo.BudgetCategory)).Selected = true;
                                    //DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                                    //foreach (DataRow row in budgetCategories.Rows)
                                    //{
                                    //    ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName])));
                                    //}
                                    //ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                                    ddlBudgetCategory.Items.FindByText(dt.Rows[0]["BudgetCategoryName"].ToString()).Selected = true;
                                }
                            }
                        */
                        #endregion

                        List<BudgetCategory> listBudgetCategory = budgetCataegoryManager.GetConfigBudgetCategoryData().ToList();

                        if (listBudgetCategory != null && listBudgetCategory.Count() > 0)
                        {
                            ddlSubBudgetCategory.Items.Clear();
                            BudgetCategory objBudgetCategory = listBudgetCategory.Where(x => x.ID == userInfo.BudgetCategory).FirstOrDefault();
                            if (objBudgetCategory != null)
                            {
                                ddlSubBudgetCategory.DataSource = (from x in listBudgetCategory
                                                                   where x.BudgetCategoryName == objBudgetCategory.BudgetCategoryName
                                                                   orderby x.BudgetSubCategory
                                                                   select new { x.ID, x.BudgetSubCategory }).ToList();
                                ddlSubBudgetCategory.DataValueField = DatabaseObjects.Columns.Id;
                                ddlSubBudgetCategory.DataTextField = DatabaseObjects.Columns.BudgetSubCategory;
                                ddlSubBudgetCategory.DataBind();
                                ddlSubBudgetCategory.Items.FindByValue(Convert.ToString(userInfo.BudgetCategory)).Selected = true;
                                ddlBudgetCategory.Items.FindByText(objBudgetCategory.BudgetCategoryName).Selected = true;
                            }
                            ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                        }
                    }
                }

                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(userInfo.FunctionalArea)))
                    {
                        ddlFunctionalArea.SetValues(Convert.ToString(userInfo.FunctionalArea));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(userInfo.StudioLookup)))
                    {
                        //if (selectedDepartment != null)
                        //{
                        //    ddlStudio.FilterExpression = $"{DatabaseObjects.Columns.DivisionLookup} = '{selectedDepartment.DivisionIdLookup}'";
                        //    ddlStudio.devexListBox.DataBind();
                        //}

                        ddlStudio.SetValues(Convert.ToString(userInfo.StudioLookup));
                    }

                    if (fromRMMCards)
                    {
                        ImgUserName.Visible = false;
                        lbName.Visible = true;
                        lbName.Text = userInfo.Name;
                    }
                    else
                    {
                        txtName.Visible = true;                        
                    }

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
                    lbEmployeeId.Visible = false;
                    txtEmployeeId.Visible = true;
                    txtEmployeeId.Text = userInfo.EmployeeId;
                    lbLocation.Visible = false;
                    glLocation.Visible = true;
                    hdnResume.Value = userInfo.Resume;
                    dvResume.Visible = string.IsNullOrEmpty(userInfo.Resume) ? false : true;
                    if (userInfo.Location != null)
                    {
                        glLocation.Value = userInfo.Location;
                        glLocation.GridView.Selection.SelectRowByKey(userInfo.Location);
                    }
                    if (!string.IsNullOrEmpty(userInfo.EmployeeType))
                    {
                        ddlEmpType.Items.FindByValue(userInfo.EmployeeType).Selected = true;
                    }
                    managerUser.SetValues(userInfo.ManagerID);
                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(userInfo.Skills)))
                    {
                        field = fmanger.GetFieldByFieldName(DatabaseObjects.Columns.UserSkillLookup);
                        if (field != null && field.Datatype == "Lookup")
                        {
                            string value = fmanger.GetFieldConfigurationData(field, string.Join(";#", userInfo.Skills));
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                lblSkills.Text = value;
                            }
                            else
                            {
                                lblSkills.Text = string.Empty;
                            }
                        }
                    }
                }

                string uRoleValue = "";
                if (userInfo != null)
                {
                    List<string> iRoleList = umanager.GetRoles(userID).ToList();
                    if (iRoleList.Count > 0)
                    {
                        foreach (string listname in iRoleList)
                        {
                            uRoleValue += rolemanagers.GetRoleByName(listname).Id + ",";                            
                        }
                    }

                    if (fromRMMCards)
                    {
                        userGroupList.Visible = false;
                        lblUserGroups.Visible = true;
                        if (iRoleList.Count > 0)
                            lblUserGroups.Text = string.Join(uGovernIT.Utility.Constants.BreakLineSeparator, iRoleList);
                        else
                            lblUserGroups.Text = string.Empty;
                    }
                    else
                    {
                        userGroupList.Visible = true;
                    }
                }
                if (!string.IsNullOrEmpty(uRoleValue))
                {
                    string[] val = uRoleValue.Split(',');
                    // ddlRole.SetValues("(None)");
                    //ddlRole.SetText("(None)");

                    if (val.Length > 1)
                    {
                        ddlRole.SetValues(uRoleValue.Substring(0, uRoleValue.Length - 1).ToString());
                    }
                    else
                    {
                        ddlRole.SetValues(uRoleValue);
                    }
                }

                if (userInfo != null && !string.IsNullOrEmpty(userInfo.UserRoleId))
                {
                    LandingPages userDetails = umanager.GetUserRoleById(userInfo.UserRoleId);
                    List<LandingPages> dtUserRole = userPages.GetLandingPages().Where(x => x.Deleted == false).OrderBy(x => x.Name).ToList();
                    if (userDetails != null)
                    {
                        ddlUserRole.DataTextField = "Name";
                        ddlUserRole.DataValueField = "ID";
                        ddlUserRole.DataSource = dtUserRole;
                        ddlUserRole.DataBind();
                        ddlUserRole.Items.FindByValue(userDetails.Id).Selected = true;
                    }
                    ddlUserRole.Items.Insert(0, new ListItem("(None)", ""));

                    if (fromRMMCards)
                    {
                        ddlUserRole.Visible = false;
                        lblUserRole.Visible = true;
                        lblUserRole.Text = userDetails.Name;
                    }
                    else
                    {
                        ddlUserRole.Visible = true;
                    }
                }

                //lblRole.Visible = false;
                //ddlRole.Visible = true;
                lbJobTitle.Visible = false;
                cmbJobTitle.Visible = true;
                //txtJobTitle.Text = userInfo.JobProfile;

                if (userInfo != null)
                {
                    cmbJobTitle.SelectedIndex = cmbJobTitle.Items.IndexOf(cmbJobTitle.Items.FindByValue(Convert.ToString(userInfo.JobTitleLookup)));
                }

                if (userInfo != null && !string.IsNullOrEmpty(userInfo.GlobalRoleId))
                {
                    GlobalRole role = roleManager.LoadById(userInfo.GlobalRoleId);
                    if (role != null)
                        txtUsersRole.Text = role.Name;
                    else
                        txtUsersRole.Text = string.Empty;
                }

                //new entries for "out of office calender" to show data start
                tr66.Visible = true;

                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.DelegateUserFor))
                    {
                        ShowMultiDelegateUsers();
                    }
                }
                tr60.Visible = true;
                lblEnableOutOfOffice.Visible = false;
                chkOutOfOffice.Visible = true;
                outOfOfficePanelEdit.Visible = true;
                outOfOfficePanelRead.Visible = false;
                if (userInfo != null)
                {
                    chkOutOfOffice.Checked = userInfo.EnableOutofOffice;
                }
                tr43.Visible = true;
                dtWorkingHoursStart.Visible = true;
                dtWorkingHoursEnd.Visible = true;
                lblWorwkingHoursStart.Visible = false;
                lblWorkingHoursEnd.Visible = false;

                if (userInfo != null)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(userInfo.WorkingHoursStart)))
                    {
                        dtWorkingHoursStart.NullText = ConfigurationVariableManager.GetValue("WorkdayStartTime");
                        dtWorkingHoursEnd.NullText = ConfigurationVariableManager.GetValue("WorkdayEndTime");
                        dtWorkingHoursStart.Value = Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayStartTime"));
                        dtWorkingHoursEnd.Value = Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayEndTime"));
                    }
                    else
                    {
                        dtWorkingHoursStart.Value = userInfo.WorkingHoursStart;
                        dtWorkingHoursEnd.Value = userInfo.WorkingHoursEnd;
                    }
                    //Keep old value of out of office
                    olderOutOfOfficeValue = userInfo.EnableOutofOffice;
                    //delegateUser = userInfo.DelegateUserOnLeave;

                    if (!string.IsNullOrEmpty(userInfo.DelegateUserOnLeave))
                        DelegateUserOnLeave.SetValues(Convert.ToString(userInfo.DelegateUserOnLeave));
                    leaveFrom = userInfo.LeaveFromDate;
                    leaveTo = userInfo.LeaveToDate;
                    if (userInfo.LeaveFromDate != DateTime.MinValue)
                    {
                        if (userInfo.LeaveFromDate == new DateTime(1754, 1, 1))
                            userInfo.LeaveFromDate = DateTime.Today;
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
                    else
                    {
                        LeaveFromDate.Value = DateTime.Today;
                        LeavetoDate.Value = LeaveFromDate.Value;
                    }

                    //new entries for "out of office calender" to show data end
                    lbHourlyRate.Visible = false;
                    txtHourlyRate.Visible = true;
                    txtHourlyRate.Text = userInfo.HourlyRate.ToString();
                    txtLoginName.Visible = false;
                    lblUserName.Text = userInfo.UserName;
                    ProfileImg.ImageUrl = userInfo.Picture;
                    if (userInfo.Picture != null)
                        lblProfilePic.Text = userInfo.Picture.Substring(userInfo.Picture.LastIndexOf("/") + 1);
                    lbManager.Visible = false;
                    
                    //FileUploadUserPics.SetImageUrl(userInfo.Picture);
                    lbBudgetCategory.Visible = false;
                    ddlBudgetCategory.Visible = true;
                    ddlSubBudgetCategory.Visible = true;
                    lbIsConsultant.Visible = false;
                    cbIsConsultant.Visible = true;
                    cbIsConsultant.Checked = userInfo.IsConsultant;
                    lbIsManager.Visible = false;
                    cbIsManager.Visible = true;
                    cbIsManager.Checked = userInfo.IsManager;
                    lbIT.Visible = false;
                    cbIT.Visible = true;
                    cbIT.Checked = userInfo.IsIT;
                    lblIsServiceAccount.Visible = false;
                    cbIsServiceAccount.Visible = true;
                    cbIsServiceAccount.Checked = userInfo.IsServiceAccount;
                    lblEnable.Visible = false;
                    chkEnable.Visible = true;
                    chkEnable.Checked = UGITUtility.StringToBoolean(userInfo.Enabled);
                    lblFunctionalArea.Visible = false;
                    ddlFunctionalArea.Visible = true;
                    lbStudio.Visible = false;
                    ddlStudio.Visible = true;
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

                    tbSkills.Value = userInfo.Skills;
                    tbCertificate.Value = userInfo.UserCertificateLookup;
                    
                    List<UserProjectExperience> userProjectExperiences = userProjectExperienceManager.Load(x => x.UserId == userInfo.Id && string.IsNullOrWhiteSpace(x.ProjectID));
                    if (userProjectExperiences != null && userProjectExperiences.Count() > 0)
                    {
                        tbExperiencedTag.Value = string.Join(",", userProjectExperiences.Select(x => x.TagLookup).ToArray());
                    }

                    lbEnablePwdExpiration.Visible = false;
                    chkEnablePwdExpiration.Visible = true;
                    chkEnablePwdExpiration.Checked = userInfo.EnablePasswordExpiration;
                    chkEnablePwdExpiration_CheckedChanged(chkEnablePwdExpiration, new EventArgs());
                    lbEnablePwdExpiration.Text = "Off";
                    if (chkEnablePwdExpiration.Checked)
                    {
                        lbEnablePwdExpiration.Text = "On";
                        //int passwordExpirationPeriod = 0; ;
                        int.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
                        if (passwordExpirationPeriod == 0)
                            passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;
                        //lbExpirationPeriod.Text = string.Format("Expiration Period: {0} days", passwordExpirationPeriod);
                        lbExpirationPeriod.Text = GetExpirationPeriodMessage(userInfo, passwordExpirationPeriod);
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
            }
            UserGroups();//Multiple user group

            if (isResourceAdmin)
            {
                if (fromRMMCards)
                    ImgUserName.Visible = false;
                else
                    ImgUserName.Visible = true;

                btnViewHistory.Visible = true;

                if (userInfo != null)
                {
                    DataRow dtUser = UGITUtility.ObjectToData(userInfo).Select()[0];
                    List<HistoryEntry> userChangeHistory = uHelper.GetHistory(dtUser, DatabaseObjects.Columns.History);
                    userChangeHistory.ForEach(x =>
                    {
                        var userProfile = umanager.GetUserInfoById(x.createdBy);
                        if (userProfile != null)
                        {
                            x.createdBy = userProfile.Name;
                        }
                    });
                    gvHistory.DataSource = userChangeHistory;
                }
                
                gvHistory.DataBind();
            }

            if (dbContext.CurrentUser.Id.EqualsIgnoreCase(userInfo.Id))
                ImgUserName.Visible = false;
        }

        private void UserGroups()
        {
            field = fmanger.GetFieldByFieldName(DatabaseObjects.Columns.UserGroup);
            if (field != null && field.Datatype == "GroupField")
            {
                if (userInfo != null)
                {
                    var groups = umanager.GetUserGroups(userInfo.Id);
                    string value = fmanger.GetFieldConfigurationData("UserGroup", string.Join(",", groups)).Replace(@"#", "");

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (!isResourceAdmin)
                        {
                            lblUserGroups.Text = value;
                            rolesListBox.Visible = false;

                        }
                        else
                        {
                            rolesListBox.SetValues(string.Join(",", groups));
                        }


                    }
                    else
                    {
                        //lblUserGroups.Text = string.Empty;
                    }
                }
            }
        }

        private void OpenNewForm()
        {
            // New Form
            // pplUserAccount.Visible = true;
            tr1.Visible = false;
            tr2.Visible = false;
            tr3.Visible = false;
            tr26.Visible = false;
            //tr4.Visible = false;
            tr5.Visible = false;
            tr6.Visible = false;
            tr7.Visible = false;
            tr8.Visible = false;
            //divassetsTickets.Visible = false;
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

            lblUserRole.Visible = false;
            ddlUserRole.Visible = true;

            tr9.Visible = false;
            tr21.Visible = false;
            btnResetPassword.Visible = false;

        }

        protected void BtSave_Click(object sender, EventArgs e)
        {
            TenantValidation tenantValidation = new TenantValidation(dbContext);
            lblMessage.Text = string.Empty;

            if (tenantValidation.IsUserLimitExceed())
            {
                limitExceed = tenantValidation.IsUserLimitExceed();
                // ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('You have exceeded number of users in the Trial plan');", true);
                //return;
                // ScriptManager.RegisterStartupScript(this, GetType(), "", "ValidateUser();", false);
                //uHelper.ClosePopUpAndEndResponse(Context, false);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>ValidateUser();</Script>", false);
            }
            else
            {
                if (Page.IsValid)
                {
                    string path = "";
                    DateTime minDate = new DateTime(1900, 1, 1);
                    DateTime maxDate = new DateTime(8900, 12, 31);
                    if (Request.QueryString.AllKeys.Contains("newUser"))
                    {
                        UserProfile user = new UserProfile();
                        //Adding 1 line for default image path if user pic is null then it will take below default image
                        user.Picture = "/Content/Images/userNew.png";
                        if (FileUploadUserPics.HasFile)
                        {
                            //path = UGITUtility.GetAbsoluteURL("/Content/ProfileImages/");  //Fix for Profile Pictures not uploading
                            path = "/Content/ProfileImages/";
                            if (!Directory.Exists(Server.MapPath(path)))
                                Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));

                            string extension = Path.GetExtension(FileUploadUserPics.FileName);
                            path = $"{path}{Guid.NewGuid().ToString()}{extension}";
                            FileUploadUserPics.SaveAs(Server.MapPath(path));
                            user.Picture = path;
                        }
                        if (FileUploadResume.HasFile)
                        {
                            //path = UGITUtility.GetAbsoluteURL("/Content/ProfileImages/");  //Fix for Profile Pictures not uploading
                            path = "/Content/ProfileImages/";
                            if (!Directory.Exists(Server.MapPath(path)))
                                Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));

                            string extension = Path.GetExtension(FileUploadResume.FileName);
                            path = $"{path}{Guid.NewGuid().ToString()}{extension}";
                            FileUploadResume.SaveAs(Server.MapPath(path));
                            user.Resume = path;
                        }
                        //user.Picture = FileUploadUserPics.GetImageUrl();
                        user.UserName = txtLoginName.Text.Trim();
                        user.Name = txtName.Text.Trim();
                        user.Email = txtEmail.Text.Trim();
                        user.EmployeeType = ddlEmpType.SelectedValue.ToString();
                        if (!string.IsNullOrEmpty(ddlSubBudgetCategory.SelectedValue.ToString()))
                        {
                            user.BudgetCategory = Convert.ToInt32(ddlSubBudgetCategory.SelectedValue.ToString());
                        }
                        if (!string.IsNullOrEmpty(ddlFunctionalArea.GetValues().ToString())) { user.FunctionalArea = Convert.ToInt32(ddlFunctionalArea.GetValues().ToString()); }
                        if (!string.IsNullOrEmpty(ddlStudio.GetValues().ToString())) { user.StudioLookup = Convert.ToInt32(ddlStudio.GetValues().ToString()); }

                        user.Skills = Convert.ToString(tbSkills.Value);
                        user.UserCertificateLookup = Convert.ToString(tbCertificate.Value);
                        user.NotificationEmail = txtNotificationEmail.Text.Trim();
                        user.PhoneNumber = txtMobileNumber.Text.Trim();
                        user.EmployeeId = txtEmployeeId.Text.Trim();
                        user.JobProfile = cmbJobTitle.Text.Trim();

                        if (cmbJobTitle.SelectedItem != null)
                            user.JobTitleLookup = Convert.ToInt64(cmbJobTitle.SelectedItem.Value);
                        else
                            user.JobTitleLookup = Convert.ToInt64(cmbJobTitle.Value);
                        JobTitle jobTitleObj = jobTitleMGR.LoadByID(user.JobTitleLookup);
                        if (jobTitleObj != null)
                            user.GlobalRoleId = jobTitleObj.RoleId;

                        user.DeskLocation = txtDeskLocation.Text.Trim();
                        user.HourlyRate = Convert.ToInt32(!string.IsNullOrEmpty(txtHourlyRate.Text.Trim()) ? txtHourlyRate.Text.ToString() : "0");
                        user.ApproveLevelAmount = !string.IsNullOrEmpty(txtApproveLevelAmount.Text.Trim()) ? double.Parse(txtApproveLevelAmount.Text.Trim()) : 0.0;
                        user.ManagerID = managerUser.GetValues();
                        user.IsIT = cbIT.Checked;
                        user.isRole = false;
                        user.IsConsultant = cbIsConsultant.Checked;
                        user.IsManager = cbIsManager.Checked;
                        user.IsServiceAccount = cbIsServiceAccount.Checked;
                        if (!string.IsNullOrEmpty(Convert.ToString(glLocation.Value))) { user.Location = glLocation.Value.ToString(); }
                        
                        if (!string.IsNullOrEmpty(Convert.ToString(dtcStartDate.Value)))
                        { user.UGITStartDate = (DateTime)dtcStartDate.Value; }
                        else { user.UGITStartDate = minDate; }
                        if (!string.IsNullOrEmpty(Convert.ToString(dtcEndDate.Value))) { user.UGITEndDate = (DateTime)dtcEndDate.Value; }
                        else { user.UGITEndDate = maxDate; }
                        user.Enabled = chkEnable.Checked;
                        //user.Enabled = false;
                        user.EnablePasswordExpiration = chkEnablePwdExpiration.Checked;
                        user.DisableWorkflowNotifications = chkDisableWorkflowNotifications.Checked;
                        user.Department = departmentCtr.GetValues();
                        //user.PasswordExpiryDate = !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)) ? (DateTime)dtcPwdExpiryDate.Value : DateTime.Today.AddYears(1);
                        user.PasswordExpiryDate = SetpasswordExpirationDate();
                        if (!string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursEnd.Value)))
                        {
                            string date = DateTime.Today.ToShortDateString();
                            string time = Convert.ToDateTime(dtWorkingHoursEnd.Value.ToString()).ToString("hh:mm tt");
                            user.WorkingHoursEnd = Convert.ToDateTime(date + " " + time);
                        }
                        else
                        {
                            string date = DateTime.Today.ToShortDateString();
                            string time = Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayEndTime")).ToString("hh:mm tt");
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
                            string time = Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayStartTime")).ToString("hh:mm tt");
                            user.WorkingHoursStart = Convert.ToDateTime(date + " " + time);
                        }
                        if (!string.IsNullOrEmpty(ddlUserRole.SelectedValue.Trim()))
                        {
                            user.UserRoleId = ddlUserRole.SelectedValue.Trim();
                        }

                        UserProfile newUser = umanager.FindById(txtLoginName.Text.Trim());
                        string message = string.Empty;
                        if (newUser == null)
                        {
                            if (!string.IsNullOrEmpty(ddlRole.GetValues().ToString()))
                            {
                                //string[] valuesList = ddlRole.GetValues().Split(',');
                                //foreach (string roleID in valuesList)
                                //{
                                //    user.Roles.Add(new IdentityUserRole() { RoleId = roleID, UserId = user.Id });
                                //}
                            }
                            string password = string.Empty;
                            if (chkimidiate.Checked)
                                password = txtPassword.Text;
                            else
                            {
                                password = umanager.GeneratePassword();
                                user.Enabled = false;
                            }
                            user.TenantID = dbContext.TenantID;

                            IdentityResult result = umanager.Create(user, password.Trim());

                            List<string> groupList = rolesListBox.GetValuesAsList();


                            if (groupList != null && groupList.Count > 0 && user.Id != null && result.Succeeded)
                            {
                                List<string> valueList = rolemanagers.GetRoleList().Where(x => groupList.Contains(x.Id)).Select(c => c.Name).ToList();

                                umanager.AddToRoles(user.Id.Trim(), valueList.ToArray());


                            }

                            if (result.Succeeded)
                            {
                                umanager.UpdateIntoCache(user);
                                //umanager.AddClaim(user.Id, new Claim("AuthProvider", "Forms"));
                                Util.Log.ULog.WriteUGITLog(dbContext.CurrentUser.Id, $"New User {user.Name} created.", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.UserProfile), dbContext.TenantID);
                                
                                if (chkimidiate.Checked)
                                {
                                    message = "User Created Successfully.";
                                }
                                else
                                {
                                    string encryUserId = uGovernITCrypto.Encrypt(user.Id, "ugitpass");
                                    string encrTenAccId = uGovernITCrypto.Encrypt(dbContext.TenantID, "ugitpass");
                                    string encrword = uGovernITCrypto.Encrypt(password, "ugitpass");
                                    SendEmailToNewApplicaitonRequester(user.Email, dbContext.TenantAccountId, user.UserName, password, encryUserId, encrTenAccId, encrword, user.Name);
                                    // uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
                                    message = "User Created Successfully. An invite email was sent to a new user.";
                                }
                                message = "<script type=\"text/javascript\">window.onload = function() { alert('"+message+"'); }</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", message, false);
                            }
                            else
                            {
                                newUser = umanager.GetUserByUserName(txtLoginName.Text.Trim());

                                if (chkbxShowPassword.Checked)
                                    chkbxShowPassword.Checked = false;
                                messageTR.Visible = true;

                                if (newUser != null && newUser.Enabled)
                                    lblMessage.Text = string.Join(",", result.Errors);
                                else if (newUser != null && !newUser.Enabled)
                                    lblMessage.Text = "Login exists in Disabled Users";
                                else
                                    lblMessage.Text = string.Join(",", result.Errors);
                            }
                        }
                        else
                        {
                            if (newUser.Enabled)
                                message = "<script type=\"text/javascript\">window.onload = function() { alert('Login already exists.'); }</script>";
                            else
                                message = "<script type=\"text/javascript\">window.onload = function() { alert('Login exists in Disabled Users.'); }</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", message, false);
                        }
                    }
                    if (Request.QueryString.AllKeys.Contains("UpdateUser"))
                    {
                        StringBuilder sbHistory = new StringBuilder();
                        user = umanager.FindById(userID);
                        sbHistory.AppendLine($"{user.Name}'s profile updated:");

                        DataTable dtOldUser = UGITUtility.ObjectToData(user);
                        if (FileUploadUserPics.HasFile)
                        {
                            //path = UGITUtility.GetAbsoluteURL("/Content/ProfileImages/"); //Fix for Profile Pictures not uploading
                            path = "/Content/ProfileImages/";
                            if (!string.IsNullOrEmpty(user.Picture))
                            {
                                if (!Directory.Exists(Server.MapPath(path)))
                                    Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));
                                if (File.Exists(Server.MapPath(user.Picture)) && !user.Picture.Equals("/Content/Images/userNew.png"))
                                {
                                    File.Delete(Server.MapPath(user.Picture));
                                }
                            }
                            string extension = Path.GetExtension(FileUploadUserPics.FileName);
                            path = $"{path}{Guid.NewGuid().ToString()}{extension}";
                            FileUploadUserPics.SaveAs(Server.MapPath(path));
                            user.Picture = path;
                        }
                        if (FileUploadResume.HasFile)
                        {
                            //path = UGITUtility.GetAbsoluteURL("/Content/ProfileImages/");  //Fix for Profile Pictures not uploading
                            path = "/Content/ProfileImages/";
                            if (!Directory.Exists(Server.MapPath(path)))
                                Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));

                            string extension = Path.GetExtension(FileUploadResume.FileName);
                            path = $"{path}{Guid.NewGuid().ToString()}{extension}";
                            FileUploadResume.SaveAs(Server.MapPath(path));
                            user.Resume = path;
                        }
                        else
                        {
                            user.Resume = hdnResume.Value;
                        }

                        if (!string.IsNullOrEmpty(txtUserName.Text.Trim()))
                        {
                            string mesg = string.Empty;
                            if (ValidateNewUsername(txtUserName.Text.Trim(), out mesg))
                                user.UserName = txtUserName.Text.Trim();
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = mesg;
                                return;
                            }
                        }

                        //user.Picture = FileUploadUserPics.GetImageUrl();
                        user.Name = txtName.Text.Trim();
                        user.Email = txtEmail.Text.Trim();
                        user.NotificationEmail = txtNotificationEmail.Text.Trim();
                        user.EmployeeType = ddlEmpType.SelectedValue.ToString();
                        if (!string.IsNullOrEmpty(ddlSubBudgetCategory.SelectedValue))
                        {
                            user.BudgetCategory = Convert.ToInt32(ddlSubBudgetCategory.SelectedValue.ToString());
                        }
                        else
                        {
                            user.BudgetCategory = null;
                        }
                        user.Skills = Convert.ToString(tbSkills.Value);
                        user.UserCertificateLookup = Convert.ToString(tbCertificate.Value);
                        user.PhoneNumber = txtMobileNumber.Text.Trim();
                        user.EmployeeId = txtEmployeeId.Text.Trim();

                        List<string> tagLookup = !string.IsNullOrWhiteSpace(tbExperiencedTag.Value.ToString()) ? tbExperiencedTag.Value.ToString().Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() : null;
                        userProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, "", user.Id);

                        if (!string.IsNullOrEmpty(departmentCtr.GetValues()))
                        {
                            user.Department = Convert.ToString(departmentCtr.GetValues());
                        }
                        else if (departmentCtr.GetValues() == string.Empty)
                        {
                            user.Department = null;
                        }

                        user.DeskLocation = txtDeskLocation.Text.Trim();
                        user.JobProfile = cmbJobTitle.Text.Trim();
                        if(cmbJobTitle.SelectedItem!=null)
                            user.JobTitleLookup = Convert.ToInt64(cmbJobTitle.SelectedItem.Value);
                        else
                            user.JobTitleLookup = Convert.ToInt64(cmbJobTitle.Value);
                        JobTitle jobTitleObj = jobTitleMGR.LoadByID(user.JobTitleLookup);
                        if (jobTitleObj != null)
                            user.GlobalRoleId = jobTitleObj.RoleId;

                        user.HourlyRate = UGITUtility.StringToInt(txtHourlyRate.Text);
                        user.ApproveLevelAmount = !string.IsNullOrEmpty(Convert.ToString(txtApproveLevelAmount.Text.Trim())) ? double.Parse(txtApproveLevelAmount.Text.Trim()) : 0.0;
                        user.ManagerID = managerUser.GetValues();
                        user.IsIT = cbIT.Checked;
                        user.isRole = false;
                        if (!string.IsNullOrEmpty(Convert.ToString(glLocation.Value))) { user.Location = glLocation.Value.ToString(); }
                        if (!string.IsNullOrEmpty(Convert.ToString(ddlFunctionalArea.GetValues()))) { user.FunctionalArea = Convert.ToInt32(ddlFunctionalArea.GetValues().ToString()); }
                        if (!string.IsNullOrEmpty(Convert.ToString(ddlStudio.GetValues()))) { user.StudioLookup = Convert.ToInt32(ddlStudio.GetValues().ToString()); }
                        user.IsConsultant = cbIsConsultant.Checked;
                        user.EnableOutofOffice = chkOutOfOffice.Checked;
                        user.IsServiceAccount = cbIsServiceAccount.Checked;
                        if (chkOutOfOffice.Checked)
                        {
                            user.LeaveFromDate = LeaveFromDate.Date;
                            user.LeaveToDate = LeavetoDate.Date;
                            user.DelegateUserFor = DelegateUserOnLeave.GetValues();
                            if (!string.IsNullOrEmpty(user.DelegateUserFor))
                            {
                                resultdata = AddMultipleDelegateUsers();
                            }
                        }
                        else
                        {
                            UncheckforOutOfOffice();
                        }
                        if (!resultdata)
                            return;
                        else
                        {
                            if (!string.IsNullOrEmpty(ddlUserRole.SelectedValue.Trim()))
                                user.UserRoleId = ddlUserRole.SelectedValue.Trim();
                            else
                                user.UserRoleId = null;

                            user.IsManager = cbIsManager.Checked;
                            if (!string.IsNullOrEmpty(Convert.ToString(dtcStartDate.Value))) { user.UGITStartDate = (DateTime)dtcStartDate.Value; }
                            else { user.UGITStartDate = minDate; }
                            if (!string.IsNullOrEmpty(Convert.ToString(dtcEndDate.Value))) { user.UGITEndDate = (DateTime)dtcEndDate.Value; }
                            else { user.UGITEndDate = maxDate; }
                            user.Enabled = chkEnable.Checked;
                            user.EnablePasswordExpiration = chkEnablePwdExpiration.Checked;
                            user.DisableWorkflowNotifications = chkDisableWorkflowNotifications.Checked;
                            //user.PasswordExpiryDate = !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)) ? (DateTime)dtcPwdExpiryDate.Value : DateTime.Today.AddYears(1);
                            user.PasswordExpiryDate = !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)) ? (DateTime)dtcPwdExpiryDate.Value : (user.PasswordExpiryDate != null ? user.PasswordExpiryDate : DateTime.Today.AddDays(GetPasswordExpirationPeriod()));
                            user.WorkingHoursEnd = !string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursEnd.Value)) ? DateTime.Parse(dtWorkingHoursEnd.Value.ToString()) : Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayEndTime"));
                            user.WorkingHoursStart = !string.IsNullOrEmpty(Convert.ToString(dtWorkingHoursStart.Value)) ? DateTime.Parse(dtWorkingHoursStart.Value.ToString()) : Convert.ToDateTime(ConfigurationVariableManager.GetValue("WorkdayStartTime"));
                            //Commented by chetan
                            //if (!string.IsNullOrEmpty(ddlRole.GetText()))
                            //{
                            //    List<string> existingRoles = umanager.GetRoles(user.Id).ToList();
                            //    List<string> valueList = rolemanagers.GetRoleList().Where(x => ddlRole.GetValuesAsList().Contains(x.Id)).Select(c => c.Name).ToList();
                            //    List<string> deleteList = existingRoles.Except(valueList).ToList();
                            //    List<string> addList = valueList.Except(existingRoles).ToList();
                            //    if (deleteList.Count > 0)
                            //        deleteList.ForEach(x => { umanager.DeleteUserRole(user, x); });
                            //    if (addList.Count > 0)
                            //        addList.ForEach(x => { umanager.AddUserRole(user, x); });
                            //}

                            List<string> groupList = rolesListBox.GetValuesAsList();

                            if (groupList != null && groupList.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(rolesListBox.GetText().ToString()) && !string.IsNullOrEmpty(user.Id))
                                {
                                    List<string> valueList = rolemanagers.GetRoleList().Where(x => groupList.Contains(x.Id)).Select(c => c.Name).ToList();

                                    List<string> existingRoles = umanager.GetRoles(user.Id.Trim()).ToList();
                                    List<string> deleteList = existingRoles.Except(valueList).ToList();
                                    List<string> addList = valueList.Except(existingRoles).ToList();

                                    if (deleteList.Count > 0)
                                    {
                                        umanager.RemoveFromRoles(user.Id.Trim(), deleteList.ToArray());
                                        sbHistory.AppendLine($"Removed from Groups: { string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, deleteList)}");
                                    }
                                    if (addList.Count > 0)
                                    {
                                        umanager.AddToRoles(user.Id.Trim(), addList.ToArray());
                                        sbHistory.AppendLine($"Added in Groups: {string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, addList)}");
                                    }//need to write delete existing 

                                    //List<string> valueList = rolemanagers.GetRoleList().Where(x => groupList.Contains(x.Id)).Select(c => c.Name).ToList();
                                    //umanager.AddToRoles(user.Id.Trim(), valueList.ToArray());
                                }
                            }

                            if (!user.Enabled)
                            {                               
                                //List<string> existingRoles = umanager.GetRoles(user.Id).ToList();
                                //existingRoles.ForEach(x => { umanager.DeleteUserRole(user, x); });
                                sbHistory.AppendLine($"Removed from Groups: { umanager.DisableUserById(user.Id, false)}");
                            }

                            List<string> lstHistory = new List<string>();
                            for (int i = 0; i < user.GetType().GetProperties().Count(); i++)
                            {
                                if (!Convert.ToString(user.GetType().GetProperties()[i].GetValue(user)).EqualsIgnoreCase(Convert.ToString(dtOldUser.Rows[0][user.GetType().GetProperties()[i].Name])))
                                {
                                    if (user.GetType().GetProperties()[i].GetCustomAttributes(true).Count() == 0)
                                        lstHistory.Add($"{user.GetType().GetProperties()[i].Name};{user.GetType().GetProperties()[i].Name}");
                                    else
                                        lstHistory.Add($"{((System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)user.GetType().GetProperties()[i].GetCustomAttributes(true)[0]).Name};{user.GetType().GetProperties()[i].Name}");
                                }
                            }

                            if (lstHistory.Count > 0)
                            {
                                //StringBuilder sbHistory = new StringBuilder();
                                DataRow drNewUser = UGITUtility.ObjectToData(user).Select()[0];
                                DataRow drUser = dtOldUser.Select()[0];
                                string oldValue = string.Empty, newValue = string.Empty;
                                //sbHistory.AppendLine($"{user.Name}'s profile updated:");
                                foreach (var item in lstHistory)
                                {
                                    string[] arrItem = item.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    field = fmanger.GetFieldByFieldName(arrItem[0]);
                                    if (field != null)
                                    {
                                        oldValue = fmanger.GetFieldConfigurationData(field, Convert.ToString(drUser[arrItem[1]]));
                                        if (string.IsNullOrEmpty(oldValue))
                                            oldValue = "[No Value]";

                                        newValue = fmanger.GetFieldConfigurationData(field, Convert.ToString(drNewUser[arrItem[1]]));
                                        if (string.IsNullOrEmpty(newValue))
                                            newValue = "[No Value]";

                                        sbHistory.AppendLine($"{arrItem[1]} : {oldValue} => {newValue}");
                                    }
                                    else
                                    {
                                        oldValue = Convert.ToString(drUser[arrItem[0]]);
                                        if (string.IsNullOrEmpty(oldValue))
                                            oldValue = "[No Value]";

                                        newValue = Convert.ToString(drNewUser[arrItem[0]]);
                                        if (string.IsNullOrEmpty(newValue))
                                            newValue = "[No Value]";

                                        sbHistory.AppendLine($"{arrItem[0]} : { oldValue } => { newValue }");
                                    }
                                }

                                uHelper.CreateHistory(dbContext.CurrentUser, Convert.ToString(sbHistory), drUser, dbContext);

                                user.History = Convert.ToString(drUser[DatabaseObjects.Columns.History]);
                            }

                            IdentityResult result = umanager.Update(user);
                            if (result.Succeeded)
                            {
                                umanager.UpdateIntoCache(user);
                                if (!user.Enabled)
                                {
                                    allocationManager.UpdateAllocationForDisabledUsers(user.Id);
                                }
                                //confirmDisablePopup.ShowOnPageLoad = true;
                                //lblinformativeMsg.Text = "Save Successfully";
                                if (lstHistory.Count > 0)
                                {
                                    for (int i = 0; i < lstHistory.Count; i++)
                                    {
                                        if (lstHistory[i].Contains(";"))
                                            lstHistory[i] = lstHistory[i].Substring(lstHistory[i].IndexOf(';') + 1);
                                    }
                                    Util.Log.ULog.WriteUGITLog(dbContext.CurrentUser.Id, $"{user.Name}'s profile updated with fields: {string.Join(uGovernIT.Utility.Constants.Separator6, lstHistory)}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.UserProfile), dbContext.TenantID);
                                }
                                uHelper.ClosePopUpAndEndResponse(Context, true);
                            }
                        }
                    }
                }
            }
        }

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            if (Request["ismail"] == "1")
            {

                //uHelper.ClosePopUpAndEndResponse(Context,false, "/Pages/userhomepage/");

                //Response.Redirect("/Pages/userhomepage");
                uHelper.ClosePopUpAndEndResponse(Context, false);
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>GotoParentWindow('ismail');</Script>", false);

            }
            else if (Request["ismail"] == "0")
            {
                uHelper.ClosePopUpAndEndResponse(Context, true, "/sitePages/RMM/");
                //uHelper.ClosePopUpAndEndResponse(Context, false);
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>GotoParentWindow('istrailuser');</Script>", false);
            }
            else
                uHelper.ClosePopUpAndEndResponse(Context, false, "/sitePages/RMM/");

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
                    if (!umanager.isCurrentUser(u.Id)) { umanager.DeleteUserById(u.Id); }
                    else { umanager.DeleteUserById(u.Id); }

                    umanager.UpdateIntoCache(u);
                }
                string logMessage = string.Format("Removed user: {0} from site collection", u.Name);
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
            if (uHelper.IsCPRModuleEnabled(dbContext))
            {
                lnkassets.Visible = false;
                lnktimeline.Visible = false;
                btntimeline.Visible = false;
            }
            if (selectedDepartment == null){
                if (!string.IsNullOrEmpty(userID))
                {
                    UserProfile userInfo = umanager.GetUserInfoById(userID);
                    selectedDepartment = objDepartmentManager.LoadByID(UGITUtility.StringToLong(userInfo.Department));
                }
            }
            if (selectedDepartment != null)
            {
                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(selectedDepartment.DivisionIdLookup)))
                {
                    int selectedDivision = 0;
                    ddlStudio.FilterExpression = $"{DatabaseObjects.Columns.DivisionLookup} = '{selectedDivision}'";
                    ddlStudio.devexListBox.DataBind();
                }
            }
        }
        protected void DDLLocation_Load()
        {
            #region old code
            /*
                //DataTable locations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location);
                DataTable locations = UGITUtility.ToDataTable(ObjLocationManager.Load());

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
                    glLocation.GridView.ExpandAll();
                }
                */
            #endregion

            List<Location> locations = new List<Location>();

            /*if (Request.QueryString.AllKeys.Contains("newUser"))
                locations = ObjLocationManager.Load().Where(x => x.Deleted == false).ToList();
            else if (Request.QueryString.AllKeys.Contains("UpdateUser"))*/

            locations = ObjLocationManager.Load().Where(x => x.Deleted == false).ToList();
            if (locations != null && locations.Count > 0)
            {
                glLocation.GridView.HtmlRowPrepared += GlLocation_HtmlRowPrepared;

                int groupIndex = 0;

                if (locations.Where(x => !string.IsNullOrEmpty(x.Region)).Count() > 0)
                {
                    glLocation.Columns[0].Visible = true;
                    ((GridViewDataTextColumn)glLocation.Columns[0]).GroupIndex = groupIndex;
                    groupIndex++;
                }

                if (locations.Where(x => !string.IsNullOrEmpty(x.Country)).Count() > 0)
                {
                    glLocation.Columns[1].Visible = true;
                    ((GridViewDataTextColumn)glLocation.Columns[1]).GroupIndex = groupIndex;
                    groupIndex++;
                }

                if (locations.Where(x => !string.IsNullOrEmpty(x.State)).Count() > 0)
                {
                    glLocation.Columns[2].Visible = true;
                    ((GridViewDataTextColumn)glLocation.Columns[2]).GroupIndex = groupIndex;
                    groupIndex++;
                }

                glLocation.DataSource = locations;
                glLocation.DataBind();
                glLocation.GridView.Width = 525;
                glLocation.GridView.ExpandAll();
            }
        }

        protected void tbSkills_Load()
        {
            CmbSkillCat.DataSource = spUserSkillList.GroupBy(o => o.CategoryName).Select(x => x.FirstOrDefault());
            CmbSkillCat.TextField = DatabaseObjects.Columns.CategoryName;
            CmbSkillCat.ValueField = DatabaseObjects.Columns.CategoryName;
            CmbSkillCat.DataBind();

            List<UserSkills> tempData = userSkillManager.Load();
            tempData.ForEach(o => o.Title = o.CategoryName + ">>" + o.Title);
            tbSkills.DataSource = tempData;
            tbSkills.TextField = DatabaseObjects.Columns.Title;
            tbSkills.ValueField = DatabaseObjects.Columns.ID;
            tbSkills.DataBind();
        }

        protected void tbExperiencedTags_Load()
        {
            tbExperiencedTag.DataSource = spExperiencedTagList;
            tbExperiencedTag.TextField = DatabaseObjects.Columns.Title;
            tbExperiencedTag.ValueField = DatabaseObjects.Columns.ID;
            tbExperiencedTag.DataBind();
        }

        protected void tbCertificate_Load()
        {
            CmbCertificateCat.DataSource = spUserCertificateList.GroupBy(o => o.CategoryName).Select(x => x.FirstOrDefault());
            CmbCertificateCat.TextField = DatabaseObjects.Columns.CategoryName;
            CmbCertificateCat.ValueField = DatabaseObjects.Columns.CategoryName;
            CmbCertificateCat.DataBind();
            
            List<UserCertificates> tempData = userCertificateManager.Load();
            tempData.ForEach(o => o.Title = o.CategoryName + ">>" + o.Title);
            tbCertificate.DataSource = tempData;
            tbCertificate.TextField = DatabaseObjects.Columns.Title;
            tbCertificate.ValueField = DatabaseObjects.Columns.ID;
            tbCertificate.DataBind();
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
                #region Old Code

                //DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories);
                //if (budgets != null && budgets.Rows.Count > 0)
                //{
                //    DataTable budgetCategories = budgets.AsDataView().ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                //    foreach (DataRow row in budgetCategories.Rows)
                //    {
                //        ddlBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName]), Convert.ToString(row[DatabaseObjects.Columns.BudgetCategoryName])));
                //    }
                //    ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
                //}

                #endregion

                List<BudgetCategory> listBudgetCategory = budgetCataegoryManager.GetConfigBudgetCategoryData().ToList();
                if (listBudgetCategory != null && listBudgetCategory.Count() > 0)
                {
                    var BudgetCategories = listBudgetCategory.OrderBy(x => x.BudgetCategoryName).Select(x => x.BudgetCategoryName).Distinct().ToList();
                    if (BudgetCategories != null && BudgetCategories.Count > 0)
                    {
                        ddlBudgetCategory.DataSource = BudgetCategories;
                        ddlBudgetCategory.DataBind();
                    }
                }

                ddlBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
        }

        protected void DDLBudgetCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubBudgetCategory.Items.Clear();

            List<BudgetCategory> listBudgetCategory = budgetCataegoryManager.GetConfigBudgetCategoryData().Where(x => x.BudgetCategoryName == ddlBudgetCategory.SelectedValue).ToList();


            //  DataTable budgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories,$"TenantID='{dbContext.TenantID}'" );
            //if (budgets != null && budgets.Rows.Count > 0)
            //{
            //    DataRow[] budgetCategories = budgets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategoryName, ddlBudgetCategory.SelectedValue));
            //    foreach (DataRow row in budgetCategories)
            //    {
            //        ddlSubBudgetCategory.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetSubCategory]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
            //    }
            //    ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
            //}

            if (listBudgetCategory != null && listBudgetCategory.Count() > 0)
            {
                ddlSubBudgetCategory.DataSource = listBudgetCategory;
                ddlSubBudgetCategory.DataTextField = DatabaseObjects.Columns.BudgetSubCategory;
                ddlSubBudgetCategory.DataValueField = DatabaseObjects.Columns.Id;
                ddlSubBudgetCategory.DataBind();
                ddlSubBudgetCategory.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
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
            //  string path = "";
            UserProfile userDelegateFor = null;
            if (!Page.IsValid)
                return;

            UserProfile user = umanager.FindById(userID);

            this.user = user;
            if (chkOutOfOffice.Checked)
            {
                user.LeaveFromDate = LeaveFromDate.Date;
                user.LeaveToDate = LeavetoDate.Date;
                user.DelegateUserFor = DelegateUserOnLeave.GetValues();
                //user = new UserProfile(user);
                //if (!string.IsNullOrEmpty(user.DelegateUserFor))
                //{
                //    userDelegateFor = umanager.FindById(user.DelegateUserFor);
                //    userDelegateFor.DelegateUserFor = user.Id;

                //    user.DelegateUserFor = null;
                //}
                if (!string.IsNullOrEmpty(user.DelegateUserFor))
                {
                    resultdata = AddMultipleDelegateUsers();
                }
            }

            else
            {
                UncheckforOutOfOffice();

            }

            if (!resultdata)
                return;

            else
            {
                user.DisableWorkflowNotifications = chkDisableWorkflowNotifications.Checked;
                IdentityResult result = umanager.Update(user);
                if (userDelegateFor != null)
                {
                    var r = umanager.Update(userDelegateFor);
                }
                //if (result.Succeeded)
                //{
                //    //confirmDisablePopup.ShowOnPageLoad = true;
                //    //lblinformativeMsg.Text = "Save Successfully";

                //    uHelper.ClosePopUpAndEndResponse(Context, true);
                //}
                uHelper.ClosePopUpAndEndResponse(Context);
            }
        }


        protected void btnDeleteFromGroup_Click(object sender, EventArgs e)
        {
            if (Request["uID"] != null)
            {

            }
        }

        protected bool AddMultipleDelegateUsers()
        {

            UserProfile userDelegateFor = umanager.FindById(user.DelegateUserFor);
            //  if (user.LeaveFromDate != null && (user.LeaveFromDate.Date <= userDelegateFor.LeaveFromDate.Date && user.LeaveToDate >= userDelegateFor.LeaveFromDate || userDelegateFor.LeaveToDate.Date >= user.LeaveFromDate.Date))
            if (user.LeaveFromDate != null && (user.LeaveToDate < userDelegateFor.LeaveFromDate || user.LeaveFromDate > userDelegateFor.LeaveToDate))
            {
                if (user.LeaveFromDate.Month >= userDelegateFor.LeaveFromDate.Month)
                {
                    if (!string.IsNullOrEmpty(userDelegateFor.DelegateUserFor))
                    {
                        arrayDelegateUserFor = userDelegateFor.DelegateUserFor.Split(',').Distinct().ToArray();
                        foreach (var id in arrayDelegateUserFor)
                        {
                            userDelegateFor.DelegateUserFor = string.Join(",", arrayDelegateUserFor.Append(user.Id).Distinct());
                        }
                    }
                    else
                        userDelegateFor.DelegateUserFor = user.Id;
                    user.DelegateUserOnLeave = user.DelegateUserFor;
                    //user.DelegateUserFor = null;
                    var r = umanager.Update(userDelegateFor);
                    return true;
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('check the Month');", true);
                return false;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('" + userDelegateFor.UserName + " is on leave from  " + userDelegateFor.LeaveFromDate.ToString("d") + " to " + userDelegateFor.LeaveToDate.ToString("d") + "');", true);
                return false;
            }

        }

        protected void ShowMultiDelegateUsers()
        {
            arrayDelegateUserFor = new string[] { userInfo.DelegateUserFor };

            List<UserProfile> userList = umanager.GetUserInfosById(string.Join(",", arrayDelegateUserFor));

            string multiName = string.Empty;
            foreach (var userName in userList)
            {
                multiName = multiName != "" ? multiName = multiName + "," + userName.Name : multiName + userName.Name;
            }
            lblDelegatedTaskFor.Text = multiName;
            delgateUserForPanelRead.CssClass = "readdegatefor";
        }

        private string GetExpirationPeriodMessage(UserProfile userInfo, int passwordExpirationPeriod)
        {
            if (userInfo != null && userInfo.PasswordExpiryDate != DateTime.MinValue && Convert.ToString(userInfo.PasswordExpiryDate) != "1/1/1754 12:00:00 AM")
            {
                return string.Format("Expiration Period: {0} days", (userInfo.PasswordExpiryDate - DateTime.Now).Days + 1);
            }
            else
            {
                userInfo.PasswordExpiryDate = DateTime.Today.AddDays(passwordExpirationPeriod);
            }

            return string.Format("Expiration Period: {0} days", passwordExpirationPeriod);
        }

        private DateTime SetpasswordExpirationDate()
        {
            int.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
            if (passwordExpirationPeriod == 0)
                passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;

            if (Convert.ToString(dtcPwdExpiryDate.Value) == "1/1/1754 12:00:00 AM" || string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)))
            {
                return DateTime.Today.AddDays(passwordExpirationPeriod);
            }
            else if (Convert.ToString(dtcPwdExpiryDate.Value) != "1/1/1754 12:00:00 AM" && !string.IsNullOrEmpty(Convert.ToString(dtcPwdExpiryDate.Value)))
            {
                return (DateTime)dtcPwdExpiryDate.Value;
            }
            else
            {
                return DateTime.Today.AddDays(passwordExpirationPeriod);
            }
        }

        private int GetPasswordExpirationPeriod()
        {
            int.TryParse(ConfigurationVariableManager.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
            if (passwordExpirationPeriod == 0)
                passwordExpirationPeriod = Utility.Constants.DefaultPasswordExpirationPeriod;

            return passwordExpirationPeriod;
        }

        private void BindRoles()
        {
            try
            {
                // userRoles = new UserRolesManager(HttpContext.Current.GetManagerContext());
                List<LandingPages> dtLandingPages = userPages.GetLandingPages().Where(x => x.Deleted == false).OrderBy(x => x.Name).ToList();
                if (dtLandingPages != null && dtLandingPages.Count > 0)
                {
                    ddlUserRole.DataTextField = "Name";
                    ddlUserRole.DataValueField = "ID";
                    ddlUserRole.DataSource = dtLandingPages;
                    ddlUserRole.DataBind();
                }

                ddlUserRole.Items.Insert(0, new ListItem("(None)", ""));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UncheckforOutOfOffice()
        {
            user.LeaveFromDate = DateTime.MinValue.AddYears(1753);
            user.LeaveToDate = DateTime.MinValue.AddYears(1753);
            userDelegateFor = umanager.FindById(user.DelegateUserOnLeave);
            user.DelegateUserOnLeave = null;
            if (userDelegateFor != null)
            {
                userDelegateFor.DelegateUserFor = null;
                var r = umanager.Update(userDelegateFor);
            }
        }

        protected void cmbLoadJobLookup(long DeptId = 0)
        {
            cmbJobTitle.Items.Clear();
            userInfo = umanager.GetUserById(Request["uID"]);
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dt = new DataTable();
            values.Add("@TenantID", dbContext.TenantID);
            values.Add("@Dept", DeptId);
            dt= GetTableDataManager.GetData("JobtitlebyDept", values);
            if (dt != null && dt.Rows.Count > 0)
            {
                cmbJobTitle.DataSource = dt;
                cmbJobTitle.TextField = DatabaseObjects.Columns.Title;
                cmbJobTitle.ValueField = DatabaseObjects.Columns.ID;
                if (DeptId > 0 && userInfo != null)
                    cmbJobTitle.Text = userInfo.JobProfile;
                cmbJobTitle.DataBind();
       

            }
            //List<JobTitle> jobTitles = null;
            //jobTitles = jobTitleMGR.Load(x => x.DepartmentId == DeptId && x.Deleted == false);

            //if (jobTitles != null && jobTitles.Count > 0)
            //{
            //    cmbJobTitle.DataSource = jobTitles;
            //    cmbJobTitle.TextField = DatabaseObjects.Columns.Title;
            //    cmbJobTitle.ValueField = DatabaseObjects.Columns.ID;
            //    cmbJobTitle.DataBind();

            //}
        }

        private void SendEmailToNewApplicaitonRequester(string emailID, string accountID, string userName, string password, string encryUserId = null, string encrTenAccId = null, string encrword = null, string name = null)
        {
            string subject = ConfigurationVariableManager.GetValue(ConfigConstants.CreateUserMailSubject);
            if (string.IsNullOrEmpty(subject))
                subject = "Your registration is successful!";
            
            //string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}Account/Login.aspx";
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/UserVerfication?id={encryUserId}&acc={encrTenAccId}&di={encrword}";
            string userCredentials = $"<br /> Account Id : {accountID} <br /> User Name : {userName} <br /> Password : {password}";
            string CurrentUserName = dbContext.CurrentUser.Name;

            string htmlBody = ConfigurationVariableManager.GetValue(ConfigConstants.CreateUserMailBody);
            if (!string.IsNullOrEmpty(htmlBody))
            {
                htmlBody = htmlBody.Replace("[$name$]", name).Replace("[$userCredentials$]", userCredentials).Replace("[$SiteUrl$]", SiteUrl).Replace("[$CurrentUserName$]", CurrentUserName);
            }
            else if (string.IsNullOrEmpty(htmlBody))
            {
                htmlBody = @"<html>
                            <head></head>
                            <body>
                                <p> Dear " + name + @": ,<br />You are set up as a user of Service Prime. <br /><br />You can create IT requests using a simple portal that is now set up for you. <br><br> This email contains your details: <strong> " + userCredentials + @" </strong>
                                    <br /><br />
                                    Please <a href=" + SiteUrl + @">click here </a> to activate your account. 
                                </p>
                                <p>
                                    Thanks,<br /> " + CurrentUserName + @"<br />Service Prime.
                                </p>
                            </body>
                        </html>";
            }
            var mail = new MailMessenger(dbContext);

            var response = mail.SendMail(emailID, subject, "", htmlBody, true, new string[] { }, true, false);
            //var response = mail.SendMail(emailID, subject, "", htmlBody, true);
        }

        /// <summary>
        /// Validate user with user name and email combination from Admin->Configuration variable->UniqueAccountForUser
        /// </summary>

        protected void cvUserName_ServerValidate(object source, ServerValidateEventArgs args)
        {


            string[] validationconstraint = ValidationParameter();

            foreach (string validationtype in validationconstraint)
            {
                //need to test this how it works after uncommenting
                if (ConfigConstants.UserName.EqualsIgnoreCase(validationtype))
                {
                    var userInfoByName = umanager.FindByName(args.Value, HttpContext.Current.GetManagerContext().TenantID);

                    args.IsValid = userInfoByName != null ? false : true;
                }
                //else if (ConfigConstants.Email.EqualsIgnoreCase(validationtype))
                //{
                //    var userInfoByMail = umanager.FindByEmail(args.Value, HttpContext.Current.GetManagerContext().TenantID);

                //    if (userInfoByMail != null)

                //        args.IsValid = false;

                //    else
                //        args.IsValid = true;
                //}

            }
            //UserValidation(UserProfile userInfo);
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string[] validationconstraint = ValidationParameter();
            //if (validationconstraint.Count() > 1)
            //{

            //    //comination of username+email;
            //}
            //else
            //{


            foreach (string validationtype in validationconstraint)
            {

                if (ConfigConstants.Email.EqualsIgnoreCase(validationtype))
                {
                    if (string.IsNullOrEmpty(userID) || userID.Equals("0"))
                    {
                        var userInfoByMail = umanager.FindByEmail(args.Value, HttpContext.Current.GetManagerContext().TenantID);
                        args.IsValid = userInfoByMail != null ? false : true;
                    }
                    else
                    {
                        var userInfoByMail = umanager.FindByEmailIgnoreCurrentUser(args.Value, HttpContext.Current.GetManagerContext().TenantID, userID);
                        args.IsValid = userInfoByMail != null ? false : true;
                    }

                }

            }

            //}

        }

        public string[] ValidationParameter()
        {

            string configValueForUser = ConfigurationVariableManager.GetValue(ConfigConstants.UniqueAccountForUser);

            string[] validationconstraint = configValueForUser.Split(',');

            return validationconstraint;

        }

        private bool ValidateNewUsername(string newUserName, out string message)
        {
            message = string.Empty;

            //DefaultUser
            if (Convert.ToString(ConfigurationManager.AppSettings["DefaultUser"]).EqualsIgnoreCase(newUserName))
            {
                message = "Cannot change User Name. Contact Administrator.";
                return false;
            }
            else
            {
                //UserProfile profile = umanager.GetUserByUserName(newUserName);
                UserProfile profile = umanager.GetUserOnlyByUserName(newUserName);
                if (profile != null)
                {
                    message = "User Name already exists.";
                    return false;
                }
                return true;
            }
        }

        public void BindEmployeeType()
        {
            EmployeeTypeManager ObjEmployeeTypeManager = new EmployeeTypeManager(HttpContext.Current.GetManagerContext());
            List<EmployeeTypes> lstemptype = ObjEmployeeTypeManager.Load().Where(x => x.Deleted == false).OrderBy(x => x.Title).ToList();
            if (lstemptype != null && lstemptype.Count > 0)
            {
                ddlEmpType.DataSource = lstemptype;
                ddlEmpType.DataTextField = "Title";
                ddlEmpType.DataValueField = "ID";
                ddlEmpType.DataBind();
                ddlEmpType.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
            else
            {
                ddlEmpType.Items.Insert(0, new ListItem("(None)", string.Empty));
            }
        }
        protected void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            DateTime maxDate = new DateTime(8900, 12, 31);
            if (chkEnable.Checked == false)
            {
                dtcEndDate.Value = DateTime.Now;
            }
            else
            {
                dtcEndDate.Value = maxDate;
            }

        }

        protected void btnResumePdfExport_Click(object sender, EventArgs e)
        {
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=User Resume.pdf");
            Response.TransmitFile(Server.MapPath(hdnResume.Value));
            Response.End();

        }

        protected void btnDeleteResume_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnResume.Value))
            {
                userID = Request["uID"];
                UserProfile userInfoResume = umanager.GetUserById(userID);
                string deleteFilePath = Server.MapPath(hdnResume.Value);
                FileInfo file = new FileInfo(deleteFilePath);
                if (file.Exists)
                {
                    file.Delete();
                }
                userInfoResume.Resume = "";
                umanager.Update(userInfoResume);
                dvResume.Visible = false;
            }
        }

        protected void tbCertificateValue_Callback(object sender, CallbackEventArgsBase e)
        {
            tbCertificateValue.DataSource = spUserCertificateList.Where(o => o.CategoryName == e.Parameter);
            tbCertificateValue.TextField = DatabaseObjects.Columns.Title;
            tbCertificateValue.ValueField = DatabaseObjects.Columns.ID;
            tbCertificateValue.DataBind();
        }

        protected void tbSkillValue_Callback(object sender, CallbackEventArgsBase e)
        {
            tbSkillValue.DataSource = spUserSkillList.Where(o => o.CategoryName == e.Parameter);
            tbSkillValue.TextField = DatabaseObjects.Columns.Title;
            tbSkillValue.ValueField = DatabaseObjects.Columns.ID;
            tbSkillValue.DataBind();
        }

        protected void cmbExperiencedTag_Callback(object sender, CallbackEventArgsBase e)
        {

        }
        protected void PnlCallbackJobTitle_Role_Callback(object sender, CallbackEventArgsBase e)
        {
            JobTitleSelectedVal = e.Parameter;
            //object jobTitleId = cmbJobTitle.Value;
            //DataTable dtJobTitle = uGITDAL.GetTable("JobTitle", "ID=" + Convert.ToInt64(jobTitleId), "RoleId");
            //DataTable dtRollName = uGITDAL.GetTable("Roles", "ID='" + dtJobTitle.Rows[0][0] + "'", "Name");
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dtRollName = new DataTable();
            values.Add("@TenantID", dbContext.TenantID);
            values.Add("@JobTitleLookup", Convert.ToString(e.Parameter));
            dtRollName = GetTableDataManager.GetData("Roleby_Dept_Job", values);
            if (dtRollName.Rows.Count > 0)
            {
                txtUsersRole.Text = dtRollName.Rows[0][0].ToString();
            }
            else
                txtUsersRole.Text = string.Empty;
        }
    }
}
