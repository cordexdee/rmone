
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class MessageBoardNew : UserControl
    {
        MessageBoard objMessageBoard;
        MessageBoardManager objMessageBoardManager = new MessageBoardManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            objMessageBoard = new MessageBoard();
            base.OnInit(e);
            rbMessageTypeList.SelectedValue = Constants.MessageTypeValues.Ok;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            objMessageBoard.Title= string.Format("{1}: {0}", UGITUtility.TruncateWithEllipsis(txtBody.Text.Trim(), 50, "."), rbMessageTypeList.SelectedValue);
            objMessageBoard.Body = txtBody.Text.Trim();
            if (dtcExpires.Value == null)
                objMessageBoard.Expires = null;
            else
                objMessageBoard.Expires = dtcExpires.Date;
            objMessageBoard.MessageType = rbMessageTypeList.SelectedValue;
            objMessageBoard.AuthorizedToView = peAuthorizedTo.GetValues();
            objMessageBoardManager.Insert(objMessageBoard);
            List<MessageBoard> MessageBoardDataList = (List<MessageBoard>)CacheHelper<object>.Get($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID);
            MessageBoardDataList.RemoveAll(x => x.ID == objMessageBoard.ID);
            objMessageBoard.ModifiedBy = HttpContext.Current.GetManagerContext().CurrentUser.Name;
            MessageBoardDataList.Add(objMessageBoard);
            CacheHelper<object>.AddOrUpdate($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID, MessageBoardDataList);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added message board: {objMessageBoard.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cusCustom_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtBody.Text.Trim() == string.Empty)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
    }
}
