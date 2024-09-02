using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.DashboardSummary)]
    public class DashboardSummary : DBBaseEntity
    {

        public long ID { get; set; }
        public double? ActualHours { get; set; }
        public string ALLSLAsMet { get; set; }
        public string AssignmentSLAMet { get; set; }
        public string Category { get; set; }
        public string CloseSLAMet { get; set; }
        public string Country { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Description { get; set; }
        public long? FunctionalAreaLookup { get; set; }
        public int? GenericStatusLookup { get; set; }
        [Column(DatabaseObjects.Columns.TicketInitiator)]
        public string Initiator { get; set; }
        [Column(DatabaseObjects.Columns.TicketInitiatorResolved)]
        public string InitiatorResolved { get; set; }
        public long? LocationLookup { get; set; }
        public string ModuleNameLookup { get; set; }
        public string ModuleStepLookup { get; set; }
        public int? OnHold { get; set; }
        [Column(DatabaseObjects.Columns.TicketORP)]
        public string ORP { get; set; }
        public string OtherSLAMet { get; set; }
        [Column(DatabaseObjects.Columns.TicketOwner)]
        public string Owner { get; set; }
        public long? PriorityLookup { get; set; }
        [Column(DatabaseObjects.Columns.TicketPRP)]
        public string PRP { get; set; }
        [Column(DatabaseObjects.Columns.PRPGroup)]
        public string PRPGroup { get; set; }
        public string Region { get; set; }
        public int? ReopenCount { get; set; }
        [Column(DatabaseObjects.Columns.TicketRequestor)]
        public string Requestor { get; set; }
        public string RequestorCompany { get; set; }
        public string RequestorContactSLAMet { get; set; }
        public string RequestorDepartment { get; set; }
        public string RequestorDivision { get; set; }
        public string RequestSourceChoice { get; set; }
        public long? RequestTypeLookup { get; set; }
        public string ResolutionSLAMet { get; set; }
        public string ResolutionTypeChoice { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceName { get; set; }
        public string SLAMet { get; set; }
        [Column(DatabaseObjects.Columns.TicketStageActionUsers)]
        public string StageActionUsers { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string SubCategory { get; set; }
        public string TicketId { get; set; }
        public string WorkflowType { get; set; }
        public string Title { get; set; }
        public bool Closed { get; set; }
        public DateTime? OnHoldTillDate { get; set; }
        public double TotalHoldDuration { get; set; }
        public DateTime? InitiatedDate { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? TestedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool? Rejected { get; set; }
        [Column(DatabaseObjects.Columns.TicketClosedBy)]
        public string ClosedBy { get; set; }
        [Column(DatabaseObjects.Columns.TicketResolvedBy)]
        public string ResolvedBy { get; set; }
        public string AssignedBy { get; set; }
        public bool SLADisabled { get; set; }
        public long Age { get; set; }
        public int? StageStep { get; set; }
        public DateTime? OnHoldStartDate { get; set; }
        public bool TicketOnHold { get; set; }
        public string ApprovedByUser { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FCRCategorization { get; set; }
        public string BreakFix { get; set; }
        public int? BulkRequestCount { get; set; }




    }

}
