using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IModuleMonitorOptionHistoryManager : IManagerBase<ModuleMonitorOptionHistory>
    {

    }

    public class ModuleMonitorOptionHistoryManager: ManagerBase<ModuleMonitorOptionHistory>, IModuleMonitorOptionHistoryManager
    {
        public ModuleMonitorOptionHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ModuleMonitorOptionHistoryStore(this.dbContext);
        }

    }
}
