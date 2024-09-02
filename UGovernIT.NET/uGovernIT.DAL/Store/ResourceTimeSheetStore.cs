using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceTimeSheetStore : StoreBase<ResourceTimeSheet>, IResourceTimeSheetStore
    {
        public ResourceTimeSheetStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceTimeSheetStore : IStore<ResourceTimeSheet>
    {
    }
}
