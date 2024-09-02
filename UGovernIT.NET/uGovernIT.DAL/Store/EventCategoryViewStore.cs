using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class EventCategoryViewStore : StoreBase<EventCategory>, IEventCategoryViewStore
    {
        public EventCategoryViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IEventCategoryViewStore :  IStore<EventCategory>
    {

    }
   
}
