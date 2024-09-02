using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class WorkflowSLASummaryManager:ManagerBase<WorkflowSLASummary>, IWorkflowSLASummary
    {
        public WorkflowSLASummaryManager(ApplicationContext context):base(context)
        {
            store = new WorkflowSLASummaryStore(this.dbContext);
        }
    }
    public interface IWorkflowSLASummary : IManagerBase<WorkflowSLASummary>
    {
    }
}
