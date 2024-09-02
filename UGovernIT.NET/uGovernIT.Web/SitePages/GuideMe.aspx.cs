using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.SitePages
{
    public partial class GuideMe : System.Web.UI.Page
    {
        private ApplicationContext _applicationContext { get; set; }
        private ConfigurationVariableManager configurationVariable { get; set; }        
        protected ApplicationContext applicationContext
        {
            get {

                if (_applicationContext == null)
                {
                    _applicationContext = HttpContext.Current.GetManagerContext();
                }
                return _applicationContext;
            }
                

        }
        public string widgetCategoy;
        protected void Page_Load(object sender, EventArgs e)
        {

            configurationVariable = new ConfigurationVariableManager(applicationContext);
            widgetCategoy = configurationVariable.GetValue(ConfigConstants.widgetsHelpCardCategory);
            widgetCategoy = string.IsNullOrEmpty(widgetCategoy) ? "" : widgetCategoy.ToLower();
        }


    }
}