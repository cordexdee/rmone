    
ALTER procedure [dbo].[usp_GetJobTitle]     
-- exec usp_GetJobTitle '35525396-E5FE-4692-9239-4DF9305B915B',0,'',0,0,0,1    
@TenantID varchar(128),@deptid int =0,      
@roleid varchar(128)='',      
@JobTitleId int =0,     
@EmpCostRate float=0,    
@Deleted bit =0,    
@IsCascading bit = 0    
as              
begin         
 if(@JobTitleId=0)      
 Begin      
  Select j.ID,  j.Title,           
  j.ShortName,            
  j.LowRevenueCapacity,            
  j.HighRevenueCapacity,            
  j.LowProjectCapacity,            
  j.HighProjectCapacity,            
  j.ResourceLevelTolerance,            
  j.Deleted, r.Name RoleName, r.Id RoleId,          
  Case when j.JobType='Billable' then 'Billable' else 'Overhead' end JobType               
  from JobTitle j    
  Left Join Roles r on r.Id=j.RoleId and r.TenantID=@TenantID    
  where j.TenantID=@TenantID    order by j.Title             
         
  Select case when isnull(b.DivisionIdLookup,0)>0 then cd.Title+' > '+b.Title else b.Title end as DepartmentName,    
  a.JobTitleLookup,a.DeptLookup, b.Title as DepartmentName, cd.Title as DivisionName,         
    a.EmpCostRate,a.Id,a.Deleted from JobTitleCostRateByDept a            
   left join Department b on a.DeptLookup=b.ID            
   left join CompanyDivisions cd on cd.ID=DivisionIdLookup         
   where a.TenantID=@TenantID            
   and b.TenantID=@TenantID      
   and cd.TenantID=@TenantID    
         
 End      
 Else     
 Begin      
  Select b.Title as DepartmentName,a.JobTitleLookup,a.DeptLookup,       
    a.EmpCostRate,a.Id,a.Deleted from JobTitleCostRateByDept a            
   left join Department b on a.DeptLookup=b.ID            
            
   where a.TenantID=@TenantID            
   and b.TenantID=@TenantID       
        
   and a.DeptLookup=@deptid      
   and a.JobtitleLookup=@JobTitleId and a.EmpCostRate=@EmpCostRate    
   and a.Deleted=@Deleted    
 End    
     
 IF (@IsCascading = 1)    
 Begin    
   Select * from RoleBillingRateByDept where DepartmentLookup in (    
    Select DeptLookup from JobTitleCostRateByDept     
    where JobTitleLookup in (select ID from  JobTitle)    
    )    
    
 End;    
end    