using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectAllocationModel
    {
        public DateTime AllocationEndDate { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public long ID { get; set; }
        public float PctAllocation { get; set; }
        public string ProjectID { get; set; }
        public long TemplateID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string SoftAllocation { get; set; }
        public string NonChargeable { get; set; }
        public string IsLocked { get; set; }
        public string Tags { get; set; }
        public bool isChangeStartDate { get; set; }
    }
}
