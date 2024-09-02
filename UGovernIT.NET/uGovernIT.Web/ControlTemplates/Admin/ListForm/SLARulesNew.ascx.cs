using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class SLARulesNew : UserControl
    {
        private DataRow SPSLARule;
        string moduleName;
        private string TenantID = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        SlaRulesManager objSlaRulesManager;
        PrioirtyViewManager prioirtyViewManager;
        ModuleViewManager moduleViewManager;
        protected override void OnInit(EventArgs e)
        {
            moduleViewManager = new ModuleViewManager(this.context);
            objSlaRulesManager = new SlaRulesManager(this.context);
            prioirtyViewManager = new PrioirtyViewManager(this.context);
            TenantID = Convert.ToString(Session["TenantID"]);
            SPSLARule = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, $"TenantID='{TenantID}'").NewRow();
            if (!IsPostBack)
                BindModule();

            if (IsPostBack)
            {
                moduleName = Request[ddlModule.UniqueID];
            }
            else
            {
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Request["Module"]));
                if (ddlModule.SelectedIndex == -1 && ddlModule.Items.Count > 0)
                    ddlModule.SelectedIndex = 0;

                if (ddlModule.SelectedItem != null)
                    moduleName = ddlModule.SelectedValue;
            }

            if (string.IsNullOrWhiteSpace(moduleName))
            {
                return;
            }

            BindPriority(ddlModule.SelectedValue);
            BindSLACategory();
            BindSLADaysRoundUpDown();
            BindModuleStep(ddlModule.SelectedValue, ddlStartStage);
            BindModuleStep(ddlModule.SelectedValue, ddlEndStage);
            //Fill();
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
        void BindSLACategory()
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.SLACategoryChoice,DatabaseObjects.Tables.SLARule);
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
            DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.SLADaysRoundUpDown,DatabaseObjects.Tables.SLARule);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow choice in dt.Rows)
                {
                    ddlSLADaysRoundUpDown.Items.Add(new ListItem(Convert.ToString(choice[DatabaseObjects.Columns.Title]), Convert.ToString(choice[DatabaseObjects.Columns.ID])));
                }
            }
            ddlSLADaysRoundUpDown.Items.Insert(0, new ListItem("None", "0"));
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
          
            if (!Page.IsValid)
                return;
            ModuleSLARule sla = new ModuleSLARule();
            sla.SLACategoryChoice = ddlSLACategory.SelectedValue.Trim();
            //SPSLARule[DatabaseObjects.Columns.SLACategory] = ddlSLACategory.SelectedValue.Trim();
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule dataRow = moduleViewManager.GetByName(ddlModule.SelectedValue);
            sla.Title = ddlModule.SelectedValue + " - " + ddlPriority.SelectedItem.Text + " - " + ddlSLACategory.SelectedItem.Text.Trim();
            sla.ModuleNameLookup = dataRow.ModuleName;
            //SPSLARule[DatabaseObjects.Columns.ModuleNameLookup] = Convert.ToInt32(dataRow[DatabaseObjects.Columns.Id]);

            sla.PriorityLookup = Convert.ToInt32(ddlPriority.SelectedValue);
            if (ddlSLAUnitType.SelectedValue == "Days")
            {
                sla.SLAHours = UGITUtility.StringToDouble(Convert.ToDecimal(txtSLAHours.Text.Trim()) * uHelper.GetWorkingHoursInADay(context, true));

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
    }
}
