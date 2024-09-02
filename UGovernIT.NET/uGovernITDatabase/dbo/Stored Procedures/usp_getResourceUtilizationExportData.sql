create procedure usp_getResourceUtilizationExportData
@TenantID nvarchar(128)
,@date varchar(4)
,@Department varchar(max)
,@ResourceManager varchar(250)
,@type varchar(15)
,@IncludeAllResources varchar(50)
,@Closed varchar(50)
,@LevelName varchar(100)
,@GlobalRoleId nvarchar(128)
,@Filter nvarchar(250)
,@Studio nvarchar(250)
,@Division varchar(100)
,@Sector nvarchar(250)
as
begin
	
	IF OBJECT_ID('tempdb..##tmpData') IS NOT NULL DROP TABLE ##tmpData;

	declare @SQLStatement NVARCHAR(MAX) = N''

	set @SQLStatement = 'select ItemOrder
							,FullAllocation
							,RUS.Id Id
							,IncludeAllResources
							,ANU.Name ResourceUser
							,ProjectCapacity
							,RevenueCapacity
							,[#LifeTime]
							,[$LifeTime]
							,ResourceUserAllocated
							,case when Jan like ''<%'' then RIGHT(Jan, CHARINDEX('':'', REVERSE(Jan)) -1) else Jan End Jan
							,case when Feb like ''<%'' then RIGHT(Feb, CHARINDEX('':'', REVERSE(Feb)) -1) else Feb End Feb
							,case when Mar like ''<%'' then RIGHT(Mar, CHARINDEX('':'', REVERSE(Mar)) -1) else Mar End Mar
							,case when Apr like ''<%'' then RIGHT(Apr, CHARINDEX('':'', REVERSE(Apr)) -1) else Apr End Apr
							,case when May like ''<%'' then RIGHT(May, CHARINDEX('':'', REVERSE(May)) -1) else May End May
							,case when Jun like ''<%'' then RIGHT(Jun, CHARINDEX('':'', REVERSE(Jun)) -1) else Jun End Jun
							,case when Jul like ''<%'' then RIGHT(Jul, CHARINDEX('':'', REVERSE(Jul)) -1) else Jul End Jul
							,case when Aug like ''<%'' then RIGHT(Aug, CHARINDEX('':'', REVERSE(Aug)) -1) else Aug End Aug
							,case when Sep like ''<%'' then RIGHT(Sep, CHARINDEX('':'', REVERSE(Sep)) -1) else Sep End Sep
							,case when Oct like ''<%'' then RIGHT(Oct, CHARINDEX('':'', REVERSE(Oct)) -1) else Oct End Oct
							,case when Nov like ''<%'' then RIGHT(Nov, CHARINDEX('':'', REVERSE(Nov)) -1) else Nov End Nov
							,case when Dec like ''<%'' then RIGHT(Dec, CHARINDEX('':'', REVERSE(Dec)) -1) else Dec End Dec
							,FlagType
							,Year
							,RUS.TenantID
							,RUS.DepartmentLookup
							,Role
							,RUS.ManagerUser 
							,LevelName
							,ClosedProjects
							,Filter
							,Studio
							,Division
							,Sector   
						 into ##tmpData
						 from ResourceUtilizationSummary RUS with (nolock) 
						 left join AspnetUsers ANU on ANU.Id = RUS.Id
						 where RUS.TenantID='''+@tenantid+'''
							   and year='''+@date+'''
							   and FlagType='''+@type+'''
							   and IncludeAllResources = '''+@IncludeAllResources+'''
							   and ClosedProjects = '''+@Closed+''''
	if(@Department != '' and @Department != '0')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(RUS.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(RUS.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))'
	end

	if(@GlobalRoleId != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(Role,'''') = CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(Role,'''')  else '''+@GlobalRoleId +''' END'
	end

	if(@ResourceManager != '' and @ResourceManager != '0')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(RUS.ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(RUS.ManagerUser,'''') else '''+@ResourceManager +''' end)'
	end

	if(@LevelName != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(LevelName,''0'') in (SELECT Case when len(Item)=0 then Isnull(LevelName,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))'
	end

	if(@Division != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(Division,''0'') in (SELECT Case when len(Item)=0 then Isnull(Division,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Division +''','';#'','',''), '',''))'
	end
	
	--Print(@SQLStatement)      
	EXEC (@SQLStatement)



	select ItemOrder
		,FullAllocation
		,Id
		,IncludeAllResources
		,ResourceUser
		,ProjectCapacity
		,RevenueCapacity
		,[#LifeTime]
		,[$LifeTime]
		,ResourceUserAllocated
		,case when ((Jan like '#%') or (Jan like 'Yellow%')) then LEFT(Jan, CHARINDEX('<', Jan) -1) else Jan End Jan
		,case when ((Feb like '#%') or (Feb like 'Yellow%')) then LEFT(Feb, CHARINDEX('<', Feb) -1) else Feb End Feb
		,case when ((Mar like '#%') or (Mar like 'Yellow%')) then LEFT(Mar, CHARINDEX('<', Mar) -1) else Mar End Mar
		,case when ((Apr like '#%') or (Apr like 'Yellow%')) then LEFT(Apr, CHARINDEX('<', Apr) -1) else Apr End Apr
		,case when ((May like '#%') or (May like 'Yellow%')) then LEFT(May, CHARINDEX('<', May) -1) else May End May
		,case when ((Jun like '#%') or (Jun like 'Yellow%')) then LEFT(Jun, CHARINDEX('<', Jun) -1) else Jun End Jun
		,case when ((Jul like '#%') or (Jul like 'Yellow%')) then LEFT(Jul, CHARINDEX('<', Jul) -1) else Jul End Jul
		,case when ((Aug like '#%') or (Aug like 'Yellow%')) then LEFT(Aug, CHARINDEX('<', Aug) -1) else Aug End Aug
		,case when ((Sep like '#%') or (Sep like 'Yellow%')) then LEFT(Sep, CHARINDEX('<', Sep) -1) else Sep End Sep
		,case when ((Oct like '#%') or (Oct like 'Yellow%')) then LEFT(Oct, CHARINDEX('<', Oct) -1) else Oct End Oct
		,case when ((Nov like '#%') or (Nov like 'Yellow%')) then LEFT(Nov, CHARINDEX('<', Nov) -1) else Nov End Nov
		,case when ((Dec like '#%') or (Dec like 'Yellow%')) then LEFT(Dec, CHARINDEX('<', Dec) -1) else Dec End Dec
		,FlagType
		,Year
		,TenantID
		,DepartmentLookup
		,Role
		,ManagerUser
		,LevelName
		,ClosedProjects
		,Filter
		,Studio
		,Division
		,Sector
	from ##tmpData
	order by ItemOrder
end