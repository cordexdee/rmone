using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class TaskEmailViewManager : ManagerBase<ModuleTaskEmail>, ITaskEmailViewManager
    {
        public TaskEmailViewManager(ApplicationContext context) : base(context)
        {
            store = new TaskEmailViewStore(this.dbContext);
        }
        
    }
    public interface ITaskEmailViewManager : IManagerBase<ModuleTaskEmail>
    {
    }
}
