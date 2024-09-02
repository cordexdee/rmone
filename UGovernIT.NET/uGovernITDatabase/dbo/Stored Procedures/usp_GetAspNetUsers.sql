ALTER Procedure [dbo].[usp_GetAspNetUsers]      
-- usp_GetAspNetUsers '35525396-E5FE-4692-9239-4DF9305B915B'  
@TenantId varchar(max), @EnableDivision bit = false  
as      
Begin      
 IF (@EnableDivision = 1)  
 BEGIN  
  select a.*, a.UserName[LoginName], cd.Title + ' > ' + d.Title[DepartmentLookup$]  
  ,dbo.fnGetusername(a.ManagerUser,@TenantId)[ManagerUser$],    
  dbo.fnGetusername(a.CreatedByUser,@TenantId)[CreatedByUser$],    
  dbo.fnGetusername(a.ModifiedByUser,@TenantId)[ModifiedByUser$],    
  dbo.fnGetusername(a.DelegateUserOnLeave,@TenantId)[DelegateUserOnLeave$],    
  dbo.fnGetusername(a.DelegateUserFor,@TenantId)[DelegateUserFor$],    
  loc.Title[LocationLookup$],    
  bc.Title[BudgetIdLookup$],    
  lp.Name[UserRoleIdLookup$],    
  r.Name[GlobalRoleId$],    
  fa.Title[FunctionalAreaLookup$],    
  et.Title[EmployeeType$],    
  jt.Title[JobTitleLookup$],    
  [dbo].[fnGetUserSkills](a.UserSkillLookup,@TenantID)[UserSkillLookup$]    
  from AspNetUsers a left join Department d on isnull(a.DepartmentLookup,0)= cast(d.ID as varchar) and d.TenantID=@TenantId      
  left join [Location] (READCOMMITTED) loc on isnull(loc.id,0)= isnull(a.LocationLookup,'') and loc.TenantID=@TenantID    
  left join LandingPages (READCOMMITTED) lp on isnull(lp.id,0)= isnull(a.UserRoleIdLookup,'') and lp.TenantID=@TenantID    
  left join Roles (READCOMMITTED) r on isnull(r.id,0)= isnull(a.GlobalRoleID,'') and r.TenantID=@TenantID    
  left join FunctionalAreas (READCOMMITTED) fa on isnull(fa.ID,0)= isnull(a.FunctionalAreaLookup ,0) and fa.TenantID=@TenantID     
  left join EmployeeTypes (READCOMMITTED) et on isnull(et.ID,0)= isnull(a.EmployeeType ,0) and et.TenantID=@TenantID     
  left join JobTitle (READCOMMITTED) jt on isnull(jt.ID,0)= isnull(a.JobTitleLookup ,0) and jt.TenantID=@TenantID     
  left join UserSkills (READCOMMITTED) usk on isnull(usk.ID,0)= isnull(a.UserSkillLookup ,0) and usk.TenantID=@TenantID     
  left join Config_BudgetCategories (READCOMMITTED) bc on isnull(bc.ID,0)= isnull(a.BudgetIdLookup ,0) and bc.TenantID=@TenantID     
  left join companydivisions (READCOMMITTED) cd on isnull(cd.ID,0)= isnull(d.DivisionIdLookup ,0) and cd.TenantID=@TenantID     
  where a.TenantID=@TenantId  and a.isRole=0    
  and a.UserName not in ('SuperAdmin')    
 END  
 ELSE  
 BEGIN  
  select a.*,a.UserName[LoginName], d.Title[DepartmentLookup$]  
  ,dbo.fnGetusername(a.ManagerUser,@TenantId)[ManagerUser$],    
  dbo.fnGetusername(a.CreatedByUser,@TenantId)[CreatedByUser$],    
  dbo.fnGetusername(a.ModifiedByUser,@TenantId)[ModifiedByUser$],    
  dbo.fnGetusername(a.DelegateUserOnLeave,@TenantId)[DelegateUserOnLeave$],    
  dbo.fnGetusername(a.DelegateUserFor,@TenantId)[DelegateUserFor$],    
  loc.Title[LocationLookup$],    
  bc.Title[BudgetIdLookup$],    
  lp.Name[UserRoleIdLookup$],    
  r.Name[GlobalRoleId$],    
  fa.Title[FunctionalAreaLookup$],    
  et.Title[EmployeeType$],    
  jt.Title[JobTitleLookup$],    
  [dbo].[fnGetUserSkills](a.UserSkillLookup,@TenantID)[UserSkillLookup$]    
  from AspNetUsers a left join Department d on isnull(a.DepartmentLookup,0)= cast(d.ID as varchar) and d.TenantID=@TenantId      
  left join [Location] (READCOMMITTED) loc on isnull(loc.id,0)= isnull(a.LocationLookup,'') and loc.TenantID=@TenantID    
  left join LandingPages (READCOMMITTED) lp on isnull(lp.id,0)= isnull(a.UserRoleIdLookup,'') and lp.TenantID=@TenantID    
  left join Roles (READCOMMITTED) r on isnull(r.id,0)= isnull(a.GlobalRoleID,'') and r.TenantID=@TenantID    
  left join FunctionalAreas (READCOMMITTED) fa on isnull(fa.ID,0)= isnull(a.FunctionalAreaLookup ,0) and fa.TenantID=@TenantID     
  left join EmployeeTypes (READCOMMITTED) et on isnull(et.ID,0)= isnull(a.EmployeeType ,0) and et.TenantID=@TenantID     
  left join JobTitle (READCOMMITTED) jt on isnull(jt.ID,0)= isnull(a.JobTitleLookup ,0) and jt.TenantID=@TenantID     
  left join UserSkills (READCOMMITTED) usk on isnull(usk.ID,0)= isnull(a.UserSkillLookup ,0) and usk.TenantID=@TenantID     
  left join Config_BudgetCategories (READCOMMITTED) bc on isnull(bc.ID,0)= isnull(a.BudgetIdLookup ,0) and bc.TenantID=@TenantID     
  where a.TenantID=@TenantId  and a.isRole=0    
  and a.UserName not in ('SuperAdmin')    
 END  
End 