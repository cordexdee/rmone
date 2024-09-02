using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
  
    [Table(DatabaseObjects.Tables.RequestTypeByLocation)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleRequestTypeLocation:DBBaseEntity
    {
        public long ID { get; set; }
        [NotMapped]
        public LookupValue RequestType { get; set; }
        [NotMapped]
        public LookupValue Location { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string Owner { get; set; }
        [Column(DatabaseObjects.Columns.PRPGroup)]
        public string PRPGroup { get; set; }
        [Column(DatabaseObjects.Columns.TicketORP)]
        public string ORP { get; set; }
        [Column("EscalationManagerUser")]
        public string EscalationManager { get; set; }
        [Column(DatabaseObjects.Columns.RequestTypeBackupEscalationManager)]
        public string BackupEscalationManager { get; set; }
        [NotMapped]
        public LookupValue Module { get; set; }
        public double AssignmentSLA { get; set; }
        public double CloseSLA { get; set; }
        public long LocationLookup { get; set; }
        public string ModuleNameLookup { get; set; }
        public double RequestorContactSLA { get; set; }
 
        public long RequestTypeLookup { get; set; }
        
        public double ResolutionSLA { get; set; }
        public string Title { get; set; }
        public string PRPUser { get; set; }


    }
}
