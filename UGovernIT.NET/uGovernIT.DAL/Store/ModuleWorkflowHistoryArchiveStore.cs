using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleWorkflowHistoryArchiveStore : StoreBase<ModuleWorkflowHistoryArchive>, IModuleWorkflowHistoryArchiveStore
    {
        public ModuleWorkflowHistoryArchiveStore(CustomDbContext context) : base(context)
        {
            
        }
    }
    public interface IModuleWorkflowHistoryArchiveStore : IStore<ModuleWorkflowHistoryArchive>
    {
    }
}
