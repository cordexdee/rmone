using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class AssociatedGroups : System.Web.UI.UserControl
    {
      
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        private string userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            userID = Request["Id"];
            var groups = umanager.GetUserRoles(userID);
            associatedGroupsGridView.DataSource = groups;
            associatedGroupsGridView.DataBind();
        }
    }
}