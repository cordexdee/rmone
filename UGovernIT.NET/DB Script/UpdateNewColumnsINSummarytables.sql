--select MonthStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceAllocationMonthly
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and ResourceWorkItemLookup=25956
--select MonthStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceAllocationMonthly
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and ResourceWorkItemLookup=39696

--select MonthStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceUsageSummaryMonthWise
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and WorkItemID=25956
--select MonthStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceUsageSummaryMonthWise
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and WorkItemID=39696

--select WeekStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceUsageSummaryWeekWise
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and WorkItemID=25956
--select WeekStartDate, actualstartdate, actualenddate, actualpctallocation, PctAllocation, * from ResourceUsageSummaryWeekWise
--where ResourceUser='bb2e0081-4b7d-4aa8-9506-2a13a484e68b' and WorkItemID=39696


alter table ResourceAllocationMonthly add ActualStartDate Datetime null
alter table ResourceAllocationMonthly add ActualEndDate Datetime null
alter table ResourceAllocationMonthly add ActualPctAllocation float null

alter table ResourceUsageSummaryMonthWise add ActualStartDate Datetime null
alter table ResourceUsageSummaryMonthWise add ActualEndDate Datetime null
alter table ResourceUsageSummaryMonthWise add ActualPctAllocation float null

alter table ResourceUsageSummaryWeekWise add ActualStartDate Datetime null
alter table ResourceUsageSummaryWeekWise add ActualEndDate Datetime null
alter table ResourceUsageSummaryWeekWise add ActualPctAllocation float null



UPDATE ResourceAllocationMonthly
SET ActualStartDate = ResourceAllocation.AllocationStartDate,
    ActualEndDate = ResourceAllocation.AllocationEndDate,
	ActualPctAllocation = ResourceAllocation.PctAllocation
FROM ResourceAllocationMonthly
INNER JOIN ResourceAllocation ON ResourceAllocationMonthly.ResourceWorkItemLookup = ResourceAllocation.ResourceWorkItemLookup

UPDATE ResourceUsageSummaryMonthWise
SET ActualStartDate = ResourceAllocation.AllocationStartDate,
    ActualEndDate = ResourceAllocation.AllocationEndDate,
	ActualPctAllocation = ResourceAllocation.PctAllocation
FROM ResourceUsageSummaryMonthWise
INNER JOIN ResourceAllocation ON ResourceUsageSummaryMonthWise.WorkItemID = ResourceAllocation.ResourceWorkItemLookup


UPDATE ResourceUsageSummaryWeekWise
SET ActualStartDate = ResourceAllocation.AllocationStartDate,
    ActualEndDate = ResourceAllocation.AllocationEndDate,
	ActualPctAllocation = ResourceAllocation.PctAllocation
FROM ResourceUsageSummaryWeekWise
INNER JOIN ResourceAllocation ON ResourceUsageSummaryWeekWise.WorkItemID = ResourceAllocation.ResourceWorkItemLookup
