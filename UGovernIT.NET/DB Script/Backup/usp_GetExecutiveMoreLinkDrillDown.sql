
Alter procedure [dbo].[usp_GetExecutiveMoreLinkDrillDown]
@TenantID nvarchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B',
@UserID nvarchar(128) = 'e0e963e5-c279-4d53-9998-88ddca47b8f6',
@Filter nvarchar(100) = 'Pipeline'
as
begin
		
		declare @rewardedStage int;
	set @rewardedStage = 0;
	select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	--pipeline
		select ms.TicketId, ms.UserName, ms.UserRole, cpr.Title, cpr.StageStep, ms.ModuleNameLookup, cpr.Closed, cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser, cpr.ProjectExecutiveUser, convert(varchar,cpr.EstimatedConstructionStart,1)as EstimatedConstructionStart, cpr.ProjectId, format(cpr.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar, cpr.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, cpr.EstimatorUser, cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,
		cpr.OpportunityTypeChoice from ModuleUserStatistics ms join crmproject cpr on ms.ticketid = cpr.ticketid where ms.TenantID=@TenantID and Closed != 1 and StageStep < @rewardedStage and UserName=@UserID
		union all
		select ms.TicketId, ms.UserName, ms.UserRole, opm.Title, opm.StageStep, ms.ModuleNameLookup, opm.Closed, opm.ProjectManagerUser, opm.SuperintendentUser, opm.AssistantProjectManagerUser, opm.ProjectExecutiveUser, convert(varchar, opm.EstimatedConstructionStart,1) as EstimatedConstructionStart, opm.ProjectId, format(opm.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar, opm.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, opm.EstimatorUser, opm.SectorChoice, opm.DivisionLookup, opm.StudioLookup,
		opm.OpportunityTypeChoice from ModuleUserStatistics ms join Opportunity opm on ms.ticketid = opm.ticketid where ms.TenantID=@TenantID and  Closed != 1 and UserName=@UserID
		union all
		select ms.TicketId, ms.UserName, ms.UserRole, cns.Title, cns.StageStep, ms.ModuleNameLookup, cns.Closed, cns.ProjectManagerUser, cns.SuperintendentUser, cns.AssistantProjectManagerUser, cns.ProjectExecutiveUser, convert(varchar,cns.EstimatedConstructionStart,1) as EstimatedConstructionStart, cns.ProjectId, format(cns.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar,cns.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, cns.EstimatorUser, cns.SectorChoice, cns.DivisionLookup, cns.StudioLookup,
		cns.OpportunityTypeChoice from ModuleUserStatistics ms join CRMServices cns on ms.ticketid = cns.ticketid where ms.TenantID=@TenantID and  Closed != 1 and UserName=@UserID
	
	  -- closed
		select ms.TicketId, ms.UserName, ms.UserRole, cpr.Title, cpr.StageStep, ms.ModuleNameLookup, cpr.Closed, cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser, cpr.ProjectExecutiveUser, convert(varchar,cpr.EstimatedConstructionStart,1) as EstimatedConstructionStart, cpr.ProjectId, format(cpr.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar,cpr.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, cpr.EstimatorUser, cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,
		cpr.OpportunityTypeChoice from ModuleUserStatistics ms join crmproject cpr on ms.ticketid = cpr.ticketid where ms.TenantID=@TenantID and Closed = 1 and UserName=@UserID
		union all
		select ms.TicketId, ms.UserName, ms.UserRole, opm.Title, opm.StageStep, ms.ModuleNameLookup, opm.Closed, opm.ProjectManagerUser, opm.SuperintendentUser, opm.AssistantProjectManagerUser, opm.ProjectExecutiveUser, convert(varchar,opm.EstimatedConstructionStart,1) as EstimatedConstructionStart, opm.ProjectId, format(opm.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar,opm.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, opm.EstimatorUser, opm.SectorChoice, opm.DivisionLookup, opm.StudioLookup,
		opm.OpportunityTypeChoice from ModuleUserStatistics ms join Opportunity opm on ms.ticketid = opm.ticketid where ms.TenantID=@TenantID and  Closed = 1 and UserName=@UserID
		union all
		select ms.TicketId, ms.UserName, ms.UserRole, cns.Title, cns.StageStep, ms.ModuleNameLookup, cns.Closed, cns.ProjectManagerUser, cns.SuperintendentUser, cns.AssistantProjectManagerUser, cns.ProjectExecutiveUser, convert(varchar,cns.EstimatedConstructionStart,1) as EstimatedConstructionStart, cns.ProjectId, format(cns.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar,cns.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, cns.EstimatorUser, cns.SectorChoice, cns.DivisionLookup, cns.StudioLookup,
		cns.OpportunityTypeChoice from ModuleUserStatistics ms join CRMServices cns on ms.ticketid = cns.ticketid where ms.TenantID=@TenantID and  Closed = 1 and UserName=@UserID
	
	-- on going
		select ms.TicketId, ms.UserName, ms.UserRole, cpr.Title, cpr.StageStep, ms.ModuleNameLookup, cpr.Closed, cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser, cpr.ProjectExecutiveUser, convert(varchar,cpr.EstimatedConstructionStart,1) as EstimatedConstructionStart, cpr.ProjectId, format(cpr.ApproxContractValue,'$0,, M') as ApproxContractValue, convert(varchar,cpr.EstimatedConstructionEnd,1) as EstimatedConstructionEnd, cpr.EstimatorUser, cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,
		cpr.OpportunityTypeChoice from ModuleUserStatistics ms join crmproject cpr on ms.ticketid = cpr.ticketid where ms.TenantID=@TenantID and Closed != 1 and StageStep >= @rewardedStage and UserName=@UserID
	
	-- thing to do
		select convert(varchar, StartDate,1) as StartDate, convert(varchar, DueDate,1) as DueDate, * from ModuleTasks where TenantID=@TenantID
		and AssignedToUser like '%' + LTRIM(RTRIM(@UserID)) + '%' and ModuleNameLookup in ('CPR','OPM','CNS')
	
End;