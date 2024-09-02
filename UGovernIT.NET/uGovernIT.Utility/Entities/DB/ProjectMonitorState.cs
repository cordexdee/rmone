using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.ProjectMonitorState)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectMonitorState:DBBaseEntity
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
        public string ModuleNameLookup { get; set; }     
        [NotMapped]
        public float OverAllScore { get; set; }
        [NotMapped]
        public string LEDClass { get; set; }
        [NotMapped]
        public float MonitorStateScore { get; set; }
        [NotMapped]
        public ModuleMonitorOption MonitorOption { get; set; }
    }
}
