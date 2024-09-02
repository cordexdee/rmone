using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using uGovernIT.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web {
    public partial class Register : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            
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
        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
            var authmanager = Context.GetOwinContext().Get<AuthenticationServiceManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new UserProfile() { UserName = tbUserName.Text, Email = tbEmail.Text, isRole=false};
            IdentityResult result = manager.Create(user, tbPassword.Text);
            
            if (result.Succeeded)
            {
                if (ddlUserType.SelectedIndex == 0)
                {
                    ContextType contextType = IsConnectedToDomain() ? ContextType.Domain : ContextType.Machine;
                    PrincipalContext principalContext = new PrincipalContext(contextType);
                    UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, user.Email);
                    if (up == null)
                    {
                        UserPrincipal userPrincipal = new UserPrincipal(principalContext, user.Email, tbPassword.Text.ToString(), true);
                        userPrincipal.Name = user.Email;
                        userPrincipal.DisplayName = user.Email;
                        if (contextType != ContextType.Machine)
                        {
                            userPrincipal.UserPrincipalName = user.Email;
                            if (!string.IsNullOrEmpty(user.Email))
                                userPrincipal.EmailAddress = user.Email;
                        }
                        userPrincipal.PasswordNeverExpires = true;
                        userPrincipal.Enabled = true;
                        userPrincipal.Save();
                        manager.AddClaim(user.Id, new Claim("AuthProvider", "Windows"));
                    }
                }
                else
                {
                    manager.AddClaim(user.Id, new Claim("AuthProvider", "Forms"));
                    Session["UserProfile"] = user;
                }
                
                signInManager.SignInAsync(user, false, false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
        }
    }
}