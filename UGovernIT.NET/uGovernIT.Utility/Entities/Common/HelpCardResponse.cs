using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Utility.Entities.Common
{
    public class HelpCardResponse
    {
        public long ID { get; set; }
        public string AuthorizedToView { get; set; }
        public bool IsDeleted { get; set; }
        
        public string HelpCardTitle { get; set; }
        public long? HelpCardContentID { get; set; }
        public string HelpCardTicketId { get; set; }
        public string HelpCardContent { get; set; }
        public string HelpCardCategory { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
        public List<AgentsDto> listAgents = new List<AgentsDto>();
        public string AgentLookUp { get; set; }
        public string AgentContent { get; set; }
        public string Description { get; set; }
    }
}
