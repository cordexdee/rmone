using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   public class ProjectClassViewManager:ManagerBase<ProjectClass>, IProjectClassViewManager
    {
        public ProjectClassViewManager(ApplicationContext context):base(context)
        {
            store = new ProjectClassViewStore(this.dbContext);
        }
    }
    public interface IProjectClassViewManager : IManagerBase<ProjectClass>
    {

    }
}
