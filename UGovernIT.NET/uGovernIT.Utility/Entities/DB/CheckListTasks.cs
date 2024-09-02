using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckListTasks)]
    public class CheckListTasks : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public long CheckListLookup { get; set; }
        public string Module { get; set; }        
        public string TicketId { get; set; }                
    }
}