using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class BusinessInitiative : UserControl
    {
        public string reportUrl = string.Empty;
        private ApplicationContext _context;

        protected void Page_Load(object sender, EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
            reportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");

            Guid newFrameId = Guid.NewGuid();
            string url = string.Format("{0}?reportName={1}&Module={2}&userId={3}&frameObjId={4}", reportUrl, "BusinessStrategy", "PMM", _context.CurrentUser.Id, newFrameId);

            LiteralControl lCtr = new LiteralControl(string.Format("<iframe src='{0}' onload='callAfterFrameLoad(this)' scrolling='yes' frameurl='{0}' width='97%' height='100%' frameborder='0' id='{1}' style='position:fixed;'></iframe>", url, newFrameId));
            pnlInitiatives.Controls.Add(lCtr);
        }
    }
}