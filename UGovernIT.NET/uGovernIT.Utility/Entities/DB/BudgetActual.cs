using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleBudgetActuals)]
    public class BudgetActual:DBBaseEntity
    {
        public long ID { get; set; }

        public DateTime AllocationEndDate { get; set; }

        public DateTime AllocationStartDate { get; set; }

        public Double BudgetAmount { get; set; }

        public string BudgetDescription { get; set; }
        [Column("ModuleNameLookup")]
        public string ModuleName { get; set; }

        public string TicketId { get; set; }

        public string Title { get; set; }

        public string VendorName { get; set; }

        public long? VendorLookup { get; set; }

        public string InvoiceNumber { get; set; }

        public long ModuleBudgetLookup { get; set; }
        public long? DepartmentLookup { get; set; }
        public string BudgetItem { get; set; }

        public Double ActualCost { get; set; }
    }
}
