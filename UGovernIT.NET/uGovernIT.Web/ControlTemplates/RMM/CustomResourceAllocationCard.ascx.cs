using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CustomResourceAllocationCard : UserControl
    {
        public string FrameId { get; set; }
        public bool IncludeClosed { get; set; }
        public string hdnParentOf = string.Empty;
        public string hdnChildOf = string.Empty;
        public string absoluteUrlEdit = "";
        public string timesheetPendingAprvlPath = "";
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}", "CustomResourceAllocation"));
            timesheetPendingAprvlPath = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=timesheetpendingapprovals");
        }
    }
}