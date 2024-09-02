Create procedure usp_GetStaffingChartData
@TenantID varchar(128)  = '35525396-E5FE-4692-9239-4DF9305B915B',
@Startdate datetime ='2022-07-01',
@Enddate datetime ='2022-12-31',
@Role nvarchar(max) = '',
@Studio nvarchar(max) = '',
@Division bigint = 0
as
Begin

	Declare @OverheadResourceCount int=0,@BillableResourceCount int =0;
		Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
		Where JobType='Billable' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null)

		Declare @ytdHours int;
		If MONTH(@Startdate)=MONTH(@Enddate)
		Begin
			Set @ytdHours= 22*8;
		End
		Else 
		Begin
			Set @ytdHours =22*8*(DateDiff(Month,@Startdate,@Enddate)+1);
		End
		Declare @totalWorkHours int = @BillableResourceCount * @ytdHours;


	IF @Division > 0
	begin
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, StudioChoice as Name, StudioChoice as Lookup
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,'Pipeline',@Studio,@Division,'') b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Enddate
		group by StudioChoice
	End
	else
	begin
		select utilization, capacity, cd.Title as Name, cd.ID as Lookup from (
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, DivisionLookup as Name
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,'Pipeline',@Studio,@Division,'') b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Enddate
		group by DivisionLookup)temp join CompanyDivisions cd on temp.Name = cd.ID where cd.TenantID=@TenantId
	end;
End;