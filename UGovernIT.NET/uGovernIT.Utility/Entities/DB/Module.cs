using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ProtoBuf;


namespace uGovernIT.Utility
{

    [Table(DatabaseObjects.Tables.Modules)]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class UGITModule : DBBaseEntity
    {
        public long ID { get; set; }

        public string ModuleName { get; set; }
        public string NavigationUrl { get; set; }
        public string NavigationType { get; set; }
        public int LastSequence { get; set; }
        public DateTime LastSequenceDate { get; set; }
        public bool ModuleAutoApprove { get; set; }
        public int ModuleHoldMaxStage { get; set; }
        public string ModuleTable { get; set; }
        [NotMapped]
        public string ListPageUrl { get; set; }
        [NotMapped]
        public string DetailPageUrl { get; set; }
        public string ModuleDescription { get; set; }

        public ModuleType ModuleType { get; set; }
        public string AuthorizedToView { get; set; }
        public bool AllowDraftMode { get; set; }
        public bool EnableEventReceivers { get; set; }
        public bool ReloadCache { get; set; }
        public int ItemOrder { get; set; }
        public bool EnableModule { get; set; }
        public bool ShowComment { get; set; }
        public bool SyncAppsToRequestType { get; set; }
        public bool EnableCache { get; set; }
        public bool DisableNewConfirmation { get; set; }
        public string OwnerBindingChoice { get; set; }
        public bool WaitingOnMeIncludesGroups { get; set; }
        public bool WaitingOnMeExcludesResolved { get; set; }
        public bool ShowNextSLA { get; set; }
        public bool EnableModuleAgent { get; set; }
        public int ModuleId { get; set; }
        public bool UseInGlobalSearch { get; set; }
        public bool EnableWorkflow { get; set; }
        public bool EnableLayout { get; set; }
        public bool StoreEmails { get; set; }
        [NotMapped]
        public bool ShowMyDeptTickets { get; set; }
        public bool EnableQuick { get; set; }
        public bool AllowChangeType { get; set; }
        public bool AllowBatchEditing { get; set; }
        public bool AllowBatchCreate { get; set; }
        public string AuthorizedToCreate { get; set; }
        public bool ShowSummary { get; set; }
        public bool RequestorNotificationOnComment { get; set; }
        public bool ActionUserNotificationOnComment { get; set; }
        public bool InitiatorNotificationOnComment { get; set; }
        public bool RequestorNotificationOnCancel { get; set; }
        public bool ActionUserNotificationOnCancel { get; set; }
        public bool InitiatorNotificationOnCancel { get; set; }
        public bool ShowBottleNeckChart { get; set; }
        public bool AllowEscalationFromList { get; set; }
        public bool AllowReassignFromList { get; set; }
        [NotMapped]
        public string IconUrl { get; set; }
        public bool PreloadAllModuleTabs { get; set; }
        public bool AutoCreateDocumentLibrary { get; set; }
        public bool KeepItemOpen { get; set; }
        public bool ReturnCommentOptional { get; set; }
        public string ShortName { get; set; }
        public string CategoryName { get; set; }
        public string ModuleRelativePagePath { get; set; }
        public string ThemeColor { get; set; }
        public string StaticModulePagePath { get; set; }
        public string Title { get; set; }
        public bool EnableRMMAllocation { get; set; }
        public bool HideWorkFlow { get; set; }
        public bool AllowDelete { get; set; }
        //public int TabNumber { get; set; }
        public bool AllowBatchClose { get; set; }
        public bool ActualHoursByUser { get; set; }
        public bool EnableCloseOnHoldExpire { get; set; }
        public string OpenChart { get; set; }
        public string CloseChart { get; set; }
        public bool EnableNewsOnHomePage { get; set; }
        public bool EnableBaseLine { get; set; }
        public bool ShowTasksInProjectTasks { get; set; }
        public bool? EnableLinkSimilarTickets { get; set; }
        public bool? EnableAddNewButton { get; set; }

        //SpDelta 42(Implementation of asset/ticket import)
        public bool EnableTicketImport { get; set; }
        public int HoldMaxStage { get; set; }
        public bool KeepTicketCounts { get; set; }
        //
        [NotMapped]
        public List<LifeCycle> List_LifeCycles { get; set; }
        [NotMapped]
        public List<ModuleFormLayout> List_FormLayout { get; set; }
        [NotMapped]
        public List<ModuleRoleWriteAccess> List_RoleWriteAccess { get; set; }
        [NotMapped]
        public List<ModuleFormTab> List_FormTab { get; set; }
        [NotMapped]
        public List<ModuleDefaultValue> List_DefaultValues { get; set; }
        [NotMapped]
        public List<ModuleTaskEmail> List_TaskEmail { get; set; }
        [NotMapped]
        public List<ModuleRequestType> List_RequestTypes { get; set; }
        [NotMapped]
        public List<ModulePriorityMap> List_PriorityMaps { get; set; }
        [NotMapped]
        public List<ModuleColumn> List_ModuleColumns { get; set; }
        [NotMapped]
        public List<ModuleUserType> List_ModuleUserTypes { get; set; }
        [NotMapped]
        public List<ModulePrioirty> List_Priorities { get; set; }
        [NotMapped]
        public List<ModuleImpact> List_Impacts { get; set; }
        [NotMapped]
        public List<ModuleSeverity> List_Severities { get; set; }
        [NotMapped]
        public List<ModuleRequestTypeLocation> List_RequestTypeByLocation { get; set; }

        public string CustomProperties { get; set; }
        [NotMapped]
        public string TicketListViewFields { get; set; }
        public int? AutoRefreshListFrequency { get; set; }

        public bool EnableIcon { get; set; }
        public string AltTicketIdField { get; set; }

        public bool IsAllocationTypeHard_Soft { get; set; }

    }
}
