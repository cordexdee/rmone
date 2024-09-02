using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class UserEntryControl : System.Web.UI.UserControl
    {
        private string newParam = "addworkitem";
        protected string formTitle = "Add Allocation";
        private const string absoluteUrlView = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        protected string filterPageNew;
        protected string url;
        protected string createProjectlink;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Add allocation link generation.
            string selectedVals = HttpContext.Current.CurrentUser().Id;
            url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "ResourceAllocation", selectedVals, false));

            // Create Project link Generation.
            Ticket moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), "CPR", HttpContext.Current.CurrentUser());
            string createProjectUrl = UGITUtility.GetAbsoluteURL(moduleRequest.Module?.StaticModulePagePath);
            string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            createProjectlink = string.Format("openTicketDialog('{0}', 'TicketId=0', '{1}', '{3}', '{4}', 0,'{2}');", createProjectUrl, "New Construction Project", "/Pages/CRMProject", 90, 90);
        }
    }
}
