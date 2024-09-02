using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    [Table(DatabaseObjects.Tables.DMSTenantDocumentsDetails)]
    public class DMSTenantDocumentsDetails
    {
        [Key]
        public long ID { get; set; }

        public string TenantID { get; set; }

        public string AppID { get; set; }

        public string AppSecret { get; set; }

        public string AWSBucket { get; set; }

        public string AWSProfileName { get; set; }

        public string AWSAccessKey { get; set; }

        public string AWSSecretKey { get; set; }

        public bool Is_Deleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}