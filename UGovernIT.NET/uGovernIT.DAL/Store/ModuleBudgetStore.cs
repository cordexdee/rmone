using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public  class ModuleBudgetStore:StoreBase<ModuleBudget>, IModuleBudgetStore
    {
        public ModuleBudgetStore(CustomDbContext context):base(context)
        {


        }
        public ModuleBudget InsertORUpdateData(ModuleBudget objModuleBudget)
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
    public interface IModuleBudgetStore:IStore<ModuleBudget>
    {
        ModuleBudget InsertORUpdateData(ModuleBudget objModuleBudget);
    }
}
