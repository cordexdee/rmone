﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class ProjectStandardWorkItemStore :StoreBase<ProjectStandardWorkItem>, IProjectStandardWorkItemStore
    {

        public ProjectStandardWorkItemStore(CustomDbContext context) : base(context)
        {

        }

    }

    public interface IProjectStandardWorkItemStore : IStore<ProjectStandardWorkItem>
    {

    }

}