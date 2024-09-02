using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
   
    [Table(DatabaseObjects.Tables.ProjectClass)]
    public class ProjectClass:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }
        public string ProjectNote { get; set; }
    }
}
