using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Web.Models;
using System.Security.Claims;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNet.Identity;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using DevExpress.Web;
using uGovernIT.Utility;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.WsFederation;
using System.Threading;
using System.Linq;
using uGovernIT.Util.Log;
using System.Configuration;
namespace uGovernIT.Web
{
    public partial class Login : System.Web.UI.Page
    {
        public string DefaultTenant { get; set; }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["DefaultTenant"] != null)
            {
                DefaultTenant = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            }
            if (!IsPostBack)
            {
                loginForm.DataSource = new LoginModel();
                loginForm.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = uGovernIT.Utility.Constants.PageTitle.Login;
            if (DefaultTenant.Equals("uGovernIT"))
            {
                ForgetPassword.Text = "Reset Password";
                loginContainer.HeaderText = "Welcome";
                loginContainer.PopupHorizontalAlign = PopupHorizontalAlign.LeftSides;
                logoImage.Visible = true;
                leftImage.Visible = true;
            }
            if (Utility.UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["ADFS_Enable"]) && (string.IsNullOrWhiteSpace(Request["login"]) || Request["login"].Trim().ToLower() != "system"))
            {
                var ctx = Request.GetOwinContext();
                var result = ctx.Authentication.AuthenticateAsync("ExternalCookie").Result;
                ctx.Authentication.SignOut("ExternalCookie");
                if (result != null && !Request.IsAuthenticated)
                {
                    var context = HttpContext.Current.GetManagerContext();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    var claims = result.Identity.Claims.ToList();

                    //Get the current claims principal
                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    // Get the claims values
                    var name = claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                       .Select(c => c.Value).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(name))
                        name = claims.Where(c => c.Type == ClaimTypes.Upn)
                                       .Select(c => c.Value).FirstOrDefault();

                    ULog.WriteLog("User Clainm");
                    foreach (Claim m in claims)
                    {
                        ULog.WriteLog(m.ToString());
                    }

                    var sid = claims.Where(c => c.Type == ClaimTypes.GivenName)
                                       .Select(c => c.Value).FirstOrDefault();
                    context.TenantID = System.Configuration.ConfigurationManager.AppSettings["ADFS_TenantID"];

                    var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
                    UserProfile user = manager.FindByName(name.Trim());
                    if (user != null)
                    {
                        Session["TenantID"] = user.TenantID;
                        signinManager.SignInAsync(user, false, false);

                        string landingPageUrl = new LandingPagesManager(context).GetLandingPageById(user.UserRoleId);
                        IdentityHelper.RedirectToReturnUrl(landingPageUrl, Response);
                    }
                }


                if (!Request.IsAuthenticated)
                {
                    //Request.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"] }, WsFederationAuthenticationDefaults.AuthenticationType);
                    return;
                }
                else
                {
                    var context = HttpContext.Current.GetManagerContext();
                    string landingPageUrl = new LandingPagesManager(context).GetLandingPageById(context.CurrentUser.UserRoleId);
                    IdentityHelper.RedirectToReturnUrl(landingPageUrl, Response);
                }
            }
            else
            {
                // Condition added, BTS-20-000044: Copy to clipboard link, Link copied in word, ppt, are not working when clicked on it
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Convert.ToString(Request.QueryString)))
                    Response.Redirect($"{uGovernIT.Utility.Constants.HomePagePath}?{Request.QueryString}");
            }
        }

        public static bool IsConnectedToDomain()
        {
            try
            {
                System.DirectoryServices.ActiveDirectory.Domain domain = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
                if (domain != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var login = new LoginModel();
            int multitenant = 0;
            if (ASPxEdit.ValidateEditorsInContainer(loginForm))
            {
                var context = HttpContext.Current.GetManagerContext();
                // Validate the user password
                login.UserName = loginForm.GetNestedControlValueByFieldName("UserName") as string;
                login.Password = loginForm.GetNestedControlValueByFieldName("Password") as string;
                login.AccountId = loginForm.GetNestedControlValueByFieldName("AccountId") as string;
                var moduleViewManager = new ModuleUserTypeManager(context);
                var tenant = moduleViewManager.GetTenant(login.AccountId);
                var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
                if (tenant == null)
                {
                    var lstusers = manager.GetUserNames(login.UserName.Trim());
                    foreach (UserProfile lstuserprofile in lstusers)
                    {
                        if (lstuserprofile.TenantID == context.TenantID)
                        {
                            tenant = moduleViewManager.GetTenantById(context.TenantID);
                            break;
                        }
                        
                    }
                    if (tenant == null && lstusers.Count> 1)
                    {
                        multitenant = -1;
                    }
                    else if(lstusers.Count == 1 && tenant == null)
                    {
                        tenant = moduleViewManager.GetTenantById(lstusers[0].TenantID);
                    }
                    else
                    {
                        tenant = moduleViewManager.GetTenantById(context.TenantID);
                    }

                }
                string landingPageUrl = string.Empty;
                SignInStatus result = SignInStatus.Failure;
                int loginStatus = 1;
                if (tenant != null && !tenant.Deleted)
                {
                    //var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                    Claim userClaim = new Claim("AuthProvider", "Forms");

                    if (userClaim.Value == "Windows")
                    {
                        //authenticate using Directory Services & DB
                        var contextType = IsConnectedToDomain() ? ContextType.Domain : ContextType.Machine;
                        var principalContext = new PrincipalContext(contextType);

                        bool isAuthenticated = principalContext.ValidateCredentials(login.UserName, login.Password, ContextOptions.Negotiate);
                        if (isAuthenticated)
                        {
                            UserProfile user = manager.FindByName(login.UserName.Trim());
                            if (user != null)
                            {
                                signinManager.SignInAsync(user, true, false);
                                result = SignInStatus.Success;
                            }
                        }
                    }
                    else if (userClaim.Value == "Forms")
                    {
                        loginStatus = 2;
                        //authenticate only in DB 
                        UserProfile user = manager.FindByName(login.UserName.Trim(), Convert.ToString(tenant.TenantID));
                        context.TenantID = Convert.ToString(tenant.TenantID);

                        if (user != null && Convert.ToString(tenant.TenantID).Equals(user.TenantID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (user.EnablePasswordExpiration == true && user.PasswordExpiryDate < DateTime.Now)
                            {
                                signinManager.SignIn(user, true, true);
                                result = SignInStatus.Failure;
                                loginStatus = 4;
                            }
                            else if (user.Enabled == false)
                            {
                                result = SignInStatus.Failure;
                                loginStatus = 5;
                            }
                            else
                            {
                                result = signinManager.PasswordSignIn(login.UserName.Trim(), login.Password, true, shouldLockout: false);

                                if (result == SignInStatus.Success)
                                {
                                    signinManager.SignIn(user, true, true);

                                    var isUGITSuperAdmin = manager.IsRole(RoleType.UGITSuperAdmin, user.UserName);
                                    if (isUGITSuperAdmin)
                                    {
                                        //landingPageUrl = "~/Admin/Admin.aspx";
                                        landingPageUrl = "~/SuperAdmin/SuperAdmin.aspx";
                                    }
                                    else
                                    {
                                        
                                        landingPageUrl = new LandingPagesManager(context).LandingPagesfinal(user);
                                    }

                                    Session["TenantID"] = user.TenantID; // Using this Session for ADO.NET related calls, will be removed after converting to Entity Framework. 
                                    loginStatus = 0;
                                }
                            }
                        }
                    }
                }
                else if(multitenant==-1)
                {
                    loginStatus = 6;
                }
                else
                    loginStatus = 3;

                switch (result)
                {
                    case SignInStatus.Success:
                        // Try to load cache if not available
                        RegisterCache.TryLoadCacheWhileLogin(context);

                        if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                            landingPageUrl = "~/" + Request.QueryString["ReturnUrl"];
                        else if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString)))
                            landingPageUrl = $"~/{uGovernIT.Utility.Constants.HomePagePath}?{Request.QueryString}";

                        IdentityHelper.RedirectToReturnUrl(landingPageUrl, Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("~/Account/Lockout.aspx");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn.aspx?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        false), true);
                        break;
                    case SignInStatus.Failure:
                        var message = "";
                        switch (loginStatus)
                        {
                            case 1:
                                message = "Invalid Account Id";
                                break;
                            case 2:
                                message = "Invalid Username or Password";
                                break;
                            case 3:
                                message = "Account Id is inactive";
                                break;
                            case 4:
                                message = "User's Password Expired";
                                Response.Redirect("/Account/ResetPasswordNew.aspx");
                                break;
                            case 5:
                                message = "User account is disabled";
                                break;
                            case 6:
                                message = "Username belongs to multiple tenants, please specify Account Id !!";
                                break;
                        }
                        lblMessage.Text = message;
                        break;
                    default:
                        //tbUserName.ErrorText = "Invalid user";
                        //tbUserName.IsValid = false;
                        break;
                }
            }
        }
    }
}