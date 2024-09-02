using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.Cache
{
    public class CacheStatisticTypeDetail
    {
        public CacheStatisticConstants CacheType { get; set; }
        public string Title { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<Dictionary<string,string>> Detail { get; set; }
    }
}
