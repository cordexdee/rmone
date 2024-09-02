using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{

    public class ApplicationRoleManager : ManagerBase<ApplicationRole>, IApplicationRoleManager
    {
        public ApplicationRoleManager(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationRoleManager : IManagerBase<ApplicationRole>
    {

    }
}
