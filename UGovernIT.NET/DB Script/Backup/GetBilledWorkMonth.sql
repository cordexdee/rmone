
ALTER Procedure [dbo].[GetBilledWorkMonth]
@tenantID nvarchar(128) = ''
as
begin

	Select Month(mk.MonthStartDate) as MonthNumber, LEFT(DATENAME(m, mk.MonthStartDate), 3) as StartMonth,
				  Sum(mk.TotalBillingLaborRate) as TotalBillingLaborRate,
		Sum(TotalEmployeeCostRate) as TotalEmployeeCostRate,
		Sum(mk.GrossMargin) as GrossMargin, count(mk.workitem) TotalProjects,
		round((Sum(mk.PctAllocation)/100),2) BilledWorkMonth,
		Abs(round((Sum(mk.PctAllocation)/100),2) - 100) UnBilledWorkMonth,
		round((Sum(mk.PctAllocation)/count(distinct Name)),2) Utilization,
		Sum(mk.PctAllocation) TotalUtilization
		from (
		Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
		((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate,
		((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate) as TotalEmployeeCostRate,
		(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)) as GrossMargin
		from ResourceUsageSummaryMonthWise RA
		left join AspNetUsers NU on RA.ResourceUser = NU.Id
		join JobTitle JT on JT.ID = NU.JobTitleLookup
		where RA.TenantID=@tenantID
		and NU.Enabled = 1
		and (JT.JobType = case when '1'='1' then 'Billable' else '' end
		or JT.JobType = case when '0'='1' then 'Overhead' else '' end)
		and JT.Deleted = 0
		and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
		and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))) mk left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID=@tenantID 
				left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID=@tenantID 
				left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID=@tenantID
				where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
				and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

End;
