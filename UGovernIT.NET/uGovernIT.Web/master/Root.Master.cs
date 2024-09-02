using System;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Core;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.master
{
    public partial class Root : System.Web.UI.MasterPage
    {
        public static string styleSet;
        public static string themeValue;

        private ConfigurationVariableManager _configurationVariableManager;
        private ApplicationContext _applicationContext = HttpContext.Current.GetManagerContext();
        private StringBuilder _sbDvExFntOverride = new StringBuilder();

        public Root()
        {
            _configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
            themeValue = UGITTheme.GetThemeName();
            UGITTheme ugitTheme = new UGITTheme();

            try
            {
                if (!(string.IsNullOrEmpty(themeValue)))
                {
                    DevExpress.Web.ASPxWebControl.GlobalTheme = themeValue;
                }
                else
                {
                    if (ugitTheme == null)
                    {
                        ugitTheme = UGITTheme.GetDefaultTheme();
                    }
                    else if ((!String.IsNullOrEmpty(_configurationVariableManager.GetValue(ConfigConstants.UgitTheme))))
                    {
                        string uGitTheme = _configurationVariableManager.GetValue(ConfigConstants.UgitTheme);
                        var xDoc = new XmlDocument();

                        xDoc.LoadXml(uGitTheme);

                        UGITTheme objUgitTheme = null;
                        objUgitTheme = (UGITTheme)uHelper.DeSerializeAnObject(xDoc, ugitTheme);

                        if (objUgitTheme != null)
                        {
                            ugitTheme = objUgitTheme;
                        }
                    }
                    else
                    {
                        ugitTheme = UGITTheme.GetDefaultTheme();
                    }

                    if (ugitTheme != null && !string.IsNullOrEmpty(ugitTheme.DevExTheme))
                    {
                        string fontName = ugitTheme.FontName.ToLower();
                        string fontSize = "11px";

                        //strBldrDvExFntOverride.Append(string.Format(".dxgvControl{{font-family:{0}!important;font-size:{1}}}", fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxtcLite_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxgvControl_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxgvDisabled_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxmLite_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxeEditArea_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxpnlControl_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxeButtonEdit_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxtvControl_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxeListBox_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxtlControl_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxpLite_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxeBase_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxpcLite_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxscControl_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".dxgvTable_{0} {{font-family:{1}!important;font-size:{2}}}", ugitTheme.DevExTheme, fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format("select {{font-family:{0}!important;font-size:{1}}}", fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format("input {{font-family:{0}!important;font-size:{1}}}", fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format("body{{font-size:{0} !important;font-family:{1} !important}}", fontSize, fontName));
                        _sbDvExFntOverride.Append(string.Format(".menutopbar{{border: none !important;background-color: transparent !important;padding: 0px !important;width: 100%;float: left;font-family:{0};font-size:{1}}}", fontName, fontSize));
                        _sbDvExFntOverride.Append(string.Format(".divSubMenu{{ display: inline-table;font-family:{0}!important;font-size:{1}}}", fontName, fontSize));

                    }
                    else
                    {
                        _sbDvExFntOverride.Append(string.Format(".menutopbar{{border: none !important;background-color: transparent !important;padding: 0px !important;width: 100%;float: left;}}"));
                        _sbDvExFntOverride.Append(string.Format(".divSubMenu{{ display: inline-table;}}"));
                    }

                    UGITTheme.SetFontName(_sbDvExFntOverride.ToString());
                    bool isThemeExist = DevExpress.Web.ASPxThemes.ThemesProviderEx.GetThemes().Exists(x => x == ugitTheme.DevExTheme);
                    if (!isThemeExist)
                    {
                        ugitTheme = UGITTheme.GetDefaultTheme();
                    }

                    if (ugitTheme != null)
                    {
                        DevExpress.Web.ASPxWebControl.GlobalTheme = ugitTheme.DevExTheme;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);

                ugitTheme = UGITTheme.GetDefaultTheme();
                if (ugitTheme != null)
                {
                    DevExpress.Web.ASPxWebControl.GlobalTheme = ugitTheme.DevExTheme;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
        
            if (LoadRootMaster())
                return;

            var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            if (Page.User.Identity.IsAuthenticated)
            {
                UserProfile user = HttpContext.Current.GetUserManager().GetUserByUserName(Page.User.Identity.Name.ToString());
                if (user != null && user.PasswordExpiryDate <= DateTime.Now)
                {
                    // Redirect to password change  
                    //string href = string.Format("javascript:window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=changepassword','',Change Password','50','50',0,'default.aspx')");
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "myJsFn", href, true);
                    //HttpContext.Current.Response.Write("<script type='text/javascript'> window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=changepassword','',Change Password','50','50',0,'default.aspx');</script>");
                }
            }
            else if ((!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest()) && !Page.User.Identity.IsAuthenticated)
            {
                if (_applicationContext.CurrentUser == null)
                {
                    _applicationContext = ApplicationContext.Create();
                }
                Session["IsRegistrationSubmit"] = "true";

                UserProfile user = manager.FindByName(_applicationContext.CurrentUser.UserName);
                signinManager.SignIn(user, true, true);
            }
            else
            {
                // Condition added, BTS-20-000044: Copy to clipboard link, Link copied in word, ppt, are not working when clicked on it
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString)))
                    Response.Redirect("/Account/Login.aspx?"+ Request.QueryString);
                else
                    Response.Redirect("/Account/Login.aspx");
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Path.Contains("undefined"))
            {
                return;
            }
            if (LoadRootMaster())
                return;

            styleSet = UGITTheme.GetFontName();
            if (!string.IsNullOrEmpty(styleSet))
            {
                //devExFontOverride.InnerText = styleSet.ToString();////comment to remove verdana font
            }

            var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            if ((!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest()) && !Page.User.Identity.IsAuthenticated)
            {
                if (_applicationContext.CurrentUser == null)
                {
                    _applicationContext = ApplicationContext.Create();
                }

                UserProfile user = manager.FindByName(_applicationContext.CurrentUser.UserName);
                signinManager.SignIn(user, true, true);
            }
            else if (!PermissionHelper.IsOnboardingUIRequest() && Session["IsRegistrationSubmit"] != null && Session["IsRegistrationSubmit"].ToString() == "true")
            {
                //Session["IsRegistrationSubmit"] = false;
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

                Response.Redirect("/account/login.aspx");
            }
            else if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/account/login.aspx");
            }
            else
            {
                UserProfile user = HttpContext.Current.CurrentUser();

                if (user != null && user.EnablePasswordExpiration && user.PasswordExpiryDate < System.DateTime.Now)
                {
                    divUserExpiration.Visible = true;
                }
            }

            string faviconIconPath = WebConfigurationManager.AppSettings["FaviconIcon"];
            if (!string.IsNullOrEmpty(faviconIconPath))
            {
                HtmlLink favicon = new HtmlLink();
                favicon.Attributes["rel"] = "shortcut icon";
                favicon.Attributes["type"] = "image/x-icon";
                favicon.Href = ResolveUrl(faviconIconPath);

                Page.Header.Controls.Add(favicon);
            }
        }

        private bool LoadRootMaster()
        {            
            if (!string.IsNullOrEmpty(Request.QueryString["isudlg"]))
                return true;
            else
                return false;
        }
    }
}