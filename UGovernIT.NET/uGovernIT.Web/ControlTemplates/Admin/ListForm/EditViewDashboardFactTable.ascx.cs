using System;
using System.Web.UI;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class EditViewDashboardFactTable : UserControl
    {
        public long Id { get; set; }
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        DashboardFactTableManager dashboardFactTableManager;
        DashboardFactTables dashboardFactTables = new DashboardFactTables();
        protected override void OnInit(EventArgs e)
        {
            Id = UGITUtility.StringToLong(Request["ID"]);
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            dashboardFactTables = dashboardFactTableManager.LoadByID(Id);
            Fill();

        }
        void Fill()
        {
            if (dashboardFactTables != null)
            {
                txtTitle.Text = dashboardFactTables.Title;
                txtCacheAfter.Text = Convert.ToString(dashboardFactTables.CacheAfter);
                //txtThreshold.Text = Convert.ToString(dashboardFactTables.CacheThreshold);
                lblLastUpdated.Text = UGITUtility.GetDateStringInFormat(dashboardFactTables.LastUpdated.ToString()); // UGITUtility.StringToDateTime(dashboardFactTables.LastUpdated);
                chkCacheTable.Checked = dashboardFactTables.CacheTable;
                dtcExpiryDate.Date =UGITUtility.StringToDateTime(dashboardFactTables.ExpiryDate);
                ddlStatus.SelectedValue = dashboardFactTables.Status;
                ddlCacheMode.SelectedValue = dashboardFactTables.CacheMode;
                ddlRefreshMode.SelectedValue = dashboardFactTables.RefreshMode;
                txtDescription.Text = dashboardFactTables.Description;

                divCacheParam.Visible = chkCacheTable.Checked;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            dashboardFactTables.Title = txtTitle.Text.Trim();
            dashboardFactTables.CacheAfter = UGITUtility.StringToInt(txtCacheAfter.Text.Trim());
            //Commenting threshold code again, as discussed with Prasad.
            //dashboardFactTables.CacheThreshold = UGITUtility.StringToInt(txtThreshold.Text.ToString());
            dashboardFactTables.CacheTable = chkCacheTable.Checked;
            dashboardFactTables.ExpiryDate = dtcExpiryDate.Date;
            dashboardFactTables.Status = ddlStatus.SelectedValue.Trim();
            dashboardFactTables.CacheMode = ddlCacheMode.SelectedValue.Trim();
            dashboardFactTables.RefreshMode = ddlRefreshMode.SelectedValue.Trim();
            dashboardFactTables.Description = txtDescription.Text.Trim();
            dashboardFactTableManager.Update(dashboardFactTables);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            dashboardFactTableManager = new DashboardFactTableManager(this.applicationContext);
            DashboardFactTables dashboardFactTables = dashboardFactTableManager.LoadByID(Convert.ToInt64(Id));
            if (dashboardFactTables != null)
            {
                dashboardFactTableManager.Delete(dashboardFactTables);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void chkCacheTable_CheckedChanged(object sender, EventArgs e)
        {
            divCacheParam.Visible = chkCacheTable.Checked;
        }
    }
}
