
ALTER FUNCTION [dbo].[GetPipelineProjects] 
(
-- select * from dbo.GetPipelineProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','Corporate Interiors','1-21 Silicon Valley','')
-- select * from dbo.GetPipelineProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','','1-21 Silicon Valley','')
-- select * from dbo.GetPipelineProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','','','')
-- select * from dbo.GetPipelineProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','',12,0)
	@TenantID nvarchar(256),
	@IsClosed char(1),
	@StartDate datetime,
	@EndDate datetime,
	@Sector nvarchar(250),
	@Division bigint,
	@Studio bigint
)
RETURNS 
@Projects TABLE 
(
	Title nvarchar(255), 
	TicketId nvarchar(250), 
	TenantID nvarchar(128), 
	StageStep int, 
	[Status] nvarchar(255), 
	Closed bit,
	SectorChoice nvarchar(250),
	Division bigint,
	Studio bigint,
	ApproxContractValue float,
	PreconStartDt datetime,
	ConstructionEndDt datetime
)
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

	If LEN(@Sector) > 0
		Begin
			Insert into @Projects
			select Title, TicketId, TenantID, StageStep, [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue, PreconStartDt = ISNULL(PreconStartDate, EstimatedConstructionStart), ConstructionEndDt = ISNULL(EstimatedConstructionEnd, PreconEndDate) from CRMProject where tenantID = @TenantID
			and StageStep < @CPRAwardedStageStep
			AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then ISNULL(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then ISNULL(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue, PreconStartDt = ISNULL(PreconStartDate, EstimatedConstructionStart), ConstructionEndDt = ISNULL(EstimatedConstructionEnd, PreconEndDate) from Opportunity where tenantID = @TenantID
			AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then ISNULL(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then ISNULL(StudioLookup,0)  else @Studio END  
		end
	Else
		Begin
			Insert into @Projects
			select Title, TicketId, TenantID, StageStep, [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue, PreconStartDt = ISNULL(PreconStartDate, EstimatedConstructionStart), ConstructionEndDt = ISNULL(EstimatedConstructionEnd, PreconEndDate) from CRMProject where tenantID = @TenantID
			and StageStep < @CPRAwardedStageStep
			AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then ISNULL(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then ISNULL(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, [Status], Closed, Null as SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue, PreconStartDt = ISNULL(PreconStartDate, EstimatedConstructionStart), ConstructionEndDt = EstimatedConstructionEnd from CRMServices where tenantID = @TenantID
			and StageStep < @CNSAwardedStageStep
			AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then ISNULL(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then ISNULL(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue, PreconStartDt = ISNULL(PreconStartDate, EstimatedConstructionStart), ConstructionEndDt = ISNULL(EstimatedConstructionEnd, PreconEndDate) from Opportunity where tenantID = @TenantID
			AND ISNULL(Cast(closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(closed as char(1)),'0') else @IsClosed END
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then ISNULL(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then ISNULL(StudioLookup,0)  else @Studio END 
		End
		
	RETURN; 
END
