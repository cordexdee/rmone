using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class WikiContentsManager : ManagerBase<WikiContents>, IWikiContentsManager
    {
        public WikiContentsManager(ApplicationContext context) : base(context)
        {
            store = new WikiContentsStore(this.dbContext);
        }

    }

    public interface IWikiContentsManager :IManagerBase<WikiContents>
    {

    }
}
