using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiDiscussionStore : StoreBase<WikiDiscussion>, IWikiDiscussionStore
    {
        public WikiDiscussionStore(CustomDbContext context) : base(context)
        {

        }

    }

    public interface IWikiDiscussionStore :IStore<WikiDiscussion>
    {

    }
}
