
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Web
{
    public partial class EnvironmentEdit : UserControl
    {
        public long Id { get; set; }
        private UGITEnvironment _SPListItem;
        EnvironmentManager environMGR = new EnvironmentManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            _SPListItem = environMGR.LoadByID(Id);
            Fill();
            base.OnInit(e);
        }

        private void Fill()
        {
            txtTitle.Text = Convert.ToString(_SPListItem.Title);
            txtDescription.Text = Convert.ToString(_SPListItem.Description);
            chkDeleted.Checked = Convert.ToBoolean(_SPListItem.Deleted);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _SPListItem.Title = txtTitle.Text.Trim();
            _SPListItem.Description = txtDescription.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;

            environMGR.Update(_SPListItem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Environment: {_SPListItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
