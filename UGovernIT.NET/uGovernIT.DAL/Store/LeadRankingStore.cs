using uGovernIT.Utility;


namespace uGovernIT.DAL.Store
{
    public class LeadRankingStore : StoreBase<LeadRanking>, ILeadRanking
    {
        public LeadRankingStore(CustomDbContext context) : base(context)
        {
        }

    }

    public interface ILeadRanking : IStore<LeadRanking>
    {
    }
}
