using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleMonthlyBudgetHistory)]
    public class ModuleMonthlyBudgetHistory : DBBaseEntity
    {
        public long ID { get; set; }

        public DateTime AllocationStartDate { get; set; }

        public double ActualCost { get; set; }

        public double BudgetAmount { get; set; }

        public string BudgetType { get; set; }

        public string TicketId { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }

        public long BudgetLookup { get; set; }

        public double EstimatedCost { get; set; }

        public double NonProjectActualTotal { get; set; }

        public double NonProjectPlanedTotal { get; set; }

        public double ProjectActualTotal { get; set; }

        public double ProjectPlanedTotal { get; set; }

        public double ResourceCost { get; set; }

        public string Title { get; set; }

        public DateTime BaselineDate { get; set; }

        public int BaselineId { get; set; }

    }
}
