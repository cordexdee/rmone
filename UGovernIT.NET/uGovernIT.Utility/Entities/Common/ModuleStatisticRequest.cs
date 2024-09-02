using System.Collections.Generic;

namespace uGovernIT.Utility
{
    public class ModuleStatisticRequest
    {
        public string ModuleName { get; set; }
        //public SPWeb SPWebObj { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string CurrentTab { get; set; }
        public string RequestType { get; set; }
        public string UserType { get; set; }

        public List<string> Tabs { get; set; }
        public List<string> HideTickets { get; set; }

        public bool IsTicketCountAsRolesRequired { get; set; }
        public bool IsFilteredTableRequired { get; set; }
        public bool ExcludeNPRResolved { get; set; }

        public bool IsGlobalSearch { get; set; }
        public bool CustomFilterTabEnable { get; set; }
        public bool Lookahead { get; set; }

        public TicketStatus Status { get; set; }
        public ModuleType ModuleType { get; set; }
        
        public ModuleStatisticRequest()
        {
            ModuleName = string.Empty;
            Tabs = new List<string>();
        }
    }
}
