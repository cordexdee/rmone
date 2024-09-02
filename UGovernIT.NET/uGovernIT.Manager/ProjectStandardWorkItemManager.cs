using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class ProjectStandardWorkItemManager : ManagerBase<ProjectStandardWorkItem>, IProjectStandardWorkItemManager
    {
        public ProjectStandardWorkItemManager(ApplicationContext context) : base(context)
        {

            store = new ProjectStandardWorkItemStore(this.dbContext);
        }
    }

    public interface IProjectStandardWorkItemManager : IManagerBase<ProjectStandardWorkItem>
    {

    }

}
