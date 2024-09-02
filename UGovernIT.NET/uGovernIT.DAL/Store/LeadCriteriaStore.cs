using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class LeadCriteriaStore : StoreBase<LeadCriteria>, ILeadCriteria
    {
        public LeadCriteriaStore(CustomDbContext context) : base(context)
        {
        }
    }

    public interface ILeadCriteria : IStore<LeadCriteria>
    {
    }
}