using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleWorkflowHistory)]
    public class ModuleWorkflowHistory:DBBaseEntity
    {
        [Key]
       public long ID { get; set; }

       public string ActionUserType { get; set; }
	   public int Duration { get; set; }
	   public string ModuleNameLookup { get; set; }
	   public int OnHoldDuration { get; set; }
	   public Boolean SLAMet { get; set; }
	   public string StageClosedBy { get; set; }
	   public string StageClosedByName { get; set; }
	   public DateTime? StageEndDate { get; set; }

	   public DateTime StageStartDate { get; set; }
	   public int StageStep { get; set; }
	   public string TicketId { get; set; }
      
	   public string Title { get; set; }
    }

    [Table(DatabaseObjects.Tables.ModuleWorkflowHistoryArchive)]
    public class ModuleWorkflowHistoryArchive : DBBaseEntity
    {
        [Key]
        public long ID { get; set; }

        public string ActionUserType { get; set; }
        public int Duration { get; set; }
        public string ModuleNameLookup { get; set; }
        public int OnHoldDuration { get; set; }
        public Boolean SLAMet { get; set; }
        public string StageClosedBy { get; set; }
        public string StageClosedByName { get; set; }
        public DateTime? StageEndDate { get; set; }

        public DateTime StageStartDate { get; set; }
        public int StageStep { get; set; }
        public string TicketId { get; set; }

        public string Title { get; set; }
    }
}
