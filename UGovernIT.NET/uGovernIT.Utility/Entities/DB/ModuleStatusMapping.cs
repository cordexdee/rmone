using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
   
    [Table(DatabaseObjects.Tables.TicketStatusMapping)]
    public class ModuleStatusMapping:DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ModuleNameLookup { get; set; }
        public int StageTitleLookup { get; set; }
        public int GenericStatusLookup { get; set; }

    }
}
