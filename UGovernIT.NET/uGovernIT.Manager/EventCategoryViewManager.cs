using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public  class EventCategoryViewManager:ManagerBase<EventCategory>, IEventCategoryViewManager
    {
        public EventCategoryViewManager(ApplicationContext context):base(context)
        {
            store = new EventCategoryViewStore(this.dbContext);
        }       
    }
    public interface IEventCategoryViewManager : IManagerBase<EventCategory>
    {

    }
}
