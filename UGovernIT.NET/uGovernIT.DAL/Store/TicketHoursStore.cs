using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class TicketHoursStore:StoreBase<ActualHour>, ITicketHoursStore
    {
        public TicketHoursStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface ITicketHoursStore : IStore<ActualHour>
    {
    }
}
