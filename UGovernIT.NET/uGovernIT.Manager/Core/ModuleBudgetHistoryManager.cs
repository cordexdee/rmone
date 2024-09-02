using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IModuleBudgetHistoryManager : IManagerBase<ModuleBudgetHistory>
    {
        
    }

    public class ModuleBudgetHistoryManager: ManagerBase<ModuleBudgetHistory>, IModuleBudgetHistoryManager
    {

        public ModuleBudgetHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ModuleBudgetHistoryStore(this.dbContext);

        }
    }
}
