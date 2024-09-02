using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.RMM.ViewModel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.Extractor;
using FuzzySharp;
using FuzzySharp.SimilarityRatio;

namespace uGovernIT.Manager
{
    
    public class ExperiencedTagManager : ManagerBase<ExperiencedTag>, IExperiencedTagManager
    {
        public ExperiencedTagManager(ApplicationContext context) : base(context)
        {
            store = new ExperiencedTagStore(this.dbContext);
        }

        public long Save(ExperiencedTag experiencedTags)
        {
            if (experiencedTags.ID > 0)
                this.Update(experiencedTags);
            else
                this.Insert(experiencedTags);
            return experiencedTags.ID;
        }

        public List<ProjectTag> GetMatchingExperienceTags(List<ExperiencedTag> tags, DataRow projectDataRow, bool tagsToUpdate)
        {
            List<ExperienceTaggedProjects> hardMatches = new List<ExperienceTaggedProjects>();
            List<ExperienceTaggedProjects> score100_weightage = new List<ExperienceTaggedProjects>();
            List<ExperienceTaggedProjects> score100_token = new List<ExperienceTaggedProjects>();
            List<ProjectTag> existingTags = new List<ProjectTag>();

            string ticketID = Convert.ToString(projectDataRow[DatabaseObjects.Columns.TicketId]);
            string description = Convert.ToString(projectDataRow[DatabaseObjects.Columns.Description]);
            string comment = Convert.ToString(projectDataRow[DatabaseObjects.Columns.Comment]);

            List<string> choices = new List<string>();
            if (!string.IsNullOrWhiteSpace(description))
                choices.Add(description.ToLower().Trim());

            if (!string.IsNullOrWhiteSpace(comment))
                choices.Add(comment.ToLower().Trim());

            if (tagsToUpdate)
            {
                var tagStr = Convert.ToString(projectDataRow[DatabaseObjects.Columns.TagMultiLookup]);
                if (!string.IsNullOrWhiteSpace(tagStr))
                {
                    try
                    {
                        existingTags = JsonConvert.DeserializeObject<List<ProjectTag>>(tagStr);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {tagStr}.");
                    }
                }
            }

            ULog.WriteLog($" -- Processing project - {ticketID} - Scanning {tags.Count} Experience tags.");

            foreach (ExperiencedTag tag in tags.Distinct())
            {
                if (string.IsNullOrEmpty(tag.Title))
                    continue;

                if (tagsToUpdate && existingTags != null && existingTags.Any(e => e.TagId == tag.ID.ToString()))
                {
                    hardMatches.Add(new ExperienceTaggedProjects
                    {
                        TagID = tag.ID,
                        TagCategory = tag.Category,
                        TagName = tag.Title,
                        MatchedScore = "100",
                        TicketID = ticketID,
                    });
                }
                else
                {
                    //Hard Match
                    var tagName = tag.Title?.ToLower().Trim();
                    Regex re = new Regex($@"\b{Regex.Escape(tagName)}\b", RegexOptions.IgnoreCase);

                    if (choices.Any(d => re.IsMatch(d)))
                    {
                        hardMatches.Add(new ExperienceTaggedProjects
                        {
                            TagID = tag.ID,
                            TagCategory = tag.Category,
                            TagName = tag.Title,
                            MatchedScore = "100",
                            TicketID = ticketID,
                        });
                    }

                    //Score 100
                    var matchedRecord = ProcessScore(tag, ticketID, choices.ToArray(), 100, typeof(WeightedRatioScorer));
                    if (matchedRecord != null)
                        score100_weightage.Add(matchedRecord);

                    //Score 100
                    matchedRecord = ProcessScore(tag, ticketID, choices.ToArray(), 100, typeof(TokenSetScorer));
                    if (matchedRecord != null)
                        score100_token.Add(matchedRecord);
                }
            }

            List<ProjectTag> commonTags = hardMatches.Union(score100_weightage).Union(score100_token).Select(t =>
                new ProjectTag
                {
                    TagId = t.TagID.ToString(),
                    IsMandatory = true,
                    MinValue = 1,
                    Type = TagType.Experience
                }).ToList();
            return commonTags;
        }

        private ExperienceTaggedProjects ProcessScore(ExperiencedTag tag, string ticketID, string[] choices, int score, Type algorithm)
        {
            ExtractedResult<string> matched = null;

            if (algorithm == typeof(WeightedRatioScorer))
            {
                matched = Process.ExtractOne(tag.Title.ToLower().Trim(), choices, s => s, ScorerCache.Get<WeightedRatioScorer>(), score);
            }
            else if (algorithm == typeof(TokenSetScorer))
            {
                matched = Process.ExtractOne(tag.Title.ToLower().Trim(), choices, s => s, ScorerCache.Get<TokenSetScorer>(), score);
            }

            if (matched != null)
            {
                return new ExperienceTaggedProjects
                {
                    TagID = tag.ID,
                    TagCategory = tag.Category,
                    TagName = tag.Title,
                    MatchedScore = matched.Score.ToString(),
                    TicketID = ticketID,
                };
            }
            else
                return null;
        }

        
    }
    public interface IExperiencedTagManager : IManagerBase<ExperiencedTag>
    {

    }
}
