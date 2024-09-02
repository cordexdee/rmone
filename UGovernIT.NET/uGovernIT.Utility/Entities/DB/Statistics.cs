using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.Statistics)]
    public class Statistics
    {
        public long ID { get; set; }

        public string TicketID { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }

    }
}
