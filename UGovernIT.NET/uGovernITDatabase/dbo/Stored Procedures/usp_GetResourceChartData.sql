
ALTER Procedure [dbo].[usp_GetResourceChartData]
@TenantId varchar(123),
@Startdate datetime,
@Endate datetime,
@filter nvarchar(250),
@division bigint = 0,
@studio bigint = 0,
@sector nvarchar(250) = '',
@base nvarchar(250)
as
Begin

		declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID
	IF @base = 'Division'
	begin
		select ResourceCount, DivisionLookup as Name, cd.Title from (
			select count(distinct a.resourceuser) ResourceCount, DivisionLookup from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
			where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
			group by DivisionLookup
			)temp join CompanyDivisions cd on temp.DivisionLookup = cd.ID where cd.TenantID=@TenantId
	End;
	Else IF @base = 'Sector'
	begin
		select count(distinct a.resourceuser) ResourceCount, SectorChoice as Name, SectorChoice as Title from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
		where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
		group by SectorChoice order by SectorChoice
	End;
	Else IF @base = 'Studio'
	begin
		select ResourceCount, StudioLookup as Name, sd.Title from (
			select count(distinct a.resourceuser) ResourceCount, StudioLookup from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
			where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
			group by StudioLookup
			)temp join Studio sd on temp.StudioLookup = sd.ID where sd.TenantID=@TenantId
			
			
	End;

End;
