
/****** Object:  StoredProcedure [dbo].[GetDashboardCardViewData]    Script Date: 1/15/2020 7:28:56 PM ******/


CREATE  PROCEDURE [dbo].[GetDashboardCardViewData]
(
	@UserId   nvarchar(128), 
	@TenantId nvarchar(128),
	@Group nvarchar(128)
) 
AS 
  BEGIN 
      DECLARE @UserTotalTask               INT, 
              @UserTotalContacts           INT, 
              @UserTotalLeads              INT, 
			  @UserTotalCompanies		   INT, 
              @UserTotalProjects           INT, 
              @UserTotalActionsItems	   INT, 
			  @UserTotalOpportunities	   INT,
              @TotalCompanies              INT, 
              @TotalContacts               INT, 
              @TotalLeads                  INT, 
              @TotalOpportunities          INT, 
              @TotalProjects               INT, 
              @TotalLiveProjects           INT, 
			  @TotalProjectsStarted        INT, 
              @TotalProjectsClosed         INT, 
              @TotalProjectsDue            INT, 
			  @UserTotalProjectsDue		   INT,
			  @TotalPipelineProjects	   INT,
			  @TotalResource			   INT,
              @TotalAmountLiveProjects     MONEY, 
              @TotalAmountClosedProjects   MONEY, 
              @TotalAmountPipelineProjects MONEY, 
			  @TotalAmountOpportunities    MONEY, 
              @UserRole                    NVARCHAR(56) 

		
      IF @Group <> '' 
		BEGIN
			SET @UserRole = @Group
		END
	  ELSE 
		BEGIN
			SELECT @UserRole = r.NAME 
			  FROM   aspnetusers AS u 
					 JOIN userroles AS r 
					   ON r.id = u.userroleid 
			  WHERE  u.id = @UserId; 
		END

      

	  DECLARE @UserRolesTable TABLE (RoleId nvarchar(128))

	  INSERT INTO @UserRolesTable
		SELECT roleid 
		FROM   aspnetusers (nolock) u 
				JOIN aspnetuserroles (nolock) ur 
					ON u.id = ur.userid 
				JOIN aspnetroles (nolock) r 
					ON r.id = ur.roleid 
		WHERE  u.id = @UserId 
		UNION 
		SELECT @UserId as RoleId;

      IF Len(@UserRole) > 0 
        BEGIN 
            -- CRM  
            IF @UserRole = 'CRM' 
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Contacts  
                  --SELECT @UserTotalContacts = Count(*) 
                  --FROM   crmcontact c					
                  --WHERE  tenantid = @TenantId

				  -- Total Companies  
     --             SELECT @TotalCompanies = Count(*) 
     --             FROM   crmcompany 
     --             WHERE  tenantid = @TenantId 

     --             -- My Leads  
     --             SELECT @UserTotalLeads = Count(*) 
     --             FROM   lead c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

                  -- My Opportunity
     --             SELECT @UserTotalOpportunities = Count(*) 
     --             FROM   Opportunity c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

                  -- Total Opportunity  
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   Opportunity 
                  WHERE  tenantid = @TenantId 


                  SELECT @UserRole			AS UserRole,
						 @UserTotalTask     AS UserTotalTask, 
                         @UserTotalContacts AS UserTotalContacts, 
                         @TotalCompanies    AS TotalCompanies, 
                         @UserTotalLeads    AS UserTotalLeads, 
                         @UserTotalOpportunities AS UserTotalOpportunities, 
                         @TotalOpportunities AS TotalOpportunities
              END 


			  -- APM  
            IF @UserRole = 'APM' 
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Contacts  
      --            SELECT @TotalContacts = Count(*) 
      --            FROM   crmcontact c					
      --            WHERE  tenantid = @TenantId

				  ---- Total Companies  
      --            SELECT @TotalCompanies = Count(*) 
      --            FROM   crmcompany 
      --            WHERE  tenantid = @TenantId 

                  -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

     --             -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

	
				 -- My Projects Due in 4 Week  
     --             SELECT @UserTotalProjectsDue = Count(*) 
     --             FROM   crmproject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 
					-- AND EstimatedConstructionEnd >= GETDATE()
					-- AND EstimatedConstructionEnd <= DATEADD(ww, 4, GETDATE())
					-- AND Closed <> 1


                  SELECT @UserRole			AS UserRole,
						 @UserTotalTask     AS UserTotalTask, 
                         @TotalContacts AS TotalContacts, 
                         @TotalCompanies    AS TotalCompanies, 
                         @UserTotalProjects    AS UserTotalProjects, 
                         @TotalProjects AS TotalProjects,
                         @UserTotalProjectsDue AS UserTotalProjectsDue
              END 


			    -- Estimator  
            IF @UserRole = 'Estimator' 
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

				   -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

				 --  -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

     --             -- My Opportunity
     --             SELECT @UserTotalOpportunities = Count(*) 
     --             FROM   Opportunity c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

                  -- Total Opportunity  
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   Opportunity 
                  WHERE  tenantid = @TenantId 


                  SELECT @UserRole			AS UserRole,
						 @UserTotalTask     AS UserTotalTask, 
                         @UserTotalProjects AS UserTotalProjects, 
                         @TotalProjects    AS TotalProjects, 
                         @UserTotalOpportunities    AS UserTotalOpportunities, 
                         @TotalOpportunities AS TotalOpportunities                         
              END 


			-- Core User  
            IF @UserRole = 'Core User' 
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

     --             -- My Contacts  
     --             SELECT @UserTotalContacts = Count(*) 
     --             FROM   crmcontact  c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value					
     --             WHERE  tenantid = @TenantId

				 -- -- My Companies  
     --             SELECT @UserTotalCompanies = Count(*) 
     --             FROM   crmcompany c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

     --             -- My Leads  
     --             SELECT @UserTotalLeads = Count(*) 
     --             FROM   lead c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

                  -- My Opportunity
     --             SELECT @UserTotalOpportunities = Count(*) 
     --             FROM   Opportunity c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

     --             -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId


                  SELECT @UserRole			AS UserRole,
						 @UserTotalTask     AS UserTotalTask, 
                         @UserTotalContacts AS UserTotalContacts, 
                         @UserTotalCompanies    AS UserTotalCompanies, 
                         @UserTotalLeads    AS UserTotalLeads, 
                         @UserTotalOpportunities AS UserTotalOpportunities, 
                         @UserTotalProjects AS UserTotalProjects
              END 


			  -- PE  & PM 
            IF @UserRole = 'PE' OR @UserRole = 'PM'
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Contacts  
     --             SELECT @TotalContacts = Count(*) 
     --             FROM   crmcontact					
     --             WHERE  tenantid = @TenantId

				 -- -- Companies  
     --             SELECT @TotalCompanies = Count(*) 
     --             FROM   crmcompany 
     --             WHERE  tenantid = @TenantId 

     --             -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId
				  
				 --  -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

                  -- My Projects Due in 4 Week  
     --             SELECT @UserTotalProjectsDue = Count(*) 
     --             FROM   crmproject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId                  
					-- AND EstimatedConstructionEnd >= GETDATE()
					-- AND EstimatedConstructionEnd <= DATEADD(ww, 4, GETDATE())
					-- AND Closed <> 1                 


                  SELECT @UserRole			AS UserRole,
						 @UserTotalTask     AS UserTotalTask, 
                         @TotalContacts AS TotalContacts, 
                         @TotalCompanies    AS TotalCompanies, 
                         @UserTotalProjects    AS UserTotalProjects, 
                         @TotalProjects AS TotalProjects, 
                         @UserTotalProjectsDue AS UserTotalProjectsDue
              END 


			-- CRM Admin  
            IF @UserRole = 'CRM Admin' 
              BEGIN 
				  -- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Total Companies  
     --             SELECT @TotalCompanies = Count(*) 
     --             FROM   crmcompany 
     --             WHERE  tenantid = @TenantId 

     --             -- Total Contacts  
     --             SELECT @TotalContacts = Count(*) 
     --             FROM   crmcontact 
     --             WHERE  tenantid = @TenantId 

     --             -- Total Leads      
     --             SELECT @TotalLeads = Count(*) 
     --             FROM   lead 
     --             WHERE  tenantid = @TenantId 

     --             -- My Opportunity
     --             SELECT @UserTotalOpportunities = Count(*) 
     --             FROM   Opportunity c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

                  -- Total Projects  
                  --SELECT @TotalProjects = Count(*) 
                  --FROM   crmproject 
                  --WHERE  tenantid = @TenantId

                  SELECT @UserRole			 AS UserRole, 
						 @UserTotalTask      AS UserTotalTask, 
                         @TotalContacts      AS TotalContacts, 
						 @TotalCompanies     AS TotalCompanies,
                         @TotalLeads         AS TotalLeads, 
                         @UserTotalOpportunities AS UserTotalOpportunities, 
                         @TotalProjects      AS TotalProjects 
              END 

			-- PM Admin  
            IF @UserRole = 'PM Admin' 
              BEGIN 
				  -- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

     --             -- My Opportunity
     --             SELECT @UserTotalOpportunities = Count(*) 
     --             FROM   Opportunity c
					--CROSS APPLY string_split(ISNULL(c.OwnerUser, ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId 

                  -- Total Opportunity  
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   Opportunity 
                  WHERE  tenantid = @TenantId 

                   -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

     --             -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

				 -- -- Projects Due in 4 Week  
     --             SELECT @TotalProjectsDue = Count(*) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					-- AND EstimatedConstructionEnd >= GETDATE()
					-- AND EstimatedConstructionEnd <= DATEADD(ww, 4, GETDATE())
					-- AND Closed <> 1

                  SELECT @UserRole			 AS UserRole, 
						 @UserTotalTask      AS UserTotalTask, 
                         @UserTotalOpportunities AS UserTotalOpportunities, 
						 @TotalOpportunities     AS TotalOpportunities,
                         @UserTotalProjects  AS UserTotalProjects, 
                         @TotalProjects AS TotalProjects, 
                         @TotalProjectsDue      AS TotalProjectsDue 
              END 

			  /*
            -- PM  
            IF @UserRole = 'PM' 
              BEGIN 
                  -- My Tasks  
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- My Projects  
                  SELECT @UserTotalProjects = Count(*) 
                  FROM   crmproject c
					CROSS APPLY string_split(CONCAT(ISNULL(c.[Owner], ''), ',' ,ISNULL(Estimator, ''), ',' ,ISNULL(ActionUserTypes, ''), ',' ,ISNULL(ActionUsers, ''), ',' ,ISNULL(StageActionUserTypes, ''), ',' ,ISNULL(StageActionUsers, ''), ',' ,ISNULL(ProjectExecutive, ''), ',' ,ISNULL(ProjectManager, ''), ',' ,ISNULL(Superintendent, ''), ',' ,ISNULL([Initiator], '')), ',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- My Action Items  
                  SELECT @UserTotalActionsItems = Count(*) 
                  FROM   crmproject 
					CROSS APPLY string_split(CONCAT(ISNULL(ActionUserTypes, ''), ',' ,ISNULL(ActionUsers, ''), ',' ,ISNULL(StageActionUserTypes, ''), ',' ,ISNULL(StageActionUsers, '')),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId 					

                  -- Total Opportunities      
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   Opportunity 
                  WHERE  tenantid = @TenantId 

                  -- Total Projects Closed This Week  
                  SELECT @TotalProjectsClosed = Count(*)
                  FROM   crmproject 
                  WHERE  tenantid = @TenantId 
					AND  Closed = 1
					AND  CloseDate >= DATEADD(ww, DATEDIFF(ww,0,DATEADD(d, -1, GETDATE())), 0)

                  -- Total Projects Due in 4 Week  
                  SELECT @TotalProjectsDue = Count(*) 
                  FROM   crmproject 
                  WHERE  tenantid = @TenantId 
					 AND EstimatedConstructionEnd >= GETDATE()
					 AND EstimatedConstructionEnd <= DATEADD(ww, 4, GETDATE())
					 AND Closed <> 1

                  SELECT @UserRole				AS UserRole,
						 @UserTotalTask			AS UserTotalTask, 
                         @UserTotalProjects		AS UserTotalProjects, 
                         @UserTotalActionsItems AS UserTotalActionsItems, 
                         @TotalOpportunities	AS TotalOpportunities, 
                         @TotalProjectsClosed	AS TotalProjectsClosed, 
                         @TotalProjectsDue		AS TotalProjectsDue 
              END 
			  */

            -- Project Executive  
            IF @UserRole = 'Project Executive' 
              BEGIN				
				  -- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   crmproject 
					--CROSS APPLY string_split(CONCAT(ISNULL([Owner], ''), ',' ,ISNULL(Estimator, ''), ',' ,ISNULL(ActionUserTypes, ''), ',' ,ISNULL(ActionUsers, ''), ',' ,ISNULL(StageActionUserTypes, ''), ',' ,ISNULL(StageActionUsers, ''), ',' ,ISNULL(ProjectExecutive, ''), ',' ,ISNULL(ProjectManager, ''), ',' ,ISNULL(Superintendent, ''), ',' ,ISNULL([Initiator], '')), ',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

                  -- My Action Items  
     --             SELECT @UserTotalActionsItems = Count(*) 
     --             FROM   crmproject 
					--CROSS APPLY string_split(CONCAT(ISNULL(ActionUserTypes, ''), ',' ,ISNULL(ActionUsers, ''), ',' ,ISNULL(StageActionUserTypes, ''), ',' ,ISNULL(StageActionUsers, '')),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

                  -- Total Amount of Live Projects       
     --             SELECT @TotalAmountLiveProjects = Sum(cast(ApproxContractValue as money)) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					--AND  Closed = 0
					--AND  StageStep <= 8

                  -- Total Amount of Closed Projects  
     --             SELECT @TotalAmountClosedProjects = Sum(cast(ApproxContractValue as money)) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					--AND  Closed = 1

     --             -- Total Amount of Pipeline Projects  
     --             SELECT @TotalAmountPipelineProjects = Sum(cast(ApproxContractValue as money))
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
     --               AND  Closed = 0
					--AND  StageStep < 8

                  SELECT @UserRole											AS UserRole,
						 @UserTotalTask										AS UserTotalTask, 
                         @UserTotalProjects									AS UserTotalProjects, 
                         @UserTotalActionsItems								AS UserTotalActionsItems, 
                         format(@TotalAmountLiveProjects,'$0,,.0M')			AS TotalAmountLiveProjects, 
                         format(@TotalAmountClosedProjects,'$0,,.0M')		AS TotalAmountClosedProjects, 
                         format(@TotalAmountPipelineProjects,'$0,,.0M')		AS TotalAmountPipelineProjects 
              END 

			-- Executive  
            IF @UserRole = 'Executive' 
              BEGIN				
				  -- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

				  -- Total & Amount Opportunity  
                  --SELECT @TotalOpportunities = Count(*) , @TotalAmountOpportunities = Sum(cast(ApproxContractValue as money)) 
                  --FROM   Opportunity 
                  --WHERE  tenantid = @TenantId 


				 -- -- Total & Amount of Pipeline Projects  
     --             SELECT @TotalPipelineProjects = Count(*) , @TotalAmountPipelineProjects = Sum(cast(ApproxContractValue as money))
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
     --               AND  Closed = 0
					--AND  StageStep < 8

				  -- Total & Amount of Live Projects       
     --             SELECT @TotalLiveProjects = Count(*) ,  @TotalAmountLiveProjects = Sum(cast(ApproxContractValue as money)) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					--AND  Closed = 0
					--AND  StageStep <= 8


     --             -- Total Started Projects  this Month
     --             SELECT @TotalProjectsStarted = Count(*) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					--AND  Closed <> 1
					--AND MONTH(EstimatedConstructionStart) = MONTH(GETDATE())
					--AND YEAR(EstimatedConstructionStart) = YEAR(GETDATE())

                 
				 -- -- Total & Amount of Closed Projects  this Year
     --             SELECT @TotalProjectsClosed = Count(*) , @TotalAmountClosedProjects = Sum(cast(ApproxContractValue as money)) 
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
					--AND  Closed = 1
					--AND YEAR(CloseDate) = YEAR(GETDATE())
					--AND CloseDate IS NOT NULL

				  -- Total Resource count
				  SELECT @TotalResource = Count(*) 
				  FROM AspNetUsers 
				  WHERE  tenantid = @TenantId 
				  AND Enabled = 1

                  SELECT @UserRole											AS UserRole,
						 @UserTotalTask										AS UserTotalTask, 
                         '(' + cast(@TotalOpportunities as nvarchar(25)) + ') (' + format(@TotalAmountOpportunities,'$0,,.0M') + ')'	AS Opportunities, 
						 '(' + cast(@TotalPipelineProjects as nvarchar(25)) + ') (' + format(@TotalAmountPipelineProjects,'$0,,.0M') + ')'  AS PipelineProjects,
                         '(' + cast(@TotalLiveProjects as nvarchar(25)) + ') (' + format(@TotalAmountLiveProjects,'$0,,.0M') + ')' AS LiveProjects, 
						 @TotalProjectsStarted AS TotalProjectsStarted,
                         '(' + cast(@TotalProjectsClosed as nvarchar(25)) + ') (' + format(@TotalAmountClosedProjects,'$0,,.0M')	+ ')'	AS ThisYearClosedProjects,        
						 @TotalResource AS TotalResource 
              END 


			-- Superintendent
			 IF @UserRole = 'Superintendent' 
              BEGIN		
				  -- My Tasks
					SELECT @UserTotalTask = Count(*) 
					FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
					WHERE  tenantid = @TenantId

				 -- Total Ready to start Projects  
     --             SELECT @TotalPipelineProjects = Count(*)
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
     --               AND  Closed = 0
					--AND  StageStep = 7

     --             -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

     --             -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

				SELECT @UserRole			AS UserRole,
					@UserTotalTask     AS UserTotalTask, 
                    @TotalPipelineProjects AS ProjectsReadyToStart, 
                    @UserTotalProjects    AS UserTotalProjects,
                    @TotalProjects    AS TotalProjects,
					'-1' AS ProjectAllocation,
					'-1' AS ActiveSubcons,
					'-1' AS AllSubCons

			  END
			

			-- PreCon Admin
			IF @UserRole = 'PreCon Admin' 
              BEGIN		
			-- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Total Opportunity  
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   Opportunity 
                  WHERE  tenantid = @TenantId 

                  -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

     --             -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

				 -- -- Total Ready to start Projects  
     --             SELECT @TotalPipelineProjects = Count(*)
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
     --               AND  Closed = 0
					--AND  StageStep = 7

				SELECT @UserRole			AS UserRole,
					@UserTotalTask     AS UserTotalTask, 
					@TotalOpportunities AS TotalOpportunities,
					@UserTotalProjects    AS UserTotalProjects,
                    @TotalProjects    AS TotalProjects,
                    @TotalPipelineProjects AS ProjectsReadyToStart,                    
					'-1' AS ResourceUtilization
			  END

			-- Field Operations
			IF @UserRole = 'Field Operations' 
              BEGIN				
			 -- My Tasks
					SELECT @UserTotalTask = Count(*) 
					FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
					WHERE  tenantid = @TenantId

				 -- Total Ready to start Projects  
     --             SELECT @TotalPipelineProjects = Count(*)
     --             FROM   crmproject 
     --             WHERE  tenantid = @TenantId 
     --               AND  Closed = 0
					--AND  StageStep = 7

     --             -- My Projects  
     --             SELECT @UserTotalProjects = Count(*) 
     --             FROM   CRMProject c
					--CROSS APPLY string_split(ISNULL(c.[Owner], ''),',') at
					--JOIN @UserRolesTable r on r.RoleId = at.value
     --             WHERE  tenantid = @TenantId

     --             -- All Projects 
     --             SELECT @TotalProjects = Count(*) 
     --             FROM   CRMProject 
     --             WHERE  tenantid = @TenantId 

				SELECT @UserRole			AS UserRole,
					@UserTotalTask     AS UserTotalTask, 
                    @TotalPipelineProjects AS ProjectsReadyToStart, 
                    @UserTotalProjects    AS UserTotalProjects,
                    @TotalProjects    AS TotalProjects,
					'-1' AS ProjectAllocation,
					'-1' AS ActiveSubcons,
					'-1' AS AllSubCons
			  END

            -- Admin  
            IF @UserRole = 'Admin' 
              BEGIN 
				  -- My Tasks
                  SELECT @UserTotalTask = Count(*) 
                  FROM   ModuleTasks c
					CROSS APPLY string_split(ISNULL(c.AssignedToUser, ''),',') at
					JOIN @UserRolesTable r on r.RoleId = at.value
                  WHERE  tenantid = @TenantId

                  -- Total Companies  
                  --SELECT @TotalCompanies = Count(*) 
                  --FROM   crmcompany 
                  --WHERE  tenantid = @TenantId 

                  ---- Total Contacts  
                  --SELECT @TotalContacts = Count(*) 
                  --FROM   crmcontact 
                  --WHERE  tenantid = @TenantId 

                  ---- Total Leads      
                  --SELECT @TotalLeads = Count(*) 
                  --FROM   lead 
                  --WHERE  tenantid = @TenantId 

                  -- Total Opportunities  
                  SELECT @TotalOpportunities = Count(*) 
                  FROM   opportunity 
                  WHERE  tenantid = @TenantId 

                  -- Total Projects  
                  --SELECT @TotalProjects = Count(*) 
                  --FROM   crmproject 
                  --WHERE  tenantid = @TenantId

                  SELECT @UserRole			 AS UserRole, 
						 @UserTotalTask      AS UserTotalTask, 
                         @TotalCompanies     AS TotalCompanies, 
                         @TotalContacts      AS TotalContacts, 
                         @TotalLeads         AS TotalLeads, 
                         @TotalOpportunities AS TotalOpportunities, 
                         @TotalProjects      AS TotalProjects 
              END 
        END 
  END 
GO


