using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class DecisionLogManager : ManagerBase<DecisionLog>, IDecisionLogManager
    {
        public DecisionLogManager(ApplicationContext context):base(context)
        {
            store = new DecisionLogStore(this.dbContext);
        }
    }
    public interface IDecisionLogManager : IManagerBase<DecisionLog>
    {
    }
}
