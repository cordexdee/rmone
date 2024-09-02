using System.Collections.Generic;

namespace uGovernIT.Utility
{

    public class ProjectComparisonScore
    {
        public string PrimaryProjectTicketID { get; set; }
        public string PrimaryProjectTitle { get; set; }
        public List<ProjectComparisonDataItem> SecondaryProjects { get; set; } = new List<ProjectComparisonDataItem>();
    }

    public class ProjectComparisonDataItem
    {
        public string SecondaryProjectTicketID { get; set; }
        public string SecondaryProjectTitle { get; set; }
        public double? TotalScore { get; set; }
        public List<ProjectComparisonField> Fields { get; set; } = new List<ProjectComparisonField>();
    }
    public class ProjectComparisonField
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public double Score { get; set; }
    }
}
