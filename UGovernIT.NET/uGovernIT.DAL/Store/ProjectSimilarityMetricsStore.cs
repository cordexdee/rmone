using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IProjectSimilarityMetricsStore : IStore<ProjectSimilarityMetrics>
    {
    }

    public class ProjectSimilarityMetricsStore : StoreBase<ProjectSimilarityMetrics>, IProjectSimilarityMetricsStore
    {
        public ProjectSimilarityMetricsStore(CustomDbContext context) : base(context)
        {
        }
    }
}
