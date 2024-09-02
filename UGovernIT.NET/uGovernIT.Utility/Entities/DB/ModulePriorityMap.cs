using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace uGovernIT.Utility
{

    [Table(DatabaseObjects.Tables.RequestPriority)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModulePriorityMap:DBBaseEntity
    {
        public long ID { get; set; }
        public string ModuleNameLookup { get; set; }

        public long PriorityLookup { get; set; }
        public long SeverityLookup { get; set; }
        public long ImpactLookup { get; set; }

        [ForeignKey("PriorityLookup")]
        public ModulePrioirty ModulePrioirty { get; set; }
        [ForeignKey("SeverityLookup")]
        public ModuleSeverity ModuleSeverity { get; set; }
        [ForeignKey("ImpactLookup")]
        public ModuleImpact ModuleImpact { get; set; }

        
    }
}
