
CREATE Procedure [dbo].[usp_GetResourceAllocation2]  
--[usp_GetResourceAllocation2]  '35525396-e5fe-4692-9239-4df9305b915b'
@TenantID VARCHAR(MAX),    
@closed char(1)='',  
@TicketId VARCHAR(MAX)='',
@Id bigint= 0
as    
Begin
if(@Id <> 0)
Begin
	select a.Id,	
	r.WorkItemType,	
	r.WorkItem,	
	r.WorkItem WorkItemLink,	
	r.SubWorkItem,	
a.PctAllocation,
a.AllocationStartDate,	
	a.AllocationEndDate,	
	a.ResourceUser ResourceId,	
	--a.ResourceUser,
	 isnull([dbo].[fnGetusername](a.ResourceUser,@TenantID),'Unfilled Roles')[ResourceUser],	
	r.ID [WorkItemID] ,	
	a.Title,	
	'True' ShowEditButton,	
	'False' ShowPartialEdit,	
	a.PctPlannedAllocation,	
	a.PlannedStartDate,	
	a.PlannedEndDate,	
	a.PctEstimatedAllocation,	
	a.EstStartDate,	
	a.EstEndDate,	
	isnull(V.Closed,0) Closed	,
	v.Status,
	a.Deleted,
	a.ResourceWorkItemLookup, a.TicketID,v.OnHold,
	v.ChanceOfSuccessChoice,
	v.ERPJobID, a.SoftAllocation,a.NonChargeable,
	dbo.CheckDateRangeOverlap(v.PreconStartDate,v.PreconEndDate,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInPrecon,
	dbo.CheckDateRangeOverlap(v.EstimatedConstructionStart,v.EstimatedConstructionEnd,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInConst,
	dbo.CheckDateRangeOverlap(v.CloseoutStartDate, v.CloseoutDate ,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInCloseOut,
	case when a.AllocationStartDate != '' and v.PreconStartDate != '' then case when a.AllocationStartDate < v.PreconStartDate then 1 else 0 end else 0 end as IsStartDateBeforePrecon,
	case when a.AllocationStartDate != '' and v.PreconEndDate != '' and v.EstimatedConstructionStart != '' then case when a.AllocationStartDate > v.PreconEndDate and a.AllocationStartDate < v.EstimatedConstructionStart then 1 else 0 end else 0 end as IsStartDateBetweenPreconAndConst,
	case when a.AllocationStartDate != '' and v.EstimatedConstructionEnd != '' and v.CloseoutStartDate != '' then case when a.AllocationStartDate > v.EstimatedConstructionEnd and a.AllocationStartDate < v.CloseoutStartDate then 1 else 0 end else 0 end as IsStartDateBetweenConstAndCloseOut
	,v.ProjectLeadUser,v.LeadEstimatorUser,v.ProjectManagerUser
	from ResourceAllocation (READCOMMITTED) a     
	left join ResourceWorkItems (READCOMMITTED) r on r.ID=a.ResourceWorkItemLookup 
	left Join vw_TicketwithSatus v on v.TicketId=a.TicketID and v.TenantID=@TenantID
	where a.TenantID=@TenantID And (a.Deleted<>1 or a.Deleted is null) and a.ID = @Id
End
Else
Begin
	select a.Id,	
	r.WorkItemType,	
	r.WorkItem,	
	r.WorkItem WorkItemLink,	
	r.SubWorkItem,	
	a.PctAllocation,
	a.AllocationStartDate,	
	a.AllocationEndDate,	
	a.ResourceUser ResourceId,	
	--a.ResourceUser,
	 isnull([dbo].[fnGetusername](a.ResourceUser,@TenantID),'Unfilled Roles')[ResourceUser],	
	r.ID [WorkItemID] ,	
	a.Title,	
	'True' ShowEditButton,	
	'False' ShowPartialEdit,	
	a.PctPlannedAllocation,	
	a.PlannedStartDate,	
	a.PlannedEndDate,	
	a.PctEstimatedAllocation,	
	a.EstStartDate,	
	a.EstEndDate,	
	isnull(V.Closed,0) Closed	,
	v.Status,
	a.Deleted,
	a.ResourceWorkItemLookup, a.TicketID,v.OnHold,
	v.ChanceOfSuccessChoice,
	v.ERPJobID, a.SoftAllocation, a.NonChargeable,
	dbo.CheckDateRangeOverlap(v.PreconStartDate,v.PreconEndDate,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInPrecon,
	dbo.CheckDateRangeOverlap(v.EstimatedConstructionStart,v.EstimatedConstructionEnd,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInConst,
	dbo.CheckDateRangeOverlap(v.CloseoutStartDate, v.CloseoutDate ,a.AllocationStartDate,a.AllocationEndDate) as IsAllocInCloseOut,
	case when a.AllocationStartDate != '' and v.PreconStartDate != '' then case when a.AllocationStartDate < v.PreconStartDate then 1 else 0 end else 0 end as IsStartDateBeforePrecon,
	case when a.AllocationStartDate != '' and v.PreconEndDate != '' and v.EstimatedConstructionStart != '' then case when a.AllocationStartDate > v.PreconEndDate and a.AllocationStartDate < v.EstimatedConstructionStart then 1 else 0 end else 0 end as IsStartDateBetweenPreconAndConst,
	case when a.AllocationStartDate != '' and v.EstimatedConstructionEnd != '' and v.CloseoutStartDate != '' then case when a.AllocationStartDate > v.EstimatedConstructionEnd and a.AllocationStartDate < v.CloseoutStartDate then 1 else 0 end else 0 end as IsStartDateBetweenConstAndCloseOut
	,v.ProjectLeadUser,v.LeadEstimatorUser,v.ProjectManagerUser
	from ResourceAllocation (READCOMMITTED) a     
	left join ResourceWorkItems (READCOMMITTED) r on r.ID=a.ResourceWorkItemLookup 
	left Join vw_TicketwithSatus v on v.TicketId=a.TicketID and v.TenantID=@TenantID
	where a.TenantID=@TenantID And (a.Deleted<>1 or a.Deleted is null) 
End

End
