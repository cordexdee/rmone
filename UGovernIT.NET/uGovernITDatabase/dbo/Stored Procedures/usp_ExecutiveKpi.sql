
ALTER PROCEDURE [dbo].[usp_ExecutiveKpi]
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','role'
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','jobtitle'
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','sector'
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','division'
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','studio'
-- usp_ExecutiveKpi '35525396-e5fe-4692-9239-4df9305b915b','2021','projectview'
	@TenantId nvarchar(256),
	@Year varchar(5),
	@Category nvarchar(50),
	@SelectedDepartments nvarchar(1000) = ''
AS
BEGIN
	--Declare @Query nvarchar(500);
	Declare @i int;
	
	Declare @StartDate date;
	Declare @EndDate date;
	Declare @CurrentDate date;
	Declare @tmpDate date;

	set @StartDate = @Year + '-' + '01' + '-' + '01';
	set @EndDate = @Year + '-' + '12' + '-' + '31';
	set @CurrentDate = GETDATE();

	Declare @StartMonth int;
	Declare @EndMonth int;
	Set @StartMonth = MONTH(@StartDate);
	Set @EndMonth = MONTH(@EndDate);
	
	CREATE TABLE #tmpResourceAllocation(	
		ResourceUser nvarchar(max),
		[Resource] nvarchar(max),
		DepartmentLookup bigint,
		AllocationStartDate datetime,
		AllocationEndDate datetime,
		PctAllocation float,
		ResourceWorkItemLookup bigint,
		RoleId nvarchar(256),
		[Role] nvarchar(max),
		JobTitle nvarchar(200),
		TicketID nvarchar(256),
		TicketTitle nvarchar(256),
		ProjectId nvarchar(256),
		PreconStartDt datetime,
		ConstructionEndDt datetime,
		TenantID nvarchar(128),
		TotalWorkingDays int,
		FltrDaysFromStartDt int,
		FltrDaysToEndDt int,
		FltrHrsFromStartDt float,
		FltrHrsToEndDt float,
		BillRate decimal,
		Cost decimal,
		Margins decimal,
		ProjectedMargins decimal,
		ProjectMargins decimal,
		ProjectedBillings decimal,
		ProjectedCosts decimal,
		CurrentBillings decimal,
		CurrentResourceCosts decimal,
		ResourceHours decimal,
		ResourceBillings decimal,
		ResourceCosts decimal,
		ResourceMargins decimal,
		Sector nvarchar(250),
		Division nvarchar(250),
		Studio bigint
	)
	
	INSERT INTO #tmpResourceAllocation (
		ResourceUser,
		AllocationStartDate,
		AllocationEndDate,
		PctAllocation,
		ResourceWorkItemLookup,
		RoleId,
		TicketID,
		TenantID,
		[Resource],
		JobTitle,
		DepartmentLookup
	)
	select RA.ResourceUser, RA.AllocationStartDate, RA.AllocationEndDate, RA.PctAllocation, RA.ResourceWorkItemLookup, RA.RoleId, RA.TicketID, RA.TenantID,
	usr.[Name], usr.JobProfile, usr.DepartmentLookup
	  from ResourceAllocation RA 
	right join AspNetUsers usr on RA.ResourceUser = usr.Id
	 where RA.TenantID = @TenantId
	and RA.AllocationStartDate >= @StartDate and RA.AllocationEndDate <= @EndDate
	and usr.isRole = 0 and usr.Enabled = 1
	and usr.JobProfile in (select Title from JobTitle where TenantID = @TenantId and JobType = 'Billable' and Deleted = 0)
	--and  isnull(usr.DepartmentLookup,'') in ( CASE WHEN LEN(@SelectedDepartments)=0 then isnull(usr.DepartmentLookup,'')  else (SELECT item FROM DBO.SPLITSTRING('''+ @SelectedDepartments +''', '')) END ) 
	order by RA.AllocationStartDate, RA.RoleId


	update tmp
	set tmp.Sector = cpr.SectorChoice, tmp.Division = cpr.CRMBusinessUnitChoice, tmp.TicketTitle = cpr.Title, tmp.ProjectId = cpr.ProjectId, tmp.PreconStartDt = ISNULL(cpr.PreconStartDate, cpr.EstimatedConstructionStart), tmp.ConstructionEndDt = ISNULL(cpr.EstimatedConstructionEnd, cpr.PreconEndDate), tmp.Studio = cpr.StudioLookup
	from #tmpResourceAllocation tmp
	join CRMProject cpr
	on tmp.TicketID = cpr.TicketId
	where cpr.TenantID = @TenantId;

	update tmp
	set tmp.Sector = opm.SectorChoice, tmp.Division = opm.CRMBusinessUnitChoice, tmp.TicketTitle = opm.Title, tmp.ProjectId = opm.ProjectId, tmp.PreconStartDt = ISNULL(opm.PreconStartDate, opm.EstimatedConstructionStart), tmp.ConstructionEndDt = ISNULL(opm.EstimatedConstructionEnd, opm.PreconEndDate), tmp.Studio = opm.StudioLookup
	from #tmpResourceAllocation tmp
	join Opportunity opm
	on tmp.TicketID = opm.TicketId
	where opm.TenantID = @TenantId;

	update tmp
	set tmp.Division = cns.CRMBusinessUnitChoice, tmp.TicketTitle = cns.Title, tmp.ProjectId = cns.ProjectId, tmp.PreconStartDt = ISNULL(cns.PreconStartDate, cns.EstimatedConstructionStart), tmp.ConstructionEndDt = cns.EstimatedConstructionEnd, tmp.Studio = cns.StudioLookup
	from #tmpResourceAllocation tmp
	join CRMServices cns
	on tmp.TicketID = cns.TicketId
	where cns.TenantID = @TenantId;

	update tmp
	set tmp.[Role] = r.[Name]
	from #tmpResourceAllocation tmp
	join Roles r
	on tmp.RoleId = r.Id
	where r.TenantID = @TenantId;

	update #tmpResourceAllocation set TotalWorkingDays = DATEDIFF(day, AllocationStartDate, AllocationEndDate) + 1;

	update #tmpResourceAllocation set FltrDaysFromStartDt = 
	Case when @CurrentDate >= AllocationEndDate Then DATEDIFF(day, @StartDate, AllocationEndDate) + 1
		 when @CurrentDate <= AllocationEndDate Then DATEDIFF(day, @StartDate, @CurrentDate) + 1
		 End;

	update #tmpResourceAllocation set FltrDaysToEndDt = 
	Case when @CurrentDate >= AllocationEndDate Then 0
		 when @CurrentDate <= AllocationEndDate Then DATEDIFF(day, @CurrentDate, AllocationEndDate) + 1
		 End;

	update #tmpResourceAllocation set FltrHrsFromStartDt = (FltrDaysFromStartDt * PctAllocation * 8), 
								FltrHrsToEndDt = (FltrDaysToEndDt * PctAllocation * 8);

	update tmp
	set tmp.[BillRate] = jt.BillingLaborRate, tmp.[Cost] = jt.EmployeeCostRate
	from #tmpResourceAllocation tmp
	join JobTitle jt
	on tmp.RoleId = jt.RoleId and tmp.JobTitle = jt.Title and tmp.DepartmentLookup = jt.DepartmentId
	where jt.TenantID = @TenantId;

	update #tmpResourceAllocation
	set [BillRate] = 0
	where [BillRate] is NULL;

	update #tmpResourceAllocation
	set [Cost] = 0
	where [Cost] is NULL;

	update #tmpResourceAllocation set Margins = (FltrHrsFromStartDt * (BillRate - Cost)),
							ProjectedMargins = (FltrHrsToEndDt * (BillRate - Cost)),
							ProjectMargins = (TotalWorkingDays * PctAllocation * 8 * (BillRate - Cost)),
							ProjectedBillings = (FltrHrsToEndDt * BillRate),
							ProjectedCosts = (FltrHrsToEndDt * Cost),
							CurrentBillings = (FltrHrsFromStartDt * BillRate),
							CurrentResourceCosts = (FltrHrsFromStartDt * Cost);

	update #tmpResourceAllocation set ResourceHours  = (FltrHrsFromStartDt + FltrHrsToEndDt),
							ResourceBillings  = (CurrentBillings + ProjectedBillings),
							ResourceCosts  = (CurrentResourceCosts + ProjectedCosts);

	update #tmpResourceAllocation set ResourceMargins  = (ResourceBillings - ResourceCosts);

	If @Category = 'role'
	Begin
			select [Role], sum(Margins) as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins, 
			sum(ProjectedBillings) as ProjectedBillings, sum(ProjectedCosts) as ProjectedCosts, 
			dbo.fnGetEffectiveUtilization(@TenantId,@StartDate,@CurrentDate,'role',[Role]) as 'EffectiveUtilization', 
			dbo.fnGetCommittedUtilization(@TenantId,@CurrentDate,@EndDate,'role',[Role]) as 'CommittedUtilization', 
			dbo.fnGetPipelineUtilization(@TenantId,@StartDate,@EndDate,'role',[Role]) as 'PipelineUtilization', 
			dbo.fnGetResourceRevenues(@TenantId,@StartDate,@CurrentDate,'','role',[Role]) as 'RevenuesRealized',
			dbo.fnGetResourceRevenues(@TenantId,@CurrentDate,@EndDate,'0','role',[Role]) as 'CommittedRevenues', 
			dbo.fnGetRevenuesLost(@TenantId,@StartDate,@CurrentDate,'role',[Role]) as 'RevenuesLost', 
			dbo.fnGetPipelineRevenues(@TenantId,@StartDate,@EndDate,'role',[Role]) as 'PipelineRevenues', 
			0 as 'MarginsRealized', 0 as 'MarginsLost', 0 as 'CommittedMargins' from #tmpResourceAllocation
			group by [Role]
			having LEN([Role]) > 0
		End
	Else If @Category = 'jobtitle'
		Begin
			select [JobTitle], sum(Margins)  as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins,
			sum(ProjectedBillings) as ProjectedBillings, sum(ProjectedCosts) as ProjectedCosts,sum(ResourceHours) as ResourceHours, 
			sum(ResourceBillings) as ResourceBillings,sum(ResourceCosts) as ResourceCosts,sum(ResourceMargins) as ResourceMargins, 
			dbo.fnGetEffectiveUtilization(@TenantId,@StartDate,@CurrentDate,'jobtitle',[JobTitle]) as 'EffectiveUtilization', 
			dbo.fnGetCommittedUtilization(@TenantId,@CurrentDate,@EndDate,'jobtitle',[JobTitle]) as 'CommittedUtilization', 
			dbo.fnGetPipelineUtilization(@TenantId,@StartDate,@EndDate,'jobtitle',[JobTitle]) as 'PipelineUtilization', 
			dbo.fnGetResourceRevenues(@TenantId,@StartDate,@CurrentDate,'','jobtitle',[jobtitle]) as 'RevenuesRealized', 
			dbo.fnGetResourceRevenues(@TenantId,@CurrentDate,@EndDate,'0','jobtitle',[jobtitle]) as 'CommittedRevenues', 
			dbo.fnGetRevenuesLost(@TenantId,@StartDate,@CurrentDate,'jobtitle',[jobtitle]) as 'RevenuesLost', 
			dbo.fnGetPipelineRevenues(@TenantId,@StartDate,@EndDate,'jobtitle',[JobTitle]) as 'PipelineRevenues', 
			0 as 'MarginsRealized', 0 as 'MarginsLost', 0 as 'CommittedMargins' from #tmpResourceAllocation
			group by [JobTitle]
			having LEN([JobTitle]) > 0
		End
	Else If @Category = 'sector'
		Begin
			select Sector, sum(Margins)  as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins, 
			sum(ProjectedBillings) as ProjectedBillings, sum(ProjectedCosts) as ProjectedCosts,sum(ResourceHours) as ResourceHours, 
			sum(ResourceBillings) as ResourceBillings,sum(ResourceCosts) as ResourceCosts,sum(ResourceMargins) as ResourceMargins, 
			dbo.fnGetEffectiveUtilization(@TenantId,@StartDate,@CurrentDate,'sector',Sector) as 'EffectiveUtilization', 
			dbo.fnGetCommittedUtilization(@TenantId,@CurrentDate,@EndDate,'sector',Sector) as 'CommittedUtilization', 
			dbo.fnGetPipelineUtilization(@TenantId,@StartDate,@EndDate,'sector',Sector) as 'PipelineUtilization', 
			dbo.fnGetResourceRevenues(@TenantId,@StartDate,@CurrentDate,'','sector',[sector]) as 'RevenuesRealized', 
			dbo.fnGetResourceRevenues(@TenantId,@CurrentDate,@EndDate,'0','sector',[sector]) as 'CommittedRevenues', 
			dbo.fnGetRevenuesLost(@TenantId,@StartDate,@CurrentDate,'sector',[sector]) as 'RevenuesLost', 
			dbo.fnGetPipelineRevenues(@TenantId,@StartDate,@EndDate,'sector',Sector) as 'PipelineRevenues', 
			dbo.fnGetResourceMargins(@TenantId,@StartDate,@CurrentDate,'','sector',[sector]) as 'MarginsRealized', 
			dbo.fnGetResourceMargins(@TenantId,@CurrentDate,@EndDate,'0','sector',[sector]) as 'CommittedMargins', 
			dbo.fnGetMarginsLost(@TenantId,@StartDate,@CurrentDate,'sector',[sector]) as 'MarginsLost' from #tmpResourceAllocation
			group by Sector
			having LEN(Sector) > 0
		End
	Else If @Category = 'division'
		Begin
			select Division, sum(Margins)  as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins, sum(ProjectedBillings) as ProjectedBillings, sum(ProjectedCosts) as ProjectedCosts,sum(ResourceHours) as ResourceHours, sum(ResourceBillings) as ResourceBillings,sum(ResourceCosts) as ResourceCosts,sum(ResourceMargins) as ResourceMargins, dbo.fnGetEffectiveUtilization(@TenantId,@StartDate,@CurrentDate,'division',Division) as 'EffectiveUtilization', dbo.fnGetCommittedUtilization(@TenantId,@CurrentDate,@EndDate,'division',Division) as 'CommittedUtilization', dbo.fnGetPipelineUtilization(@TenantId,@StartDate,@EndDate,'division',Division) as 'PipelineUtilization', dbo.fnGetResourceRevenues(@TenantId,@StartDate,@CurrentDate,'','division',[division]) as 'RevenuesRealized', dbo.fnGetResourceRevenues(@TenantId,@CurrentDate,@EndDate,'0','division',[division]) as 'CommittedRevenues', dbo.fnGetRevenuesLost(@TenantId,@StartDate,@CurrentDate,'division',[division]) as 'RevenuesLost', dbo.fnGetPipelineRevenues(@TenantId,@StartDate,@EndDate,'division',Division) as 'PipelineRevenues', dbo.fnGetResourceMargins(@TenantId,@StartDate,@CurrentDate,'','division',[division]) as 'MarginsRealized', dbo.fnGetResourceMargins(@TenantId,@CurrentDate,@EndDate,'0','division',[division]) as 'CommittedMargins', dbo.fnGetMarginsLost(@TenantId,@StartDate,@CurrentDate,'division',[division]) as 'MarginsLost' from #tmpResourceAllocation
			group by Division
			having LEN(Division) > 0
		End
	Else If @Category = 'studio'
		Begin
			select Studio, sum(Margins)  as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins, 
				sum(ProjectedBillings) as ProjectedBillings, sum(ProjectedCosts) as ProjectedCosts,sum(ResourceHours) as ResourceHours, 
				sum(ResourceBillings) as ResourceBillings,sum(ResourceCosts) as ResourceCosts,sum(ResourceMargins) as ResourceMargins, 
				dbo.fnGetEffectiveUtilization(@TenantId,@StartDate,@CurrentDate,'studio',Studio) as 'EffectiveUtilization', 
				dbo.fnGetCommittedUtilization(@TenantId,@CurrentDate,@EndDate,'studio',Studio) as 'CommittedUtilization', 
				dbo.fnGetPipelineUtilization(@TenantId,@StartDate,@EndDate,'studio',Studio) as 'PipelineUtilization', 
				dbo.fnGetResourceRevenues(@TenantId,@StartDate,@CurrentDate,'','studio',[Studio]) as 'RevenuesRealized', 
				dbo.fnGetResourceRevenues(@TenantId,@CurrentDate,@EndDate,'0','studio',[Studio]) as 'CommittedRevenues', 
				dbo.fnGetRevenuesLost(@TenantId,@StartDate,@CurrentDate,'studio',[Studio]) as 'RevenuesLost', 
				dbo.fnGetPipelineRevenues(@TenantId,@StartDate,@EndDate,'studio',Studio) as 'PipelineRevenues', 
				dbo.fnGetResourceMargins(@TenantId,@StartDate,@CurrentDate,'','studio',[Studio]) as 'MarginsRealized', 
				dbo.fnGetResourceMargins(@TenantId,@CurrentDate,@EndDate,'0','studio',[Studio]) as 'CommittedMargins', 
				dbo.fnGetMarginsLost(@TenantId,@StartDate,@CurrentDate,'studio',[Studio]) as 'MarginsLost' from #tmpResourceAllocation
			group by Studio
			having Studio > 0
		End
	Else If @Category = 'projectview'
		Begin
			select ProjectId, TicketID, TicketTitle, dbo.fnGetUtilizationRate(@TenantId, TicketID, PreconStartDt, ConstructionEndDt) as 'UtilizationRate',
				sum(Margins)  as Margins, sum(ProjectedMargins) as ProjectedMargins, sum(ProjectMargins) as ProjectMargins, sum(ProjectedBillings) as ProjectedBillings,
				sum(ProjectedCosts) as ProjectedCosts,sum(ResourceHours) as ResourceHours, sum(ResourceBillings) as ResourceBillings,
				sum(ResourceCosts) as ResourceCosts,sum(ResourceMargins) as ResourceMargins, 0 as 'EffectiveUtilization', 
				0 as 'CommittedUtilization', 0 as 'PipelineUtilization', 0 as 'RevenuesRealized', 0 as 'CommittedRevenues', 0 as 'RevenuesLost', 
				0 as 'PipelineRevenues', 0 as 'MarginsRealized', 0 as 'CommittedMargins', 0 as 'MarginsLost' from #tmpResourceAllocation
			group by ProjectId, TicketID, TicketTitle , PreconStartDt, ConstructionEndDt
			having LEN(TicketID) > 0
			order by ProjectId desc
		End

	Drop table #tmpResourceAllocation;

END
