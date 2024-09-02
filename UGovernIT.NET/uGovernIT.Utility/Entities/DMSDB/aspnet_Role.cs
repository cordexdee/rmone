using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.AspnetDMSRoles)]
    public class aspnet_Role
    {
        [ForeignKey("aspnet_Application")]
        public Guid ApplicationId { get; set; }
        [JsonIgnore]
        public virtual DMSAspnetApplications aspnet_Application { get; set; }

        [Key]
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public string LoweredRoleName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<aspnet_UsersInRole> aspnet_UsersInRole { get; set; }
    }
}