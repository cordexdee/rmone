using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.WikiDiscussion)]
    public class WikiDiscussion : DBBaseEntity
    {
        public long ID { get; set; }
        public string Comment { get; set; }
        public string TicketId { get; set; }
    }
}
