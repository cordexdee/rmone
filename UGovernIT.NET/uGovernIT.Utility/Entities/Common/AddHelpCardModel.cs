
using System.Collections.Generic;
using uGovernIT.Utility.Entities.Common;


namespace uGovernIT.Utility.Entities
{
    public class AddHelpCardModel
    {
        public long ID { get; set; }
        public string HelpCardTicketId { get; set; }
        public string HelpCardTitle { get; set; }
        public string HelpCardCategory { get; set; }
        public string HelpCardContent { get; set; }
        public List<AgentsDto> listAgents = new List<AgentsDto>();
        public string AgentLookUp { get; set; }
        public string AgentContent { get; set; }
        public string Description { get; set; }
    }
}
