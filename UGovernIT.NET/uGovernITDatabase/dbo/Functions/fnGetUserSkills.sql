CREATE FUNCTION [dbo].[fnGetUserSkills]        
(        
 @str NVARCHAR(MAX),  
 @TenantID NVARCHAR(MAX)  
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
 Declare @outputStr nvarchar(max)        
 select @outputStr= coalesce(@outputStr + '; ', '') + con.Title   from  string_split(replace(@str,';#',','), ',') as SP         
 join UserSkills con on con.ID = SP.value  and con.TenantID=@TenantID    
        
 RETURN @outputStr        
        
END 
