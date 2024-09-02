using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ChartFormula:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ChartTemplateIds { get; set; }
        public string Formula { get; set; }
        public string FormulaValue { get; set; }
        public string ModuleNameLookup { get; set; }
        public string ModuleType { get; set; }        
        public bool IsDefault { get; set; }
    }
}
