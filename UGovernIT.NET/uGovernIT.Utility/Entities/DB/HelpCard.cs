using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.HelpCard)]
    public class HelpCard : DBBaseEntity
    {
        public long ID { get; set; }
        public string AuthorizedToView { get; set; }
        //public bool IsDeleted { get; set; }        
        public string Title { get; set; }
        public string Category { get; set; }
        public string AgentLookUp { get; set; }
        public long? HelpCardContentID { get; set; }
        public string TicketId { get; set; }
        public string Description { get; set; }
        
        
    }
}
