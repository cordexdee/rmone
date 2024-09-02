using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CompanyDivisions)]
    public class CompanyDivision:DBBaseEntity
    {
        public long ID { get; set; }
        public string GLCode { get; set; }
        public long? CompanyIdLookup { get; set; }
        //public bool IsDeleted { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        [Column(DatabaseObjects.Columns.Manager)]
        public string Manager { get; set; }
        [NotMapped]
        public Company Company { get; set; }
        [NotMapped]
        public List<Department> Departments { get; set; }

        public CompanyDivision()
        {
            Title = string.Empty;
            GLCode = string.Empty;
        }
    }
}
