using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{  
    [Table(DatabaseObjects.Tables.LinkCategory)]
    public class LinkCategory:DBBaseEntity
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? ItemOrder { get; set; }
        public long LinkViewLookup { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public string LinkViewTitle { get; set; }
    }

}
