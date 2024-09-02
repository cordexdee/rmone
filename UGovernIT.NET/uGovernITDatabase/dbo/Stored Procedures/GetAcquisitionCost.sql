CREATE PROCEDURE [dbo].[GetAcquisitionCost]
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B', 
@TicketID varchar(128),
@Module varchar(10)
AS
BEGIN
	Declare @Query nvarchar(max);
	Declare @Table nvarchar(500);

	Declare @PreconStartDate Datetime;
	Declare @PreconEndDate Datetime;
	Declare @EstimatedConstructionStart Datetime;

	Declare @StartDate Datetime;
	Declare @EndDate Datetime;
	Declare @AllocStartDt Datetime;
	Declare @AllocEndDate Datetime;
	
	Declare @Resource nvarchar(128);
	Declare @JobTitleLookup bigint;
	Declare @Cost float;
	Declare @AcquisitionCost float;
	Declare @Hours int;


	Select @Table = ModuleTable from Config_Modules where TenantID = @TenantID and ModuleName = @Module;
	set @Query = 'Select @PreconStartDate = PreconStartDate, @PreconEndDate = PreconEndDate, @EstimatedConstructionStart = EstimatedConstructionStart  from ' + @Table + ' where TenantID = ''' + @TenantID + '''' + ' and TicketId = ''' + @TicketID + '''';

	--select @Query;
	EXECUTE sp_executesql @Query, N'@PreconStartDate Datetime OUTPUT, @PreconEndDate Datetime OUTPUT, @EstimatedConstructionStart Datetime  OUTPUT', @PreconStartDate = @PreconStartDate OUTPUT, @PreconEndDate = @PreconEndDate OUTPUT, @EstimatedConstructionStart = @EstimatedConstructionStart  OUTPUT;
	
	--select @PreconStartDate, @PreconEndDate, @EstimatedConstructionStart;

	--Set @AcquisitionCost = 0;

	If @PreconStartDate = NULL
	Begin
		Return @AcquisitionCost;
	End
	Else If @PreconEndDate = NULL and @EstimatedConstructionStart = NULL
	Begin
		Return @AcquisitionCost;
	End
	Else If @PreconStartDate IS NOT NULL and @PreconEndDate IS NOT NULL
	Begin
		Set @StartDate = @PreconStartDate;
		Set @EndDate = @PreconEndDate;
	End
	Else If @PreconStartDate IS NOT NULL and @EstimatedConstructionStart IS NOT NULL
	Begin
		Set @StartDate = @PreconStartDate;
		Set @EndDate = @EstimatedConstructionStart;
	End

	If @StartDate = NULL OR @EndDate = NULL
	Begin
		Return @AcquisitionCost;
	End

	Set @AcquisitionCost = 0;
	--Select @StartDate, @EndDate;

	create table #tmpAllocations(
		Id int identity,
		AllocStartDt Datetime,
		AllocEndDt Datetime,
		ResourceUser nvarchar(128)
	)
	Declare @i int, @Count int;

	Insert into #tmpAllocations 
	(
		AllocStartDt,
		AllocEndDt,
		ResourceUser
	)
	Select AllocationStartDate, AllocationEndDate, AssignedToUser 
	from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID

	--select * from #tmpAllocations

	Set @i = 1;
	Select @Count = count(*) from #tmpAllocations;

	While @i <= @Count
	Begin
		Set @Cost = 0;
		Select @AllocStartDt = AllocStartDt, @AllocEndDate = AllocEndDt, @Resource = ResourceUser from #tmpAllocations where Id = @i;
		
		IF @StartDate <= @AllocStartDt and (@EndDate between @AllocStartDt and @AllocEndDate)
		Begin
			select @Hours = DATEDIFF(day, @AllocStartDt, @EndDate) * 8; 
		End
		Else IF @StartDate >= @AllocStartDt and (@StartDate between @AllocStartDt and @AllocEndDate) and @EndDate >= @AllocEndDate
		Begin
			select @Hours = DATEDIFF(day, @StartDate, @AllocEndDate) * 8; 
		End
		Else IF @StartDate <= @AllocStartDt and @EndDate >= @AllocEndDate
		Begin
			select @Hours = DATEDIFF(day, @AllocStartDt, @AllocEndDate) * 8; 
		End
		Else IF @StartDate >= @AllocStartDt and @EndDate <= @AllocEndDate
		Begin
			select @Hours = DATEDIFF(day, @StartDate, @EndDate) * 8; 
		End

		select @Cost = ISNULL(CONVERT(float, TRIM(j.EmpCostRate)), 0) from AspNetUsers a join JobTitleCostRateByDept j on a.JobTitleLookup = j.JobTitleLookup
		where a.Id = @Resource;

		--select @Resource, @Hours, @Cost;

		Set @AcquisitionCost = @AcquisitionCost + (@Cost * ISNULL(@Hours, 0));

		Set @i = @i + 1;
	End

	Select @AcquisitionCost as AcquisitionCost;	

	drop table #tmpAllocations;
END
