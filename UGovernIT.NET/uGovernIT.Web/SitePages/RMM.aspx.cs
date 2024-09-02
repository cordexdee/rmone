using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web.SitePages
{
    public partial class RMM :UPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Visited Page: RMM.aspx");
        }
    }
}