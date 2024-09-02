using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{

    [Table(DatabaseObjects.Tables.RequestType)]
    public class ModuleRequestType : DBBaseEntity
    {
        public long ID { get; set; }

        // public string RMMCategory { get; set; }
        public string Category { get; set; }
        public string RequestType { get; set; }
        public long? FunctionalAreaLookup { get; set; }

        [ForeignKey("FunctionalAreaLookup")]
        public FunctionalArea FunctionalArea { get; set; }
        public string WorkflowType { get; set; }

        //public bool? IsDeleted { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string Owner { get; set; }
        //public List<UserInfo> Owner { get; set; }//changes done on 07/12/2016 it is not in the table.
        // public string ModuleName { get; set; }
        public string KeyWords { get; set; }
        // public string CustomProperties { get; set; }
        [Column(DatabaseObjects.Columns.PRPGroup)]
        public string PRPGroup { get; set; }//changes done on 07/12/2016
        //public UserInfo PRPGroup { get; set; }
        //public List<UserInfo> ORP { get; set; }
        [Column(DatabaseObjects.Columns.TicketORP)]
        public string ORP { get; set; }
        [Column("EscalationManagerUser")]
        public string EscalationManager { get; set; }
        [Column(DatabaseObjects.Columns.RequestTypeBackupEscalationManager)]
        public string BackupEscalationManager { get; set; }
        public bool? ServiceWizardOnly { get; set; }
        public double? EstimatedHours { get; set; }
        //public string Description { get; set; }
        //public bool IsAttachment { get; set; }
        public string SubCategory { get; set; }
        [NotMapped]
        public int? TaskTemplateLookup { get; set; }
        // public string IssueTypeOption { get; set; }
        public bool? AutoAssignPRP { get; set; }
        public string RequestCategory { get; set; }

        //  public string FunctionalAreaLookup { get; set; }
        public string Title { get; set; }
        public string ModuleNameLookup { get; set; }
        [NotMapped]
        public string ApplicationModulesLookup { get; set; }
        public bool SortToBottom { get; set; }
        public string AppReferenceInfo { get; set; }
        [NotMapped]
        public int? APPTitleLookup { get; set; }
        public int AssignmentSLA { get; set; }
        public long? BudgetIdLookup { get; set; }
        public int CloseSLA { get; set; }
        public string IssueTypeOptions { get; set; }
        public int ResolutionSLA { get; set; }
        public int RequestorContactSLA { get; set; }
        public bool? OutOfOffice { get; set; }
        public int? StagingId { get; set; }
        public string ResolutionTypes { get; set; }
        public string EmailToTicketSender { get; set; }
        public string Description { get; set; }
        public bool SLADisabled { get; set; }

        [NotMapped]
        public int SortRequestTypeCol { get; set; }
        [NotMapped]
        public string ParentID { get; set; }
        [NotMapped]
        public string ItemID { get; set; }

        public bool Use24x7Calendar { get; set; }

        public string PRPUser { get; set; }

        public bool MatchAllKeywords { get; set; }
        
        public string BreakFix{ get; set; }

    }
}
