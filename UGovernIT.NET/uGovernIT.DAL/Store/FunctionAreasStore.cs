using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class FunctionAreasStore:StoreBase<FunctionalArea>, IFunctionAreasStore
    {
        public FunctionAreasStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IFunctionAreasStore: IStore<FunctionalArea>
    {

    }
}
