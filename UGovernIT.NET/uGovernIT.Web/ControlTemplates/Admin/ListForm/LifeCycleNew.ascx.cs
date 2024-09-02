using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Core;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class LifeCycleNew : UserControl
    {
        List<LifeCycle> projectLifeCycles;
        LifeCycleManager lcHelper;

        protected override void OnInit(EventArgs e)
        {
            lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            projectLifeCycles = lcHelper.LoadProjectLifeCycles();
            txtOrder.Text = (projectLifeCycles.Count + 1).ToString();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {          
        }

        protected void btSaveLifeCycle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            
            int order = 0;
            int.TryParse(txtOrder.Text.Trim(), out order);
            
            LifeCycle newLifeCycle = new LifeCycle();
            newLifeCycle.ItemOrder = order;
            newLifeCycle.Name = txtTitle.Text;
            newLifeCycle.Description = txtDescription.Text;
            newLifeCycle.ModuleNameLookup = ModuleNames.PMM;
            LifeCycleManager lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            lcHelper.Insert(newLifeCycle);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added life cycle: {newLifeCycle.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (projectLifeCycles.Exists(x => x.Name.ToLower() == txtTitle.Text.Trim().ToLower()))
                args.IsValid = false;
        }      
    }
}
