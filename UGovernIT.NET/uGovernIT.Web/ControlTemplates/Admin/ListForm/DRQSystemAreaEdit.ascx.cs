using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
namespace uGovernIT.Web
{
    public partial class DRQSystemAreaEdit : UserControl
    {
        public int Id { get; set; }
        ///private DataTable dt;
        // private SPListItem _SPListItem;
        DrqSyetemAreaViewManager drqSyetemAreaViewManager = new DrqSyetemAreaViewManager(HttpContext.Current.GetManagerContext());
        DRQSystemArea dRQSystemArea;
        protected override void OnInit(EventArgs e)
        {
            dRQSystemArea = new DRQSystemArea();
            dRQSystemArea = drqSyetemAreaViewManager.LoadByID(Convert.ToInt64(Id));
            Fill();
            base.OnInit(e);
        }

        private void Fill()
        {
            if (dRQSystemArea != null)
            {
                txtTitle.Text = dRQSystemArea.Title;
                chkDeleted.Checked = dRQSystemArea.Deleted;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            dRQSystemArea.Title = txtTitle.Text.Trim();
            dRQSystemArea.Deleted = chkDeleted.Checked;
            drqSyetemAreaViewManager.Update(dRQSystemArea);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
