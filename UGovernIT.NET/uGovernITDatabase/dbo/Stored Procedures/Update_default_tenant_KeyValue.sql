CREATE PROCEDURE [dbo].[Update_default_tenant_KeyValue]
(
	@TenantId uniqueidentifier,
	@AccountId nvarchar(100)
)
AS
BEGIN

	update Config_ConfigurationVariable
	set KeyValue =
		(
			select id
			from Config_Dashboard_DashboardPanelView
			where viewName = 'ticketsummary'
				  and TenantID = @TenantId
		)
	where TenantID = @TenantId
		  and KeyName in ( 'OpenTicketsChart', 'ClosedTicketsChart' );
		  
   declare @Id varchar(20)
	select @Id = ID from  Config_Dashboard_DashboardPanels where Title = 'Project Summary' and TenantID = @TenantId
	update Config_PageConfiguration set ControlInfo = REPLACE(ControlInfo, '<PanelId>0</PanelId>', '<PanelId>'+ @Id +'</PanelId>')
	where TenantID = @TenantId and Title='HomePMM'

	select @Id = ID from  Config_Dashboard_DashboardPanels where Title = 'My Tickets' and TenantID = @TenantId
	update Config_PageConfiguration set ControlInfo = REPLACE(ControlInfo, '<PanelId>0</PanelId>', '<PanelId>'+ @Id +'</PanelId>')
	where TenantID = @TenantId and Title='UserHomePage'


	update Config_ConfigurationVariable
		set KeyValue =
			(
				select id
				from Config_Dashboard_DashboardPanelView
				where viewName = 'ProjectSummary'
					  and TenantID = @TenantId
			)
		where TenantID = @TenantId
			  and KeyName in ( 'OpenProjectsChart', 'ClosedProjectsChart' );

	-- Set Owner for RequestType
	declare @AdminId nvarchar(256);
	declare @FunctionalAreaId bigint;
	select @AdminId = Id from AspNetUsers where TenantID = @TenantId and UserName = 'Administrator_' + @AccountId;

	update Config_Module_RequestType set OwnerUser = @AdminId where OwnerUser = '91d3d9f8-4bbc-490d-97bc-4964e6fafeae'
	and TenantID = @TenantId;

	update Config_Module_RequestType set OwnerUser = (select Id from AspNetUsers where TenantID = @TenantId and UserName = OwnerUser)
	where TenantID = @TenantId and LEN(OwnerUser) > 0;

	update Config_Module_RequestType set PRPGroupUser = (select Id from AspNetUsers where TenantID = @TenantId and UserName = PRPGroupUser)
	where TenantID = @TenantId and LEN(PRPGroupUser) > 0;

	select @FunctionalAreaId = Id from FunctionalAreas where TenantID = @TenantId and Title = 'Business Systems Support';
	update Config_Module_RequestType set FunctionalAreaLookup = @FunctionalAreaId where ModuleNameLookup in ('ACR','APP','CMT','DRQ','INC')
	and TenantID = @TenantId;

	select @FunctionalAreaId = Id from FunctionalAreas where TenantID = @TenantId and Title = 'PMO';
	update Config_Module_RequestType set FunctionalAreaLookup = @FunctionalAreaId where ModuleNameLookup in ('NPR','PMM')
	and TenantID = @TenantId;

	select @FunctionalAreaId = Id from FunctionalAreas where TenantID = @TenantId and Title = 'User Support';
	update Config_Module_RequestType set FunctionalAreaLookup = @FunctionalAreaId where ModuleNameLookup in ('CMDB')
	and TenantID = @TenantId;

	update Config_Module_RequestType set FunctionalAreaLookup = @FunctionalAreaId where ModuleNameLookup in ('TSR') and Category = 'User Devices'
	and TenantID = @TenantId;

	select @FunctionalAreaId = Id from FunctionalAreas where TenantID = @TenantId and Title = 'Production Services';
	update Config_Module_RequestType set FunctionalAreaLookup = @FunctionalAreaId where ModuleNameLookup in ('TSR') and Category <> 'User Devices'
	and TenantID = @TenantId;


	-- Configure Menu Navigation
	update Config_MenuNavigation
	set CustomProperties = '<?xml version="1.0" encoding="utf-16"?><MenuNavigationProperties xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><MenuFontSize /><MenuFontFontFamily>Default</MenuFontFontFamily><MenuIconAlignment /></MenuNavigationProperties>',
	MenuHeight = 0, MenuWidth = 0, MenuFontColor = 'White'
	where TenantID = @TenantId;

	-- Update Help Card CreatedBy Users
	update HelpCardContent set CreatedByUser = @AdminId, ModifiedByUser = @AdminId
	where TenantID = @TenantId;

	update helpcard set CreatedByUser = @AdminId, ModifiedByUser = @AdminId
	where TenantID = @TenantId;

END
