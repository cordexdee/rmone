using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceAllocation)]
    [JsonObject(MemberSerialization.OptOut)]
    public class RResourceAllocation:DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        //public bool? IsDeleted { get; set; }
        public double? PctAllocation { get; set; }
        public double? PctPlannedAllocation { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        [NotMapped]
        public ResourceWorkItems ResourceWorkItems { get; set; }
        public long ResourceWorkItemLookup { get; set; }
        public string Title { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public double? PctEstimatedAllocation { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public string RoleId { get; set; }
        public string ProjectEstimatedAllocationId { get; set; }
        public string TicketID { get; set; }
        public bool SoftAllocation { get; set; }
        public bool NonChargeable { get; set; }
        public RResourceAllocation()
        {
            AllocationStartDate = DateTime.Now;
            AllocationEndDate = new DateTime(DateTime.Now.Year, 12, 31);
        }

        public RResourceAllocation(string userId)
        {
            Resource = userId;
            AllocationStartDate = DateTime.Now;
            AllocationEndDate = new DateTime(DateTime.Now.Year, 12, 31);
        }
        [NotMapped]
        public bool IsAllocInPrecon { get; set; }
        [NotMapped]
        public bool IsAllocInConst { get;set; }
        [NotMapped]
        public bool IsAllocInCloseOut { get; set; }
        [NotMapped]
        public bool IsStartDateBeforePrecon { get; set; }
        [NotMapped]
        public bool IsStartDateBetweenPreconAndConst { get;set; }
        [NotMapped]
        public bool IsStartDateBetweenConstAndCloseOut { get;set; }
    }

    public class UserWithPercentage
    {
        public string UserId { get; set; }
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoleTitle { get; set; }
        public string CustomProperty { get; set; }
        public string ProjectEstiAllocId { get; set; }
        public string RoleId { get; set; }
        public bool SoftAllocation { get; set; }
        public bool NonChargeable { get; set; }
    }
}
