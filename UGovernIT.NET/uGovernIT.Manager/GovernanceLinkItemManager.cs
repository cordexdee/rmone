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
    
    public class GovernanceLinkItemManager : ManagerBase<GovernanceLinkItem>, IGovernanceLinkItemManager
    {
        public GovernanceLinkItemManager(ApplicationContext context) : base(context)
        {
            store = new GovernanceLinkItemStore(this.dbContext);
        }
    }
    public interface IGovernanceLinkItemManager : IManagerBase<GovernanceLinkItem>
    {

    }
}
