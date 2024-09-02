using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectSimilarityConfig)]
    public class ProjectSimilarityConfig : DBBaseEntity
    {
        public long ID { get; set; }        
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string Title { get; set; }
        public int StageWeight { get; set; }
        public string ModuleNameLookup { get; set; }
        public int Weight { get; set; }
        [NotMapped]
        public double NormalizeWeight { get; set; }
        [NotMapped]
        public double WeightedScore { get; set; }
        [NotMapped]
        public double NormalizedScore { get; set; }
        public string MetricType { get; set; }
    }
}
