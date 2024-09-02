using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Managers
{
    public class DashboardSummaryManager : ManagerBase<DashboardSummary>, IDashboardSummaryManager
    {
        public DashboardSummaryManager(ApplicationContext context) : base(context)
        {
            store = new DashboardSummaryStore(this.dbContext);
        }
    }
    public interface IDashboardSummaryManager : IManagerBase<DashboardSummary>
    {

    }
}
