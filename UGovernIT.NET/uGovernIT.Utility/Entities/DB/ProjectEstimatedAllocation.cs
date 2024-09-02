using System.ComponentModel.DataAnnotations.Schema;
using System;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CRMProjectAllocation)]
    [JsonObject(MemberSerialization.OptOut)]
      public  class ProjectEstimatedAllocation : DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public int Duration { get; set; }
        public int ItemOrder { get; set; }
        public double PctAllocation { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        //public bool IsDeleted { get; set; }
        public string TicketId { get; set; }
        public bool SoftAllocation { get; set; }
        public bool NonChargeable { get; set; }
        public bool IsLocked { get; set; }
    }
}
