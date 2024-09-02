using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class UserSkillStore:StoreBase<UserSkills>, IUserSkillStore
    {
        public UserSkillStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IUserSkillStore : IStore<UserSkills>
    {
    }
}
