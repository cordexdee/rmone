using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
   
    [Table(DatabaseObjects.Tables.ModuleDefaultValues)]
    public class ModuleDefaultValue:DBBaseEntity
    {
        public long ID { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
       // public LookupValue StageLookup { get; set; }
       // public string ModuleName { get; set; }
        public string ModuleNameLookup { get; set; }
        public string Title { get; set; }
        public string ModuleStepLookup { get; set; }
       
       

        //CustomProperties
        public string CustomProperties { get; set; }
        [NotMapped]
        public bool? Prop_Override { get; set; }

        [NotMapped]
        public string ModuleStageName { get; set; } = "unknown";
    }
}
