using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IProjectSimilarityConfigStore : IStore<ProjectSimilarityConfig>
    {
    }

    public class ProjectSimilarityConfigStore : StoreBase<ProjectSimilarityConfig>, IProjectSimilarityConfigStore
    {
        public ProjectSimilarityConfigStore(CustomDbContext context) : base(context)
        {
        }
    }
}
