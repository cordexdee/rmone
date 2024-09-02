﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class CRMActivitiesStore : StoreBase<CRMActivities>, ICRMActivitiesStore
    {
        public CRMActivitiesStore(CustomDbContext context) : base(context)
        {
        }
    }

    public interface ICRMActivitiesStore : IStore<CRMActivities>
    {

    }
}
