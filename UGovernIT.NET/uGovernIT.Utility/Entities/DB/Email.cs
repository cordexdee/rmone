using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Emails)]
    public class Email:DBBaseEntity
    {
        public long ID { get; set; }
        public string EmailIDCC { get; set; }
        public string EmailIDFrom { get; set; }
        public string EmailIDTo { get; set; }
        public string EmailReplyTo { get; set; }
        public bool IsIncomingMail { get; set; }
        public string EscalationEmailBody { get; set; }
        public string MailSubject { get; set; }
        public string MessageId { get; set; }
        public string ModuleNameLookup { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string EmailStatus { get; set; }
        public string EmailError { get; set; }
    }
}
