using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ApplicationRole)]
    public class ApplicationRole  : DBBaseEntity
    {
         public long ID { get; set; }
        public string ApplicationRoleModuleLookup { get; set; }
        public long APPTitleLookup { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }    
        [NotMapped]
        public string Modules { get; set; }
        public long? ItemOrder { get; set; }
    }
}
