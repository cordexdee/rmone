using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Core;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Core;

namespace uGovernIT.Web
{
    public partial class LifeCycleStageEdit : UserControl
    {

        public int StageID { get; set; }
        public string LifeCycle { get; set; }
        LifeCycleManager lcHelper;
        LifeCycle projectLifeCycle;
        LifeCycleStage lifeCycleStage;
        LifeCycleStageManager objLifeCycleStageManager;
        protected override void OnInit(EventArgs e)
        {
            objLifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
            if (Request["lifecycle"] != null)
            {
                LifeCycle = Uri.UnescapeDataString(Request["lifecycle"]);
            }

            lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            projectLifeCycle = lcHelper.LoadProjectLifeCycleByName(LifeCycle);
            if (Request["stageid"] != null)
            {

                int stageID = 0;
                int.TryParse(Request["stageid"], out stageID);
                StageID = stageID;
            }

            lifeCycleStage = projectLifeCycle.Stages.FirstOrDefault(x => x.ID == StageID);
            if (lifeCycleStage != null)
            {
                txtTitle.Text = lifeCycleStage.Name;
                txtDescription.Text = lifeCycleStage.Description;
                txtStep.Text = lifeCycleStage.StageStep.ToString();
                txtWeight.Text = lifeCycleStage.StageWeight.ToString("0");
                //Added by mudassir 24 feb 2020
                txtCapacityMax.Text = lifeCycleStage.StageCapacityMax.ToString();
                txtCapacityNormal.Text = lifeCycleStage.StageCapacityNormal.ToString();
                UGITFileUploadManager1.SetImageUrl(lifeCycleStage.IconUrl);
            }

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btSaveLifeCycleStage_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (lifeCycleStage != null)
            {
                lifeCycleStage.Name = txtTitle.Text.Trim();
                lifeCycleStage.StageTitle = lifeCycleStage.Name;
                lifeCycleStage.Description = txtDescription.Text.Trim();

                double weight = 0;
                double.TryParse(txtWeight.Text.Trim(), out weight);
                lifeCycleStage.StageWeight = Math.Round(weight, 2);
                
                int StageStep = 0;
                int.TryParse(txtStep.Text.Trim(), out StageStep);
                lifeCycleStage.StageStep = StageStep;
                //Added by mudassir 24 feb 2020
                lifeCycleStage.StageCapacityNormal = UGITUtility.StringToInt(txtCapacityNormal.Text.Trim());
                lifeCycleStage.StageCapacityMax = UGITUtility.StringToInt(txtCapacityMax.Text.Trim());
                //
                lifeCycleStage.IconUrl = UGITFileUploadManager1.GetImageUrl();
                lifeCycleStage.ModuleNameLookup = ModuleNames.PMM;
                objLifeCycleStageManager.Update(lifeCycleStage);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated life cycle stage: {lifeCycleStage.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            }
            

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void cvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0 &&
                projectLifeCycle.Stages.Exists(x=>x.ID != lifeCycleStage.ID && x.Name.ToLower() == txtTitle.Text.Trim().ToLower()))
            {
                args.IsValid = false;
            }
        }

        protected void cvStep_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int stepNumber = 0;
            int.TryParse(txtStep.Text.Trim(), out stepNumber);

            if (stepNumber <= 0)
            {
                args.IsValid = false;
            }
            else if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0 && projectLifeCycle.Stages.Exists(x => x.StageStep == stepNumber && x.ID != StageID))
            {
                args.IsValid = false;
            }
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (lifeCycleStage != null)
            {
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Delete life cycle stage: {lifeCycleStage.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                objLifeCycleStageManager.Delete(lifeCycleStage);
            }


            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);  // UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestageaddedit"));
        }
    }
}
