CREATE FUNCTION [dbo].[fnGetCompanyTitle]        
(        
 @str NVARCHAR(MAX),  
 @TenantID NVARCHAR(MAX)  
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
 Declare @outputStr nvarchar(max)        
 select @outputStr= coalesce(@outputStr + '; ', '') + con.Title from  string_split(@str, ',') as SP         
 join CRMCompany con on con.TicketId = SP.value  and con.TenantID=@TenantID    
        
 RETURN @outputStr        
        
END 