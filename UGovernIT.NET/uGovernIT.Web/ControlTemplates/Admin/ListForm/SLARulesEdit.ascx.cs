using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class SLARulesEdit : UserControl
    {
        public int SLARuleID { private get; set; }
        public bool ReadOnly { get; set; }
        //private DataRow SPSLARule;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleSLARule SPSLARule;
        SlaRulesManager objSlaRulesManager;
        PrioirtyViewManager prioirtyViewManager;
        ModuleViewManager moduleViewManager;
        protected override void OnInit(EventArgs e)
        {
            if (UGITUtility.StringToBoolean(Request["ReadOnly"]))
            {
                ReadOnly = true;
                tblactiontable.Visible = false;
            }

            moduleViewManager = new ModuleViewManager(this.context);
            objSlaRulesManager = new SlaRulesManager(this.context);
            prioirtyViewManager = new PrioirtyViewManager(this.context);
            SPSLARule = objSlaRulesManager.LoadByID(Convert.ToInt64(SLARuleID));
            BindModule();
            BindPriority(ddlModule.SelectedValue);
            BindSLACategory();
            BindSLADaysRoundUpDown();
            BindModuleStep(ddlModule.SelectedValue, ddlStartStage);
            BindModuleStep(ddlModule.SelectedValue, ddlEndStage);
            if (!IsPostBack)
            {
                Fill();
            }
            base.OnInit(e);
        }

        private void BindModule()
        {
            List<UGITModule> ugitModule = new List<UGITModule>();
            ugitModule = moduleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (ugitModule != null && ugitModule.Count > 0)
            {

                foreach (UGITModule moduleRow in ugitModule)
                {
                    string moduleName = Convert.ToString(moduleRow.ModuleName);


                    if (moduleName != string.Empty)
                        ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow.Title), Value = moduleName });
                }
                ddlModule.DataBind();
            }
        }

        void BindPriority(string Module)
        {
            ddlPriority.Items.Clear();
            List<ModulePrioirty> ModulePrioirtyList = prioirtyViewManager.Load(x => x.ModuleNameLookup == Module);
            if (ModulePrioirtyList != null && ModulePrioirtyList.Count > 0)
            {
                foreach (ModulePrioirty row in ModulePrioirtyList)
                {
                    ddlPriority.Items.Add(new ListItem(Convert.ToString(row.uPriority), Convert.ToString(row.ID)));
                }
                ddlPriority.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        void BindSLACategory()
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.SLACategoryChoice, DatabaseObjects.Tables.SLARule);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow choice in dt.Rows)
                {
                    ddlSLACategory.Items.Add(new ListItem(Convert.ToString(choice[DatabaseObjects.Columns.Title]), Convert.ToString(choice[DatabaseObjects.Columns.ID])));
                }
            }
            ddlSLACategory.Items.Insert(0, new ListItem("None", "0"));
        }

        void BindSLADaysRoundUpDown()
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.SLADaysRoundUpDown, DatabaseObjects.Tables.SLARule);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow choice in dt.Rows)
                {
                    ddlSLADaysRoundUpDown.Items.Add(new ListItem(Convert.ToString(choice[DatabaseObjects.Columns.Title]), Convert.ToString(choice[DatabaseObjects.Columns.ID])));
                }
            }
        }

        void BindModuleStep(string Module, DropDownList control)
        {
            control.Items.Clear();
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(this.context);
            List<LifeCycle> lifecyle = lifeCycleHelper.LoadLifeCycleByModule(Module);
            if (lifecyle != null && lifecyle.Count > 0)
            {
                foreach (LifeCycle obj in lifecyle)
                {
                    foreach (LifeCycleStage row in obj.Stages)
                    {
                        control.Items.Add(new ListItem(string.Format("{0} - {1}", Convert.ToString(row.StageStep), Convert.ToString(row.StageTitle)),
                                                       Convert.ToString(row.ID)));
                    }
                }
                control.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        void Fill()
        {
            ddlSLACategory.SelectedValue = Convert.ToString(SPSLARule.SLACategoryChoice);
            ddlModule.SelectedValue = Convert.ToString(SPSLARule.ModuleNameLookup);
            BindPriority(ddlModule.SelectedValue);
            BindModuleStep(ddlModule.SelectedValue, ddlStartStage);
            BindModuleStep(ddlModule.SelectedValue, ddlEndStage);
            ddlPriority.SelectedValue = Convert.ToString(Convert.ToString(SPSLARule.PriorityLookup));
            double SLAHours = Convert.ToDouble(SPSLARule.SLAHours);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
            if (SLAHours % workingHoursInADay == 0)
            {
                txtSLAHours.Text = string.Format("{0:0.##}", SLAHours / workingHoursInADay);
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (SLAHours % 1 == 0)
            {
                txtSLAHours.Text = string.Format("{0:0.##}", SLAHours);
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                txtSLAHours.Text = string.Format("{0:0.##}", Convert.ToInt32(SLAHours * 60));
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Minutes;
            }
            ddlSLADaysRoundUpDown.SelectedValue = Convert.ToString(SPSLARule.SLADaysRoundUpDownChoice);
            txtSLATarget.Text = Convert.ToString(SPSLARule.SLATarget);
            ddlStartStage.SelectedIndex = Convert.ToInt32(SPSLARule.StartStageStep);
            ddlEndStage.SelectedIndex = Convert.ToInt32(SPSLARule.EndStageStep);
            txtDescription.Text = Convert.ToString(SPSLARule.ModuleDescription);
            if (!string.IsNullOrEmpty(Request["ID"]))
            {
                tr11.Visible = true;
            }
            //if (SPSLARule.IsDeleted)
            //{
            //    tr11.Visible = true;
            //}
            else
            {
                tr11.Visible = false;
            }
            chkDeleted.Checked = Convert.ToBoolean(SPSLARule.Deleted);
            txtTitle.Text = Convert.ToString(SPSLARule.Title);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            ModuleSLARule sla = new ModuleSLARule();
            sla.SLACategoryChoice = ddlSLACategory.SelectedValue.Trim();
            ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule dataRow = ObjModuleViewManager.GetByName(ddlModule.SelectedValue);
            sla.Title = ddlModule.SelectedValue + " - " + ddlPriority.SelectedItem.Text + " - " + ddlSLACategory.SelectedItem.Text.Trim();
            sla.ModuleNameLookup = dataRow.ModuleName;
            sla.PriorityLookup = Convert.ToInt32(ddlPriority.SelectedValue);
            if (ddlSLAUnitType.SelectedValue == "Days")
            {
                sla.SLAHours = Convert.ToInt32(txtSLAHours.Text.Trim()) * uHelper.GetWorkingHoursInADay(context, true);
            }
            else if (ddlSLAUnitType.SelectedValue == "Hours")
            {
                sla.SLAHours = Convert.ToInt32(txtSLAHours.Text.Trim());
            }
            else // Minutes
            {
                sla.SLAHours = UGITUtility.StringToDouble(Convert.ToDecimal(txtSLAHours.Text.Trim()) / 60);
            }
            sla.SLADaysRoundUpDownChoice = ddlSLADaysRoundUpDown.SelectedValue;
            sla.SLATarget = Convert.ToInt32(txtSLATarget.Text.Trim());
            sla.StageTitleLookup = Convert.ToInt32(ddlStartStage.SelectedValue);
            sla.EndStageTitleLookup = Convert.ToInt32(ddlEndStage.SelectedValue);
            sla.ModuleDescription = txtDescription.Text.Trim();
            sla.StartStageStep = ddlStartStage.SelectedIndex;
            sla.EndStageStep = ddlEndStage.SelectedIndex;
            sla.Deleted = chkDeleted.Checked;
            sla.Title = txtTitle.Text.Trim();
            sla.ID = SLARuleID;
            bool added = objSlaRulesManager.AddUpdateSLARule(sla).ID > 0;
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPriority(ddlModule.SelectedValue);
            BindModuleStep(ddlModule.SelectedValue, ddlStartStage);
            BindModuleStep(ddlModule.SelectedValue, ddlEndStage);
        }

        protected void DeletButton_Click(object sender, EventArgs e)
        {
            if (SPSLARule != null)
            {
                SPSLARule.Deleted = true;
                objSlaRulesManager.Update(SPSLARule);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }

        }
    }
}
