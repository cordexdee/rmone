
CREATE PROCEDURE [dbo].[usp_GetUserAllProjectDetails]
(
@TenantId nvarchar(128),
@UserId nvarchar(128)
)
AS

BEGIN
select 	[dbo].fnGetRelatedCompanyInfo(c.TicketId,@TenantId, 'title') as ExternalTeam, 
		[dbo].fnGetRelatedCompanyInfo(c.TicketId,@TenantId, 'type') as ExternalTeamType, 
		[dbo].fnGetResourceAllocationCount(c.TicketId,@TenantId) as ResourceAllocationCount,
		dbo.fnGetCompanyTitle(c.CRMCompanyLookup,@TenantID) as ClientName,
		CASE WHEN r.Category IS NOT NULL THEN r.Category+'>' ELSE '' END 
		+ CASE WHEN r.SubCategory IS NOT NULL THEN r.SubCategory+'>' ELSE '' END 
		+ CASE WHEN r.RequestType IS NOT NULL THEN r.RequestType ELSE '' END RequestTypeTitle,
		r.RequestType,
		c.DueDate,
		c.ChanceOfSuccessChoice,
		c.TicketId,
		c.CRMCompanyLookup,
		dbo.fnGetCompanyType(c.CRMCompanyLookup,@TenantID) as RelationshipType,
		c.Title,
		c.ShortName,
		c.ERPJobID as ERPJobID,
		c.PreconStartDate,
		c.EstimatedConstructionEnd,
		c.ApproxContractValue,
		'CPR' as ModuleName
from CRMProject c 
LEFT JOIN Config_Module_RequestType r on c.RequestTypeLookup=r.ID
where c.LeadEstimatorUser = @UserId and c.Status <> 'Closed' and c.Closed<>1

union

select  [dbo].fnGetRelatedCompanyInfo(o.TicketId,@TenantId, 'title') as ExternalTeam, 
		[dbo].fnGetRelatedCompanyInfo(o.TicketId,@TenantId, 'type') as ExternalTeamType, 
		[dbo].fnGetResourceAllocationCount(o.TicketId,@TenantId) as ResourceAllocationCount,
		dbo.fnGetCompanyTitle(o.CRMCompanyLookup,@TenantID) as ClientName,
		CASE WHEN r.Category IS NOT NULL THEN r.Category+'>' ELSE '' END 
		+ CASE WHEN r.SubCategory IS NOT NULL THEN r.SubCategory+'>' ELSE '' END 
		+ CASE WHEN r.RequestType IS NOT NULL THEN r.RequestType ELSE '' END RequestTypeTitle,
		r.RequestType,
		o.DueDate,
		o.ChanceOfSuccessChoice,
		o.TicketId,
		o.CRMCompanyLookup,
		dbo.fnGetCompanyType(o.CRMCompanyLookup,@TenantID) as RelationshipType,
		o.Title,
		o.ShortName,
		o.ERPJobIDNC as ERPJobID,
		o.PreconStartDate,
		o.EstimatedConstructionEnd,
		o.ApproxContractValue ,
		'OPM' as ModuleName
from Opportunity o 
LEFT JOIN Config_Module_RequestType r on o.RequestTypeLookup=r.ID
where o.LeadEstimatorUser = @UserId and o.Status <> 'Closed' and o.Closed<>1
union
select  [dbo].fnGetRelatedCompanyInfo(o.TicketId,@TenantId, 'title') as ExternalTeam, 
		[dbo].fnGetRelatedCompanyInfo(o.TicketId,@TenantId, 'type') as ExternalTeamType, 
		[dbo].fnGetResourceAllocationCount(o.TicketId,@TenantId) as ResourceAllocationCount,
		dbo.fnGetCompanyTitle(o.CRMCompanyLookup,@TenantID) as ClientName,
		CASE WHEN r.Category IS NOT NULL THEN r.Category+'>' ELSE '' END 
		+ CASE WHEN r.SubCategory IS NOT NULL THEN r.SubCategory+'>' ELSE '' END 
		+ CASE WHEN r.RequestType IS NOT NULL THEN r.RequestType ELSE '' END RequestTypeTitle,
		r.RequestType,
		o.DueDate,
		'' ChanceOfSuccessChoice,
		o.TicketId,
		o.CRMCompanyLookup,
		dbo.fnGetCompanyType(o.CRMCompanyLookup,@TenantID) as RelationshipType,
		o.Title,
		o.ShortName,
		o.ERPJobIDNC as ERPJobID,
		o.PreconStartDate,
		o.EstimatedConstructionEnd,
		o.ApproxContractValue ,
		'CNS' as ModuleName
from CRMServices o 
LEFT JOIN Config_Module_RequestType r on o.RequestTypeLookup=r.ID
where o.LeadEstimatorUser = @UserId and o.Status <> 'Closed' and o.Closed<>1	
END
