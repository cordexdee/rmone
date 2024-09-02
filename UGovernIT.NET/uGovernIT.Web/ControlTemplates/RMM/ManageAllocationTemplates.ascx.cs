using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class ManageAllocationTemplates : System.Web.UI.UserControl
    {
        public bool IsResourceAdmin
        {
            get
            {
                return HttpContext.Current.GetManagerContext().UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            }
        }
        public bool enableSimiarityFunction; 
        ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            enableSimiarityFunction = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableSimilarityFunction);
        }
    }
}