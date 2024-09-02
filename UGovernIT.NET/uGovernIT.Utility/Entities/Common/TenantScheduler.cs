using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    [Table("TenantScheduler")]
    public class TenantScheduler : DBBaseEntity
    {
        public int Id { get; set; }
        public string JobType { get; set; }
        public string CronExpression { get; set; }
    }

}
