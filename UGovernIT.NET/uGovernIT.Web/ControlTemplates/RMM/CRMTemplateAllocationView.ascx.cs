using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class CRMTemplateAllocationView : System.Web.UI.UserControl
    {
        public string TemplateId { get; set; }
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}