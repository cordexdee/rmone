using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.SLARule)]
    public class ModuleSLARule:DBBaseEntity
    {
        public long ID { get; set; }
        public int EndStageStep { get; set; }
        public long EndStageTitleLookup { get; set; }
        //public bool IsDeleted { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleNameLookup { get; set; }
        public long PriorityLookup { get; set; }
        public string SLACategoryChoice { get; set; }
        public string SLADaysRoundUpDownChoice { get; set; }
        public double SLAHours { get; set; }
        public int SLATarget { get; set; }
        public long StageTitleLookup { get; set; }
        public int StagingId { get; set; }
        public int StartStageStep { get; set; }
        public string Title { get; set; }

        [NotMapped]
        public string StartStage { get; set; }
        [NotMapped]
        public string EndStage { get; set; }
        [NotMapped]
        public string SLA { get; set; }
    }
}
