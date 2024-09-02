ALTER Procedure usp_GetRoleby_Dept_Job    
@TenantID varchar(128),    
@Dept varchar(200)='',    
@JobTitleLookup int=0    
as    
Begin    

If (Len(@Dept) > 0 and @JobTitleLookup = 0)
BEGIN
	select RoleLookup from RoleBillingRateByDept where DepartmentLookup IN (SELECT value FROM STRING_SPLIT( @Dept , ','))
	and TenantID = @TenantID	
END
If (Len(@Dept)= 0 and @JobTitleLookup >0)
BEGIN
Select a.Name from Roles a where a.TenantID=@TenantID and a.Id in (
	select RoleId from JobTitle where ID=@JobTitleLookup
	and TenantID = @TenantID)
END
ELSE
BEGIN
	Select RoleId as RoleLookup from JobTitle where ID in (  
	Select JobTitleLookup from JobTitleCostRateByDept  
	where TenantID=@TenantID and DeptLookup IN (SELECT value FROM STRING_SPLIT( @Dept , ','))
	and JobTitleLookup=@JobTitleLookup  
	) and TenantID=@TenantID   
END
End  

