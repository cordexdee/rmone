CREATE Procedure [dbo].[usp_GetBenchReportChartData]
	@TenantID varchar(128), 
	@IncludeAllResources bit, 
	@IncludeClosedProject bit, 
	@DisplayMode varchar(15), --'FTE', PERCENT , COUNT, AVAILABILITY
	@Department varchar(500), --'574,570',
	@StartDate datetime, 
	@EndDate datetime, 
	@ResourceManager varchar(max), -- 0 means all users
	@AllocationType varchar(50), 
	@LevelName varchar(50), 
	@GlobalRoleId nvarchar(128), -- RoleId
	@Mode varchar(10), 
	@Function varchar(10) 
As
BEGIN
DECLARE @SQLStatement nvarchar(max)
SET @SQLStatement =	'Select  
		Round((sum(a.AllocationHour)),2)/Count(a.ResourceUser)
		PctAllocation,
		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-''+ right(YEAR(a.MonthStartDate),2) MName
		from ResourceUsageSummaryMonthWise a where a.TenantID=''' + @TenantID + '''
		and a.monthstartdate >=''' + CONVERT(varchar, @StartDate, 121) + ''' and a.monthstartdate  <='''  + CONVERT(varchar,DATEADD(year, 1, @StartDate), 121) + '''
		and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM DBO.SPLITSTRING(replace('''','';#'','',''), '',''))'
		--and WorkItem not in (SELECT case when ''0'' = ''0'' then  TicketId else '' end from #tmpClosedTickets )
if (LEN(ISNULL(@GlobalRoleId,'')) > 0)
BEGIN
	SET @SQLStatement =	@SQLStatement + ' and a.GlobalRoleID = ''' + @GlobalRoleId + ''''
END

if (len(isnull(@Department,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.ResourceUser in (Select u.Id from AspNetUsers u where u.DepartmentLookup in 
	(SELECT VALUE FROM string_split(''' + @Department + ''', '',''))) '
END		

if (LEN(ISNULL(@ResourceManager,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.ResourceUser in (Select u.Id from AspNetUsers u WHERE ManagerUser =''' + @ResourceManager + ''')'
END		

if (LEN(ISNULL(@Function,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.GlobalRoleID in (Select RoleId FROM FunctionRoleMapping WHERE FunctionId =''' + @Function + ''')'
END		

SET @SQLStatement =	@SQLStatement + ' group by a.MonthStartDate--, a.ResourceUser'
--select @SQLStatement 
EXEC sp_executesql @SQLStatement

END

if (len(isnull(@Department,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.ResourceUser in (Select u.Id from AspNetUsers u where u.DepartmentLookup in 
	(SELECT VALUE FROM string_split(''' + @Department + ''', '',''))) '
END		

if (LEN(ISNULL(@ResourceManager,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.ResourceUser in (Select u.Id from AspNetUsers u WHERE ManagerUser =''' + @ResourceManager + ''')'
END		

if (LEN(ISNULL(@Function,'')) > 0)		
BEGIN 
	SET @SQLStatement =	@SQLStatement + ' and a.GlobalRoleID in (Select RoleId FROM FunctionRoleMapping WHERE FunctionId =''' + @Function + ''')'
END		

SET @SQLStatement =	@SQLStatement + ' group by a.WeekStartDate--, a.ResourceUser'
print( @SQLStatement )
EXEC sp_executesql @SQLStatement

END
						

Select      Round((sum(a.AllocationHour)),2)/Count(a.ResourceUser)    PctAllocation,    Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+'-'+right(convert(date,a.MonthStartDate,121),2)+'-'+ right(YEAR(a.MonthStartDate),2) MName    from ResourceUsageSummaryMonthWise a where a.TenantID='35525396-e5fe-4692-9239-4df9305b915b'    and a.monthstartdate >='Mar  4 2024 12:00AM' and a.monthstartdate  <='Mar  4 2024 12:00AM'    and ISNULL(a.WorkItemType,'') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'') else Item end  FROM DBO.SPLITSTRING(replace('',';#',','), ',')) and a.ResourceUser in (Select u.Id from AspNetUsers u where u.DepartmentLookup in    (SELECT VALUE FROM string_split('586,587', ',')))  group by a.MonthStartDate--, a.ResourceUser

Select  Round((sum(a.AllocationHour)),2)/Count(a.ResourceUser)    PctAllocation,    
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+'-'+right(convert(date,a.MonthStartDate,121),2)+'-'+ right(YEAR(a.MonthStartDate),2) MName    
from ResourceUsageSummaryMonthWise a where a.TenantID='35525396-e5fe-4692-9239-4df9305b915b'    
--and a.monthstartdate >='Mar  4 2024 12:00AM'  and a.monthstartdate  <='Mar  4 2024 12:00AM'    
and ISNULL(a.WorkItemType,'') in 
(SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'') else Item end  FROM DBO.SPLITSTRING(replace('',';#',','), ',')) 
group by a.MonthStartDate--, a.ResourceUser

select * from ResourceUsageSummaryMonthWise 

select CONVERT(varchar,'Mar  4 2024 12:00AM',20)

Select  Round((sum(a.AllocationHour)),2)/Count(a.ResourceUser)    PctAllocation,    
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+'-'+right(convert(date,a.MonthStartDate,121),2)+'-'+ right(YEAR(a.MonthStartDate),2) MName
from ResourceUsageSummaryMonthWise a where a.TenantID='35525396-e5fe-4692-9239-4df9305b915b'    
and a.monthstartdate >='2024-03-01 00:00:00.000' and a.monthstartdate  <='2024-03-01 00:00:00.000'   
and ISNULL(a.WorkItemType,'') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'') else Item end  
FROM DBO.SPLITSTRING(replace('',';#',','), ',')) and a.ResourceUser in (Select u.Id from AspNetUsers u where u.DepartmentLookup in   
(SELECT VALUE FROM string_split('586', ',')))  group by a.MonthStartDate--, a.ResourceUser

