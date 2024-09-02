﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class HolidaysAndWorkDaysCalendarStore:StoreBase<HolidaysAndWorkDaysCalendar>, IHolidaysAndWorkDaysCalendarStore
    {
        public HolidaysAndWorkDaysCalendarStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IHolidaysAndWorkDaysCalendarStore : IStore<HolidaysAndWorkDaysCalendar>
    {
    }
}
