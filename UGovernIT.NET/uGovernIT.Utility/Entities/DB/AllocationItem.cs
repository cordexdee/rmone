using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class AllocationItem
    {
        public RResourceAllocation Alloc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Updated { get; set; }
    }
}
