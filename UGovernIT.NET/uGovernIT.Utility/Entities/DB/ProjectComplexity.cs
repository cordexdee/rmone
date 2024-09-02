using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectComplexity)]
    public class ProjectComplexity : DBBaseEntity
    {
        public long ID { get; set; }
        [Column("CRMProjectComplexityChoice")]
        public string CRMProjectComplexity { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}