CREATE PROCEDURE usp_PipelineUtilization
-- usp_PipelineUtilization '35525396-e5fe-4692-9239-4df9305b915b','','',''
	@TenantID nvarchar(256),
	@IsClosed char(1),
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	Declare @CPRAwardedStageStep int;
	Declare @CNSAwardedStageStep int;

	select @CPRAwardedStageStep = StageStep from Config_Module_ModuleStages where TenantID = @TenantID
	and ModuleNameLookup = 'CPR'
	and CustomProperties like '%awardstage%'

	select @CNSAwardedStageStep = StageStep from Config_Module_ModuleStages where TenantID = @TenantID
	and ModuleNameLookup = 'CNS'
	and CustomProperties like '%awardstage%'

	select Title, TicketId, TenantID, StageStep, [Status], Closed from CRMProject where tenantID = @TenantID
	and StageStep < @CPRAwardedStageStep
	AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
	UNION
	select Title, TicketId, TenantID, StageStep, [Status], Closed from CRMServices where tenantID = @TenantID
	and StageStep < @CNSAwardedStageStep
	AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
	UNION
	select Title, TicketId, TenantID, StageStep, [Status], Closed from Opportunity where tenantID = @TenantID
	AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END

END