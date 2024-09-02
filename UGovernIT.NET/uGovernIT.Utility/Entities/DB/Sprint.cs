using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.Sprint)]
    public class Sprint:DBBaseEntity
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ItemOrder { get; set; }
        public double? PercentComplete { get; set; }
        public long PMMIdLookup { get; set; }
        public double? RemainingHours { get; set; }
        public DateTime? StartDate { get; set; }
        public double? TaskEstimatedHours { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
        [NotMapped]
        public string PMMTitle { get; set; }
    }

}
