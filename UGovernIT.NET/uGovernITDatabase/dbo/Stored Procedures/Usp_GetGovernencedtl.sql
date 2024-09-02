   
    
Create Procedure [dbo].[Usp_GetGovernencedtl]    
@TenantID nvarchar(128)  ,  
@url nvarchar(max),  
@nprAbsoluteURL nvarchar(max),  
@pmmAbsoluteURL nvarchar(max),  
@userid nvarchar(128)  
   
as    
begin    
Declare @budamt float=0  
   
Select NPR.TicketId, NPR.Title,Status, p.Title PriorityLookup, dbo.fnGetusername(SponsorsUser,@TenantID)SponsorsUser,    
REPLACE(CONVERT(NVARCHAR,NPR.ActualCompletionDate, 106), ' ', '-')ActualCompletionDate, REPLACE(CONVERT(NVARCHAR,NPR.ActualStartDate, 106), ' ', '-')ActualStartDate,    
(Select  isnull(sum(m.BudgetAmount),0) from modulebudget m where m.TicketId=NPR.TicketId and m.TenantID=@TenantID and m.ModuleNameLookup='NPR')BudgetAmount,  
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@nprAbsoluteURL+''',''TicketId='+NPR.TicketId+''', ''NPR Request:'+NPR.Title+' '', 90, 90);">'+NPR.Title+'</a>' TitleLink,  
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@url+''',''control=nprbudget&IsReadOnly=true&NPRID='+cast(NPR.ID as varchar)+''', ''NPR Request:'+NPR.TicketId+' '', 80, 90);">'
+(Select FORMAT(sum(m.BudgetAmount),'C') from modulebudget m where m.TicketId=NPR.TicketId and m.TenantID=@TenantID and m.ModuleNameLookup='NPR')+'</a>' BudgetAmountWithLink,  
'NPR' ModuleName, NPR.StageStep,dbo.CheckUserIsInGroup(@TenantID ,@userid,'NPRITGovApprover') AuthorizedToApprove  
from NPR left join Config_Module_Priority p on p.ID= NPR.PriorityLookup      
where NPR.TenantID=@TenantID   
and ModuleStepLookup in (Select id from Config_Module_ModuleStages where ModuleNameLookup='NPR'    
and (CustomProperties like '%ITGReview%') and TenantID=@TenantID)  
  
union   
Select NPR.TicketId, NPR.Title,Status, p.Title PriorityLookup, dbo.fnGetusername(SponsorsUser,@TenantID)SponsorsUser,    
REPLACE(CONVERT(NVARCHAR,NPR.ActualCompletionDate, 106), ' ', '-')ActualCompletionDate, REPLACE(CONVERT(NVARCHAR,NPR.ActualStartDate, 106), ' ', '-')ActualStartDate,    
(Select  isnull(sum(m.BudgetAmount),0) from modulebudget m where m.TicketId=NPR.TicketId and m.TenantID=@TenantID and m.ModuleNameLookup='NPR')BudgetAmount,  
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@nprAbsoluteURL+''',''TicketId='+NPR.TicketId+''', ''NPR Request:'+NPR.Title+' '', 90, 90);">'+NPR.Title+'</a>' TitleLink,  
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@url+''',''control=nprbudget&IsReadOnly=true&NPRID='+cast(NPR.ID as varchar)+''', ''NPR Request:'+NPR.TicketId+' '', 80, 90);">'+
(Select FORMAT(sum(m.BudgetAmount),'C') from modulebudget m where m.TicketId=NPR.TicketId and m.TenantID=@TenantID and m.ModuleNameLookup='NPR')+'</a>' BudgetAmountWithLink,  
'NPR' ModuleName, NPR.StageStep,dbo.CheckUserIsInGroup(@TenantID ,@userid,'NPRITSCApprover') AuthorizedToApprove  
from NPR left join Config_Module_Priority p on p.ID= NPR.PriorityLookup      
where NPR.TenantID=@TenantID   
and ModuleStepLookup in (Select id from Config_Module_ModuleStages where ModuleNameLookup='NPR'    
and (CustomProperties like '%ITSCReview%') and TenantID=@TenantID)  
  
  
Union    
Select TicketId, PMM.Title, 'Pending Budget Approval' Status , p.Title PriorityLookup ,dbo.fnGetusername(SponsorsUser,@TenantID)SponsorsUser ,    
REPLACE(CONVERT(NVARCHAR,PMM.ActualCompletionDate, 106), ' ', '-')ActualCompletionDate, REPLACE(CONVERT(NVARCHAR,PMM.ActualStartDate, 106), ' ', '-')ActualStartDate,    
(Select Sum(Case When m.UnapprovedAmount > 0 then  m.BudgetAmount + m.UnapprovedAmount else 0 end )from modulebudget m where m.TicketId=PMM.TicketId and m.ModuleNameLookup='PMM' and m.TenantID=@TenantID and BudgetStatus=0)BudgetAmount,    
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@pmmAbsoluteURL+''',''TicketId='+PMM.TicketId+''', ''PMM Project:'+PMM.Title+' '', 90, 90);">'+PMM.Title+'</a>' TitleLink,  
'<a href=''javascript:void(0)'' onclick="window.parent.UgitOpenPopupDialog('''+@pmmAbsoluteURL+''',''control=nprbudget&IsReadOnly=true&NPRID='+cast(PMM.ID as varchar)+''', ''NPR Request:'+PMM.TicketId+' '', 80, 50);">'+
(Select FORMAT(Sum(Case When m.UnapprovedAmount > 0 then  m.BudgetAmount + m.UnapprovedAmount else 0 end ),'C') from modulebudget m where m.TicketId=PMM.TicketId and m.ModuleNameLookup='PMM' and m.TenantID=@TenantID and BudgetStatus=0)+'</a>' BudgetAmountWithLink,  
'PMM' ModuleName,PMM.StageStep,dbo.CheckUserIsInGroup(@TenantID ,@userid,'PMMBudgetApprover') AuthorizedToApprove  
from PMM left join Config_Module_Priority p on p.ID= PMM.PriorityLookup where PMM.TenantID=@TenantID     
and PMM.TicketId in (Select TicketId from ModuleBudget where TenantID=@TenantID and ModuleNameLookup='PMM' and BudgetStatus=0)    
    
End    
    
    
    
     
    
    