using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Department)]
    public class Department:DBBaseEntity
    {
        
        public long ID { get; set; }
        //public string Name { get; set; }
        public string GLCode { get; set; }
        [NotMapped]
        public LookupValue CompanyLookup { get; set; }
        [NotMapped]
        public LookupValue DivisionLookup { get; set; }
        //public bool IsDeleted { get; set; }
        public long? CompanyIdLookup { get; set; }

        public long? DivisionIdLookup { get; set; }

        public string DepartmentDescription { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        [Column(DatabaseObjects.Columns.Manager)]
        public string Manager { get; set; }
        public Department()
        {
            CompanyLookup = new LookupValue();
            DivisionLookup = new LookupValue();
            Title = string.Empty;
            GLCode = string.Empty;
        }
    }
}
