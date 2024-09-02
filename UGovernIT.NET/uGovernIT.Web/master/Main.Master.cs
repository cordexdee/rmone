using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web.Security;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.ControlTemplates.Shared;
using System.Linq;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        public PageConfiguration pageConfiguration = null;
        public string ScripText { get; set; }
        public string AssemblyVersion = string.Empty;
        protected string defaultAdminPageUrl = "/Admin/NewAdminUI.aspx" + "?deft=true&shwD=true";
        /// <summary>
        /// this property is used to hide menu bar from aspx page, without pageeditor
        /// </summary>
        public bool IsHideMenuBar { get; set; }
        protected void Page_Init(object sender, EventArgs e)
        {
             
            var context = HttpContext.Current.GetManagerContext();
            if (context == null)
                return;
            if (Request.QueryString["showTabFromEmail"] != null)
            {
                Session["tabDetailFromEmail"] = Request.QueryString["showTabFromEmail"];
            }
            ULog.Create(context.TenantAccountId, context.CurrentUser.Name);
            var configurationVariableHelper = new ConfigurationVariableManager(context);
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            string AccountName = configurationVariableHelper.GetValue("AccountName");
            #region tenatname
            //configurationVariableHelper.GetValue(ConfigConstants.showDefaultHelpPageButton);
            if (bool.TryParse(configurationVariableHelper.GetValue(ConfigConstants.showDefaultHelpPageButton), out bool showDefaultHelpPageButton) && showDefaultHelpPageButton)
            {
                btnDefaultAdminPage.Visible = true;
                if (!string.IsNullOrEmpty(configurationVariableHelper.GetValue(ConfigConstants.DefaultPageUrl)))
                {
                    defaultAdminPageUrl = configurationVariableHelper.GetValue(ConfigConstants.DefaultPageUrl);
                }
            }
            if (!string.IsNullOrEmpty(AccountName))
            {
                accountName.Visible = true;
                tenantLogo.Visible = true;
                accountName.Text = AccountName;
            }
            else
            {
                accountName.Visible = true;
                tenantLogo.Visible = true;
                accountName.Text = context.TenantAccountId;
            }
            if (!string.IsNullOrEmpty(AccountName) && bool.TryParse(configurationVariableHelper.GetValue(ConfigConstants.showDefaultHelpPageButton), out bool _showDefaultHelpPageButton) && _showDefaultHelpPageButton)
            {
                accountName.Text = AccountName + " | ";
            }

            #endregion

            #region pageTitle
            //pagetitle.Visible = true;
            if ((Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeSuperAdmin") || (Page.Request.AppRelativeCurrentExecutionFilePath == "~/SuperAdmin/SuperAdmin.aspx"))
                pagetitle.Text = "Super Admin";

            if (Page.AppRelativeVirtualPath == "~/Admin/NewAdminUI.aspx" && Page.Request.QueryString.Count == 0)
                pagetitle.Text = "Administrator";

            if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/TrialUser")
                pagetitle.Text = "Trial User";

            if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomePM" || Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomePMO")
                pagetitle.Text = "My Requests";
            if (Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeTasks" || Page.Request.AppRelativeCurrentExecutionFilePath == "~/Pages/HomeUser")
                pagetitle.Text = "My Tasks";


            if (Page.Request.QueryString.Count > 0)
            {
                if (Page.Request.QueryString.AllKeys.Contains("tkt"))
                {
                    if (Page.Request.QueryString["tkt"].ToString() == "tkt")
                    {
                        pagetitle.Text = "My Tasks";

                    }
                    if (Page.Request.QueryString["tkt"].ToString() == "morit")
                    {
                        pagetitle.Text = "More then Just IT";

                    }

                    if (Page.Request.QueryString["tkt"].ToString() == "newt")
                    {
                        pagetitle.Text = "Technologies Enabled Services";

                    }

                }


            }



            #endregion
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            if (Request["isudlg"] != null)
            {
                main_page_section.Style.Add(HtmlTextWriterStyle.Width, "100% important;");
                main_page_section.Attributes.Add("class", $"{main_page_section.Attributes["class"]} popup_wrap");
                titleSectionContainer.Visible = false;
            }

            Page.PreLoad += master_Page_PreLoad;
            pageConfiguration = this.Page.PageConfig();
            txPageTitle.InnerText = pagetitle.Text;

            if (pageConfiguration != null)
            {
                bool hideHeader = true;
                if (Request["IsDlg"] != null && UGITUtility.StringToBoolean(Request["IsDlg"]))
                    hideHeader = false;
                else
                    hideHeader = !pageConfiguration.HideHeader;
                headerPaneMain.Visible = HeaderPane.Visible = hideHeader;
                customTopMenu.Visible = !pageConfiguration.HideTopMenu;
                dvSideBarMain.Visible = dvSideBar.Visible = !pageConfiguration.HideLeftMenu;
                txPageTitle.InnerText = pageConfiguration.Title;

                if (pageConfiguration.ControlInfoList != null && pageConfiguration.ControlInfoList.Count > 0)
                {
                    for (int mi = 0; mi < pageConfiguration.ControlInfoList.Count; mi++)
                    {
                        var pagetitlestatus = pageConfiguration.ControlInfoList[mi];
                        if (pagetitlestatus.ShowTitle)
                        {
                            pagetitle.Text = pagetitlestatus.Title.ToString();
                            break;
                        }
                        if(pagetitlestatus.AssemblyName == "uGovernIT.Web.ControlTemplates.DockPanels.ModuleWebPartDockPanel")
                            FooterPane.Visible = false;
                    }
                }

                if (!pageConfiguration.HideLeftMenu)
                {
                    if (pageConfiguration.LeftMenuType == "Menu" || string.IsNullOrEmpty(pageConfiguration.LeftMenuType))
                    {
                        CustomMenuBar menubar = (CustomMenuBar)this.Page.LoadControl("~/Controltemplates/shared/CustomMenuBar.ascx");
                        LeftPane.Visible = true;
                        menubar.Visible = true;
                        LeftPane.Controls.Add(menubar);
                    }
                    else
                    {
                        //uGovernITDashboardChartsUserControl dashboardControl = (uGovernITDashboardChartsUserControl)this.Page.LoadControl("~/Controltemplates/ugovernit/dashboard/uGovernITDashboardChartsUserControl.ascx");
                        //dashboardControl.ViewName = pageConfiguration.LeftMenuType;
                        //LeftPane.Controls.Add(dashboardControl);
                        LeftPane.Visible = true;
                        //customTopMenuleft.Visible = false;
                    }
                }
                //FooterPane.Visible = true; // !pageConfiguration.HideFooter;
                if (pageConfiguration.HideSearch)
                    SettingsMenuBar.UgitHideGlobalSearch = true;
            }

            string clienttype = configurationVariableHelper.GetValue("ClientType");
            if (!string.IsNullOrEmpty(clienttype))
            {
                if (clienttype.EqualsIgnoreCase("ITSM") || clienttype.EqualsIgnoreCase("GC") || clienttype.EqualsIgnoreCase("ITPS"))
                    imageugovernIilogo2.ImageUrl = "/content/images/Service_Prime_Logo.svg";
                else
                    imageugovernIilogo2.ImageUrl = "/content/images/rmone/rm-one-logo.png";
            }
            string headerLogo = configurationVariableHelper.GetValue(ConfigConstants.HeaderLogo);
            if (!string.IsNullOrEmpty(headerLogo))
                imageugovernIilogo2.ImageUrl = headerLogo;
            else
                imageugovernIilogo2.ImageUrl = "/content/images/nologo.png";

            PageEx.SetDockContainer(ContentPlaceHolderContainer);
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (Request.Path.Contains("undefined"))
            {
                return;
            }
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Path.Contains("undefined"))
            {
                return;
            }
            //ASPxLabel2.Text = DateTime.Now.Year + Server.HtmlDecode(" &copy; Copyright by [company name]");
            MainPane.Visible = true;
            if (IsHideMenuBar)
            {
                LeftPane.Visible = false;
                customTopMenu.Visible = false;
                customBottomMenu.Visible = false;
            }
        }

        
        protected void MainPane_Init(object sender, EventArgs e)
        {
            MainPane.Height = Unit.Percentage(80);
            MainPane.Width = Unit.Percentage(80);
        }

        protected void MainPaneCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {

        }

        //protected void popupControlPanel_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string param = Convert.ToString(e.Parameter);
        //    string[] paramAll = null;
        //    if (!string.IsNullOrWhiteSpace(param))
        //        paramAll = param.Split('|');
        //    ASPxPopupControl popup = new ASPxPopupControl();
        //    popup.Text = "this is new popup control";
        //    string paramwithoutspace = Regex.Replace(paramAll[1].Split(':')[0], @"[~#%&*{}\<>?/+|"":-]", string.Empty, RegexOptions.IgnoreCase);
        //    string paramwithoutspace1 = Regex.Replace(paramwithoutspace, @"\s+", "");
        //    popup.ID = "popUp_" + paramwithoutspace1;
        //    popup.ClientInstanceName = "popUp_" + paramwithoutspace1;
        //    popup.ContentUrl = Convert.ToString(paramAll[0]);
        //    popup.AllowResize = true;
        //    popup.AllowDragging = true;
        //    popup.HeaderText = paramAll[1];
        //    popup.Width = Unit.Parse(Convert.ToString(paramAll[2]));
        //    popup.Height = Unit.Parse(Convert.ToString(paramAll[3]));
        //    //popup.EnableViewState = false;
        //    popup.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
        //    popup.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
        //    //popup.CloseAction = CloseAction.CloseButton;
        //    //popup.ClientSideEvents.CloseUp = "OnCloseUp";
        //    //popup.ScrollBars = ScrollBars.Horizontal;
        //    //popup.ShowPageScrollbarWhenModal = true;
        //    popupControlPanel.Controls.Add(popup);
        //    popupControlPanel.JSProperties["cppopupId"] = "popUp_" + Convert.ToString(paramwithoutspace1);
        //    hidCurrentPopupId.Text = "popUp_" + Convert.ToString(paramwithoutspace1);

        //}
    }
}