using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class DepartmentStore : StoreBase<Department>, IDepartmentStore
    {
        public DepartmentStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDepartmentStore : IStore<Department>
    {

    }
}
