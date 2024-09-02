using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleMonthlyBudgetManager:ManagerBase<ModuleMonthlyBudget>, IModuleMonthlyBudgetManager
    {
        public ModuleMonthlyBudgetManager(ApplicationContext context):base(context)
        {
            store = new ModuleMonthlyStore(this.dbContext);
        }
        public  DataTable LoadModuleMonthlyBudget()
        {
            
            List<ModuleMonthlyBudget> configModuleMonthlyBudgetList = store.Load();
            return UGITUtility.ToDataTable<ModuleMonthlyBudget>(configModuleMonthlyBudgetList);
        }
        public ModuleMonthlyBudget InsertORUpdateData(ModuleMonthlyBudget objModuleBudget)
        {
            return (store as ModuleMonthlyStore).InsertORUpdateData(objModuleBudget);
        }
    }
    public interface IModuleMonthlyBudgetManager : IManagerBase<ModuleMonthlyBudget>
    {
        DataTable LoadModuleMonthlyBudget();

    }
}
