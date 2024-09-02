using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class AddProjectSimilarityConfig : UserControl
    {
        public long Id { get; set; }
        public string moduleName { get; set; }
        public string selectedMetricType { get; set; }

        private string _ModuleTable = string.Empty;

        UGITModule ugitModule = null;
        ProjectSimilarityConfig spitem;
        ProjectSimilarityConfigManager objProjectSimilarityConfigManager = new ProjectSimilarityConfigManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        ProjectSimilarityConfigManager projectSimilarityConfigManager = new ProjectSimilarityConfigManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
                BindModule(moduleName, selectedMetricType);

            if (Convert.ToInt64(Request.QueryString["ID"]) > 0 && Request.QueryString["ID"] != null)
            {
                Id = Convert.ToInt64(Request.QueryString["ID"]);
                spitem = objProjectSimilarityConfigManager.LoadByID(Id);
                if (spitem != null)
                {
                    txtTitle.Text = spitem.Title;
                    ddlMetricType.SelectedValue = spitem.MetricType;
                    cmbFieldName.Text = spitem.ColumnName;
                    ddlColumnType.SelectedValue = spitem.ColumnType;
                    txtStageWeight.Text = Convert.ToString(spitem.StageWeight);
                    txtWeight.Text = Convert.ToString(spitem.Weight);
                    ddlModule.SelectedValue = spitem.ModuleNameLookup;
                    chkDeleted.Checked = spitem.Deleted;
                }
                
            }
            else
            {
                spitem = new ProjectSimilarityConfig();
                txtTitle.Text = spitem.Title;
                ddlMetricType.SelectedValue = spitem.MetricType;
                cmbFieldName.Text = spitem.ColumnName;
                ddlColumnType.SelectedIndex = 0;
                txtStageWeight.Text = Convert.ToString(spitem.StageWeight);
                txtWeight.Text = Convert.ToString(spitem.Weight);
                lnkDelete.Visible = false;
            }
        }

        private void FillColumnName()
        {
            TicketManager objTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ugitModule = ObjModuleManager.GetByName(ddlModule.SelectedValue);

            if (ugitModule != null)
                _ModuleTable = ugitModule.ModuleTable;
            else
                _ModuleTable = moduleName;

            DataTable dt = objTicketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, _ModuleTable);
            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = DatabaseObjects.Columns.ColumnNameSchema + " ASC";
                cmbFieldName.TextField = DatabaseObjects.Columns.ColumnNameSchema;
                cmbFieldName.ValueField = DatabaseObjects.Columns.ColumnNameSchema;
                cmbFieldName.DataSource = dt;
                cmbFieldName.DataBind();
            }
        }

        private void BindModule(string moduleName, string selectedMetricType)
        {
            List<UGITModule> ugitModule = new List<UGITModule>();
            ugitModule = ObjModuleManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (ugitModule != null && ugitModule.Count > 0)
            {
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataSource = ugitModule;
                ddlModule.DataBind();

                if (!string.IsNullOrEmpty(moduleName))
                {
                    ddlModule.SelectedValue = moduleName;
                }
            }
            List<string> list = projectSimilarityConfigManager.Load().Where(l => l != null).Select(x => x.MetricType.Trim()).Distinct().ToList();
            if (list != null && list.Count > 0)
            {
                //ddlType.DataTextField = DatabaseObjects.Columns.Title;
                //ddlType.DataValueField = DatabaseObjects.Columns.ModuleName;
                list.Insert(0, "");
                ddlMetricType.DataSource = list;
                ddlMetricType.DataBind();

                if (!string.IsNullOrEmpty(selectedMetricType))
                {
                    ddlMetricType.SelectedValue = selectedMetricType;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillColumnName();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            if (Validate())
            {
                spitem.Title = txtTitle.Text;
                spitem.ColumnName = cmbFieldName.Text;
                spitem.ColumnType = ddlColumnType.SelectedValue;
                spitem.MetricType = ddlMetricType.SelectedValue == "" ? txtMetricType.Text : ddlMetricType.SelectedValue;
                spitem.MetricType = spitem.MetricType.Trim();
                spitem.StageWeight = Convert.ToInt32(txtStageWeight.Text);
                spitem.Weight = Convert.ToInt32(txtWeight.Text);
                spitem.ModuleNameLookup = ddlModule.SelectedValue;
                spitem.Deleted = chkDeleted.Checked;
                objProjectSimilarityConfigManager.Save(spitem);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated Column Name: {spitem.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

                Context.Cache.Add(string.Format("CURRENTMODULEINDEX-{0}", Context.CurrentUser().Id), ddlModule.SelectedValue, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                Context.Cache.Add(string.Format("CURRENTSIMILARITYTYPEINDEX-{0}", Context.CurrentUser().Id), selectedMetricType, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        private Boolean Validate()
        {
            if (ddlMetricType.SelectedIndex < 1 && (txtMetricType.Text == null || string.IsNullOrWhiteSpace(txtMetricType.Text)))
            {
                return false;
            }
            string metricValue = ddlMetricType.SelectedValue == "" ? txtMetricType.Text : ddlMetricType.SelectedValue;
            List<ProjectSimilarityConfig> collection = objProjectSimilarityConfigManager.Load(x => x.ID != Id && x.ColumnName == cmbFieldName.Text 
            && x.ModuleNameLookup == ddlModule.SelectedValue && x.MetricType == metricValue.Trim());
            if (collection.Count > 0)
            {
                lblErrorMessage.Text = "Column Name is already in the list";
                return false;
            }
            else
            { return true; }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            spitem = objProjectSimilarityConfigManager.LoadByID(Id);
            spitem.Deleted = true;
            objProjectSimilarityConfigManager.Update(spitem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Column Name: {spitem.ColumnName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillColumnName();
        }

        protected void csvdivMetricType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool argsval = false;
            if (ddlMetricType.SelectedIndex < 1 && txtMetricType.Text == "")
                argsval = false;
            else
                argsval = true;

            args.IsValid = argsval;
        }
    }
}
