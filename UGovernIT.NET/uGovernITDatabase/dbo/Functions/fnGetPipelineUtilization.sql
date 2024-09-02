-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','role','Architect')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','jobtitle','Assistant Project Manager')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','sector','Aviation')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','division','1-21 Silicon Valley')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','studio','SF Studio 2')
CREATE FUNCTION [dbo].[fnGetPipelineUtilization] 
(
	@TenantID nvarchar(256),
	@StartDate datetime,
	@EndDate datetime,
	@Category nvarchar(50),
	@value nvarchar(1000)
)
RETURNS int
AS
BEGIN
	DECLARE @Result decimal;
	DECLARE @MonthlyResult decimal;
	--Declare @Query nvarchar(500);
	Declare @i int;
	Declare @NoofDays int;
	Declare @NoofResources int;

	Declare @tmpDate date;
	Declare @StartMonth int;
	Declare @EndMonth int;

	Set @StartMonth = MONTH(@StartDate);
	Set @EndMonth = MONTH(@EndDate);

	Set @Result = 0;
	Set @NoofDays = 0;

	DECLARE @Tickets TABLE
	(
		TicketId varchar(30)
	)

	IF @Category = 'sector'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','',@value,'','')
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','',@value,'','')
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','',@value,'')
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','','',@value,'')
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','','','',@value,'')
		End
	ELSE
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','','','')
		End
			
	--select * from @Tickets

	DECLARE @tmpMonthDays TABLE (
		Month varchar(20),
		Days int
	);

	--INSERT INTO #tmpMonthDays Values (0,0,0,0,0,0,0,0,0,0,0,0);	
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@i as varchar(4)) + '-' + '01';

		INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, EOMONTH(@tmpDate)))
		Set @i = @i + 1;
	End

	--select * from @tmpMonthDays;

	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		PctAllocation float
		)

	IF @Category = 'sector' OR @Category = 'studio'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'jobtitle'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select usr.JobProfile, r.MonthStartDate, r.PctAllocation
			 from ResourceAllocationMonthly as r
			 join AspNetUsers as usr on r.ResourceUser = usr.Id
			 where r.TenantID = @TenantID and usr.TenantID = @TenantID
			 and r.MonthStartDate >= @StartDate and r.MonthStartDate <= @EndDate
			and usr.JobProfile = @value
			and r.ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1 and JobProfile = @value;
		End
	ELSE IF @Category = 'role'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select ResourceSubWorkItem, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and ResourceSubWorkItem = @value
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers as usr join Roles as r on usr.GlobalRoleID = r.Id where usr.TenantID = @TenantID
			and r.Name = @value and r.TenantID = @TenantID and usr.Enabled = 1;
		End


	--select * from @tmpRAMonthly order by MonthStartDate

	Set @Result = 0;
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		Set @MonthlyResult = 0;
		select @NoofDays = Days from @tmpMonthDays where Month = FORMAT(DATEADD(mm, @i,-1), 'MMM');
		select @MonthlyResult = SUM(PctAllocation) * @NoofDays  from @tmpRAMonthly  where MONTH(MonthStartDate) = @i  group by MonthStartDate

		--print @NoofDays
		--print @MonthlyResult
		--print '-----------'

		Set @Result = @Result + @MonthlyResult;
		Set @i = @i + 1;
	End
	
	select @NoofDays = Sum(Days) from @tmpMonthDays;
	--select @NoofDays as 'No of days';
	Set @Result = (@Result / @NoofDays)/@NoofResources;

	RETURN @Result
END