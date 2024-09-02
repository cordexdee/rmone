using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.AspnetDMSUsersInRoles)]
    public class aspnet_UsersInRole
    {
        [Key]
        public Guid UserRoleId { get; set; }

        [ForeignKey("aspnet_User")]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual aspnet_User aspnet_User { get; set; }
               
        [ForeignKey("aspnet_Role")]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public virtual aspnet_Role aspnet_Role { get; set; }

        public Guid CreatedByUser { get; set; }

        public Guid UpdatedBy { get; set; }

        public DateTime ? CreatedOn { get; set; }

        public DateTime ? UpdatedOn { get; set; }
    }
}