using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace uGovernIT.Utility
{
    [Serializable]
    public class UserInfo
    {
        public string UserName { get; set; }
        public string  ID { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public bool IsGroup
        {
            get; set;
        }

        public UserInfo(string id, string name)
        {
            UserName = name;
            ID = id;
            Title = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            LoginName = string.Empty;
            Name = name;
        }

    }

    public class UserSkill
    {

        public int id { get; set; }

        public string name { get; set; }

    }



}
