USE [core2]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetNCOHours]    Script Date: 2/22/2023 15:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER Procedure [dbo].[usp_GetNCOHours]
@TenantID nvarchar(128) = '',
@Filter nvarchar(250) = 'Division',
@Studio nvarchar(250) = '',
@Division bigint = 0,
@Sector nvarchar(250) = '',
@Startdate datetime = '',
@Enddate datetime = '' 
as
Begin

	if (@Filter = 'Division')
	begin
		select TotalHrs, CASE When TotalHrs between 1000 and 999999
								THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000.0, 1) AS NUMERIC(4, 1)) )) ) + 'K'
						  When TotalHrs >= 1000000
								THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000000.0, 2) AS NUMERIC(6, 2)) )) ) + 'M'
						  Else CONVERT (VARCHAR(10), TotalHrs) End as TotalHrsLabel,
						  DivisionLookup, cd.Title, ResourceCount as #Projects, ProjectCount as #Resources from (
					select Sum(temp.HoursTaken) as TotalHrs,
					count(distinct [temp].ResourceUser) as ResourceCount
					,count(distinct rw.SubWorkItem) as ProjectCount,
					--temp.ResourceUser, rw.WorkItem, rw.SubWorkItem, rw.StartDate, rw.EndDate, rw.SubSubWorkItem, rw.WorkItemType, temp.Title,
					case when cpr.DivisionLookup is null then opm.DivisionLookup else cpr.DivisionLookup end as DivisionLookup
				from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
				left join CRMProject cpr on rw.WorkItem = cpr.TicketId
				left join Opportunity opm on rw.WorkItem = opm.TicketId
				where temp.TenantID=@TenantID 
				and rw.TenantID = @TenantID and temp.WorkDate between @Startdate and @Enddate
				group by cpr.DivisionLookup, opm.DivisionLookup
		) temp join CompanyDivisions cd on temp.DivisionLookup = cd.ID
	end;
	else if(@Filter = 'Sector')
	begin
			select TotalHrs, CASE When TotalHrs between 1000 and 999999
									THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000.0, 1) AS NUMERIC(4, 1)) )) ) + 'K'
							  When TotalHrs >= 1000000
									THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000000.0, 2) AS NUMERIC(6, 2)) )) ) + 'M'
							  Else CONVERT (VARCHAR(10), TotalHrs) End as TotalHrsLabel,
							  SectorChoice as Title, ResourceCount as #Projects, ProjectCount as #Resources from (
						select Sum(temp.HoursTaken) as TotalHrs,
						count(distinct [temp].ResourceUser) as ResourceCount
						,count(distinct rw.SubWorkItem) as ProjectCount,
						--temp.ResourceUser, rw.WorkItem, rw.SubWorkItem, rw.StartDate, rw.EndDate, rw.SubSubWorkItem, rw.WorkItemType, temp.Title,
						case when cpr.SectorChoice is null then opm.SectorChoice else cpr.SectorChoice end as SectorChoice
					from ResourceTimeSheet temp join ResourceWorkItems rw on temp.ResourceWorkItemLookup = rw.ID
					left join CRMProject cpr on rw.WorkItem = cpr.TicketId
					left join Opportunity opm on rw.WorkItem = opm.TicketId
					where temp.TenantID=@TenantID 
					and rw.TenantID = @TenantID and temp.WorkDate between @Startdate and @Enddate
					group by cpr.SectorChoice, opm.SectorChoice
		) temp 
	End;

End;



