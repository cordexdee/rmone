using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL
{
    public class TicketCountTrendsStore : StoreBase<TicketCountTrends>, ITicketCountTrendsStore
    {
        public TicketCountTrendsStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ITicketCountTrendsStore : IStore<TicketCountTrends>
    {

    }
}
