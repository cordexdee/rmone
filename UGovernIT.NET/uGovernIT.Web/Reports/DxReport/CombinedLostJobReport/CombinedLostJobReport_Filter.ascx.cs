using System;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class CombinedLostJobReport_Filter : UserControl
    {
       

        public string ModuleName { get; set; }
        public string  ReportTitle { get; set; }
        public string delegateControl;


        private ApplicationContext _context = null;
       

        // protected string CombinedLostJobReportURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=combinedlostjobreport");
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

        public CombinedLostJobReport_Filter()
        {
            ModuleName = "CPR";
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            //  ModuleName = Request["Module"];


        }

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
                ReportTitle = Request["title"];

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}