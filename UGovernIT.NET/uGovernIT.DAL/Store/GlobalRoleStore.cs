using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class GlobalRoleStore : StoreBase<GlobalRole>, IRolesStore
    {
        public GlobalRoleStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IRolesStore : IStore<GlobalRole>
    {

    }
}
