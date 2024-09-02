using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IProjectSimilarityMetricsManager : IManagerBase<ProjectSimilarityMetrics>
    {
    }

    public class ProjectSimilarityMetricsManager : ManagerBase<ProjectSimilarityMetrics>, IProjectSimilarityMetricsManager
    {
        public ProjectSimilarityMetricsManager(ApplicationContext context) : base(context)
        {
            store = new ProjectSimilarityMetricsStore(this.dbContext);
        }
        
    }
}
