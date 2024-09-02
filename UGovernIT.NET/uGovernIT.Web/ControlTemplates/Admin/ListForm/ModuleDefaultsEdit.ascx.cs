using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ModuleDefaultsEdit : UserControl
    {
        public int Id { private get; set; }
        public string ModuleName { get; set; }
        ModuleDefaultValue spItemModuleDefaults;
        ModuleDefaultValueManager ObjModuleDefaultValueManager = new ModuleDefaultValueManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            if (Id > 0)
            {
                lnkDelete.Visible = true;
                spItemModuleDefaults = ObjModuleDefaultValueManager.LoadByID(Id);
            }
            if (spItemModuleDefaults ==null)
            {
                spItemModuleDefaults = new ModuleDefaultValue();
            }

            if (!IsPostBack)
            {
                FillFieldName(ModuleName);

                BindModule();
                Fill();
                BindModuleStep();
                if (!string.IsNullOrEmpty(spItemModuleDefaults.ModuleStepLookup))
                    ddlModuleStep.SelectedIndex = ddlModuleStep.Items.IndexOf(ddlModuleStep.Items.FindByValue(Convert.ToString(spItemModuleDefaults.ModuleStepLookup)));
            }
           
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            long moduleId = ObjModuleViewManager.LoadByName(ddlModule.SelectedValue).ID;
            //spItemModuleDefaults[DatabaseObjects.Columns.Title] = moduleId + " - " + txtKeyName.Text;
            spItemModuleDefaults.ModuleNameLookup = ddlModule.SelectedValue;         
            spItemModuleDefaults.Title = moduleId + " - " + cmbFieldName.Text;
            spItemModuleDefaults.KeyName = cmbFieldName.Text;

            string keyValue = string.Empty;
            if (ddlKeyValue.SelectedItem.Text == "Text")
                keyValue = txtKeyValue.Text;
            else
                keyValue = ddlKeyValue.SelectedItem.Text;

            spItemModuleDefaults.KeyValue = keyValue;

            spItemModuleDefaults.ModuleStepLookup = ddlModuleStep.SelectedValue;
            spItemModuleDefaults.CustomProperties = txtCustomProperties.Text;
            ObjModuleDefaultValueManager.AddOrUpdate(spItemModuleDefaults);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated Module Defaults: {spItemModuleDefaults.KeyName}; Module: {spItemModuleDefaults.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        void BindModule()
        {
            ddlModule.Items.Clear();
            List<UGITModule> spModuleList = ObjModuleViewManager.Load(x=>x.EnableModule).OrderBy(x=>x.ModuleName).ToList();
             foreach (UGITModule moduleRow in spModuleList)
            {
                ddlModule.Items.Add(new ListItem { Text = moduleRow.Title, Value =moduleRow.ModuleName });
            }
            ddlModule.DataBind();
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = ddlModule.SelectedValue;
            FillFieldName(module);
            cmbFieldName.Text = "";
            BindModuleStep();
        }

        void BindModuleStep()
        {
            string moduleName = ddlModule.SelectedValue;
            ddlModuleStep.Items.Clear();
            LifeCycleStageManager lifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
            
            List<LifeCycleStage> spModuleStepList = lifeCycleStageManager.Load(x=>x.ModuleNameLookup==moduleName).OrderBy(x=>x.StageStep).ToList();//SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleStages);
           if (spModuleStepList != null && spModuleStepList.Count > 0)
            {
                foreach (LifeCycleStage moduleStepRow in spModuleStepList)
                {
                    ddlModuleStep.Items.Add(new ListItem { Text = string.Format("{1}- {0}", Convert.ToString(moduleStepRow.StageTitle), Convert.ToString(moduleStepRow.StageStep)), Value = Convert.ToString(moduleStepRow.ID) });
                }
            }
            ddlModuleStep.DataBind();
        }


        private void Fill()
        {
            
            ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(spItemModuleDefaults.ModuleNameLookup));
            cmbFieldName.Text = spItemModuleDefaults.KeyName;

           int selectedIndex = ddlKeyValue.Items.IndexOf(ddlKeyValue.Items.FindByValue(spItemModuleDefaults.KeyValue));
           if (selectedIndex == -1)
           {
               ddlKeyValue.SelectedValue = "Text";
               txtKeyValue.Visible = true;
               txtKeyValue.Text = spItemModuleDefaults.KeyValue;
           }
           else
               ddlKeyValue.SelectedIndex = selectedIndex;
           
            ddlModuleStep.SelectedIndex = ddlModuleStep.Items.IndexOf(ddlModuleStep.Items.FindByValue(spItemModuleDefaults.ModuleStepLookup));
            txtCustomProperties.Text = spItemModuleDefaults.CustomProperties;
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Module Defaults: {spItemModuleDefaults.KeyName}; Module: {spItemModuleDefaults.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            ObjModuleDefaultValueManager.Delete(spItemModuleDefaults);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        //fill Field Dropdown.
        void FillFieldName(string moduleName)
        {
            UGITModule spListItem = ObjModuleViewManager.LoadByName(moduleName);
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            DataTable SPTickets = ticketManager.GetAllTickets(spListItem);//  GetSPList(Convert.ToString(spListItem[DatabaseObjects.Columns.ModuleTicketTable]));

            List<string> fields = new List<string>();
            foreach (DataColumn spField in SPTickets.Columns)
            {
                  fields.Add(spField.ColumnName);               
            }

            fields.Sort();
            cmbFieldName.DataSource = fields;
            cmbFieldName.DataBind();
        }

        protected void ddlKeyValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKeyValue.SelectedItem.Text == "Text")
                txtKeyValue.Visible = true;
            else
                txtKeyValue.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
