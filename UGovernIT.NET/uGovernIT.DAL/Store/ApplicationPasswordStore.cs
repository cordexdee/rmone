using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.DAL.Store
{
    public class ApplicationPasswordStore:StoreBase<ApplicationPasswordEntity>, IApplicationPasswordStore
    {
        public ApplicationPasswordStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IApplicationPasswordStore : IStore<ApplicationPasswordEntity>
    {

    }
}
