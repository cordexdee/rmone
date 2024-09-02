using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class ModuleMonitorManager : ManagerBase<ModuleMonitor>, IModuleMonitorManager
    {
        public ModuleMonitorManager(ApplicationContext context) : base(context)
        {
            store = new ModuleMonitorStore(context);
        }
    }
    public interface IModuleMonitorManager : IManagerBase<ModuleMonitor>
    {

    }
}
