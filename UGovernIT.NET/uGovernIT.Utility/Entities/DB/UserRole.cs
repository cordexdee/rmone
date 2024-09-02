using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.AspNetUserRoles)]
    public class UserRole:DBBaseEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string Title { get; set; }       
    }
}
