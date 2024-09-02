CREATE procedure [dbo].[usp_realTimeUpdation]
@TenantID varchar(128)= '',
@Fromdate datetime='',
@Todate datetime='', 
@mode varchar(10) ='',
@Closed bit= '',
@type varchar(15) = '', --FTE', PERCENT , COUNT, AVAILABILITY
@Department varchar(2000)='', --'570',
@IncludeAllResources bit='',
@IsAssignedallocation bit='',
@ResourceManager varchar(250)='',
@url varchar(500)='',
@AllocationType varchar(50)='',
@LevelName varchar(50)='',
@JobProfile varchar(500)='',
@GlobalRoleId nvarchar(128)='',
@UserList nvarchar(max) = '',
@IsManager  bit='',
@Filter nvarchar(250) = '',
@Studio nvarchar(250) = '',
@Division bigint = 0,
@Sector nvarchar(250) = ''
as
begin
begin try
	IF OBJECT_ID('TempDB..#tmpStatus') IS NOT NULL Drop table #tmpStatus
	create table #tmpStatus
	(
		Description varchar(1000),
		TypeCompleted varchar(20)
	)
	declare @yearCheck bit, @fromYear int, @toYear int, @startYearCheck int = 0, @startType int = 1;
	set @fromYear = (select year(@Fromdate))
	set @toYear = (select YEAR(@Todate))
	if((@toYear - @fromYear) > 0)
	begin
		set @yearCheck = @toYear - @fromYear
	end
	else
	begin
		set @yearCheck = 0
	end

	if @yearCheck = 0
	begin
		while(@startType <= 4)
		begin

			if(@startType = 1)
			begin
				set @type = 'FTE'
			end
			if(@startType = 2)
			begin
				set @type = 'PERCENT'
			end
			if(@startType = 3)
			begin
				set @type = 'COUNT'
			end
			if(@startType = 4)
			begin
				set @type = 'AVAILABILITY'
			end
			exec [dbo].[usp_insert_update_ResourceUtilizationUser] @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			insert into #tmpStatus values('Closed Projects - Include All Resources', @type)
			exec [dbo].[usp_insert_update_ResourceUtilizationUser] @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			insert into #tmpStatus values('Closed Projects - Not Include All Resources', @type)
			exec [dbo].[usp_insert_update_ResourceUtilizationUser] @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			insert into #tmpStatus values('Exclude Closed Projects - Include All Resources', @type)
			exec [dbo].[usp_insert_update_ResourceUtilizationUser] @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			insert into #tmpStatus values('Exclude Closed Projects - Not Include All Resources', @type)

			set @startType = @startType + 1
		end
		select * from #tmpStatus
	end
	else
	begin
		while(@startYearCheck <= @yearCheck)
		begin
			if(@startYearCheck = 0)
			begin
				set @Fromdate = convert(varchar,@fromYear) + '-01-01 00:00:00.000'
				set @Todate = DATEADD(MONTH,0, convert(varchar,@fromYear) + '-12-31 00:00:00.000')

				select @Fromdate FromDate, @Todate ToDate
				
				while(@startType <= 4)
				begin

					if(@startType = 1)
					begin
						set @type = 'FTE'
					end
					if(@startType = 2)
					begin
						set @type = 'PERCENT'
					end
					if(@startType = 3)
					begin
						set @type = 'COUNT'
					end
					if(@startType = 4)
					begin
						set @type = 'AVAILABILITY'
					end

					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
														   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
																   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
																   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					set @startType = @startType + 1
				end
			end
			else
			begin
				set @Fromdate = convert(varchar,(@fromYear + 1)) + '-01-01 00:00:00.000'
				set @Todate = DATEADD(MONTH,0, convert(varchar,(@fromYear + 1)) + '-12-31 00:00:00.000')

				select @Fromdate as ElseFromDate, @Todate as ElseToDate
				set @startType = 1;
				while(@startType <= 4)
				begin
					
					if(@startType = 1)
					begin
						set @type = 'FTE'
					end
					if(@startType = 2)
					begin
						set @type = 'PERCENT'
					end
					if(@startType = 3)
					begin
						set @type = 'COUNT'
					end
					if(@startType = 4)
					begin
						set @type = 'AVAILABILITY'
					end

					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
														   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
																   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
																   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					exec usp_insert_update_ResourceUtilizationUser @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
					set @startType = @startType + 1
				end
			end
			set @startYearCheck = @startYearCheck + 1;
		end
	end
end try
begin catch
	select ERROR_MESSAGE()
end catch
end
