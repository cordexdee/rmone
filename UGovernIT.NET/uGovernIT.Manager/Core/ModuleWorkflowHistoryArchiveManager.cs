using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class ModuleWorkflowHistoryArchiveManager:ManagerBase<ModuleWorkflowHistoryArchive>, IModuleWorkflowHistoryArchiveManager
    {
        public ModuleWorkflowHistoryArchiveManager(ApplicationContext context):base(context)
        {
            store = new ModuleWorkflowHistoryArchiveStore(this.dbContext);
        }
    }
    public interface IModuleWorkflowHistoryArchiveManager : IManagerBase<ModuleWorkflowHistoryArchive>
    {

    }
}
