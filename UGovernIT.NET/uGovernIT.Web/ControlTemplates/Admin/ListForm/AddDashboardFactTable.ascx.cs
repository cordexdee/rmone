using System;
using System.Web.UI;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;
namespace uGovernIT.Web
{
    public partial class AddDashboardFactTable : UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        DashboardFactTableManager dashboardFactTableManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            divCacheParam.Visible = chkCacheTable.Checked;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            DashboardFactTables dashboardFactTables = new DashboardFactTables();
            dashboardFactTables.Title = txtTitle.Text.Trim();
            dashboardFactTables.CacheAfter = UGITUtility.StringToInt(txtCacheAfter.Text.Trim());
            //dashboardFactTables.CacheThreshold = UGITUtility.StringToInt(txtCacheThresold.Text.ToString());
            dashboardFactTables.CacheTable = chkCacheTable.Checked;
            dashboardFactTables.ExpiryDate = dtcExpiryDate.Date;
            dashboardFactTables.Status = ddlStatus.SelectedValue.Trim();
            dashboardFactTables.CacheMode = ddlCacheMode.SelectedValue.Trim();
            dashboardFactTables.RefreshMode = ddlRefreshMode.SelectedValue.Trim();
            dashboardFactTables.Description = txtDescription.Text.Trim();
            dashboardFactTables.TenantID = applicationContext.TenantID;
            dashboardFactTableManager.Insert(dashboardFactTables);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void chkCacheTable_CheckedChanged(object sender, EventArgs e)
        {
            divCacheParam.Visible = chkCacheTable.Checked;
        }
    }
}
