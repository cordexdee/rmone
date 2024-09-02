using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.UsersRoles)]
    public class UserRoles : DBBaseEntity,IRole
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Discriminator { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public bool IsSystem { get; set; }
        [NotMapped]
        public string Title { get; set; }
        public string LandingPage { get; set; }
        [NotMapped]
        public RoleType RoleType { get; set; }
      
        public UserRoles()
        {
            Id = Guid.NewGuid().ToString();
            Description = string.Empty;
            Discriminator = string.Empty;
            Title = string.Empty;
            Name = string.Empty;
            LandingPage = string.Empty;
        }
    }
}
