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
    public class ModuleWorkflowHistoryManager:ManagerBase<ModuleWorkflowHistory>, IModuleWorkflowHistoryManager
    {
        public ModuleWorkflowHistoryManager(ApplicationContext context):base(context)
        {
            store = new ModuleWorkflowHistoryStore(this.dbContext);
        }
    }
    public interface IModuleWorkflowHistoryManager:IManagerBase<ModuleWorkflowHistory>
    {

    }
}
