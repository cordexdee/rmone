CREATE Procedure usp_GetNCOHoursTemp
@TenantID nvarchar(128) = '',
@Filter nvarchar(250) = 'Division',
@Studio nvarchar(250) = '',
@Division bigint = 0,
@Sector nvarchar(250) = ''
as
Begin

	select TotalHrs, CASE When TotalHrs between 1000 and 999999
							THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000.0, 1) AS NUMERIC(4, 1)) )) ) + 'K'
					  When TotalHrs >= 1000000
							THEN ( CONVERT (VARCHAR(10), ( CAST(ROUND(TotalHrs / 1000000.0, 2) AS NUMERIC(6, 2)) )) ) + 'M'
					  Else CONVERT (VARCHAR(10), TotalHrs) End as TotalHrsLabel,
					  DivisionLookup, cd.Title, ResourceCount as #Projects, ProjectCount as #Resources from (
	select Sum(temp.NWHR) as TotalHrs, 
					case when cpr.DivisionLookup is null then opm.DivisionLookup
							else cpr.DivisionLookup end as DivisionLookup, count(distinct [Employee Name]) as ResourceCount
							,count(distinct [Job Name]) as ProjectCount 
							from timesheettemp temp left join CRMProject cpr on temp.job = cpr.ERPJobID or temp.job = cpr.ProjectId
	left join Opportunity opm on temp.job = opm.ERPJobID or temp.job = opm.ProjectId
	group by cpr.DivisionLookup, opm.DivisionLookup
	) temp join CompanyDivisions cd on temp.DivisionLookup = cd.ID

End;