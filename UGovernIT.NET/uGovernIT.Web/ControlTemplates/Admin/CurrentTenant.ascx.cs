using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Utility;
using ApplicationContext = uGovernIT.Manager.ApplicationContext;

namespace uGovernIT.Web
{
    public partial class CurrentTenant : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable currentTenants = CurrentTenantInfo();

            //currentTenants.DefaultView.Sort = "Created";
            currentTenants = currentTenants.DefaultView.ToTable();

            superAdminGrid.DataSource = currentTenants;
            superAdminGrid.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            superAdminGrid.SettingsFilterControl.ViewMode = FilterControlViewMode.VisualAndText;
            superAdminGrid.SettingsFilterControl.AllowHierarchicalColumns = false;
            superAdminGrid.SettingsFilterControl.ShowAllDataSourceColumns = true;
            superAdminGrid.SettingsFilterControl.HierarchicalColumnPopupHeight = 200;
            superAdminGrid.DataBind();
        }

        private DataTable CurrentTenantInfo()
        {     
            return GetTableDataManager.GetTenantDataUsingQueries($"GetUsageStatistics");
        }
    }
}

