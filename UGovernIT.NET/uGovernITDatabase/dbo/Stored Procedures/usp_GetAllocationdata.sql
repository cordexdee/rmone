
ALTER Procedure [dbo].[usp_GetAllocationdata]  
--usp_GetAllocationdata '35525396-e5fe-4692-9239-4df9305b915b' , 'CPR,OPM,CNS','2021-01-01 00:00:00.000','2024-01-01 00:00:00.000',0 
@tenantid varchar(128),  
@ModuleNames varchar(max),  
@Fromdate datetime,  
@Todate datetime,  
@Isclosed bit
AS  
BEGIN  
	IF(@Isclosed=0)  
	Begin  
		CREATE TABLE #spResult  
		(  
		   ID varchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL  
		);  
		Insert into #spResult exec usp_GetClosedTickets @tenantid , @ModuleNames;  
		Select   [ID],[MonthStartDate],[PctAllocation],[PctPlannedAllocation],  
		[ResourceUser],[SubWorkItem],[WorkItem],  
		CAST([WorkItemID] as varchar)[ResourceWorkItemLookup],[WorkItemType],[Title]  
		,[TenantID],[Created],[Modified],[CreatedByUser],[ModifiedByUser]  
		,[ActualStartDate],[ActualEndDate],[ActualPctAllocation],[SoftAllocation] 
		 from ResourceUsageSummaryMonthWise where TenantID=@tenantid  
		and CONVERT(datetime, MonthStartDate,121)>= CONVERT(datetime, @Fromdate,121)  and CONVERT(datetime, MonthStartDate,121)<= CONVERT(datetime, @Todate,121)   
		and [WorkItem] not in (select ID from #spResult)   
		Drop table #spResult  
	END  
	ELSE 
	BEGIN  
		Select [ID],[MonthStartDate],[PctAllocation],[PctPlannedAllocation],  
		[ResourceUser],[SubWorkItem],[WorkItem],  
		CAST([WorkItemID] as varchar)[ResourceWorkItemLookup],[WorkItemType],[Title],  
		[TenantID],[Created],[Modified],[CreatedByUser],[ModifiedByUser] ,[ActualStartDate],[ActualEndDate],[ActualPctAllocation],[SoftAllocation] 
		from ResourceUsageSummaryMonthWise where TenantID=@tenantid  
		and CONVERT(datetime, MonthStartDate,121)>=   CONVERT(datetime, @Fromdate,121)  
		and CONVERT(datetime, MonthStartDate,121)<= CONVERT(datetime, @Todate,121)   
	END  
END
