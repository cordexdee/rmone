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
   public class HolidaysAndWorkDaysCalendarManager:ManagerBase<HolidaysAndWorkDaysCalendar>,IHolidaysAndWorkDaysCalendarManager
    {
        public HolidaysAndWorkDaysCalendarManager(ApplicationContext context):base(context)
        {
            store = new HolidaysAndWorkDaysCalendarStore(this.dbContext);
        }
    }
    public interface IHolidaysAndWorkDaysCalendarManager : IManagerBase<HolidaysAndWorkDaysCalendar>
    {

    }
}
