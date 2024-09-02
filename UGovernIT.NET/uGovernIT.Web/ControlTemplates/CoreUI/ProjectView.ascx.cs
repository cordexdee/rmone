using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class ProjectView : System.Web.UI.UserControl
    {
        public Unit Width { get; internal set; }
        public Unit Height { get; internal set; }
        public string HeadType { get; internal set; }
        public string DivisionLabel { get; set; }
        public string StudioLabel { get; set; }
        ConfigurationVariableManager _configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            DivisionLabel = _configManager.GetValue(ConfigConstants.DivisionLabel);
            StudioLabel = _configManager.GetValue(ConfigConstants.StudioLabel);
        }
    }
}