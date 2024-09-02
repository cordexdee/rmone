using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ResourceProjectComplexityManager : ManagerBase<SummaryResourceProjectComplexity>, IResourceProjectComplexityManager
    {
        public ResourceProjectComplexityManager(ApplicationContext context) : base(context)
        {
            store = new ResourceProjectComplexityStore(this.dbContext);
        }
    }
    public interface IResourceProjectComplexityManager : IManagerBase<SummaryResourceProjectComplexity>
    {

    }
}
