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
using System.Threading;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.DAL;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json;

namespace uGovernIT.Manager
{

    public class UserProjectExperienceManager : ManagerBase<UserProjectExperience>, IUserProjectExperienceManager
    {
        ApplicationContext _context = null;
        ModuleViewManager _moduleViewManager = null;
        TicketManager _ticketManager = null;
        UserProfileManager _userProfileManager = null;
        ProjectEstimatedAllocationManager projectEstimatedAllocationManager = null;
        public UserProjectExperienceManager(ApplicationContext context) : base(context)
        {
            store = new UserProjectExperienceStore(this.dbContext);
            _context = context;
            _moduleViewManager = new ModuleViewManager(_context);
            _ticketManager = new TicketManager(_context);
            _userProfileManager = new UserProfileManager(context);
            projectEstimatedAllocationManager = new ProjectEstimatedAllocationManager(context);
        }
        public long Save(UserProjectExperience userProjectExperiences)
        {
            if (userProjectExperiences.ID > 0)
                this.Update(userProjectExperiences);
            else
                this.Insert(userProjectExperiences);
            return userProjectExperiences.ID;
        }

        public List<ProjectTag> GetProjectExperienceTags(string projectId, bool includeCertification)
        {
            List<ProjectTag> projectTags = new List<ProjectTag>();
            string modulename = uHelper.getModuleNameByTicketId(projectId);
            UGITModule opmModuleObj = _moduleViewManager.GetByName(modulename);
            DataRow row = _ticketManager.GetByTicketID(opmModuleObj, projectId);
            if (UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.TagMultiLookup)
                && !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TagMultiLookup])))
            {
                try
                {
                    projectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TagMultiLookup]));
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TagMultiLookup])}.");
                }

                if (!includeCertification && projectTags?.Count > 0)
                {
                    projectTags = projectTags.Where(x => x.Type == TagType.Experience).ToList();
                }
            }

            return projectTags;
        }

        public void UpdateUserProjectTagExperience(List<string> tags, string ticketId, string userId = "")
        {
            List<UserProfile> userProfiles = _userProfileManager.GetUsersProfile();

            if (!string.IsNullOrWhiteSpace(ticketId))
            {
                List<UserProjectExperience> userProjectExperience = this.Load(x => x.ProjectID == ticketId /*&& !tags.Contains(x.TagLookup.ToString())*/)?.ToList() ?? null;
                if (userProjectExperience != null && userProjectExperience.Count > 0)
                {
                    this.Delete(userProjectExperience);
                }
                if (tags != null && tags.Count > 0)
                {
                    List<ProjectEstimatedAllocation> projectAllocations = projectEstimatedAllocationManager.Load(x => x.TicketId == ticketId && x.AssignedTo != "00000000-0000-0000-0000-000000000000")?.GroupBy(x => x.AssignedTo)?.Select(x => x.First())?.ToList() ?? null;
                    if (projectAllocations != null && projectAllocations.Count > 0)
                    {
                        projectAllocations.ForEach(o =>
                        {
                            tags.ForEach(x =>
                            {
                                this.Save(new UserProjectExperience
                                {
                                    ProjectID = o.TicketId,
                                    TagLookup = UGITUtility.StringToInt(x),
                                    UserId = o.AssignedTo,
                                    ResourceUser = userProfiles.FirstOrDefault(y => y.Id == o.AssignedTo)?.Name ?? string.Empty,
                                    Deleted = false
                                });
                            });
                        });
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                List<UserProjectExperience> userProjectExperiences = this.Load(x => x.UserId == userId && string.IsNullOrWhiteSpace(x.ProjectID));
                if (userProjectExperiences != null && userProjectExperiences.Count > 0)
                {
                    this.Delete(userProjectExperiences);
                }
                if (tags != null && tags.Count > 0)
                {
                    tags.ForEach(x =>
                    {
                        this.Save(new UserProjectExperience
                        {
                            ProjectID = null,
                            TagLookup = UGITUtility.StringToInt(x),
                            UserId = userId,
                            ResourceUser = userProfiles.FirstOrDefault(y => y.Id == userId)?.Name ?? string.Empty,
                            Deleted = false,
                        });
                    });
                }
            }
        }

        public List<string> GetProjectTagsToUpdateForUserTags(bool toUpdateTags, UserProfile profile)
        {
            List<string> commonTags = new List<string>();

            var userTags = this.Load(x => x.UserId == profile.Id);
            if (userTags == null)
                return commonTags;

            var userExperienceTags = userTags.Where(x => string.IsNullOrWhiteSpace(x.ProjectID)).Select(t => t.TagLookup.ToString());
            var userProjectExperienceTags = userTags.Where(x => !string.IsNullOrWhiteSpace(x.ProjectID)).Select(t => t.TagLookup.ToString());

            commonTags = userProjectExperienceTags.Distinct().ToList();
            if (toUpdateTags)
                commonTags = userExperienceTags.Union(userProjectExperienceTags).ToList();

            return commonTags;
        }
    }

    public interface IUserProjectExperienceManager : IManagerBase<UserProjectExperience>
    {

    }
}
