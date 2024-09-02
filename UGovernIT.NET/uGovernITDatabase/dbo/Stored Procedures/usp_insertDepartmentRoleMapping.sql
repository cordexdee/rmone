 ALTER Procedure usp_insertDepartmentRoleMapping        
 @RoleId varchar(128),@DepartmentId int=0,     
 @BillingRate float=0,@TenantID varchar(128) ,      
 @Deleted bit ,  
 @id int=0  
 As        
 Begin        
    
 If(@id=0)        
 Begin        
 INSERT INTO [dbo].[RoleBillingRateByDept]          
 ([DepartmentLookup],[RoleLookup],[BillingRate],[TenantID],[Deleted])          
 values (@DepartmentId,@RoleId,@BillingRate,@TenantID,@Deleted)          
End        
Else Begin        
Update [RoleBillingRateByDept] set DepartmentLookup= @DepartmentId,          
RoleLookup= @RoleId,BillingRate= @BillingRate, Deleted=@Deleted where TenantID=@TenantID          
and Id=@id;          
End        
END        
        
         