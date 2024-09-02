using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class DashboardFactTablesStore : StoreBase<DashboardFactTables>, IDashboardFactTablesStore
    {
        public DashboardFactTablesStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface IDashboardFactTablesStore : IStore<DashboardFactTables>
    {

    }
}
