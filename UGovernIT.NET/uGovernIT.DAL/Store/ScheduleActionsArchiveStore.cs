using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ScheduleActionsArchiveStore: StoreBase<SchedulerActionArchive>, IScheduleActionsArchiveStore
    {
        public ScheduleActionsArchiveStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IScheduleActionsArchiveStore : IStore<SchedulerActionArchive>
    {

    }
}
