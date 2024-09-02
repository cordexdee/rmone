

ALTER Procedure [dbo].[usp_GetChartData]
@TenantId varchar(123)= '', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',
@Startdate datetime = '', --'2022-01-01 00:00:00.000',
@Endate datetime = '', --'2022-07-31 00:00:00.000',
@filter nvarchar(250) = '',
@division int = 0,
@studio nvarchar(250) = '',
@sector nvarchar(250) = '',
@base nvarchar(250) = ''
as
Begin

		declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID
	IF @base = 'Division'
	begin
	select FTE, ResourceUser, Amount, Name, cd.Title as ArgumentField from (
	select sum(temp.FTE)/100 FTE, Sum(temp.ResourceUser) ResourceUser, Sum(temp.Amount) Amount, ArgumentField as Name from	(
		select Sum(a.PctAllocation) FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(a.ResourceWorkItem) Amount,
		   b.DivisionLookup as ArgumentField
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate 
		group by ResourceWorkItem, DivisionLookup Having DivisionLookup > 0
		)temp group by temp.ArgumentField
		)divisions join CompanyDivisions cd on divisions.Name = cd.ID where cd.TenantID=@TenantId
		Order By Name
	End;
	Else IF @base = 'Sector'
	begin
	select sum(temp.FTE)/100 FTE, Sum(temp.ResourceUser) ResourceUser, Sum(temp.Amount) Amount, ArgumentField as Name, ArgumentField from	(
		select Sum(a.PctAllocation) FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(ResourceWorkItem) Amount,
		   b.SectorChoice as ArgumentField, b.SectorChoice as Name
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by ResourceWorkItem, SectorChoice Having LEN(SectorChoice) > 0
		)temp group by temp.ArgumentField
		Order By Name
	End;
	Else IF @base = 'Studio'
	begin
	
	select sum(temp.FTE)/100 FTE, Sum(temp.ResourceUser) ResourceUser, Sum(temp.Amount) Amount, ArgumentField as Name, ArgumentField from	(
		select Sum(a.PctAllocation) FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(a.ResourceWorkItem) Amount,
		   b.StudioChoice as ArgumentField
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by ResourceWorkItem, b.StudioChoice Having LEN(b.StudioChoice) > 0
		)temp group by temp.ArgumentField
		Order By Name
	End;
	Else
	Begin
	select temp.FTE, temp.ResourceUser, temp.Amount, temp.ArgumentField, st.Title as Name from	(
	select Sum(a.PctAllocation)/100 FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(a.ResourceWorkItem) Amount,
		   b.StudioChoice as ArgumentField
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by ResourceWorkItem, StudioChoice Having LEN(StudioChoice) > 0
		)temp join Studio st on temp.ArgumentField = st.ID
		Order By Name
	End;
End;