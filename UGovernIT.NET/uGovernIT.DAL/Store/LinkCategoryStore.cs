using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class LinkCategoryStore : StoreBase<LinkCategory>, ILinkCategoryStore
    {
        public LinkCategoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ILinkCategoryStore : IStore<LinkCategory>
    {

    }
}
