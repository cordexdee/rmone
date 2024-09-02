using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CheckListTemplates)]
    public class CheckListTemplates : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public bool AutoLoadChecklist { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }        
    }
}