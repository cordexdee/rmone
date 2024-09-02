using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleMonthlyStore:StoreBase<ModuleMonthlyBudget>, IModuleMonthlyStore
    {
        public ModuleMonthlyStore(CustomDbContext context):base(context)
        {

        }
        public ModuleMonthlyBudget InsertORUpdateData(ModuleMonthlyBudget objModuleBudget)
        {
            if (objModuleBudget.ID > 0)
            {
              Update(objModuleBudget);
            }
            else
            {
               Insert(objModuleBudget);

            }
            return objModuleBudget;
        }
    }
    public interface IModuleMonthlyStore:IStore<ModuleMonthlyBudget>
    {
        ModuleMonthlyBudget InsertORUpdateData(ModuleMonthlyBudget objModuleBudget);
    }
}
