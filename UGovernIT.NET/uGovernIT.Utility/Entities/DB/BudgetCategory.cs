using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.BudgetCategories)]
    public class BudgetCategory: DBBaseEntity
    {
        public long ID { get; set; }
        public string AuthorizedToEdit { get; set; }
        public string AuthorizedToView { get; set; }
        public string BudgetAcronym { get; set; }
        public string BudgetCategoryName { get; set; }
        public string BudgetCOA { get; set; }
        public string BudgetDescription { get; set; }
        public string BudgetSubCategory { get; set; }
        public string BudgetType { get; set; }
        public string BudgetTypeCOA { get; set; }
        public bool   CapitalExpenditure { get; set; }
        public bool IncludesStaffing { get; set; }
        //public bool IsDeleted { get; set; }
        public string Title { get; set; }
    }
}
