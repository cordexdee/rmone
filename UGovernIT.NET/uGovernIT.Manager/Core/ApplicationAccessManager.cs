using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ApplicationAccessManager : ManagerBase<ApplicationAccess>, IApplicationAccessManager
    {
        public ApplicationAccessManager(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationAccessManager : IManagerBase<ApplicationAccess>
    {

    }
}
