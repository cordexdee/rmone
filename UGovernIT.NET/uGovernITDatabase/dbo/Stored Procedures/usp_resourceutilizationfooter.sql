
ALTER procedure [dbo].[usp_resourceutilizationfooter]
@TenantID varchar(128)='', 
  @Fromdate datetime='',      
 @Todate datetime='',       
 @mode varchar(10) = '', --'Daily',
 @Closed bit= '',      
 @type varchar(15) ='',      
 @Department varchar(2000)= '',     
 @IncludeAllResources varchar(10)='',      
 @IsAssignedallocation bit='',      
 @ResourceManager varchar(250)= '',
 @AllocationType varchar(50)='',  
 @LevelName varchar(50)='0',
 @JobProfile varchar(500)='',
 @GlobalRoleId nvarchar(128)='',
 @UserList nvarchar(max) = '',
 @IsManager bit='',
 @Filter nvarchar(250) = '',
 @Studio nvarchar(250) = '',
 @Division bigint = 0,
 @Sector nvarchar(250) = '',
 @url varchar(500)=''
 as
 begin
	
	declare @SQLStatement NVARCHAR(MAX) = N'',
			@cols varchar(max),
			@i as integer, 
			@WeekStartDate DateTime ,  
			@totalrecords int=0,
			@_MonthStartDate DateTime,
			@_MonthEndDate DateTime,
			@WeekEndDate DateTime;
	
	set @SQLStatement = 'select ''AFTE'' ResourceUser,
								 sum(try_convert(float,[Jan])) Jan,
								 SUM(try_convert(float,[Feb])) Feb,
								 SUM(try_convert(float,[Mar])) Mar,
								 SUM(try_convert(float,[Apr])) Apr,
								 SUM(try_convert(float,[May])) May,
								 SUM(try_convert(float,[Jun])) Jun,
								 SUM(try_convert(float,[Jul])) Jul,
								 SUM(try_convert(float,[Aug])) Aug,
								 SUM(try_convert(float,[Sep])) Sep,
								 SUM(try_convert(float,[Oct])) Oct,
								 SUM(try_convert(float,[Nov])) Nov,
								 SUM(try_convert(float,[Dec])) Dec
						 from ResourceUtilizationSummaryFooter 
						 where isRole=0 and TenantID='''+@tenantid+'''
						 and year = year('''+ CONVERT(varchar, @Todate,121) + ''')
						 and ((UGITStartDate between '''+CONVERT(varchar, @Fromdate,121)+''' and '''+CONVERT(varchar, @Todate,121)+''') OR (UGITStartDate < '''+CONVERT(varchar, @Fromdate,121)+'''))
						 and ((UGITEndDate between '''+CONVERT(varchar, @Fromdate,121)+''' and '''+CONVERT(varchar, @Todate,121)+''') OR (UGITEndDate > '''+CONVERT(varchar, @Todate,121)+'''))'

	if(@IncludeAllResources = 'False')
	begin
		set @SQLStatement = @SQLStatement + ' and Enabled = 1 and ResourceUserAllocated is not null'
	end

	if(@Department != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))'
	end

	if(@GlobalRoleId != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(GlobalRoleID,'''') = CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END'
	end

	if(@JobProfile != '')
	begin
		set @SQLStatement = @SQLStatement + ' and isnull(JobProfile,'''') = CASE WHEN '''+@JobProfile +'''='''' then isnull(JobProfile,'''')  else '''+@JobProfile +''' END'
	end

	if(@ResourceManager != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(ManagerUser,'''') else '''+@ResourceManager +''' end)'
	end

	if(@LevelName != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(ModuleNameLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))'
	end
	
	Print(@SQLStatement)      
	EXEC (@SQLStatement)

	IF OBJECT_ID('tempdb..#tmpResourceUtilizationSummary') IS NOT NULL DROP TABLE #tmpResourceUtilizationSummary;

	CREATE TABLE #tmpResourceUtilizationSummary(
	[ItemOrder] [int] NULL,
	[FullAllocation] [varchar](10) NULL,
	[Id] [varchar](max) NULL,
	[IncludeAllResources] [varchar](10) NULL,
	[ResourceUser] [varchar](1000) NULL,
	[ProjectCapacity] [varchar](1000) NULL,
	[RevenueCapacity] [varchar](1000) NULL,
	[#LifeTime] [varchar](1000) NULL,
	[$LifeTime] [varchar](1000) NULL,
	[ResourceUserAllocated] [varchar](1000) NULL,
	[Jan] [varchar](max) NULL,
	[Feb] [varchar](max) NULL,
	[Mar] [varchar](max) NULL,
	[Apr] [varchar](max) NULL,
	[May] [varchar](max) NULL,
	[Jun] [varchar](max) NULL,
	[Jul] [varchar](max) NULL,
	[Aug] [varchar](max) NULL,
	[Sep] [varchar](max) NULL,
	[Oct] [varchar](max) NULL,
	[Nov] [varchar](max) NULL,
	[Dec] [varchar](max) NULL,
	[FlagType] [varchar](20) NULL,
	[Year] [varchar](5) NULL,
	[TenantID] [varchar](max) NULL,
	[DepartmentLookup] [varchar](100) NULL,
	[Role] [varchar](max) NULL,
	[ManagerUser] [varchar](max) NULL,
	[LevelName] [varchar](1000) NULL,
	[ClosedProjects] [varchar](10) NULL,
	[Filter] [nvarchar](250) NULL,
	[Studio] [nvarchar](250) NULL,
	[Division] [bigint] NULL,
	[Sector] [nvarchar](250) NULL
)

	Declare @allProjects varchar(10);
	set @allProjects = (select case when @Closed = 'False' then 'False' else 'True' end)
	set @SQLStatement = 'select * from ResourceUtilizationSummary with (nolock)
						 where TenantID='''+@tenantid+'''
						 and FlagType='''+@type+'''
						 and IncludeAllResources='''+@IncludeAllResources+'''
						 and ClosedProjects='''+@allProjects+'''
						 and year = year('''+ CONVERT(varchar, @Todate,121) + ''')
						 '

	if(@IncludeAllResources = 'False')
	begin
		set @SQLStatement = @SQLStatement + ' and ResourceUserAllocated is not null'
	end

	if(@Department != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))'
	end

	if(@GlobalRoleId != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(Role,'''') = CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(Role,'''')  else '''+@GlobalRoleId +''' END'
	end

	if(@JobProfile != '')
	begin
		set @SQLStatement = @SQLStatement + ' and isnull(JobProfile,'''') = CASE WHEN '''+@JobProfile +'''='''' then isnull(JobProfile,'''')  else '''+@JobProfile +''' END'
	end

	if(@ResourceManager != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(ManagerUser,'''') else '''+@ResourceManager +''' end)'
	end
	if(@LevelName != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(LevelName,''0'') in (SELECT Case when len(Item)=0 then Isnull(LevelName,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))'
	end

	Print(@SQLStatement)     
	
	insert into #tmpResourceUtilizationSummary
	EXEC (@SQLStatement)

	declare @Jan float, @Feb float, @Mar float,@Apr float,@May float,@Jun float,@Jul float,@Aug float,@Sep float,@Oct float,@Nov float,@Dec float
	set @Jan = (select count(Jan) as Jan from #tmpResourceUtilizationSummary where Jan <> '')
	set @Feb = (select count(Feb) as Jan from #tmpResourceUtilizationSummary where Feb <> '')
	set @Mar = (select count(Mar) as Jan from #tmpResourceUtilizationSummary where Mar <> '')
	set @Apr = (select count(Apr) as Jan from #tmpResourceUtilizationSummary where Apr <> '')
	set @May = (select count(May) as Jan from #tmpResourceUtilizationSummary where May <> '')
	set @Jun = (select count(Jun) as Jan from #tmpResourceUtilizationSummary where Jun <> '')
	set @Jul = (select count(Jul) as Jan from #tmpResourceUtilizationSummary where Jul <> '')
	set @Aug = (select count(Aug) as Jan from #tmpResourceUtilizationSummary where Aug <> '')
	set @Sep = (select count(Sep) as Jan from #tmpResourceUtilizationSummary where Sep <> '')
	set @Oct = (select count(Oct) as Jan from #tmpResourceUtilizationSummary where Oct <> '')
	set @Nov = (select count(Nov) as Jan from #tmpResourceUtilizationSummary where Nov <> '')
	set @Dec = (select count(Dec) as Jan from #tmpResourceUtilizationSummary where Dec <> '')

	select 'TFTE' as ResourceUser,@Jan Jan,@Feb Feb,@Mar Mar,@Apr Apr,@May May,@Jun Jun,@Jul Jul,@Aug Aug,@Sep Sep,@Oct Oct,@Nov Nov,@Dec Dec

end

