using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class LeftTicketCountBar : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string viewTicketsPath;
        public string viewResourcesPath;
        public string ViewID { get; set; }
        public string ShowDetails { get; set; }
        public string AssemblyVersion = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            viewTicketsPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=homedashboardtickets");
            viewResourcesPath = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ugovernitconfiguration?control=resourcemanagement");
        }
    }
}