using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ManagerProjectAllocationView : System.Web.UI.UserControl
    {
        private ApplicationContext _context = null;
        public int UserOpportunitiesCount { get; set; }
        public int UserTrackedWorkCount { get; set; }
        public int UserOnGoingWorkCount { get; set; }
        public Unit Height { get; set; }
        protected int closeoutperiod = 0;
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        public Unit Width { get; set; }
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            closeoutperiod = uHelper.getCloseoutperiod(ApplicationContext);
            Dictionary<string, object> arrParams1 = new Dictionary<string, object>();
            arrParams1.Add("TenantId", ApplicationContext.CurrentUser.TenantID);
            arrParams1.Add("UserId", ApplicationContext.CurrentUser.Id);
            arrParams1.Add("IsManager", ApplicationContext.CurrentUser.IsManager);
            DataTable data = GetTableDataManager.GetData("MyProjectCount", arrParams1);
            this.UserOpportunitiesCount = int.Parse(data.Rows[0]["Opportunities"].ToString());
            this.UserTrackedWorkCount = int.Parse(data.Rows[0]["TrackedWork"].ToString());
            this.UserOnGoingWorkCount = int.Parse(data.Rows[0]["OngoingWork"].ToString());
        }
    }
}