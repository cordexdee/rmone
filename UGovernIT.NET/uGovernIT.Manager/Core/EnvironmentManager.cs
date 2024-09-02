using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class EnvironmentManager : ManagerBase<UGITEnvironment>, IEnvironmentManager
    {
        public EnvironmentManager(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IEnvironmentManager : IManagerBase<UGITEnvironment>
    {

    }
}
