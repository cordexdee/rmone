using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.HelpCardContent)]
    public class HelpCardContent : DBBaseEntity
    {
        public long ID { get; set; }
        public string Content { get; set; }
        public string AgentContent { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
    }
}
