using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class CompanyStore : StoreBase<Company>, ICompanyStore
    {
        public CompanyStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICompanyStore : IStore<Company>
    {

    }
}
