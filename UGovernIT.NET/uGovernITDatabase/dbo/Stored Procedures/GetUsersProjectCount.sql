CREATE PROCEDURE GetUsersProjectCount     
 @resource nvarchar(256),    
 @tenantid varchar(max),    
 @includeClosedProjects bit    
AS    
BEGIN    
 -- exec GetUsersProjectCount 'f66c66d3-55d4-47dc-8094-33804dfad0a8', '35525396-e5fe-4692-9239-4df9305b915b', 0    
 IF @includeClosedProjects = 0    
  BEGIN    
   DECLARE @tbTickets TABLE    
   (    
    ticketId nvarchar(25)    
   )    
    
   INSERT INTO @tbTickets    
   EXEC ('usp_GetClosedTickets @tenantid='''+ @tenantid + ''',@ModuleNames=''CPR,OPM,CNS,PMM,NPR''')    
    
   --select * from @tbTickets      
   select count(distinct(ra.TicketID)) from ResourceAllocation ra    
   join AspNetUsers usr on ra.ResourceUser = usr.Id    
   where (usr.Id = @resource or usr.ManagerUser = @resource)    
   and usr.TenantID = @tenantid and ra.TenantID = @tenantid    
   and ra.TicketID is not null    
   --and CONVERT(datetime, ra.AllocationEndDate,121) >= CONVERT(datetime, GETDATE(),121)    
   and ra.TicketID not in    
   (    
    select ticketId from @tbTickets    
   )    
  END    
 ELSE    
  BEGIN    
   select count(distinct(ra.TicketID)) from ResourceAllocation ra    
   join AspNetUsers usr on ra.ResourceUser = usr.Id    
   where (usr.Id = @resource or usr.ManagerUser = @resource)    
   and usr.TenantID = @tenantid and ra.TenantID = @tenantid    
   and ra.TicketID is not null    
   --and CONVERT(datetime, ra.AllocationEndDate,121) >= CONVERT(datetime, GETDATE(),121)    
  END    
END 