
ALTER PROCEDURE [dbo].[GetOtherAllocationDetails]          
@StartDate datetime='2022-11-01',          
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B',          
@AllocationType varchar(50) = 'conflicts',  -- conflicts, unfilled, unfilledProject, unfilledPipeline          
@ResultType varchar(50) = 'records',  -- records, count      
@ShowByUsersDivision bit = 'False',
@UserId nvarchar(128) = '44380d17-c887-488c-856b-31753e4197b7'
AS          
BEGIN          
 Declare @CPRAwardedStageStep int;          
 Declare @OPMCloseStageStep int;          
          
 select @CPRAwardedStageStep = StageStep from Config_Module_ModuleStages where TenantID = @TenantID          
 and ModuleNameLookup = 'CPR'          
 --and CustomProperties like '%awardstage%'          
 and StageTypeChoice = 'Resolved'  
          
 select @OPMCloseStageStep = StageStep from Config_Module_ModuleStages where TenantID = @TenantID          
 and StageTypeChoice = 'Closed'          
          
 create table #tmpProjects          
 (          
  TicketId varchar(50),          
  Title nvarchar(1000),          
  TenantID nvarchar(128),          
  CPRStageStep int,          
  OPMStageStep int,          
  CNSStageStep int,
  Division nvarchar(1000)
 )          
           
          
 insert into #tmpProjects          
 (          
 TicketId,          
 Title,          
 TenantID,          
 CPRStageStep,
 Division
 )          
 select TicketId, Title,          
 TenantID,          
 StageStep, CRMBusinessUnitChoice from CRMProject where TenantID = @TenantID;          
          
          
          
 insert into #tmpProjects          
 (          
 TicketId,          
 Title,          
 TenantID,          
 OPMStageStep,
 Division
 )          
 select TicketId, Title,          
 TenantID,          
 StageStep, CRMBusinessUnitChoice from Opportunity where TenantID = @TenantID;          
          
          
 insert into #tmpProjects          
 (          
 TicketId,          
 Title,          
 TenantID,          
 CNSStageStep,
 Division
 )          
 select TicketId, Title,          
 TenantID,          
 StageStep, CRMBusinessUnitChoice from CRMServices where TenantID = @TenantID;          
                    
 --select * from #tmpProjects;          
 -- drop table #tmpProjects;          

 Declare @DepartmentLookup bigint;
 Declare @DivisionLookup bigint;
 Declare @Division nvarchar(1000);

 select @DepartmentLookup = IsNULL(DepartmentLookup, 0) from AspNetUsers where Id = @UserId and TenantID = @TenantID and isRole = 0;
 select @DivisionLookup = DivisionIdLookup from Department where id = @DepartmentLookup;
 select @Division = Title from CompanyDivisions where id = @DivisionLookup;
 --select @Division;

 IF @AllocationType = 'conflicts'          
 Begin          
          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, u.[Name] as [User], pe.TicketID, '' as Title, pe.TicketID as Project, pe.PctAllocation, pe.AllocationStartDate, pe.AllocationEndDate, '' as 'RoleName', pe.[RoleId] as [Type], pe.ResourceUser as AssignedToUser from ResourceAllocation pe         
   --join Roles r on r.Id = pe.[RoleId]          
   join AspNetUsers u on u.Id = pe.ResourceUser 
   --left join Department d on d.ID = u.DepartmentLookup
   --left join CompanyDivisions c on c.ID = d.DivisionIdLookup
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.AllocationStartDate >= @StartDate          
   and pe.Attachments not like '%replaced%'      
   --and ISNULL(c.Title,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(c.Title,'') else '%' + @Division + '%' end)    
   and (u.Id = @UserId or u.ManagerUser = @UserId)
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ResourceAllocation pe          
   --join Roles r on r.Id = pe.[RoleId]          
   join AspNetUsers u on u.Id = pe.ResourceUser            
   --left join Department d on d.ID = u.DepartmentLookup
   --left join CompanyDivisions c on c.ID = d.DivisionIdLookup
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.Attachments not like '%replaced%'      
   and pe.AllocationStartDate >= @StartDate    
   and (u.Id = @UserId or u.ManagerUser = @UserId)
   --and ISNULL(c.Title,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(c.Title,'') else '%' + @Division + '%' end)    
  End          
          
 End          
 Else IF @AllocationType = 'unfilled'          
 Begin          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, pe.TicketID, tmp.Title, pe.TicketID + ', ' + tmp.Title as Project, pe.PctAllocation, pe.AllocationStartDate, pe.AllocationEndDate, r.[Name] as 'RoleName', pe.[Type], pe.AssignedToUser, tmp.Division from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)     
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID       
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)     
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
  End          
 End          
 Else IF @AllocationType = 'unfilledProject'          
 Begin          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, pe.TicketID, tmp.Title,pe.TicketID + ', ' + tmp.Title as Project, pe.PctAllocation, pe.AllocationStartDate, pe.AllocationEndDate, r.[Name] as 'RoleName', pe.[Type], pe.AssignedToUser, tmp.Division from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
   and tmp.CPRStageStep >= @CPRAwardedStageStep and tmp.TicketId like 'CPR-%' 
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)        
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
   and tmp.CPRStageStep >= @CPRAwardedStageStep and tmp.TicketId like 'CPR-%'          
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)     
  End          
          
 End          
 Else IF @AllocationType = 'unfilledPipeline'          
 Begin          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, pe.TicketID, tmp.Title, pe.TicketID + ', ' + tmp.Title as Project, pe.PctAllocation, pe.AllocationStartDate, pe.AllocationEndDate, r.[Name] as 'RoleName', pe.[Type], pe.AssignedToUser, tmp.Division from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
   and (tmp.CPRStageStep < @CPRAwardedStageStep or tmp.OPMStageStep < @OPMCloseStageStep)  
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)  
   --and tmp.Division like '%' + @Division + '%'
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   --and pe.AllocationStartDate <= @StartDate and @StartDate <= pe.AllocationEndDate          
   and (tmp.CPRStageStep < @CPRAwardedStageStep or tmp.OPMStageStep < @OPMCloseStageStep)         
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)     
  End          
 End          
       
 drop table #tmpProjects;          
END
