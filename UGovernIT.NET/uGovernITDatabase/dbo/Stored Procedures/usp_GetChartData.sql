

ALTER Procedure [dbo].[usp_GetChartData]
@TenantId varchar(123),
@Startdate datetime,
@Endate datetime,
@filter nvarchar(250) = '',
@division bigint = 0,
@studio bigint = 0,
@sector nvarchar(250) = '',
@base nvarchar(250) = ''
--exec usp_GetChartData '35525396-e5fe-4692-9239-4df9305b915b','2023-01-01','2023-12-31','Pipeline',0,0,'','Sector'
as
Begin

		declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID
	IF @base = 'Division'
	begin
	select FTE, ResourceUser, Amount, cd.ID as Name, cd.Title as ArgumentField from (
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
	select FTE, ResourceUser, Amount, sd.ID as Name, sd.Title as ArgumentField from (
	select sum(temp.FTE)/100 FTE, Sum(temp.ResourceUser) ResourceUser, Sum(temp.Amount) Amount, ArgumentField as Name, ArgumentField from	(
		select Sum(a.PctAllocation) FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(a.ResourceWorkItem) Amount,
		   b.StudioLookup as ArgumentField
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by ResourceWorkItem, b.StudioLookup Having b.StudioLookup > 0
		)temp group by temp.ArgumentField
		)studios join Studio sd on studios.Name = sd.ID where sd.TenantID=@TenantId
		Order By Name
	End;
	Else
	Begin
	select temp.FTE, temp.ResourceUser, temp.Amount, temp.ArgumentField, st.Title as Name from	(
	select Sum(a.PctAllocation)/100 FTE, count(distinct a.ResourceUser) as ResourceUser, sum(b.ApproxContractValue)/count(a.ResourceWorkItem) Amount,
		   b.StudioLookup as ArgumentField
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by ResourceWorkItem, StudioLookup Having b.StudioLookup > 0
		)temp join Studio st on temp.ArgumentField = st.ID
		Order By Name
	End;
End;