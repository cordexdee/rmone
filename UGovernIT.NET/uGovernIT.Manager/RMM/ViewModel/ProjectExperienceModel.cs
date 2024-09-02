using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectExperienceModel
    {
        public string ProjectId { get; set; }
        public List<ProjectTag> ProjectTags { get; set; }
    }
    public class ProjectTag : IEquatable<ProjectTag>
    {
        public string TagId { get; set; }
        public bool IsMandatory { get; set; }
        public int MinValue { get; set; }
        public TagType Type { get; set; }

        public bool Equals(ProjectTag other)
        {
            if (other is null)
                return false;

            return this.TagId == other.TagId && this.Type == other.Type;
        }

        public override bool Equals(object obj) => Equals(obj as ProjectTag);
        public override int GetHashCode() => (TagId).GetHashCode();
    }

    public class ExperienceTaggedProjects : IEquatable<ExperienceTaggedProjects>
    {
        public long TagID { get; set; }
        public string TagCategory { get; set; }
        public string TagName { get; set; }
        public string MatchedScore { get; set; }
        public string TicketID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }

        public bool Equals(ExperienceTaggedProjects other)
        {
            if (other is null)
                return false;

            return this.TagID == other.TagID && this.TagName == other.TagName;
        }

        public override bool Equals(object obj) => Equals(obj as ExperienceTaggedProjects);
        public override int GetHashCode() => (TagName).GetHashCode();
    }

    public enum TagType
    {
        Certificate = 1,
        Experience = 2
    }
}
