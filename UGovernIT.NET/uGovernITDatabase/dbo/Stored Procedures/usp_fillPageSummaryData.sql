CREATE procedure [dbo].[usp_fillPageSummaryData]
as
begin

	declare @complexity float, 
			@volatility float, 
			@Title varchar(max), 
			@estimatedStartDate datetime, 
			@estimatedEndDate datetime, 
			@module varchar(5),
			@startPC int = 1,
			@countPC int,
			@startCS int = 1,
			@countCS int;

	if OBJECT_ID('tempdb..#tmpProjects') is not null
	begin
		drop table #tmpProjects
	end

	if OBJECT_ID('tempdb..#tmpProjectsCosts') is not null
	begin
		drop table #tmpProjectsCosts
	end

	if OBJECT_ID('tempdb..#tmpComplexitityStart') is not null
	begin
		drop table #tmpComplexitityStart
	end

	if OBJECT_ID('tempdb..#tmpProjectsRN') is not null
	begin
		drop table #tmpProjectsRN
	end

	if OBJECT_ID('tempdb..#tmpAllocation') is not null
	begin
		drop table #tmpAllocation
	end
	
	if OBJECT_ID('tempdb..#tmpCountAllocation') is not null
	begin
		drop table #tmpCountAllocation
	end

	if OBJECT_ID('tempdb..#tmpCountAllocationPercentile') is not null
	begin
		drop table #tmpCountAllocationPercentile
	end
	
	if OBJECT_ID('tempdb..#tmpVolatilityFinal') is not null
	begin
		drop table #tmpVolatilityFinal
	end
	
	if OBJECT_ID('tempdb..#tmpActualHours') is not null
	begin
		drop table #tmpActualHours
	end

	create table #tmpProjectsCosts
	(
		TicketId varchar(100),
		TenantId nvarchar(256),
		AllocatedAcquisitionCost float,
		ActualACquisitionCost float,	
		AllocatedResourceCost float,
		ActualResourceCost float,
		AcquisitionCostVariance	float,
		ResourceCostVariance float
	)

	create table #tmpProjects
	(	
		[TenantID] [nvarchar](256) NULL,
		[Title] [nvarchar](255) NULL,
		[TicketId] [nvarchar](250) NULL,
		[ERPJobID] [nvarchar](500) NULL,
		[EnableStdWorkItems] [bit] NULL,
		[AcquisitionCost] [float] NULL,
		[Volatility] [float] NULL,
		[ERPJobIDNC] [nvarchar](500) NULL,
		[ERPJobID_old] [nvarchar](500) NULL,
		[ERPJobIDNC_old] [nvarchar](500) NULL,
		AllocatedAcquisitionCost float,
		ActualAcquisitionCost float,
		AllocatedResourceCost float,
		ActualResourceCost float,
		ResourceCostVariance float,
		AcquisitionCostVariance float
	)

	create table #tmpComplexitityStart
	(
		[TenantID] [nvarchar](256) NULL,
		[Title] [nvarchar](255) NULL,
		[TicketId] [nvarchar](250) NULL,
		[ERPJobID] [nvarchar](500) NULL,
		[EnableStdWorkItems] [bit] NULL,
		[AcquisitionCost] [float] NULL,
		[Volatility] [float] NULL,
		[ERPJobIDNC] [nvarchar](500) NULL,
		[ERPJobID_old] [nvarchar](500) NULL,
		[ERPJobIDNC_old] [nvarchar](500) NULL,
		AllocatedAcquisitionCost float,
		ActualAcquisitionCost float,
		AllocatedResourceCost float,
		ActualResourceCost float,
		ResourceCostVariance float,
		AcquisitionCostVariance float
	)
	
	insert into #tmpProjects
	select TenantID
			,Title
			,TicketId
			,ERPJobID
			,EnableStdWorkItems
			,AcquisitionCost
			,Volatility
			,ERPJobIDNC
			,ERPJobID_old
			,ERPJobIDNC_old
			,0 AllocatedAcquisitionCost,
			0 ActualAcquisitionCost,
			0 AllocatedResourceCost,
			0 ActualResourceCost,
			0 ResourceCostVariance,
			0 AcquisitionCostVariance
	from CRMProject

	insert into #tmpProjects
	select TenantID
			,Title
			,TicketId
			,ERPJobID
			,EnableStdWorkItems
			,AcquisitionCost
			,Volatility
			,ERPJobIDNC
			,ERPJobID_old
			,ERPJobIDNC_old
			,0 AllocatedAcquisitionCost,
			0 ActualAcquisitionCost,
			0 AllocatedResourceCost,
			0 ActualResourceCost,
			0 ResourceCostVariance,
			0 AcquisitionCostVariance
	from Opportunity

	insert into #tmpProjects
	select TenantID
			,Title
			,TicketId
			,ERPJobID
			,EnableStdWorkItems
			,AcquisitionCost
			,null Volatility
			,ERPJobIDNC
			,null ERPJobID_old
			,null ERPJobIDNC_old
			,0 AllocatedAcquisitionCost,
			0 ActualAcquisitionCost,
			0 AllocatedResourceCost,
			0 ActualResourceCost,
			0 ResourceCostVariance,
			0 AcquisitionCostVariance
	from CRMServices

	insert into #tmpComplexitityStart
	select TenantID
			,Title
			,TicketId
			,ERPJobID
			,EnableStdWorkItems
			,AcquisitionCost
			,Volatility
			,ERPJobIDNC
			,ERPJobID_old
			,ERPJobIDNC_old
			,0 AllocatedAcquisitionCost,
			0 ActualAcquisitionCost,
			0 AllocatedResourceCost,
			0 ActualResourceCost,
			0 ResourceCostVariance,
			0 AcquisitionCostVariance
	from CRMProject
	where AcquisitionCost > 0 

	insert into #tmpComplexitityStart
	select TenantID
			,Title
			,TicketId
			,ERPJobID
			,EnableStdWorkItems
			,AcquisitionCost
			,Volatility
			,ERPJobIDNC
			,ERPJobID_old
			,ERPJobIDNC_old
			,0 AllocatedAcquisitionCost,
			0 ActualAcquisitionCost,
			0 AllocatedResourceCost,
			0 ActualResourceCost,
			0 ResourceCostVariance,
			0 AcquisitionCostVariance
	from Opportunity
	where AcquisitionCost > 0

	select row_number() over (order by TicketId) row_num, 
	* 
	into #tmpProjectsRN
	from #tmpProjects 

	set @countPC = (select count(*) from #tmpProjects)

	declare @ticketid varchar(100), @tenantid varchar(max)

	while(@startPC <= @countPC)
	begin
		set @ticketid = (select TicketId from #tmpProjectsRN where row_num = @startPC)
		set @tenantid = (select TenantID from #tmpProjectsRN where row_num = @startPC)

		insert into #tmpProjectsCosts
		exec GetProjectCosts @TicketId, @TenantId

		set @startPC = @startPC + 1
	end

	while(@startCS <= @countCS)
	begin
		set @ticketid = (select TicketId from #tmpProjectsRN where row_num = @startCS)
		set @tenantid = (select TenantID from #tmpProjectsRN where row_num = @startCS)

		insert into #tmpProjectsCosts
		exec GetProjectCosts @TicketId, @TenantId

		set @startCS = @startCS + 1
	end
	

	update A
	set A.AllocatedAcquisitionCost = Isnull(B.AllocatedAcquisitionCost, 0), 
		A.ActualAcquisitionCost = Isnull(B.ActualACquisitionCost, 0),
		A.AllocatedResourceCost = Isnull(B.AllocatedResourceCost, 0),
		A.ActualResourceCost =Isnull(B.ActualResourceCost, 0),
		A.AcquisitionCostVariance = Isnull(B.AcquisitionCostVariance,0),
		A.ResourceCostVariance = Isnull(B.ResourceCostVariance,0)
	from #tmpProjects A
	inner join #tmpProjectsCosts B on A.TicketId = B.TicketId and A.TenantID = B.TenantId

	update A
	set A.AllocatedAcquisitionCost = Isnull(B.AllocatedAcquisitionCost, 0), 
		A.ActualAcquisitionCost = Isnull(B.ActualACquisitionCost, 0),
		A.AllocatedResourceCost = Isnull(B.AllocatedResourceCost, 0),
		A.ActualResourceCost =Isnull(B.ActualResourceCost, 0),
		A.AcquisitionCostVariance = Isnull(B.AcquisitionCostVariance,0),
		A.ResourceCostVariance = Isnull(B.ResourceCostVariance,0)
	from #tmpComplexitityStart A
	inner join #tmpProjectsCosts B on A.TicketId = B.TicketId and A.TenantID = B.TenantId

	/*Complexity Start*/
	select TicketId, Title, TenantID,
	case when ActualAcquisitionCost > 0 then ActualAcquisitionCost
		else AllocatedAcquisitionCost end as Cost
	into #tmpComplexitityMid
	from #tmpComplexitityStart

	select TicketId, Title, TenantID, sum(cost) Cost 
	into #tmpComplexitityMid1
	from #tmpComplexitityMid group by TicketId, Title, TenantID
	
	select TicketId, Title, TenantID, Cost, 
	cast(round(PERCENT_RANK() over(order by Cost),2,2) as numeric(38,3)) as percentile 
	into #tmpComplexitityLast 
	from #tmpComplexitityMid1
	where Cost is not null
	
	select TicketId, Title, TenantID, Cost, percentile,
	case when percentile between 0 and 0.19 then 1
		 when percentile between 0.20 and 0.39 then 2
		 when percentile between 0.40 and 0.59 then 3
		 when percentile between 0.60 and 0.79 then 4
		 when percentile between 0.80 and 1 then 5
	else 0 end as Complexitity
	into #tmpComplexityFinal
	from #tmpComplexitityLast
	/*End*/

	update CP
	set CP.Complexity = isnull(TCF.Complexitity, 0)
	from CRMProject CP inner join #tmpComplexityFinal TCF on CP.TicketId = TCF.TicketId and CP.TenantId = TCF.TenantID

	update CS
	set CS.Complexity = isnull(TCF.Complexitity, 0)
	from CRMServices CS inner join #tmpComplexityFinal TCF on CS.TicketId = TCF.TicketId and CS.TenantId = TCF.TenantID

	update Opp
	set Opp.Complexity = isnull(TCF.Complexitity, 0)
	from Opportunity Opp inner join #tmpComplexityFinal TCF on Opp.TicketId = TCF.TicketId and Opp.TenantId = TCF.TenantID

	/*Volatility Start*/

	create table #tmpAllocation
	(
		row_num int,
		Title varchar(max),
		TicketId varchar(1000),
		TenantID varchar(max),
		[Count] int
	)

	insert into #tmpAllocation
	select ROW_NUMBER() over(partition by Title order by TicketId) as row_num, 
	   Title, 
	   TicketId, 
	   TenantID,
	   dbo.fn_GetPhraseCount(History, 'allocation') as [count]
	from CRMProject 
	where History like '%allocation%'

	insert into #tmpAllocation
	select ROW_NUMBER() over(partition by Title order by TicketId) as row_num, 
	Title, 
	TicketId, 
	TenantID,
	dbo.fn_GetPhraseCount(History, 'allocation') as [count]
	from Opportunity 
	where History like '%allocation%'

	insert into #tmpAllocation
	select ROW_NUMBER() over(partition by Title order by TicketId) as row_num, 
	Title, 
	TicketId, 
	TenantID,
	dbo.fn_GetPhraseCount(History, 'allocation') as [count] 
	from CRMServices 
	where History like '%allocation%'
	
	select Title, TicketId, TenantID, SUM(count) as AllocationCount into #tmpCountAllocation from #tmpAllocation group by Title, TicketId, TenantID
	
	select Title, TicketId, TenantID, AllocationCount,
	cast(round(PERCENT_RANK() over(order by AllocationCount),2,2) as numeric(38,3)) as percentile
	into #tmpCountAllocationPercentile
	from #tmpCountAllocation
	
	select Title, TicketId, TenantID, AllocationCount, percentile,
	case when percentile between 0 and 0.19 then 1
		 when percentile between 0.20 and 0.39 then 2
		 when percentile between 0.40 and 0.59 then 3
		 when percentile between 0.60 and 0.79 then 4
		 when percentile between 0.80 and 1 then 5
	else 0 end as Volitility
	into #tmpVolatilityFinal
	from #tmpCountAllocationPercentile
	/*End*/

	/*Added to update volatility in summary tables*/

	update CP
	set CP.Volatility = isnull(TVF.Volitility, 0)
	from CRMProject CP inner join #tmpVolatilityFinal TVF on CP.TicketId = TVF.TicketId and CP.TenantId = TVF.TenantID

	update CS
	set CS.Volatility = isnull(TCF.Volitility, 0)
	from CRMServices CS inner join #tmpVolatilityFinal TCF on CS.TicketId = TCF.TicketId and CS.TenantId = TCF.TenantID

	update Opp
	set Opp.Volatility = isnull(TCF.Volitility, 0)
	from Opportunity Opp inner join #tmpVolatilityFinal TCF on Opp.TicketId = TCF.TicketId and Opp.TenantId = TCF.TenantID

	/*End*/

	select ROW_NUMBER() over(order by AllocationStartDate) row_num, * 
	into #tmpProjectAllocation 
	from ProjectEstimatedAllocation

	select ROW_NUMBER() over(order by WorkDate) row_num, * 
	into #tmpTicketHours 
	from TicketHours 

	select AllocationStartDate, AllocationEndDate, AssignedToUser, TicketId, TenantID into #tmpProjectAllocationGB 
	from #tmpProjectAllocation group by TenantID, TicketId, AssignedToUser, AllocationStartDate, AllocationEndDate
	
	select AssignedToUser, TenantID into #tmpProjectAllocation_Cost from #tmpProjectAllocation group by TenantID, AssignedToUser
	
	select TPAC.AssignedToUser, TPAC.TenantID, 
	case when (ANU.HourlyRate <> 0 and ANU.HourlyRate is not null) then convert(float, ANU.HourlyRate)
	else JTCRD.EmpCostRate
	end as Cost 
	into #tmpProjectAllocation_Cost_Resource
	from #tmpProjectAllocation_Cost TPAC 
	inner join AspNetUsers ANU on TPAC.AssignedToUser = ANU.Id and TPAC.TenantID = ANU.TenantID 
	inner join JobTitleCostRateByDept JTCRD on JTCRD.DeptLookup = ANU.DepartmentLookup and JTCRD.JobTitleLookup = ANU.JobTitleLookup
	
	select b.*, a.Cost as BillingCost, 0 ResourceHoursPrecon, 0 ResourceHoursToDate, 0 ResourceHoursRemaining, 0.0 TotalResourceCost 
	into #tmpPAfinal 
	from #tmpProjectAllocation_Cost_Resource a 
	inner join #tmpProjectAllocationGB b on a.AssignedToUser = b.AssignedToUser and a.TenantID = b.TenantID
	
	select AllocationStartDate, AllocationEndDate, AssignedToUser, TicketId, TenantID, BillingCost,
	ISNULL((DATEDIFF(DAY, AllocationStartDate, AllocationEndDate) * 8), 0) as ResourceHoursPrecon,
	
	(case when AllocationEndDate is null then 0
	when AllocationEndDate < getdate() then isnull((DATEDIFF(DAY, AllocationStartDate, AllocationEndDate) * 8), 0) 
	else isnull((DATEDIFF(DAY, AllocationStartDate, getdate()) * 8), 0) 
	end) as ResourceHoursToDate,
	
	ISNULL((DATEDIFF(DAY, AllocationStartDate, AllocationEndDate) * 8), 0) - (case when AllocationEndDate is null then 0
	when AllocationEndDate < getdate() then isnull((DATEDIFF(DAY, AllocationStartDate, AllocationEndDate) * 8), 0) 
	else isnull((DATEDIFF(DAY, AllocationStartDate, getdate()) * 8), 0) 
	end) as ResourceHoursRemaining,
	ISNULL((DATEDIFF(MONTH, AllocationStartDate, AllocationEndDate) * BillingCost), 0) as TotalResourceCost
	into #test
	from #tmpPAfinal 

	select TicketId, TenantID, 
		   SUM(ResourceHoursPrecon) ResourceHoursPrecon, 
		   SUM(ResourceHoursToDate) ResourceHoursToDate, 
		   SUM(ResourceHoursRemaining) ResourceHoursRemaining, 
		   SUM(TotalResourceCost) TotalResourceCost 
	into #testfinal
	from #test 
	group by TicketId, TenantID

	update CP
	set CP.TotalResourceCost = isnull(TF.TotalResourceCost,0),
		CP.ResourceHoursPrecon = isnull(TF.ResourceHoursPrecon,0),
		CP.ResourceHoursRemaining = ABS(ISNULL((TF.ResourceHoursPrecon - TF.ResourceHoursToDate),0)) 
	from CRMProject CP left join #testfinal TF on CP.TicketId = TF.TicketId and CP.TenantId = TF.TenantID 

	update CS
	set CS.TotalResourceCost = isnull(TF.TotalResourceCost,0),
		CS.ResourceHoursPrecon = isnull(TF.ResourceHoursPrecon,0),
		CS.ResourceHoursRemaining = ABS(ISNULL((TF.ResourceHoursPrecon - TF.ResourceHoursToDate),0)) 
	from CRMServices CS left join #testfinal TF on CS.TicketId = TF.TicketId and CS.TenantId = TF.TenantID

	update Opp
	set Opp.TotalResourceCost = isnull(TF.TotalResourceCost,0),
		Opp.ResourceHoursPrecon = isnull(TF.ResourceHoursPrecon,0),
		Opp.ResourceHoursRemaining = ABS(ISNULL((TF.ResourceHoursPrecon - TF.ResourceHoursToDate),0)) 
	from Opportunity Opp left join #testfinal TF on Opp.TicketId = TF.TicketId and Opp.TenantId = TF.TenantID

	select TicketId, 
		   TenantID,
		   SUM(HoursTaken) as HoursTaken 
	into #tmpActualHours
	from #tmpTicketHours 
	group by TicketId, 
			 TenantID--, 

	update CP
	set CP.ResourceHoursBilledtoDate = isnull(TAH.HoursTaken,0),
		CP.ResouceHoursBilled = isnull(TAH.HoursTaken,0),
		CP.ResourceHoursActual = isnull(TAH.HoursTaken,0)
	from CRMProject CP left join #tmpActualHours TAH on CP.TicketId = TAH.TicketId and CP.TenantId = TAH.TenantID

	update CS
	set CS.ResourceHoursBilledtoDate = isnull(TAH.HoursTaken,0),
		CS.ResouceHoursBilled = isnull(TAH.HoursTaken,0),
		CS.ResourceHoursActual = isnull(TAH.HoursTaken,0)
	from CRMServices CS left join #tmpActualHours TAH on CS.TicketId = TAH.TicketId and CS.TenantId = TAH.TenantID

	update Opp
	set Opp.ResourceHoursBilledtoDate = isnull(TAH.HoursTaken,0),
		Opp.ResouceHoursBilled = isnull(TAH.HoursTaken,0),
		Opp.ResourceHoursActual = isnull(TAH.HoursTaken,0)
	from Opportunity Opp left join #tmpActualHours TAH on Opp.TicketId = TAH.TicketId and Opp.TenantId = TAH.TenantID

	update CRMProject set TotalResourceHours = ISNULL((ResourceHoursPrecon + ResourceHoursActual), 0)
	update CRMServices set TotalResourceHours = ISNULL((ResourceHoursPrecon + ResourceHoursActual), 0)
	update Opportunity set TotalResourceHours = ISNULL((ResourceHoursPrecon + ResourceHoursActual), 0)

end