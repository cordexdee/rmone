using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ScheduleActionsStore:StoreBase<SchedulerAction>, IScheduleActionsStore
    {
        public ScheduleActionsStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IScheduleActionsStore : IStore<SchedulerAction>
    {
    }
}
