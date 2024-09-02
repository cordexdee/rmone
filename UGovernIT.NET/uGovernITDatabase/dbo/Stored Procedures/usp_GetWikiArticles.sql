CREATE Procedure usp_GetWikiArticles  
--[usp_GetWikiArticles] '35525396-e5fe-4692-9239-4df9305b915b'    
@TenantID VARCHAR(MAX),        
@IsClosed char(1) ='',      
@TicketId  varchar(max)=''      
as  
Begin  
select  a.*,[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$],[dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$],    
r.RequestType[RequestTypeLookup$],a.ModuleNameLookup[ModuleNameLookup$]    
from WikiArticles (READCOMMITTED) a     left join Config_Module_RequestType r on cast(isnull(r.Id,0) as varchar)=isnull(a.RequestTypeLookup,0) and r.TenantID=@TenantID    
where a.TenantID=@TenantID And  (a.Deleted<>1 or a.Deleted is null)        
and  isnull(a.TicketId,'') =CASE WHEN LEN(@TicketId)=0 then isnull(a.TicketId,'')  else @TicketId END        
End  
  
  
  