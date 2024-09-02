using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ProjectInitiativeViewStore : StoreBase<ProjectInitiative>, IProjectInitiativeViewStore
    {
        public ProjectInitiativeViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IProjectInitiativeViewStore : IStore<ProjectInitiative>
    {

    }
}
