using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class UserInfoRequestModel
    {
        public List<UserData> UserData { get; set; }
    }

    public class UserData
    {
        public string username { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }

}
