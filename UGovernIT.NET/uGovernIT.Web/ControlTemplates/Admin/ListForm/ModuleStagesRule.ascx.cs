using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager.Core;
using System.Web;
using uGovernIT.Manager.Managers;


namespace uGovernIT.Web
{
    public partial class ModuleStagesRule : UserControl
    {
        public List<FormulaExpression> expressionList = new List<FormulaExpression>();
        public TextBox TargetTextBox { get; set; }
        public Button SourceButton { get; set; }
        string expressionJson = string.Empty;
        public string ModuleName = string.Empty;
        List<LifeCycleStage> moduleStageList;
        LifeCycleStage moduleStageItem;    
        public List<FactTableField> DataFields { get; set; }
        protected string ajaxPageURL;
        public string StageId = string.Empty;
        public string SkipCondition { get; set; }
        public string ControlId { get; set; }
        TicketManager ObjTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        LifeCycleStageManager ObjLifeCycleStageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["stageID"]))
            {
                trModules.Visible = false;
                trStage.Visible = false;
            }
            else
            {
                BindModuleName();
                BindModuleStep(ddlModule.SelectedValue);
                moduleStageList = ObjLifeCycleStageManager.Load();
            }
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/Account");
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (this.ModuleName != string.Empty)
                {
                    ddlModule.SelectedValue = this.ModuleName;
                    BindModuleStep(ddlModule.SelectedValue);
                    if (this.StageId == string.Empty || this.StageId == "0")
                        this.StageId = ddlModuleStep.SelectedValue;
                }
                if (this.StageId != string.Empty && this.StageId != "0")
                {
                    if (moduleStageList != null)
                        moduleStageItem = moduleStageList.FirstOrDefault(x=>x.ID==Convert.ToInt32(this.StageId));  //moduleStageList.GetItemById(Convert.ToInt32(this.StageId));
                    if (moduleStageItem != null && !string.IsNullOrEmpty(moduleStageItem.SkipOnCondition))
                    {
                        expressionJson_hidden.Value = FormulaBuilder.GetSkipConditionExpression(Convert.ToString(moduleStageItem.SkipOnCondition), ModuleName, true);

                    }
                    else
                    {
                        string formulaVal = string.Empty;
                        expressionJson_hidden.Value = formulaVal;
                    }

                    if (!string.IsNullOrEmpty(moduleStageItem.ModuleNameLookup))
                    {
                      string spFieldLookupModule = moduleStageItem.ModuleNameLookup;
                        ddlModule.SelectedValue = spFieldLookupModule;
                        BindModuleStep(ddlModule.SelectedValue);
                        ddlModuleStep.SelectedValue = this.StageId;
                    }
                }

                if (!string.IsNullOrEmpty(SkipCondition))
                {
                    //txtStageRule.Text = Uri.UnescapeDataString(SkipCondition);
                    expressionJson_hidden.Value = Uri.UnescapeDataString(SkipCondition);
                }
            }
            if (ModuleName == "WIKI")
                BindWikiFields();
            else
                 BindFields();
                base.OnLoad(e);
        }

        #endregion

        # region control events
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModuleStep(ddlModule.SelectedValue);
            ddlModuleStep_SelectedIndexChanged(sender, e);
        }
        protected void btSaveRule_Click(object sender, EventArgs e)
        {          
            if (string.IsNullOrEmpty(this.ControlId))
            {
                LifeCycleStage moduleStageItem = ObjLifeCycleStageManager.LoadByID(UGITUtility.StringToInt(ddlModuleStep.SelectedValue));
                if (moduleStageItem != null)
                {
                    moduleStageItem.SkipOnCondition = FormulaBuilder.GetSkipConditionExpression(expressionJson_hidden.Value.Trim(), ModuleName, false);
                    ObjLifeCycleStageManager.Update(moduleStageItem);
                }
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else
            {
                string sourceUrl = string.Empty;
                if (Context.Request["source"] != null && Context.Request["source"].Trim() != string.Empty)
                {
                    sourceUrl = Context.Request["source"].Trim();
                }
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("frameUrl", sourceUrl);
                if (!string.IsNullOrWhiteSpace(Request["callbackfunction"]))
                {
                    dict.Add("ReturnValue", Uri.EscapeDataString("result"));
                    string callbackFunction = Convert.ToString(Request["callbackfunction"].Replace("filter", string.Format("\\\"{0}\\\"", Uri.EscapeDataString(expressionJson_hidden.Value.Replace("'", "@").Replace("&&", "AND").Replace("||", "OR").Trim()))));
                    dict.Add("callbackfunction", callbackFunction);
                }
                else
                {
                    dict.Add("ControlId", this.ControlId);
                    dict.Add("ReturnValue", Uri.EscapeDataString(expressionJson_hidden.Value.Replace("'", "@").Trim()));
                }
                var vals = UGITUtility.GetJsonForDictionary(dict);
                uHelper.ClosePopUpAndEndResponse(Context, false, vals);
            }
        }

        protected void ResetRule_Click(object sender, EventArgs e)
        {
            if (this.StageId != string.Empty && this.StageId != "0")
            {
                if (moduleStageList != null)
                    moduleStageItem = moduleStageList.FirstOrDefault(x=>x.ID==UGITUtility.StringToInt(ddlModuleStep.SelectedValue)); //moduleStageList.GetItemById(Convert.ToInt32(ddlModuleStep.SelectedValue));
                if (moduleStageItem != null )
                {
                    moduleStageItem.SkipOnCondition = string.Empty;
                    ObjLifeCycleStageManager.Update(moduleStageItem);                  
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void ddlModuleStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtStageRule.Text = string.Empty;
            if (moduleStageList != null && ddlModuleStep.SelectedIndex!=-1)
            {
                moduleStageItem = moduleStageList.FirstOrDefault(x=>x.ModuleNameLookup.Equals(ddlModule.SelectedValue));  
                /*if (!string.IsNullOrEmpty(moduleStageItem.SkipOnCondition))
                {
                    txtStageRule.Text = Convert.ToString(moduleStageItem.SkipOnCondition);
                    expressionJson_hidden.Value = txtStageRule.Text;
                }
                else
                {
                    txtStageRule.Text = string.Empty;
                    expressionJson_hidden.Value = txtStageRule.Text;
                }*/
            }
        }

        #endregion

        #region helpers
        /// <summary>
        /// This will bind the fields based on the module table.
        /// </summary>
        private void BindFields()
        {
            ddlField.Items.Clear();
            if (string.IsNullOrEmpty(Request["stageID"]) && !string.IsNullOrEmpty(Request["moduleName"]))
            {
                ModuleName = Convert.ToString(Request["moduleName"]); //new Ticket(Request["moduleName"]);
            }
            else
            {
                ModuleName = ddlModule.SelectedValue;
            }           
            ListItem item = null;
            String value = "";
            String text = "";
            string ticketList = string.Empty;
            ddlField.Items.Clear();
            Ticket tick;
            if (string.IsNullOrEmpty(Request["stageID"]) && !string.IsNullOrEmpty(Request["moduleName"]))
            {
                tick = new Ticket(HttpContext.Current.GetManagerContext(),Request["moduleName"]);
            }
            else
            {
                tick = new Ticket(HttpContext.Current.GetManagerContext(), ddlModule.SelectedValue);
            }
            List<uGovernIT.FactTableField> queryTableFields = UGITModuleConstraint.GetColumnNamesWithDataType(HttpContext.Current.GetManagerContext(),tick.Module.ModuleTable);

            if (queryTableFields != null)
            {
                foreach (uGovernIT.FactTableField field in queryTableFields)
                {
                    text = string.Format("{0} ({1}-{2})", field.FieldDisplayName, field.FieldName, field.DataType.Replace("System.", string.Empty));
                    value = string.Format("{0}", field.FieldDisplayName);
                    item = new ListItem(text, value);
                    item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                    ddlField.Items.Add(item);
                }
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                fieldDataJson.Value = ser.Serialize(queryTableFields);
            }
        }

        private void BindModuleName()
        {
          
            List<UGITModule> dtModule = ObjModuleViewManager.LoadAllModule().OrderBy(x=>x.Title).ToList(); 
            List<LifeCycleStage> dtMStages = ObjLifeCycleStageManager.Load();
            List<LifeCycleStage> rowCollection = null;
            if (dtMStages != null)
            {
                //dtMStages = moduleStages.Items.GetDataTable();
                if (dtMStages != null && dtMStages.Count > 0)
                {
                    for (int i = 0; i < dtModule.Count; i++)
                    {
                        rowCollection = dtMStages.Where(x=>x.ModuleNameLookup.Equals(Convert.ToString(dtModule[i].ModuleName))).ToList();
                        if (rowCollection != null && rowCollection.Count() > 2)
                            ddlModule.Items.Add(new ListItem { Text = dtModule[i].Title.ToString(), Value = dtModule[i].ModuleName.ToString() });
                    }
                }
                ddlModule.Items.Insert(0, new ListItem("(None)", "0"));
            }
        }

        void BindModuleStep(string selectedModule)
        {
            ddlModuleStep.ClearSelection();
            ddlModuleStep.Items.Clear();
            List<LifeCycleStage> selectedStages = null;
            if (string.IsNullOrEmpty(ddlModule.SelectedValue))
                return;
            int counter = 0;
            UGITModule module = ObjModuleViewManager.GetByName(ddlModule.SelectedValue);
            List<LifeCycleStage> stages = ObjLifeCycleStageManager.Load(x=>x.ModuleNameLookup.Equals(selectedModule)).OrderBy(x => x.StageStep).ToList();
            if (stages != null && stages.Count > 0)
            {
                counter = stages.Count();
                List<int> stageSteps = new List<int>();
                stageSteps.Add(1);
                stageSteps.Add(counter);
                selectedStages = stages.Where(x => !stageSteps.Contains(x.StageStep)).ToList();
               
                if (selectedStages != null && selectedStages.Count() > 0)
                {
                    foreach (LifeCycleStage row in selectedStages)
                    {
                        ddlModuleStep.Items.Add(new ListItem(string.Format("{0} - {1}",
                                                            Convert.ToString(row.StageStep),
                                                            Convert.ToString(row.StageTitle)),
                                                Convert.ToString(row.ID)));
                    }
                    ddlModuleStep.Items.Insert(0, new ListItem("(None)", "0"));
                    ddlModuleStep.SelectedIndex = 0;
                }
            } 
        }

        /// <summary>
        /// this will bind the fields of wiki articles.
        /// </summary>
        private void BindWikiFields()
        {
            var wikiArticlesManager = new WikiArticlesManager(HttpContext.Current.GetManagerContext());

            ListItem item = null;
            string value = "";
            string text = "";
           // string wikilist = string.empty;
            ddlField.Items.Clear();
            List<uGovernIT.FactTableField> queryTableFields = UGITModuleConstraint.GetColumnNamesWithDataType(HttpContext.Current.GetManagerContext(), DatabaseObjects.Tables.WikiArticles);
            //DataTable querytablefields = GetTableDataManager.GetTableData(DatabaseObjects.Tables.WikiArticles);
            if (queryTableFields != null)
            {
                foreach (uGovernIT.FactTableField field in queryTableFields)
                {
                    text = string.Format("{0} ({1}-{2})", field.FieldDisplayName, field.FieldName, field.DataType.Replace("System.", string.Empty));
                    value = string.Format("{0}", field.FieldDisplayName);
                    item = new ListItem(text, value);
                    item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                    ddlField.Items.Add(item);
                }
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                fieldDataJson.Value = ser.Serialize(queryTableFields);
            }
        }
        #endregion
    }
}
