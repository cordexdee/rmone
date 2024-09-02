CREATE procedure [dbo].[usp_fillResourceUtilization]
@TenantId varchar(max) = '',
@url varchar(max) = ''
as
begin
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

	select distinct SRPC.UserId,
			  STUFF((SELECT distinct ', ' + SRPC1.ModuleNameLookup
			         from Summary_ResourceProjectComplexity SRPC1
			         where SRPC.UserId = SRPC1.UserId and TenantID = @TenantId
			            FOR XML PATH(''), TYPE
			            ).value('.', 'NVARCHAR(MAX)') 
			        ,1,2,'') ModuleNameLookup
			into #tmpSummary_ResourceProjectComplexity
			from Summary_ResourceProjectComplexity SRPC;

	Declare @EnableDivision bit;
	select @EnableDivision = KeyValue from Config_ConfigurationVariable where TenantID = @TenantId and KeyName = 'EnableDivision';

	IF @EnableDivision = 1
	begin
		IF OBJECT_ID('tempdb..#tmpDivisions') IS NOT NULL DROP TABLE #tmpDivisions;

		select CD.ID DivId, CD.Title Division, dept.ID DeptId, dept.Title Department into #tmpDivisions 
		from CompanyDivisions CD inner join Department dept on CD.ID = dept.DivisionIdLookup where CD.TenantID = @TenantID
	end

	declare @FromDate datetime, @ToDate datetime, @startType int = 1, @start int = 1, @currentDate datetime = getdate(), 
			@startYear int, @totalYears int = 6, @type varchar(15),@mode varchar(10) = 'Monthly', @IsAssignedallocation bit='False',
			@ResourceManager varchar(250)='0', @AllocationType varchar(50)='Estimated', @IsManager  bit='True', @Department varchar(2000)='',
			@LevelName varchar(50)='', @JobProfile varchar(500)='', @GlobalRoleId nvarchar(128)='', @UserList nvarchar(max), @Filter nvarchar(250) = '',
			@Studio nvarchar(250) = '', @Division bigint = 0, @Sector nvarchar(250) = '';

	set @startYear = YEAR(@currentDate)
	while(@start <= @totalYears)
	begin
		set @startType = 1
		if(@start = 1)
		begin
			set @FromDate = convert(varchar,@startYear) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear) + '-12-31 00:00:00.000')

			--select @FromDate FromDate_1, @ToDate ToDate_1
		end
		if(@start = 2)
		begin
			set @FromDate = convert(varchar,@startYear + 1) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear + 1) + '-12-31 00:00:00.000')
			--select @FromDate FromDate_2, @ToDate ToDate_2
		end
		if(@start = 3)
		begin
			set @FromDate = convert(varchar,@startYear + 2) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear + 2) + '-12-31 00:00:00.000')
			--select @FromDate FromDate_3, @ToDate ToDate_3
		end
		if(@start = 4)
		begin
			set @FromDate = convert(varchar,@startYear - 1) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear - 1) + '-12-31 00:00:00.000')
			--select @FromDate FromDate_4, @ToDate ToDate_4
		end
		if(@start = 5)
		begin
			set @FromDate = convert(varchar,@startYear - 2) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear - 2) + '-12-31 00:00:00.000')
			--select @FromDate FromDate_5, @ToDate ToDate_5
		end
		if(@start = 6)
		begin
			set @FromDate = convert(varchar,@startYear -3) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear -3) + '-12-31 00:00:00.000')
			--select @FromDate FromDate_6, @ToDate ToDate_6
		end
		
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

			truncate table #tmpResourceUtilizationSummary

			insert into #tmpResourceUtilizationSummary(ItemOrder,FullAllocation,Id,IncludeAllResources,ResourceUser,ProjectCapacity,RevenueCapacity,[#LifeTime],[$LifeTime],ResourceUserAllocated,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec)
			exec usp_GetResourceUtilzation @TenantId,@FromDate,@ToDate,@mode,'1',@type,@Department,'True',@IsAssignedallocation,@ResourceManager,@url,@AllocationType,
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'

			update #tmpResourceUtilizationSummary 
			set FlagType = @type, [Year] = YEAR(@Fromdate), TenantID = @TenantId, ClosedProjects = 'True', [Filter] = @Filter ,Studio = @Studio, Division =@Division, Sector = @Sector

			IF EXISTS(select 1 from ResourceUtilizationSummary where FlagType=@type 
						and TenantID=@TenantID 
						and Year=YEAR(@Fromdate) 
						and IncludeAllResources='True' 
						and ClosedProjects='True')
			begin
				Delete from ResourceUtilizationSummary where FlagType=@type and TenantID=@TenantID and Year=YEAR(@Fromdate) and IncludeAllResources='True' and ClosedProjects='True'
			end
			
			insert into ResourceUtilizationSummary
			select * from #tmpResourceUtilizationSummary

			update RUS
			set RUS.DepartmentLookup = ANU.DepartmentLookup,
				RUS.[Role] = ANU.GlobalRoleID,
				RUS.ManagerUser = ANU.ManagerUser,
				RUS.LevelName = TSRPC.ModuleNameLookup
			from ResourceUtilizationSummary RUS 
				 inner join AspNetUsers ANU on RUS.Id = ANU.Id 
				 left join #tmpSummary_ResourceProjectComplexity TSRPC on RUS.Id = TSRPC.UserId
			where RUS.Year = YEAR(@Fromdate) and RUS.FlagType=@type and IncludeAllResources='True' and ClosedProjects='True'

			if @EnableDivision = 1
			begin
				update RUS
				set RUS.Division = tDiv.DivId
				from ResourceUtilizationSummary RUS inner join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId
			end
			
			truncate table #tmpResourceUtilizationSummary

			insert into #tmpResourceUtilizationSummary(ItemOrder,FullAllocation,Id,IncludeAllResources,ResourceUser,ProjectCapacity,RevenueCapacity,[#LifeTime],[$LifeTime],ResourceUserAllocated,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec)
			exec usp_GetResourceUtilzation @TenantID, @Fromdate, @Todate, @mode, '1', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			
			update #tmpResourceUtilizationSummary 
			set FlagType = @type, [Year] = YEAR(@Fromdate), TenantID = @TenantId, ClosedProjects = 'True', [Filter] = @Filter ,Studio = @Studio, Division =@Division, Sector = @Sector

			IF EXISTS(select 1 from ResourceUtilizationSummary where FlagType=@type 
						and TenantID=@TenantID 
						and Year=YEAR(@Fromdate) 
						and IncludeAllResources='False' 
						and ClosedProjects='True')
			begin
				Delete from ResourceUtilizationSummary where FlagType=@type and TenantID=@TenantID and Year=YEAR(@Fromdate) and IncludeAllResources='False' and ClosedProjects='True'
			end
			
			insert into ResourceUtilizationSummary
			select * from #tmpResourceUtilizationSummary

			update RUS
			set RUS.DepartmentLookup = ANU.DepartmentLookup,
				RUS.[Role] = ANU.GlobalRoleID,
				RUS.ManagerUser = ANU.ManagerUser,
				RUS.LevelName = TSRPC.ModuleNameLookup
			from ResourceUtilizationSummary RUS inner join AspNetUsers ANU on RUS.Id = ANU.Id left join #tmpSummary_ResourceProjectComplexity TSRPC on RUS.Id = TSRPC.UserId
			where RUS.Year = YEAR(@Fromdate) and RUS.FlagType=@type and IncludeAllResources='False' and ClosedProjects='True'

			if @EnableDivision = 1
			begin
				update RUS
				set RUS.Division = tDiv.DivId
				from ResourceUtilizationSummary RUS inner join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId
			end

			truncate table #tmpResourceUtilizationSummary

			insert into #tmpResourceUtilizationSummary(ItemOrder,FullAllocation,Id,IncludeAllResources,ResourceUser,ProjectCapacity,RevenueCapacity,[#LifeTime],[$LifeTime],ResourceUserAllocated,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec)
			exec [dbo].[usp_GetResourceUtilzation] @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'True', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			
			update #tmpResourceUtilizationSummary 
			set FlagType = @type, [Year] = YEAR(@Fromdate), TenantID = @TenantId, ClosedProjects = 'False', [Filter] = @Filter ,Studio = @Studio, Division =@Division, Sector = @Sector

			IF EXISTS(select 1 from ResourceUtilizationSummary where FlagType=@type 
						and TenantID=@TenantID 
						and Year=YEAR(@Fromdate) 
						and IncludeAllResources='True' 
						and ClosedProjects='False')
			begin
				Delete from ResourceUtilizationSummary where FlagType=@type and TenantID=@TenantID and Year=YEAR(@Fromdate) and IncludeAllResources='True' and ClosedProjects='False'
			end
			
			insert into ResourceUtilizationSummary
			select * from #tmpResourceUtilizationSummary

			update RUS
			set RUS.DepartmentLookup = ANU.DepartmentLookup,
				RUS.[Role] = ANU.GlobalRoleID,
				RUS.ManagerUser = ANU.ManagerUser,
				RUS.LevelName = TSRPC.ModuleNameLookup
			from ResourceUtilizationSummary RUS inner join AspNetUsers ANU on RUS.Id = ANU.Id left join #tmpSummary_ResourceProjectComplexity TSRPC on RUS.Id = TSRPC.UserId
			where RUS.Year = YEAR(@Fromdate) and RUS.FlagType=@type and IncludeAllResources='True' and ClosedProjects='False'

			if @EnableDivision = 1
			begin
				update RUS
				set RUS.Division = tDiv.DivId
				from ResourceUtilizationSummary RUS inner join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId
			end

			truncate table #tmpResourceUtilizationSummary

			insert into #tmpResourceUtilizationSummary(ItemOrder,FullAllocation,Id,IncludeAllResources,ResourceUser,ProjectCapacity,RevenueCapacity,[#LifeTime],[$LifeTime],ResourceUserAllocated,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec)
			exec [dbo].[usp_GetResourceUtilzation] @TenantID, @Fromdate, @Todate, @mode, '0', @type, @Department, 'False', @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, 'True'
			
			update #tmpResourceUtilizationSummary 
			set FlagType = @type, [Year] = YEAR(@Fromdate), TenantID = @TenantId, ClosedProjects = 'False', [Filter] = @Filter ,Studio = @Studio, Division =@Division, Sector = @Sector

			IF EXISTS(select 1 from ResourceUtilizationSummary where FlagType=@type 
						and TenantID=@TenantID 
						and Year=YEAR(@Fromdate) 
						and IncludeAllResources='False' 
						and ClosedProjects='False')
			begin
				Delete from ResourceUtilizationSummary where FlagType=@type and TenantID=@TenantID and Year=YEAR(@Fromdate) and IncludeAllResources='False' and ClosedProjects='False'
			end
			
			insert into ResourceUtilizationSummary
			select * from #tmpResourceUtilizationSummary

			update RUS
			set RUS.DepartmentLookup = ANU.DepartmentLookup,
				RUS.[Role] = ANU.GlobalRoleID,
				RUS.ManagerUser = ANU.ManagerUser,
				RUS.LevelName = TSRPC.ModuleNameLookup
			from ResourceUtilizationSummary RUS inner join AspNetUsers ANU on RUS.Id = ANU.Id left join #tmpSummary_ResourceProjectComplexity TSRPC on RUS.Id = TSRPC.UserId
			where RUS.Year = YEAR(@Fromdate) and RUS.FlagType=@type and IncludeAllResources='False' and ClosedProjects='False'

			if @EnableDivision = 1
			begin
				update RUS
				set RUS.Division = tDiv.DivId
				from ResourceUtilizationSummary RUS inner join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId
			end
			
			set @startType = @startType + 1
		end
		
		set @start = @start + 1

	end
end