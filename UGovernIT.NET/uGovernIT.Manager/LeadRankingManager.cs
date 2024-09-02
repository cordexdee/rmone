using uGovernIT.DAL.Store;
using uGovernIT.Utility;


namespace uGovernIT.Manager
{
    public class LeadRankingManager : ManagerBase<LeadRanking>, ILeadRanking
    {
        public LeadRankingManager(ApplicationContext context) : base(context)
        {
            store = new LeadRankingStore(this.dbContext);
        }
    }

    public interface ILeadRanking : IManagerBase<LeadRanking>
    {
    }
}
