
CREATE Procedure [dbo].[usp_GetWorkforceAllocationTrend]
@TenantID varchar(128)  = '',
@Startdate datetime = '',
@Enddate datetime = '',
@Role nvarchar(max) = '',
@Studio nvarchar(max) = '',
@Division bigint = 0,
@Sector nvarchar(max) = ''
as
begin

	select case when week1 is null then week2 else week1 end as week,
		case when A1 is null then 0 else A1 end as A1,
		case when A2 is null then 0 else A2 end as A2,
		case when A3 is null then 0 else A3 end as A3,
		case when AvgPctA1 is null then 0 else AvgPctA1 end as AvgPctA1,
		case when AvgPctA2 is null then 0 else AvgPctA2 end as AvgPctA2,
		case when AvgPctA3 is null then 0 else AvgPctA3 end as AvgPctA3
from (
		select DATEPART(week, table1.week) as weekno,
	min(table1.week) as week1,
	SUM(CAST(A1 as float)) as A3, Convert(float, FORMAT(SUM(AvgPctA3)/Count(1),'N2')) as AvgPctA3 from (
		select  temp.WorkDate as week, SUM(HoursTaken) as A1, 
		(SUM(HoursTaken) / (8.0 * count(1))) * 100 AS AvgPctA3
		from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		left join Opportunity opm on rw.WorkItem = opm.TicketId
		where WorkDate between @Startdate and @Enddate and rw.WorkItemType in ('CPR','OPM') and temp.TenantID = @TenantID
		group by temp.WorkDate
		)table1
	group by DATEPART(week, table1.week)
)timesheet

	full outer join 	
		
 (
	select DATEPART(week, table1.week) as weekno,
	CAST(table1.week as datetime) as week2, 
	CAST(A1 as float) as A1, CAST(A2 as float) as A2, CONVERT(float, Format(table1.AvgPct,'N2')) as AvgPctA1, 
	CONVERT(float, Format(table2.AvgPct,'N2')) as AvgPctA2
	from (
		select  temp.WeekStartDate as week, SUM(AllocationHour) as A1,
		SUM(temp.PctAllocation)/COUNT(1) as AvgPct
		from ResourceUsageSummaryWeekWise temp join ResourceWorkItems rw on temp.WorkItemID = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		left join Opportunity opm on rw.WorkItem = opm.TicketId
		where WeekStartDate between @Startdate and @Enddate and rw.WorkItemType = 'OPM' and temp.TenantID = @TenantID
		group by temp.WeekStartDate)table1
	  left join
		(select  temp.WeekStartDate as week, SUM(AllocationHour) as A2, SUM(temp.PctAllocation)/COUNT(1) as AvgPct
		from ResourceUsageSummaryWeekWise temp join ResourceWorkItems rw on temp.WorkItemID = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		where WeekStartDate between @Startdate and @Enddate and rw.WorkItemType='CPR' and temp.TenantID = @TenantID
		group by temp.WeekStartDate)table2
	on table1.week = table2.week
)weekwise
	on timesheet.week1 = weekwise.week2

End;
