using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.PMMEvents)]
    public class PMMEvents:DBBaseEntity
    {
        public long ID { get; set; }
        [Column(DatabaseObjects.Columns.CategoryChoice)]
        public string Category { get; set; }
        public string Comments { get; set; }
        public string Duration { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? EventCanceled { get; set; }
        public string EventType { get; set; }
        //public bool fAllDayEvent { get; set; }
        public string fAllDayEvent { get; set; }
        public string fRecurrence { get; set; }
        //public bool? IsDeleted { get; set; }
        public string Location { get; set; }
        public string MasterSeriesItemID { get; set; }
        public string PMMIdLookup { get; set; }
        public string RecurrenceData { get; set; }
        public DateTime? RecurrenceID { get; set; }
        public string RecurrenceInfo { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
        public string TimeZone { get; set; }
        public string UID { get; set; }
        public string Workspace { get; set; }
        public string WorkspaceLink { get; set; }
        public string XMLTZone { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
    }

}
