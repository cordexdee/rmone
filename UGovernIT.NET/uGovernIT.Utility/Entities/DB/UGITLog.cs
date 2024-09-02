using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.UGITLog)]
    public class UGITLog : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Severity { get; set; }
        public string ItemUser { get; set; }
        public string ModuleNameLookup { get; set; }
        public string TicketId { get; set; }
        public string Description { get; set; }
    }
}