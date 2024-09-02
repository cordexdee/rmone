using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;


namespace uGovernIT.Web
{
    public partial class TimesheetPendingApprovals : System.Web.UI.UserControl
    {
        public string ResourceId { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = null;
        public string viewTimesheetPath = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=customresourcetimeSheet");

        protected void Page_Load(object sender, EventArgs e)
        {
            resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(context);

            pendingStatusGrid.DataSource = GetTimesheetPendingApprovals();
            pendingStatusGrid.DataBind();            
        }

        private List<ResourceTimeSheetSignOff> GetTimesheetPendingApprovals()
        {
            List<ResourceTimeSheetSignOff> resourceTimeSheetSignOffs = resourceTimeSheetSignOffManager.Load(x => x.Deleted == false && x.Resource.EqualsIgnoreCase(ResourceId) && x.SignOffStatus == Constants.PendingApproval).ToList();
            return resourceTimeSheetSignOffs;
        }
    }
}