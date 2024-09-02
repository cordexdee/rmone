using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleMonitorStore : StoreBase<ModuleMonitor>, IModuleMonitorStore
    {
        public ModuleMonitorStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IModuleMonitorStore : IStore<ModuleMonitor>
    {

    }
}
