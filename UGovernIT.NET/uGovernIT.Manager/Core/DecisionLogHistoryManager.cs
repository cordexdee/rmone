using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IDecisionLogHistoryManager : IManagerBase<DecisionLogHistory>
    {

    }

    public class DecisionLogHistoryManager:ManagerBase<DecisionLogHistory>,IDecisionLogHistoryManager
    {
        public DecisionLogHistoryManager(ApplicationContext context) : base(context)
        {
            store = new DecisionLogHistoryStore(this.dbContext);
        }
    }
}
