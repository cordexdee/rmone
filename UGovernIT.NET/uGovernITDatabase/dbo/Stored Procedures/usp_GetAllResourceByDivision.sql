
/****** Object:  StoredProcedure [dbo].[usp_GetAllResourceByDivision]    Script Date: 05/05/2021 18:42:32 ******/

Create procedure [dbo].[usp_GetAllResourceByDivision]
@TenantID varchar(max)
as
Begin
SELECT        dbo.AspNetUsers.Id, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Name, dbo.Department.Title, dbo.CompanyDivisions.Title AS Division, dbo.AspNetUsers.TenantID
FROM            dbo.CompanyDivisions INNER JOIN
                         dbo.Department ON dbo.CompanyDivisions.ID = dbo.Department.DivisionIdLookup INNER JOIN
                         dbo.AspNetUsers ON dbo.Department.ID = dbo.AspNetUsers.DepartmentLookup
WHERE        (dbo.AspNetUsers.Enabled = 1) AND (dbo.Department.Deleted = 0) AND (dbo.CompanyDivisions.Deleted = 0)
AND dbo.CompanyDivisions.TenantID=@TenantID
END


GO
