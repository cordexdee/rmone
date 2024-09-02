using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DAL.Store
{
    public class RequestRoleWriteAccessStore : StoreBase<ModuleRoleWriteAccess>, IRequestRoleWriteAccessStore
    {
        public RequestRoleWriteAccessStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IRequestRoleWriteAccessStore : IStore<ModuleRoleWriteAccess>
    {

    }
}
