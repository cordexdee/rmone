-- select dbo.fnGetMarginsLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-22','sector','Aviation')
-- select dbo.fnGetMarginsLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-22','division','1-21 Silicon Valley')
-- select dbo.fnGetMarginsLost('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-22','studio','SF Studio 1')
CREATE FUNCTION [dbo].[fnGetMarginsLost] 
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
			--select TicketId,SectorChoice,Division from dbo.GetLostProjects(@TenantID,'','','',@value)
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetLostProjects(@TenantID,'','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetLostProjects(@TenantID,'','','','',@value)
		End
			
	--select * from @Tickets
	
	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		ResourceUser nvarchar(256),
		DepartmentId bigint,
		BillingRate decimal,
		Cost decimal
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

	Update tmp
	set tmp.DepartmentId = usr.DepartmentLookup
	from @tmpRAMonthly tmp
	join AspNetUsers usr on tmp.ResourceUser = usr.Id
	where usr.TenantID = @TenantID and usr.TenantID = @TenantID and usr.Enabled = 1;
	
	Update tmp
	set tmp.BillingRate = jt.BillingLaborRate, tmp.Cost = jt.EmployeeCostRate
	from @tmpRAMonthly tmp
	join AspNetUsers usr on tmp.ResourceUser = usr.Id
	join JobTitle jt on jt.ID = usr.JobTitleLookup
	where usr.DepartmentLookup = jt.DepartmentId and usr.GlobalRoleID = jt.RoleId and usr.TenantID = @TenantID and usr.TenantID = @TenantID and usr.Enabled = 1;


	Select @Result = (ISNULL(SUM(BillingRate), 0) - ISNULL(SUM(Cost), 0)) from @tmpRAMonthly;

	RETURN @Result
END