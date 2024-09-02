CREATE Procedure usp_GetResourceUsageSummaryWeekWise
@TenantID varchar(max)
as
Begin
select a.*,[dbo].[fnGetusername](a.ResourceUser,@TenantID)[ResourceUser$],
 [dbo].[fnGetusername](a.ResourceNameUser,@TenantID)[ResourceNameUser$],
 [dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$],
 [dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$],
 [dbo].[fnGetusername](a.ManagerLookup,@TenantID)[ManagerLookup$],f.Title[FunctionalAreaTitleLookup$]
 from ResourceUsageSummaryWeekWise (Readcommitted) a 
 left join FunctionalAreas (Readcommitted) f on f.ID= ISNULL(a.FunctionalAreaTitleLookup,0) and f.TenantID=@TenantID
 where a.TenantID=@TenantID 
End