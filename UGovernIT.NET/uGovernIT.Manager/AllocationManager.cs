﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
   public class AllocationManager:ManagerBase<Allocation>, IAllocationManager
   {
        public AllocationManager(ApplicationContext context):base(context)
        {
            store = new AllocationStore(this.dbContext);
        }

    }
    public interface IAllocationManager : IManagerBase<Allocation>
    {

    }
}
