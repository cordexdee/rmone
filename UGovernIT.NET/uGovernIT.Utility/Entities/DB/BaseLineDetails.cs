using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.BaseLineDetails)]

    public class BaseLineDetails:DBBaseEntity
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public int BaselineId { get; set; }

        public string BaselineComment { get; set; }

        public DateTime BaselineDate { get; set; }

        public string  TicketID { get; set; }

        public string ModuleNameLookup { get; set; }

    }
}
