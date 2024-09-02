
using System.Data;

namespace uGovernIT.Report.Entities
{
    public class TicketSummaryByPRPEntity
    {
        public DataTable Data { get; set; }
        public bool GroupByCategory { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ModuleName { get; set; }
        public bool IsModuleSort { get; set; }
        public bool IncludeTechnician { get; set; }
        public string IncludeCounts { get; set; }
    }
}