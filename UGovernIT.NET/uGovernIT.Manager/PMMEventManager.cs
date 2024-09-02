using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class PMMEventManager : ManagerBase<PMMEvents>, IPMMEventManager
    {
        public PMMEventManager(ApplicationContext context) : base(context)
        {
            store = new PMMEventStore(this.dbContext);
        }
    }
    public interface IPMMEventManager : IManagerBase<PMMEvents>
    {

    }
}
