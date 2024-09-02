    
CREATE FUNCTION [dbo].[fnGetusername]        
(            
 @str NVARCHAR(MAX),      
 @TenantID NVARCHAR(128)      
)            
RETURNS NVARCHAR(MAX)            
AS            
BEGIN            
DECLARE @tbluser TABLE  
(  
Valuess varchar(MAX)  
)   
insert into @tbluser (Valuess) SELECT * FROM  DBO.SPLITSTRING(replace(@str,';#',','), ',')  
 --Declare @outputStr nvarchar(max)            
 --select @outputStr= coalesce(@outputStr + ',', '') + con.Name   from dbo.split_string(replace(@str,';#',','), ',') as SP             
 --join AspNetUsers con on con.ID = SP.tuple            
 Declare @outputStr nvarchar(max)            
 select @outputStr= coalesce(@outputStr + '; ', '') + con.Name   from  @tbluser SP              
 join AspNetUsers con on con.ID = SP.Valuess  and con.TenantID=@TenantID        
        -- if GUID belongs to roles table then it will show role name    
 IF(Isnull(@outputStr,0)='0')    
 Begin    
 select @outputStr= coalesce(@outputStr + '; ', '') + con.Name   from  @tbluser SP               
 join AspNetRoles con on con.ID = SP.Valuess  and con.TenantID=@TenantID     
 End    
 RETURN @outputStr     
             
END    
    
    
    