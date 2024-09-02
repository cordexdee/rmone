CREATE Procedure [dbo].[usp_GetOpenworkitems]
@tenantid varchar(128),  
@ModuleNames varchar(max),  
@Isclosed bit  
AS  
BEGIN  
IF(@Isclosed=0) 
	Begin  	-- Get all allocations excluding those for closed projects
		-- Create temp table
			CREATE TABLE #ClosedTickets  
			(
ID varchar(20)   
			); 
			-- Add index for performance
			CREATE NONCLUSTERED INDEX [IX_ID] ON #ClosedTickets (ID ASC);     

			-- Get list of all closed tickets in temp table
			Insert into #ClosedTickets exec usp_GetClosedTickets @tenantid, @ModuleNames;  

			-- Get all workitems NOT associated with closed tickets
			Select *  from ResourceWorkItems where TenantID=@tenantid  
					  and WorkItem not in (select ID from #ClosedTickets)

Drop table #ClosedTickets;
		END  
ELSE 
	BEGIN
	-- Get ALL WorkItems
	Select * from ResourceWorkItems where TenantID=@tenantid  	
	END  
END

