using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.AspnetDMSUsers)]
    public class aspnet_User
    {
        [ForeignKey("aspnet_Application")]
        public Guid ApplicationId { get; set; }
        [JsonIgnore]
        public virtual DMSAspnetApplications aspnet_Application { get; set; }

        [Key]
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string LoweredUserName { get; set; }

        public string MobileAlias { get; set; }

        public bool IsAnonymous { get; set; }

        public DateTime LastActivityDate { get; set; }

        // public virtual ICollection<DMSDirectory> DMSDirectory { get; set; }
        //public virtual ICollection<Document> Document { get; set; }
        [NotMapped]
        public virtual ICollection<DMSDocument> Document { get; set; }
        
        public virtual ICollection<aspnet_UsersInRole> aspnet_UsersInRole { get; set; }
    }
}
