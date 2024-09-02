Create Procedure usp_getNPRResources
@TenantID varchar(128)
as
begin
Select a.ID
,a._ResourceType
,a.AllocationEndDate
,a.AllocationStartDate
,a.BudgetDescription
,a.BudgetTypeChoice
,a.EstimatedHours
,a.NoOfFTEs
,a.TicketId
, dbo.fnGetusername(a.RequestedResourcesUser,@TenantID)RequestedResourcesUser
,a.Title
, s.Title UserSkillLookup
,a.RoleNameChoice
,a.HourlyRate
,a.ModuleNameLookup
,a.TenantID
,a.Created
,a.Modified
,a.CreatedByUser
,a.ModifiedByUser
,a.Deleted
 from NPRResources a  left join UserSkills s on a.UserSkillLookup=s.ID and s.TenantID=@TenantID where a.TenantID=@TenantID
end
