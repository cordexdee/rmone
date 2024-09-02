Create procedure [dbo].[usp_GetResourceSummaryByDivision]
@TenantID varchar(max)
as
Begin
SELECT DISTINCT 
dbo.Summary_ResourceProjectComplexity.UserId, dbo.Summary_ResourceProjectComplexity.UserName, dbo.Summary_ResourceProjectComplexity.DepartmentID, dbo.Department.Title AS Department, 
dbo.Department.DivisionIdLookup, dbo.CompanyDivisions.Title AS Division, dbo.Summary_ResourceProjectComplexity.TenantID
FROM dbo.Department INNER JOIN
dbo.CompanyDivisions ON dbo.Department.DivisionIdLookup = dbo.CompanyDivisions.ID INNER JOIN
dbo.Summary_ResourceProjectComplexity ON dbo.Department.ID = dbo.Summary_ResourceProjectComplexity.DepartmentID
where  dbo.Department.TenantID=@TenantID
END

GO

Create procedure [dbo].[usp_GetDashboardSummary]
@TenantID varchar(max)
as
Begin
SELECT a.*,f.Name[ModuleStepLookup$],h.Title[RequestTypeLookup$],b.Title[FunctionalAreaLookup$],
[dbo].[fnGetusername](a.InitiatorUser)[InitiatorUser$],[dbo].[fnGetusername](a.PRPGroupUser)[PRPGroupUser$],[dbo].[fnGetusername](a.ORPUser)[ORPUser$],
[dbo].[fnGetusername](a.OwnerUser)[OwnerUser$],[dbo].[fnGetusername](a.RequestorUser)[RequestorUser$],[dbo].[fnGetusername] (Replace(a.StageActionUsersUser,';#',','))[StageActionUsersUser$]
,[dbo].[fnGetusername](a.ModifiedByUser)[ModifiedByUser$],[dbo].[fnGetusername](a.CreatedByUser)[CreatedByUser$]
FROM  dbo.DashboardSummary (READCOMMITTED) a
left join FunctionalAreas (READCOMMITTED) b on isnull(b.ID,'')= isnull(a.FunctionalAreaLookup,'') and b.TenantID=@TenantID 
left join Config_Module_ModuleStages (READCOMMITTED) f on isnull(f.ID,'')= isnull(a.ModuleStepLookup,'') and f.TenantID=@TenantID
left join Config_Module_RequestType (READCOMMITTED) h on isnull(h.ID,'')= isnull(a.RequestTypeLookup,'') and h.TenantID=@TenantID 
where a.TenantID=@TenantID 
END

GO
Create procedure [dbo].[usp_GetRequestCategoryWiseProjects] 
--[usp_GetRequestCategoryWiseProjects] '35525396-e5fe-4692-9239-4df9305b915b'
@TenantID varchar(max)
as
Begin
SELECT        dbo.CRMProject.ID, dbo.CRMProject.RequestTypeLookup,dbo.Config_Module_RequestType.RequestType[RequestTypeLookup$], dbo.CRMProject.Status, 
Isnull(dbo.CRMProject.Closed,0)Closed, dbo.CRMProject.ApproxContractValue, dbo.Config_Module_RequestType.Category, 
                         dbo.CRMProject.TenantID
FROM            dbo.CRMProject INNER JOIN
                         dbo.Config_Module_RequestType ON dbo.CRMProject.RequestTypeLookup = dbo.Config_Module_RequestType.ID

where dbo.CRMProject.TenantID=@TenantID 
END

GO
Create procedure [dbo].[usp_GetResourceByBusinessSector]
@TenantID varchar(max)
as
Begin
SELECT DISTINCT dbo.vw_ProjectAllocation.AssignedToUser, dbo.vw_ProjectAllocation.TenantID, dbo.Config_Module_RequestType.Category, ISNULL(dbo.vw_AllProjectItems.Closed,0)Closed
FROM            dbo.vw_ProjectAllocation INNER JOIN
                         dbo.vw_AllProjectItems ON dbo.vw_ProjectAllocation.TicketID = dbo.vw_AllProjectItems.TicketId AND dbo.vw_ProjectAllocation.TenantID = dbo.vw_AllProjectItems.TenantID INNER JOIN
                         dbo.Config_Module_RequestType ON dbo.vw_AllProjectItems.RequestTypeLookup = dbo.Config_Module_RequestType.ID
WHERE        (dbo.vw_AllProjectItems.Closed <> 1)
AND  dbo.vw_ProjectAllocation.TenantID=@TenantID 
END

GO
Create procedure [dbo].[usp_GetAllResourceByDivision]
@TenantID varchar(max)
as
Begin
SELECT        dbo.AspNetUsers.Id, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Name, dbo.Department.Title, dbo.CompanyDivisions.Title AS Division, dbo.AspNetUsers.TenantID
FROM            dbo.CompanyDivisions INNER JOIN
                         dbo.Department ON dbo.CompanyDivisions.ID = dbo.Department.DivisionIdLookup INNER JOIN
                         dbo.AspNetUsers ON dbo.Department.ID = dbo.AspNetUsers.DepartmentLookup
WHERE        (dbo.AspNetUsers.Enabled = 1) AND (dbo.Department.IsDeleted = 0) AND (dbo.CompanyDivisions.IsDeleted = 0)
AND dbo.CompanyDivisions.TenantID=@TenantID
END


GO

 
Create procedure [dbo].[usp_GetSummary_ResourceProjectComplexity]
@TenantID varchar(max)
as
Begin
SELECT   * from Summary_ResourceProjectComplexity where TenantID=@TenantID
END




GO

/*
Update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'vw_RequestCategoryWiseProjects','RequestCategoryWiseProjects')  where 
DashboardPanelInfo like '%vw_RequestCategoryWiseProjects%'

Update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'vw_AllResourceByDivision','AllResourceByDivision')  where 
DashboardPanelInfo like '%vw_AllResourceByDivision%'

Update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'vw_ResourceSummaryByDivision','ResourceSummaryByDivision')  where 
DashboardPanelInfo like '%vw_ResourceSummaryByDivision%'

Update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'vw_ResourceByBusinessSector','ResourceByBusinessSector')  where 
DashboardPanelInfo like '%vw_ResourceByBusinessSector%'


update Config_Dashboard_DashboardFactTables set Title='ResourceByBusinessSector' where TenantID='35525396-e5fe-4692-9239-4df9305b915b' and Title='vw_ResourceByBusinessSector'
update Config_Dashboard_DashboardFactTables set Title='RequestCategoryWiseProjects' where TenantID='35525396-e5fe-4692-9239-4df9305b915b' and Title='vw_RequestCategoryWiseProjects'
update Config_Dashboard_DashboardFactTables set Title='ResourceSummaryByDivision' where TenantID='35525396-e5fe-4692-9239-4df9305b915b' and Title='vw_ResourceSummaryByDivision'
update Config_Dashboard_DashboardFactTables set Title='AllResourceByDivision' where TenantID='35525396-e5fe-4692-9239-4df9305b915b' and Title='vw_AllResourceByDivision'

*/
