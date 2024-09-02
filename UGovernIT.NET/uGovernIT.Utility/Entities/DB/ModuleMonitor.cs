using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleMonitors)]
    public class ModuleMonitor:DBBaseEntity
    {
       
        public long ID { get; set; }
        public bool IsDefault { get; set; }
        //public int ModuleMonitorOptionNameLookup { get; set; }   //creating a reverse relationship with modulemonitoroptions table
        public string ModuleNameLookup { get; set; }
        public string MonitorName { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
    }
}
