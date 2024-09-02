using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class WikiLinksStore : StoreBase<WikiLinks>, IWikiLinksStore
    {
        public WikiLinksStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IWikiLinksStore :IStore<WikiLinks>
    {

    }
}
