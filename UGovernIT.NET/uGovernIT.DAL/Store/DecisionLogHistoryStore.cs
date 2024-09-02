using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public interface IDecisionLogHistoryStore:IStore<DecisionLogHistory>
    {

    }

    public class DecisionLogHistoryStore:StoreBase<DecisionLogHistory>
    {
        public DecisionLogHistoryStore(CustomDbContext context) : base(context)
        {

        }

    }
}
