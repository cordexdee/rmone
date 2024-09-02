
ALTER FUNCTION [dbo].[fnGetDivisonStudioTicketWise]      
(	
@TenantID varchar(128) ,
@filter nvarchar(max),
@studio bigint = 0,
@division bigint = 0,
@sector nvarchar(max) = '' 
)      
RETURNS @Result TABLE
(
	ProjectId nvarchar(120), EstimatedConstructionStart datetime, EstimatedConstructionEnd datetime, ProjectManagerUser nvarchar(max), SuperintendentUser nvarchar(max), 
	CRMCompanyLookup nvarchar(500), ApproxContractValue float, DivisionLookup bigint, SectorChoice nvarchar(max), 
	StudioLookup bigint, TicketId nvarchar(20), StageStep int, Title nvarchar(max), CRMProjectComplexityChoice nvarchar(max), OpportunityTypeChoice nvarchar(max), 
	TenantID nvarchar(128)
)
AS 
begin
	declare @rewardedStage int;
	set @rewardedStage = 0;
	select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	IF @filter = 'Contracted'
	begin
		Insert into @Result select * from(
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMProject 
		where TenantID=@TenantID and StageStep > @rewardedStage - 1 and  Closed != 1  
		and ( SectorChoice = (case when @sector IS NULL OR LEN(@sector) = 0 then SectorChoice else @sector end)  )
		and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
		and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
		)temp
	end;
	Else IF @filter = 'Pipeline'
	begin
		Insert into @Result select * from(
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMProject 
		where TenantID=@TenantID and  Closed != 1 and StageStep <= @rewardedStage  
		and ( SectorChoice = (case when @sector IS NULL OR LEN(@sector) = 0 then SectorChoice else @sector end)  )
		and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
		and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
		union all
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMServices 
		where TenantID=@TenantID and  Closed != 1  
		and ( SectorChoice = (case when @sector IS NULL OR LEN(@sector) = 0 then SectorChoice else @sector end)  ) 
		and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
		and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
		union all
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from Opportunity 
		where TenantID=@TenantID and  Closed != 1  
		and ( SectorChoice = (case when @sector IS NULL OR LEN(@sector) = 0 then SectorChoice else @sector end)  ) 
		and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
		and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
		)temp
	end;
	Else IF @filter = 'Closed'
	begin
		Insert into @Result select * from(
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMProject where TenantID=@TenantID and  Closed = 1 and
		( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
		and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
		and ( SectorChoice = (case when len(@sector) > 0 then @sector else SectorChoice end)  )
		)temp
	end;
	Else 
	Begin
		Insert into @Result select * from(
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMProject where TenantID=@TenantID 
		union 
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from CRMServices where TenantID=@TenantID 
		union all
		Select ProjectId, EstimatedConstructionStart, EstimatedConstructionEnd, ProjectManagerUser, SuperintendentUser, CRMCompanyLookup, ApproxContractValue, DivisionLookup,SectorChoice,StudioLookup, TicketId, StageStep, Title, ISNULL(CRMProjectComplexityChoice,1) as CRMProjectComplexityChoice, OpportunityTypeChoice, TenantID from Opportunity where TenantID=@TenantID
		)temp
	End;
	Return
end;
