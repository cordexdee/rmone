
Create Function CheckUserIsInGroup(
@TenantID varchar(128),
@userid varchar(128),
@KeyName varchar(128)
)
RETURNS Bit
As begin
Declare @flag bit
Set @flag =(SELECT 
    CASE 
        WHEN EXISTS(
            
Select UserId from (
Select UserId from AspNetUserRoles where  TenantID =@TenantID  and  RoleId= (
Select Id from AspNetRoles where  TenantID =@TenantID and Title = (
Select KeyValue from Config_ConfigurationVariable where TenantID =@TenantID and KeyName=@KeyName)
)) t where t.UserId in (@userid))
        THEN 'True' 

        ELSE 'False'
    END)

		  RETURN @Flag;	  
END