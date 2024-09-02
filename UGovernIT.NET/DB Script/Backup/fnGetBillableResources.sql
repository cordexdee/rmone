ALTER FUNCTION [dbo].[fnGetBillableResources]        
(  
@TenantID varchar(128)        
)        
RETURNS TABLE        
AS        
RETURN        
--Select u.Id,u.Name,u.JobProfile,(Case when j.JobType is null then 'Overhead' else 'Billable' end)JobType , GlobalRoleID,Enabled       
--from AspNetUsers u left join JobTitle j on j.ID=u.JobTitleLookup        
--where u.TenantID=@TenantID and j.TenantID=@TenantID   
  
Select distinct u.Id,u.Name,u.JobProfile, JobType JobType, GlobalRoleID,Enabled, b.EmpCostRate,r.BillingRate BillingRate , j.Deleted 
from AspNetUsers u left join JobTitle j on j.ID=u.JobTitleLookup    
left join JobTitleCostRateByDept b  on b.JobtitleLookup= u.JobtitleLookup  
and b.DeptLookup=u.DepartmentLookup  and b.Deleted=0
left join RoleBillingRateByDept r on r.DepartmentLookup=u.DepartmentLookup
and r.RoleLookup=u.GlobalRoleID and r.Deleted=0
where u.TenantID=@TenantID and j.TenantID=@TenantID   
  