using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public  class SlaRuleStore:StoreBase<ModuleSLARule>,ISlaRuleStore
    {
        public SlaRuleStore(CustomDbContext context) : base(context)
        {

        }

    }
    public interface ISlaRuleStore : IStore<ModuleSLARule>
    {

    }
}
