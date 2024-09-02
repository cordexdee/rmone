
ALTER procedure [dbo].[usp_GetResourceUtlizationFooter_New]      
 @TenantID varchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069' ,   
 @Fromdate datetime ='2023-01-01 00:00:00.000',      
 @Todate datetime ='2023-12-01 00:00:00.000',       
 @Mode varchar(10) = 'Monthly', --'Daily',
 @Closed bit = 'False',      
 @type varchar(15) ='FTE',      
 @Department varchar(2000) = '', -- '570',     
 @IncludeAllResources bit ='True',      
 @IsAssignedallocation bit ='False',      
 @ResourceManager varchar(250) = 'f66c66d3-55d4-47dc-8094-33804dfad0a8',
 @AllocationType varchar(250) = 'Estimated',
 @LevelName varchar(50) ='',
 @JobProfile varchar(500) ='', --'Assistant Project Manager',
 @GlobalRoleId nvarchar(128) ='',
 @UserList nvarchar(max) = '',
 @IsManager  bit =1, 
 @Filter nvarchar(250) = '',   @Studio nvarchar(250) = '',   @Division bigint = 0,   @Sector nvarchar(250)   = '',
 @url nvarchar(max)=''
As      
begin     
	
	DECLARE @cols AS NVARCHAR(MAX), @isnullcols AS NVARCHAR(MAX), @issumcols AS NVARCHAR(MAX), @SQLStatement AS NVARCHAR(MAX)

	if @Department = '0'
	begin
		SET @Department = ''
	end

	SELECT * INTO #tmpClosedTickets	FROM (	select TicketId from CRMProject where TenantID = @TenantID and Closed = 1	Union all																					select TicketId from Opportunity where TenantID = @TenantID and Closed = 1	Union all																					select TicketId from CRMServices where TenantID = @TenantID and Closed = 1	)t;

	SET @cols = STUFF((SELECT ',' + QUOTENAME(months)             FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode)             FOR XML PATH(''), TYPE            ).value('.', 'NVARCHAR(MAX)')         ,1,1,'')

	SET @isnullcols = STUFF((SELECT ',' + 'Isnull(' + QUOTENAME(months) + ',''0'')' + QUOTENAME(months)					FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 				FOR XML PATH(''), TYPE				).value('.', 'NVARCHAR(MAX)') 			,1,1,'')

	SET @issumcols = STUFF((SELECT ',' + 'Sum(' + QUOTENAME(months) + ')' + QUOTENAME(months)					FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 				FOR XML PATH(''), TYPE				).value('.', 'NVARCHAR(MAX)') 			,1,1,'')

	SET @SQLStatement = 'select ''Total FTE'' ResourceUser,'''','''','''','''','''' , '+@issumcols+' FROM (		Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder,		u.Name  ResourceUser,		t.ResourceUserAllocated , 		' + @isnullcols+ '  		from AspNetUsers u 		left join		(		Select ResourceUser ResourceUserAllocated , '+@cols+' from (		Select  a.ResourceUser,		case When '''+@type+'''=''COUNT'' then (Count(ResourceUser)) 		when '''+@type+'''=''PERCENT'' then Convert(varchar, (sum(a.PctAllocation)*100/100))		When '''+@type+'''=''FTE'' then Round((sum(a.PctAllocation)/100),2)		when '''+@type+'''=''AVAILABILITY'' then		Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end)) end		PctAllocation,		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName		from ResourceAllocationMonthly a where a.TenantID='''+@TenantID+'''		and a.monthstartdate >='''+ CONVERT(varchar, @Fromdate,121)+''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''		and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''','''',''0'','''' ) )		and ResourceWorkItem not in 					(SELECT case when '''+CONVERT(varchar,@Closed)+''' = ''0'' then  TicketId else '''' end from #tmpClosedTickets )		and ISNULL(a.ResourceWorkItemType,'''') in 			(SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''','';#'','',''), '',''))		group by a.MonthStartDate, a.ResourceUser		)s 		pivot(max(PctAllocation) for MName  in (' + @cols + ')) p		) t 		on u.id=t.ResourceUserAllocated 		where u.TenantID='''+@tenantid+''' and u.isRole=0  		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))		and ISNULL(u.ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerUser,'''') else '''+@ResourceManager +''' end)		and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END		and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END		AND (				('''+CONVERT(varchar,@IncludeAllResources)+''' = 0 AND ISNULL(t.ResourceUserAllocated, ''0'') != ''0'')				OR				('''+CONVERT(varchar,@IncludeAllResources)+''' = 1)			)		  AND (				-- Apply the u.Enabled condition only when @IncludeAllResources = 1				('''+CONVERT(varchar,@IncludeAllResources)+''' = 0 AND u.Enabled = 1)				OR				-- Include all records when @IncludeAllResources = 0				('''+CONVERT(varchar,@IncludeAllResources)+''' = 1)			)		and ((not(year(u.UGITStartDate) < year('''+ CONVERT(varchar, @Fromdate,121)+''')  and year(u.UGITEndDate) < year('''+ CONVERT(varchar, @Fromdate,121)+'''))		or (year(u.UGITStartDate) > year('''+ CONVERT(varchar, @Todate,121)+''') and year(u.UGITEndDate) > year('''+ CONVERT(varchar, @Todate,121)+'''))))
		)temp'
	
	Print(@SQLStatement)
	exec (@SQLStatement)
	
End;