using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class TaskEmailViewStore : StoreBase<ModuleTaskEmail>, ITaskEmailViewStore
    {
        public TaskEmailViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ITaskEmailViewStore : IStore<ModuleTaskEmail>
    {

    }
}
