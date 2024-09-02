using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class UserWelcomePanel : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string viewId { get; set; }
        public string criticalTaskListPath { get; set; }
        public string editTaskFormUrl { get; set; }
        public string userLinkUrl { get; set; }
        public string WelcomeMessage { get; set; }
        public string MoreLinkUrl { get; set; }
        ApplicationContext AppContext;
        protected void Page_Load(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1", AppContext.CurrentUser.Id));
            criticalTaskListPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=CriticalTaskList");
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit&");
            MoreLinkUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=moreurllink");

            divUserWelcomePanel.Style.Add("height", Height.ToString());
            divUserWelcomePanel.Style.Add("width", Width.ToString());
        }
    }
}