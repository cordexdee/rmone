using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace uGovernIT.Utility
{
   [Table(DatabaseObjects.Tables.ModuleFormTab)]
   [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleFormTab: DBBaseEntity
    {
        public long ID { get; set; }
        //public int TabNumber { get; set; }
        //public string Name { get; set; }
        //public int Sequence { get; set; }
        //public string ModuleName { get; set; }
        public string AuthorizedToView { get; set; }
        public string AuthorizedToEdit { get; set; }
        public string CustomProperties { get; set; }
        //public bool? Prop_IsScrumTab { get; set; }
        //public bool prop_IsDocumentTab { get; set; }
        //public bool prop_IsDeliverableTab { get; set; }
        //public bool prop_IsActionLogTab { get; set; }
        //public bool? prop_IsSummaryTab { get; set; }
        public string Title { get; set; }
        public string TabName { get; set; }
        public string ModuleNameLookup { get; set; }
        public int TabSequence { get; set; }
        public int TabId { get; set; }
        // public object TabNumber { get; set; }
        public bool? ShowInMobile { get; set; }
        [NotMapped]
        public int TabSequenceOnScreen { get; set; }
    }
}
