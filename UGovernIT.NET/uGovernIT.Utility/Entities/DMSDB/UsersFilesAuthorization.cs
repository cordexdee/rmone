using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.DMSUsersFilesAuthorization)]
    public class DMSUsersFilesAuthorization
    {
        [Key]
        public int UsersFilesAuthorizationId { get; set; }
        
        [ForeignKey("Access")]
        public int AccessId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual DMSAccess Access { get; set; }

     
       
        public int ? FileId { get; set; }
     
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public virtual DMSCustomer Customer { get; set; }

        public int? DirectoryId { get; set; }

        [ForeignKey("UserProfile")]
        public string UserId { get; set; }

        //[ForeignKey("UserProfile")]
        //public string Id { get; set; }

        //[JsonIgnore]
        //[NotMapped]
        public virtual UserProfile UserProfile { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime ? CreatedOn { get; set; }

        public DateTime ? UpdatedOn { get; set; }

        public string TenantID { get; set; }
    }
}