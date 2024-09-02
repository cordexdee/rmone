using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class FunctionRoleManager : ManagerBase<FunctionRole>, IFunctionRoleManager
    {
        public FunctionRoleManager(ApplicationContext context) : base(context)
        {
            store = new FunctionRoleStore(this.dbContext);
        }
    }

    public interface IFunctionRoleManager : IManagerBase<FunctionRole> { }
}
