using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleStageConstraintTemplates)]
    public class ModuleStageConstraintTemplates:DBBaseEntity
    {
        public long ID { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string Body { get; set; }
        public string Comment { get; set; }
        public string DocumentInfo { get; set; }
        public string DocumentLibraryName { get; set; }
        public string FormulaValue { get; set; }
        public bool? ModuleAutoApprove { get; set; }
        public string ModuleNameLookup { get; set; }
        public int? ModuleStep { get; set; }
        public double? PercentComplete { get; set; }
        public string Predecessors { get; set; }
        public string Priority { get; set; }
        public DateTime? ProposedDate { get; set; }
        public string ProposedStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public double? TaskActualHours { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public double? EstimatedHours { get; set; }
        public string TaskStatus { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
       
        public int ItemOrder { get; set; }
       
        public string UserRoleType { get; set; }
        public string DateExpression { get; set; }
        [NotMapped]
        public string ModuleStageName { get; set; }
    }

}
