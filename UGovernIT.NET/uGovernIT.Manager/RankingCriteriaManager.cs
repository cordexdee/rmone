using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public  class RankingCriteriaManager : ManagerBase<RankingCriterias>, IRankingCriterias
    {
        public RankingCriteriaManager(ApplicationContext context) : base(context)
        {
            store = new RankingCriteriaStore(this.dbContext);
        }
    }

    public interface IRankingCriterias : IManagerBase<RankingCriterias>
    {
    }
}
