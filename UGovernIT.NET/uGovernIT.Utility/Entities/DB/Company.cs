using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Company)]
    public class Company:DBBaseEntity
    {
        public long ID { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string GLCode { get; set; }
        //public bool IsDeleted { get; set; }
        public string Title{get; set;}
        public string Description { get; set; }
        [NotMapped]
        public List<Department> Departments { get; set; }
        [NotMapped]
        public List<CompanyDivision> CompanyDivisions { get; set; }

        public Company()
        {
            Departments = new List<Department>();
            CompanyDivisions = new List<CompanyDivision>();
            Title = string.Empty;
            GLCode = string.Empty;
        }
    }
}
