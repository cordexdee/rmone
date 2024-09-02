using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleMonitorOptions)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ModuleMonitorOption:DBBaseEntity
    {
       
        public long ID { get; set; }
        public string Title { get; set; }
        public int ModuleMonitorMultiplier { get; set; }
        public string ModuleMonitorOptionName { get; set; }
        public string ModuleMonitorOptionLEDClass { get; set; }
        public long ModuleMonitorNameLookup { get; set; }

        public bool IsDefault { get; set; }
    }
}
