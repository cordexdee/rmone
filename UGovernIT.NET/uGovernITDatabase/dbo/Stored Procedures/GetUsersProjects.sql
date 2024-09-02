CREATE PROCEDURE [dbo].[GetUsersProjects] 
	@resource nvarchar(256),
	@tenantid varchar(max),
	@includeClosedProjects bit
AS
BEGIN
	-- exec GetUsersProjects '3ed90305-64bd-437b-aa56-935ebd0481d0', '35525396-e5fe-4692-9239-4df9305b915b', 0
	IF @includeClosedProjects = 0
		BEGIN
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from CRMProject where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
			Union
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from CRMServices where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
			Union
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from Opportunity where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
		END
	ELSE
		BEGIN
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from CRMProject where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
			and (Closed != 0 OR Closed Is not NULL)
			Union
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from CRMServices where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
			and (Closed != 0 OR Closed Is not NULL)
			Union
			select ID, Title, TicketId, CloseDate, Modified, IconBlob, ActualStartDate from Opportunity where TenantID=@TenantId and TicketId in (
			select distinct TicketID from ResourceAllocation pa where pa.TenantID=@TenantId and pa.ResourceUser = @resource)
			and (Closed != 0 OR Closed Is not NULL)
		END
END
