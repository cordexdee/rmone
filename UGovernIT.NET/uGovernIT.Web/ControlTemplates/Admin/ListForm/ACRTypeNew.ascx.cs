using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ACRTypeNew : UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        ACRTypeManager aCRTypeManager;
        protected override void OnInit(EventArgs e)
        {
           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            aCRTypeManager = new ACRTypeManager(this.applicationContext);
            ACRType aCRType = new ACRType();
            aCRType.Title = txtTitle.Text.Trim();
            aCRType.Deleted = chkDeleted.Checked;
            aCRTypeManager.Insert(aCRType);
            Util.Log.ULog.WriteUGITLog(applicationContext.CurrentUser.Id, $"Added ACR type: {txtTitle.Text.Trim()}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), applicationContext.TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}
