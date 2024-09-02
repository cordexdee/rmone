using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table("Config_ModuleLifeCycles")]
    public class ModuleLifeCycle:DBBaseEntity
    {
        public long ID { get; set; }
        public int ItemOrder { get; set; }
        public string Description{ get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }
        public string ModuleNameLookup { get; set; }
    }
}
