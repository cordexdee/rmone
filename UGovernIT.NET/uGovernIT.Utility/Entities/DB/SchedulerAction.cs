using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.SchedulerActions)]
    public class SchedulerAction:DBBaseEntity
    {
        public long ID { get; set; }
        public string ActionType { get; set; }
        public string ActionTypeData { get; set; }
        public string AlertCondition { get; set; }
        public string AttachmentFormat { get; set; }
        public string CustomProperties { get; set; }
        public string EmailBody { get; set; }
        public string EmailIDCC { get; set; }
        public string EmailIDTo { get; set; }
        public string ListName { get; set; }
        public string MailSubject { get; set; }
        public string ModuleNameLookup { get; set; }
        public bool Recurring { get; set; }
        public DateTime? RecurringEndDate { get; set; }
        public int RecurringInterval { get; set; }
        public DateTime? StartTime { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }

        public string FileLocation { get; set; }
    }
}
