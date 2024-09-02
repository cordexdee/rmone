using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public  class AgentsStore:StoreBase<Agents>
    {
        public AgentsStore(CustomDbContext context) :base(context)
        {

        }

    }
}
