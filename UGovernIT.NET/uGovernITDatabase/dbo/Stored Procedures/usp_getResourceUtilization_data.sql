
ALTER procedure [dbo].[usp_getResourceUtilization_data]
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

	declare @SQLStatement NVARCHAR(MAX) = N''

	set @SQLStatement = 'select * from ResourceUtilizationSummary with (nolock) 
						 where TenantID='''+@tenantid+'''
							   and year='''+@date+'''
							   and FlagType='''+@type+'''
							   and IncludeAllResources = '''+@IncludeAllResources+'''
							   and ClosedProjects = '''+@Closed+''''

	if(@IncludeAllResources = 'False')
	begin
		set @SQLStatement = @SQLStatement + ' and ResourceUserAllocated is not null'
	end
	if(@Department != '' and @Department != '0')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))'
	end

	if(@GlobalRoleId != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(Role,'''') = CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(Role,'''')  else '''+@GlobalRoleId +''' END'
	end

	if(@ResourceManager != '' and @ResourceManager != '0')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(ManagerUser,'''') else '''+@ResourceManager +''' end)'
	end

	if(@LevelName != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(LevelName,''0'') in (SELECT Case when len(Item)=0 then Isnull(LevelName,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))'
	end

	if(@Division != '')
	begin
		set @SQLStatement = @SQLStatement + ' and ISNULL(Division,''0'') in (SELECT Case when len(Item)=0 then Isnull(Division,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Division +''','';#'','',''), '',''))'
	end
	
	Print(@SQLStatement)      
	EXEC (@SQLStatement)
end


