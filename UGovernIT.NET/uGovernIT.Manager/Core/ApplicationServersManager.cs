using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
    public class ApplicationServersManager : ManagerBase<ApplicationServer>, IApplicationServersManager
    {
        public ApplicationServersManager(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationServersManager : IManagerBase<ApplicationServer>
    {

    }
}
