
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Web.Security;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Linq;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL;
using uGovernIT.Web.Helpers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    /// <summary>
    /// This is for ChangePassword for Normal Users
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        UserProfile user = null;
        UserProfileManager umanager;
        string resetUserPwd = string.Empty;
        string UserId = string.Empty;
        string TenantId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string data = Request["accid"];
            umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    data = QueryString.Decode(data);
                    UserId = data.Split(new string[] { uGovernIT.Utility.Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
                    TenantId = data.Split(new string[] { uGovernIT.Utility.Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[1];

                    HttpContext.Current.GetManagerContext().TenantID = TenantId;
                    user = umanager.GetUserProfile(UserId);
                    HttpContext.Current.GetManagerContext().SetCurrentUser(user);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
                if (user == null)
                {
                    lblError.Text = "<div style='color:#fff; padding:5px; background:red;font-weight:bold; margin-top:5px;' >Reset password link is invalid.</div>";
                    btnChangePassword.Visible = false;
                }
                oldPasswordTR.Visible = false;
                btnCancel.Text = "Log In";
                btnCancel.ToolTip = "Log In";
            }
            else
            {            
                UserId = Request["userCode"];
                if (UserId != null)
                {
                    user = umanager.GetUserById(UserId);
                    oldPasswordTR.Visible = false;
                    btnChangePassword.Text = "Reset Password";
                }
                else
                {
                    user = HttpContext.Current.CurrentUser();
                }
            }
            resetUserPwd = Request["resetUserPwd"];

            if (user == null)
                return;
            UserName.Text = user.UserName;
            ConfigurationVariableManager con = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            policy.Text = con.GetValue(ConfigConstants.PasswordPolicy);
            if (!IsPostBack && string.IsNullOrEmpty(data) && !umanager.CheckPassword(user))
            {
                oldPasswordTR.Visible = false;
               // btnSave.Visible = false;
                //PageHeader.InnerText = "Set password";
                //PageDescription.InnerText = "You do not have a local password for this site. Add a local password so you can log in without an external login.";
                //tbCurrentPassword.Visible = false;
                //btnChangePassword.Visible = false;
                //btnSetPassword.Visible = true;
                UserName.Visible = false;
            }
            
        }

        private void ChangeUserPassword()
        {
            if (txtOldPassword.Text.Trim() == string.Empty || txtNewPassword.Text.Trim() == string.Empty || txtConfirmPassword.Text.Trim() == string.Empty)
            {
                lblError.Text = "<div style='color:#fff; padding:5px; background:red;font-weight:bold; margin-top:5px;' >Please enter all fields.</div>";
            }
            else if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                lblError.Text = "<div style='color:#fff; padding:5px; background:red;font-weight:bold; margin-top:5px;'>The passwords that you entered did not match.</div>";
            }
            else if (txtNewPassword.Text.Length < 7)
            {
                lblError.Text = "<div style='color:#fff; padding:5px; background:red;font-weight:bold; margin-top:5px;' >The password does not meet the password policy requirements - check the minimum password length, password complexity and password history requirements.</div>";

            }

        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
           //  ChangeUserPassword();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            IdentityResult result;
                        
            // check call is from Reset Password Page
            if (resetUserPwd != null && Convert.ToBoolean(Convert.ToInt32(resetUserPwd)))
            {
                string passwordToken = umanager.GeneratePasswordResetToken(user.Id.Trim());
                result = umanager.ResetPassword(user.Id.Trim(), passwordToken, txtNewPassword.Text.Trim());
            }
            else
            {
                result = umanager.ChangePassword(user.Id.Trim(), txtOldPassword.Text.Trim(), txtNewPassword.Text.Trim());
            }

            if (result.Succeeded)
            {
                var Newuser = umanager.FindById(user.Id);
                if (!Newuser.Enabled)
                    Newuser.Enabled = true;
                UpdatePasswordExpirationDate(Newuser);

                // If call is from Change Password, re Log in with new credentials.
                if (resetUserPwd == null && !Convert.ToBoolean(Convert.ToInt32(resetUserPwd)))
                {
                    signInManager.SignIn(Newuser, isPersistent: false, rememberBrowser: false);
                }
                
                // Check if Change/Reset Password related call is coming form login screen or from RMM module/Home page
                string UrlReferrer = Request.UrlReferrer.AbsolutePath;
                if (UrlReferrer != null && UrlReferrer.Contains("DelegateControl.aspx"))
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                else if (UrlReferrer != null && UrlReferrer.Contains("ResetPasswordNew.aspx"))
                    Response.Redirect("~/");
                else if (UrlReferrer != null && UrlReferrer.Contains("ChangePasswordNew"))
                {
                    trTitle.Visible = false;
                    trPassword.Visible = false;
                    btnChangePassword.Visible = false;
                    usernameTR.Visible = false;
                    policy.Visible = false;
                    ErrorMessage.Text = "<div class='password-change-success'>Password Change Successful.</div>";
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                    HttpContext.Current.GetManagerContext().SetCurrentUser(Newuser);
                    //signInManager.SignInAsync(Newuser, true, true);
                }
                else
                    uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else
            {
                ErrorMessage.Text ="<div style='color:#fff; padding:5px; background:red; font-weight:bold; margin-top:5px;'>"+ result.Errors.FirstOrDefault()+"</div>";
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Check if Change/Reset Password related call is coming form login screen or from RMM module/Home page
            string UrlReferrer = Request.UrlReferrer.AbsolutePath;
            if (UrlReferrer != null && UrlReferrer.Contains("DelegateControl.aspx"))
                uHelper.ClosePopUpAndEndResponse(Context, false);
            else if (UrlReferrer != null && (UrlReferrer.Contains("ResetPasswordNew.aspx") || UrlReferrer.Contains("ChangePasswordNew")))
            {
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Redirect("/Account/Login.aspx");
            }
            else
                uHelper.ClosePopUpAndEndResponse(Context, false);            
        }
        public void UpdatePasswordExpirationDate(UserProfile user)
        {
            int passwordExpirationPeriod = 0;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            int.TryParse(configurationVariableManager.GetValue(ConfigConstants.PasswordExpirationPeriod), out passwordExpirationPeriod);
            if (user != null)
            {                              
                    if (passwordExpirationPeriod > 0)
                        user.PasswordExpiryDate = DateTime.Now.AddDays(passwordExpirationPeriod);
                    else
                        user.PasswordExpiryDate = DateTime.Now.AddDays(Utility.Constants.DefaultPasswordExpirationPeriod);
                HttpContext.Current.GetUserManager().Update(user);
            }
        }
    }
}
