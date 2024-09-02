

ALTER procedure [dbo].[usp_GetMonthWiseUtilizationChart]
--exec [usp_GetMonthWiseUtilization] 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069', 'Aviation', '', 'Los Angeles', 'Pipeline'
@Startdate datetime ='',
@Endate datetime ='',
@tenantID nvarchar(max) = '',
@sector nvarchar(max) = '',
@studio nvarchar(max) = '',
@division nvarchar(max) = '',
@filter nvarchar(max) = '',
@billable nvarchar(100) = 'Billable'
as
begin

	Declare @Days int=0,@BillableResourceCount int =0;
	Set @Days=(Select dbo.fnGetWorkingDays(@Startdate,@Endate))
	Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
	Where JobType=@billable  and GlobalRoleID is not null and JobProfile is not null)

	Select LEFT(DATENAME(m,a.MonthStartDate), 3) Months,
	Ceiling((Sum(a.PctAllocation)/100)) FTE,
	[dbo].[fnGetWorkingDaysTillDate](Month(a.MonthStartDate))WorkingDays,
	Round(((Sum(a.PctAllocation)/100) *[dbo].[fnGetWorkingDaysTillDate](Month(a.MonthStartDate))),2) FinalCount
	from ResourceAllocationMonthly a 
	Where a.TenantID=@TenantId
	and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
	and a.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId)
	Where JobType=@billable  and GlobalRoleID is not null and JobProfile is not null 
	)
	and a.ResourceWorkItem in (
		Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
	)
	group by  DATENAME(m,a.MonthStartDate), Month(a.MonthStartDate) order by Month(a.MonthStartDate)	
End;


