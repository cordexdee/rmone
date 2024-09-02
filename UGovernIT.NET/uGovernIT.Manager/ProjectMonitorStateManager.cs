using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ProjectMonitorStateManager : ManagerBase<ProjectMonitorState>, IProjectMonitorStateManager
    {
        public ProjectMonitorStateManager(ApplicationContext context) : base(context)
        {
            store = new ProjectMonitorStateStore(this.dbContext);
        }
    }
    public interface IProjectMonitorStateManager : IManagerBase<ProjectMonitorState>
    {

    }
}
