using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class UserResume : System.Web.UI.UserControl
    {
        public string ProjectID { get; set; }
        public string UserID { get; set; }
        public string ModuleName
        {
            get
            {
                if (ViewState["modulename"] == null)
                    return string.Empty;
                else
                    return Convert.ToString(ViewState["modulename"]);
            }
            set { ViewState["modulename"] = value; }
        }

        public string ResumeManualEscalationUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ManualEscalation");

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}