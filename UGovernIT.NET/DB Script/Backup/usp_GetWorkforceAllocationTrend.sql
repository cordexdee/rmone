USE [core2]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetWorkforceAllocationTrend]    Script Date: 2/27/2023 16:02:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER Procedure [dbo].[usp_GetWorkforceAllocationTrend]
@TenantID varchar(128)  = '35525396-E5FE-4692-9239-4DF9305B915B',
@Startdate datetime = '2022-07-01 00:00:00.000',
@Enddate datetime = '2022-12-31 00:00:00.000',
@Role nvarchar(max) = '',
@Studio nvarchar(max) = '',
@Division bigint = 0,
@Sector nvarchar(max) = ''
as
begin

	select case when week1 is null then week2 else week1 end as week,
		case when A1 is null then 0 else A1 end as A1,
		case when A2 is null then 0 else A2 end as A2,
		case when A3 is null then 0 else A3 end as A3
from (
		select DATEPART(week, table1.week) as weekno,
	min(table1.week) as week1,
	SUM(CAST(A1 as float)) as A3 from (
		select  temp.WorkDate as week, SUM(HoursTaken) as A1
		from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		left join Opportunity opm on rw.WorkItem = opm.TicketId
		where WorkDate between @Startdate and @Enddate and rw.WorkItemType in ('CPR','OPM') and temp.TenantID = @TenantID
		group by temp.WorkDate)table1
	group by DATEPART(week, table1.week)
)timesheet

	full outer join 	
		
 (
	select DATEPART(week, table1.week) as weekno,
	CAST(table1.week as datetime) as week2, 
	--DateName(dw, CAST(table1.week as datetime)) as weekday, 
	CAST(A1 as float) as A1, CAST(A2 as float) as A2 from (
		select  temp.WeekStartDate as week, SUM(AllocationHour) as A1
		from ResourceUsageSummaryWeekWise temp join ResourceWorkItems rw on temp.WorkItemID = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		left join Opportunity opm on rw.WorkItem = opm.TicketId
		where WeekStartDate between @Startdate and @Enddate and rw.WorkItemType in ('CPR','OPM') and temp.TenantID = @TenantID
		group by temp.WeekStartDate)table1
	  left join
		(select  temp.WeekStartDate as week, SUM(AllocationHour) as A2
		from ResourceUsageSummaryWeekWise temp join ResourceWorkItems rw on temp.WorkItemID = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		where WeekStartDate between @Startdate and @Enddate and rw.WorkItemType='CPR' and temp.TenantID = @TenantID
		group by temp.WeekStartDate)table2
	on table1.week = table2.week
)weekwise
	on timesheet.week1 = weekwise.week2

End;
