
CREATE procedure [dbo].[usp_GetProjectCapacity]
@TenantID varchar(250),
@StartDate Datetime,
@EndDate Datetime
as
Begin

		select Distinct  LEFT(DATENAME(m, RA.MonthStartDate), 3) as StartMonth,  MONTH(RA.MonthStartDate) MonthOrder,
	Format(SUM(ISNULL(CRM.ApproxContractValue,0) +
	ISNULL(CNS.ApproxContractValue,0) +
	ISNULL(OPM.ApproxContractValue,0)),'C2') as Capacity,
	Count(*) as ProjectCount
	from ResourceUsageSummaryMonthWise RA
	left join CRMProject CRM on RA.WorkItem = CRM.TicketId 
	and CRM.TenantID=@TenantID
	left join CRMServices CNS on RA.WorkItem = CNS.TicketId
	and CNS.TenantID=@TenantID
	left join Opportunity OPM on RA.WorkItem = OPM.TicketId
	and CRM.TenantID=@TenantID
	where RA.TenantID=@TenantID
	and RA.WorkItemType in ('CPR','CNS','OPM') 
	and RA.MonthStartDate >= @StartDate and RA.MonthStartDate <= @EndDate
	group by LEFT(DATENAME(m, RA.MonthStartDate), 3), MONTH(RA.MonthStartDate)
	order by MONTH(RA.MonthStartDate)
End;


