
ALTER PROCEDURE [dbo].[GetOtherAllocationDetails]          
@StartDate datetime,          
@TenantID varchar(128),          
@AllocationType varchar(50),	-- conflicts, unfilled, unfilledProject, unfilledPipeline          
@ResultType varchar(50),		-- records, count      
@ShowByUsersDivision bit,
@UserId nvarchar(128)
AS          
BEGIN          
 Declare @CPRAwardedStageStep int;          
 Declare @OPMCloseStageStep int;          
          
 select @CPRAwardedStageStep = StageStep from Config_Module_ModuleStages where TenantID = @TenantID          
 and ModuleNameLookup = 'CPR'          
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
  Division bigint,
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
 StageStep, DivisionLookup from CRMProject where TenantID = @TenantID;          
          
          
          
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
 StageStep, DivisionLookup from Opportunity where TenantID = @TenantID;          
          
          
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
 StageStep, DivisionLookup from CRMServices where TenantID = @TenantID;          
                    
 Declare @DepartmentLookup bigint;
 Declare @DivisionLookup bigint;
 Declare @Division nvarchar(max);

 select @DepartmentLookup = IsNULL(DepartmentLookup, 0) from AspNetUsers where Id = @UserId and TenantID = @TenantID and isRole = 0;
 select @DivisionLookup = DivisionIdLookup from Department where id = @DepartmentLookup;
 select @Division = Title from CompanyDivisions where id = @DivisionLookup;

 IF @AllocationType = 'conflicts'          
 Begin          
          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, u.[Name] as [User], pe.TicketID, '' as Title, pe.TicketID as Project, pe.PctAllocation, pe.AllocationStartDate, 
   pe.AllocationEndDate, '' as 'RoleName', pe.[RoleId] as [Type], pe.ResourceUser as AssignedToUser 
   from ResourceAllocation pe                 
   join AspNetUsers u on u.Id = pe.ResourceUser 
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.AllocationStartDate >= @StartDate          
   and pe.Attachments not like '%replaced%'       
   and (u.Id = @UserId or u.ManagerUser = @UserId)
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ResourceAllocation pe                   
   join AspNetUsers u on u.Id = pe.ResourceUser            
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.Attachments not like '%replaced%'      
   and pe.AllocationStartDate >= @StartDate    
   and (u.Id = @UserId or u.ManagerUser = @UserId)   
  End          
          
 End          
 Else IF @AllocationType = 'unfilled'          
 Begin          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, pe.TicketID, tmp.Title, pe.TicketID + ', ' + tmp.Title as Project, pe.PctAllocation, pe.AllocationStartDate, 
   pe.AllocationEndDate, r.[Name] as 'RoleName', pe.[Type], pe.AssignedToUser, tmp.Division 
   from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)  
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
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
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
  End          
 End          
 Else IF @AllocationType = 'unfilledProject'          
 Begin          
  IF @ResultType = 'records'          
  Begin          
   select pe.ID, pe.TicketID, tmp.Title,pe.TicketID + ', ' + tmp.Title as Project, pe.PctAllocation, pe.AllocationStartDate, 
   pe.AllocationEndDate, r.[Name] as 'RoleName', pe.[Type], pe.AssignedToUser, tmp.Division from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   and tmp.CPRStageStep >= @CPRAwardedStageStep and tmp.TicketId like 'CPR-%' 
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end) 
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'                  
   and tmp.CPRStageStep >= @CPRAwardedStageStep and tmp.TicketId like 'CPR-%'          
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)   
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
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
   and (tmp.CPRStageStep < @CPRAwardedStageStep or tmp.OPMStageStep < @OPMCloseStageStep)  
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
   order by AllocationStartDate asc          
  End          
  Else IF @ResultType = 'count'          
  Begin          
   select count(*) [Count] from ProjectEstimatedAllocation pe          
   join Roles r on r.Id = pe.[Type]          
   join #tmpProjects tmp on tmp.TicketId = pe.TicketID          
   where pe.TenantID = @TenantID          
   and pe.AssignedToUser = '00000000-0000-0000-0000-000000000000'          
   and (tmp.CPRStageStep < @CPRAwardedStageStep or tmp.OPMStageStep < @OPMCloseStageStep)         
   and ISNULL(tmp.Division,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(tmp.Division,'') else '%' + @Division + '%' end)   
   and pe.TicketId in (select TicketId from ModuleUserStatistics where UserName = @UserId and TenantID = @TenantID)
  End          
 End          
       
 drop table #tmpProjects;          
END
