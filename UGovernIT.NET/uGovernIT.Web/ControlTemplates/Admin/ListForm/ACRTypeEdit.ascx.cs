using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
 

namespace uGovernIT.Web
{
    public partial class ACRTypeEdit : UserControl
    {
        public int Id { get; set; }
        ACRType objAcrType;
        ACRTypeManager acrTypeManager = new ACRTypeManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {          
            objAcrType = acrTypeManager.LoadByID(Id);
            Fill();
            base.OnInit(e);
        }

        private void Fill()
        {
            txtTitle.Text = objAcrType.Title;
            chkDeleted.Checked =objAcrType.Deleted;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            objAcrType.Title= txtTitle.Text.Trim();
            objAcrType.Deleted=chkDeleted.Checked;          
            acrTypeManager.Update(objAcrType);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated ACR type: {objAcrType.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context,true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

    }
}
