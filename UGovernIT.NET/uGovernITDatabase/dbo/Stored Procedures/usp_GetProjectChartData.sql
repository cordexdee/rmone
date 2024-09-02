
ALTER Procedure [dbo].[usp_GetProjectChartData]
@TenantId varchar(123)= '35525396-E5FE-4692-9239-4DF9305B915B', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',
@Startdate datetime = '2023-01-01', --'2022-01-01 00:00:00.000',
@Endate datetime = '2023-12-31', --'2022-07-31 00:00:00.000',
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
	IF @base = 'Division'
	begin
	select Projects, ArgumentValue as Name, cd.Title as ArgumentField from
		(select count(resourceworkItem) as Projects, DivisionLookup as ArgumentValue from
		(select distinct ResourceWorkItem, SectorChoice, DivisionLookup, StudioLookup
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp
		group by DivisionLookup
		)divisions join CompanyDivisions cd on divisions.ArgumentValue = cd.ID where cd.TenantID=@TenantId
				Order By Name
	End;
	Else IF @base = 'Sector'
	begin
	select count(resourceworkItem) as Projects, SectorChoice as Name, SectorChoice as ArgumentField  from
		(select distinct ResourceWorkItem, SectorChoice, DivisionLookup, StudioLookup
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp
		group by SectorChoice
	End;
	Else IF @base = 'Studio'
	begin
		select Projects, ArgumentValue as Name, sd.Title as ArgumentField from
		( select count(resourceworkItem) as Projects, StudioLookup as Name, StudioLookup as ArgumentValue from
			(select distinct ResourceWorkItem, SectorChoice, DivisionLookup, StudioLookup
			from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
					where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp
			group by StudioLookup
		)studios join Studio sd on studios.ArgumentValue = sd.ID where sd.TenantID=@TenantId
				Order By Name
	End;
	Else IF @base = 'Complexity'
	begin
			select count(resourceworkItem) as Projects, 'Level ' + CRMProjectComplexityChoice as Name, CRMProjectComplexityChoice as ArgumentField from(
					select distinct ResourceWorkItem, SectorChoice, DivisionLookup, StudioLookup, CRMProjectComplexityChoice
			from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp
				group by CRMProjectComplexityChoice
	End;
	Else
	Begin
	select distinct ResourceWorkItem, SectorChoice, DivisionLookup, StudioLookup
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
	End;
End;
