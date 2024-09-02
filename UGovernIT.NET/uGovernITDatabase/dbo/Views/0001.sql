 
Create view [dbo].[vDashboardSummary] as
--select ID,Title,ModuleNameLookup,GenericStatusLookup,Status,TicketId,InitiatorUser,RequestorUser,OwnerUser,PRPGroupUser,PRPUser,ORPUser,ActualHours,PriorityLookup,RequestTypeLookup,InitiatorResolvedChoice,OnHold,CreationDate,RequestSourceChoice,SLAMet,Category,WorkflowType,StageActionUsersUser,FunctionalAreaLookup,LocationLookup,State,Country,Region,RequestorCompany,RequestorDivision,RequestorDepartment,Closed,ServiceName,ServiceCategoryName,SubCategory,AssignmentSLAMet,RequestorContactSLAMet,ResolutionSLAMet,CloseSLAMet,OtherSLAMet,ALLSLAsMet,ResolutionTypeChoice,Rejected,TotalHoldDuration,OnHoldTillDate,ClosedByUser,ResolvedByUser,SLADisabled,TenantID,Age
select * from DashboardSummary readonly WITH (NOLOCK)   

GO
Create view [dbo].[vResourceUsageSummaryMonthWise] as
--select ID,Title,WorkItemType,WorkItem,SubWorkItem,ResourceUser,ManagerLookup,FunctionalAreaTitleLookup,IsManager,IsIT,IsConsultant,MonthStartDate,PctAllocation,AllocationHour,PctActual,ActualHour,TenantID,WorkItemID
select *
from ResourceUsageSummaryMonthWise readonly WITH (NOLOCK)   

GO

Create view [dbo].[vResourceUsageSummaryWeekWise] as
--select ID,Title,WorkItemType,WorkItem,SubWorkItem,ResourceUser,ManagerLookup,FunctionalAreaTitleLookup,IsManager,IsIT,IsConsultant,WeekStartDate,PctAllocation,AllocationHour,PctActual,ActualHour,TenantID,WorkItemID
select *
from ResourceUsageSummaryWeekWise readonly WITH (NOLOCK)   

GO

Create view [dbo].[vSprintSummary] as
--select ID,Title,PMMIdLookup,Description,ItemOrder,StartDate,EndDate,RemainingHours,TaskEstimatedHours,SprintLookup,PercentComplete,TenantID,Created

select * from SprintSummary readonly WITH (NOLOCK)   

