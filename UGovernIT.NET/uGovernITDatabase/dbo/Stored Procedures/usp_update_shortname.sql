  
CREATE procedure usp_update_shortname  
@TenantID varchar(128)  
as  
Begin  
Declare @shortnamelength int=20;-- default is 20  
SET @shortnamelength= (Select KeyValue from Config_ConfigurationVariable where KeyName='ShortNameCharacters' and TenantID=@TenantID)   
Update CRMProject set ShortName=  left(Title,@shortnamelength)  where TenantID=@TenantID  
Update Opportunity set ShortName=  left(Title,@shortnamelength) where TenantID=@TenantID  
Update CRMServices set ShortName=  left(Title,@shortnamelength) where TenantID=@TenantID  
End  