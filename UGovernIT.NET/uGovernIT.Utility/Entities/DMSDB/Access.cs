using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    [Table(DatabaseObjects.Tables.DMSAccess)]
    public class DMSAccess
    {
        [Key]
        public int AccessId { get; set; }

        public string AccessType { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime ? CreatedOn { get; set; }

        public DateTime ? UpdatedOn { get; set; }

        public virtual ICollection<DMSUsersFilesAuthorization> UsersFilesAuthorization { get; set; }
    }
}