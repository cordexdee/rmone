using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class PriorityNew : UserControl
    {
        private ModulePrioirty item;
        private PrioirtyViewManager prioirtyMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
        private ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            item = new ModulePrioirty();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["moduleName"] != null)
                {
                    string moduleName = Request["moduleName"].ToString();
                    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }
                if (Session["ModuleName"] != null)
                {
                    string moduleName = Convert.ToString(Session["ModuleName"]);
                    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<ModulePrioirty> lstModulePrioirty = prioirtyMGR.Load();
            if (chkVIP.Checked)
            {
                List<ModulePrioirty> dr = lstModulePrioirty.Where(x => x.ModuleNameLookup == ObjModuleViewManager.GetByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName && x.IsVIP == true).ToList();  //.Select(string.Format("{0} ='{1}' or  {2}='True'", DatabaseObjects.Columns.ModuleNameLookup, ddlModule.SelectedItem.Value, DatabaseObjects.Columns.IsVIP));
            }
            long moduleid = Convert.ToInt64(ddlModule.GetValues());
            UGITModule moduleObj = ObjModuleViewManager.GetByID(moduleid);
            item.ModuleNameLookup = moduleObj.ModuleName;
            ModulePrioirty lstDuplicateRow = lstModulePrioirty.FirstOrDefault(x => x.ModuleNameLookup == moduleObj.ModuleName && x.uPriority == txtImpact.Text.Trim());
            if (lstDuplicateRow != null)
            {
                lblError.Text = "Cannot Insert Duplicate Impact For Priority " + moduleObj.ModuleName;
                lblError.Visible = true;
                return;
            }
            item.Title = txtImpact.Text.Trim();
            item.uPriority = txtImpact.Text.Trim();
            item.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
            item.IsVIP = chkVIP.Checked;
            item.EmailIDTo = txtNotifyTo.Text.Trim();
            item.Deleted = chkDeleted.Checked;
            item.NotifyInPlainText = chkAllowPlainText.Checked;
            item.Color = ColorEditHeaderFontColor.Text;
            prioirtyMGR.Insert(item);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Priority: {item.Title}; Module: {item.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
            Session["ModuleName"] = ObjModuleViewManager.GetByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
         
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
