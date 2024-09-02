    
CREATE Procedure [dbo].[usp_GetApplicationServers]    
@TenantID varchar(200),    
@APPTitleLookup int =0 ,
@AssetsTitleLookup int=0

as    
begin    
if(@APPTitleLookup>0)  
begin  
Select a.*,b.Title'AppTitle',Isnull(c.Title,'NA')'EnvTitle',d.Title'AssetTitle',   
d.AssetTagNum,d.AssetDescription , d.TicketId,d.HostName  
from ApplicationServers a    
left join Applications b on a.APPTitleLookup=b.ID    
left join Environment c on a.EnvironmentLookup=c.ID    
left join Assets d on a.AssetsTitleLookup=d.ID    
where a.APPTitleLookup=@APPTitleLookup and  a.TenantID =@TenantID   
and  b.TenantID =@TenantID and  c.TenantID =@TenantID and  b.TenantID =@TenantID  
end 
Else If (@AssetsTitleLookup>0)
Begin
Select a.*,b.Title'AppTitle',Isnull(c.Title,'NA')'EnvTitle',d.Title'AssetTitle',   
d.AssetTagNum,d.AssetDescription , d.TicketId,d.HostName  
from ApplicationServers a    
left join Applications b on a.APPTitleLookup=b.ID    
left join Environment c on a.EnvironmentLookup=c.ID    
left join Assets d on a.AssetsTitleLookup=d.ID    
where a.AssetsTitleLookup=@AssetsTitleLookup and  a.TenantID =@TenantID   
and  b.TenantID =@TenantID and  c.TenantID =@TenantID and  b.TenantID =@TenantID  
End
Else Begin  
Select a.*,b.Title'AppTitle',Isnull(c.Title,'NA')'EnvTitle',d.Title'AssetTitle',d.AssetTagNum,  
d.TicketId,d.AssetDescription,d.TicketId,d.HostName from ApplicationServers a    
left join Applications b on a.APPTitleLookup=b.ID    
left join Environment c on a.EnvironmentLookup=c.ID    
left join Assets d on a.AssetsTitleLookup=d.ID    
where a.TenantID =@TenantID    
and  b.TenantID =@TenantID and  c.TenantID =@TenantID and  b.TenantID =@TenantID  
End  
end