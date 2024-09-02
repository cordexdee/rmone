using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class RankingCriteriaStore : StoreBase<RankingCriterias>, IRankingCriterias
    {
        public RankingCriteriaStore(CustomDbContext context) : base(context)
        {
        }

    }

    public interface IRankingCriterias : IStore<RankingCriterias>
    {
    }
}
