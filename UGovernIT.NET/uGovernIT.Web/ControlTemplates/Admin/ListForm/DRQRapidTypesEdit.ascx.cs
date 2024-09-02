using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{
    public partial class DRQRapidTypesEdit : UserControl
    {
        public int Id { private get; set; }
        public DataTable dt;
        //private SPListItem _SPListItem;
        DrqRapidTypesManager drqRapidTypesManager = new DrqRapidTypesManager(HttpContext.Current.GetManagerContext());
        DRQRapidType dRQRapidType;
        protected override void OnInit(EventArgs e)
        {
            dRQRapidType = drqRapidTypesManager.LoadByID(Convert.ToInt64(Id));
            Fill();
            base.OnInit(e);
        }

        private void Fill()
        {

            if (dRQRapidType != null)
            {
                txtTitle.Text = dRQRapidType.Title;
                chkDeleted.Checked = dRQRapidType.Deleted;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dRQRapidType.Title = txtTitle.Text.Trim();
            dRQRapidType.Deleted = chkDeleted.Checked;
            drqRapidTypesManager.Update(dRQRapidType);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
