using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IProjectSimilarityConfigManager : IManagerBase<ProjectSimilarityConfig>
    {
    }

    public class ProjectSimilarityConfigManager : ManagerBase<ProjectSimilarityConfig>, IProjectSimilarityConfigManager
    {
        private ConfigurationVariableManager _configurationVariableManager;

        public ProjectSimilarityConfigManager(ApplicationContext context) : base(context)
        {
            store = new ProjectSimilarityConfigStore(this.dbContext);
            _configurationVariableManager = new ConfigurationVariableManager(context);
        }

        public long Save(ProjectSimilarityConfig ProjectSimilarityConfig_)
        {
            if (ProjectSimilarityConfig_.ID > 0)
                this.Update(ProjectSimilarityConfig_);
            else
                this.Insert(ProjectSimilarityConfig_);
            return ProjectSimilarityConfig_.ID;
        }

        public List<ProjectSimilarityConfig> LoadDataFromProcedure(string moduleName)
        {
            List<ProjectSimilarityConfig> resultList = new List<ProjectSimilarityConfig>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@ModuleName", moduleName);
            values.Add("@TenantID", this.dbContext.TenantID);
            DataTable dtResult = GetTableDataManager.GetData("ProjectSimilarityScore", values);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    ProjectSimilarityConfig projectSimilarityConfigObj = new ProjectSimilarityConfig();
                    projectSimilarityConfigObj.Title = UGITUtility.ObjectToString(row["Title"]);
                    projectSimilarityConfigObj.ColumnName = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ColumnName]);
                    projectSimilarityConfigObj.ColumnType = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ColumnType]);
                    projectSimilarityConfigObj.StageWeight = UGITUtility.StringToInt(row[DatabaseObjects.Columns.StageWeight]);
                    projectSimilarityConfigObj.Weight = UGITUtility.StringToInt(row[DatabaseObjects.Columns.Weight]);
                    projectSimilarityConfigObj.NormalizeWeight = UGITUtility.StringToDouble(row["NormalizeWeight"]);
                    projectSimilarityConfigObj.NormalizedScore = UGITUtility.StringToDouble(row["NormalizedScore"]);
                    projectSimilarityConfigObj.WeightedScore = UGITUtility.StringToDouble(row["WeightedScore"]);
                    projectSimilarityConfigObj.MetricType = UGITUtility.ObjectToString(row["MetricType"]);
                    resultList.Add(projectSimilarityConfigObj);
                }
            }
            return resultList;
        }

        public List<ProjectComparisonScore> GetComparisonScore(DataRow[] primaryProjects, DataRow[] secondaryProjects, List<ProjectSimilarityConfig> projectSimilarityFields, bool includeFieldScore = false)
        {
            var comparisonScoreList = new List<ProjectComparisonScore>();

            if (projectSimilarityFields == null || projectSimilarityFields.Count == 0)
                return comparisonScoreList;

            string defaultNormalizedScoreValue = _configurationVariableManager.GetValue(ConfigConstants.DefaultNormalizedScore);
            if (!int.TryParse(defaultNormalizedScoreValue, out int defaultNormalizedScore))
                defaultNormalizedScore = 100;

            int totalScore = projectSimilarityFields.Sum((x) => x.StageWeight * x.Weight);

            try
            {
                List<ProjectTag> matchingTags = new List<ProjectTag>();
                List<ProjectTag> primaryProjectTags = new List<ProjectTag>();
                List<ProjectTag> secondaryProjectTags = new List<ProjectTag>();

                foreach (DataRow drPrimaryProject in primaryProjects)
                {
                    ProjectComparisonScore projectComparisonScore = new ProjectComparisonScore()
                    {
                        PrimaryProjectTitle = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", drPrimaryProject[DatabaseObjects.Columns.TicketId], drPrimaryProject[DatabaseObjects.Columns.Title]), 50),
                        PrimaryProjectTicketID = UGITUtility.ObjectToString(drPrimaryProject[DatabaseObjects.Columns.TicketId])
                    };

                    foreach (DataRow drSecondaryProject in secondaryProjects)
                    {
                        ProjectComparisonDataItem projectComparisonDataItem = new ProjectComparisonDataItem()
                        {
                            SecondaryProjectTitle = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", drSecondaryProject[DatabaseObjects.Columns.TicketId], drSecondaryProject[DatabaseObjects.Columns.Title]), 50),
                            SecondaryProjectTicketID = UGITUtility.ObjectToString(drSecondaryProject[DatabaseObjects.Columns.TicketId])
                        };

                        if (Convert.ToString(drSecondaryProject[DatabaseObjects.Columns.TicketId]) == Convert.ToString(drPrimaryProject[DatabaseObjects.Columns.TicketId]))
                        {
                            projectComparisonDataItem.TotalScore = null;
                            projectComparisonScore.SecondaryProjects.Add(projectComparisonDataItem);
                            continue;
                        }

                        if (!includeFieldScore)
                        {
                            var item = comparisonScoreList
                                                .Where(x => x.PrimaryProjectTicketID == projectComparisonDataItem.SecondaryProjectTicketID)
                                                .SelectMany(x => x.SecondaryProjects
                                                    .Where(y => y.SecondaryProjectTicketID == projectComparisonScore.PrimaryProjectTicketID));

                            if (item != null && item.Count() > 0)
                            {
                                projectComparisonDataItem.TotalScore = item.FirstOrDefault()?.TotalScore;
                                projectComparisonScore.SecondaryProjects.Add(projectComparisonDataItem);
                                continue;
                            } 
                        }

                        double score = 0.0;

                        foreach (var dr in projectSimilarityFields.OrderBy(f => f.Title))
                        {
                            var projectComparisonField = new ProjectComparisonField() { Score = 0 };
                            string column = Convert.ToString(dr.ColumnName);
                            double normalizedScore = Convert.ToDouble(((dr.StageWeight * dr.Weight) / Convert.ToDouble(totalScore)) * defaultNormalizedScore);

                            //if (item.Fields.ContainsField(column))
                            if (UGITUtility.IfColumnExists(drPrimaryProject, column))
                            {
                                //Match string value
                                if (Convert.ToString(dr.ColumnType) == "MatchValue")
                                {
                                    string val1 = UGITUtility.ObjectToString(drPrimaryProject[column]);
                                    string val2 = UGITUtility.ObjectToString(drSecondaryProject[column]);
                                    if (!string.IsNullOrWhiteSpace(val1) && !string.IsNullOrWhiteSpace(val2)
                                        && val1.Trim() == val2.Trim())
                                    {
                                        score += Math.Round(normalizedScore, 1, MidpointRounding.AwayFromZero);
                                        projectComparisonField.Score = Math.Round(normalizedScore, 1, MidpointRounding.AwayFromZero);
                                    }
                                }
                                //Take ratio of numeric values
                                else if (Convert.ToString(dr.ColumnType) == "Ratio")
                                {
                                    double num1 = UGITUtility.StringToDouble(drSecondaryProject[column]);
                                    double num2 = UGITUtility.StringToDouble(drPrimaryProject[column]);
                                    double ratio = 0;

                                    if (num1 == 0 || num2 == 0)
                                    {
                                        ratio = 0;
                                    }
                                    else
                                    {
                                        if (num1 >= num2)
                                        {
                                            ratio = Convert.ToDouble(num2 / num1);
                                        }
                                        else if (num1 < num2)
                                        {
                                            ratio = Convert.ToDouble(num1 / num2);
                                        }
                                    }

                                    var roundOffScore = Math.Round(ratio * normalizedScore, 1, MidpointRounding.AwayFromZero);
                                    score += Math.Round(roundOffScore, 1, MidpointRounding.AwayFromZero);
                                    projectComparisonField.Score = roundOffScore;
                                }
                                else if (Convert.ToString(dr.ColumnType) == "MatchList")
                                {
                                    int tagCountSecItem = 0;
                                    int tagCountItem = 0;
                                    double denominator = 0;

                                    matchingTags = new List<ProjectTag>();
                                    primaryProjectTags = new List<ProjectTag>();
                                    secondaryProjectTags = new List<ProjectTag>();

                                    try
                                    {
                                        string secondaryProjectTagsStr = UGITUtility.ObjectToString(drSecondaryProject[DatabaseObjects.Columns.TagMultiLookup]);
                                        if (!string.IsNullOrEmpty(secondaryProjectTagsStr))
                                            secondaryProjectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(secondaryProjectTagsStr);
                                        string primaryProjectTagsStr = UGITUtility.ObjectToString(drPrimaryProject[DatabaseObjects.Columns.TagMultiLookup]);
                                        if (!string.IsNullOrEmpty(primaryProjectTagsStr))
                                            primaryProjectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(primaryProjectTagsStr);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {UGITUtility.ObjectToString(drSecondaryProject[DatabaseObjects.Columns.TagMultiLookup])}.");
                                        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {UGITUtility.ObjectToString(drPrimaryProject[DatabaseObjects.Columns.TagMultiLookup])}.");
                                    }

                                    if (secondaryProjectTags != null && secondaryProjectTags.Count > 0)
                                        tagCountSecItem = secondaryProjectTags.Count;

                                    if (primaryProjectTags != null && primaryProjectTags.Count > 0)
                                        tagCountItem = primaryProjectTags.Count;
                                    if (tagCountSecItem > 0 && tagCountItem > 0)
                                    {
                                        if (tagCountSecItem >= tagCountItem)
                                        {
                                            denominator = tagCountSecItem;
                                            matchingTags = secondaryProjectTags.Where(x => x.Type == TagType.Experience && primaryProjectTags.Any(y => y.TagId == x.TagId && y.Type == TagType.Experience)).ToList();
                                        }
                                        else
                                        {
                                            denominator = tagCountItem;
                                            matchingTags = primaryProjectTags.Where(x => x.Type == TagType.Experience && secondaryProjectTags.Any(y => y.TagId == x.TagId && y.Type == TagType.Experience)).ToList();
                                        }
                                    }
                                    if (matchingTags != null && matchingTags.Count > 0)
                                    {
                                        var roundOffScore = Math.Round(Convert.ToDouble(matchingTags.Count / denominator) * normalizedScore, 1, MidpointRounding.AwayFromZero);
                                        score += Math.Round(roundOffScore, 1, MidpointRounding.AwayFromZero);
                                        projectComparisonField.Score = roundOffScore;
                                    }
                                }
                                projectComparisonField.Name = column;
                                projectComparisonField.Title = dr.Title;
                                projectComparisonDataItem.Fields.Add(projectComparisonField);
                            }
                        }
                        projectComparisonDataItem.TotalScore = Math.Round(score, 1, MidpointRounding.AwayFromZero);
                        projectComparisonScore.SecondaryProjects.Add(projectComparisonDataItem);
                    }
                    comparisonScoreList.Add(projectComparisonScore);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "GetComparisonScore - Failed to compare the projects.");
            }
            return comparisonScoreList;
        }
    }
}
