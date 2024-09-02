using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleStageConstraintTemplatesManager:ManagerBase<ModuleStageConstraintTemplates>, IModuleStageConstraintTemplatesManager
    {
        public ModuleStageConstraintTemplatesManager(ApplicationContext context):base(context)
        {
            store = new ModuleStageConstraintTemplatesStore(this.dbContext);
        }
    }
    public interface IModuleStageConstraintTemplatesManager : IManagerBase<ModuleStageConstraintTemplates>
    {

    }
}
