using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using uGovernIT.DAL;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager.Managers
{
    public class TicketCountTrendsManager : ManagerBase<TicketCountTrends>, ITicketCountTrendsManager
    {
        public TicketCountTrendsManager(ApplicationContext context) : base(context)
        {
            store = new TicketCountTrendsStore(this.dbContext);
        }
    }
    public interface ITicketCountTrendsManager : IManagerBase<TicketCountTrends>
    {

    }
}
