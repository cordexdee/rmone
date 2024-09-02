using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DashboardStore : StoreBase<Dashboard>, IDashboardStore
    {
        public DashboardStore(CustomDbContext context) : base(context)
        {
        }
        public Dashboard InsertORUpdateData(Dashboard objUDashboard)
        {
            if (objUDashboard.ID > 0)
            {
                Update(objUDashboard);
            }
            else
            {
                Insert(objUDashboard);

            }
            return objUDashboard;
        }
    }
    public interface IDashboardStore : IStore<Dashboard>
    { }
}
