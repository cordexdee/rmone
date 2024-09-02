 
CREATE Procedure [dbo].[usp_GetCRMProject]         
--[usp_GetCRMProject] '35525396-e5fe-4692-9239-4df9305b915b'         
@TenantID VARCHAR(128),            
@IsClosed char(1)='',            
@TicketId VARCHAR(50)=''            
as            
Begin       
Declare @Usercol varchar(max), @strsql varchar(max)    
SET @Usercol = (STUFF((SELECT ', ' + '[dbo].[fnGetusername] (a.' + a.COLUMN_NAME + ','''+@TenantID+''')['+a.COLUMN_NAME+'$]'  from INFORMATION_SCHEMA.COLUMNS a            
where  a.TABLE_NAME='CRMProject' and Right(a.COLUMN_NAME,4) like '%User%' FOR XML PATH('')),1,2,''))    
    
    
Set @strsql='    
select  a.*,       
b.Title[CRMCompanyLookup$],c.Category[CategoryLookup$],d.Title[ContactLookup$], f.StageTitle[ModuleStepLookup$],      
bu.Title[BusinessUnitLookup$],        
c.Category[CategoryLookup$],      
d.Title[ContactLookup$],       
f.StageTitle[ModuleStepLookup$] ,      
a.OPMIdLookup[OPMIdLookup$],      
h.requesttype[RequestTypeLookup$],      
s.Title[StateLookup$],        
ss.Title[SubstatusLookup$] ,      
--l.Title[LEMIdLookup$],      
b.Title[CRMCompanyLookup$], 
cd.Title[DivisionLookup$],
st.Description[StudioLookup$],
[dbo].fnGetResourceAllocationCount(a.TicketId, '''+@TenantID+''') as ResourceAllocationCount$ , ' + @Usercol +'    
    
from CRMProject (READCOMMITTED) a             
left join CRMCompany (READCOMMITTED) b on b.TicketId = isnull(a.CRMCompanyLookup,'''') and b.TenantID='''+@TenantID+'''            
left join CRMContact (READCOMMITTED) d on d.TicketId = isnull(a.ContactLookup ,'''') and d.TenantID='''+@TenantID+'''            
--left join Opportunity (READCOMMITTED) g on g.TicketId= a.OPMIdLookup and g.TenantID='''+@TenantID+'''          
--left join Lead l (READCOMMITTED) on l.TicketId= a.OPMIdLookup and l.TenantID='''+@TenantID+'''    
left join Config_Module_RequestType (READCOMMITTED) c on c.ID = isnull(a.CategoryLookup,'''') and c.TenantID='''+@TenantID+'''           
left join Config_Module_ModuleStages (READCOMMITTED) f on f.ID = isnull(a.ModuleStepLookup,'''') and f.TenantID='''+@TenantID+'''         
left join BusinessUnits (READCOMMITTED) bu on bu.id= isnull(a.BusinessUnitLookup,'''') and bu.TenantID='''+@TenantID+'''      
left join Config_Module_RequestType (READCOMMITTED) h on h.ID = isnull(a.RequestTypeLookup,'''') and h.TenantID='''+@TenantID+'''    
left join state (READCOMMITTED) s on s.ID= isnull(a.StateLookup,'''') and s.TenantID='''+@TenantID+'''    
left join Substatus (READCOMMITTED) ss on ss.ID= isnull(a.SubstatusLookup,0) and ss.TenantID='''+@TenantID+'''   
left join CompanyDivisions (READCOMMITTED) cd on cd.ID= isnull(a.DivisionLookup,0) and cd.TenantID='''+@TenantID+'''   
left join Studio (READCOMMITTED) st on st.ID= isnull(a.StudioLookup,0) and st.TenantID='''+@TenantID+'''   
     
where a.TenantID='''+@TenantID+'''            
And a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+''' END and (a.Deleted<>1 or a.Deleted is null)            
and  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'    
    
--Print(@strsql)    
Exec(@strsql)    
End    