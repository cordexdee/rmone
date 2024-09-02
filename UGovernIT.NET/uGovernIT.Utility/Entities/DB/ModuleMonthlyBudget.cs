using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleMonthlyBudget)]
    public class ModuleMonthlyBudget:DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public Double ActualCost { get; set; }
        public Double BudgetAmount { get; set;}
        public string BudgetType { get; set; }
        public string TicketId { get; set; }
        [Column("ModuleNameLookup")]
        public string ModuleName { get; set; }
        public long BudgetCategoryLookup { get; set; }
        public Double EstimatedCost { get; set; }
        public Double NonProjectActualTotal { get; set; }
        public Double NonProjectPlanedTotal { get; set; }
        public Double ProjectActualTotal { get; set; }
        public Double ProjectPlanedTotal { get; set; }
        public Double ResourceCost { get; set; }
        public string Title { get; set; }

    }
}
