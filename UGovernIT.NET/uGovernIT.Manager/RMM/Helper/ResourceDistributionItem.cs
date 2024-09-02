using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM
{
    public class ResourceDistributionItem
    {
        public DateTime Date { get; set; }
        public double PctAllocation { get; set; }
        public double PctPlannedAllocation { get; set; }
        public long AllocationId { get; set; }
    }
}
