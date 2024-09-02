CREATE Procedure [dbo].[usp_GetResourceAllocation]    
@TenantID VARCHAR(MAX),    
@IsClosed char(1)='',  
@TicketId VARCHAR(MAX)=''   
as    
Begin    
select  a.*,r.title[a.ResourceWorkItemLookup$] ,[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$],  
[dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$],[dbo].[fnGetusername](a.ResourceUser,@TenantID)[ResourceUser$]    
from ResourceAllocation (READCOMMITTED) a     
left join ResourceWorkItems (READCOMMITTED) r on r.ID=a.ResourceWorkItemLookup    
where a.TenantID=@TenantID And  (a.Deleted<>1 or a.Deleted is null)    
End    
    
      
