using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DashboardSummaryStore : StoreBase<DashboardSummary>, IDashboardSummaryStore
    {
        public DashboardSummaryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDashboardSummaryStore : IStore<DashboardSummary>
    {

    }
}
