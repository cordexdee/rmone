using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.WikiContents)]
    public class WikiContents : DBBaseEntity
    {
        public long ID { get; set; }
        public string  Content { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
    }
}
