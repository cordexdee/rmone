using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class CRMLeadEstimatorView : System.Web.UI.UserControl
    {
        public string OpenUserResumeUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&refreshpage=0";
        public string DivisionLabel { get; set; }
        public string StudioLabel { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurationVariableManager ConfigVariableMGR = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            DivisionLabel = ConfigVariableMGR.GetValue(ConfigConstants.DivisionLabel);
            StudioLabel = ConfigVariableMGR.GetValue(ConfigConstants.StudioLabel);
            OpenUserResumeUrl = UGITUtility.GetAbsoluteURL(string.Format(OpenUserResumeUrl, "openuserresume", "User Resume", "ResourceAllocation"));
        }
    }
}