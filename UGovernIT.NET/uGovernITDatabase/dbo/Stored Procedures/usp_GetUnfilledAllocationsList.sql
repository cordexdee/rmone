ALTER procedure [dbo].[usp_GetUnfilledAllocationsList]  
-- usp_GetUnfilledAllocationsList '2024-05-01 17:26:27.140','2024-05-31 17:26:27.140','5931A003-EDE3-4566-9A41-1746525F3C5E', '35525396-e5fe-4692-9239-4df9305b915b',1   
(@StartDate datetime,   
@EndDate datetime,  
@UserRole varchar(128),
@TenantId varchar(128),
@SoftAllocation bit)  
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
  ModuleName varchar(3),  
  PreconStartDate datetime,   
  PreconEndDate datetime,   
  EstimatedConstructionStart datetime,   
  EstimatedConstructionEnd datetime,  
  CloseoutStartDate datetime,   
  CloseoutDate datetime ,
  TagMultiLookup varchar(max),
  CRMProjectComplexityChoice nvarchar(300),
  RequestTypeLookup bigint,
  CRMCompanyLookup nvarchar(300),
  SectorChoice nvarchar(300)
 )    
    
 Create table #tmpUnfilledAlloc    
 (    
 ID	bigint,
 AssignedTo nvarchar(100),
 AssignedToName nvarchar(100),
 RoleId varchar(300),
 AllocationStartDate	datetime,
 AllocationEndDate	datetime, 
 PctAllocation	float,
 ResourceWorkItemLookup	bigint,
 ProjectId nvarchar(30),  
 Title nvarchar(500),  
  ERPJobID nvarchar(100),  
  DivisionId nvarchar(200),  
  ModuleName varchar(3),  
  PreconStartDate datetime,   
  PreconEndDate datetime,   
  EstimatedConstructionStart datetime,   
  EstimatedConstructionEnd datetime,  
  CloseoutStartDate datetime,   
  CloseoutDate datetime,
  TagMultiLookup varchar(max),
  CRMProjectComplexityChoice nvarchar(300),
  RequestTypeLookup bigint,
  CRMCompanyLookup nvarchar(300),
  SectorChoice nvarchar(300),
  Division varchar(300),
TypeName varchar(300),
[Length] int,
ProjectEstimatedAllocationId	nvarchar(15),
SoftAllocation	bit,
NonChargeable	bit,
FunctionalName varchar(300),
StaticModulePagePath varchar(300),
RequestType varchar(300),
CompanyTitle varchar(300)
 )    

 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate, TagMultiLookup, CRMProjectComplexityChoice, RequestTypeLookup,CRMCompanyLookup, SectorChoice)     
 select TicketId, Title, ERPJobID, DivisionLookup, 'CPR' ,PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate,TagMultiLookup, CRMProjectComplexityChoice, RequestTypeLookup,CRMCompanyLookup, SectorChoice from CRMProject where TenantID = @Tenantid and Closed = 0;    
 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate, TagMultiLookup, CRMProjectComplexityChoice, RequestTypeLookup ,CRMCompanyLookup, SectorChoice)     
 select TicketId, Title, ERPJobIDNC, DivisionLookup, 'OPM',PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate,TagMultiLookup,CRMProjectComplexityChoice, RequestTypeLookup, CRMCompanyLookup, SectorChoice from Opportunity where TenantID = @Tenantid and Closed = 0;    
 Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutStartDate, CloseoutDate, TagMultiLookup, CRMProjectComplexityChoice, RequestTypeLookup,CRMCompanyLookup,SectorChoice )     
 select TicketId, Title, ERPJobID, DivisionLookup, 'CNS',PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd,   
 CloseoutStartDate, CloseoutDate,TagMultiLookup,CRMProjectComplexityChoice, RequestTypeLookup, CRMCompanyLookup, SectorChoice from CRMServices where TenantID = @Tenantid and Closed = 0;    
  
 
	-- Fetch all unfilled allocations  
	INSERT INTO #tmpUnfilledAlloc --convert(varchar(10), r.AllocationStartDate, 101)
SELECT r.ID,'' AssignedTo, '' AssignedToName,r.RoleId, r.AllocationStartDate, r.AllocationEndDate, r.PctAllocation, r.ResourceWorkItemLookup, t.*,  
 (Select Title from CompanyDivisions where id = t.DivisionId) Division, (Select rl.Name from Roles rl where id = r.RoleId) [TypeName],  
 DATEDIFF(Week, r.AllocationStartDate, r.AllocationEndDate) [Length], r.ProjectEstimatedAllocationId, r.SoftAllocation,r.NonChargeable,--, r.IsLocked  
  (Select Title from FunctionRole where id in (Select FunctionId from FunctionRoleMapping where RoleId = r.RoleId)) [FunctionalName],
  --'openTicketDialog('''+ cm.StaticModulePagePath+''',TicketId='''+TicketId+''',''' + ERPJobID +''':'''+ t.Title +''')' TicketUrl,
  (select top 1 StaticModulePagePath from Config_Modules where ModuleName = t.ModuleName and TenantID = @TenantId) StaticModulePagePath,
  (select RequestType from Config_Module_RequestType where ID = t.RequestTypeLookup) RequestType,
  dbo.fnGetCompanyTitle(t.CRMCompanyLookup, @TenantId) CompanyTitle
 FROM   
 ResourceAllocation r INNER JOIN #tmpTickets t  
 ON r.TicketId = t.ProjectId
 WHERE r.ResourceUser = '00000000-0000-0000-0000-000000000000' 
 and r.AllocationStartDate < @EndDate and @StartDate < r.AllocationEndDate
 and r.AllocationEndDate > GETDATE()
 and r.RoleId = @UserRole
  and r.SoftAllocation = CASE WHEN CONVERT(varchar,@SoftAllocation)= 0 then CONVERT(varchar,@SoftAllocation) else r.SoftAllocation END

 --and r.RoleId in  
 --(  
 -- SELECT CASE   
 -- WHEN @UserFunctionId IS NULL THEN @UserRole  
 -- ELSE RoleId END  
 -- FROM FunctionRoleMapping WHERE FunctionId = @UserFunctionId  and TenantID=@TenantId
 --)  
 Order by t.ProjectId, r.RoleId, r.AllocationStartDate  
-- Unfilled allocations
select * from #tmpUnfilledAlloc

 DROP TABLE #tmpTickets 
 DROP TABLE #tmpUnfilledAlloc

END
