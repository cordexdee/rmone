using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class UserValue
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public  UserProfile userProfile { get; set; }

        public UserValue()
        {
        }
        public UserValue(ApplicationContext context,string Value, bool includeDetail = false)
        {
            ID = Value;
            if (includeDetail)
            {
               userProfile = context.UserManager.GetUserInfoById(ID);
                if (userProfile != null)
                {
                    Value = userProfile.UserName;
 
                }
            }
        }
    }

}
