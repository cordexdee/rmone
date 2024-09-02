CREATE procedure [dbo].[usp_GetPotentialAllocationsList]  
-- usp_GetPotentialAllocationsList '2024-04-15 17:26:27.140','2024-04-30 17:26:27.140', 'AD6EA8C7-CE84-4BE8-947D-4B942255DE6B','e4d13263-1a0d-4e86-b1e7-5c71e0a41241', '35525396-e5fe-4692-9239-4df9305b915b'   
(@StartDate datetime,   
@EndDate datetime,  
@UserRole varchar(128),
@UserID varchar(128),
@TenantId varchar(128))  
AS   
BEGIN  
 DECLARE @UserFunctionId as bigint  
 DECLARE @minStartDate as datetime  
 DECLARE @maxEndDate as datetime  
 Create table #tmpTickets    
 (    
  ProjectId nvarchar(30),  
  Title nvarchar(500),  
  ERPJobID nvarchar(100),  
  DivisionId nvarchar(200),
  DepartmentId nvarchar(200),
  ModuleName varchar(3),  
  PreconStartDate datetime,   
  PreconEndDate datetime,   
  EstimatedConstructionStart datetime,   
  EstimatedConstructionEnd datetime,  
  CloseoutStartDate datetime,   
  CloseoutDate datetime  
 )    
    
 Create table #tmpUnfilledAlloc    
 (    
 ID	bigint,
 AllocationStartDate	datetime,
 AllocationEndDate	datetime, 
 PctAllocation	float,
 ResourceWorkItemLookup	bigint,
 ProjectId nvarchar(30),  
 Title nvarchar(500),  
  ERPJobID nvarchar(100),  
  DivisionId nvarchar(200),
  DepartmentId nvarchar(200),  
  ModuleName varchar(3),  
  PreconStartDate datetime,   
  PreconEndDate datetime,   
  EstimatedConstructionStart datetime,   
  EstimatedConstructionEnd datetime,  
  CloseoutStartDate datetime,   
  CloseoutDate datetime,
  Division varchar(300),
  Department varchar(300),
  DepartmentDivision varchar(300),
  FunctionId bigint,
  FunctionalName varchar(300),
TypeName varchar(300),
[Length] int,
ProjectEstimatedAllocationId	nvarchar(15),
SoftAllocation	bit,
NonChargeable	bit,
IsSelected bit,
 )    

 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId,DepartmentId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate )     
 select TicketId, Title, ERPJobID, DivisionLookup,DepartmentLookup, 'CPR' ,PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate from CRMProject where TenantID = @Tenantid and Closed = 0;    
 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId,DepartmentId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate )     
 select TicketId, Title, ERPJobID, DivisionLookup,DepartmentLookup, 'OPM',PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate from Opportunity where TenantID = @Tenantid and Closed = 0;    
 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId,DepartmentId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate )     
 select TicketId, Title, ERPJobID, DivisionLookup,DepartmentLookup, 'CNS',PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate from CRMServices where TenantID = @Tenantid and Closed = 0;    
  
 Select @UserFunctionId = FunctionId from FunctionRoleMapping where RoleId = @UserRole and TenantID=@TenantId
  
 -- Fetch all unfilled allocations  
INSERT INTO #tmpUnfilledAlloc --convert(varchar(10), r.AllocationStartDate, 101)
SELECT r.ID, r.AllocationStartDate, r.AllocationEndDate, r.PctAllocation, r.ResourceWorkItemLookup, t.*,  
 (Select Title from CompanyDivisions where id = t.DivisionId) Division,
 (Select Title from Department where id = t.DepartmentId) Department,
 (Select Title from CompanyDivisions where id in (Select DivisionIdLookup from Department where id = t.DepartmentId)) DepartmentDivision,
 (Select FunctionId from FunctionRoleMapping where RoleId = r.RoleId) FunctionId,
 (Select Title from FunctionRole where id in (Select FunctionId from FunctionRoleMapping where RoleId = r.RoleId)) [FunctionalName],
 (Select rl.Name from Roles rl where id = pe.Type) [TypeName],
 DATEDIFF(Week, r.AllocationStartDate, r.AllocationEndDate) [Length], r.ProjectEstimatedAllocationId, r.SoftAllocation,r.NonChargeable, 0--, r.IsLocked  
 FROM   
 ResourceAllocation r INNER JOIN #tmpTickets t  
 ON r.TicketId = t.ProjectId  
 inner join ProjectEstimatedAllocation pe
 on r.ProjectEstimatedAllocationId = pe.ID
 WHERE r.ResourceUser = '00000000-0000-0000-0000-000000000000'  
 and ((r.AllocationEndDate >= @StartDate and r.AllocationStartDate <= @EndDate) or r.AllocationStartDate > @StartDate)
 and r.AllocationEndDate > GETDATE()
 /*and r.RoleId in  
 (  
  SELECT CASE   
  WHEN @UserFunctionId IS NULL THEN @UserRole  
  ELSE RoleId END  
  FROM FunctionRoleMapping WHERE FunctionId = @UserFunctionId  and TenantID=@TenantId
 ) */ 
 Order by r.AllocationStartDate  

-- Unfilled allocations
select * from #tmpUnfilledAlloc

select @minStartDate = MIN(AllocationStartDate), @maxEndDate = MAX(AllocationEndDate) 
from #tmpUnfilledAlloc

--select @maxstartDate , @minEndDate 

-- ResourceUsageSummaryWeekWise records for User's Allocations
 SELECT R.ID,	R.ActualHour,R.AllocationHour,	R.FunctionalAreaTitleLookup,R.IsConsultant,	R.IsIT,	R.IsManager,R.ManagerLookup,
 R.ManagerName,	R.PctActual,R.PctAllocation,	R.PctPlannedAllocation,	R.PlannedAllocationHour,	R.ResourceUser [Resource],	
 R.ResourceNameUser [ResourceName],	R.SubWorkItem,	R.WeekStartDate,	R.WorkItem,	R.WorkItemID,	R.WorkItemType,	R.Title,	R.TenantID,	
 R.Created,	R.Modified,	R.CreatedByUser, R.ModifiedByUser,	R.Deleted,	R.Attachments,	R.GlobalRoleID,	R.SoftAllocation,	
 R.FunctionalArea,	R.ERPJobID,	R.ActualStartDate, R.ActualEndDate,	R.ActualPctAllocation
 FROM ResourceUsageSummaryWeekWise R INNER JOIN #tmpTickets t
 ON R.WorkItem = t.ProjectId
 WHERE ActualEndDate >= @minStartDate
 and ActualStartDate <= @maxEndDate
 and ResourceUser = @UserID and TenantID = @TenantId

-- ResourceUsageSummaryWeekWise records for unfilled allocations
SELECT R.ID, R.ActualHour,R.AllocationHour,	R.FunctionalAreaTitleLookup,R.IsConsultant,	R.IsIT,	R.IsManager,R.ManagerLookup,
 R.ManagerName,	R.PctActual,R.PctAllocation,	R.PctPlannedAllocation,	R.PlannedAllocationHour,	R.ResourceUser [Resource],	
 R.ResourceNameUser [ResourceName],	R.SubWorkItem,	R.WeekStartDate,	R.WorkItem,	R.WorkItemID,	R.WorkItemType,	R.Title,	R.TenantID,	
 R.Created,	R.Modified,	R.CreatedByUser, R.ModifiedByUser,	R.Deleted,	R.Attachments,	R.GlobalRoleID,	R.SoftAllocation,	
 R.FunctionalArea,	R.ERPJobID,	R.ActualStartDate, R.ActualEndDate,	R.ActualPctAllocation 
FROM ResourceUsageSummaryWeekWise R
where R.WorkItemID in (select ResourceWorkItemLookup from #tmpUnfilledAlloc)
and R.WorkItem in (select ProjectID from #tmpUnfilledAlloc)
and R.TenantID = @TenantId
and (R.ResourceUser = '00000000-0000-0000-0000-000000000000' OR ResourceUser is NULL)
and R.ActualEndDate is not Null and ActualStartDate is not NULL
and Deleted = 0

SELECT D.ID DivisionId,D.Title Division,R.Name TypeName FROM AspNetUsers U 
LEFT JOIN Department DEPT ON U.DepartmentLookup=DEPT.ID
LEFT JOIN CompanyDivisions D ON DEPT.DivisionIdLookup=D.ID
LEFT JOIN Roles R ON U.GlobalRoleID=R.Id
WHERE U.Id= @UserID and U.TenantID = @TenantId

 DROP TABLE #tmpTickets 
 DROP TABLE #tmpUnfilledAlloc

END
