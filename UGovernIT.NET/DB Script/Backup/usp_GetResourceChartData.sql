

ALTER Procedure [dbo].[usp_GetResourceChartData]
@TenantId varchar(123)= 'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',
@Startdate datetime = '2022-01-01 00:00:00.000',
@Endate datetime = '2022-07-31 00:00:00.000',
@filter nvarchar(250) = 'Pipeline',
@division int = 0,
@studio nvarchar(250) = '',
@sector nvarchar(250) = '',
@base nvarchar(250) = 'Sector'
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
	
	select count(distinct a.resourceuser) ResourceCount, StudioChoice as Name, StudioChoice as Title from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
			where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
			group by StudioChoice
			
	End;

End;