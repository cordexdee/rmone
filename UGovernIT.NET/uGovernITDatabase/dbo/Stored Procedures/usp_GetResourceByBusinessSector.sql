CREATE procedure [dbo].[usp_GetResourceByBusinessSector]  
@TenantID varchar(max)  
as  
Begin  
SELECT DISTINCT dbo.vw_ProjectAllocation.AssignedToUser, [dbo].[fnGetusername](dbo.vw_ProjectAllocation.AssignedToUser,@TenantID)[AssignedToUser$], dbo.vw_ProjectAllocation.TenantID, dbo.Config_Module_RequestType.Category, ISNULL(dbo.vw_AllProjectItems.Closed,0)Closed  
FROM            dbo.vw_ProjectAllocation INNER JOIN  
                         dbo.vw_AllProjectItems ON dbo.vw_ProjectAllocation.TicketID = dbo.vw_AllProjectItems.TicketId AND dbo.vw_ProjectAllocation.TenantID = dbo.vw_AllProjectItems.TenantID INNER JOIN  
                         dbo.Config_Module_RequestType ON dbo.vw_AllProjectItems.RequestTypeLookup = dbo.Config_Module_RequestType.ID  
WHERE        (dbo.vw_AllProjectItems.Closed <> 1)  
AND  dbo.vw_ProjectAllocation.TenantID=@TenantID   
END  
  