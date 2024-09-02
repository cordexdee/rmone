using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class ModuleStageConstraintTemplatesStore:StoreBase<ModuleStageConstraintTemplates>, IModuleStageConstraintTemplatesStore
    {
        public ModuleStageConstraintTemplatesStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IModuleStageConstraintTemplatesStore : IStore<ModuleStageConstraintTemplates>
    {
    }
}
