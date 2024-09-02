using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web
{
    public partial class ChangePasswordNew : System.Web.UI.Page
    {
        public string DefaultTenant { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["DefaultTenant"] != null)
            {
                DefaultTenant = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            }
            if (DefaultTenant.Equals("uGovernIT")) {
                logoImage.Visible = true;
                leftImage.Visible = true;
            }
        }
    }
}