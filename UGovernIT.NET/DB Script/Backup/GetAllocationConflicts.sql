
ALTER PROCEDURE [dbo].[GetAllocationConflicts]          
@StartDate datetime='2022-10-31',  
@EndDate datetime='2022-11-06',  
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B',     
@ShowByUsersDivision bit = 'False',
@UserId nvarchar(128) = '44380d17-c887-488c-856b-31753e4197b7'
AS          
BEGIN          
           
 Declare @DepartmentLookup bigint;
 Declare @DivisionLookup bigint;
 Declare @Division nvarchar(1000);

 Declare @TotalAllocConflicts int;
 Declare @TotalAllocConflictsThisWeek int;
 Declare @TotalAllocConflictsin3Weeks int;

 select @DepartmentLookup = IsNULL(DepartmentLookup, 0) from AspNetUsers where Id = @UserId and TenantID = @TenantID and isRole = 0;
 select @DivisionLookup = DivisionIdLookup from Department where id = @DepartmentLookup;
 select @Division = Title from CompanyDivisions where id = @DivisionLookup;
 --select @Division;

   select @TotalAllocConflicts = count(*) from ResourceAllocation pe     
   join AspNetUsers u on u.Id = pe.ResourceUser            
   --left join Department d on d.ID = u.DepartmentLookup
   --left join CompanyDivisions c on c.ID = d.DivisionIdLookup
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.Attachments not like '%replaced%'      
   and pe.AllocationStartDate >= @StartDate          
   --and ISNULL(c.Title,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(c.Title,'') else '%' + @Division + '%' end); 
   and (u.Id = @UserId or u.ManagerUser = @UserId);

   select @TotalAllocConflictsThisWeek = count(*) from ResourceAllocation pe     
   join AspNetUsers u on u.Id = pe.ResourceUser            
   --left join Department d on d.ID = u.DepartmentLookup
   --left join CompanyDivisions c on c.ID = d.DivisionIdLookup
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.Attachments not like '%replaced%'      
   and pe.AllocationStartDate >= @StartDate and  pe.AllocationStartDate <= @EndDate        
   --and ISNULL(c.Title,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(c.Title,'') else '%' + @Division + '%' end)
   and (u.Id = @UserId or u.ManagerUser = @UserId); 

   select @TotalAllocConflictsin3Weeks = count(*) from ResourceAllocation pe     
   join AspNetUsers u on u.Id = pe.ResourceUser            
   --left join Department d on d.ID = u.DepartmentLookup
   --left join CompanyDivisions c on c.ID = d.DivisionIdLookup
   where pe.TenantID = @TenantID          
   and pe.TicketID not like '%-__-%'          
   and pe.Attachments not like '%replaced%'      
   and pe.AllocationStartDate >= @StartDate and  pe.AllocationStartDate <= DATEADD(DAY, 21, @StartDate)       
   --and ISNULL(c.Title,'') like (SELECT Case when @ShowByUsersDivision=0 then Isnull(c.Title,'') else '%' + @Division + '%' end)
   and (u.Id = @UserId or u.ManagerUser = @UserId); 

   select @TotalAllocConflicts as TotalAllocConflicts, @TotalAllocConflictsThisWeek as TotalAllocConflictsThisWeek, @TotalAllocConflictsin3Weeks as TotalAllocConflictsin3Weeks
        
END
