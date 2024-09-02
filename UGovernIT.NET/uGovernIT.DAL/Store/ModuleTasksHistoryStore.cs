using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public interface IModuleTasksHistoryStore : IStore<ModuleTasksHistory>
    {

    }

    public class ModuleTasksHistoryStore: StoreBase<ModuleTasksHistory>, IModuleTasksHistoryStore
    {
        public ModuleTasksHistoryStore(CustomDbContext context) : base(context)
        {

        }
    }
}
