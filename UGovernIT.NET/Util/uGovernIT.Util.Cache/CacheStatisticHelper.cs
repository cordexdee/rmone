using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.Cache
{
    public static class CacheStatisticHelper
    {
        static CacheStatisticHelper()
        {
            CacheHelper<CacheStatistic>.AddCacheInstance();
        }

        public static CacheStatistic UpdateStat(CacheStatisticConstants key, string region, object newValue)
        {
            CacheStatistic stat = CacheHelper<CacheStatistic>.Get(key.ToString(), region);
            if (stat == null)
            {
                stat = new CacheStatistic(null, newValue);
            }
            else
            {
                stat.OldValue = stat.NewValue;
                stat.NewValue = newValue;
            }

            CacheHelper<CacheStatistic>.AddOrUpdate(key.ToString(), region, stat);

            return stat;
        }

        public static CacheStatistic GetStat(CacheStatisticConstants key, string region)
        {
            return CacheHelper<CacheStatistic>.Get(key.ToString(), region);
        }

    }
}
