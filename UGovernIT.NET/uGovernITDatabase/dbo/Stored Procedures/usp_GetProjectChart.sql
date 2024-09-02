
CREATE procedure [dbo].[usp_GetProjectChart]  
-- [usp_GetProjectChart] '35525396-E5FE-4692-9239-4DF9305B915B', '', '', 'Healthcare', 0, 0, 0
(  
@TenantId varchar(128)= '', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',    
@Startdate datetime = '', --'2022-01-01 00:00:00.000',    
@Endate datetime = '', --'2022-07-31 00:00:00.000',    
@sector nvarchar(250) = '',  
@division int = 0,    
@studio int = 0,
@closed bit
)  
as  
begin  
  declare @rewardedStage int;    
  set @rewardedStage = 0;    
  select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@TenantID    
 DECLARE @SQLOpportunities nvarchar(max), @SQLTrackedProjects nvarchar(max), @SQLConstruction nvarchar(max), @SQLwhere nvarchar(max) 
  
  declare @Pipeline varchar(20), @Precon varchar(20), @Construction varchar(20), @Closeout varchar(20)  
  
 DECLARE @tmpProjects table
 (  
  [Status] varchar(1000),  
  [TicketId] varchar(100),  
  [ResourceAllocationCount] varchar(20),
  ApproxContractValue float,
  Closed bit
 )  

  SET @SQLwhere = ' TenantID=''' + @TenantID + ''' and (OnHold = 0 or OnHold is null) '
   if (@sector is not null AND LEN(@sector) > 0)
	  set @SQLwhere = @SQLwhere +' and ( SectorChoice = (case when len(''' + @sector+ ''') > 0 then '''+ @sector + ''' else SectorChoice end)  )  '
   if (@division != 0)
	  set @SQLwhere = @SQLwhere +' and ( divisionlookup = (case when ' + cast(@division as varchar(10)) + ' > 0 then ' + cast(@division as varchar(10))  + ' else divisionlookup end)  )  '    
   if (@division != 0)
	  set @SQLwhere = @SQLwhere +' and ( StudioLookup = (case when ' + cast(@studio  as varchar(10))  + ' > 0 then ' + cast(@studio as varchar(10)) + ' else StudioLookup end)  ) '
 set @SQLwhere = @SQLwhere + ' )temp  '    

-- Fill Open Opportunities OPM records in the temp table
   set @SQLOpportunities = 'select ''Opportunities'', TicketId, ResourceAllocationCount, ApproxContractValue, Closed from  ( 
   Select TicketId, [dbo].fnGetResourceAllocationCount(TicketId,''' + @TenantID + ''') as ResourceAllocationCount, ApproxContractValue, Closed  
   from Opportunity where ' + @SQLwhere

 insert into @tmpProjects    
 EXEC sp_executesql @SQLOpportunities

-- Fill CPR Tracked Work records in the temp table
SET @SQLTrackedProjects = 'select ''Tracked Projects'', TicketId, ResourceAllocationCount, ApproxContractValue, Closed from  (      
   Select TicketId, [dbo].fnGetResourceAllocationCount(TicketId,''' + @TenantID + ''') as ResourceAllocationCount, ApproxContractValue, Closed  
   from CRMProject   
   where StageStep < ' + CAST(@rewardedStage as varchar(2)) + ' and ' + @SQLwhere

 insert into @tmpProjects    
 EXEC sp_executesql @SQLTrackedProjects


-- Fill CPR Construction Projects records in the temp table
SET @SQLConstruction = 'select ''Construction'', TicketId, ResourceAllocationCount, ApproxContractValue, Closed from  (      
   Select TicketId, [dbo].fnGetResourceAllocationCount(TicketId,''' + @TenantID + ''') as ResourceAllocationCount, ApproxContractValue, Closed  
   from CRMProject   
   where StageStep >= ' + CAST(@rewardedStage as varchar(2)) + ' and ' + @SQLwhere

 insert into @tmpProjects    
 EXEC sp_executesql @SQLConstruction
  
-- select @SQLOpportunities [Opp], @SQLTrackedProjects [TP], @SQLConstruction [Const], @SQLwhere [where]

 Select Status, count(*) as [No_of_projects] from @tmpProjects  
 where Closed != 1
 Group By Status  
 having count(*) > 0
 
	IF @closed = 1
	BEGIN
		SELECT * FROM @tmpProjects;
	END
	ELSE
	BEGIN
		SELECT * FROM @tmpProjects WHERE Closed != 1;
	END;
 
end  
