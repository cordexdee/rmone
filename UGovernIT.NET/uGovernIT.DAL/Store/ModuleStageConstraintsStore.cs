using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public  class ModuleStageConstraintsStore:StoreBase<ModuleStageConstraints>, IModuleStageConstraintsStore
    {
        public ModuleStageConstraintsStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IModuleStageConstraintsStore : IStore<ModuleStageConstraints>
    {
    }
}
