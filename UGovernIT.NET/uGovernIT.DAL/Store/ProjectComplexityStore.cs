using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ProjectComplexityStore : StoreBase<ProjectComplexity>, IProjectComplexity
    {
        public ProjectComplexityStore(CustomDbContext context) : base(context)
        {
        }
    }

    public interface IProjectComplexity : IStore<ProjectComplexity>
    {
    }
}