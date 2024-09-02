using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class SprintSummaryManager : ManagerBase<SprintSummary>, ISprintSummaryManager
    {
        public SprintSummaryManager(ApplicationContext context) : base(context)
        {
            store = new SprintSummaryStore(this.dbContext);
        }
    }
    public interface ISprintSummaryManager : IManagerBase<SprintSummary>
    {

    }
}
