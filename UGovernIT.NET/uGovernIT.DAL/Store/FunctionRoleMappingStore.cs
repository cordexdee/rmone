using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class FunctionRoleMappingStore : StoreBase<FunctionRoleMapping>, IFunctionRoleMappingStore
    {
        public FunctionRoleMappingStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IFunctionRoleMappingStore : IStore<FunctionRoleMapping>
    {

    }
}
