
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class MessageBoardEdit : UserControl
    {
        public int Id { get; set; }
        // private SPListItem _SPListItem;
        MessageBoard objMessageBoard;
        MessageBoardManager objMessageBoardManager;

        protected override void OnInit(EventArgs e)
        {
            objMessageBoardManager = new MessageBoardManager(HttpContext.Current.GetManagerContext());
            objMessageBoard = objMessageBoardManager.LoadByID(Id);
            if (!IsPostBack)
                Fill();

            base.OnInit(e);
        }

        private void Fill()
        {
            txtBody.Text = UGITUtility.StripHTML(Convert.ToString(objMessageBoard.Body));
            dtcExpires.Date = Convert.ToDateTime(objMessageBoard.Expires);
            // lbStartDate.Text = dtcExpires..ToString("MMM-dd-yyyy");
            rbMessageTypeList.SelectedValue = Convert.ToString(objMessageBoard.MessageType);
            peAuthorizedTo.SetValues(objMessageBoard.AuthorizedToView);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            objMessageBoard.Title = string.Format("{1}: {0}", UGITUtility.TruncateWithEllipsis(txtBody.Text.Trim(), 50, "."), rbMessageTypeList.SelectedValue);
            objMessageBoard.Body = txtBody.Text.Trim();
            if (dtcExpires.Value == null)
                objMessageBoard.Expires = null;
            else
                objMessageBoard.Expires = dtcExpires.Date;
            objMessageBoard.MessageType = rbMessageTypeList.SelectedValue;
            objMessageBoard.AuthorizedToView = peAuthorizedTo.GetValues();
            objMessageBoardManager.Update(objMessageBoard);

            //CacheHelper<MessageBoardManager>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            List<MessageBoard> MessageBoardDataList = (List<MessageBoard>)CacheHelper<object>.Get($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID);
            MessageBoardDataList.RemoveAll(x => x.ID == objMessageBoard.ID);
            objMessageBoard.ModifiedBy = HttpContext.Current.GetManagerContext().CurrentUser.Name;
            MessageBoardDataList.Add(objMessageBoard);
            CacheHelper<object>.AddOrUpdate($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID, MessageBoardDataList);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated message board: {objMessageBoard.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (objMessageBoard != null)
            {
                objMessageBoardManager.Delete(objMessageBoard);
            }
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
