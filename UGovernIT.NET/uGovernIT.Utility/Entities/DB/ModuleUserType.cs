using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleUserTypes)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleUserType:DBBaseEntity
    {
        public long ID { get; set; }
        public string ColumnName { get; set; }
        public string CustomProperties { get; set; }
        public string DefaultUser { get; set; }
        [Column(DatabaseObjects.Columns.Groups)]
        public string Groups { get; set; }
        public bool ITOnly { get; set; }
        public bool ManagerOnly { get; set; }
        public string ModuleNameLookup { get; set; }
        public string UserTypes { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string FieldName { get; set; }
        [NotMapped]
        public string Prop_ManagerOf { get; set; }
        [NotMapped]
        public bool? Prop_DisableEmailTicketLink { get; set; }

    }
}
