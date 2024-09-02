using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class LeadCriteriaManager : ManagerBase<LeadCriteria>, ILeadCriteria
    {
        public LeadCriteriaManager(ApplicationContext context) : base(context)
        {
            store = new LeadCriteriaStore(this.dbContext);
        }
    }

    public interface ILeadCriteria : IManagerBase<LeadCriteria>
    {
    }
}