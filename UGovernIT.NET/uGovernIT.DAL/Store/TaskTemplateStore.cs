﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class TaskTemplateStore:StoreBase<TaskTemplate>,IStore<TaskTemplate>
    {
        public TaskTemplateStore(CustomDbContext context):base(context)
        {

        }

    }
    public interface ITaskTemplateStore:IStore<TaskTemplate>
    { }
}