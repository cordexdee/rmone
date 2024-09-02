using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ProjectComplexityManager : ManagerBase<ProjectComplexity>, IProjectComplexity
    {
        public ProjectComplexityManager(ApplicationContext context) : base(context)
        {
            store = new ProjectComplexityStore(this.dbContext);
        }
    }

    public interface IProjectComplexity : IManagerBase<ProjectComplexity>
    {
    }
}