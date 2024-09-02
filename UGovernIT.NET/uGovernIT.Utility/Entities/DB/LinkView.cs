using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{   
    [Table(DatabaseObjects.Tables.LinkView)]
    public class LinkView:DBBaseEntity
    {
        public long ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }       
    }

}
