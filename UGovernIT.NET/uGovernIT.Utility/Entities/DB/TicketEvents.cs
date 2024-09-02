using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.TicketEvents)]
    public class TicketEvents 
    {

        public long ID { get; set; }
        public string Title { get; set; }
        public string Ticketid { get; set; }
        public string SubTaskTitle { get; set; }
        public string SubTaskId { get; set; }
        [Column(DatabaseObjects.Columns.AffectedUsers)]
        public string AffectedUsers { get; set; }

        public bool Automatic { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public string EventReason { get; set; }
        public DateTime EventTime { get; set; }

        public DateTime Modified { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public long StageStep { get; set; }
        public string Status { get; set; }

        public string TicketEventBy { get; set; }
        public string TicketEventType { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }

        public string TenantID { get; set; }
    }
}
