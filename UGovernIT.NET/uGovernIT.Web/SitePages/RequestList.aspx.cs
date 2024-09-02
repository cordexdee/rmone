using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web.Modules
{
    
    public partial class RequestList : UPage
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            CustomFilteredTickets.ModuleName = Request["Module"];
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext().TenantAccountId}|{HttpContext.Current.CurrentUser().Name}: Visited Page: RequestList.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}