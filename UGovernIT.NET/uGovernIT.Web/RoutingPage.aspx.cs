using System;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using System.Web;

namespace uGovernIT.Web
{
    public partial class RoutingPage : UPage
    {
        protected  void Page_Init( object sender, EventArgs e)
        {
             
            //if(!Page.IsPostBack)
            //    Util.Log.ULog.WriteLog(HttpContext.Current.CurrentUser()?.Name + ": Visited: " + "Page RoutingPage.aspx");

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}