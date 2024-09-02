using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.RankingCriteriaMaster)]
    public class RankingCriterias : DBBaseEntity
    {
        public long ID { get; set; }
        public string RankingCriteria { get; set; }
        public string Description { get; set; }
        public int ItemOrder { get; set; }
        public int Ranking { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightedScore { get; set; }
    }
}
