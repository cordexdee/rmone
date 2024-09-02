using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class TicketTemplateStore : StoreBase<TicketTemplate> , ITicketTemplateStore
    {
        public TicketTemplateStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ITicketTemplateStore : IStore<TicketTemplate>
    {

    }
}
