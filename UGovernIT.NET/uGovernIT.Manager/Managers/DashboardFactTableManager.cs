using System.Collections.Generic;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
namespace uGovernIT.Manager.Managers
{
    public class DashboardFactTableManager : ManagerBase<DashboardFactTables>, IDashboardFactTableManager
    {
        //ApplicationContext _context;
        public DashboardFactTableManager(ApplicationContext context) : base(context)
        {
            store = new DashboardFactTablesStore(this.dbContext);
        }
        public void RefreshCache()
        {
            List<DashboardFactTables> tables = this.Load();
            if (tables == null || tables.Count == 0)
                return;

            Task.Run(async () =>
            {
                await Task.FromResult(0);
                foreach (var row in tables)
                {
                    if (row.CacheTable)
                        DashboardCache.RefreshDashboardCache(row.Title, this.dbContext);
                }
            });
        }
    }
    public interface IDashboardFactTableManager : IManagerBase<DashboardFactTables>
    {

    }
}
