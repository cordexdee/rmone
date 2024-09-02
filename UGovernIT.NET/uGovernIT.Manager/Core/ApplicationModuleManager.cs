using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ApplicationModuleManager : ManagerBase<ApplicationModule>, IApplicationModuleManager
    {
        public ApplicationModuleManager(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationModuleManager : IManagerBase<ApplicationModule>
    {

    }
}
