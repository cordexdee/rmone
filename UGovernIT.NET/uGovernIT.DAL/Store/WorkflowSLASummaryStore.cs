using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class WorkflowSLASummaryStore:StoreBase<WorkflowSLASummary>, IWorkflowSLASummaryStore
    {
        public WorkflowSLASummaryStore(CustomDbContext context):base(context)
        {
        }

    }
    public interface IWorkflowSLASummaryStore:IStore<WorkflowSLASummary>
    {

    }

}
