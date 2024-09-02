using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class ExecutiveSummary_Filter : UserControl
    {
        public string ModuleName { get; set; }
        //public string delegateControl;
        //public string reportUrl = string.Empty;
        public string CoreServiceReportURL = string.Empty;

        private ApplicationContext _context = null;

        public ApplicationContext ApplicationContext
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

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            { }
            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //delegateControl = ApplicationContext.ReportUrl + "BuildReport.aspx";
            //CoreServiceReportURL = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=coreservicereport");
            CoreServiceReportURL = ConfigurationManager.AppSettings["SiteUrl"] + "/layouts/uGovernIT/DelegateControl.aspx?control=coreservicereport";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}