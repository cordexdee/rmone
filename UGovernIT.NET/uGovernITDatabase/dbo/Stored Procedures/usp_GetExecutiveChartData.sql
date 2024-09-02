
ALTER Procedure [dbo].[usp_GetExecutiveChartData]
@TenantId varchar(123)= '35525396-E5FE-4692-9239-4DF9305B915B',
@Startdate datetime = '2023-01-01 00:00:00.000',
@Endate datetime = '2023-01-31 00:00:00.000',
@filter nvarchar(250) = 'Pipeline',
@division bigint = 0,
@studio bigint = 0,
@sector nvarchar(250) = '',
@base nvarchar(250) = 'Studio'
as
Begin

		declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

		Declare @OverheadResourceCount int=0,@BillableResourceCount int =0;
		Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
		Where JobType='Billable' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null)

		Declare @ytdHours int;
		If MONTH(@Startdate)=MONTH(@Endate)
		Begin
			Set @ytdHours= 22*8;
		End
		Else 
		Begin
			Set @ytdHours =22*8*(DateDiff(Month,@Startdate,@Endate)+1);
		End
		Declare @totalWorkHours int = @BillableResourceCount * @ytdHours;

	IF @base = 'Division'
	begin
		select utilization, capacity, cd.Title as Name, cd.ID as Lookup from (
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, DivisionLookup as Name
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by DivisionLookup)temp join CompanyDivisions cd on temp.Name = cd.ID where cd.TenantID=@TenantId
	End;
	Else IF @base = 'Sector'
	begin
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, SectorChoice as Name, SectorChoice as Lookup
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by SectorChoice
	End;
	Else IF @base = 'Studio'
	begin
	
		select utilization, capacity, sd.Title as Name, sd.ID as Lookup from (
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, StudioLookup as Name
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by StudioLookup)temp join Studio sd on temp.Name = sd.ID where sd.TenantID=@TenantId
	End;
	Else
	Begin
		select Round(((sum(a.AllocationHour)/@ytdHours)/@BillableResourceCount)*100, 1) as utilization, count(distinct a.ResourceUser) as capacity, SubWorkItem as Name, SubWorkItem as Lookup
		from ResourceUsageSummaryMonthWise a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.WorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by SubWorkItem
	End;
End;
