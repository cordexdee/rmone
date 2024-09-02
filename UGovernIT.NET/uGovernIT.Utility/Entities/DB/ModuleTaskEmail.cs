using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
   
    [Table(DatabaseObjects.Tables.TaskEmails)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class ModuleTaskEmail:DBBaseEntity
    {
        public long ID { get; set; }
        public string Status { get; set; }
       // public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public int? StageStep { get; set; }
        public string EmailUserTypes { get; set; }
        [NotMapped]
        public string ModuleName { get; set; }
        public string Title { get; set; }
        public string EmailTitle { get; set; }
        public string ModuleNameLookup { get; set; }
        //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
        public long? TicketPriorityLookup { get; set; }
        //
        //Added by mudassir 19 march 2020
        //Added 23 march 2020
        public string EmailEventType { get; set; }
        //
        public bool NotifyInPlainText { get; set; }
        //
        
        public string CustomProperties { get; set; }
        public bool HideFooter { get; set; }
        public bool SendEvenIfStageSkipped { get; set; }
        public string EmailIDCC { get; set; }
        [NotMapped]
        public string Stage { get; set; }
        [NotMapped]
        public string Priority { get; set; }
    }
}
