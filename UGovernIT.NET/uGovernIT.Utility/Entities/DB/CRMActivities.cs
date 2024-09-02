using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CRMActivities)]
    public class CRMActivities : DBBaseEntity
    {

        public long ID { get; set; }
        public string ActivityStatus { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string ContactLookup { get; set; }
        //public DateTime Created { get; set; }
        public string Description { get; set; }
        
        public DateTime DueDate { get; set; }
        public DateTime ? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
       // public DateTime Modified { get; set; }

        public int StageStep { get; set; }
        public string TaskID { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
       // public string Modified { get; set; }
       // public string ModifiedByUser { get; set; }
         //public bool IsDeleted { get; set; }


    }
}
