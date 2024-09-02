using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Utility;

namespace uGovernIT.Manager.RMM
{
    public class FindTeamsRequest
    {
        public string TicketID { get; set; }
        public SearchTeamCriteria SearchTeamCriteria { get; set; }      
        public List<AllocationResource> SelectedResources { get; set; } 
    }

    public class AllocationResource
    {
        public string ID { get; set; }
        public int Index { get; set; }
        public string TypeName { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public double RequiredPctAllocation { get; set; }
    }

    public class SearchTeamCriteria
    {
        public List<ProjectTag> SelectedTags { get; set; }
        public string SelectedCertifications { get; set; }
        public bool CommonExperiences { get; set; }
        public bool WorkedTogether { get; set; }
        public bool CommonClient { get; set; }
        public bool CommonSector { get; set; }
    }
}
