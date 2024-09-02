update rw set rw.SubWorkItem = r.Name
from ResourceAllocation ra join ResourceWorkItems rw on ra.ResourceWorkItemLookup = rw.ID
left join Roles r on ra.RoleId = r.Id
where ra.TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
and Len(r.Name) > 0 
and r.Name != rw.SubWorkItem

select ra.TicketId, ra.ResourceUser, ISNULL(u.name, 'Unfilled') as Resource, ra.RoleId, r.Name, rw.SubWorkItem,
CONVERT(nvarchar, ra.AllocationStartDate, 101) as StartDate, CONVERT(nvarchar, ra.AllocationEndDate, 101) as EndDate, ra.Deleted as deletedfromallocation, 
CASE WHEN ea.Deleted = 1 THEN 'Deleted' when ea.Deleted is null then 'Deleted' ELSE 'Not Deleted' END AS deletedfromteamtab
from ResourceAllocation ra join ResourceWorkItems rw on ra.ResourceWorkItemLookup = rw.ID
left join Roles r on ra.RoleId = r.Id
left join AspNetUsers u on ra.ResourceUser = u.Id
left join ProjectEstimatedAllocation ea on ra.ProjectEstimatedAllocationId = ea.ID
where ra.TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
and r.Name != rw.SubWorkItem

