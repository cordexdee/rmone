using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class WikiMenuLeftNavigationManager :ManagerBase<WikiCategory>, IWikiMenuLeftNavigationManager
    {
        public WikiMenuLeftNavigationManager(ApplicationContext context) : base(context)
        {
            store = new WikiMenuLeftNavigationStore(this.dbContext);
        }

    }

    public interface IWikiMenuLeftNavigationManager : IManagerBase<WikiCategory>
    {

    }

}
