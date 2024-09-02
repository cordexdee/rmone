using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class WikiDiscussionManager : ManagerBase<WikiDiscussion>, IWikiDiscussionManager
    {
        public WikiDiscussionManager(ApplicationContext context) : base(context)
        {
            store = new WikiDiscussionStore(this.dbContext);
        }
    }

    public interface IWikiDiscussionManager : IManagerBase<WikiDiscussion>
    {

    }
}
