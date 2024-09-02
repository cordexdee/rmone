
/****** Object:  StoredProcedure [dbo].[usp_GetResourceSummaryByDivision]    Script Date: 05/05/2021 18:42:32 ******/

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
