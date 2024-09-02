using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;


namespace uGovernIT.Utility.Entities.DMSDB
{
    [Table(DatabaseObjects.Tables.DMSDirectory)]
    public class DMSDirectory
    {
        [Key]
        public int DirectoryId { get; set; }

        public string DirectoryName { get; set; }

        public int? DirectoryParentId { get; set; }

        [ForeignKey("AspNetUsers")]
        public string AuthorId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual aspnet_User aspnet_User { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool Deleted { get; set; }

        public string TenantID { get; set; }

        public virtual ICollection<DMSDocument> Document { get; set; }
        public string OwnerUser { get; set; }

        [NotMapped]
        public virtual ICollection<DMSDirectory> DirectoryList { get; set; }
    }
}
