using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class BudgetActualsStore:StoreBase<BudgetActual>, IBudgetActualsStore
    {
        public BudgetActualsStore(CustomDbContext context) : base(context)
        {


        }
        public BudgetActual InsertOrUpdateData(BudgetActual objActuals)
        {
            if (objActuals.ID > 0)
            {
                Update(objActuals);
            }
            else
            {
                Insert(objActuals);
            }
            return objActuals;
        }
    }
    public interface IBudgetActualsStore:IStore<BudgetActual>
    {
        BudgetActual InsertOrUpdateData(BudgetActual objActuals);

    }
}
