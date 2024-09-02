using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using uGovernIT.Web.Models;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class Confirm : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);
            if (code != null && userId != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
                var result = manager.ConfirmEmail(userId, code);
                if (result.Succeeded)
                {
                    successPanel.Visible = true;
                    return;
                }
            }
            successPanel.Visible = false;
            errorPanel.Visible = true;
        }
    }
}