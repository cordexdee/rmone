
/*
0. Fetch all data as backup
1. Run select query that will fetch all corrupt data.
2. Run Delete query that will delete corupt data.
3. Run Update query that will modify start date and enddate of Resource Allocation.
4. Rebuild Summary data
*/

Declare @TenantID varchar(128)='35525396-e5fe-4692-9239-4df9305b915b'

-- Start>> 1. Run select query that will fetch all corrupt data.

--ResourceUsageSummaryWeekWise
select * from ResourceUsageSummaryWeekWise where TenantID=@TenantID
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID)
union
select * from ResourceUsageSummaryWeekWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid))

--ResourceUsageSummaryMonthWise 
select *  from ResourceUsageSummaryMonthWise where TenantID=@TenantID and
WorkItemID in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID
)
union
select *  from ResourceUsageSummaryMonthWise where TenantID=@TenantID and
WorkItemID in (
select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid)
)

--ResourceAllocationMonthly table
select *  from ResourceAllocationMonthly where TenantID=@TenantID
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID)
union
select *  from ResourceAllocationMonthly where TenantID=@TenantID 
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid))

--Extra data in ResourceAllocation which are not present on ProjectEstimatedAllocation table
select * from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID
union
select * from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid)

--Extra data in ProjectEstimatedAllocation which are not present on ResourceAllocation table
select * from ProjectEstimatedAllocation 
where id not in 
(select ISNULL(ProjectEstimatedAllocationId, 0) from ResourceAllocation where TenantID = @TenantID) 
and TenantID = @TenantID

-- Qurey to fetch mismatch start and end date in ProjectEstimatedAllocation and  ResourceAllocation table
select p.* from ProjectEstimatedAllocation p inner join ResourceAllocation r on p.ID = r.ProjectEstimatedAllocationId
where 
(p.AllocationStartDate != r.AllocationStartDate or p.AllocationEndDate != r.AllocationEndDate)
and r.ID not in 
	(
	select id from ResourceAllocation where  ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
		(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
		and TenantID = @TenantID
	union
	select id from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
		FROM ResourceAllocation where TenantID=@TenantID   
		GROUP BY ProjectEstimatedAllocationid)
	)

-- END >> 1. Run select query that will fetch all corrupt data.


-- Start >> 2. Run Delete query that will delete corupt data.

--1
delete from ResourceUsageSummaryWeekWise where TenantID=@TenantID 
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid))

--2
delete  from ResourceUsageSummaryMonthWise where TenantID=@TenantID and
WorkItemID in (
select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid)
)

--3
delete  from ResourceAllocationMonthly where TenantID=@TenantID 
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid))

--4
delete from ResourceAllocation where TenantID=@TenantID and ProjectEstimatedAllocationId is not null and ResourceWorkItemLookup  not in (SELECT min(ResourceWorkItemLookup)
FROM ResourceAllocation where TenantID=@TenantID   
GROUP BY ProjectEstimatedAllocationid)


--ResourceUsageSummaryWeekWise
delete from ResourceUsageSummaryWeekWise where TenantID=@TenantID
and WorkItemID  in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID)

--ResourceUsageSummaryMonthWise 
delete  from ResourceUsageSummaryMonthWise where TenantID=@TenantID and
WorkItemID in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID
)

--ResourceAllocationMonthly table
delete  from ResourceAllocationMonthly where TenantID=@TenantID
and ResourceWorkItemLookup  in (select ResourceWorkItemLookup from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID)

--Extra data in ResourceAllocation which are not present on ProjectEstimatedAllocation table
delete from ResourceAllocation 
where ((ProjectEstimatedAllocationId is null and (TicketId like 'CPR%' or TicketId like 'CNS%' or TicketId like 'OPM%')) or (ProjectEstimatedAllocationId not in 
(select ISNULL(Id, 0) from ProjectEstimatedAllocation where TenantID = @TenantID))) 
and TenantID = @TenantID

--Extra data in ProjectEstimatedAllocation which are not present on ResourceAllocation table
delete from ProjectEstimatedAllocation 
where id not in 
(select ISNULL(ProjectEstimatedAllocationId, 0) from ResourceAllocation where TenantID = @TenantID) 
and TenantID = @TenantID


-- END >> 2. Run Delete query that will delete corupt data.

-- Start >> 3. Run Update query that will modify start date and enddate of Resource Allocation.

update ResourceAllocation
set AllocationStartDate = (select AllocationStartDate from ProjectEstimatedAllocation where id = ProjectEstimatedAllocationId),
	AllocationEndDate = (select AllocationEndDate from ProjectEstimatedAllocation where id = ProjectEstimatedAllocationId),
	EstStartDate = (select AllocationStartDate from ProjectEstimatedAllocation where id = ProjectEstimatedAllocationId),
	EstEndDate = (select AllocationEndDate from ProjectEstimatedAllocation where id = ProjectEstimatedAllocationId)
where id in (select r.ID from ProjectEstimatedAllocation p inner join ResourceAllocation r on p.ID = r.ProjectEstimatedAllocationId
				where (p.AllocationStartDate != r.AllocationStartDate or p.AllocationEndDate != r.AllocationEndDate)
				)

-- END >> 3. Run Update query that will modify start date and enddate of Resource Allocation.

