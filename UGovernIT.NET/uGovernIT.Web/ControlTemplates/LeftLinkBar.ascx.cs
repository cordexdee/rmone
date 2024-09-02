using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class LeftLinkBar : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string viewTicketsPath;
        public string ViewID { get; set; }
        public string ShowDetails { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            viewTicketsPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?");
        }
    }
}