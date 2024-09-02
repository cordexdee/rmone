using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class EmployeeTypeStore:StoreBase<EmployeeTypes>, IEmployeeTypeStore
    {
        public EmployeeTypeStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IEmployeeTypeStore:IStore<EmployeeTypes>
    {
    }
}
