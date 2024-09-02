using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class LinkViewStore : StoreBase<LinkView>, ILinkViewStore
    {
        public LinkViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ILinkViewStore : IStore<LinkView>
    {

    }
}
