using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.LeadCriteria)]
    public class LeadCriteria : DBBaseEntity
    {
        public long ID { get; set; }
        public string Priority { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }                
    }
}