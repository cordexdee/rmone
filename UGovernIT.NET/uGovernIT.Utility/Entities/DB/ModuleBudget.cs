using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleBudget)]
    public class ModuleBudget:DBBaseEntity
    {
    [NotMapped]
    public BudgetCategory budgetCategory { get; set; }
    public long ID { get; set; }
    public DateTime AllocationEndDate { get; set; }
    public DateTime AllocationStartDate { get; set; }
    public bool IsAutoCalculated { get; set; }
    public double BudgetAmount { get; set; } 
    public string BudgetDescription { get; set; } 
    public string BudgetItem { get; set; }
    public long BudgetCategoryLookup { get; set; }
    public int BudgetStatus { get; set; }
    public string Comment { get; set; }
    public long? DepartmentLookup { get; set; }
    public double UnapprovedAmount { get; set; }
    public string GLCode { get; set; }
    [Column("ModuleNameLookup")]
    public string ModuleName { get; set;}
    public string TicketId { get; set; }
	public string Title { get; set; }
        [NotMapped]
    public string VendorName { get; set; }
        [NotMapped]
    public int    VendorLookup { get; set; }
        [NotMapped]
    public double InvoiceNumber { get; set; }
    public int ResourceLookup { get; set; }
    }
}
