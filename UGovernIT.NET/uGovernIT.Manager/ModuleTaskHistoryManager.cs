using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public interface IModuleTaskHistoryManager : IManagerBase<ModuleTasksHistory>
    {

    }

   public  class ModuleTaskHistoryManager : ManagerBase<ModuleTasksHistory>, IModuleTaskHistoryManager
    {
        public ModuleTaskHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ModuleTasksHistoryStore(this.dbContext);
        }
    }
}
