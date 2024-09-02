using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.AspNetUserLogins)]
    public class UserLogin
    {
        
        public string LoginProvider { get; set; }
       
        public string ProviderKey { get; set; }
        
        public string UserId { get; set; }
        public string Title { get; set; }
    }
}
