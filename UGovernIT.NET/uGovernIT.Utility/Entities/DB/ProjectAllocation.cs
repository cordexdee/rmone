using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectAllocations)]
    public class ProjectAllocation : DBBaseEntity
    {
        public long ID { get; set; }
        public string TicketID { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public double AllocationHour { get; set; }
        public double PctAllocation { get; set; }

        [NotMapped]
        public double TotalWorkingHour { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }

    }
}
