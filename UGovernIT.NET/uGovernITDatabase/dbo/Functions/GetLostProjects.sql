
ALTER FUNCTION [dbo].[GetLostProjects] 
(
-- select * from dbo.GetLostProjects('35525396-e5fe-4692-9239-4df9305b915b','','','Corporate Interiors','1-21 Silicon Valley','')
-- select * from dbo.GetLostProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','1-21 Silicon Valley','')
-- select * from dbo.GetLostProjects('35525396-e5fe-4692-9239-4df9305b915b','','','','','SF Studio 2')
-- select * from dbo.GetLostProjects('35525396-e5fe-4692-9239-4df9305b915b','','','',12,0)
	@TenantID nvarchar(256),
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
	ApproxContractValue float
)
AS
BEGIN
	If LEN(@Sector) > 0
		Begin
			Insert into @Projects
			select Title, TicketId, TenantID, StageStep, CRMProjectStatusChoice as [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue from CRMProject where tenantID = @TenantID
			AND CRMProjectStatusChoice in ('Lost','Cancelled')
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then isnull(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then isnull(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, CRMOpportunityStatusChoice as [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue from Opportunity where tenantID = @TenantID
			AND CRMOpportunityStatusChoice in ('Lost','Cancelled')
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then isnull(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then isnull(StudioLookup,0)  else @Studio END 
		end
	Else
		Begin
			Insert into @Projects
			select Title, TicketId, TenantID, StageStep, CRMProjectStatusChoice as [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue from CRMProject where tenantID = @TenantID
			AND CRMProjectStatusChoice in ('Lost','Cancelled')
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then isnull(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then isnull(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, CRMProjectStatusChoice as [Status], Closed, Null as SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue from CRMServices where tenantID = @TenantID
			AND CRMProjectStatusChoice in ('Lost','Cancelled')
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then isnull(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then isnull(StudioLookup,0)  else @Studio END 
			UNION
			select Title, TicketId, TenantID, StageStep, CRMOpportunityStatusChoice as [Status], Closed, SectorChoice, DivisionLookup as Division, StudioLookup as Studio, ApproxContractValue from Opportunity where tenantID = @TenantID
			AND CRMOpportunityStatusChoice in ('Lost','Cancelled')
			AND ISNULL(SectorChoice,'') =CASE WHEN LEN(@Sector)=0 then isnull(SectorChoice,'')  else @Sector END 
			AND ISNULL(DivisionLookup,0) =CASE WHEN @Division=0 then isnull(DivisionLookup,0)  else @Division END 
			AND ISNULL(StudioLookup,0) =CASE WHEN @Studio=0 then isnull(StudioLookup,0)  else @Studio END 
		End
		
	RETURN; 
END
