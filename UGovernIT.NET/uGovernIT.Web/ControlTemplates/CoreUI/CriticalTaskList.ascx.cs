using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CriticalTaskList : System.Web.UI.UserControl
    {
        public string editTaskFormUrl { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit&");
        }
    }
}