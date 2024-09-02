using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Core;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Web;
using System.Data;
using System.Linq;
using uGovernIT.Manager.Core;

namespace uGovernIT.Web
{
    public partial class LifeCycleStageNew : UserControl
    {
        //SPList spList;
        //SPListItem item;
        public int StageID { get; set; }
        public string LifeCycle { get; set; }
        //bool? isNewStage;
        LifeCycleManager lcHelper;
        LifeCycle projectLifeCycle;
        LifeCycleStage lifeCycleStage;
        LifeCycleStageManager objLifeCycleStageManager;
        protected override void OnInit(EventArgs e)
        {
            //spList = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectLifeCycleStages);
            objLifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
            if (Request["lifecycle"] != null)
            {
                LifeCycle = Uri.UnescapeDataString(Request["lifecycle"]);
            }

            lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            projectLifeCycle = lcHelper.LoadProjectLifeCycleByName(LifeCycle);


            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSaveLifeCycleStage_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;


            if (lifeCycleStage == null)
                lifeCycleStage = new LifeCycleStage();
            lifeCycleStage.LifeCycleName = lcHelper.LoadLifeCycleByModule(ModuleNames.PMM).Where(x => x.Name == LifeCycle).First().ID;
            lifeCycleStage.Name = txtTitle.Text.Trim();
            lifeCycleStage.StageTitle = lifeCycleStage.Name;
            lifeCycleStage.Description = txtDescription.Text.Trim();
            ushort step = 0;
            ushort.TryParse(txtStep.Text.Trim(), out step);
            lifeCycleStage.StageStep = step;
            double weight = 0;
            double.TryParse(txtWeight.Text.Trim(), out weight);
            lifeCycleStage.StageWeight = weight;
            lifeCycleStage.IconUrl = UGITFileUploadManager1.GetImageUrl();
            //newStage.LifeCycleID = projectLifeCycle.ID;
            lifeCycleStage.ModuleNameLookup = ModuleNames.PMM;
            objLifeCycleStageManager.Insert(lifeCycleStage);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added life cycle stage: {lifeCycleStage.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

        protected void cvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
           if( projectLifeCycle.Stages.Exists(x=>x.Name.ToLower() == txtTitle.Text.Trim().ToLower()))
           {
               args.IsValid=false;
           }
        }

        protected void cvStep_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0)
            {
                int stepNumber = 0;
                int.TryParse(txtStep.Text.Trim(), out stepNumber);

                if (projectLifeCycle.Stages.Exists(x => x.StageStep == stepNumber && x.ID != StageID))
                {
                    args.IsValid = false;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
