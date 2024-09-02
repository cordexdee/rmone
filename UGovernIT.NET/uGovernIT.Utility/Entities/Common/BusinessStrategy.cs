using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class BusinessStrategy : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
