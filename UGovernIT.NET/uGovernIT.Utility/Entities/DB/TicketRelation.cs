using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketRelation)]
    public class TicketRelation:DBBaseEntity
    {
        public long ID { get; set; }
        public string ParentModuleName { get; set; }
        public string ParentTicketID { get; set; }
        public string ChildModuleName { get; set; }
        public string ChildTicketID { get; set; }
    }
}
