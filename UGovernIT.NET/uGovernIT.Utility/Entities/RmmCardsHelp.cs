using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    public class RmmCardsHelp
    {
    }

    public class ResourceCard
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class  AsistantsResponse
    {
        public bool isMoveUpVisible { get; set; }
        public List<AssitantsResource> lstAssitantsResource = new List<AssitantsResource>();
    }

    public class AssitantsResource
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string BudgetLookup { get; set; }
        public int ResourceHourlyRate { get; set; }
        public string DepartmentLookup { get; set; }
        public string LocationLookup { get; set; }
        public string Allocation { get; set; }
        public string imageUrl { get; set; }
        public string UsrEditLinkUrl { get; set; }
        public string ManagerLinkUrl { get; set; }
        public string AllocationBar { get; set; }
        public string JobTitle { get; set; }
        public bool IsAssistantExist { get; set; }
        public int AssitantCount { get; set; }
        public int ProjectCount { get; set; }

        public string Consultant { get; set; }
        public int PendingApprovalCount { get; set; }        
    }


}
