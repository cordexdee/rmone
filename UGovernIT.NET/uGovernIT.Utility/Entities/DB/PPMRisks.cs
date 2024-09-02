using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.PMMRisks)]
    public class PMMRisks:DBBaseEntity
    {
        public long ID { get; set; }
        public string AssignedTo { get; set; }
        public string ContingencyPlan { get; set; }
        public string Description { get; set; }
        //public bool? IsDeleted { get; set; }
        public string IssueImpact { get; set; }
        public string MitigationPlan { get; set; }
        public long PMMIdLookup { get; set; }
        public int? RiskProbability { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
    }
}
