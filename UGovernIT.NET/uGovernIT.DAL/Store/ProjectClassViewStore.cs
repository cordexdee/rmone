using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ProjectClassViewStore : StoreBase<ProjectClass>, IProjectClassViewStore
    {
        public ProjectClassViewStore(CustomDbContext context) : base(context)
        {

        }
        
    }
    public interface IProjectClassViewStore : IStore<ProjectClass>
    { }
}
