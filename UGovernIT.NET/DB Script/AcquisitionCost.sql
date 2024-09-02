
-- select  dbo.fnGetAcquisitionCost('35525396-E5FE-4692-9239-4DF9305B915B','CPR-23-000666','CPR','2023-05-15','2023-09-01')
ALTER FUNCTION [dbo].[fnGetAcquisitionCost]
(
@TenantID varchar(128), 
@TicketID varchar(128),
@Module varchar(10),
@StartDate Datetime,	--Precon StartDate
@EndDate Datetime		--Precon EndDate
)
RETURNS float
AS
BEGIN
Declare @Query nvarchar(max);
	Declare @Table nvarchar(500);

	Declare @PreconStartDate Datetime;
	Declare @PreconEndDate Datetime;
	Declare @EstimatedConstructionStart Datetime;


	Declare @AllocStartDt Datetime;
	Declare @AllocEndDate Datetime;
	
	Declare @Resource nvarchar(128);
	Declare @JobTitleLookup bigint;
	Declare @Cost float;
	Declare @AcquisitionCost float;
	Declare @Hours float;
	Declare @PctAllocation float;
	Declare @HoursPerDay int;    

	Set @AcquisitionCost = 0;

	-- Get No of hours per day from config variable
	select @HoursPerDay = KeyValue from Config_ConfigurationVariable where TenantID = '35525396-E5FE-4692-9239-4DF9305B915B' and KeyName = 'ResourceWorkingHours';

	-- Declare & user Table variable, to store Allocations for a Project/Opportunity
	Declare @tmpAllocations Table(
		Id int identity,
		AllocStartDt Datetime,
		AllocEndDt Datetime,
		ResourceUser nvarchar(128),
		PctAllocation float
	)
	Declare @i int, @Count int;

	Insert into @tmpAllocations 
	(
		AllocStartDt,
		AllocEndDt,
		ResourceUser,
		PctAllocation
	)
	Select AllocationStartDate, AllocationEndDate, AssignedToUser, PctAllocation
	from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID

	Set @i = 1;
	Select @Count = count(*) from @tmpAllocations;
	-- Iterating Resultset in @tmpAllocations
	While @i <= @Count
	Begin
		Set @Cost = 0;
		Select @AllocStartDt = AllocStartDt, @AllocEndDate = AllocEndDt, @Resource = ResourceUser, @PctAllocation = PctAllocation from @tmpAllocations where Id = @i;
		-- Checking Date overlaps between Precon and Allocation dates
		IF @StartDate <= @AllocStartDt and (@EndDate between @AllocStartDt and @AllocEndDate)
		Begin
			-- Calculating No of Hours between Date range, excluding Weekends and Holidays
			 select @Hours = dbo.fnGetWorkingDays(@AllocStartDt, @EndDate, @TenantID) * (@HoursPerDay * (@PctAllocation/100));
		End
		Else IF @StartDate >= @AllocStartDt and (@StartDate between @AllocStartDt and @AllocEndDate) and @EndDate >= @AllocEndDate
		Begin
			select @Hours = dbo.fnGetWorkingDays(@StartDate,  @AllocEndDate, @TenantID) * (@HoursPerDay * (@PctAllocation/100));
		End
		Else IF @StartDate <= @AllocStartDt and @EndDate >= @AllocEndDate
		Begin
			select @Hours = dbo.fnGetWorkingDays(@AllocStartDt,  @AllocEndDate, @TenantID) * (@HoursPerDay * (@PctAllocation/100));
		End
		Else IF @StartDate >= @AllocStartDt and @EndDate <= @AllocEndDate
		Begin
			select @Hours = dbo.fnGetWorkingDays(@StartDate, @EndDate, @TenantID) * (@HoursPerDay * (@PctAllocation/100));
		End
		Else
		Begin
			Set @Hours = 0;
		End
		-- Fetching Resource's Cost Rate from JobTitleCostRateByDept table
		select @Cost = ISNULL(CONVERT(float, TRIM(j.EmpCostRate)), 0) from AspNetUsers a join JobTitleCostRateByDept j on a.JobTitleLookup = j.JobTitleLookup and a.DepartmentLookup = j.DeptLookup
		where a.Id = @Resource;

		Set @AcquisitionCost = @AcquisitionCost + (@Cost * ISNULL(@Hours, 0));

		Set @i = @i + 1;
	End

	-- Returning AcquisitionCost
	return @AcquisitionCost;	

END

GO
-------------

-- select  dbo.fnGetActualAcquisitionCost('35525396-E5FE-4692-9239-4DF9305B915B','CPR-21-000460','CPR','2022-12-27','2023-12-27')
CREATE FUNCTION [dbo].[fnGetActualAcquisitionCost]
(
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B', 
@TicketID varchar(128),
@Module varchar(10),
@StartDate Datetime,
@EndDate Datetime
)
RETURNS float
AS
BEGIN

	Declare @PreconStartDate Datetime;
	Declare @PreconEndDate Datetime;
	Declare @EstimatedConstructionStart Datetime;

	Declare @AllocStartDt Datetime;
	Declare @AllocEndDate Datetime;
	
	Declare @Resource nvarchar(128);
	Declare @JobTitleLookup bigint;
	Declare @Cost float;
	Declare @AcquisitionCost float;
	Declare @Hours float;
	   
	Set @AcquisitionCost = 0;
	--Select @StartDate, @EndDate;

	Declare @tmpAllocations Table(
		Id int identity,
		AllocStartDt Datetime,
		AllocEndDt Datetime,
		ResourceUser nvarchar(128)
	)
	Declare @i int, @Count int;

	Insert into @tmpAllocations 
	(
		AllocStartDt,
		AllocEndDt,
		ResourceUser
	)
	Select Min(WorkDate), Max(WorkDate), ResourceUser 
	from TicketHours where TenantID = @TenantID and TicketID = @TicketID
	group by ResourceUser

	--select * from #tmpAllocations

	Set @i = 1;
	Select @Count = count(*) from @tmpAllocations;

	While @i <= @Count
	Begin
		Set @Cost = 0;
		Select @AllocStartDt = AllocStartDt, @AllocEndDate = AllocEndDt, @Resource = ResourceUser from @tmpAllocations where Id = @i;
		--Select @Resource = ResourceUser from @tmpAllocations where Id = @i;
		--select @AllocStartDt = min(WorkDate), @AllocEndDate = MAX(WorkDate) from TicketHours where ResourceUser = @Resource and TenantID = @TenantID and TicketId = @TicketID;

		IF @StartDate <= @AllocStartDt and (@EndDate between @AllocStartDt and @AllocEndDate)
		Begin
			select @Hours = sum(HoursTaken) from TicketHours where ResourceUser = @Resource and TenantID = @TenantID and TicketId = @TicketID
				and WorkDate between @AllocStartDt and @EndDate;
				--select @Hours = DATEDIFF(day, @AllocStartDt, @EndDate); 

		End
		Else IF @StartDate >= @AllocStartDt and (@StartDate between @AllocStartDt and @AllocEndDate) and @EndDate >= @AllocEndDate
		Begin
			select @Hours = sum(HoursTaken) from TicketHours where ResourceUser = @Resource and TenantID = @TenantID and TicketId = @TicketID
				and WorkDate between @StartDate and @AllocEndDate;
			--select @Hours = DATEDIFF(day, @StartDate, @AllocEndDate); 
		End
		Else IF @StartDate <= @AllocStartDt and @EndDate >= @AllocEndDate
		Begin
			select @Hours = sum(HoursTaken) from TicketHours where ResourceUser = @Resource and TenantID = @TenantID and TicketId = @TicketID
				and WorkDate between @AllocStartDt and @AllocEndDate;
			--select @Hours = DATEDIFF(day, @AllocStartDt, @AllocEndDate); 
		End
		Else IF @StartDate >= @AllocStartDt and @EndDate <= @AllocEndDate
		Begin
			select @Hours = sum(HoursTaken) from TicketHours where ResourceUser = @Resource and TenantID = @TenantID and TicketId = @TicketID
				and WorkDate between @StartDate and @EndDate;
			--select @Hours = DATEDIFF(day, @StartDate, @EndDate); 
		End

		select @Cost = ISNULL(CONVERT(float, TRIM(j.EmpCostRate)), 0) from AspNetUsers a join JobTitleCostRateByDept j on a.JobTitleLookup = j.JobTitleLookup and a.DepartmentLookup = j.DeptLookup
		where a.Id = @Resource;

		--select @Resource, @Hours, @Cost;

		Set @AcquisitionCost = @AcquisitionCost + (@Cost * ISNULL(@Hours, 0));

		Set @i = @i + 1;
	End

	return @AcquisitionCost;	

END

GO
------------
-- select  dbo.fnGetActualProjectCost('35525396-E5FE-4692-9239-4DF9305B915B','CPR-23-000666','2023-05-15','2023-09-01')

CREATE FUNCTION [dbo].[fnGetActualProjectCost]
(
@TenantID varchar(128), 
@TicketID varchar(128),
@StartDate Datetime,
@EndDate Datetime
)
RETURNS float
AS
BEGIN
	Declare @Hours int;
	Declare @ActualProjectCost float;

	Declare @EnablePrjStdWorkItem bit;

	Set @ActualProjectCost = 0;

	Set @EnablePrjStdWorkItem = 0;
	select @EnablePrjStdWorkItem = KeyValue from Config_ConfigurationVariable where TenantID = @TenantId and KeyName = 'EnableProjStdWorkItems';
	
	If @EnablePrjStdWorkItem = 0 
		Begin
			Select @Hours = sum(r.HoursTaken) from ResourceTimeSheet r join ResourceWorkItems wi on r.ResourceWorkItemLookup = wi.ID
			where r.TenantID = @TenantID and wi.WorkItem = @TicketID and r.WorkDate between @StartDate and @EndDate;			
		End
	Else If @EnablePrjStdWorkItem = 1
		Begin
			Select @Hours = sum(HoursTaken) from TicketHours  where TenantID = @TenantID and WorkItem = @TicketID and WorkDate between @StartDate and @EndDate;
		End

		select @ActualProjectCost = Sum(ISNULL(CONVERT(float, TRIM(j.EmpCostRate)), 0)) * @Hours from AspNetUsers a join JobTitleCostRateByDept j on a.JobTitleLookup = j.JobTitleLookup
		join ProjectEstimatedAllocation pe on pe.AssignedToUser = a.Id and a.DepartmentLookup = j.DeptLookup
		and a.TenantID = @TenantID and pe.TicketId = @TicketID

	Return @ActualProjectCost;
END;




GO
------

-- select  dbo.fnGetForecastedProjectCost('35525396-E5FE-4692-9239-4DF9305B915B','CPR-23-000666','CPR','2023-05-15','2023-09-01')
CREATE FUNCTION [dbo].[fnGetForecastedProjectCost]
(
@TenantID varchar(128), 
@TicketID varchar(128),
@Module varchar(10),
@StartDate Datetime,	--Precon StartDate
@EndDate Datetime		--Precon EndDate
)
RETURNS float
AS
BEGIN
	Declare @AllocStartDt Datetime;
	Declare @AllocEndDate Datetime;
	
	Declare @Resource nvarchar(128);
	Declare @JobTitleLookup bigint;
	Declare @Cost float;
	Declare @ForecastedProjectCost float;
	Declare @PctAllocation float;
	Declare @Hours float;
	Declare @HoursPerDay int; 

	Set @ForecastedProjectCost = 0;

	Declare @tmpAllocations Table(
		Id int identity,
		AllocStartDt Datetime,
		AllocEndDt Datetime,
		ResourceUser nvarchar(128),
		PctAllocation float
	)
	Declare @i int, @Count int;

	-- Get No of hours per day from config variable
	select @HoursPerDay = KeyValue from Config_ConfigurationVariable where TenantID = '35525396-E5FE-4692-9239-4DF9305B915B' and KeyName = 'ResourceWorkingHours';

	-- Declare & user Table variable, to store Allocations for a Project/Opportunity
	Insert into @tmpAllocations 
	(
		AllocStartDt,
		AllocEndDt,
		ResourceUser,
		PctAllocation
	)
	Select AllocationStartDate, AllocationEndDate, AssignedToUser, PctAllocation
	from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID

	Set @i = 1;
	Select @Count = count(*) from @tmpAllocations;

	While @i <= @Count
	Begin
		Set @Cost = 0;
		Select @AllocStartDt = AllocStartDt, @AllocEndDate = AllocEndDt, @Resource = ResourceUser, @PctAllocation = PctAllocation from @tmpAllocations where Id = @i;
		
		-- Checking Date overlaps between Precon and Allocation dates
		select @Hours = dbo.fnGetWorkingDays(@AllocStartDt, @AllocEndDate, @TenantID) * (@HoursPerDay * @PctAllocation);

		select @Cost = ISNULL(CONVERT(float, TRIM(j.EmpCostRate)), 0) from AspNetUsers a join JobTitleCostRateByDept j on a.JobTitleLookup = j.JobTitleLookup and a.DepartmentLookup = j.DeptLookup
		where a.Id = @Resource;

		Set @ForecastedProjectCost = @ForecastedProjectCost + (@Cost * ISNULL(@Hours, 0));

		Set @i = @i + 1;
	End

	return @ForecastedProjectCost;	

END



GO
----------------


-- GetForecastAndAcquisitionCosts 'CPR-23-000666','35525396-e5fe-4692-9239-4df9305b915b'
CREATE PROCEDURE [dbo].[GetForecastAndAcquisitionCosts]
	@TicketId nvarchar(100), 
	@TenantId nvarchar(128)
AS
BEGIN
	Declare @PreconStartDate Datetime;
	Declare @PreconEndDate Datetime;
	Declare @EstimatedConstructionStart Datetime;
	Declare @EstimatedConstructionEnd Datetime;
	Declare @CloseoutStartDate Datetime;

	Declare @StartDate Datetime;
	Declare @EndDate Datetime;
	Declare @AllocStartDt Datetime;
	Declare @AllocEndDate Datetime;
	
	Declare @Resource nvarchar(128);
	Declare @JobTitleLookup bigint;

	Declare @Cost float;
	Declare @ForecastedAcquisitionCost float;
	Declare @ActualAcquisitionCost float;
	Declare @ForecastedProjectCost float;
	Declare @ActualProjectCost float;
	Declare @Hours float;
	
	Declare @Module nvarchar(10);

	Declare @EnablePrjStdWorkItem bit;

	--print @TicketId;
	Select @Module = left(@TicketId, charindex('-', @TicketId) - 1);

	If @Module = 'CPR'
	Begin
		Select @PreconStartDate = PreconStartDate, @PreconEndDate = PreconEndDate, @EstimatedConstructionStart = EstimatedConstructionStart, @EstimatedConstructionEnd = EstimatedConstructionEnd, @CloseoutStartDate = CloseoutStartDate from CRMProject where TenantID = @TenantID and TicketId = @TicketID;

	End
	Else If @Module = 'OPM'
	Begin
		Select @PreconStartDate = PreconStartDate, @PreconEndDate = PreconEndDate, @EstimatedConstructionStart = EstimatedConstructionStart, @EstimatedConstructionEnd = EstimatedConstructionEnd, @CloseoutStartDate = CloseoutStartDate from Opportunity where TenantID = @TenantID and TicketId = @TicketID;		
	End

	--select @PreconStartDate as PreconStartDate, @PreconEndDate as PreconEndDate, @EstimatedConstructionStart as EstimatedConstructionStart, @EstimatedConstructionEnd as EstimatedConstructionEnd, @CloseoutStartDate as CloseoutStartDate;

	If @PreconStartDate IS NULL or @PreconEndDate IS NULL
	Begin	
	print 'start'
				If @PreconEndDate IS  NULL
			Begin
				Set @EndDate = @EstimatedConstructionStart - 1;
				Set @PreconEndDate = @EstimatedConstructionStart - 1;
			End

			If @EstimatedConstructionEnd IS  NULL
			Begin
				Set @EstimatedConstructionEnd = @CloseoutStartDate - 1;
			End

			-- SUPERINTENDENT RULE
			If @EstimatedConstructionEnd IS  NULL and @CloseoutStartDate IS NULL
			Begin
					Set @EnablePrjStdWorkItem = 0;
					select @EnablePrjStdWorkItem = KeyValue from Config_ConfigurationVariable where TenantID = @TenantId and KeyName = 'EnableProjStdWorkItems';
	
					If @EnablePrjStdWorkItem = 0 
						Begin
							Select @EstimatedConstructionStart = MIN(r.WorkDate), @EstimatedConstructionEnd = MAX(r.WorkDate) from ResourceTimeSheet r join ResourceWorkItems wi on r.ResourceWorkItemLookup = wi.ID
							where r.TenantID = @TenantID and wi.WorkItem = @TicketID and wi.SubWorkItem = 'Superintendent';			
						End
					Else If @EnablePrjStdWorkItem = 1
						Begin
							Select @EstimatedConstructionStart = MIN(WorkDate), @EstimatedConstructionEnd = MAX(WorkDate) from TicketHours  where TenantID = @TenantID and WorkItem = @TicketID and SubWorkItem = 'Superintendent';
						End
			End

	End

--	select @PreconStartDate as PreconStartDate, @PreconEndDate as PreconEndDate
	
	If @PreconStartDate IS NULL
		Begin
			Set @ForecastedAcquisitionCost = NULL;
			Set @ActualAcquisitionCost = NULL;
		End
	Else
		Begin
			select @ForecastedAcquisitionCost = dbo.fnGetAcquisitionCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);
			select @ActualAcquisitionCost = dbo.fnGetActualAcquisitionCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);
		end

	select @ForecastedProjectCost = dbo.fnGetForecastedProjectCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);
	select @ActualProjectCost = dbo.fnGetActualProjectCost(@TenantID, @TicketId, @PreconStartDate, @PreconEndDate);	

	select Round(@ForecastedAcquisitionCost, 2) as ForecastedAcquisitionCost, Round(@ActualAcquisitionCost, 2) as ActualAcquisitionCost, Round(@ForecastedProjectCost, 2) as ForecastedProjectCost, Round(@ActualProjectCost, 2) as ActualProjectCost, @TicketId as TicketId;
	
End