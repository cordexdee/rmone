using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleMonitorOptionManager : ManagerBase<ModuleMonitorOption>, IModuleMonitorOptionManager
    {
        public ModuleMonitorOptionManager(ApplicationContext context) : base(context)
        {
            store = new ModuleMonitorOptionStore(this.dbContext);
        }
    }
    public interface IModuleMonitorOptionManager : IManagerBase<ModuleMonitorOption>
    {

    }
}
