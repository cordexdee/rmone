
CREATE procedure [dbo].[usp_GetResourceUtilzationweekly_AllocationHrs]    
--[usp_GetResourceUtilzationweekly_AllocationHrs] '35525396-E5FE-4692-9239-4DF9305B915B',1,1,'PERCENT','','2021-11-01','2021-12-31','0','','','','Weekly','0',''
@TenantID varchar(128),
@IncludeAllResources bit,
@IncludeClosedProject bit,
@DisplayMode varchar(15),
@Department varchar(500),
@StartDate datetime,
@EndDate datetime,
@ResourceManager varchar(max), -- 0 means all users
@AllocationType varchar(50), 
@LevelName varchar(50), 
@GlobalRoleId nvarchar(max),-- RoleId
@Mode varchar(10),--= 'Monthly' 
@ShowAvgColumns bit = 0,
@Function nvarchar(100),
@SoftAllocation bit=0,
@OnlyNCO bit=0
As    
Begin    
	DECLARE @Cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = '', @IsNullCols NVARCHAR(MAX)= ''--, @DataType varchar(15)= ''      
	DECLARE @tblWeeks TABLE  
	(
		ID INT IDENTITY,  
		WeekDayName VARCHAR(50),  
		WeekStartDate DATETIME,  
		WeekEndDate DATETIME  
	)
	INSERT INTO @tblWeeks (WeekDayName, WeekStartDate, WeekEndDate)
		SELECT DATENAME(dw, DateColumn) AS WeekDayName,
			   CASE WHEN DATENAME(dw, DateColumn) = 'Sunday' THEN DATEADD(DAY, 1, DateColumn)
					ELSE DATEADD(DAY, -DATEPART(WEEKDAY, DateColumn) + 2, DateColumn) END AS WeekStartDate,
			   CASE WHEN DATENAME(dw, DateColumn) = 'Sunday' THEN DATEADD(DAY, 7, DateColumn)
					ELSE DATEADD(DAY, 8 - DATEPART(WEEKDAY, DateColumn), DateColumn) END AS WeekEndDate
		FROM (
			SELECT TOP (DATEDIFF(DAY, @StartDate, @EndDate) + 1)
				   DATEADD(DAY, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1, @StartDate) AS DateColumn
			FROM master.dbo.spt_values
	) AS Dates
	WHERE DATENAME(dw, DateColumn) = 'Monday'
  
	SET @cols = STUFF((SELECT ',' + QUOTENAME(Left(DATENAME(MONTH,DATEADD(MONTH, 0, WeekStartDate)),3)+'-'+right(convert(date,WeekStartDate,121),2)+'-' + right(YEAR(WeekStartDate),2))     
				FROM  @tblWeeks     
				FOR XML PATH(''), TYPE    
				).value('.', 'NVARCHAR(MAX)')     
			,1,1,'') 

	SELECT * INTO #tmpTickets
	FROM 
		(
		select TicketId, ApproxContractValue, Closed from CRMProject  where   TenantID = @TenantID and Status != 'Cancelled'  
		Union all 
		select TicketId, ApproxContractValue, Closed from  Opportunity where  TenantID = @TenantID and Status != 'Cancelled' and Closed = 0
		Union all 
		select TicketId, ApproxContractValue, Closed from  CRMServices  where  TenantID = @TenantID and Status != 'Cancelled'  
		) t1;

	CREATE NONCLUSTERED INDEX [IX_tmpTickets_TicketId]
	ON #tmpTickets(TicketId);

-- Table to store total utilization and NonChargeable flag
  CREATE TABLE #tmpResourceAvgUtil (      
	  ResourceID VARCHAR(128),      
	  NonChargeable bit,
	  WorkItemType VARCHAR(250),
	  PctAllocation float
  )
	Declare @SQLAvgCols AS NVARCHAR(MAX) = ''
	Declare @NoOfWeeks as INT
-- Calculating values from ResourceUsageSummaryWeekWise and ResourceAllocation, excluding/including closed projects as per input.
IF(@ShowAvgColumns = 1)
BEGIN
	INSERT INTO #tmpResourceAvgUtil
	select a.ResourceUser, b.NonChargeable, a.WorkItemType, Sum(a.PctAllocation) PctAllocation
	from 
		ResourceUsageSummaryWeekWise a left join 
		(
			select distinct ResourceWorkItemLookup, ResourceUser, NonChargeable, SoftAllocation 
			from ResourceAllocation where TenantID = @TenantID 
		) b
	on 
		a.WorkItemID = b.ResourceWorkItemLookup
	where a.TenantID= @TenantID 
	and b.NonChargeable = CASE WHEN CONVERT(varchar,@OnlyNCO)= 0 then b.NonChargeable else CONVERT(varchar,@OnlyNCO) END
	and b.SoftAllocation = CASE WHEN CONVERT(varchar,@SoftAllocation)= 0 then CONVERT(varchar,@SoftAllocation) else b.SoftAllocation END
	and a.WeekStartDate >=CONVERT(varchar, @StartDate, 121) and a.WeekStartDate <=CONVERT(varchar, @EndDate, 121)
	and a.ResourceUser = b.ResourceUser and a.WorkItem != 'PTO HR'  -- Exclude time off in utilization
	and a.WorkItem in 
		(SELECT case when CONVERT(varchar, @IncludeClosedProject)= '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)
	group by a.ResourceUser, b.NonChargeable, a.WorkItemType

	-- Find out the number of weeks by counting the no. of separators(comma) in the col name list.
	SET @NoOfWeeks = Len(@Cols) - LEN(Replace(@Cols, ',', '')) + 1
	--SET @NoOfWeeks = 1
	-- SQL to be appended in the main sql statement below.
	SET @SQLAvgCols = '(Select CAST(ROUND(ISNULL(SUM(PctAllocation),0)/' + CAST(@NoOfWeeks AS VARCHAR(2)) +',0) AS INT) from #tmpResourceAvgUtil avg where avg.ResourceID = u.Id) AvgerageUtil,
	(Select CAST(ROUND(ISNULL(SUM(PctAllocation),0)/'+ CAST(@NoOfWeeks AS VARCHAR(2)) + ',0) AS INT) from #tmpResourceAvgUtil avg 
	where avg.ResourceID = u.Id and avg.NonChargeable = 0 and avg.WorkItemType !=''OPM'') AvgerageChargeableUtil, '
	
END



	If(@Mode='Weekly')    
	Begin    
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
		'u.Id,
		u.Name  ResourceUser,
		t.ResourceUserAllocated , ' + @Cols + '  
		from AspNetUsers u 
		left join
		(
		Select ResourceUser ResourceUserAllocated , ' + @cols + ' from (
		Select  a.ResourceUser,

		''P:''+Convert(varchar,sum(a.PctAllocation)) +''#''+
		''H:''+Convert(varchar,sum(a.AllocationHour)) +''#''+
		''C:''+Convert(varchar, Count(a.ResourceUser))+''#''+
		''F:''+Convert(varchar, Round((sum(a.PctAllocation)/100),2))+''#'' +
		''A:''+Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round((100-(sum(a.PctAllocation))),2) end))

		--case When ''' + @DisplayMode + '''=''COUNT'' then Convert(varchar, Count(ResourceUser)) 
		--when ''' + @DisplayMode + '''=''PERCENT'' then Isnull(Convert(varchar,(sum(a.PctAllocation))),0)+''%''
		--When ''' + @DisplayMode + '''=''FTE'' then Convert(varchar, Round((sum(a.PctAllocation)/100),2))
		--when ''' + @DisplayMode + '''=''AVAILABILITY'' then
		--Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round((100-(sum(a.PctAllocation))),2) end))+''%'' end 

		PctAllocation,
		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName
		from ResourceUsageSummaryWeekWise a inner join ResourceAllocation res on res.ResourceWorkItemLookup = a.WorkItemID where a.TenantID=''' + @TenantID + ''' 
		and res.AllocationStartDate = a.ActualStartDate 
		and res.AllocationEndDate = a.ActualEndDate
		and a.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else a.SoftAllocation  END
		and res.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then res.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
		and a.WeekStartDate >=''' + CONVERT(varchar, @StartDate, 121)+ ''' and a.WeekStartDate  <=''' + CONVERT(varchar, @EndDate, 121)+ '''
		and Workitem in (SELECT case when ''' + CONVERT(varchar, @IncludeClosedProject)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)
		and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName+''','';#'','',''), '',''))
		
		group by a.WeekStartDate, a.ResourceUser
		)s 
		pivot(max(PctAllocation) for MName  in (' + @cols + ')) p
		) t 
		on u.id=t.ResourceUserAllocated 
		where u.TenantID=''' + @tenantid + ''' and u.isRole=0   
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
    
		print(@SQLStatement)
		EXEC (@SQLStatement)  
		select * from fnGetBillableResources(@TenantID)

	END   
END