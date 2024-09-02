using System;
using System.Collections.Generic;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class ExecutiveSummary_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public string SelectedType { get; set; }
        public string SortType { get; set; }
        public string IsModuleSort { get; set; }
        public string StrDateFrom { get; set; }
        public string StrDateTo { get; set; }
        public string Module { get; set; }
        public string ReportFilterURl;
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");

        private ApplicationContext _context = null;

        public ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = System.Web.HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            paramRequired.Add("SelectedType");
            Module = Request["Module"];
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                //ReportFilterURl = ApplicationContext.ReportUrl + delegateControl + "?reportName=ExecutiveSummary&Filter=Filter&Module=" + Module;
                ReportFilterURl = delegateControl + "?reportName=ExecutiveSummary&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}