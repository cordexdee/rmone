using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AllocationTemplateRequestModel
    {
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int ProjectDuration { get; set; }
        public bool DeleteExistingAllocations { get; set; }
        public List<AllocationTemplateModel> Allocations { get; set; }
    }
}
