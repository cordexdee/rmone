using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.NPRResources)]
    public class NPRResource : DBBaseEntity
    {
        public long ID { get; set; }
        public string _ResourceType { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public string BudgetDescription { get; set; }
        public string BudgetTypeChoice { get; set; }
        public double EstimatedHours { get; set; }
        public double NoOfFTEs { get; set; }
        public string TicketId { get; set; }
        public string RequestedResourcesUser { get; set; }
        public string Title { get; set; }
        public long? UserSkillLookup { get; set; }
        public string RoleNameChoice { get; set; }
        public decimal HourlyRate { get; set; }
        public string ModuleNameLookup { get; set; }
    }
}
