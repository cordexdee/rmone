CREATE PROCEDURE [dbo].[GetProjectCosts]
	@TicketId nvarchar(100), 
	@TenantId nvarchar(128)
AS
BEGIN
	
	select usr.[Name], 
	CONVERT(NVARCHAR, pe.AllocationStartDate, 106) as 'StartDate', 
	CONVERT(NVARCHAR, pe.AllocationEndDate, 106) as 'EndDate', /* pe.AssignedToUser, */ 
	pe.PctAllocation, 
	dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate) as 'days',
	jt.EmpCostRate, 
	dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED') as 'AllocatedAcquisitionCost',
	dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL') as 'ActualACquisitionCost',
	(pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate) * 8) * jt.EmpCostRate) as 'AllocatedResourceCost',
	dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate) as 'ActualResourceCost',
	
	case when (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')) > 0 and (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL')) > 0 then 
	cast(round(((dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')) - (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL'))) / (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')), 1, 1) as numeric(38,1))
	else 0 end as 'Acquisition Cost Variance',
	
	case when (pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate) * 8) * jt.EmpCostRate) > 0 and (dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate)) > 0 then 
	cast(round(((pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate) * 8) * jt.EmpCostRate) - (dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate))) / (pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate) * 8) * jt.EmpCostRate), 1, 1) as numeric(38,1))
	else 0 end as 'Resource Cost Variance' 
	into #temp
	from ProjectEstimatedAllocation pe join AspNetUsers usr on pe.AssignedToUser = usr.Id
	join JobTitleCostRateByDept jt on usr.JobTitleLookup = jt.JobTitleLookup and usr.DepartmentLookup = jt.DeptLookup
	where pe.TenantID = @TenantId and pe.TicketID = @TicketId;

	--select * from #temp

	select SUM(AllocatedAcquisitionCost) as AllocatedAcquisitionCost, SUM(ActualACquisitionCost) as ActualACquisitionCost, 
		   SUM(AllocatedResourceCost) as AllocatedResourceCost, SUM(ActualResourceCost) as ActualResourceCost 
		   into #tempCalculation
		   from #temp

	 select @TicketId as TicketId, @TenantId as TenantId, AllocatedAcquisitionCost, ActualACquisitionCost, AllocatedResourceCost, ActualResourceCost, 
			case when ((AllocatedAcquisitionCost > 0) and (ActualACquisitionCost > 0)) then
			cast(round(((AllocatedAcquisitionCost - ActualACquisitionCost) / AllocatedAcquisitionCost),1,1) as numeric(38,1)) else 0 end as AcquisitionCostVariance,
			case when ((AllocatedResourceCost > 0) and (ActualResourceCost > 0)) then
			cast(round(((AllocatedResourceCost - ActualResourceCost) / AllocatedResourceCost), 1, 1) as numeric(38,1)) else 0 end as ResourceCostVariance
	from #tempCalculation

END