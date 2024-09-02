using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MostAvailableResourceResponse
    {
        public long ID { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public double? PctAllocation { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public int Complexity { get; set; } = 0;
        public int AllocationRange { get; set; }
        public double PctAllocationCloseOut { get; set; }
        public double PctAllocationConst { get; set; }

    }
}
