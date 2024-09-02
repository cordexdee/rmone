﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class GovernanceLinkItemStore : StoreBase<GovernanceLinkItem>, IGovernanceLinkItemStore
    {
        public GovernanceLinkItemStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IGovernanceLinkItemStore : IStore<GovernanceLinkItem>
    {

    }
}