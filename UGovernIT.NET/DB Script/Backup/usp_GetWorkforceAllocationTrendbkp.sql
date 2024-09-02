


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

	select DATEPART(week, table1.week) as weekno,
	min(table1.week) as week,
	--CAST(table1.week as datetime) as week, 
	--DateName(dw, CAST(table1.week as datetime)) as weekday, 
	SUM(CAST(A1 as float)) as A1, SUM(CAST(A2 as float)) as A2 from (
		select  temp.WorkDate as week, SUM(HoursTaken) as A1
		from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		left join Opportunity opm on rw.WorkItem = opm.TicketId
		where WorkDate between @Startdate and @Enddate and rw.WorkItemType in ('CPR','OPM') and temp.TenantID = @TenantID
		group by temp.WorkDate)table1
	join
		(select  temp.WorkDate as week, SUM(HoursTaken) as A2
		from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
		left join CRMProject cpr on rw.WorkItem = cpr.TicketId
		where WorkDate between @Startdate and @Enddate and rw.WorkItemType='CPR' and temp.TenantID = @TenantID
		group by temp.WorkDate)table2
	on table1.week = table2.week
	group by DATEPART(week, table1.week)
	order by DATEPART(week, table1.week)
	--where DateName(dw, CAST(table1.week as datetime)) not in ('Saturday','Sunday')


End;
