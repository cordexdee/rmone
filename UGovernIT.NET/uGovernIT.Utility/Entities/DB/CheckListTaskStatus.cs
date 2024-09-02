using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckListTaskStatus)]
    public class CheckListTaskStatus : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public long CheckListLookup { get; set; }
        public long CheckListRoleLookup { get; set; }
        public long CheckListTaskLookup { get; set; }
        public string Module { get; set; }
        public string TicketId { get; set; }
        public string UGITCheckListTaskStatus { get; set; }
    }
}