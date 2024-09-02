using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.DMSDocument)]
    public class DMSDocument
    {
        [Key]
        public int FileId { get; set; }
        
        public string FileName { get; set; }

        public string FullPath { get; set; }

        public string StoredFileName { get; set; }

        public int Size { get; set; }

        public string Version { get; set; }

        public int MainVersionFileId { get; set; }

        public bool IsCheckedOut { get; set; }

        [ForeignKey("DMSDirectory")]
        public int  DirectoryId { get; set; }
        [JsonIgnore]
       // public virtual DMSDirectory DMSDirectory { get; set; }

        [ForeignKey("AspNetUsers")]
        public string AuthorId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual aspnet_User Id { get; set; }
                
        [ForeignKey("DMSCustomer")]
        public int  CustomerId { get; set; }
        [JsonIgnore]
        public virtual DMSCustomer Customer { get; set; }

        public int ? FileParentId { get; set; }

        public bool Deleted { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime ? CreatedOn { get; set; }

        public DateTime ? UpdatedOn { get; set; }

        public string CheckOutBy { get; set; }

        public bool? ReviewRequired { get; set; }

        public double? ReviewStep { get; set; }

        public string Title { get; set; }

        public string Tags { get; set; }

        public string DocumentControlID { get; set; }

        public string TenantID { get; set; }
    }
}