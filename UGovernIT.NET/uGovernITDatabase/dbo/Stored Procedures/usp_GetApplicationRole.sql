CREATE Procedure usp_GetApplicationRole  
--usp_GetApplicationRole 'c345e784-aa08-420f-b11f-2753bbebfdd5','31'  
@TenantId varchar(200),  
@ApplicationId varchar(20)  
as  
begin  
Select ROW_NUMBER() OVER(ORDER BY a.ItemOrder ASC) AS ItemOrder, a.* ,
Isnull([dbo].[fnGetApplicationModules](a.ApplicationRoleModuleLookup,@TenantId),'All') ApplicationRoleModuleLookupName, 
c.Title as APPTitleLookupName   from ApplicationRole a   
left join Applications c on a.APPTitleLookup=c.ID  
where a.TenantID=@TenantId and a.APPTitleLookup= CAST(@ApplicationId as int)  
end