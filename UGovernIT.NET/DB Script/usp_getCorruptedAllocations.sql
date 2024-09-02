CREATE Procedure [dbo].[usp_getCorruptedAllocations]    
@TenantID varchar(128)='',  
@tabname varchar(100)  
--Declare @TenantID varchar(128)='35525396-E5FE-4692-9239-4DF9305B915B'    
as    
begin    
if(@tabname='RAllocation')  
Begin  
--Extra data in ResourceAllocation which are not present on ProjectEstimatedAllocation table  
Select ID,ProjectEstimatedAllocationId,TicketId,AllocationEndDate,AllocationStartDate,  
PctAllocation,PctPlannedAllocation,ResourceUser,PlannedStartDate,PlannedEndDate,PctEstimatedAllocation,  
EstStartDate,EstEndDate 
from ResourceAllocation   
where TenantID = @TenantID
and (
Cast (ProjectEstimatedAllocationId as bigint) not in  (select ID from ProjectEstimatedAllocation where TenantID = @TenantID)
OR ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')
)   
union  
select ID,ProjectEstimatedAllocationId,TicketId,AllocationEndDate,AllocationStartDate,  
PctAllocation,PctPlannedAllocation,ResourceUser,PlannedStartDate,PlannedEndDate,PctEstimatedAllocation,  
EstStartDate,EstEndDate  
from ResourceAllocation where TenantID=@TenantID 
and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)  
FROM ResourceAllocation where TenantID=@TenantID     
GROUP BY ProjectEstimatedAllocationid)  
union
select ID,ProjectEstimatedAllocationId,TicketId,AllocationEndDate,AllocationStartDate,  
PctAllocation,PctPlannedAllocation,ResourceUser,PlannedStartDate,PlannedEndDate,PctEstimatedAllocation,  
EstStartDate,EstEndDate from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000'))
END  
Else If (@tabname='ProjEst')  
Begin  
--Extra data in ProjectEstimatedAllocation which are not present on ResourceAllocation table  
select ID,TicketId,AllocationEndDate,AllocationStartDate,AssignedToUser,  
PctAllocation from ProjectEstimatedAllocation   
where id not in   
(select ISNULL(ProjectEstimatedAllocationId, 0) from ResourceAllocation where TenantID = @TenantID)   
and TenantID = @TenantID
union
select ID,TicketId,AllocationEndDate,AllocationStartDate,AssignedToUser,  
PctAllocation from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')
END  
Else If (@tabname='RMonthly')  
Begin  
select ID,MonthStartDate,PctAllocation,PctPlannedAllocation,ResourceUser,ResourceSubWorkItem,  
ResourceWorkItem,ResourceWorkItemType,ActualStartDate,ActualEndDate,  
ActualPctAllocation  from ResourceAllocationMonthly where TenantID=@TenantID  
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation   
where (ProjectEstimatedAllocationId is null 
and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in   
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))   
and TenantID = @TenantID)  
union  
select ID,MonthStartDate,PctAllocation,PctPlannedAllocation,ResourceUser,ResourceSubWorkItem,  
ResourceWorkItem,ResourceWorkItemType,ActualStartDate,ActualEndDate,  
ActualPctAllocation  from ResourceAllocationMonthly where TenantID=@TenantID   
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation 
where TenantID=@TenantID and ProjectEstimatedAllocationId is not null 
and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)  
FROM ResourceAllocation where TenantID=@TenantID     
GROUP BY ProjectEstimatedAllocationid))  
union 
select ID,MonthStartDate,PctAllocation,PctPlannedAllocation,ResourceUser,ResourceSubWorkItem,  
ResourceWorkItem,ResourceWorkItemType,ActualStartDate,ActualEndDate,  
ActualPctAllocation  from ResourceAllocationMonthly where TenantID=@TenantID 
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))
END  
Else If (@tabname='RMonthWise')  
Begin  
----ResourceUsageSummaryMonthWise     
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID,
ActualHour,ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,PctPlannedAllocation,ActualStartDate,
ActualEndDate  from ResourceUsageSummaryMonthWise where TenantID=@TenantID and  
WorkItemID in (select ResourceWorkItemLookup from ResourceAllocation   
where (ProjectEstimatedAllocationId is null 
and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in   
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))   
and TenantID = @TenantID  
)  
union  
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID, ActualHour,
ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,PctPlannedAllocation,ActualStartDate,ActualEndDate  
from ResourceUsageSummaryMonthWise where TenantID=@TenantID and  
WorkItemID in (  
select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID 
and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)  
FROM ResourceAllocation where TenantID=@TenantID     
GROUP BY ProjectEstimatedAllocationid)  
) 
union
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID, ActualHour,
ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,PctPlannedAllocation,ActualStartDate,ActualEndDate  
from ResourceUsageSummaryMonthWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))
 END   
 Else If (@tabname='RWeekWise')  
Begin  
----ResourceUsageSummaryWeekWise    
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID, ActualHour,ActualPctAllocation,
PctActual,PctAllocation,ActualPctAllocation,PctPlannedAllocation, WeekStartDate,ActualStartDate,ActualEndDate 
from ResourceUsageSummaryWeekWise where TenantID=@TenantID  
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation   
where (ProjectEstimatedAllocationId is null 
and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in   
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))   
and TenantID = @TenantID)  
union  
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID, 
ActualHour,ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation
,PctPlannedAllocation, WeekStartDate,ActualStartDate,ActualEndDate 
from ResourceUsageSummaryWeekWise where TenantID=@TenantID   
and WorkItemID  in (select ResourceWorkItemLookup 
from ResourceAllocation where TenantID=@TenantID 
and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)  
FROM ResourceAllocation where TenantID=@TenantID     
GROUP BY ProjectEstimatedAllocationid)) 
union
select ID, WorkItem,SubWorkItem,ResourceNameUser UserName,ManagerName,WorkItemID, 
ActualHour,ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation
,PctPlannedAllocation, WeekStartDate,ActualStartDate,ActualEndDate  from ResourceUsageSummaryWeekWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))
END  
END
