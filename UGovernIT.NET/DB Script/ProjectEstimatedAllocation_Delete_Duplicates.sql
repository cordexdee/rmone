
/*
1. Run select query from ProjectEstimatedAllocation.
2. Run Delete query that will delete corupt data.
*/

--1. Run select query from ProjectEstimatedAllocation.

Declare @TenantID varchar(128)='35525396-e5fe-4692-9239-4df9305b915b'

select * from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')

select * from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000'))

select *  from ResourceAllocationMonthly where TenantID=@TenantID 
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

select *  from ResourceUsageSummaryMonthWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

select *  from ResourceUsageSummaryWeekWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

--2. Run Delete query that will delete corupt data.

delete from ResourceAllocationMonthly where TenantID=@TenantID 
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

delete from ResourceUsageSummaryMonthWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

delete from ResourceUsageSummaryWeekWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')))

delete from ResourceAllocation where ProjectEstimatedAllocationId in (select ID from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000'))

delete from ProjectEstimatedAllocation where TenantID=@TenantID and id in (select MAX(id) from ProjectEstimatedAllocation 
group by TicketId,AssignedToUser,Type, AllocationStartDate,AllocationEndDate HAVING 
    COUNT(*) > 1 and AssignedToUser != '00000000-0000-0000-0000-000000000000')