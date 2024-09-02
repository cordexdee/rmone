-- select dbo.fnGetRevenuesLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','role','Architect')
-- select dbo.fnGetRevenuesLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','jobtitle','Assistant Project Manager')
-- select dbo.fnGetRevenuesLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','sector','Aviation')
-- select dbo.fnGetRevenuesLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','division','1-21 Silicon Valley')
-- select dbo.fnGetRevenuesLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','studio','SF Studio 1')
CREATE FUNCTION [dbo].[fnGetRevenuesLost] 
(
	@TenantID nvarchar(256),
	@StartDate datetime,
	@EndDate datetime,
	@Category nvarchar(50),
	@value nvarchar(1000)
)
RETURNS decimal
AS
BEGIN
	DECLARE @Result decimal;
	Declare @tmpDate date;
	Declare @StartMonth int;
	Declare @EndMonth int;

	Set @Result = 0;

	DECLARE @Tickets TABLE
	(
		TicketId varchar(30)
	)

	IF @Category = 'sector'
		Begin
			Insert into @Tickets select TicketId from dbo.GetLostProjects(@TenantID,'','',@value,'','')
			--select TicketId,SectorChoice,Division from dbo.GetLostProjects(@TenantID,'','',@value,'','')
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @Tickets select TicketId from dbo.GetLostProjects(@TenantID,'','','',@value,'')
			--select TicketId,SectorChoice,Division from dbo.GetLostProjects(@TenantID,'','','',@value,'')
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetLostProjects(@TenantID,'','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetLostProjects(@TenantID,'','','','',@value)
		End
	ELSE
		Begin
			Insert into @Tickets select TicketId from dbo.GetLostProjects(@TenantID,'','','','','')
		End
			
	--select * from @Tickets
	
	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		ResourceUser nvarchar(256),
		Revenue decimal
		)

	IF @Category = 'sector' OR @Category = 'studio'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, ResourceUser) select @value, MonthStartDate, ResourceUser from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			--Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, ResourceUser) select @value, MonthStartDate, ResourceUser from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			--Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'jobtitle'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, ResourceUser) select usr.JobProfile, r.MonthStartDate, r.ResourceUser
			 from ResourceAllocationMonthly as r
			 join AspNetUsers as usr on r.ResourceUser = usr.Id
			 where r.TenantID = @TenantID and usr.TenantID = @TenantID
			 and r.MonthStartDate >= @StartDate and r.MonthStartDate <= @EndDate
			and usr.JobProfile = @value
			and r.ResourceWorkItem in (select TicketId from @Tickets)

			--Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1 and JobProfile = @value;
		End
	ELSE IF @Category = 'role'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, ResourceUser) select ResourceSubWorkItem, MonthStartDate, ResourceUser from ResourceAllocationMonthly where TenantID = @TenantID
			and ResourceSubWorkItem = @value
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select  TicketId from @Tickets)

			--Select @NoofResources = COUNT(*) from AspNetUsers as usr join Roles as r on usr.GlobalRoleID = r.Id where usr.TenantID = @TenantID
			--and r.Name = @value and r.TenantID = @TenantID and usr.Enabled = 1;
		End


	Update tmp
	set tmp.Revenue = jt.BillingLaborRate
	from @tmpRAMonthly tmp
	join AspNetUsers usr on tmp.ResourceUser = usr.Id
	join JobTitle jt on jt.ID = usr.JobTitleLookup
	where usr.TenantID = @TenantID and usr.TenantID = @TenantID and usr.Enabled = 1;
	
	Select @Result = ISNULL(SUM(Revenue), 0) from @tmpRAMonthly;

	RETURN @Result
END