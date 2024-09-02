using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class FunctionRoleStore : StoreBase<FunctionRole>, IFunctionRole
    {
        public FunctionRoleStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IFunctionRole : IStore<FunctionRole>
    {

    }
}
