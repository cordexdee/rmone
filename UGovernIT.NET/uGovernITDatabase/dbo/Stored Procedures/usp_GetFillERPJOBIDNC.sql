CREATE Procedure Usp_GetFillERPJOBIDNC  
@TenantID varchar(128)  
as  
begin  
Select TicketId from Opportunity where TenantID= @TenantID and LEN(ERPJobID)>0
Declare @count int=0  
Set @count= (Select Count(1) from Opportunity where TenantID=@TenantID and LEN(ERPJobID)>0);  
if(@count>0)  
begin  
update Opportunity set ERPJobIDNC=ERPJobID where TenantID=@TenantID  
and LEN(ERPJobID)>0;  
update Opportunity set ERPJobID=Null where TenantID=@TenantID  
and LEN(ERPJobID)>0;  
End  
End


 


