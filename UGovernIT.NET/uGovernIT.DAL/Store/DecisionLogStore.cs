using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class DecisionLogStore : StoreBase<DecisionLog>, IDecisionLogStore
    {
        public DecisionLogStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDecisionLogStore : IStore<DecisionLog>
    {

    }
}
