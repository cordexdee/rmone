using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class BusinessUnitDistribution_Filter : UserControl
    {
        public string ModuleName { get; set; }
        public string ReportTitle { get; set; }
        public string BusinessunitdistributionReportURL = string.Empty;

        private ApplicationContext _context = null;
        FieldConfigurationManager fmanger = null;

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

        public FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (fmanger == null)
                {
                    fmanger = new FieldConfigurationManager(ApplicationContext);
                }
                return fmanger;
            }
        }

        public BusinessUnitDistribution_Filter()
        {
            ModuleName = "CPR";
            BusinessunitdistributionReportURL = ApplicationContext.SiteUrl + "BuildReport.aspx";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
                ReportTitle = Request["title"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //delegateControl = ApplicationContext.ReportUrl + "BuildReport.aspx";
            //BusinessunitdistributionReportURL = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=businessunitdistributionreport");        
            BindDivisions();
        }

        private void BindDivisions()
        {
            string divisions = FieldConfigurationManager.Load(x => x.FieldName == DatabaseObjects.Columns.CRMBusinessUnit).FirstOrDefault().Data;

            if (!string.IsNullOrEmpty(divisions))
            {
                List<string> lstDivisions = divisions.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
                tbDivision.DataSource = lstDivisions;
                tbDivision.DataBind();

                tbDivision.Items.Insert(0, new DevExpress.Web.ListEditItem { Text = "None", Value = "None" });
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}