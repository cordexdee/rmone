

ALTER Procedure [dbo].[usp_GetResourceAllocation2]  
--[usp_GetResourceAllocation2]  '35525396-e5fe-4692-9239-4df9305b915b'
@TenantID VARCHAR(MAX),    
@closed char(1)='',  
@TicketId VARCHAR(MAX)=''   
as    
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
a.Deleted,
a.ResourceWorkItemLookup, a.TicketID,
v.ERPJobID, a.SoftAllocation
from ResourceAllocation (READCOMMITTED) a     
left join ResourceWorkItems (READCOMMITTED) r on r.ID=a.ResourceWorkItemLookup 
left Join vw_TicketwithSatus v on v.TicketId=a.TicketID and v.TenantID=@TenantID
where a.TenantID=@TenantID      And  (a.Deleted<>1 or a.Deleted is null)  

--and a.ResourceUser='00000000-0000-0000-0000-000000000000'
End
