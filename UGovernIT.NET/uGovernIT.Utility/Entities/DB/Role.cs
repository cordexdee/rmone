using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.AspNetRoles)]
    public class Role : DBBaseEntity,IRole
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
        public string Title { get; set; }
        public string LandingPage { get; set; }
        public RoleType RoleType { get; set; }
      
        public Role()
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
