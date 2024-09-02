--Step 1
Alter table ResourceAllocationMonthly  add GlobalRoleID nvarchar(128) NULL
Alter table ResourceUsageSummaryMonthWise  add GlobalRoleID nvarchar(128) NULL
Alter table ResourceUsageSummaryWeekWise  add GlobalRoleID nvarchar(128) NULL

-- Step 2
Declare @tenantID nvarchar(128) = 'BCD2D0C9-9947-4A0B-9FBF-73EA61035069'

update rm set rm.GlobalRoleID = r.Id from ResourceAllocationMonthly RM join Roles R on RM.ResourceSubWorkItem = R.Name where RM.TenantID = @tenantID and R.TenantID=@tenantID

update rm set rm.GlobalRoleID = r.Id from ResourceUsageSummaryMonthWise RM join Roles R on RM.SubWorkItem = R.Name where RM.TenantID = @tenantID and R.TenantID=@tenantID

update rm set rm.GlobalRoleID = r.Id from ResourceUsageSummaryWeekWise RM join Roles R on RM.SubWorkItem = R.Name where RM.TenantID = @tenantID and R.TenantID=@tenantID