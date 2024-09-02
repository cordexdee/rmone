CREATE Procedure [dbo].[usp_GetAspNetUsers]  
-- usp_GetAspNetUsers 'c345e784-aa08-420f-b11f-2753bbebfdd5'  
@TenantId varchar(max)  
as  
Begin  
select a.*,d.Title[DepartmentLookup$],dbo.fnGetusername(a.ManagerIDUser,@TenantId)[ManagerIDUser$],
dbo.fnGetusername(a.CreatedByUser,@TenantId)[CreatedByUser$],
dbo.fnGetusername(a.ModifiedByUser,@TenantId)[ModifiedByUser$],
dbo.fnGetusername(a.DelegateUserOnLeave,@TenantId)[DelegateUserOnLeave$],
dbo.fnGetusername(a.DelegateUserFor,@TenantId)[DelegateUserFor$],
loc.Title[LocationLookup$],
bc.Title[BudgetIdLookup$],
lp.Name[UserRoleIdLookup$],
r.Name[GlobalRoleID$],
fa.Title[FunctionalAreaLookup$],
et.Title[EmployeeType$],
jt.Title[JobTitleLookup$],
[dbo].[fnGetUserSkills](a.UserSkillLookup,@TenantID)[UserSkillLookup$]
from AspNetUsers a left join Department d on isnull(a.DepartmentLookup,0)= cast(d.ID as varchar) and d.TenantID=@TenantId  
left join [Location] (READCOMMITTED) loc on isnull(loc.id,0)= isnull(a.LocationLookup,'') and loc.TenantID=@TenantID
left join LandingPages (READCOMMITTED) lp on isnull(lp.id,0)= isnull(a.UserRoleIdLookup,'') and lp.TenantID=@TenantID
left join Roles (READCOMMITTED) r on isnull(r.id,0)= isnull(a.GlobalRoleID,'') and lp.TenantID=@TenantID
left join FunctionalAreas (READCOMMITTED) fa on isnull(fa.ID,0)= isnull(a.FunctionalAreaLookup ,0) and d.TenantID=@TenantID 
left join EmployeeTypes (READCOMMITTED) et on isnull(et.ID,0)= isnull(a.EmployeeType ,0) and d.TenantID=@TenantID 
left join JobTitle (READCOMMITTED) jt on isnull(jt.ID,0)= isnull(a.JobTitleLookup ,0) and d.TenantID=@TenantID 
left join UserSkills (READCOMMITTED) usk on isnull(usk.ID,0)= isnull(a.UserSkillLookup ,0) and d.TenantID=@TenantID 
left join Config_BudgetCategories (READCOMMITTED) bc on isnull(bc.ID,0)= isnull(a.BudgetIdLookup ,0) and d.TenantID=@TenantID 
where a.TenantID=@TenantId  and a.isRole=0
and a.UserName not in ('SuperAdmin')
End