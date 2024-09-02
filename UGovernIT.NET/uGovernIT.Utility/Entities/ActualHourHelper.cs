
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ActualHourHelper
    {
        public int ItemId { get; set; }
        public DateTime WorkDate { get; set; }
        public double TimeSpent { get; set; }
        public string ResolutionDescription { get; set; }
        public bool NotifyRequestor { get; set; }
        public string TicketId { get; set; }
        public string WorkItem { get; set; }
        public long TaskId { get; set; }
        public int TicketStageStep { get; set; }
        public long RequestTypeLookup { get; set; }
        public string Title { get; set; }
    }
}
