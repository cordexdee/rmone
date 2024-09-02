using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckLists)]
    public class CheckLists : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string CheckListName { get; set; }
        public long CheckListTemplateLookup { get; set; }
        public string Module { get; set; }
        public string TicketId { get; set; }
        public string Type { get; set; }
    }
}