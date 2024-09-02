using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{     
        public class ModuleFormTabManager : ManagerBase<ModuleFormTab>, IModuleFormTabManager
        {
            public ModuleFormTabManager(ApplicationContext context) : base(context)
            {
                store = new ModuleFormTabStore(this.dbContext);
            }
        }
        public interface IModuleFormTabManager : IManagerBase<ModuleFormTab>
        {

        }
    
}
