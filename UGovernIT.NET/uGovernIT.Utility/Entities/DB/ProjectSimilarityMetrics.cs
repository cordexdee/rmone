using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectSimilarityMetrics)]
    public class ProjectSimilarityMetrics : DBBaseEntity
    {
        public long Id { get; set; }
        public string ModuleNameLookup { get; set; }
        public string SearchColumns { get; set; }
    }
}
