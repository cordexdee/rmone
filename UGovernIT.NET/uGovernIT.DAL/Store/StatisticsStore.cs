using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class StatisticsStore : StoreBase<Statistics>, IStatisticsStore
    {
        public StatisticsStore(CustomDbContext context) : base(context)
        {


        }

    }
    public interface IStatisticsStore : IStore<Statistics>
    { }
}
