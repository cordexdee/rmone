using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class AllocationGantt : System.Web.UI.UserControl
    {
        public string UserAll { get; set; }
        public string SelectedUser { get; set; }
        public string SelectedUsers { get; set; }
        public string IncludeClosed { get; set; }
        public string SelectedYear { get; set; }
        public string AddNewUrl { get; set; }
        private const string AddNewUrlView = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";

        public string MultiAddUrl { get; set; }
        private const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        protected void Page_Load(object sender, EventArgs e)
        {
            string newParam = "addworkitem";
            string formTitle = "Add Allocation";
            MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "addmultiallocation", "Add Multiple Allocations", UGITUtility.ObjectToString(""), SelectedUser, IncludeClosed));
            AddNewUrl = UGITUtility.GetAbsoluteURL(string.Format(AddNewUrlView, newParam, formTitle, "ResourceAllocation", SelectedUsers, IncludeClosed));

        }
    }
}