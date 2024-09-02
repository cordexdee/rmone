CREATE FUNCTION [dbo].[fnGetResourceAllocationCount]
(  
 @TicketID NVARCHAR(100),    
 @TenantID NVARCHAR(128)    
)  
RETURNS nvarchar(50)  
AS  
BEGIN  
/*
 Declare @Count int;  
 Select @Count = count(distinct AssignedToUser) from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID and Deleted = 0;  
 return @Count;  
 */

 Declare @AllocationCount nvarchar(25); 
 Declare @UnfilledCount nvarchar(25); 

 Select @AllocationCount = count(AssignedToUser) from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID and Deleted = 0; -- and AssignedToUser != '00000000-0000-0000-0000-000000000000';  
 Select @UnfilledCount = count(AssignedToUser) from ProjectEstimatedAllocation where TenantID = @TenantID and TicketID = @TicketID and Deleted = 0 and AssignedToUser = '00000000-0000-0000-0000-000000000000';  

 return @AllocationCount + ';#' + @UnfilledCount;
END
