
ALTER Procedure [dbo].[usp_GetProjectList]
@TenantId varchar(123)= '', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',
@Startdate datetime = '', --'2022-01-01 00:00:00.000',
@Endate datetime = '', --'2022-07-31 00:00:00.000',
@filter nvarchar(250) = '',
@division bigint = 0,
@studio bigint = 0,
@sector nvarchar(250) = '',
@base nvarchar(250) = ''
as
Begin

		declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

		select distinct ResourceWorkItem as TicketId, SectorChoice, DivisionLookup, 
		DivisionLookup, b.Title as Title, b.ApproxContractValue, b.EstimatedConstructionStart, b.EstimatedConstructionEnd, b.ProjectId, b.ProjectManagerUser, b.CRMCompanyLookup, b.StudioLookup, b.OpportunityTypeChoice
		from ResourceAllocationMonthly a join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate


End;
