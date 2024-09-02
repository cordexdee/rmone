using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class UserProjectExperienceStore : StoreBase<UserProjectExperience>, IUserProjectExperienceStore
    {
        public UserProjectExperienceStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IUserProjectExperienceStore : IStore<UserProjectExperience>
    {
    }
}
