using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CompanyDivisionStore : StoreBase<CompanyDivision>, ICompanyDivisionStore
    {
        public CompanyDivisionStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICompanyDivisionStore : IStore<CompanyDivision>
    {

    }
}
