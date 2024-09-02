using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class UserLookupValue
    {
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ID { get; set; }
        public UserLookupValue()
        {
            LoginName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
        }
        public UserLookupValue( int id)
        {
            LoginName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            //SPFieldUserValue val = new SPFieldUserValue(spWeb, id.ToString());
            //if (val.LookupId > 0 && val.User != null)
            //{
            //    LoginName = val.User.LoginName;
            //    Name = val.User.Name;
            //    Email = val.User.Email;
            //    ID = id;
            //}
        }
        public UserLookupValue(string fieldValue)
        {
            LoginName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;

            //SPFieldUserValue val = new SPFieldUserValue(spWeb, fieldValue);
            //if (val.LookupId > 0 && val.User != null)
            //{
            //    LoginName = val.User.LoginName;
            //    Name = val.User.Name;
            //    Email = val.User.Email;
            //    ID = val.User.ID;
            //}
        }
    }
}
