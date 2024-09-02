using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class RequestTypeStore : StoreBase<ModuleRequestType>, IRequestTypeStore
    {
        public RequestTypeStore(CustomDbContext context) : base(context)
        {

        }

    }
    public interface IRequestTypeStore : IStore<ModuleRequestType>
    {

    }
}
