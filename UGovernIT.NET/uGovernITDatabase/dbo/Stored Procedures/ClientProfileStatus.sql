  
/****** Object:  StoredProcedure [dbo].[ClientProfileStatus]    Script Date: 1/15/2020 7:20:27 PM ******/  
  
  
  
CREATE PROCEDURE [dbo].[ClientProfileStatus]   
(  
    @TenantId nvarchar(128)   
)  
AS  
BEGIN  
      
    DECLARE   
        @RegistrationCount INT = 1,  
        @RegistrationPercentage INT = 100,  
        @UserCount INT = 0,  
        @UserPercentage INT = 100,  
        @ServiceCount INT = 0,  
        @ServicePercentage INT = 0,  
        @ManageIncidentCount INT = 0,  
        @ManageIncidentPercentage INT = 0,  
        @UserCreatedIncidentCount INT = 0,  
        @UserCreatedIncidentPercentage INT = 0,
		@DepartmentCount INT = 0,  
		@DepartmentPercentage INT = 0,  
		@RoleCount INT = 0,  
		@RolePercentage INT = 0,  
		@TitleCount INT = 0,  
		@TitlePercentage INT = 0, 
		@ProjectCount INT = 0,  
		@ProjectPercentage INT = 0, 
		@AllocationCount INT = 0,
		@AllocationPercentage INT = 0
      
    -- Manage Incidents --   
  
  
    SELECT   
        @ManageIncidentCount = COUNT(*)   
    FROM TSR   
    WHERE TenantID = @TenantId;  
  
  
    SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM ACR   
    WHERE TenantID = @TenantId;  
  
  
    SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM DRQ   
    WHERE TenantID = @TenantId;  
  
  
    SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM SVCRequests   
    WHERE TenantID = @TenantId;  

	SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM AspNetUserRoles   
    WHERE TenantID = @TenantId;  

	SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM Department   
    WHERE TenantID = @TenantId;  

	SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM CRMProject   
    WHERE TenantID = @TenantId;  

	SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM ProjectEstimatedAllocation   
    WHERE TenantID = @TenantId;  

	SELECT   
        @ManageIncidentCount += COUNT(*)   
    FROM JobTitle   
    WHERE TenantID = @TenantId;  
  
  
  
    IF(@ManageIncidentCount > 0)  
    BEGIN  
        SET @ManageIncidentPercentage = 100  
    END  
  
  
    -- End Manage Incidents --   
  
  
  
    -- Added Users --  
  
  
    SELECT   
        @UserCount = COUNT(*)   
    FROM AspNetUsers   
    WHERE TenantID = @TenantId AND isRole = 0;  
  
  
    --IF (@UserCount = 1 or @UserCount = 0)  
	IF (@UserCount <= 2)  
    BEGIN  
        -- 1 user count means tenant admin user so ignore tenant admin user  
        -- 2 Super Admin
        SET @UserCount = 0;  
        SET    @UserPercentage = 0;  
    END  
    ELSE  
    BEGIN  
        SET @UserCount = @UserCount - 1;  
        SET    @UserPercentage = 100;  
    END  
    -- End Added Users --  
  
  
    --  User Created Incidents --   
  
  
    DECLARE @AdminUsers TABLE (  
        UserId nvarchar(128)  
    );  
  
  
    INSERT INTO @AdminUsers (UserId)  
    SELECT   
        u.Id as UserId  
    FROM AspNetUsers u  
        JOIN AspNetUserRoles ur ON ur.UserId = u.Id AND ur.TenantID = u.TenantID  
        JOIN AspNetRoles r ON r.Id = ur.RoleId AND ur.TenantID = r.TenantID  
    WHERE r.Name = 'Admin'  
    AND u.TenantID = @TenantId;  
  
  
  
    SELECT   
        @UserCreatedIncidentCount = COUNT(*)   
    FROM TSR   
    WHERE TenantID = @TenantId  
    AND [InitiatorUser] NOT IN (SELECT UserId FROM @AdminUsers);  
  
  
    SELECT   
        @UserCreatedIncidentCount += COUNT(*)   
    FROM ACR   
    WHERE TenantID = @TenantId  
    AND [InitiatorUser] NOT IN (SELECT UserId FROM @AdminUsers);  
  
  
    SELECT   
        @UserCreatedIncidentCount += COUNT(*)   
    FROM DRQ   
    WHERE TenantID = @TenantId  
    AND [InitiatorUser] NOT IN (SELECT UserId FROM @AdminUsers);  
  
  
    SELECT   
        @UserCreatedIncidentCount += COUNT(*)   
    FROM SVCRequests   
    WHERE TenantID = @TenantId  
    AND [InitiatorUser] NOT IN (SELECT UserId FROM @AdminUsers);  


  
  
  
    IF(@UserCreatedIncidentCount > 0)  
    BEGIN  
        SET @UserCreatedIncidentPercentage = 100;  
    END  
  
  
    --  End User Created Incidents --  
  
  
  
    --  Used Services --   
  
  
    SELECT   
        @ServiceCount = COUNT(*)   
    FROM SVCRequests   
    WHERE TenantID = @TenantId;  
  
  
    IF(@ServiceCount > 0)  
    BEGIN  
        SET @ServicePercentage = 100;  
    END  
  
  
    --  End Used Services --  
  
   SELECT   
        @ServiceCount = COUNT(*)   
    FROM SVCRequests   
    WHERE TenantID = @TenantId;  



	 SELECT   
        @DepartmentCount = COUNT(*)   
    FROM Department   
    WHERE TenantID = @TenantId;  
  
  
    IF(@DepartmentCount > 0)  
    BEGIN  
        SET @DepartmentPercentage = 100;  
    END  


	
	 SELECT   
        @RoleCount = COUNT(*)   
    FROM Roles   
    WHERE TenantID = @TenantId;  
  
  
    IF(@RoleCount > 0)  
    BEGIN  
        SET @RolePercentage = 100;  
    END  

	SELECT   
        @TitleCount = COUNT(*)   
    FROM JobTitle   
    WHERE TenantID = @TenantId;  
  
  
    IF(@TitleCount > 0)  
    BEGIN  
        SET @TitlePercentage = 100;  
    END  

	
	SELECT   
        @ProjectCount = COUNT(*)   
    FROM CRMProject   
    WHERE TenantID = @TenantId;  
  
  
    IF(@ProjectCount > 0)  
    BEGIN  
        SET @ProjectPercentage = 100;  
    END  


	SELECT   
        @AllocationCount = COUNT(*)   
    FROM ProjectEstimatedAllocation   
    WHERE TenantID = @TenantId;  
  
  
    IF(@AllocationCount > 0)  
    BEGIN  
        SET @AllocationPercentage = 100;  
    END  





  
  
  
  
    SELECT 'Registration' as WorkFlow, @RegistrationPercentage as Percentage, @RegistrationCount as TotalCount UNION ALL  
       
  
  
    SELECT 'ManageIncidents' as WorkFlow, @ManageIncidentPercentage as Percentage, @ManageIncidentCount as TotalCount UNION ALL  
  
  
   
    SELECT 'AddedUsers' as WorkFlow, @UserPercentage as Percentage, @UserCount as TotalCount UNION ALL  
       
  
  
    SELECT 'UserCreatedIncidents' as WorkFlow, @UserCreatedIncidentPercentage as Percentage, @UserCreatedIncidentCount as TotalCount UNION ALL 
	
    SELECT 'UsedServices' as WorkFlow, @ServicePercentage as Percentage, @ServiceCount as TotalCount UNION ALL

    SELECT 'Department' as WorkFlow, @DepartmentPercentage as Percentage, @DepartmentCount as TotalCount UNION ALL 
	SELECT 'Role' as WorkFlow, @RolePercentage as Percentage, @RoleCount as TotalCount UNION ALL 
	SELECT 'Title' as WorkFlow, @TitlePercentage as Percentage, @TitleCount as TotalCount UNION ALL  
	SELECT 'Project' as WorkFlow, @ProjectPercentage as Percentage, @ProjectCount as TotalCount UNION ALL
	SELECT 'Allocation' as WorkFlow, @AllocationPercentage as Percentage, @AllocationCount as TotalCount ;  
  
  
  
  
  
END  