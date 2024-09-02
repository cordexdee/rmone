 
Create view [dbo].[vw_ResourceUsageSummaryWeekWise] as
--select ID,Title,WorkItemType,WorkItem,SubWorkItem,ResourceUser,ManagerLookup,FunctionalAreaTitleLookup,IsManager,IsIT,IsConsultant,WeekStartDate,PctAllocation,AllocationHour,PctActual,ActualHour,TenantID,WorkItemID
select * from ResourceUsageSummaryWeekWise readonly WITH (NOLOCK)   

 