using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager.RMM
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AllocationTemplateModel
    {
        public long ID { get; set; }
        public int TemplateID { get; set; }
        public string ProjectID { get; set; }
        public string Title { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        public int TotalWorkingDays { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public double PctAllocation { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public bool IsResourceDisabled { get; set; }
        public bool IsResourceOverAllocated { get; set; }
        public bool SoftAllocation { get; set; }
        public bool NonChargeable { get; set; }
        public bool IsInPreconStage { get; set; }
        public bool IsInConstStage { get; set; }
        public bool IsInCloseoutStage { get; set; }
        public string UserImageUrl { get; set; }
        public string Tags { get; set; }
        public bool IsLocked { get; set; }
        public double PctAllocationConst { get; set; }
        public double PctAllocationCloseOut { get; set; }
    }
}
