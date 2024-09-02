using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleWorkflowHistoryStore:StoreBase<ModuleWorkflowHistory>, IModuleWorkflowHistoryStore
    {
        public ModuleWorkflowHistoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IModuleWorkflowHistoryStore:IStore<ModuleWorkflowHistory>
    {
    }
}
