 
Create Procedure [dbo].[usp_GetCRMServices]        
--[usp_GetCRMServices] '35525396-e5fe-4692-9239-4df9305b915b'      
@TenantID VARCHAR(128),        
@IsClosed char(1)='',        
@TicketId VARCHAR(50)=''        
as        
Begin   
Declare @Usercol varchar(max), @strsql varchar(max)
SET @Usercol = (STUFF((SELECT ', ' + '[dbo].[fnGetusername] (a.' + a.COLUMN_NAME + ','''+@TenantID+''')['+a.COLUMN_NAME+'$]'  from INFORMATION_SCHEMA.COLUMNS a        
where  a.TABLE_NAME='CRMServices' and Right(a.COLUMN_NAME,4) like '%User%' FOR XML PATH('')),1,2,''))
Set @strsql='
select  a.* ,    
bu.Title[BusinessUnitLookup$] ,    
dbo.[fnGetRequestTypeCategory](a.CategoryLookup,'''+@TenantID+''')[CategoryLookup$],     
d.Title[ContactLookup$],     
f.StageTitle[ModuleStepLookup$],    
g.Title[OPMIdLookup$],    
dbo.[fnGetRequestType](a.RequestTypeLookup,'''+@TenantID+''')[RequestTypeLookup$],  
s.Title[StateLookup$],    
req.SubCategory[SubCategoryLookup$],    
b.Title[CRMCompanyLookup$],    
su.Title[SubstatusLookup$],  
cd.Title[DivisionLookup$],
st.Description[StudioLookup$],
[dbo].fnGetResourceAllocationCount(a.TicketId,'''+@TenantID+''') as ResourceAllocationCount$ ,'+ @Usercol+'
from CRMServices (READCOMMITTED) a         
left join CRMCompany (READCOMMITTED) b on isnull(b.TicketId,'''')= isnull(a.CRMCompanyLookup,'''') and  b.TenantID='''+@TenantID+'''      
left join CRMContact (READCOMMITTED) d on Cast(isnull(d.TicketId,'''') as varchar)= isnull(a.ContactLookup ,'''')and d.TenantID='''+@TenantID+'''     
left join Opportunity (READCOMMITTED) g on g.TicketId= a.OPMIdLookup and g.TenantID='''+@TenantID+'''     
left join Config_Module_ModuleStages (READCOMMITTED) f on f.ID= isnull(a.ModuleStepLookup,'''') and f.TenantID='''+@TenantID+'''  
left join BusinessUnits (READCOMMITTED) bu on bu.id = isnull(a.BusinessUnitLookup,'''') and bu.TenantID='''+@TenantID+'''  
left join state (READCOMMITTED) s on Cast(isnull(s.ID,'''') as varchar)= isnull(a.StateLookup,'''') and s.TenantID='''+@TenantID+'''  
left join Config_Module_RequestType (READCOMMITTED) req on Cast(isnull(req.id,'''') as varchar)= isnull(a.SubCategoryLookup,'''') and req.TenantID='''+@TenantID+'''         
left join Substatus (READCOMMITTED) su on isnull(su.ID,0)= isnull(a.SubstatusLookup,0) and su.TenantID='''+@TenantID+'''
left join CompanyDivisions (READCOMMITTED) cd on cd.ID= isnull(a.DivisionLookup,0) and cd.TenantID='''+@TenantID+'''   
left join Studio (READCOMMITTED) st on st.ID= isnull(a.StudioLookup,0) and st.TenantID='''+@TenantID+'''   
where a.TenantID='''+@TenantID+'''    AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''     END        
and (a.Deleted<>1 or a.Deleted is null)        
and  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'       
--print(@strsql)
exec(@strsql)
End  