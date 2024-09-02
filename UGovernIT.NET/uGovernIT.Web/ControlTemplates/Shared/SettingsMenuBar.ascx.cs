using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility.DockPanels;
using Constants = uGovernIT.Utility.Constants;
using uGovernIT.Utility.Entities.Common;
using System.Web.UI.HtmlControls;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class SettingsMenuBar : UserControl
    {
        public bool UgitHideGlobalSearch { get; set; }
        public TaskDockPanelSetting ticketDockPanelSetting { get; set; }

        public long WarningCount { get; set; }
        public long Criticalcount { get; set; }
        public long Sucesscount { get; set; }
        public bool ShowHelpButton { get; set; }
        public bool ShowAccountName { get; set; }

        string userID = "";
        string urlThemeControl;
        protected ApplicationContext _context = null;
        private LandingPagesManager _LandingPagesManager = null;
        private MessageBoardManager _objMessageBoardManager=null;
        private UserRoleManager _userRolesManager = null;
        private UserProfileManager _userProfileManager = null;


        protected string filterPage;
        protected string filterPageNew;
        protected string UserLandingPage = "/";
        protected string Groupdefaultlandingpage = "/";
        protected string defaultrole = "";
        protected string defaultroleTitle = "";
        public string clientProfilePer = "";
        public string redirectToclientProfile = "";
        public bool isProgressBarActivated = false;

        public string ajaxPageURL = string.Empty;
        public string ajaxAPIURL = string.Empty;



        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected LandingPagesManager LandingPagesManager
        {
            get
            {
                if (_LandingPagesManager == null)
                {
                    _LandingPagesManager = new LandingPagesManager(ApplicationContext);
                }
                return _LandingPagesManager;
            }
        }

        protected MessageBoardManager objMessageBoardManager
        {
            get
            {
                if (_objMessageBoardManager == null)
                {
                    _objMessageBoardManager = new MessageBoardManager(ApplicationContext);
                }
                return _objMessageBoardManager;
            }
        }

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(ApplicationContext);
                }
                return _userProfileManager;
            }
        }
       
        protected UserRoleManager UserRolesManager
        {
            get
            {
                if (_userRolesManager == null)
                {
                    _userRolesManager = new UserRoleManager(ApplicationContext);
                }
                return _userRolesManager;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            var configurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

            // filterPage = UGITUtility.GetAbsoluteURL(ConfigurationVariableHelper.GetValue("filterticketspageurl"));
            filterPage = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=cutomfilterTicket");
            filterPageNew = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=globalsearchpage");
            imageugovernIilogo.ImageUrl = configurationVariableHelper.GetValue(ConfigConstants.HeaderLogo);
            string AccountName = configurationVariableHelper.GetValue("AccountName");
            if (!string.IsNullOrEmpty(AccountName))
            {
                accountName.Visible = true;
                accountName.Text = AccountName;
                imageugovernIilogo.ToolTip = AccountName;
            }
            else
            {
                accountName.Visible = true;
                accountName.Text = HttpContext.Current.GetManagerContext().TenantAccountId;
                imageugovernIilogo.ToolTip = accountName.Text;
            }
            if (string.IsNullOrWhiteSpace(imageugovernIilogo.ImageUrl))
            {
                imageugovernIilogo.ImageUrl = "/content/images/Service_Prime_Logo.svg";
            }

            ShowHelpButton = false;
            ShowAccountName = false;
            if (bool.TryParse(configurationVariableHelper.GetValue(ConfigConstants.showDefaultHelpPageButton), out bool showDefaultHelpPageButton) && showDefaultHelpPageButton)
            {
                ShowHelpButton = true;
            }
            if (!string.IsNullOrEmpty(AccountName))
            {
                ShowAccountName = true;
            }

            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/phraseSearchlobalTicketCreate/");
            ajaxAPIURL = UGITUtility.GetAbsoluteURL("/api/Account/");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //SettingsMenuBar settingsMenuBar = (SettingsMenuBar)Page.LoadControl("ControlTemplates/Shared/SettingsMenuBar.ascx");
            // settingsMenuBar.taskDockPanelSetting = dockPanel as TicketDockPanelSetting;
            #region messageBorad 
            List<MessageBoard> MessageBoardList = null;
            //List<MessageBoard> MessageBoardList = objMessageBoardManager.Load();
            MessageBoardList = (List<MessageBoard>)CacheHelper<object>.Get($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID);
            if (MessageBoardList == null)
            {
                MessageBoardList = objMessageBoardManager.Load();
                var modifiedBy = string.Join(",", MessageBoardList.Select(x => x.ModifiedBy).Distinct().ToList());
                UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                List<UserProfile> userInfoList = UserManager.GetUserInfosById(modifiedBy);
                foreach (var dataList in MessageBoardList)
                {
                    var userName = userInfoList.Where(x => x.Id == dataList.ModifiedBy).Select(x => x.Name).FirstOrDefault();
                    dataList.ModifiedBy = userName;
                }
                CacheHelper<object>.AddOrUpdate($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID, MessageBoardList);
            }
            List<MessageBoard> messageAlert = new List<MessageBoard>();
            List<MessageBoard> messageWarning = new List<MessageBoard>();
            List<MessageBoard> messageSuccess = new List<MessageBoard>();
            if (MessageBoardList.Count > 0)
            {
                messageAlert = MessageBoardList.Where(x => x.MessageType == Constants.MessageTypeValues.Critical && (x.Expires >= DateTime.Now ||x.Expires== null)).ToList();
                
                Criticalcount = messageAlert.Count;
                if (messageAlert.Count > 0)
                {
                    //DataTable messageList = UGITUtility.ToDataTable(messageAlert);
                    MyMessageAlert.DataSource = messageAlert;
                    MyMessageAlert.DataBind();
                }
                messageWarning = MessageBoardList.Where(x => x.MessageType == Constants.MessageTypeValues.Warning && (x.Expires >= DateTime.Now || x.Expires == null)).ToList();
                WarningCount = messageWarning.Count;
                if (messageWarning.Count > 0)
                {
                    //DataTable messageList = UGITUtility.ToDataTable(messageWarning);
                    MyMessageWarning.DataSource = messageWarning;
                    MyMessageWarning.DataBind();
                }

                messageSuccess = MessageBoardList.Where(x => (x.MessageType == Constants.MessageTypeValues.Ok && (x.Expires >= DateTime.Now || x.Expires == null) )||( x.MessageType == Constants.MessageTypeValues.Information && (x.Expires >= DateTime.Now || x.Expires == null))).ToList();
                Sucesscount = messageSuccess.Count;
                if (messageSuccess.Count > 0)
                {
                    //DataTable messageList = UGITUtility.ToDataTable(messageSuccess);
                    MyMessageSuccess.DataSource = messageSuccess;
                    MyMessageSuccess.DataBind();
                }



            }
            #endregion

            if (Session["isFromSuprAdmin"] != null)
            {
                ViewState["isFromSuprAdmin"] = Session["isFromSuprAdmin"].ToString(); 
                popupMenuLogInUser.Items.FindByName("SuperAdminLogin").Visible = true;
            }
            else
            {
                popupMenuLogInUser.Items.FindByName("SuperAdminLogin").Visible = false;
            }
            //claim code start
            //var identity = (System.Security.Claims.ClaimsIdentity)Context.User.Identity;
            //var claimIsFromSuperAdmin = identity.Claims.FirstOrDefault(c => c.Type == "IsFromSuperAdmin");
            //if (claimIsFromSuperAdmin != null && claimIsFromSuperAdmin.Value != null)
            //{
            //    popupMenuLogInUser.Items.FindByName("SuperAdminLogin").Visible = true;
            //}
            //else
            //{
            //    popupMenuLogInUser.Items.FindByName("SuperAdminLogin").Visible = false;
            //}
            //claim code end

            if (Page.User.Identity.IsAuthenticated)
            {
                #region pageTitle
                pageTitle.Visible = false;
                        if ((Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeSuperAdmin")||(Page.Request.AppRelativeCurrentExecutionFilePath == "~/SuperAdmin/SuperAdmin.aspx"))
                    pageTitle.Text = "Super Admin";

                if (Page.AppRelativeVirtualPath == "~/Admin/NewAdminUI.aspx")
                    pageTitle.Text = "Administrator";

                if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/TrialUser")
                    pageTitle.Text = "Trial User";

                if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomePM" || Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomePMO")
                    pageTitle.Text = "My Requests";
                if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeTasks" || Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeUser")
                    pageTitle.Text = "My Tasks";


                if (Page.Request.QueryString.Count > 0)
                {
                    if(Page.Request.QueryString.AllKeys.Contains("tkt"))
                    {
                        if (Page.Request.QueryString["tkt"].ToString() == "tkt")
                        {
                            pageTitle.Text = "My Tasks";

                        }
                        if (Page.Request.QueryString["tkt"].ToString() == "morit")
                        {
                            pageTitle.Text = "More then Just IT";

                        }

                        if (Page.Request.QueryString["tkt"].ToString() == "newt")
                        {
                            pageTitle.Text = "Technologies Enabled Services";

                        }

                    }


                }

                #endregion
                lblLogInUser.Text = Page.User.Identity.Name.ToString();
                //var userId = HttpContext.Current.User.Identity.GetUserId();
                UserProfile user = HttpContext.Current.CurrentUser();
                UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                var applicationContext = HttpContext.Current.GetManagerContext();
                List<UserProfile> userList = applicationContext.UserManager.GetUsersProfile();
                user = userList.Where(x => x.Id == user.Id).FirstOrDefault();
                HttpContext.Current.GetManagerContext().CurrentUser.UserRoleId = user.UserRoleId;
                var userRoleList = userManager.GetUserRoles(user.Id);
                if (ApplicationContext != null && !string.IsNullOrEmpty(user.UserRoleId))
                {
                    
                    var landingPageUrl = LandingPagesManager.GetLandingPageById(user.UserRoleId);
                    
                    if (!string.IsNullOrEmpty(landingPageUrl))
                    {
                        UserLandingPage = landingPageUrl;
                        
                    }
                }
                
                if (user != null)
                {
                    //client Profile code start
                    if (userManager.IsAdmin(user))
                    {
                        isProgressBarActivated = true;
                        TenantStatusHelper tenantsatusHelper = new TenantStatusHelper(applicationContext);
                        StatusSummary statusSummary = new StatusSummary();
                        statusSummary = tenantsatusHelper.GetProfileStatusSummary(applicationContext.TenantID);
                        int ClientProgressPercentage = 0;
                        ClientProgressPercentage = tenantsatusHelper.GetProfileCompletePercent(statusSummary);
                        clientProfilePer = ClientProgressPercentage.ToString();
                        string statusred = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=status")) + "&tenantId=" + applicationContext.TenantID; 
                        redirectToclientProfile = string.Format("JavaScript:UgitOpenPopupDialog('{0}','','Client Profile','625px','700px','','')", statusred);
                        

                    }//client Profile status 
                    

                    lblLogInUser.Text = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserName;

                    if (File.Exists(Server.MapPath(user.Picture)))
                    {
                        imgUserProfile.ImageUrl = user.Picture;
                       // imgUserProfilexs.ImageUrl = user.Picture;
                    }

                    else
                    {
                        imgUserProfile.ImageUrl = @"/Content/images/userNew.png";
                       // imgUserProfilexs.ImageUrl = @"/Content/images/userNew.png";
                    }
                }
                if (user == null)
                {
                    Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    Response.Redirect("/Account/Login.aspx");

                }
                userID = user.Id.ToString();
                bool isAdmin = userManager.IsAdmin(user) || userManager.IsUGITSuperAdmin(user);
                if (isAdmin)
                {
                    //popupMenuLogInUser.Items.FindByName("Admin").Visible = true;
                    //popupMenuLogInUser.Items.FindByName("ChangeLook").Visible = true;
                    popupMenuLogInUser.Items.FindByName("Admin").Visible = true;
                }
                if (HttpContext.Current.Request.Browser.IsMobileDevice)
                {
                    popupMenuLogInUser.Items.FindByName("LoginUser").Visible = true;
                    //popupMenuLogInUser.Items.FindByName("LoginUser").Text = Page.User.Identity.Name.ToString();
                    popupMenuLogInUser.Items.FindByName("LoginUser").Text = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserName;
                }
                else
                {
                    popupMenuLogInUser.Items.FindByName("LoginUser").Visible = false;
                }
                //ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                //string fullName = user.UserName;
                string url = "javascript:window.parent.UgitOpenPopupDialog('/ControlTemplates/RMM/userinfo.aspx?uID=" + userID + "&UpdateUser=1', '', 'User Details:" + lblLogInUser.Text + "', '600px', '90', 'false', 'sitePages RMM ugroup')";
                popupMenuLogInUser.Items.FindByName("MyProfile").NavigateUrl = url;
            }
            else
            {
                lblLogInUser.Text = "No Logged In";
            }
            string absPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=setugittheme");
            string height = "90";
            string width = "90";
            string source = Uri.EscapeDataString(Request.Url.AbsolutePath);
            urlThemeControl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '{4}')", absPath, "Set Theme", width, height, source);
            //popupMenuLogInUser.Items.FindByName("ChangeLook").NavigateUrl = urlThemeControl;
            popupMenuLogInUser.Items.FindByName("Admin").NavigateUrl = "~/Admin/Admin.aspx";
            //popupMenuLogInUser.Items.FindByName("Admin").NavigateUrl = "~/Admin/NewAdminUI.aspx";

            string absPathChangePassword = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=changepassword");
            string heightt = "70";
            string widthh = "30";
            string sourcee = Uri.EscapeDataString(Request.Url.AbsolutePath);
            string PasswordControl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','{2}','{3}', false, '{4}')", absPathChangePassword, "Change Password", widthh, heightt, sourcee);
            popupMenuLogInUser.Items.FindByName("Change Password").NavigateUrl = PasswordControl;


            GlobalSearch.Visible = true;

            if (UgitHideGlobalSearch == true)
            {
                GlobalSearch.Visible = false;
            }
            DataTable dummy = null;
            BuildMenu(ASPxMenu1, dummy);
        }

        protected void popupMenuLogInUser_ItemClick(object source, MenuItemEventArgs e)
        {
            if (e.Item == null)
                return;
            //if(e.Item.Name == Utility.Constants.SettingMenuType.ClientProfile)
            //{
            //    e.Item.n
            //}

            if (e.Item.Name == Utility.Constants.SettingMenuType.SignOut)
            {
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //Util.Cache.CacheHelper<object>.Clear();
                Session["isFromSuprAdmin"] = null;
                    Response.Redirect("/Account/Login.aspx");
            }
            //if (e.Item.Name == Utility.Constants.SettingMenuType.ChangeLook)
            //{
            //    popupMenuLogInUser.Items.FindByName("ChangeLook").NavigateUrl = urlThemeControl;
            //}
        }

        protected void cbSettingMenuBar_Callback(object source, CallbackEventArgs e)
        {
            //Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
            //Response.Redirect("/Account/Login.aspx");
        }


        protected void GroupCallback_Callback(object source, CallbackEventArgs e)
        {
            Session["SelectedGroup"] = e.Parameter;
        }

        protected void BuildMenu(DevExpress.Web.ASPxMenu menu, DataTable dataSource)
        {
            // Get DataView
            //DataSourceSelectArguments arg = new DataSourceSelectArguments();
            //DataView dataView = dataSource.Select(arg) as DataView;
            //dataView.Sort = "ParentID";
            // Build Menu Items

            menu.Items.Clear();
            popupLandingPageItems.Items.Clear();

            Dictionary<string, MenuItem> menuItems = new Dictionary<string, MenuItem>();
            DataTable MenuData = BuilMenuData();
            if (MenuData.Rows.Count <= 4)
            {
                divLandingPagesPopupimage.Visible = false;
            }
            else
            {
                divLandingPagesPopupimage.Visible = true;
            }
            for (int i = 0; i < MenuData.Rows.Count; i++)
            {
                DataRow row = MenuData.Rows[i];
                MenuItem item = CreateMenuItem(row);                
                if (Convert.ToInt32(MenuData.Rows[i]["parentID"]) < 4)
                {                    
                    if (row["Group"].ToString().Replace(" ", String.Empty) == defaultrole.Replace(" ", String.Empty))
                    {
                        item.Selected = true;
                    }
                    //string itemID = row["ID"].ToString();
                    string itemID = i.ToString();
                    string parentID = row["ParentID"].ToString();
                    if (menuItems.ContainsKey(parentID))
                        menuItems[parentID].Items.Add(item);
                    else
                    {
                        if (parentID == "0")
                            menu.Items.Add(item);
                    }
                    menuItems.Add(itemID, item);
                    menu.Items.Add(item);
                }
                else
                {
                    if (!popupLandingPageItems.Items.Contains(item))
                    {
                        popupLandingPageItems.Items.Add(item);
                    }
                }
            }

        }
               
        private MenuItem CreateMenuItem(DataRow row)
        {
            MenuItem ret = new MenuItem();
            ret.Text = row["Group"].ToString();
            ret.NavigateUrl = row["landingPage"].ToString(); ;
            return ret;
        }

        protected DataTable BuilMenuData()
        {
            var MenuData = new DataTable();

            MenuData.Columns.Add("Group", typeof(string));
            MenuData.Columns.Add("landingPage", typeof(string));
            MenuData.Columns.Add("parentID", typeof(string));

            var user = HttpContext.Current.CurrentUser();

            UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            var userRoleList = userManager.GetUserRoles(user.Id);//b
            //var listUserRoles = UserRolesManager.GetRoleList();
            var listLandingPages = LandingPagesManager.GetLandingPages().Where(x => userRoleList.Any(y => y.Title.Trim() == x.Name.Trim())).OrderBy(x => x.Name).ToList();

            listLandingPages.ForEach(
                    x => {
                        var page = userRoleList.Where(y => y.Title.EqualsIgnoreCase(x.Name)).Select(z => z.LandingPage).FirstOrDefault();
                        if (!string.IsNullOrEmpty(page))
                        {
                            x.LandingPage = page;
                        }                        
                    });

            var landingPageUrl = LandingPagesManager.GetLandingPageById(user.UserRoleId);

            int i = 0;
            DataRow adminRow = null;
            foreach (var rl in listLandingPages)
            {

                if (rl.Name != "Admin" && rl.Name.ToLower() != "super admin")
                {
                    DataRow newrow = MenuData.NewRow();
                    newrow["Group"] = rl.Name;
                    if (string.IsNullOrWhiteSpace(rl.LandingPage))
                    {
                        newrow["landingPage"] = UGITUtility.GetAbsoluteURL("/Pages/HomeCore");
                    }
                    else
                    {
                        newrow["landingPage"] = UGITUtility.GetAbsoluteURL(rl.LandingPage);
                       // if(taskDockPanelSetting!=null)
                        //lblTitle.Text = taskDockPanelSetting.Title;
                    }

                    newrow["parentID"] = i;
                    MenuData.Rows.Add(newrow);
                    i = i + 1;
                }
                else if (rl.Name == "Admin")
                {
                    adminRow = MenuData.NewRow();
                    adminRow["Group"] = rl.Name;
                    //newrow["landingPage"] = "~/Admin/NewAdminUI.aspx";
                    adminRow["landingPage"] = rl.LandingPage;
                    adminRow["parentID"] = i;
                    //MenuData.Rows.Add(newrow);
                    i = i + 1;

                }
                else if (rl.Name.ToLower() == "super admin")
                {
                    DataRow newrow = MenuData.NewRow();
                    newrow["Group"] = rl.Name;
                    newrow["landingPage"] = "~/SuperAdmin/SuperAdmin.aspx";
                    newrow["parentID"] = i;
                    MenuData.Rows.Add(newrow);
                    i = i + 1;

                }

                //if(rl.Name == "CRM" || rl.Name == "CRMAdmin" || rl.Name == "Estimator" || rl.Name == "Executive" || rl.Name == "Finance" || rl.Name == "FieldOperations" || rl.Name == "PA"
                //    || rl.Name == "PM" || rl.Name == "PMAdmin" || rl.Name == "ProjectManager")
                //{

                //    DataRow newrow = MenuData.NewRow();
                //    newrow["Group"] = rl.Name;
                //    newrow["landingPage"] = UGITUtility.GetAbsoluteURL("/Pages/Home" + rl.Name);
                //    newrow["parentID"] = i;
                //    MenuData.Rows.Add(newrow);
                //    i = i + 1;
                //}
                //else if(rl.Name == "Admin")
                //{
                //    DataRow newrow = MenuData.NewRow();
                //    newrow["Group"] = rl.Name;
                //    newrow["landingPage"] = "~/Admin/Admin.aspx";
                //    newrow["parentID"] = i;
                //    MenuData.Rows.Add(newrow);
                //    i = i + 1;
                //}
                //else
                //{
                //    DataRow newrow = MenuData.NewRow();
                //    newrow["Group"] = rl.Name;
                //    newrow["landingPage"] = "";
                //    newrow["parentID"] = i;
                //    MenuData.Rows.Add(newrow);
                //    i = i + 1;
                //}

            }
            if (adminRow != null)
            {
                MenuData.Rows.Add(adminRow);
            }

            return MenuData;
        }

        

        protected void LoginToSuperAdmin_Click(object sender, EventArgs e)
        {
            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            ApplicationContext   applicationContext = ApplicationContext.Create();
            UserProfileManager userProfileManager = new UserProfileManager(applicationContext);
            UserProfile SuperAdminUser = userProfileManager.LoadById(ViewState["isFromSuprAdmin"].ToString());
            
            //claim code start - This code is better way to store superadmin Id we will use it  during security optimaztion
            //var identity = (System.Security.Claims.ClaimsIdentity)Context.User.Identity;
            //var claimIsFromSuperAdmin = identity.Claims.FirstOrDefault(c => c.Type == "IsFromSuperAdmin");
            //string parentId = claimIsFromSuperAdmin.Value;
           //claim  code end 

            if (SuperAdminUser != null)
            {
                signinManager.SignInAsync(SuperAdminUser, true, true);
            }
            Session["isFromSuprAdmin"] = null;
            Response.Redirect("~/SuperAdmin/SuperAdmin.aspx");

        }
    }
}
