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
    public partial class EstimatedBillingHoursAnalytic : System.Web.UI.UserControl
    {
        ConfigurationVariableManager _configVariableManager = null;
        public string DivisionLabel = "Division";
        protected void Page_Load(object sender, EventArgs e)
        {
            _configVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            DivisionLabel = _configVariableManager.GetValue(ConfigConstants.DivisionLabel);
        }
    }
}