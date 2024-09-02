CREATE procedure [dbo].[usp_insert_update_ResourceUtilizationUser]
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
@IsManager  bit='True',
@Filter nvarchar(250) = '',
@Studio nvarchar(250) = '',
@Division bigint = 0,
@Sector nvarchar(250) = '',
@fill bit = ''
As
begin
	begin try

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

			Declare @allResources varchar(10), @IncludeClosed nvarchar(10);
			set @allResources = (select case when @IncludeAllResources = 'False' then 'False' else 'True' end)

			If(@Closed = 0)
			Begin
				Set @IncludeClosed = 'False';
			End
			Else
			Begin
				Set @IncludeClosed = 'True';
			End

			IF OBJECT_ID('tempdb..#tmpDivisions') IS NOT NULL DROP TABLE #tmpDivisions;

			select CD.ID DivId, CD.Title Division, dept.ID DeptId, dept.Title Department into #tmpDivisions 
			from CompanyDivisions CD inner join Department dept on CD.ID = dept.DivisionIdLookup where CD.TenantID = @TenantID

			select distinct SRPC.UserId,
			  STUFF((SELECT distinct ', ' + SRPC1.ModuleNameLookup
			         from Summary_ResourceProjectComplexity SRPC1
			         where SRPC.UserId = SRPC1.UserId and TenantID=@TenantID
			            FOR XML PATH(''), TYPE
			            ).value('.', 'NVARCHAR(MAX)') 
			        ,1,2,'') ModuleNameLookup
			into #tmpSummary_ResourceProjectComplexity
			from Summary_ResourceProjectComplexity SRPC;

			DECLARE  @UserCols AS NVARCHAR(MAX)
			SET @UserCols =  REPLACE(@UserList,  ',', ''',''')   
			
			insert into #tmpResourceUtilizationSummary(ItemOrder,FullAllocation,Id,IncludeAllResources,ResourceUser,ProjectCapacity,RevenueCapacity,[#LifeTime],[$LifeTime],ResourceUserAllocated,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec)
			exec usp_GetResourceUtilzation @TenantID, @Fromdate, @Todate, @mode, @Closed, @type, @Department, @IncludeAllResources, @IsAssignedallocation, @ResourceManager, @url, @AllocationType, 
													   @LevelName, @JobProfile, @GlobalRoleId, @UserList, @IsManager, @Filter, @Studio, @Division, @Sector, @fill


			update #tmpResourceUtilizationSummary 
			set FlagType = @type, [Year] = YEAR(@Fromdate), TenantID = @TenantId, ClosedProjects = @IncludeClosed, [Filter] = @Filter ,Studio = @Studio, Division =@Division, Sector = @Sector
		
			IF EXISTS(select 1 from ResourceUtilizationSummary where TenantID=@TenantID  and Year=YEAR(@Fromdate) and FlagType=@type and (ResourceUserAllocated in (@UserCols) or Id in (@UserCols))
																	 and IncludeAllResources=@allResources and ClosedProjects=@IncludeClosed)
			begin

				delete from ResourceUtilizationSummary where TenantID=@TenantID  
															 and Year=YEAR(@Fromdate) 
															 and FlagType=@type 
															 and (ResourceUserAllocated in (@UserCols) or Id in (@UserCols))
															 and IncludeAllResources=@allResources 
															 and ClosedProjects=@IncludeClosed
															 
			end

			insert into ResourceUtilizationSummary
			select * from #tmpResourceUtilizationSummary


			
			update RUS
			set RUS.DepartmentLookup = ANU.DepartmentLookup,
				RUS.[Role] = ANU.GlobalRoleID,
				RUS.ManagerUser = ANU.ManagerUser,
				RUS.LevelName = TSRPC.ModuleNameLookup,
				RUS.Division = tDiv.DivId
			from ResourceUtilizationSummary RUS inner join AspNetUsers ANU on RUS.Id = ANU.Id 
						left join #tmpSummary_ResourceProjectComplexity TSRPC on RUS.Id = TSRPC.UserId 
						left join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId
						where RUS.ResourceUserAllocated IN (@UserCols) and RUS.TenantID=@TenantID and RUS.Year=YEAR(@Fromdate) and RUS.FlagType=@type and RUS.IncludeAllResources=@allResources 
															 and RUS.ClosedProjects=@IncludeClosed


			update RUS
			set RUS.Division = tDiv.DivId
			from ResourceUtilizationSummary RUS inner join #tmpDivisions tDiv on RUS.DepartmentLookup = tDiv.DeptId where RUS.ResourceUserAllocated IN (@UserCols)
	end try
	begin catch
		select ERROR_MESSAGE()
	end catch
end
