 CREATE Procedure [dbo].[usp_GetAllWorkitems] 
--usp_GetAllWorkitems '35525396-e5fe-4692-9239-4df9305b915b' ,'OPM-21-001442,OPM-21-001639,OPM-21-001682,OPM-21-001699,CPR-19-000127,CPR-20-000304,CPR-20-000304,OPM-21-001541,CNS-21-002577,CPR-22-000503,CPR-22-000503,OPM-20-001219,CPR-21-000390,CPR-22-000511,CPR-18-000369'
@tenantid varchar(max),
@tickets varchar(max)
as
begin

DECLARE @tblTickets TABLE
(
	TicketId varchar(50)
)

insert into @tblTickets (TicketId) SELECT * FROM DBO.SPLITSTRING(@tickets, ',');

Select  TicketId, ProjectID, Title, Closed  from CRMProject where TenantID=@tenantid and TicketId in (SELECT * FROM @tblTickets)
union all
Select  TicketId, ProjectID, Title, Closed  from Opportunity where TenantID=@tenantid and TicketId in (SELECT * FROM @tblTickets)
union all
Select  TicketId, ProjectID, Title, Closed  from CRMServices where TenantID=@tenantid and TicketId in (SELECT * FROM @tblTickets)
union all
Select  TicketId, '', Title, Closed  from NPR where TenantID=@tenantid and TicketId in (SELECT * FROM @tblTickets)
union all
Select  TicketId, '', Title, Closed  from PMM where TenantID=@tenantid and TicketId in (SELECT * FROM @tblTickets)

END

