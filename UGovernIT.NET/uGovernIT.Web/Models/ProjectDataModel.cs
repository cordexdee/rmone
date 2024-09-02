using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Web.Models
{
    public class ProjectDataModel
    {
        public string TicketId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }

        public string Priority { get; set; }
        public string Type { get; set; }
        public string Volume { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PreConStartDate { get; set; }
        public DateTime PreConEndDate { get; set; }
        public string TotalTasks { get; set; }
        public string CompletedTasks { get; set; }
        public string TotalAllocations { get; set;}
        public string FilledAllocations { get; set; }
        public string Studio { get; set; }
        public string Division { get; set; }
        public string ERPJobId { get; set; }
        public string Partners { get; set; }
        public string PartnersType { get; set; }
        public string ProjectShortName { get; set; }
        public string ModuleName { get; set; }
        public string RequestTypeTitle { get; set; }
        public string RequestType { get; set; }
        public string ChanceOfSuccess { get; set; }
        public string moduleLink { get; set; }
        public string AllocationLink { get; set; }
        public string LeadUserImageUrl { get; set; }
        public string LeadUserName { get; set; }
        public string LeadUserId { get; set; }
        public string LeadRole { get; set; }
        public string LeadProjectUrl { get; set; }
        public List<ModuleStageConstraints> UserTasks { get; set; }
        public List<UpdateLeadUsersResponse> LeadUsers { get; set; }
    }

    public class Estimatordetails 
    {
        public string ResourceId { get; set; }
        public string UserName { get; set; }
        public string UserImageURL { get; set; }
        public string PctAllocations { get; set; }
        public string Role { get; set; }

        public List<ProjectDataModel> items { get; set; }

    }
}