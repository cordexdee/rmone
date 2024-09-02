CREATE FUNCTION [dbo].[fnGetRequestType]        
(        
 @str NVARCHAR(MAX),  
 @TenantID NVARCHAR(MAX)  
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
 Declare @outputStr nvarchar(max)        
 select @outputStr= coalesce(@outputStr + '; ', '') + con.RequestType   from  string_split(replace(@str,';#',','), ',') as SP         
 join Config_Module_RequestType con on con.ID = SP.value  and con.TenantID=@TenantID    
        
 RETURN @outputStr        
        
END 