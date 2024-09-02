
/****** Object:  StoredProcedure [dbo].[usp_GetRequestCategoryWiseProjects]    Script Date: 05/05/2021 18:42:32 ******/

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
