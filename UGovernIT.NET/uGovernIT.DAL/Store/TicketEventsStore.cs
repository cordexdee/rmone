using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
//Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
namespace uGovernIT.DAL.Store
{
    public class TicketEventsStore : StoreBase<TicketEvents>, ITicketEventsStore
    {
        public TicketEventsStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface ITicketEventsStore : IStore<TicketEvents>
    {

    }
}
