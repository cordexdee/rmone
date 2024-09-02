using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    
    public class UserSkillManager : ManagerBase<UserSkills>, IUserSkillManager
    {
        public UserSkillManager(ApplicationContext context) : base(context)
        {
            store = new UserSkillStore(this.dbContext);
        }
        public long Save(UserSkills userSkills)
        {
            if (userSkills.ID > 0)
                this.Update(userSkills);
            else
                this.Insert(userSkills);
            return userSkills.ID;
        }
    }
    public interface IUserSkillManager : IManagerBase<UserSkills>
    {

    }
}
