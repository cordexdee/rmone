CREATE Procedure [dbo].[usp_GetHelpCard]     
--[usp_GetHelpCard]  'C345E784-AA08-420F-B11F-2753BBEBFDD5'    
@TenantID VARCHAR(MAX),      
@IsClosed char(1) ='',    
@TicketId  varchar(max)=''    
as      
Begin      
select  a.ID
,a.AuthorizedToView
,a.HelpCardContentID
,a.TicketId
,a.Category
,a.Title
,a.TenantID
,a.Created
,a.Modified
,a.CreatedByUser [CreatedByUser$Id]
,a.ModifiedByUser [ModifiedByUser$Id]
,a.Deleted
,a.Attachments
,a.AgentLookUp [AgentLookUp$Id]
,a.Description,
[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser],[dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser],  
ag.Title[AgentLookUp]  
from HelpCard (READCOMMITTED) a     left join Agents ag on cast(isnull(ag.Id,0) as varchar)=isnull(a.AgentLookUp,0) and ag.TenantID=@TenantID  
where a.TenantID=@TenantID And  (a.Deleted<>1 or a.Deleted is null)      
and  isnull(a.TicketId,'') =CASE WHEN LEN(@TicketId)=0 then isnull(a.TicketId,'')  else @TicketId END      
End 




