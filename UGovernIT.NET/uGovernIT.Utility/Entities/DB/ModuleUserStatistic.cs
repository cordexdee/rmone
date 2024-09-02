using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleUserStatistics)]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ModuleUserStatistic:DBBaseEntity
    {
        public long ID { get; set; }
        public bool IsActionUser { get; set; }
        public string ModuleNameLookup { get; set; }
        public string TicketId { get; set; }
        public string UserRole { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
    }
}
