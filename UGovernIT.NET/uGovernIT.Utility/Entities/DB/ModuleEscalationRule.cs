using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.EscalationRule)]
    public class ModuleEscalationRule:DBBaseEntity
    {      
        public long ID { get; set; }
        public string EscalationDescription { get; set; }
        public string EscalationEmailBody { get; set; }
        public double EscalationFrequency { get; set; }
        public string EscalationMailSubject { get; set; }
        public double EscalationMinutes { get; set; }
        public string EscalationToEmails { get; set; }
        public string EscalationToRoles { get; set; }
        public bool IncludeActionUsers { get; set; }
        public long SLARuleIdLookup { get; set; }
        public bool NotifyInPlainText { get; set; }
        public bool UseDesiredCompletionDate { get; set; }
        public string Title { get; set; }


    }
}
