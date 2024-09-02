CREATE procedure usp_GetConfig_Dashboard_DashboardPanels      
@TenantID varchar(max)      
As Begin      
select a.*,[dbo].[fnGetusername](a.DashboardPermissionUser,@TenantID)[DashboardPermissionUser$],    
[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$],     
(Case When a.DashboardType=0 then 'Panel' When a.DashboardType=1 then 'Chart' When a.DashboardType=2 then 'Query' else '' end ) [DashboardType$],    
[dbo].[fnGetModuleName](a.DashboardModuleMultiLookup)[DashboardModuleMultiLookup$] from  Config_Dashboard_DashboardPanels a        
where a.TenantID=@TenantID      
END 