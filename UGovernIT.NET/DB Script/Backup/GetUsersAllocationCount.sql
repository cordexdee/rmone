
ALTER PROCEDURE [dbo].[GetUsersAllocationCount]   
 @resource nvarchar(256),  
 @tenantid varchar(max),  
 @startdate datetime,  
 @enddate datetime,  
 @includeClosedProjects bit  
AS  
BEGIN  
 -- exec GetUsersAllocationCount 'b8a1abfc-ee78-4031-95df-1ca65f288815', '35525396-e5fe-4692-9239-4df9305b915b', 0  
 IF @includeClosedProjects = 0  
  BEGIN  
   DECLARE @tbTickets TABLE  
   (  
    ticketId nvarchar(25)  
   )  
  
   INSERT INTO @tbTickets  
   EXEC ('usp_GetClosedTickets @tenantid='''+ @tenantid + ''',@ModuleNames=''CPR,OPM,CNS,PMM,NPR''')  
  
   --select * from @tbTickets    
   --select count(ra.TicketID) from ProjectEstimatedAllocation ra  
   select count(ra.TicketID) from ResourceAllocation ra  
   join AspNetUsers usr on ra.ResourceUser = usr.Id  
   where (usr.Id = @resource or usr.ManagerUser = @resource)  
   and usr.TenantID = @tenantid and ra.TenantID = @tenantid  
   and ra.TicketID is not null  
   and ((ra.AllocationStartDate > @startdate and ra.AllocationStartDate <= @enddate) or (ra.AllocationEndDate > @startdate and ra.AllocationEndDate <= @enddate))
   and ra.TicketID not in  
   (  
    select ticketId from @tbTickets  
   )  
   and (ra.TicketID like 'CPR-%' or ra.TicketID like 'OPM-%' or ra.TicketID like 'CNS-%' or ra.TicketID like 'PMM-%' or ra.TicketID like 'NPR-%')  
  END  
 ELSE  
  BEGIN  
   select count(ra.TicketID) from ResourceAllocation ra  
   join AspNetUsers usr on ra.ResourceUser = usr.Id  
   where (usr.Id = @resource or usr.ManagerUser = @resource)  
   and usr.TenantID = @tenantid and ra.TenantID = @tenantid  
   and ra.TicketID is not null  
   and (ra.TicketID like 'CPR-%' or ra.TicketID like 'OPM-%' or ra.TicketID like 'CNS-%' or ra.TicketID like 'PMM-%' or ra.TicketID like 'NPR-%')  
   --and ra.AllocationStartDate > @startdate and ra.AllocationStartDate <= @enddate  
   and ((ra.AllocationStartDate > @startdate and ra.AllocationStartDate <= @enddate) or (ra.AllocationEndDate > @startdate and ra.AllocationEndDate <= @enddate))
  END  
END  

