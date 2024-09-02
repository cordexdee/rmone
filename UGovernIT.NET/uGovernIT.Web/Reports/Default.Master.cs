using DevExpress.Security.Resources;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccessSettings.StaticResources.TrySetRules(UrlAccessRule.Allow(), DirectoryAccessRule.Allow());
        }
    }
}