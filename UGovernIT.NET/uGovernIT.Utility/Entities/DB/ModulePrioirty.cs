using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketPriority)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModulePrioirty:DBBaseEntity
    {
        [Key]
        public long ID { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public string ModuleNameLookup { get; set; }
        public int ItemOrder { get; set; }
        public bool IsVIP { get; set; }
        [NotMapped]
        public string NotifyTo { get; set; }
        public string Color { get; set; }
        public string EmailIDTo { get; set; }
        //public bool IsDeleted { get; set; }
        public string uPriority { get; set; }
        public string Title { get; set; }
        public bool NotifyInPlainText { get; set; }
    }
}
