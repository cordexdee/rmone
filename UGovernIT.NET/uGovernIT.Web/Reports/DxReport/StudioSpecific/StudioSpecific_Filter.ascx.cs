using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class StudioSpecific_Filter : UserControl
    {
        public string ModuleName { get; set; }
        public string ReportTitle { get; set; }
        public string StudioSpecificReportURL = string.Empty;

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

        public StudioSpecific_Filter()
        {
            ModuleName = "CPR";
            StudioSpecificReportURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = Session["context"] as ApplicationContext;
            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
                ReportTitle = Request["title"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //delegateControl = ApplicationContext.ReportUrl + "BuildReport.aspx";
            //StudioSpecificReportURL = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=StudioSpecificreport");    
            //BindStudios();
        }

        private void BindStudios()
        {
            //string studios = FieldConfigurationManager.Load(x => x.FieldName == DatabaseObjects.Columns.StudioLookup).FirstOrDefault().Data;

            //if (!string.IsNullOrEmpty(studios))
            //{
            //    List<string> lstStudios = studios.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
            //    tbStudio.DataSource = lstStudios;
            //    tbStudio.DataBind();

            //    tbStudio.Items.Insert(0, new DevExpress.Web.ListEditItem { Text = "None", Value = "None" });
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}