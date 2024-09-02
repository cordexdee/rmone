
ALTER View  [dbo].[vw_TicketwithSatus]
as
Select distinct  TicketId,Closed,TenantID, ERPJobID from CRMProject --where TenantID='35525396-e5fe-4692-9239-4df9305b915b'
Union all
Select distinct TicketId,Closed,TenantID, ERPJobID from Opportunity --where TenantID='35525396-e5fe-4692-9239-4df9305b915b'



