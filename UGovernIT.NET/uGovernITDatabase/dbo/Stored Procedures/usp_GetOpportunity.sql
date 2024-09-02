
Declare @Usercol varchar(max), @strsql varchar(max)
SET @Usercol = (STUFF((SELECT ', ' + '[dbo].[fnGetusername] (a.' + a.COLUMN_NAME + ','''+@TenantID+''')['+a.COLUMN_NAME+'$]'  from INFORMATION_SCHEMA.COLUMNS a        
where  a.TABLE_NAME='Opportunity' and Right(a.COLUMN_NAME,4) like '%User%' FOR XML PATH('')),1,2,''))
Set @strsql='
select   a.*,   
cr1.Title[AdditionalRecipientsLookup$]  ,  
bu.Title[BusinessUnitLookup$],  
dbo.[fnGetRequestTypeCategory](a.CategoryLookup,'''+@TenantID+''')[CategoryLookup$],   
cr.Title[ContactLookup$],  
de.Title[DepartmentLookup$],  
cd.Title[DivisionLookup$],  
d.Title[FunctionalAreaLookup$],  
i.Title[ImpactLookup$],  
l.Title[LEMIdLookup$],  
f.StageTitle[ModuleStepLookup$],  
cr0.Title[ProposalRecipientLookup$],  
dbo.[fnGetRequestType](a.RequestTypeLookup,'''+@TenantID+''')[RequestTypeLookup$],  
css.Title[ServiceLookUp$],    
s.Title[StateLookup$],  
req.SubCategory[SubCategoryLookup$],    
p.Title[PriorityLookup$],  
crm.Title[CRMCompanyLookup$],
st.Description[StudioLookup$],
[dbo].fnGetResourceAllocationCount(a.TicketId,'''+@TenantID+''') as ResourceAllocationCount$ , '+ @Usercol + '
from  Opportunity a   
left join Config_Module_ModuleStages (READCOMMITTED) f on f.ID= isnull(a.ModuleStepLookup,'''') and f.TenantID='''+@TenantID+'''  
left join Config_Module_Priority (READCOMMITTED) p on p.ID = isnull(a.PriorityLookup,'''') and p.TenantID='''+@TenantID+'''     
left join FunctionalAreas (READCOMMITTED) d on d.ID= isnull(a.FunctionalAreaLookup ,'''') and d.TenantID='''+@TenantID+''' 
left join Config_Module_Impact (READCOMMITTED) i on i.ID= isnull(a.ImpactLookup,'''') and i.TenantID='''+@TenantID+'''
left join state (READCOMMITTED) s on s.ID = isnull(a.StateLookup,'''') and s.TenantID='''+@TenantID+'''
left join CRMContact (READCOMMITTED) cr on convert(varchar, cr.ID)= isnull(a.ContactLookup,'''') and cr.TenantID='''+@TenantID+'''
left join Department (READCOMMITTED) de on de.ID= isnull(a.DepartmentLookup,'''') and de.TenantID='''+@TenantID+'''   
left join Config_Services (READCOMMITTED) css on Cast(isnull(css.ID,'''') as varchar)= isnull(a.ServiceLookUp,'''') and css.TenantID='''+@TenantID+'''
left join CRMCompany (READCOMMITTED) crm on Cast(isnull(crm.TicketId,'''') as varchar)= isnull(a.CRMCompanyLookup,'''') and crm.TenantID='''+@TenantID+''' 
left join CompanyDivisions (READCOMMITTED) cd on cd.id= isnull(a.DivisionLookup,'''') and cd.TenantID='''+@TenantID+'''
left join BusinessUnits (READCOMMITTED) bu on bu.id= isnull(a.BusinessUnitLookup,'''') and bu.TenantID='''+@TenantID+'''
left join Config_Module_RequestType (READCOMMITTED) req on Cast(isnull(req.id,'''') as varchar)= isnull(a.SubCategoryLookup,'''') and req.TenantID='''+@TenantID+'''
left join CRMContact (READCOMMITTED) cr0 on convert(varchar, cr0.ID)= isnull(a.ProposalRecipientLookup,'''') and cr0.TenantID='''+@TenantID+'''   
left join CRMContact (READCOMMITTED) cr1 on convert(varchar, cr1.ID)= isnull(a.AdditionalRecipientsLookup,'''') and cr1.TenantID='''+@TenantID+'''  
left join Lead l (READCOMMITTED) on l.TicketId= isnull(a.LEMIdLookup,0) and l.TenantID='''+@TenantID+'''    
left join Studio (READCOMMITTED) st on st.ID= isnull(a.StudioLookup,0) and st.TenantID='''+@TenantID+'''   
where a.TenantID='''+@TenantID+''' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END      
and (a.Deleted<>1 or a.Deleted is null)      
and  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'     
--print(@strsql)
exec(@strsql)
End    