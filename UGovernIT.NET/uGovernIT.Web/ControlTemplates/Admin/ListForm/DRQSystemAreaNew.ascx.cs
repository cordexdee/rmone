using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
namespace uGovernIT.Web
{
    public partial class DRQSystemAreaNew : UserControl
    {
        private string TenantID = string.Empty;
        DrqSyetemAreaViewManager drqSyetemAreaViewManager = new DrqSyetemAreaViewManager(HttpContext.Current.GetManagerContext());
        DRQSystemArea dRQSystemArea;
        protected override void OnInit(EventArgs e)
        {
            TenantID = Convert.ToString(Session["TenantID"]);
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dRQSystemArea = new DRQSystemArea();
            Dictionary<String, object> values = new Dictionary<string, object>();
            dRQSystemArea.Title = txtTitle.Text.Trim();
            dRQSystemArea.Deleted = chkDeleted.Checked;
            dRQSystemArea.TenantID = TenantID;
            drqSyetemAreaViewManager.Insert(dRQSystemArea);
            uHelper.ClosePopUpAndEndResponse(Context,true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
