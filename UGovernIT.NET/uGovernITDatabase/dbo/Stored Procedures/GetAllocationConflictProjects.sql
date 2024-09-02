CREATE PROCEDURE GetAllocationConflictProjects
@StartDate datetime='2022-09-26 00:00:00.000',  
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B'
AS
BEGIN
		create table #tmpProjects  
		 (  
		  TicketId varchar(50),  
		  Title nvarchar(1000)
		 )  
   
  
		 insert into #tmpProjects  
		 (  
		 TicketId,  
		 Title	
		 )  
		 select TicketId, Title  
		 from CRMProject where TenantID = @TenantID;  
  
  
  
		 insert into #tmpProjects  
		 (  
		 TicketId,  
		 Title 
		 )  
		 select TicketId, Title  
		 from Opportunity where TenantID = @TenantID;  
  
  
		 insert into #tmpProjects  
		 (  
		 TicketId,  
		 Title
		 )  
		 select TicketId, Title  
		 from CRMServices where TenantID = @TenantID;  

		 Declare @MinDt DateTime; 

		select @MinDt = MIN(AllocationStartDate) from ResourceAllocation where TenantID = @TenantID
		and TicketID not like '%-__-%' and AllocationStartDate >= @StartDate

		select ra.ID, ra.TicketID, tmp.Title, ra.RoleId, r.[Name] [Role], ra.AllocationStartDate, ra.AllocationEndDate, ra.PctAllocation, ra.ResourceUser from ResourceAllocation ra
		join #tmpProjects tmp on tmp.TicketId = ra.TicketID  
		join Roles r on r.Id = ra.RoleId
		where ra.TenantID = @TenantID and
		ra.ResourceUser in (
		select ResourceUser from ResourceAllocation where TenantID = @TenantID
		and TicketID not like '%-__-%' and AllocationStartDate >= @StartDate)
		and @MinDt between ra.AllocationStartDate and ra.AllocationEndDate
		and ra.TicketID like '%-__-%' 


		 drop table #tmpProjects; 
END
