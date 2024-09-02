using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class ProjectAllocationTemplateManager : ManagerBase<ProjectAllocationTemplate>, IProjectAllocationTemplateManager
    {
        public ProjectAllocationTemplateManager(ApplicationContext context) : base(context)
        {
            store = new ProjectAllocationTemplateStore(this.dbContext);
        }
    }

    public interface IProjectAllocationTemplateManager : IManagerBase<ProjectAllocationTemplate>
    {

    }
}
