using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ChartFilter:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ColumnName { get; set; }
        public string ListName { get; set; }
        public string ModuleNameLookup { get; set; }
        public string ModuleType { get; set; }
        public string ValueAsId { get; set; }
        public bool IsDefault { get; set; }
        
    }
}
