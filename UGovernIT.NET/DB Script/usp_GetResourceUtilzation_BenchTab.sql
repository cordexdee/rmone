
CREATE Procedure [dbo].[usp_GetResourceUtilzation_BenchTab] 
--[usp_GetResourceUtilzation_BenchTab] '35525396-E5FE-4692-9239-4DF9305B915B',1,0,'FTE','','2023-01-01 00:00:00.000','2023-12-31 00:00:00.000','0','','','','Monthly','0','1'
@TenantID varchar(128), 
@IncludeAllResources bit, 
@IncludeClosedProject bit, 
@DisplayMode varchar(15), --'FTE', PERCENT , COUNT, AVAILABILITY
@Department varchar(500),--'574,570',
@StartDate datetime,
@EndDate datetime, 
@ResourceManager varchar(max)= '0', -- 0 means all users
@AllocationType varchar(50), 
@LevelName varchar(50), 
@GlobalRoleId nvarchar(max), -- RoleId
@Mode varchar(10),
@ShowAvgColumns bit = 0,
@Function varchar(100),
@SoftAllocation bit=0,
@OnlyNCO bit=0
As Begin 
DECLARE @Cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = '', @IsNullCols NVARCHAR(MAX)= '', @DataType varchar(15)= ''  

	SELECT * INTO #tmpTickets
	FROM 
		(
		select TicketId, ApproxContractValue, Closed from CRMProject  where   TenantID = @TenantID  and Status != 'Cancelled'   
		Union all 
		select TicketId, ApproxContractValue, Closed from  Opportunity where  TenantID = @TenantID and Status != 'Cancelled' and Closed = 0
		Union all 
		select TicketId, ApproxContractValue, Closed from  CRMServices  where  TenantID = @TenantID and Status != 'Cancelled'  
		) t1;

	CREATE NONCLUSTERED INDEX [IX_tmpTickets_TicketId]
	ON #tmpTickets(TicketId);

	SET 
	  @cols = STUFF(
		(
		  SELECT ',' + QUOTENAME(months) 
		  FROM dbo.GetMonthListForResourceAllocation(@StartDate, @EndDate, @Mode) FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'), 
		1, 
		1, 
		''
	  ) 
	SET 
	  @IsNullCols = STUFF(
		(
		  SELECT 
			',' + 'Isnull(' + QUOTENAME(months) + ','''')' + QUOTENAME(months) 
		  FROM 
			dbo.GetMonthListForResourceAllocation(@StartDate, @EndDate, @Mode) FOR XML PATH(''), 
			TYPE
		).value('.', 'NVARCHAR(MAX)'), 
		1, 
		1, 
		''
	  ) 

-- Table to store total utilization and NonChargeable flag
  CREATE TABLE #tmpResourceAvgUtil (      
  ResourceID VARCHAR(128),      
  NonChargeable bit,
  WorkItemType VARCHAR(250),
  PctAllocation float
  )
Declare @SQLAvgCols AS NVARCHAR(MAX) = ''
Declare @NoOfWeeks as INT

-- Calculating values from ResourceUsageSummaryMonthWise and ResourceAllocation, excluding/including closed projects as per input.
IF(@ShowAvgColumns = 1)
BEGIN
	INSERT INTO #tmpResourceAvgUtil
	select a.ResourceUser, b.NonChargeable, a.WorkItemType, Sum(a.PctAllocation) PctAllocation
	from 
		ResourceUsageSummaryMonthWise a left join 
		(
			select distinct ResourceWorkItemLookup, ResourceUser, NonChargeable, SoftAllocation 
			from ResourceAllocation where TenantID = @TenantID 
		) b
	on 
		a.WorkItemID = b.ResourceWorkItemLookup
	where a.TenantID= @TenantID 
	and b.NonChargeable = CASE WHEN CONVERT(varchar,@OnlyNCO)= 0 then b.NonChargeable  else CONVERT(varchar,@OnlyNCO) END
	and b.SoftAllocation = CASE WHEN CONVERT(varchar,@SoftAllocation)= 0 then CONVERT(varchar,@SoftAllocation) else b.SoftAllocation END
	and a.monthstartdate >=CONVERT(varchar, @StartDate, 121) and a.monthstartdate <=CONVERT(varchar, @EndDate, 121)
	and a.ResourceUser = b.ResourceUser and a.WorkItem != 'PTO HR'  -- Exclude time off from utilization
	and a.WorkItem in 
		(SELECT case when CONVERT(varchar, @IncludeClosedProject)= '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)
	group by a.ResourceUser, b.NonChargeable, a.WorkItemType

	-- Find out the number of weeks by counting the no. of separators(comma) in the col name list.
	SET @NoOfWeeks = Len(@Cols) - LEN(Replace(@Cols, ',', '')) + 1

	-- SQL to be appended in the main sql statement below.
	SET @SQLAvgCols = '(Select CAST(ROUND(ISNULL(SUM(PctAllocation),0)/' + CAST(@NoOfWeeks AS VARCHAR(2)) +',0) AS INT) from #tmpResourceAvgUtil avg 
	where avg.ResourceID = u.Id) AvgerageUtil,
	(Select CAST(ROUND(ISNULL(SUM(PctAllocation),0)/'+ CAST(@NoOfWeeks AS VARCHAR(2)) + ',0) AS INT) from #tmpResourceAvgUtil avg 
	where avg.ResourceID = u.Id  and avg.NonChargeable = 0 and avg.WorkItemType !=''OPM'') AvgerageChargeableUtil, '
END


SET 
  @SQLStatement = 'Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder,
					(Select Count(distinct(x.TicketId)) from ResourceAllocation x where x.TenantID=''' + @Tenantid + '''
					and x.resourceUser =u.Id and TicketId != ''PTO HR''
					and x.TicketId in (SELECT case when ''' + CONVERT(varchar, @IncludeClosedProject)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)
					)''ProjectCapacity'',
					(Select (CONVERT (VARCHAR(10), (CAST(ROUND(ISNULL(SUM(rc.ApproxContractValue),0) / 1000000.0, 2) AS NUMERIC(6, 1))))) from (
					Select x.TicketId, y.ApproxContractValue from ResourceAllocation x Left join #tmpTickets y 
					on x.TicketId = y.TicketId where x.TenantID=''' + @Tenantid + '''
					and x.resourceUser =u.Id '

if (@IncludeClosedProject = 0)
Begin	
	SET @SQLStatement = @SQLStatement + ' and y.Closed = 0'
end


SET @SQLStatement = @SQLStatement + ' group by x.TicketId, y.ApproxContractValue)rc )''RevenueCapacity'','
					+ @SQLAvgCols +
					' u.Id, u.Name  ResourceUser, 
					t.ResourceUserAllocated , ' + @IsNullCols + '  
					from AspNetUsers u 
					left join
					(
					Select ResourceUser ResourceUserAllocated , ' + @cols + ' from (
					Select  a.ResourceUser,
					''P:''+Convert(varchar,sum(a.PctAllocation)) +''#''+
					''H:''+Convert(varchar, Round(sum(a.AllocationHour),0)) +''#''+
					''C:''+Convert(varchar, Count(a.ResourceUser))+''#''+
					''F:''+Convert(varchar, Round((sum(a.PctAllocation)/100),2))+''#''+
					''A:''+Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round((100-(sum(a.PctAllocation))),2) end))
					PctAllocation,
					Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
					from ResourceUsageSummaryMonthWise a inner join ResourceAllocation res on res.ResourceWorkItemLookup = a.WorkItemID where a.TenantID=''' + @TenantID + '''
					and a.monthstartdate >=''' + CONVERT(varchar, @StartDate, 121)+ ''' and a.monthstartdate  <=''' + CONVERT(varchar, @EndDate, 121)+ '''
					and res.AllocationStartDate = a.ActualStartDate 
					and res.AllocationEndDate = a.ActualEndDate
					and a.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else a.SoftAllocation  END
					and res.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then res.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''   END
					and Workitem in (SELECT case when ''' + CONVERT(varchar, @IncludeClosedProject)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)
					and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName+''','';#'','',''), '',''))
					group by a.MonthStartDate, a.ResourceUser
					)s 
					pivot(max(PctAllocation) for MName  in (' + @cols + ')) p
					) t 
					on u.id=t.ResourceUserAllocated 
					--left join #tmpResourceAvgUtil avg
					--on u.id = avg.UserName
					where u.TenantID=''' + @tenantid + ''' and u.isRole=0   
					--and u.id in(''4f2fca19-784b-4b13-b1b7-fdc3989ebd0f'')
					and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace(''' + @Department + ''','';#'','',''), '',''))
					and ISNULL(u.ManagerUser,'''') in (Case when ''' + @ResourceManager + ''' =''0'' then Isnull(u.ManagerUser,'''') else ''' + @ResourceManager + ''' end)
					and ISNULL(u.GlobalRoleID,'''') in (SELECT Case when len(Item)=0 then Isnull(u.GlobalRoleID,'''') else Item end  FROM  DBO.SPLITSTRING('''+@GlobalRoleId +''', '','')) 
					AND (
							(''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 0 AND ISNULL(t.ResourceUserAllocated, ''0'') != ''0'')
							OR
							(''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 1)
						)
					  AND (
							-- Apply the u.Enabled condition only when @IncludeAllResources = 1
							(''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 0 AND u.Enabled = 1)
							OR
							-- Include all records when @IncludeAllResources = 0
							(''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 1)
						)
					and ((not(year(u.UGITStartDate) < year(''' + CONVERT(varchar, @StartDate, 121)+ ''')  and year(u.UGITEndDate) < year(''' + CONVERT(varchar, @StartDate, 121)+ '''))
					or (year(u.UGITStartDate) > year(''' + CONVERT(varchar, @StartDate, 121)+ ''') and year(u.UGITEndDate) > year(''' + CONVERT(varchar, @StartDate, 121)+ '''))))' 

					if(len(@Function) > 0)
					begin
					 set @SQLStatement = @SQLStatement + ' and u.GlobalRoleID in (Select RoleId FROM FunctionRoleMapping WHERE FunctionId in (select cast(Item as bigint) from DBO.SPLITSTRING(''' + @Function + ''','','')))'
					end

					set @SQLStatement = @SQLStatement + ' order by u.Name' 

Print(@SQLStatement);
EXEC(@SQLStatement);
DROP TABLE #tmpTickets;
DROP table #tmpResourceAvgUtil;
select * from fnGetBillableResources(@TenantID)
		
END
