-- select dbo.fnGetUtilizationRate('35525396-e5fe-4692-9239-4df9305b915b', 'CPR-19-000104','2021-07-01','2021-07-29')
-- select dbo.fnGetUtilizationRate('35525396-e5fe-4692-9239-4df9305b915b','CPR-19-000104','2021-01-01',NULL)
CREATE FUNCTION [dbo].[fnGetUtilizationRate] 
(
	@TenantID nvarchar(256),
	@TicketId nvarchar(100),
	@PreconStartDate datetime,
	@ConstructionEndDate datetime
)
RETURNS decimal(18,2)
AS
BEGIN
	DECLARE @Result decimal(18,2);

	DECLARE @i int;
	DECLARE @count int;
	DECLARE @workingDaysinWeek int;
	DECLARE @TotalworkingDays int;
	DECLARE @AvgPctAllocation decimal(18,2);
	DECLARE @PctAllocation decimal(18,2);

	DECLARE @StartDate datetime;
	DECLARE @EndDate datetime;
	DECLARE @firstWkStartDt datetime;

	Set @PctAllocation = 0;
	Set @TotalworkingDays = 0;
	Set @Result = 0;

	set @StartDate = @PreconStartDate;
	set @EndDate = @ConstructionEndDate;

	If @PreconStartDate IS NULL OR @ConstructionEndDate IS NULL	
	RETURN @Result;
	
	DECLARE @Dates TABLE
	(
		Id int identity,
		WeekStartDate datetime
	)
	
	set @StartDate =  DATEADD(day, DATEDIFF(day, 0, @PreconStartDate) /7*7, 0);
	--set @StartDate = DATEADD(DAY, 1 - 4, @PreconStartDate); --start from first day of week of @PreconStartDate
	WHILE (@StartDate <= @EndDate)
	BEGIN
		--print @StartDate;
		Insert into @Dates(WeekStartDate) values (@StartDate)
		set @StartDate = DATEADD(day, 7, @StartDate);
	END;
			
	--select * from @Dates
	set @firstWkStartDt =  DATEADD(day, DATEDIFF(day, 0, @PreconStartDate) /7*7, 0);
	set @workingDaysinWeek = dbo.fnGetWorkingDays(@PreconStartDate, DATEADD(DAY, 4, @firstWkStartDt));

	select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
	and WorkItem = @TicketId
	and WeekStartDate = @firstWkStartDt
	set @PctAllocation = (@AvgPctAllocation * @workingDaysinWeek);
	set @TotalworkingDays = @workingDaysinWeek;

	--select @PctAllocation as PctAllocation, @workingDaysinWeek as workingDays;
	--From Second Week calculation
	set @i = 2;
	select @count = count(*) from @Dates;
	While @i < @count
	Begin
		select @StartDate = WeekStartDate from @Dates where Id = @i;

		select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
		and WorkItem = @TicketId
		and WeekStartDate = @StartDate
		set @workingDaysinWeek = dbo.fnGetWorkingDays(@StartDate, DATEADD(DAY, 4, @StartDate));

		set @PctAllocation = @PctAllocation + (@AvgPctAllocation * @workingDaysinWeek);

		If @AvgPctAllocation > 0
		set @TotalworkingDays =  @TotalworkingDays + @workingDaysinWeek;
		--select (@AvgPctAllocation * @workingDaysinWeek) as PctAllocation, @workingDaysinWeek as workingDays;
		set @i = @i + 1;
	End;

	-- Last Week calculations
	select @StartDate = WeekStartDate from @Dates where Id = @count;
	select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
	and WorkItem = @TicketId
	and WeekStartDate = @StartDate;
	set @workingDaysinWeek = dbo.fnGetWorkingDays(@StartDate, @ConstructionEndDate);
	set @PctAllocation = @PctAllocation + (@AvgPctAllocation * @workingDaysinWeek);
	set @TotalworkingDays = @TotalworkingDays + @workingDaysinWeek;


	--RETURN @Result
	--select @PctAllocation as 'FinalPctAlloc', @TotalworkingDays as 'totalDays'
	--select (@AvgPctAllocation) as PctAllocation, @workingDaysinWeek as TotalworkingDays;

	select @Result = (@PctAllocation / @TotalworkingDays);
	RETURN @Result;
END