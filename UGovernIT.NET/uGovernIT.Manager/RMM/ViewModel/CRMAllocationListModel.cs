using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AllocationListModel
    {
        public string ProjectID { get; set; }
        public string PreConStart { get; set; }
        public string PreConEnd { get; set; }
        public string ConstStart { get; set; }
        public string ConstEnd { get; set; }
        public string LastEditedRow { get; set; }
        public bool OverrideAllocations { get; set; }
        public bool IsAllocationSplitted { get; set; }
        public List<ProjectAllocationModel> Allocations { get; set; }
        public string CalledFrom { get; set; }
        public bool UpdateAllProjectAllocations { get; set; } = true;
        public bool UseThreading { get; set; } = true;
        public bool NeedReturnData { get; set; } = true;

    }
}
