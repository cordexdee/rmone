using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ServiceTaskWorkFlow : System.Web.UI.UserControl
    {
        public string TicketId = string.Empty;
        public string ModuleName = string.Empty;
        public string editTaskUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}