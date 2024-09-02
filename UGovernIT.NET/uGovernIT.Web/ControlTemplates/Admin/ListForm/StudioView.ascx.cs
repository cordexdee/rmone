using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class StudioView : System.Web.UI.UserControl
    {
        public bool EnableStudioDivisionHierarchy = true;
        protected override void OnInit(EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(context);
            EnableStudioDivisionHierarchy = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
            {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.Studio + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Studio, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);

            TicketManager ticketManager = new TicketManager(context);
            ticketManager.ApplyChangesToTicketCache(DatabaseObjects.Columns.StudioLookup);            
        }
    }
}