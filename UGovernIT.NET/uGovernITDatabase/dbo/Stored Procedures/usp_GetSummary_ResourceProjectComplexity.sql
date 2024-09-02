CREATE procedure [dbo].[usp_GetSummary_ResourceProjectComplexity]
--[usp_GetSummary_ResourceProjectComplexity] '35525396-e5fe-4692-9239-4df9305b915b'
@TenantID varchar(max)
as
Begin
SELECT   a.*, [dbo].[fnGetusername](a.ManagerUser,@TenantID)[ManagerUser$],
 [dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$],Dbo.[fnGetRequestType](a.RequestTypes,@TenantID)[RequestTypes$],
 [dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$],a.ModuleNameLookup[ModuleNameLookup$],d.Title[DepartmentID$],
 f.Title[FunctionalAreaID$]
 from Summary_ResourceProjectComplexity 
 (ReadCommitted) a  
 left join Department d on d.ID= isnull(a.DepartmentID,0) and d.TenantID=@TenantID
 left join FunctionalAreas f on f.ID= isnull(a.FunctionalAreaID,0) and f.TenantID=@TenantID
  where a.TenantID=@TenantID
END
