using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.Cache
{
    public class CacheStatistic
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }


        public CacheStatistic(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
