CREATE PROCEDURE GetProjectWorkItems   
 @TenantId nvarchar(128) = '2e4d74d8-b163-4de1-8154-63cc2b8b87ef',  
 @WorkItemType nvarchar(50) = 'CPR',
 @WorkItemCategory nvarchar(50) = 'current'    --all, current
AS  
BEGIN  
 Declare @ModuleTable varchar(100);  
 Declare @Query nvarchar(max);    

 select @ModuleTable = ModuleTable from Config_Modules where TenantID = @TenantId and ModuleName = @WorkItemType;  
  
 If LEN(@WorkItemType) > 0  
  Begin  
  /*
	IF @WorkItemCategory = 'current'
	Begin
		Set @Query = 'select t.ID as workItemId, t.WorkItem as workItem, c.Title as projectTitle, c.TicketId + '' : '' + c.Title as Project , r.RequestType as projectType, ISNULL(c.EnableStdWorkItems, 0) as enableStdWorkItems,
					 t.SubWorkItem as subWorkItem, t.SubWorkItem as role , SUBSTRING(t.SubSubWorkItem,CHARINDEX('';#'',t.SubSubWorkItem)+2,LEN(t.SubSubWorkItem)) as jobCode, Convert(varchar(10), t.StartDate,120)  as startDate, Convert(varchar(10), t.EndDate,120)  as endDate
					 from '+ @ModuleTable +' c left join Config_Module_RequestType r on c.RequestTypeLookup = r.ID  
					 left join ResourceWorkItems t on t.WorkItem = c.TicketId   
					 where c.TenantID = '''+@TenantID+''' and t.Deleted = 0  and (c.Closed IS NULL or c.Closed = 0)';
	End
	Else if @WorkItemCategory = 'all'
	Begin
		Set @Query = 'select c.ID as workItemId, c.TicketId as workItem, c.Title as projectTitle, c.TicketId + '' : '' + c.Title as Project, r.RequestType as projectType, ISNULL(c.EnableStdWorkItems, 0) as enableStdWorkItems,
			'''' as subWorkItem, '''' as role , '''' as jobCode, '''' as startDate, '''' as endDate 
			from '+ @ModuleTable +' c left join Config_Module_RequestType r on c.RequestTypeLookup = r.ID  
			where c.TenantID = '''+@TenantID+''' and (c.Closed IS NULL or c.Closed = 0)';
	End
	*/

	IF @WorkItemCategory = 'current'
	Begin
		Set @Query = 'select distinct t.WorkItem as workItem, c.Title as projectTitle, c.TicketId + '' : '' + c.Title as Project , c.Status , r.RequestType as projectType, ISNULL(c.EnableStdWorkItems, 0) as enableStdWorkItems,
					 Convert(varchar(10), t.StartDate,120)  as startDate, Convert(varchar(10), t.EndDate,120)  as endDate
					 from '+ @ModuleTable +' c left join Config_Module_RequestType r on c.RequestTypeLookup = r.ID  
					 left join ResourceWorkItems t on t.WorkItem = c.TicketId   
					 where c.TenantID = '''+@TenantID+''' and t.Deleted = 0  and (c.Closed IS NULL or c.Closed = 0)
					 order by t.WorkItem';
	End
	Else if @WorkItemCategory = 'all'
	Begin
		Set @Query = 'select distinct c.TicketId as workItem, c.Title as projectTitle, c.TicketId + '' : '' + c.Title as Project, c.Status , r.RequestType as projectType, ISNULL(c.EnableStdWorkItems, 0) as enableStdWorkItems,
			'''' as startDate, '''' as endDate 
			from '+ @ModuleTable +' c left join Config_Module_RequestType r on c.RequestTypeLookup = r.ID  
			where c.TenantID = '''+@TenantID+''' and (c.Closed IS NULL or c.Closed = 0)
			order by c.TicketId';
	End

   Print(@Query)  
   EXEC(@Query)  
  
  End  
  
END  
