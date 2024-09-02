using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketHours)]
    public class ActualHour : DBBaseEntity
    {
        public long ID { get; set; }
        public string TicketID { get; set; }
        public int StageStep { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        public DateTime WorkDate { get; set; } = new DateTime(1800, 1, 1);
        public double HoursTaken { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public string ModuleNameLookup { get; set; }
        public long TaskID { get; set; }
        public string WorkItem { get; set; }
        public string SubWorkItem { get; set; }
        public bool StandardWorkItem { get; set; }
        public DateTime WeekStartDate { get; set; } = new DateTime(1800, 1, 1);
        public DateTime MonthStartDate { get; set; } = new DateTime(1800, 1, 1);
        //public string TenantID { get; set; }
    }
}
