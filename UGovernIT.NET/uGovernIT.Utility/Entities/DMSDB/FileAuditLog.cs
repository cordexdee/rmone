using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DMSDB
{
    
    [Table(DatabaseObjects.Tables.DMSFileAuditLog)]
    public class DMSFileAuditLog
    {
        [Key]
        public int FileAuditLogId { get; set; }
        public int FileId { get; set; }
        public string UserId { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}