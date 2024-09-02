using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.DMSAspnetApplications)]
    public class DMSAspnetApplications
    {
        public string ApplicationName { get; set; }

        public string LoweredApplicationName { get; set; }

        [Key]
        public  Guid ApplicationId { get; set; }

        public string Description { get; set; }

        public virtual ICollection<aspnet_Role> Role { get; set; }
        public virtual ICollection<aspnet_User> User { get; set; }
    }
}