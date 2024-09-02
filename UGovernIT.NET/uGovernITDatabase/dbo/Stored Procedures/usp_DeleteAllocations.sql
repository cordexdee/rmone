
ALTER PROCEDURE usp_DeleteAllocations @TenantId varchar(128), @ProjectEstId varchar(max), @TicketID VARCHAR(25) 
AS 
 BEGIN 
	  
SELECT VALUE into #templist FROM STRING_SPLIT((SELECT @ProjectEstId),',')

DECLARE @COUNTER INT = 0;
DECLARE @MAX INT = (SELECT COUNT(*) FROM #templist)
DECLARE @VALUE VARCHAR(50);
DECLARE @StarDate datetime;
DECLARE @EndDate datetime;

WHILE @COUNTER < @MAX
BEGIN

SET @VALUE = (SELECT VALUE FROM
      (SELECT (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))) [index] , Value from #templist) R 
       ORDER BY R.[index] OFFSET @COUNTER 
       ROWS FETCH NEXT 1 ROWS ONLY);

set @StarDate = (select top 1 AllocationStartDate from ProjectEstimatedAllocation where id = @VALUE);
set @EndDate = (select top 1 AllocationEndDate from ProjectEstimatedAllocation where id = @VALUE);
	
	DELETE FROM ResourceAllocationMonthly WHERE TenantID=@TenantId and ActualStartDate = @StarDate and ActualEndDate = @EndDate     
    and ResourceWorkItemLookup in ( Select ResourceWorkItemLookup  FROM ResourceAllocation WHERE 
    ProjectEstimatedAllocationId = @VALUE)AND TenantID=@TenantId  AND ResourceWorkItem = @TicketID 
	 
    DELETE FROM ResourceUsageSummaryMonthWise WHERE TenantID=@TenantId and ActualStartDate = @StarDate and ActualEndDate = @EndDate       
    and WorkItemID in ( Select ResourceWorkItemLookup  FROM ResourceAllocation WHERE 
    ProjectEstimatedAllocationId = @VALUE)AND TenantID=@TenantId AND WorkItem = @TicketID    
      
    DELETE FROM ResourceUsageSummaryWeekWise WHERE TenantID=@TenantId and ActualStartDate = @StarDate and ActualEndDate = @EndDate      
    AND WorkItemID in ( Select ResourceWorkItemLookup  FROM ResourceAllocation WHERE 
    ProjectEstimatedAllocationId = @VALUE)AND TenantID=@TenantId  AND WorkItem = @TicketID   

PRINT @VALUE

SET @COUNTER = @COUNTER + 1

END
DROP TABLE #templist	  
     DELETE FROM ResourceAllocation WHERE TenantID=@TenantId      
     AND ProjectEstimatedAllocationId in (SELECT VALUE FROM STRING_SPLIT((SELECT @ProjectEstId),',')) AND TicketId = @TicketID    
     --End    
     DELETE FROM ProjectEstimatedAllocation WHERE TenantID=@TenantId      AND Id in (SELECT VALUE FROM STRING_SPLIT((SELECT @ProjectEstId),',')) AND TicketId = @TicketID  
   --End  
End