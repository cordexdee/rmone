using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketSeverity)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleSeverity:DBBaseEntity
    {
      
        public long ID { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public string Title { get; set; }
        public string ModuleNameLookup { get; set; }
        public int ItemOrder { get; set; }
        //public bool IsDeleted { get; set; }
        public string Severity { get; set; }
    }
}
