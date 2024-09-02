using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ScheduleActionsManager:ManagerBase<SchedulerAction>, IScheduleActionsManager
    {
        public ScheduleActionsManager(ApplicationContext context):base(context)
        {
            store = new ScheduleActionsStore(this.dbContext);
        }
        public  SchedulerAction AddUpdate(SchedulerAction relation)
        {
            if (relation != null)
            {
                if (relation.ID > 0)
                {
                    store.Update(relation);
                }
                else
                {
                    store.Insert(relation);
                }
            }
            else
            {
                relation = null;
            }
            return relation;
        }
    }
    public interface IScheduleActionsManager : IManagerBase<SchedulerAction>
    {
        SchedulerAction AddUpdate(SchedulerAction relation);
    }
}
