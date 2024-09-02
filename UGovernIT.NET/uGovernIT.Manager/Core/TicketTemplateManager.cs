using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class TicketTemplateManager : ManagerBase<TicketTemplate> , ITicketTemplateManager
    {
        public TicketTemplateManager(ApplicationContext context) : base(context)
        {
            store = new TicketTemplateStore(this.dbContext);
        }
    }
    public interface ITicketTemplateManager : IManagerBase<TicketTemplate> { }
}
