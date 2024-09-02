using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.Appointment)]
    public class Appointment:DBBaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int Label { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public int EventType { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }
        [NotMapped]
        public object OwnerId { get; set; }
        public int ID { get; set; }
        [NotMapped]
        public double Price { get; set; }
        [NotMapped]
        public string ContactInfo { get; set; }
    }
}
