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
    public partial class SLAEscalationEdit : UserControl
    {
        public int SLAEscalationID { get; set; }
        private ModuleEscalationRule _SPListItemEscalation;
        private string moduleName = string.Empty;
        HtmlEditorControl htmlEditor;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleEscalationRuleManager objModuleEscalationRuleManager;
        ModuleViewManager moduleViewManager;
        SlaRulesManager slaRulesManager;
        ModuleUserTypeManager moduleUserTypeManager;
        protected override void OnInit(EventArgs e)
        {
            moduleUserTypeManager = new ModuleUserTypeManager(this.context);
            moduleViewManager = new ModuleViewManager(this.context);
            slaRulesManager = new SlaRulesManager(this.context);
            objModuleEscalationRuleManager = new ModuleEscalationRuleManager(context);
            htmlEditor = (HtmlEditorControl)HtmlEditorControl;
            _SPListItemEscalation = objModuleEscalationRuleManager.LoadByID(SLAEscalationID);
          //  BindSLARules(moduleName);
            Fill();

            base.OnInit(e);
        }

        private void BindSLARules(string moduleName)
        {
            List<ModuleSLARule> moduleSLARuleList = slaRulesManager.Load(x => x.ModuleNameLookup == moduleName);
            ddlSLARule.Items.Clear();
            if (moduleSLARuleList != null && moduleSLARuleList.Count() > 0)
            {
                foreach (ModuleSLARule row in moduleSLARuleList)
                {
                    ddlSLARule.Items.Add(new ListItem(Convert.ToString(row.Title), Convert.ToString(row.ID)));
                }
            }
        }

        private void BindEscalationRole(int slaRuleIdLookUp)
        {
            chklstEscalationRules.Items.Clear();
            ///in Escalation modules are not specified
            ///to get module need to load SLA Rules.
            ModuleSLARule moduleSLARule = slaRulesManager.LoadByID(Convert.ToInt64(slaRuleIdLookUp));
            if (moduleSLARule != null)
            {
                moduleName = Convert.ToString(moduleSLARule.ModuleNameLookup);
            }

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

        private void Fill()
        {
          
            string moduleValue = GetModuleName(Convert.ToInt64(_SPListItemEscalation.SLARuleIdLookup));
            BindModule();
            ddlModule.SelectedValue = moduleValue;
            BindEscalationRole(Convert.ToInt32( _SPListItemEscalation.SLARuleIdLookup));
            BindSLARules(moduleValue);
            //ddlSLARule.Items.FindByValue(Convert.ToString(_SPListItemEscalation.SLARuleIdLookup)).Selected = true;
            ddlSLARule.SelectedValue = Convert.ToString(_SPListItemEscalation.SLARuleIdLookup);
            double escalationMinutes = Convert.ToDouble(_SPListItemEscalation.EscalationMinutes);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
            if (escalationMinutes % (workingHoursInADay * 60) == 0)
            {
                txtEscalationMinutes.Text = string.Format("{0:0.##}", escalationMinutes / (workingHoursInADay * 60));
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (escalationMinutes % 60 == 0)
            {
                txtEscalationMinutes.Text = string.Format("{0:0.##}", escalationMinutes / 60);
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                txtEscalationMinutes.Text = string.Format("{0:0.##}", escalationMinutes);
                ddlSLAUnitType.SelectedValue = Constants.SLAConstants.Minutes;
            }


            if (Convert.ToDouble(_SPListItemEscalation.EscalationFrequency) >= uHelper.GetWorkingHoursInADay(context,true) * 60)
            {
                txtEscalationFrequency.Text = string.Format("{0:0.##}", Convert.ToDouble(_SPListItemEscalation.EscalationFrequency) / (uHelper.GetWorkingHoursInADay(context, true) * 60));
                ddlSLAFreqUnitType.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (Convert.ToDouble(_SPListItemEscalation.EscalationFrequency) >= 60)
            {
                txtEscalationFrequency.Text = string.Format("{0:0.##}", Convert.ToDouble(_SPListItemEscalation.EscalationFrequency) / 60);
                ddlSLAFreqUnitType.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                txtEscalationFrequency.Text = string.Format("{0:0.##}", Convert.ToDouble(_SPListItemEscalation.EscalationFrequency));
                ddlSLAFreqUnitType.SelectedValue = Constants.SLAConstants.Minutes;
            }

            txtEscalationEmail.Text = Convert.ToString(_SPListItemEscalation.EscalationToEmails);
            //commented by munna as not found field
            chkbxNotifyInPlainText.Checked = _SPListItemEscalation.NotifyInPlainText;
            hdnEscalationRules.Value = Convert.ToString(_SPListItemEscalation.EscalationToRoles);
            SelectEmailUserTypesValue();
            txtMailSubject.Text = Convert.ToString(_SPListItemEscalation.EscalationMailSubject);
            htmlEditor.Html = Convert.ToString(_SPListItemEscalation.EscalationEmailBody);
            txtDescription.Text = Convert.ToString(_SPListItemEscalation.EscalationDescription);
            cbIncludeActionUser.Checked = UGITUtility.StringToBoolean(_SPListItemEscalation.IncludeActionUsers);
            cbUseDesiredComDate.Checked = UGITUtility.StringToBoolean(_SPListItemEscalation.UseDesiredCompletionDate);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            _SPListItemEscalation.Title = ddlSLARule.SelectedValue + " - " + txtEscalationMinutes.Text.Trim() + " " + ddlSLAUnitType.SelectedValue;

            _SPListItemEscalation.SLARuleIdLookup = Convert.ToInt32(ddlSLARule.SelectedValue);

            // Converting days,hours into minutes
            if (ddlSLAUnitType.SelectedValue == Constants.SLAConstants.Days)
            {
                _SPListItemEscalation.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, true);
            }
            else if (ddlSLAUnitType.SelectedValue == Constants.SLAConstants.Hours)
            {
                _SPListItemEscalation.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim()) * 60;
            }
            else
            {
                _SPListItemEscalation.EscalationMinutes = Convert.ToDouble(txtEscalationMinutes.Text.Trim());
            }

            if (ddlSLAFreqUnitType.SelectedValue == Constants.SLAConstants.Days)
            {
                _SPListItemEscalation.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, true);
            }
            else if (ddlSLAFreqUnitType.SelectedValue == Constants.SLAConstants.Hours)
            {
                _SPListItemEscalation.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim()) * 60;
            }
            else
            {
                _SPListItemEscalation.EscalationFrequency = Convert.ToDouble(txtEscalationFrequency.Text.Trim());
            }

            string[] emails = txtEscalationEmail.Text.Trim().Split(new char[] { ' ', ';', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            _SPListItemEscalation.EscalationToEmails = String.Join(";", emails);
            _SPListItemEscalation.NotifyInPlainText = chkbxNotifyInPlainText.Checked;
            _SPListItemEscalation.EscalationToRoles = hdnEscalationRules.Value.Trim();
            _SPListItemEscalation.EscalationMailSubject = txtMailSubject.Text.Trim();
            _SPListItemEscalation.EscalationEmailBody = htmlEditor.Html.Trim();
            _SPListItemEscalation.EscalationDescription = txtDescription.Text.Trim();
            _SPListItemEscalation.IncludeActionUsers = cbIncludeActionUser.Checked ? true : false;
            _SPListItemEscalation.UseDesiredCompletionDate = cbUseDesiredComDate.Checked ? true : false;
            objModuleEscalationRuleManager.AddUpdateSLARule(_SPListItemEscalation);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (_SPListItemEscalation != null)
            {
                objModuleEscalationRuleManager.Delete(_SPListItemEscalation);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
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
        private void SelectEmailUserTypesValue()
        {
            string[] emailUsertype = hdnEscalationRules.Value.Split(';', '#');
            foreach (var item in emailUsertype)
            {
                int index = chklstEscalationRules.Items.IndexOf(chklstEscalationRules.Items.FindByValue(item));
                if (index >= 0)
                {
                    chklstEscalationRules.Items[index].Selected = true;
                }
            }
        }

        protected void ddlSLARule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEscalationRole(Convert.ToInt32(ddlSLARule.SelectedValue));
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSLARules(ddlModule.SelectedValue);
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

        private string GetModuleName(long slaRuleId)
        {
            ModuleSLARule moduleSLARule = slaRulesManager.LoadByID(slaRuleId);
            return Convert.ToString(moduleSLARule.ModuleNameLookup);
        }
    }
}
