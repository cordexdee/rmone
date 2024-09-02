using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility 
{
   
    [Table(DatabaseObjects.Tables.GenericTicketStatus)]
    public class GenericTicketStatus : DBBaseEntity
    {
        public int ID { get; set; }
        public string GenericStatus { get; set; }
        public string Title { get; set; }
    }
}
