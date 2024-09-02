CREATE PROCEDURE [dbo].[GetRoleWiseAllocations]
(
@TenantId nvarchar(128)
)
AS
BEGIN
	select top 5 RoleId, (select Name from Roles where Id=RoleId)as RoleName,
count(ResourceUser) as Capacity, Round(avg(pctallocation),1) as Utilization from ResourceAllocation where DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) <= AllocationEndDate 
and DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) >= AllocationStartDate and TenantID = @TenantId
group by RoleId having RoleId is not null
--order by count(ResourceUser) desc
union
	select '00000000-0000-0000-0000-000000000000', 'Overall' as RoleName,
count(ResourceUser) as Capacity, ISNULL(Round(avg(pctallocation),1), 0 ) as Utilization from ResourceAllocation where DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) <= AllocationEndDate 
and DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0) >= AllocationStartDate and TenantID = @TenantId
and RoleId is not null
order by count(ResourceUser) desc
End