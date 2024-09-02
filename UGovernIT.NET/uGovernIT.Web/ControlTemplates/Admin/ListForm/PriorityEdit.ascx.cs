
using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Collections.Generic;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class PriorityEdit : UserControl
    {
        public int ItemID { get; set; }
        private string listName = string.Empty;
        private string columnName = string.Empty;
        private ModulePrioirty item;
        private PrioirtyViewManager prioirtyMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
        /// <summary>
        /// Initialize control Event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            listName = DatabaseObjects.Tables.TicketPriority;
            columnName = DatabaseObjects.Columns.Priority;
            item = prioirtyMGR.LoadByID(Convert.ToInt64(ItemID));

            if (item != null)
            {
                ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), item.ModuleNameLookup)));  // spFieldLookupModule.LookupValue;
                txtImpact.Text = Convert.ToString(item.uPriority);
                txtItemOrder.Text = Convert.ToString(item.ItemOrder);
                chkDeleted.Checked = Convert.ToBoolean(item.Deleted);
                chkVIP.Checked = UGITUtility.StringToBoolean(item.IsVIP);
                chkAllowPlainText.Checked = item.NotifyInPlainText;
                txtNotifyTo.Text = Convert.ToString(item.EmailIDTo);
                item.NotifyTo = item.EmailIDTo;
                if(item.Color != null)
                {
                    ColorEditHeaderFontColor.Text = item.Color.ToString();
                }
                
                ColorEditHeaderFontColor.Items.Assign(uHelper.CreatePalette());
                //chkAllowPlainText.Checked = UGITUtility.StringToBoolean(item.NotifyTo);
            }
            base.OnInit(e);
        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            string moduleNameLookup = moduleViewManager.GetByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
            item.ModuleNameLookup = moduleNameLookup;
            List<ModulePrioirty> listModulePrioirty = prioirtyMGR.Load(x=> x.ModuleNameLookup==moduleNameLookup && x.uPriority== txtImpact.Text.Trim() && x.ID!= Convert.ToInt64(item.ID));
            if (listModulePrioirty!=null && listModulePrioirty.Count > 0)
            {
                lblError.Text = "Cannot Insert Duplicate Impact For Module " + moduleNameLookup;
                lblError.Visible = true;
                return;
            }
            item.Title = txtImpact.Text.Trim();
            item.uPriority = txtImpact.Text.Trim();
            item.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
            item.IsVIP = chkVIP.Checked;
            item.EmailIDTo = txtNotifyTo.Text.Trim();
            item.Deleted = chkDeleted.Checked;
            item.NotifyTo = Convert.ToString(chkAllowPlainText.Checked);
            item.NotifyInPlainText = chkAllowPlainText.Checked;
            item.Color = ColorEditHeaderFontColor.Text;

            prioirtyMGR.Update(item);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Priority: {item.Title}; Module: {item.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
