    
CREATE Procedure [dbo].[usp_Getdistinctmanageruser]      
@TenantID varchar(max)      
as      
Begin      
/*    
select distinct ManagerLookup, [dbo].[fnGetusername](a.ManagerLookup,@TenantID)[ManagerLookup$]     
 from ResourceUsageSummaryMonthWise (Readcommitted) a      
 where a.TenantID=@TenantID        
 */    
    
select distinct ManagerUser as ManagerLookup, (select name from AspNetUsers where id = u1.ManagerUser) as [ManagerLookup$] from AspNetUsers u1    
where TenantID=@TenantID and u1.Enabled = 1    
and LEN(ManagerUser) > 0     
order by [ManagerLookup$]    
    
End