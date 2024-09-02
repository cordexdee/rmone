using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceWorkItemsStore : StoreBase<ResourceWorkItems>, IResourceWorkItemsStore
    {
        public ResourceWorkItemsStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceWorkItemsStore : IStore<ResourceWorkItems>
    {
    }
}
