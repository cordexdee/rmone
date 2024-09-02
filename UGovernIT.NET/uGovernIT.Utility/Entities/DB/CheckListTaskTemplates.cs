using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckListTaskTemplates)]
    public class CheckListTaskTemplates : DBBaseEntity
    {
        public long ID { get; set; }
        public long CheckListTemplateLookup { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
    }
}