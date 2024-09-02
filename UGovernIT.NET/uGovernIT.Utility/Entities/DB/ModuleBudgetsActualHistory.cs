using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleBudgetActualsHistory)]
    public class ModuleBudgetsActualHistory:DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public Double BudgetAmount { get; set; }
        public string BudgetDescription { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string VendorName { get; set; }
        public long? VendorLookup { get; set; }
        public string InvoiceNumber { get; set; }
        public long ModuleBudgetLookup { get; set; }
        public int BaselineId { get; set; }
        public DateTime BaselineDate { get; set; }
    }
}
