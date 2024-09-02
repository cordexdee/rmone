using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{
    public partial class SLAEscalationNew : UserControl
    {
      
        HtmlEditorControl htmlEditor;
        string moduleName;
        private string TenantID = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleEscalationRuleManager objModuleEscalationRuleManager;
        ModuleViewManager moduleViewManager;
        SlaRulesManager slaRulesManager;
        ModuleUserTypeManager moduleUserTypeManager;
        protected override void OnInit(EventArgs e)
        {
            moduleViewManager = new ModuleViewManager(this.context);
            objModuleEscalationRuleManager = new ModuleEscalationRuleManager(this.context);
            slaRulesManager = new SlaRulesManager(this.context);
            moduleUserTypeManager = new ModuleUserTypeManager(this.context);
            htmlEditor = (HtmlEditorControl)HtmlEditorControl;
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

            BindSLARules(moduleName);

            BindEscalationRole();
            base.OnInit(e);
        }

        private void BindSLARules(string moduleName)
        {
            List<ModuleSLARule> moduleSLARuleList = slaRulesManager.Load(x => x.ModuleNameLookup == moduleName && !x.Deleted).OrderBy(x => x.Title).ToList();
            ddlSLARule.Items.Clear();
            if (moduleSLARuleList != null && moduleSLARuleList.Count() > 0)
            {
                foreach (ModuleSLARule row in moduleSLARuleList)
                {
                    ddlSLARule.Items.Add(new ListItem(Convert.ToString(row.Title), Convert.ToString(row.ID)));
                }
            }
        }

        private void BindEscalationRole()
        {
            chklstEscalationRules.Items.Clear();
            List<ModuleUserType> moduleUserType = moduleUserTypeManager.Load(x => x.ModuleNameLookup == moduleName);
            if (moduleUserType != null && moduleUserType.Count > 0)
            {
                foreach (ModuleUserType dr in moduleUserType)
                {
                    chklstEscalationRules.Items.Add(new ListItem(Convert.ToString(dr.UserTypes), Convert.ToString(dr.ColumnName)));
                }
            }
            chklstEscalationRules.Items.Add(new ListItem("Escalation Manager", "RequestTypeEscalationManager"));
            chklstEscalationRules.Items.Add(new ListItem("Backup Escalation Manager", "RequestTypeBackupEscalationManager"));
            chklstEscalationRules.Items.Add(new ListItem("PRP Manager", "PRPManager"));
            chklstEscalationRules.Items.Add(new ListItem("ORP Manager", "ORPManager"));
        }

        protected void ddlSLARule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEscalationRole();
        }

        protected void chklstEscalationRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnEscalationRules.Value = string.Empty;
            if (chklstEscalationRules.SelectedIndex > -1)
            {
                foreach (ListItem item in chklstEscalationRules.Items)
                {
                    if (item.Selected)
                    {
                        hdnEscalationRules.Value = hdnEscalationRules.Value.Trim() + (String.IsNullOrEmpty(hdnEscalationRules.Value.Trim()) ? "" : Constants.Separator) + item.Value;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleEscalationRule mer = new ModuleEscalationRule();
            mer.Title = ddlSLARule.SelectedValue + " - " + txtEscalationMinutes.Text.Trim() + " " + ddlSLAUnitType.SelectedValue;
            mer.SLARuleIdLookup = Convert.ToInt32(ddlSLARule.SelectedValue);
            if (ddlSLAUnitType.SelectedValue == "Days")
            {
                mer.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, true);

            }
            else if (ddlSLAUnitType.SelectedValue == "Hours")
            {
                mer.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim()) * 60;

            }
            else
            {
                mer.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim());

            }

            if (ddlSLAFreqUnitType.SelectedValue == "Days")
            {
                mer.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, true);

            }
            else if (ddlSLAFreqUnitType.SelectedValue == "Hours")
            {
                mer.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim()) * 60;

            }
            else
            {
                mer.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim());

            }

            string[] emails = txtEscalationEmail.Text.Trim().Split(new char[] { ' ', ';', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            mer.EscalationToEmails = String.Join(";", emails);
            mer.NotifyInPlainText = chkbxNotifyInPlainText.Checked;
            mer.EscalationToRoles = hdnEscalationRules.Value.Trim();
            mer.EscalationMailSubject = txtMailSubject.Text.Trim();
            mer.EscalationEmailBody = htmlEditor.Html.Trim();
            mer.EscalationDescription = txtDescription.Text.Trim();
            mer.IncludeActionUsers = cbIncludeActionUser.Checked ? true : false;
            mer.UseDesiredCompletionDate = cbUseDesiredComDate.Checked ? true : false;
            objModuleEscalationRuleManager.AddUpdateSLARule(mer);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void BindModule()
        {
            List<UGITModule> ugitModule = new List<UGITModule>();
            ugitModule = moduleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (ugitModule != null && ugitModule.Count > 0)
            {
                List<ModuleSLARule> slaRule = slaRulesManager.Load();
                foreach (UGITModule moduleRow in ugitModule)
                {
                    string moduleName = Convert.ToString(moduleRow.ModuleName);
                    bool contains = slaRule.AsEnumerable().Any(row => moduleName == row.ModuleNameLookup);

                    if (moduleName != string.Empty && contains)
                        ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow.Title), Value = moduleName });
                }
                ddlModule.DataBind();
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSLARules(ddlModule.SelectedValue);
            BindEscalationRole();
        }
    }
}
