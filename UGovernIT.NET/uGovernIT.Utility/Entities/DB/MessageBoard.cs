using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.MessageBoard)]
    public class MessageBoard:DBBaseEntity
    {
        public long ID { get; set; }
        public string AuthorizedToView { get; set; }
        public string Body { get; set; }
        public DateTime? Expires { get; set; }       
        public string MessageType { get; set; }
        public string NavigationUrl { get; set; }
        public string TicketId { get; set; }       
        public string Title { get; set; }
        public MessageBoard()
        {
            Expires = (DateTime)SqlDateTime.MinValue;
        }
    }
}
