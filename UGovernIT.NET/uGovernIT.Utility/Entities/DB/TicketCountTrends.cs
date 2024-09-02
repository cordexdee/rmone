using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.TicketCountTrends)]
    public class TicketCountTrends : DBBaseEntity
    {
        public long ID { get; set; }
        public string ModuleName {get;set;}
        public string Title { get;set;}
        public DateTime? EndOfDay { get; set; }
        public int NumCreated { get; set; }
        public int NumResolved { get; set; }
        public int NumClosed { get; set; }
        public int TotalActive { get; set; }
        public int TotalOnHold { get; set; }
        public int TotalResolved { get; set; }
        public int TotalClosed { get; set; }

    }
}
