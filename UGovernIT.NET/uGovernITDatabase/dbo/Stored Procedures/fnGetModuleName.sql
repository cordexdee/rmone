 
CREATE FUNCTION [dbo].[fnGetModuleName]  
(  
 @str NVARCHAR(MAX)  
)  
RETURNS NVARCHAR(MAX)  
AS  
BEGIN   
 Declare @outputStr nvarchar(max)  
 select @outputStr= coalesce(@outputStr + ',', '') + con.ModuleName   from dbo.split_string(@str, ',') as SP   
 join config_modules con on con.ID = SP.tuple  
 --where con.TenantID='35525396-E5FE-4692-9239-4DF9305B915B'  
 RETURN @outputStr  
END  


