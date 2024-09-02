CREATE procedure [dbo].[usp_GetDashboardSummary]          
--[usp_GetDashboardSummary] '35525396-e5fe-4692-9239-4df9305b915b'         
@TenantID varchar(128)          
as          
Begin          
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''  
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'DashboardSummary')  
SET @strsql='Select  '+ @AllColumnsList+' from DashboardSummary (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''  
and (a.Deleted<>1 or a.Deleted is null)'  
EXEC(@strsql)  
--Print(@strsql)          
END    
  