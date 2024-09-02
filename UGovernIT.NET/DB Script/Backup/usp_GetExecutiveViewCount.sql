CREATE procedure [dbo].[usp_GetExecutiveViewCount]    
@TenantID nvarchar(128) = '35525396-e5fe-4692-9239-4df9305b915b',    
@UserID nvarchar(128) = '3ed90305-64bd-437b-aa56-935ebd0481d0'    
as    
begin    
      
  declare @rewardedStage int;    
 set @rewardedStage = 0;    
 select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID    
    
 select  'Pipeline' as KpiName, count(1) as Count from (    
 select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed   
 from crmproject cpr where cpr.TenantID=@TenantID and Closed != 1  
 and StageStep < @rewardedStage and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)    
 union all    
 select opm.TicketId, opm.Title, opm.StageStep, opm.Closed  from Opportunity opm where opm.TenantID=@TenantID and  Closed != 1   
 and opm.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)      
 union all    
 select cns.TicketId, cns.Title, cns.StageStep, cns.Closed  from CRMServices cns where cns.TenantID=@TenantID and  Closed != 1   
 and cns.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID))temp    
       
Union All    
    
 Select  'Closed' as KpiName, count(1) as Count from (    
 Select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed from crmproject cpr where cpr.TenantID=@TenantID and Closed = 1   
 and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
 Union all    
 Select opm.TicketId, opm.Title, opm.StageStep, opm.Closed  from Opportunity opm   
 where opm.TenantID=@TenantID and  Closed = 1 and   
 opm.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)  
 union all    
 Select cns.TicketId, cns.Title, cns.StageStep, cns.Closed  from CRMServices cns where cns.TenantID=@TenantID and  Closed = 1   
 and cns.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID))temp  
    
    
 Union All    
    
 select  'Construction' as KpiName, count(1) as Count from (    
 select cpr.TicketId, cpr.Title, cpr.StageStep, cpr.Closed   
 from crmproject cpr where cpr.TenantID=@TenantID and Closed != 1  
 and cpr.TicketId in (select distinct ms.TicketId from ModuleUserStatistics ms   
where ms.TenantID=@TenantID and ms.UserName=@UserID)and    
 StageStep >= @rewardedStage    
 )temp    
    
 Union All    
    
 select 'Things to do' as KpiName, count(1) as Count from ModuleTasks where TenantID=@TenantID    
 and AssignedToUser like '%' + LTRIM(RTRIM(@UserID)) + '%' and ModuleNameLookup in ('CPR','OPM','CNS')    
    
End;    
    