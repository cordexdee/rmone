using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiContentsStore : StoreBase<WikiContents>, IWikiContentsStore
    {
        public WikiContentsStore(CustomDbContext context) : base(context)
        {

        }

    }

    public interface IWikiContentsStore : IStore<WikiContents>
    {

    }

}
