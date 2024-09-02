using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketWorkflowSLASummary)]
    public class WorkflowSLASummary : DBBaseEntity
    {
        public long ID { get; set; }
        public int? ActualTime { get; set; }
        public bool? Closed { get; set; }
        public DateTime? DueDate { get; set; }
        public string EndStageName { get; set; }
        public int? EndStageStep { get; set; }
        public string ModuleNameLookup { get; set; }
        public long? RuleNameLookup { get; set; }
        public string SLACategoryChoice { get; set; }
        public string SLARuleName { get; set; }
        public DateTime? StageEndDate { get; set; }
        public DateTime? StageStartDate { get; set; }
        public string StartStageName { get; set; }
        public int? StartStageStep { get; set; }
        public int? TargetTime { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public bool Use24x7Calendar { get; set; }
        public long? ServiceLookup { get; set; }
        public int? OnHold { get; set; }
        public double? TotalHoldDuration { get; set; }
        public DateTime? OnHoldStartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? OnHoldTillDate { get; set; }

        public string Status { get; set; }
    }
}
