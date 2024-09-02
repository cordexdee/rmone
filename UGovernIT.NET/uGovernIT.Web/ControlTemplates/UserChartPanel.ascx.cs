using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class UserChartPanel : System.Web.UI.UserControl
    {
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        public string spTheme;
        protected void Page_Load(object sender, EventArgs e)
        {
            spTheme = string.Empty;
            // viewTicketsPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=homedashboardtickets&Module=CPR");
        }
    }
}