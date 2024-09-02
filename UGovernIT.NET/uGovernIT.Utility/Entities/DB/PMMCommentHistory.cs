using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.PMMCommentsHistory)]
    public class PMMCommentHistory:DBBaseEntity
    {
        public long ID { get; set; }

        public DateTime? AccomplishmentDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string TicketId { get; set; }

        public long PMMIdLookup { get; set; }

        public string ProjectNote { get; set; }

        public string ProjectNoteType { get; set; }

        public string Title { get; set; }

        //public bool IsDeleted { get; set; }

        public DateTime? BaselineDate { get; set; }

        public int BaselineId { get; set; }

        [NotMapped]
        public string PMMTitle { get; set; }

    }
}
