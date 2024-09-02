using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class HtmlEditorControl : UserControl
    {
        public delegate void SaveEventHandler(object sender, EventArgsSubmitEventHandler e);
        public event SaveEventHandler OnSubmit = delegate { };
        public ControlMode ControlMode { get; set; }
        public bool EnableHoverEdit { get; set; }
        public bool EnablePickToken { get; set; }
        public bool IsShowBaseline { get; set; }
        public string ModuleName { get; set; }
        public string PopupTitle { get; set; }
        public string ModuleStage { get; set; }
        public bool isHideInlineHtml { get; set; }
        public string Html
        {
            get
            {

                return HtmlBody.Html;
            }
            set
            {
                tdvalues.InnerHtml = value;
                HtmlBody.Html = value;
            }
        }
        public string HtmlData { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (!String.IsNullOrEmpty(PopupTitle))
            {
                pcHtmlEditor.HeaderText = PopupTitle;
            }
            else
            {
                pcHtmlEditor.HeaderText = "Body";
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ControlMode != ControlMode.Invalid)
            {
                aEditbutton.Visible = EnableHoverEdit;
            }
            if (EnablePickToken)
                HtmlBody.Toolbars[0].Items.FindByCommandName("picktoken").Visible = true;
            if (IsShowBaseline)
                aEditbutton.Visible = false;

            HtmlData = Html;

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string html = HtmlBody.Html.Replace("\r\n", string.Empty).Trim();
            html = html.Replace("<h3>", string.Empty);
            html = html.Replace("</h3>", string.Empty);
            tdvalues.InnerHtml = html;
            if (isHideInlineHtml)
            {
                tdvalues.Attributes.CssStyle.Add("display", "none");
            }
            pcHtmlEditor.ShowOnPageLoad = false;
            EventArgsSubmitEventHandler eventArg = new EventArgsSubmitEventHandler();
            eventArg.Value = html;
            //HtmlData = html;
            OnSubmit(this, eventArg);
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);
        }
        private void BindAgentDDL()
        {

            string query = string.Format(" {4}='True'  and {0}='{1}' and {2}= '{3}'", DatabaseObjects.Columns.ServiceType, Constants.ModuleAgent.Replace(Constants.Separator2, string.Empty), DatabaseObjects.Columns.ModuleNameLookup, ModuleName, DatabaseObjects.Columns.IsActivated);

            //string view = string.Format("{0},{1},{2}", DatabaseObjects.Columns.ID, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStageMultiLookup);
            DataRow[] lstItemCol = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Services, query).Select();//Select(view);

            foreach (DataRow item in lstItemCol)
            {
                string[] modulestages = uHelper.GetMultiLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ModuleStageMultiLookup]));
                if (modulestages.Length > 0)
                {
                    string modules = string.Join(Constants.Separator5, modulestages);
                    if (modules.IndexOf(ModuleStage) != -1)
                    {
                        ddlAgents.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), string.Format("[$ModuleAgent|{0}|Please click here to fill in the required data$]", Convert.ToString(item[DatabaseObjects.Columns.Title]))));
                    }
                }
            }
        }
        protected void ddlTokenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTokenType.SelectedIndex > 0)
            {
                if (ddlTokenType.SelectedValue == Constants.ModuleAgent)
                {
                    trAgents.Visible = true;
                    BindAgentDDL();
                }
                else if (ddlTokenType.SelectedValue == Constants.ModuleField)
                {
                    trModuleFields.Visible = true;
                    BindModuleFields();
                }
                else
                {
                    trAgents.Visible = false;
                    trModuleFields.Visible = false;
                }
            }
        }
        private void BindModuleFields()
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            Manager.Managers.TicketManager ticketViewManager = new Manager.Managers.TicketManager(HttpContext.Current.GetManagerContext());
            UGITModule moduleObj = moduleViewManager.LoadByName(ModuleName); //uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, ModuleName);
            if (moduleObj != null)
            {
                DataTable SPTickets = ticketViewManager.GetAllTickets(moduleObj);//SPListHelper.GetSPList(moduleObj.ModuleTicketTable);
                List<string> fields = new List<string>();
                if (SPTickets != null)
                {
                    DataTable dtModuleFields = new DataTable();
                    dtModuleFields.Columns.Add("InternalName");
                    dtModuleFields.Columns.Add("FieldName");
                    dtModuleFields.Columns.Add("FieldType");
                    foreach (DataColumn spField in SPTickets.Columns)
                    {

                        DataRow dr = dtModuleFields.NewRow();
                        dr["InternalName"] = spField.ColumnName;
                        dr["FieldName"] = string.Format("{0} ({1})", spField.ColumnName, spField.ColumnName);
                        dr["FieldType"] = spField.DataType.ToString();
                        dtModuleFields.Rows.Add(dr);

                    }

                    DataRow dr1 = dtModuleFields.NewRow();
                    dr1["InternalName"] = "Today";
                    dr1["FieldName"] = string.Format("{0} ({1})", "Today", "Today");
                    dr1["FieldType"] = typeof(DateTime).ToString();
                    dtModuleFields.Rows.Add(dr1);

                    glModuleFields.DataSource = dtModuleFields;
                    glModuleFields.DataBind();
                    glModuleFields.GridView.Selection.UnselectAll();
                }
            }
        }
        protected void aspxdayscallback_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                speDefaultDateNoofDays.MinValue = 0;
                trDays.Visible = false;
                string strField = e.Parameter;
                DataTable fieldData = glModuleFields.DataSource as DataTable;
                if (fieldData != null)
                {
                    DataRow row = fieldData.AsEnumerable().FirstOrDefault(x => x.Field<string>("InternalName") == strField);
                    if (row != null)
                    {
                        string fType = Convert.ToString(row["FieldType"]);
                        if (fType == typeof(DateTime).ToString())
                        trDays.Visible = true;
                    }
                }
            }
        }
    }
}
