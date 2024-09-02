using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Text.RegularExpressions;
using System.Linq;
using uGovernIT.Manager.RMM.ViewModel;
using Newtonsoft.Json;
using uGovernIT.Util.Log;
using System.Web.UI.DataVisualization.Charting;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class SimilarityScoreDetail : UserControl
    {
        public string primaryProjectId { get; set; }
        public string secondaryProjectId { get; set; }
        public string similarityScore { get; set; }
        public string MetricType { get; set; }
        public string primaryProjectTitle { get; set; }
        public string secondaryProjectTitle { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager ModuleManager = null;
        FieldConfigurationManager configFieldManager = null;
        ProjectSimilarityConfigManager projectSimilarityConfigManager = null;
        ExperiencedTagManager experiencedTagManager = null;
        protected override void OnInit(EventArgs e)
        {
            ModuleManager = new ModuleViewManager(context);
            configFieldManager = new FieldConfigurationManager(context);
            projectSimilarityConfigManager = new ProjectSimilarityConfigManager(context);
            experiencedTagManager = new ExperiencedTagManager(context);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string module = uHelper.getModuleNameByTicketId(primaryProjectId);

            DataRow primaryProject = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(primaryProjectId), primaryProjectId);
            primaryProjectTitle = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", primaryProject[DatabaseObjects.Columns.TicketId], primaryProject[DatabaseObjects.Columns.Title]), 35);
            DataRow secondaryProject = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(secondaryProjectId), secondaryProjectId);
            secondaryProjectTitle = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", secondaryProject[DatabaseObjects.Columns.TicketId], secondaryProject[DatabaseObjects.Columns.Title]), 35);

            //DataTable dtProjSimilarityConfig = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectSimilarityConfig, $"TenantID='{context.TenantID}'"); //SPListHelper.GetDataTable(DatabaseObjects.Tables.ProjectSimilarityConfig);
            List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == module && x.Deleted == false && x.MetricType == MetricType).ToList();

            UGITModule ugitModule = ModuleManager.LoadByName(uHelper.getModuleNameByTicketId(primaryProjectId)); //uGITCache.ModuleConfigCache.GetCachedModule(context, uHelper.getModuleNameByTicketId(primaryProjectId));

            DataTable prjSimilarityData = new DataTable();
            if (!prjSimilarityData.Columns.Contains(DatabaseObjects.Columns.FieldName))
            {
                prjSimilarityData.Columns.Add(DatabaseObjects.Columns.FieldName);
            }

            string pFieldValue = "pFieldValue";
            if (!prjSimilarityData.Columns.Contains(pFieldValue))
                prjSimilarityData.Columns.Add(pFieldValue).Caption = primaryProjectTitle;

            string sFieldValue = "sFieldValue";
            if (!prjSimilarityData.Columns.Contains(sFieldValue))
                prjSimilarityData.Columns.Add(sFieldValue).Caption = secondaryProjectTitle;

            string Score = "Score";
            if (!prjSimilarityData.Columns.Contains(Score))
                prjSimilarityData.Columns.Add(Score);

            var projectComparisonScores = projectSimilarityConfigManager.GetComparisonScore(
                new DataRow[] { primaryProject },
                new DataRow[] { secondaryProject },
                lstProjSimilarityConfig,
                true);

            var projectComparisonScore = projectComparisonScores.Where(x =>
                x.PrimaryProjectTicketID == UGITUtility.ObjectToString(primaryProject[DatabaseObjects.Columns.TicketId])
                && x.SecondaryProjects.Any(y => y.SecondaryProjectTicketID == UGITUtility.ObjectToString(secondaryProject[DatabaseObjects.Columns.TicketId]) && y.TotalScore != null))
                .FirstOrDefault();

            if (projectComparisonScore != null)
            {
                if (lstProjSimilarityConfig != null && lstProjSimilarityConfig.Count > 0)
                {
                    var fields = projectComparisonScore.SecondaryProjects[0].Fields;

                    foreach (var dr in lstProjSimilarityConfig.OrderBy(x => x.Title))
                    {
                        double score = 0.0;
                        string factor = string.Empty;

                        string column = Convert.ToString(dr.ColumnName);
                        string value1 = string.Empty;
                        string value2 = string.Empty;

                        if (UGITUtility.IfColumnExists(primaryProject, column))
                        {
                            FieldConfiguration spField = configFieldManager.GetFieldByFieldName(column);
                            var columnField = fields.Where(x => x.Name == column).FirstOrDefault();
                            if (columnField != null)
                            {
                                score = columnField.Score;
                                // Now using the Titles from Project Similarity Configuration
                                factor = columnField.Title;
                            }
                            if (spField != null)
                            {
                                FieldType fType = FieldType.None;
                                Enum.TryParse(spField.Datatype, out fType);

                                value1 = Convert.ToString(primaryProject[column]);
                                if (fType == FieldType.Lookup && !string.IsNullOrEmpty(value1))
                                    value1 = configFieldManager.GetFieldConfigurationData(spField.FieldName, Convert.ToString(primaryProject[column]));
                                //if (spField.Type == SPFieldType.Lookup && !string.IsNullOrEmpty(value1))
                                //value1 = uHelper.GetLookupValue(Convert.ToString(primaryProject[column]));

                                value2 = Convert.ToString(secondaryProject[column]);

                                if (fType == FieldType.Lookup && !string.IsNullOrEmpty(value2))
                                    value2 = configFieldManager.GetFieldConfigurationData(spField.FieldName, Convert.ToString(secondaryProject[column]));
                            }
                            else
                            {
                                value1 = Convert.ToString(primaryProject[column]);
                                value2 = Convert.ToString(secondaryProject[column]);
                            }

                            if (column == DatabaseObjects.Columns.TagMultiLookup)
                            {
                                List<ProjectTag> primaryProjectTags = new List<ProjectTag>();
                                List<ProjectTag> secondaryProjectTags = new List<ProjectTag>();

                                if (!string.IsNullOrWhiteSpace(value1))
                                {
                                    try
                                    {
                                        primaryProjectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(value1);
                                        List<ExperiencedTag> primaryProjectExperiencedTags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID && primaryProjectTags.Any(y => Convert.ToInt64(y.TagId) == x.ID));
                                        value1 = String.Join(", ", primaryProjectExperiencedTags.OrderBy(p => p.Title).Select(p => p.Title).ToList());
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {value1}.");
                                        value1 = string.Empty;
                                    }
                                }
                                    
                                if (!string.IsNullOrWhiteSpace(value2))
                                {
                                    try
                                    {
                                        secondaryProjectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(value2);
                                        List<ExperiencedTag> secondaryProjectExperiencedTags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID && secondaryProjectTags.Any(y => Convert.ToInt64(y.TagId) == x.ID));
                                        value2 = String.Join(", ", secondaryProjectExperiencedTags.OrderBy(p => p.Title).Select(p => p.Title).ToList());
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {value2}.");
                                        value2 = string.Empty;
                                    }
                                }
                            }

                            prjSimilarityData.Rows.Add(new object[] { factor, value1, value2, score });
                        }
                    }
                }
            }

            dxgridProjectSimilarity.DataSource = prjSimilarityData;
            dxgridProjectSimilarity.DataBind();
        }

        protected void dxgridProjectSimilarity_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (object.Equals(e.Item, dxgridProjectSimilarity.TotalSummary["FieldName"]))
            {
                e.TotalValue = "Total";
            }
            if (object.Equals(e.Item, dxgridProjectSimilarity.TotalSummary["Score"]))
            {
                e.TotalValue = similarityScore;
            }
        }
    }
}