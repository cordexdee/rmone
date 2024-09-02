
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class EnvironmentNew : UserControl
    {

        private UGITEnvironment _SPListItem;
        EnvironmentManager environMGR = new EnvironmentManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            _SPListItem = new UGITEnvironment();  // SPListHelper.GetSPList(DatabaseObjects.Lists.Environment).AddItem();
            base.OnInit(e);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            _SPListItem.Title = txtTitle.Text.Trim();
            _SPListItem.Description = txtDescription.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;
            

            if (_SPListItem.ID > 0)
            {
                environMGR.Update(_SPListItem);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Environment: {_SPListItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            }
            else
            {
                environMGR.Insert(_SPListItem);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Environment: {_SPListItem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            }

          

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
