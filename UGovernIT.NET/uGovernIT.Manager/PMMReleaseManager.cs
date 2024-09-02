using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class PMMReleaseManager : ManagerBase<ProjectReleases>, IPMMReleaseManager
    {
        public PMMReleaseManager(ApplicationContext context) : base(context)
        {
            store = new PMMReleaseStore(this.dbContext);
        }
    }
    public interface IPMMReleaseManager : IManagerBase<ProjectReleases>
    {

    }
}
