CREATE procedure [dbo].[usp_GetExecutiveViewDrillDown]    
@TenantID nvarchar(128) = '35525396-e5fe-4692-9239-4df9305b915b',    
@UserID nvarchar(128) = '3ed90305-64bd-437b-aa56-935ebd0481d0',    
@Filter nvarchar(100) = 'Construction'    
as    
begin    
declare @rewardedStage int;    
set @rewardedStage = 0;    
select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID    
    
IF (@Filter = 'Pipeline')    
Begin  
Select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed, cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser,  
cpr.ProjectExecutiveUser, cpr.EstimatedConstructionStart, cpr.ProjectId, cpr.ApproxContractValue, cpr.EstimatedConstructionEnd, cpr.EstimatorUser,  
cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,    
cpr.OpportunityTypeChoice from  
crmproject cpr where cpr.TenantID=@TenantID and cpr.Closed != 1 and cpr.StageStep < @rewardedStage  
and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
union all    
select opm.TicketId,  opm.Title, opm.StageStep, opm.Closed, opm.ProjectManagerUser,  
opm.SuperintendentUser, opm.AssistantProjectManagerUser, opm.ProjectExecutiveUser, opm.EstimatedConstructionStart,  
opm.ProjectId, opm.ApproxContractValue, opm.EstimatedConstructionEnd, opm.EstimatorUser, opm.SectorChoice,  
opm.DivisionLookup, opm.StudioLookup,    
opm.OpportunityTypeChoice from Opportunity opm where opm.TenantID=@TenantID  
and  opm.Closed != 1 and opm.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
union all    
select cns.TicketId, cns.Title, cns.StageStep, cns.Closed, cns.ProjectManagerUser, cns.SuperintendentUser, cns.AssistantProjectManagerUser, cns.ProjectExecutiveUser, cns.EstimatedConstructionStart  
,cns.ProjectId, cns.ApproxContractValue, cns.EstimatedConstructionEnd, cns.EstimatorUser, cns.SectorChoice, cns.DivisionLookup, cns.StudioLookup,    
cns.OpportunityTypeChoice from CRMServices cns where cns.TenantID=@TenantID and  cns.Closed != 1   
and cns.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
End    
Else IF (@Filter = 'Closed')    
 Begin    
Select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed,   
cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser, cpr.ProjectExecutiveUser,  
cpr.EstimatedConstructionStart, cpr.ProjectId, cpr.ApproxContractValue, cpr.EstimatedConstructionEnd, cpr.EstimatorUser,  
cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,    
cpr.OpportunityTypeChoice from crmproject cpr where cpr.TenantID=@TenantID and cpr.Closed = 1   
and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
  
union all    
  select opm.TicketId, opm.Title, opm.StageStep, opm.Closed, opm.ProjectManagerUser, opm.SuperintendentUser,   
opm.AssistantProjectManagerUser, opm.ProjectExecutiveUser,  
opm.EstimatedConstructionStart, opm.ProjectId, opm.ApproxContractValue, opm.EstimatedConstructionEnd, opm.EstimatorUser,  
opm.SectorChoice, opm.DivisionLookup, opm.StudioLookup,    
opm.OpportunityTypeChoice from   
Opportunity opm where opm.TenantID=@TenantID and  opm.Closed = 1   
and opm.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
  union all    
select cns.TicketId, cns.Title, cns.StageStep, cns.Closed, cns.ProjectManagerUser,   
cns.SuperintendentUser, cns.AssistantProjectManagerUser, cns.ProjectExecutiveUser, cns.EstimatedConstructionStart,   
cns.ProjectId, cns.ApproxContractValue, cns.EstimatedConstructionEnd, cns.EstimatorUser, cns.SectorChoice, cns.DivisionLookup,   
cns.StudioLookup,  cns.OpportunityTypeChoice from CRMServices cns   
where cns.TenantID=@TenantID and  cns.Closed = 1  
and cns.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
 End    
 Else if (@Filter = 'Construction')    
 Begin    
select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed,   
cpr.ProjectManagerUser, cpr.SuperintendentUser, cpr.AssistantProjectManagerUser, cpr.ProjectExecutiveUser,   
cpr.EstimatedConstructionStart, cpr.ProjectId, cpr.ApproxContractValue, cpr.EstimatedConstructionEnd,  
cpr.EstimatorUser, cpr.SectorChoice, cpr.DivisionLookup, cpr.StudioLookup,    
cpr.OpportunityTypeChoice from crmproject cpr  
where cpr.TenantID=@TenantID and cpr.Closed != 1 and cpr.StageStep >= @rewardedStage    
and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
 End    
 Else if(@Filter = 'Things to do')    
 Begin    
  select * from ModuleTasks where TenantID=@tenantID    
  and AssignedToUser like '%' + LTRIM(RTRIM(@UserID)) + '%' and ModuleNameLookup in ('CPR','OPM','CNS')    
 End    
End;    