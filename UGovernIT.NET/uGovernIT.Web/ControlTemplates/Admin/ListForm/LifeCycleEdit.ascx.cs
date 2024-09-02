using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Web.UI.HtmlControls;
using uGovernIT.Core;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class LifeCycleEdit : UserControl
    {
        //SPList spList;
        List<LifeCycle> projectLifeCycles;
        LifeCycleManager lcHelper;
        LifeCycle lifeCycle;

        protected override void OnInit(EventArgs e)
        {
            lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            projectLifeCycles = lcHelper.LoadProjectLifeCycles();

            if (!string.IsNullOrEmpty(Request["lifecycle"]))
            {
                lifeCycle = lcHelper.LoadProjectLifeCycleByName(Uri.UnescapeDataString(Request["lifecycle"]));
            }


            if (lifeCycle != null)
            {
                txtTitle.Text = lifeCycle.Name;
                txtDescription.Text = lifeCycle.Description;
                txtOrder.Text = lifeCycle.ItemOrder.ToString();
            }
            else
            {
                lifecycleForm.Visible = false;
                pMsgDetail.Visible = true;

                Label msg = new Label();
                msg.Text = string.Format("{0} Lifecycle does not exist.", Request["lifecycle"]);
                msg.Style.Add(HtmlTextWriterStyle.Color, "red");
                msg.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                msg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                pMsgDetail.Controls.Add(msg);
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void btSaveLifeCycle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            lifeCycle.Name = txtTitle.Text.Trim();
            lifeCycle.Description = txtDescription.Text.Trim();
            int order = 0;
            int.TryParse(txtOrder.Text.Trim(), out order);
            lifeCycle.ItemOrder = order;
            lifeCycle.ModuleNameLookup = ModuleNames.PMM;
            LifeCycleManager lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            lcHelper.Update(lifeCycle);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated life cycle: {lifeCycle.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            //SPListItem item = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ProjectLifeCycles, lifeCycle.ID, SPContext.Current.Web);
            //lcHelper.FeedDataInPLCItem(item, lifeCycle);
            //item.UpdateOverwriteVersion();

            uHelper.ClosePopUpAndEndResponse(Context, true);

           
        }

        protected void cvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if ( projectLifeCycles.Exists(x =>x.ID != lifeCycle.ID && x.Name.ToLower() == txtTitle.Text.Trim().ToLower()))
            {
                args.IsValid = false;
            }
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            //SPListItem item = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ProjectLifeCycles, lifeCycle.ID, SPContext.Current.Web);
            //if (item != null)
            //{
            //    item.Delete();
            //}
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
