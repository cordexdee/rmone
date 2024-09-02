using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.ProjectStandardWorkItems)]
    public class ProjectStandardWorkItem : DBBaseEntity
    {
        public int ID { get; set; }
        public string ItemOrder { get; set; }
        public int?  BudgetCategoryLookup { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
