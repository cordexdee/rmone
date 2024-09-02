using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class MyProjectCount : System.Web.UI.UserControl
    {
        public Unit Width { get; internal set; }
        public Unit Height { get; internal set; }
        public string viewTasksPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            viewTasksPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketTask");
        }
    }
}