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
    public class GovernanceLinkCategoryManager : ManagerBase<GovernanceLinkCategory>, IGovernanceLinkCategoryManager
    {
        public GovernanceLinkCategoryManager(ApplicationContext context) : base(context)
        {
            store = new GovernanceLinkCategoryStore(this.dbContext);
        }
    }
    public interface IGovernanceLinkCategoryManager : IManagerBase<GovernanceLinkCategory>
    {

    }
}
