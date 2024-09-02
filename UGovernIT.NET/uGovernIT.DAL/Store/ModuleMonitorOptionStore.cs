using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleMonitorOptionStore : StoreBase<ModuleMonitorOption>, IModuleMonitorOptionStore
    {
        public ModuleMonitorOptionStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IModuleMonitorOptionStore : IStore<ModuleMonitorOption>
    {

    }

}