using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using Newtonsoft.Json;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ProjectSimilarityControl : UserControl
    {
        public string ProjectIds { get; set; }
        private string SelectedMetricType = "";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager ModuleManager = null;
        ConfigurationVariableManager configurationVariableManager = null;
        ProjectSimilarityConfigManager projectSimilarityConfigManager = null;
        private List<ScoreColorRange> scoreColorRanges;

        protected override void OnInit(EventArgs e)
        {
            ModuleManager = new ModuleViewManager(context);
            configurationVariableManager = new ConfigurationVariableManager(context);
            projectSimilarityConfigManager = new ProjectSimilarityConfigManager(context);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strProjectComparisonMatrixColor = configurationVariableManager.GetValue(ConfigConstants.ProjectComparisonMatrixColor);
            if (!string.IsNullOrWhiteSpace(strProjectComparisonMatrixColor))
            {
                var items = strProjectComparisonMatrixColor.Split(';');
                if (items.Length > 0)
                {
                    scoreColorRanges = new List<ScoreColorRange>();
                    double startRange = 0;
                    foreach (var item in items)
                    {
                        var values = item.Split('=');

                        scoreColorRanges.Add(new ScoreColorRange { HexColor = values[0].Trim(), MinRange = startRange, MaxRange = Convert.ToDouble(values[1].Trim())});
                        startRange = Convert.ToDouble(values[1]) + 0.1;
                    }
                }
            }

            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";

            if (!string.IsNullOrEmpty(ProjectIds))
            {
                string[] arrProjectIds = ProjectIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string moduleName = uHelper.getModuleNameByTicketId(arrProjectIds[0]);
                string moduleTable = ModuleManager.GetModuleTableName(moduleName);
                //DataTable dtProjSimilarityConfig = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectSimilarityConfig, $"TenantID='{context.TenantID}'"); //SPListHelper.GetDataTable(DatabaseObjects.Tables.ProjectSimilarityConfig);

                List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == moduleName && x.Deleted == false).ToList();
                //List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.LoadDataFromProcedure(moduleName);

                List<string> metricTypes = lstProjSimilarityConfig.Select(c => c.MetricType).Distinct().ToList();

                string metricTypeQuery = Request.QueryString["metricType"];
                
                if (!string.IsNullOrWhiteSpace(metricTypeQuery))
                {
                    SelectedMetricType = metricTypeQuery;
                    lstProjSimilarityConfig = lstProjSimilarityConfig.Where(c => c.MetricType == SelectedMetricType).ToList();
                }
                else
                {
                    SelectedMetricType = metricTypes.FirstOrDefault();
                    lstProjSimilarityConfig = lstProjSimilarityConfig.Where(c => c.MetricType == SelectedMetricType).ToList();
                }
                foreach (string type in metricTypes)
                {
                    ASPxButton button = new DevExpress.Web.ASPxButton();
                    button.Width = Unit.Pixel(60);
                    button.Text = type;
                    button.Click += Button_Click;

                    if (type == SelectedMetricType)
                    {
                        button.BackColor = System.Drawing.Color.Green;
                        chkShowScore.Text = $"Show {SelectedMetricType} Scores";
                    }

                    buttonPanel.Controls.Add(button);
                }

                StringBuilder spQuery = new StringBuilder();

                spQuery.Append($"{DatabaseObjects.Columns.TicketId} in (");
                foreach (string s in arrProjectIds)
                {
                    spQuery.Append($"'{s}',");
                }
                spQuery.Remove(spQuery.Length - 1, 1);
                spQuery.Append(")");

                DataTable spListitemCollection = GetTableDataManager.GetTableData(moduleTable, $"{spQuery.ToString()} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);

                DataTable prjSimilarityData = new DataTable();
                string col = string.Empty;

                grid.Columns.Clear();

                GridViewDataTextColumn dataTextColumn = new GridViewDataTextColumn();
                dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                dataTextColumn.PropertiesTextEdit.EncodeHtml = false;
                dataTextColumn.FieldName = DatabaseObjects.Columns.TicketId;
                dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                dataTextColumn.CellStyle.CssClass = "cellheight";
                dataTextColumn.Caption = " ";
                dataTextColumn.HeaderStyle.Font.Bold = false;
                dataTextColumn.HeaderTemplate = new GridHeaderTemplate(dataTextColumn);
                dataTextColumn.FixedStyle = GridViewColumnFixedStyle.Left;
                dataTextColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                dataTextColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                dataTextColumn.Width = Unit.Pixel(160);
                grid.Columns.Add(dataTextColumn);

                if (!prjSimilarityData.Columns.Contains(DatabaseObjects.Columns.TicketId))
                {
                    prjSimilarityData.Columns.Add(DatabaseObjects.Columns.TicketId);
                }

                foreach (DataRow item in spListitemCollection.Rows)
                {
                    col = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                    if (!prjSimilarityData.Columns.Contains(col))
                    {
                        prjSimilarityData.Columns.Add(col);
                    }

                    dataTextColumn = new GridViewDataTextColumn();
                    dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    dataTextColumn.PropertiesTextEdit.EncodeHtml = false;
                    dataTextColumn.FieldName = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                    dataTextColumn.Caption = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", Convert.ToString(item[DatabaseObjects.Columns.TicketId]), Convert.ToString(item[DatabaseObjects.Columns.Title])), 50);
                    dataTextColumn.HeaderStyle.Font.Bold = false;
                    dataTextColumn.CellStyle.CssClass = "cellheight";
                    dataTextColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                    dataTextColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    dataTextColumn.Width = Unit.Pixel(60);
                    dataTextColumn.HeaderCaptionTemplate = new GridHeaderTemplate(dataTextColumn);
                    grid.Columns.Add(dataTextColumn);

                }

                var projectComparisonScores = projectSimilarityConfigManager.GetComparisonScore(
                    spListitemCollection.Rows.OfType<DataRow>().ToArray(),
                    spListitemCollection.Rows.OfType<DataRow>().ToArray(),
                    lstProjSimilarityConfig);

                foreach (var score in projectComparisonScores)
                {
                    List<object> data = new List<object>();
                    data.Add(score.PrimaryProjectTitle);
                    foreach (var item in score.SecondaryProjects)
                    {
                        data.Add(item.TotalScore);
                    }
                    prjSimilarityData.Rows.Add(data.ToArray());
                }

                grid.DataSource = prjSimilarityData;
                grid.DataBind();

                if (!IsPostBack)
                    chkShowScore.Checked = true;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (Request.Url.ToString().Contains("&metricType"))
            {
                var url = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("&metricType"));
                HttpContext.Current.Response.Redirect(url + "&metricType=" + Uri.EscapeDataString(((ASPxButton)sender).Text));
            }
            else
                HttpContext.Current.Response.Redirect(Request.Url.ToString() + "&metricType=" + Uri.EscapeDataString(((ASPxButton)sender).Text));
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.KeyValue == null)
                return;

            double score = 0;
            string colorCode = string.Empty, hexRep = string.Empty;
            if (double.TryParse(Convert.ToString(e.CellValue), out score))
            {
                string viewUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?Control=SimilarityScoreDetail&primaryProjectId={0}&secondaryProjectId={1}&Score={2}&MetricType={3}", UGITUtility.SplitString(e.KeyValue, Constants.Separator7, 0), UGITUtility.SplitString(e.DataColumn.Caption, Constants.Separator7, 0), e.CellValue, SelectedMetricType));
                e.Cell.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"Comparison Score Detail\", \"850px\", \"500px\", 0, \"{1}\");", viewUrl, Server.UrlEncode(Request.Url.AbsolutePath)));
                e.Cell.CssClass = "score score-cell";
                
                var colorRange = scoreColorRanges?.Where(x => x.MinRange <= score && x.MaxRange >= score).FirstOrDefault();
                if (colorRange != null)
                {
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(colorRange.HexColor);
                }
            }
            else if (e.CellValue == DBNull.Value)
            {
                e.Cell.CssClass = "clear-border";
            }

        }
    }

    class GridHeaderTemplate : ITemplate
    {
        GridViewDataTextColumn colID = null;
        public GridHeaderTemplate(GridViewDataTextColumn coID)
        {
            this.colID = coID;
        }

        public void InstantiateIn(Control container)
        {
            Panel pnl = new Panel();
            Label lbl = new Label();
            lbl.ID = colID.Name;
            lbl.Text = colID.Caption;

            lbl.CssClass = "label";
            pnl.Controls.Add(lbl);
            if (colID.FieldName == DatabaseObjects.Columns.TicketId)
            {
                pnl.CssClass = "firstcolheader";
            }
            else
            {
                pnl.CssClass = "rotate";
            }
            container.Controls.Add(pnl);
        }
    }

    class ScoreColorRange
    {
        public string HexColor { get; set; }
        public double MinRange { get; set; }
        public double MaxRange { get; set; }
    }
}