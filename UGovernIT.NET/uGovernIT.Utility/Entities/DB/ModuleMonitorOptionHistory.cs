using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleMonitorOptionsHistory)]

    public class ModuleMonitorOptionHistory:DBBaseEntity
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public int ModuleMonitorMultiplier { get; set; }

        public string ModuleMonitorOptionName { get; set; }

        public string ModuleMonitorOptionLEDClass { get; set; }

        public long ModuleMonitorNameLookup { get; set; }
        
        public bool IsDefault { get; set; }

        public int BaselineId { get; set; }

        public DateTime BaselieDate { get; set; }

    }
}
