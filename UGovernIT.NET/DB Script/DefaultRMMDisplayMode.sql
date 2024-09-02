



-- usp_GetPotentialAllocationsList '2024-01-01 17:26:27.140','2024-01-31 17:26:27.140', '20EB6220-399C-4A3E-B050-EA258AE49378', '35525396-e5fe-4692-9239-4df9305b915b' 
alter procedure usp_GetPotentialAllocationsList
(@StartDate datetime, 
@EndDate datetime,
@UserRole varchar(128),
@TenantId varchar(128))
AS 
BEGIN
	DECLARE @UserFunctionId as bigint
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
	 CloseoutDate datetime
	)  
  
	Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd)   
	select TicketId, Title, ERPJobID, DivisionLookup, 'CPR' ,PreconStartDate, PreconEndDate, EstimatedConstructionStart, 
	EstimatedConstructionEnd from CRMProject where TenantID = @Tenantid and Closed = 0;  
	Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd)   
	select TicketId, Title, ERPJobID, DivisionLookup, 'OPM',PreconStartDate, PreconEndDate, EstimatedConstructionStart, 
	EstimatedConstructionEnd from Opportunity where TenantID = @Tenantid and Closed = 0;  
	Insert into #tmpTickets (ProjectId, Title, ERPJobID, DivisionId, ModuleName, PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd)   
	select TicketId, Title, ERPJobID, DivisionLookup, 'CNS',PreconStartDate, PreconEndDate, EstimatedConstructionStart, 
	EstimatedConstructionEnd from CRMServices where TenantID = @Tenantid and Closed = 0;  

	Select @UserFunctionId = FunctionId from FunctionRoleMapping where RoleId = @UserRole

	-- Fetch all unfilled allocations
	SELECT r.ID, convert(varchar(10), r.AllocationStartDate, 101) AllocationStartDate, convert(varchar(10), 
	r.AllocationEndDate, 101) AllocationEndDate, r.PctAllocation, r.ResourceWorkItemLookup, t.*,
	(Select Title from CompanyDivisions where id = t.DivisionId) Division, (Select rl.Name from Roles rl where id = r.RoleId) [Role],
	DATEDIFF(Week, r.AllocationStartDate, r.AllocationEndDate) [Length], r.ProjectEstimatedAllocationId, r.SoftAllocation,r.NonChargeable,--, r.IsLocked
	dbo.CheckDateRangeOverlap(t.PreconStartDate,t.PreconEndDate,r.AllocationStartDate,r.AllocationEndDate) as IsAllocInPrecon,
	dbo.CheckDateRangeOverlap(t.EstimatedConstructionStart,t.EstimatedConstructionEnd,r.AllocationStartDate,r.AllocationEndDate) as IsAllocInConst,
	dbo.CheckDateRangeOverlap(t.CloseoutStartDate, t.CloseoutDate ,r.AllocationStartDate,r.AllocationEndDate) as IsAllocInCloseOut
	FROM 
	ResourceAllocation r INNER JOIN #tmpTickets t
	ON r.TicketId = t.ProjectId
	WHERE r.ResourceUser = '00000000-0000-0000-0000-000000000000'
	and r.AllocationEndDate >= @StartDate and r.AllocationStartDate <= @EndDate
	and r.RoleId in
	(
		SELECT CASE 
		WHEN @UserFunctionId IS NULL THEN @UserRole
		ELSE RoleId END
		FROM FunctionRoleMapping WHERE FunctionId = @UserFunctionId
	)
	Order by r.AllocationStartDate
	drop table #tmpTickets 
END


GO


insert into Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, TenantID)
values('RMM','This is used to set default display mode either monthly or weekly.','DefaultRMMDisplayMode','Weekly','DefaultRMMDisplayMode', '35525396-E5FE-4692-9239-4DF9305B915B')


