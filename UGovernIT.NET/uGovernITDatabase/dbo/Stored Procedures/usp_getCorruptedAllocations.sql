
ALTER Procedure [dbo].[usp_getCorruptedAllocations]        
@TenantID varchar(128)='',      
@tabname varchar(100) ='' ,    
@IncludedClosed bit
--[usp_getCorruptedAllocations] '35525396-E5FE-4692-9239-4DF9305B915B'  ,'ProjEst'  ,'false'    
as        
begin     
Create table #spResult (TicketId varchar(20))    
Insert into #spResult exec usp_GetClosedTickets @TenantID, 'CPR,OPM,CNS'    
if(@tabname='RAllocation')      
Begin      
--Extra data in ResourceAllocation which are not present on ProjectEstimatedAllocation table      
 Select 'Missing Proj.Est' IssueType,ID ,dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy, CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified, ProjectEstimatedAllocationId as 'Proj.Est Id',TicketId,    
 Convert(varchar(11),AllocationStartDate,100)AllocationStartDate, Convert(varchar(11),AllocationEndDate,100)AllocationEndDate,    
 dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,PctEstimatedAllocation  
 from ResourceAllocation       
 where TenantID = @TenantID    
 and (    
 Cast (ProjectEstimatedAllocationId as bigint) not in  (select ID from ProjectEstimatedAllocation where TenantID = @TenantID)    
 OR ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')    
 -- Only include when @IncludedClosed is true
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND TicketId NOT IN (SELECT TicketId FROM #spResult)))
 )       
UNION      
 select 'Missing Workitem' IssueType,ID, dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy, CONVERT(varchar,Modified,100)Modified,     
 ProjectEstimatedAllocationId as 'Proj.Est Id',TicketId,    
 Convert(varchar(11),AllocationStartDate,100)AllocationStartDate , Convert(varchar(11),AllocationEndDate,100)AllocationEndDate,     
 dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,PctEstimatedAllocation     
 from ResourceAllocation where TenantID=@TenantID     
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND TicketId NOT IN (SELECT TicketId FROM #spResult)))
 and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)      
 FROM ResourceAllocation where TenantID=@TenantID         
 GROUP BY ProjectEstimatedAllocationid)      
UNION    
 select 'Overlapping Alloc' IssueType, ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified,     
 ProjectEstimatedAllocationId as 'Proj.Est Id',TicketId,    
 Convert(varchar(11),AllocationStartDate,100)AllocationStartDate, Convert(varchar(11),AllocationEndDate,100)AllocationEndDate,    
 dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,PctEstimatedAllocation      
 from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID     
 and id in (    
  SELECT a.ID    
  FROM ProjectEstimatedAllocation a    
  JOIN (SELECT TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate     
  FROM ProjectEstimatedAllocation  where TenantID   =@TenantID   
  GROUP BY TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate    
  HAVING count(*) > 1 ) b ON     
  a.TicketId = b.TicketId    
  AND a.Type = b.Type    
  AND a.AllocationEndDate = b.AllocationEndDate    
  AND a.AllocationStartDate = b.AllocationStartDate    
  AND a.AssignedToUser = b.AssignedToUser    
  --and a.TicketId='CPR-23-000707' and a.AssignedToUser='5624496e-4a04-4858-934d-7efba70cac6f'    
  and a.AssignedToUser != '00000000-0000-0000-0000-000000000000'    
  ))    
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND TicketId NOT IN (SELECT TicketId FROM #spResult)))
END      
Else If (@tabname='ProjEst')      
Begin      
--Extra data in ProjectEstimatedAllocation which are not present on ResourceAllocation table      
 Select 'Missing Res.Alloc' IssueType ,ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified, TicketId,    
 Convert(varchar(11),AllocationStartDate,100)AllocationStartDate,     
 Convert(varchar(11),AllocationEndDate,100)AllocationEndDate,    
  dbo.fnGetusername(AssignedToUser,@TenantID)AssignedToUser,      
 PctAllocation from ProjectEstimatedAllocation        
 where id not in  (select ISNULL(ProjectEstimatedAllocationId, 0) from ResourceAllocation where TenantID = @TenantID)       
 and TenantID = @TenantID    
 AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND TicketId NOT IN (SELECT TicketId FROM #spResult)))

UNION    
    
 Select 'Overlapping Alloc' IssueType, ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified, TicketId,    
 Convert(varchar(11),AllocationStartDate,100)AllocationStartDate, Convert(varchar(11),AllocationEndDate,100)AllocationEndDate,    
 dbo.fnGetusername(AssignedToUser,@TenantID)AssignedToUser,      
 PctAllocation from ProjectEstimatedAllocation where TenantID=@TenantID and id in (SELECT a.ID    
  FROM ProjectEstimatedAllocation a    
  JOIN (SELECT TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate     
  FROM ProjectEstimatedAllocation  where TenantID=@TenantID
  GROUP BY TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate    
  HAVING count(*) > 1 ) b ON     
  a.TicketId = b.TicketId    
  AND a.Type = b.Type    
  AND a.AllocationEndDate = b.AllocationEndDate    
  AND a.AllocationStartDate = b.AllocationStartDate    
  AND a.AssignedToUser = b.AssignedToUser    
  --and a.TicketId='CPR-23-000707' and a.AssignedToUser='5624496e-4a04-4858-934d-7efba70cac6f'    
  and a.AssignedToUser != '00000000-0000-0000-0000-000000000000')    
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND TicketId NOT IN (SELECT TicketId FROM #spResult)))

END      
Else If (@tabname='RMonthly')      
Begin      
 Select 'Missing Res.Alloc' IssueType,ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified,     
 Convert(varchar(11),MonthStartDate,100)MonthStartDate,PctAllocation, dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,ResourceSubWorkItem 'SubWorkItem',      
 ResourceWorkItem 'WorkItem',Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate,      
 ActualPctAllocation 'PctAllocation'  from ResourceAllocationMonthly where TenantID=@TenantID      
 and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation       
 where (ProjectEstimatedAllocationId is null     
 and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in       
 (select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))       
 and TenantID = @TenantID)  
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND ResourceWorkItem NOT IN (SELECT TicketId FROM #spResult)))
UNION    
    
 Select 'Missing Workitem' IssueType,ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified,     
 CONVERT(varchar,MonthStartDate,100)MonthStartDate,PctAllocation,     
 dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,ResourceSubWorkItem 'SubWorkItem',      
 ResourceWorkItem 'WorkItem',Convert(varchar,ActualStartDate,100)ActualStartDate,Convert(varchar,ActualEndDate,100)ActualEndDate,      
 ActualPctAllocation  from ResourceAllocationMonthly where TenantID=@TenantID       
 and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation     
 where TenantID=@TenantID and ProjectEstimatedAllocationId is not null     
 and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)      
 FROM ResourceAllocation where TenantID=@TenantID         
 GROUP BY ProjectEstimatedAllocationid))   
 AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND ResourceWorkItem NOT IN (SELECT TicketId FROM #spResult)))
    
UNION     
 Select 'Overlapping Alloc' IssueType,ID,    
 dbo.fnGetusername(CreatedByUser,@TenantID)CreatedBy,CONVERT(varchar,Created,100)Created,    
 dbo.fnGetusername(ModifiedByUser,@TenantID)ModifiedBy,CONVERT(varchar,Modified,100)Modified,     
 CONVERT(varchar,MonthStartDate,100)MonthStartDate,PctAllocation, dbo.fnGetusername(ResourceUser,@TenantID)ResourceUser,ResourceSubWorkItem 'SubWorkItem',      
 ResourceWorkItem 'WorkItem',Convert(varchar,ActualStartDate,100)ActualStartDate,Convert(varchar,ActualEndDate,100)ActualEndDate,      
 ActualPctAllocation  from ResourceAllocationMonthly where TenantID=@TenantID     
 and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID     
 and id in (SELECT a.ID    
  FROM ProjectEstimatedAllocation a    
  JOIN (SELECT TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate     
  FROM ProjectEstimatedAllocation    where TenantID=@TenantID
  GROUP BY TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate    
  HAVING count(*) > 1 ) b ON     
  a.TicketId = b.TicketId    
  AND a.Type = b.Type    
  AND a.AllocationEndDate = b.AllocationEndDate    
  AND a.AllocationStartDate = b.AllocationStartDate    
  AND a.AssignedToUser = b.AssignedToUser    
  --and a.TicketId='CPR-23-000707' and a.AssignedToUser='5624496e-4a04-4858-934d-7efba70cac6f'    
  and a.AssignedToUser != '00000000-0000-0000-0000-000000000000')))   
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND ResourceWorkItem NOT IN (SELECT TicketId FROM #spResult)))
END      
Else If (@tabname='RMonthWise')      
Begin      
 ----ResourceUsageSummaryMonthWise         
 Select 'Missing Proj.Est' IssueType,ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,    
 ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,    
 Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate    
 from ResourceUsageSummaryMonthWise where TenantID=@TenantID and      
 WorkItemID in (select ResourceWorkItemLookup from ResourceAllocation       
 where (ProjectEstimatedAllocationId is null     
 and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in       
 (select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))       
 and TenantID = @TenantID      
 ) AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND WorkItem NOT IN (SELECT TicketId FROM #spResult))) 
UNION      
 Select 'Missing Workitem' IssueType,ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,     
 ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,    
 Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate    
 from ResourceUsageSummaryMonthWise where TenantID=@TenantID and      
 WorkItemID in (      
 select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID     
 and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)      
 FROM ResourceAllocation where TenantID=@TenantID         
 GROUP BY ProjectEstimatedAllocationid)      
 )  AND (@IncludedClosed = 1 AND WorkItem NOT IN (SELECT TicketId FROM #spResult))    
Union    
 select 'Overlapping Alloc' IssueType, ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,     
 ActualPctAllocation,PctActual,PctAllocation,ActualPctAllocation,    
 Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate    
 from ResourceUsageSummaryMonthWise where TenantID=@TenantID     
 and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where     
 ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID     
 and id in (SELECT a.ID    
  FROM ProjectEstimatedAllocation a    
  JOIN (SELECT TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate     
  FROM ProjectEstimatedAllocation    where TenantID=@TenantID
  GROUP BY TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate    
  HAVING count(*) > 1 ) b ON     
  a.TicketId = b.TicketId    
  AND a.Type = b.Type    
  AND a.AllocationEndDate = b.AllocationEndDate    
  AND a.AllocationStartDate = b.AllocationStartDate    
  AND a.AssignedToUser = b.AssignedToUser    
  --and a.TicketId='CPR-23-000707' and a.AssignedToUser='5624496e-4a04-4858-934d-7efba70cac6f'    
  and a.AssignedToUser != '00000000-0000-0000-0000-000000000000')))   
  AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND WorkItem NOT IN (SELECT TicketId FROM #spResult))) 
 END       
Else If (@tabname='RWeekWise')      
Begin      
----ResourceUsageSummaryWeekWise        
 Select 'Missing Proj.Est' IssueType, ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,    
 PctActual,PctAllocation,ActualPctAllocation,Convert(varchar(11),WeekStartDate,100) WeekStartDate,    
 Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate    
 from ResourceUsageSummaryWeekWise where TenantID=@TenantID      
 and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation       
 where (ProjectEstimatedAllocationId is null     
 and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in       
 (select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))       
 and TenantID = @TenantID)      
 AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND WorkItem NOT IN (SELECT TicketId FROM #spResult))) 
UNION      
 Select 'Missing Workitem' IssueType,ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,     
 PctActual,PctAllocation,ActualPctAllocation    
 , Convert(varchar(11),WeekStartDate,100)WeekStartDate,Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate     
 from ResourceUsageSummaryWeekWise where TenantID=@TenantID       
 and WorkItemID  in (select ResourceWorkItemLookup     
 from ResourceAllocation where TenantID=@TenantID     
 and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)      
 FROM ResourceAllocation where TenantID=@TenantID         
 GROUP BY ProjectEstimatedAllocationid))    
 AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND WorkItem NOT IN (SELECT TicketId FROM #spResult))) 
UNION    
select 'Overlapping Alloc' IssueType, ID, WorkItem,SubWorkItem,ResourceNameUser UserName,WorkItemID,     
PctActual,PctAllocation,ActualPctAllocation    
, Convert(varchar(11),WeekStartDate,100)WeekStartDate,Convert(varchar(11),ActualStartDate,100)ActualStartDate,Convert(varchar(11),ActualEndDate,100)ActualEndDate    
from ResourceUsageSummaryWeekWise where TenantID=@TenantID     
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID     
and id in (SELECT a.ID    
 FROM ProjectEstimatedAllocation a    
 JOIN (SELECT TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate     
 FROM ProjectEstimatedAllocation  where TenantID=@TenantID   
 GROUP BY TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate    
 HAVING count(*) > 1 ) b ON     
 a.TicketId = b.TicketId    
 AND a.Type = b.Type    
 AND a.AllocationEndDate = b.AllocationEndDate    
 AND a.AllocationStartDate = b.AllocationStartDate    
 AND a.AssignedToUser = b.AssignedToUser    
 --and a.TicketId='CPR-23-000707' and a.AssignedToUser='5624496e-4a04-4858-934d-7efba70cac6f'    
 and a.AssignedToUser != '00000000-0000-0000-0000-000000000000')))    
 AND ((@IncludedClosed = 1) OR (@IncludedClosed = 0 AND WorkItem NOT IN (SELECT TicketId FROM #spResult)))  
END      
    
drop table #spResult     
END 