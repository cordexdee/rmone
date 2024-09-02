﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class PMMIssueStore:StoreBase<PMMIssues>, IPMMIssueStore
    {
        public PMMIssueStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IPMMIssueStore : IStore<PMMIssues>
    {

    }
}