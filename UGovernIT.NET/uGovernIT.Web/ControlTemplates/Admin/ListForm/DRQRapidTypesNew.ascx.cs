using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{
    public partial class DRQRapidTypesNew : UserControl
    {
        private string TenantID = string.Empty;
        DrqRapidTypesManager drqRapidTypesManager = new DrqRapidTypesManager(HttpContext.Current.GetManagerContext());
        DRQRapidType dRQRapidType;
        protected override void OnInit(EventArgs e)
        {
            TenantID = Convert.ToString(Session["TenantID"]);
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            dRQRapidType = new DRQRapidType();
            dRQRapidType.Title= txtTitle.Text.Trim();
            dRQRapidType.TenantID=TenantID;
            dRQRapidType.Deleted = chkDeleted.Checked;
            drqRapidTypesManager.Insert(dRQRapidType);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
