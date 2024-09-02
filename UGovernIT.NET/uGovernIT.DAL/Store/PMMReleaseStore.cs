using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class PMMReleaseStore:StoreBase<ProjectReleases>, IPMMReleaseStore
    {
        public PMMReleaseStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IPMMReleaseStore : IStore<ProjectReleases>
    {

    }
}
