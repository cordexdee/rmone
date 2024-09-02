using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceTimeSheetSignOff)]
    public class ResourceTimeSheetSignOff : DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SignOffStatus { get; set; }

        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        public string Title { get; set; }
        public string History { get; set; }
    }
}
