using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class TicketRelationStore : StoreBase<TicketRelation>, ITicketRelationStore
    {
        public TicketRelationStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface ITicketRelationStore : IStore<TicketRelation>
    {
        
    }
}
