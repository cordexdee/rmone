using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.DMSCustomer)]
    public class DMSCustomer
    {
        [Key]
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime ? CreatedOn { get; set; }

        public DateTime ? UpdatedOn { get; set; }

        public virtual ICollection<DMSDocument> Document { get; set; }
        public virtual ICollection<DMSUsersFilesAuthorization> UsersFilesAuthorization { get; set; }
    }
}