using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectMonitorStateHistory)]
    public class ProjectMonitorStateHistory:DBBaseEntity
    {
        public long ID { get; set; }

        public bool? AutoCalculate { get; set; }

        public long ModuleMonitorNameLookup { get; set; }

        public long ModuleMonitorOptionIdLookup { get; set; }

        [NotMapped]
        public long ModuleMonitorOptionLEDClass { get; set; }

        [NotMapped]
        public string ModuleMonitorOptionLEDName { get; set; }

        [NotMapped]
        public string ModuleMonitorOptionName { get; set; }

        public long PMMIdLookup { get; set; }

        public string ProjectMonitorNotes { get; set; }

        public int? ProjectMonitorWeight { get; set; }

        public string Title { get; set; }

        public string TicketId { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }

        [NotMapped]
        public float OverAllScore { get; set; }

        [NotMapped]
        public string LEDClass { get; set; }

        [NotMapped]
        public float MonitorStateScore { get; set; }

        [NotMapped]
        public ModuleMonitorOption MonitorOption { get; set; }

        public int BaselineId { get; set; }

        public DateTime BaselineDate { get; set; }


    }
}
