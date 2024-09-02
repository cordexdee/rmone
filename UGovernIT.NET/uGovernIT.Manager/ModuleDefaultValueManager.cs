using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ModuleDefaultValueManager : ManagerBase<ModuleDefaultValue>, IModuleDefaultValueManager
    {
        public ModuleDefaultValueManager(ApplicationContext context):base(context)
        {
            store = new ModuleDefaultValueStore(this.dbContext);
        }
        public ModuleDefaultValue AddOrUpdate(ModuleDefaultValue moduleDefaultValue)
        {
            if (moduleDefaultValue != null)
            {
                if (moduleDefaultValue.ID > 0)
                    this.Update(moduleDefaultValue);
                else
                    this.Insert(moduleDefaultValue);
            }
            return moduleDefaultValue;
        }
    }
    public interface IModuleDefaultValueManager : IManagerBase<ModuleDefaultValue>
    {
         ModuleDefaultValue AddOrUpdate(ModuleDefaultValue moduleDefaultValue);
    }
}