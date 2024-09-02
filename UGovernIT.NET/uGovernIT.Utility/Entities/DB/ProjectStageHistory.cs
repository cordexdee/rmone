using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectStageHistory)]
    public class ProjectStageHistory : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string TaskID { get; set; }
        public string TicketId { get; set; } 
        public int StageStep { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
