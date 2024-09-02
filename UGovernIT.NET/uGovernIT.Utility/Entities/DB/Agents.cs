using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.Agents)]
    public class Agents:DBBaseEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public string Control { get; set; }
        public string Url { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public long? ServiceLookUp { get; set; }        
    }
}
