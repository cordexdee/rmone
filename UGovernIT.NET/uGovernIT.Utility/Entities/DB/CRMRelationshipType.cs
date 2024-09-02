using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.CRMRelationshipType)]
    public class CRMRelationshipType : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
