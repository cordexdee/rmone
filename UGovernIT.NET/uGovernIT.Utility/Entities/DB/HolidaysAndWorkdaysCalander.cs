using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.HolidaysAndWorkDaysCalendar)]
    public class HolidaysAndWorkDaysCalendar:DBBaseEntity
    {
        public long ID { get; set; }
        [Column(DatabaseObjects.Columns.CategoryChoice)]
        public string Category { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? EventCanceled { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventType { get; set; }
        public string Facilities { get; set; }
        public string fAllDayEvent { get; set; }
        public string fRecurrence { get; set; }
        public string FreeBusy { get; set; }
        //public bool? IsDeleted { get; set; }
        public string Location { get; set; }
        public string MasterSeriesItemID { get; set; }
        public string Overbook { get; set; }
        public string Participants { get; set; }
        public string ParticipantsPicker { get; set; }
        public string RecurrenceData { get; set; }
        public DateTime? RecurrenceID { get; set; }
        public string RecurrenceInfo { get; set; }
        public string Status { get; set; }
        public string TimeZone { get; set; }
        public string Title { get; set; }
        public string UID { get; set; }
        public string Workspace { get; set; }
        public string WorkspaceLink { get; set; }
        public string XMLTZone { get; set; }
       
    }

}
