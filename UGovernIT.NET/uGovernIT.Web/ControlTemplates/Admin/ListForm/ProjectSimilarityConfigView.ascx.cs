using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager.Managers;
using DevExpress.Web.Internal;

namespace uGovernIT.Web
{
    public partial class ProjectSimilarityConfigView : UserControl
    {
        string addNewItem = string.Empty;
        private string absoluteUrlEdit = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&ID={1}&moduleName={2}&selectedMetricType={3}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&module={2}&showdelete={3}&selectedModule={4}&selectedMetricType={5}";
        private string formTitle = string.Empty;
        private string viewParam = "ProjectSimilarityConfigView";
        private string newParam = "AddProjectSimilarityConfig";
        private string editParam = "AddProjectSimilarityConfig";
        private string _ModuleTable = string.Empty;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectSimilarityConfigManager projectSimilarityConfigManager = null;
        ProjectSimilarityMetricsManager projectSimilarityMetricsManager = null;

        ModuleViewManager ObjModuleViewManager = null;

        UGITModule ugitModule = null;
        protected override void OnInit(EventArgs e)
        {
            projectSimilarityConfigManager = new ProjectSimilarityConfigManager(context);
            projectSimilarityMetricsManager = new ProjectSimilarityMetricsManager(context);
            ObjModuleViewManager = new ModuleViewManager(context);

            if (!IsPostBack)
                BindData();

            formTitle = string.Format("Project Comparison");

            base.OnInit(e);
        }

        private void BindData()
        {
            List<UGITModule> ugitModule = new List<UGITModule>();
            ugitModule = ObjModuleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (ugitModule != null && ugitModule.Count > 0)
            {
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataSource = ugitModule;
                ddlModule.DataBind();
            }

            if (Request["selectedModule"] != null && Request["selectedModule"] != "")
            {
                ddlModule.SelectedValue = Request["selectedModule"];
            }
            
            object cacheVal = Context.Cache.Get(string.Format("CURRENTMODULEINDEX-{0}", context.CurrentUser.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("CURRENTMODULEINDEX-{0}", context.CurrentUser.Id));
                ddlModule.SelectedValue = Convert.ToString(cacheVal);
            }

            ddlType.Items.Clear();

            List<string> list = projectSimilarityConfigManager.Load().Where(l => l != null).Select(x => x.MetricType.Trim()).Distinct().ToList();
            if (list != null && list.Count > 0)
            {
                ddlType.DataSource = list;
                ddlType.DataBind();
            }

            if (Request["selectedMetricType"] != null && Request["selectedMetricType"] != "")
            {
                ddlType.SelectedValue = Request["selectedMetricType"];
            }

            cacheVal = Context.Cache.Get(string.Format("CURRENTSIMILARITYTYPEINDEX-{0}", context.CurrentUser.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("CURRENTSIMILARITYTYPEINDEX-{0}", context.CurrentUser.Id));
                ddlType.SelectedValue = Convert.ToString(cacheVal);
            }
        }

        private void FillColumnName()
        {
            TicketManager objTicketManager = new TicketManager(context);
            ugitModule = ObjModuleViewManager.GetByName(ddlModule.SelectedValue);

            if (ugitModule != null)
                _ModuleTable = ugitModule.ModuleTable;
            else
                _ModuleTable = ddlModule.SelectedValue;

            DataTable dt = objTicketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, _ModuleTable);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = DatabaseObjects.Columns.ColumnNameSchema + " ASC";
                glFieldName.DataSource = dt;
                glFieldName.DataBind();
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            FillColumnName();

            

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, 0, ddlModule.SelectedValue, ddlType.SelectedValue));
            string aAddItem_TopURL = (string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", addNewItem, "/Layouts/uGovernIT/uGovernITConfiguration/", formTitle));
            aAddItem_Top.ClientSideEvents.Click = "function(){ " + aAddItem_TopURL + " }";

            if (!Page.IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    dxshowdeleted.Checked = showdelete == "0" ? false : true;
                }
            }
            BindGridView(ddlModule.SelectedValue, ddlType.SelectedValue);

            if (!Page.IsPostBack)
                BindMetrics(ddlModule.SelectedValue);

            base.OnLoad(e);
        }

        private void BindGridView(string SelectedModule, string SelectedType)
        {
            List<ProjectSimilarityConfig> list = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == SelectedModule).ToList();
            if (string.IsNullOrWhiteSpace(SelectedType))
            {
                List<string> types = list.Select(l => l.MetricType).ToList();
                string type = types.Count > 0 ? types.First() : "";
                list = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == SelectedModule && x.MetricType == type).ToList();
                ddlType.SelectedValue = type;
            }
            else
            {
                list = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == SelectedModule && x.MetricType == ddlType.SelectedValue).ToList();
            }
            if (list != null)
            {
                if (!dxshowdeleted.Checked)
                {
                    list = list.Where(x => !x.Deleted).ToList();
                }
                dxgridProjectSimilarity.DataSource = list;
                dxgridProjectSimilarity.DataBind();
            }
        }

        private void BindMetrics(string SelectedModule)
        {
            var metrics = projectSimilarityMetricsManager.Load(x => x.ModuleNameLookup == SelectedModule).FirstOrDefault();
            if (metrics != null)
                glFieldName.Text = metrics.SearchColumns;
            else
                glFieldName.Text = "";
        }

        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxshowdeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, "", showdelete, ddlModule.SelectedValue, ddlType.SelectedValue));
            Response.Redirect(url);

        }
        protected void dxgridProjectSimilarity_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue("Title"));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, dataKeyValue, ddlModule.SelectedValue, ddlType.SelectedValue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','600',0,'{2}','true')", editItem, title, "/Layouts/uGovernIT/uGovernITConfiguration/", this.formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxgridProjectSimilarity.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void dxgridProjectSimilarity_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGridView(ddlModule.SelectedValue, ddlType.SelectedValue);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillColumnName();
            BindMetrics(ddlModule.SelectedValue);
            //addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.newParam, 0, ddlModule.SelectedValue));
            //BindGridView(ddlModule.SelectedValue);
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ProjectSimilarityMetrics metrics = projectSimilarityMetricsManager.Load(x => x.ModuleNameLookup == ddlModule.SelectedValue).FirstOrDefault();

            if (metrics == null)
                metrics = new ProjectSimilarityMetrics();

            metrics.ModuleNameLookup = ddlModule.SelectedValue;
            metrics.SearchColumns = glFieldName.Text;
            metrics.Deleted = false;
            projectSimilarityMetricsManager.Update(metrics);
        }
    }
}
