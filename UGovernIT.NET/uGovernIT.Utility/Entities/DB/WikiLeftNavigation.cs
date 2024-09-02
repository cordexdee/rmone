using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
     public class WikiLeftNavigation:DBBaseEntity
    {
        public long ID { get; set; }
        public string ColumnType { get; set; }
        public string ConditionalLogic { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int ItemOrder { get; set; }

    }
}
