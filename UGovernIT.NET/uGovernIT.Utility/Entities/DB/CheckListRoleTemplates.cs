using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckListRoleTemplates)]
    public class CheckListRoleTemplates : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public long CheckListTemplateLookup { get; set; }
        public string Module { get; set; }
        public string EmailAddress { get; set; }
        public string TicketId { get; set; }        
        public string Type { get; set; }
    }
}