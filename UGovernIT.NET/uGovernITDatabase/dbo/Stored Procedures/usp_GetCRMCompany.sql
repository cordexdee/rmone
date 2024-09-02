CREATE Procedure [dbo].[usp_GetCRMCompany]        
--[usp_GetCRMCompany]  '35525396-e5fe-4692-9239-4df9305b915b'      
@TenantID VARCHAR(MAX),        
@IsClosed char(1)='',        
@TicketId VARCHAR(MAX)=''         
as        
Begin        
select  a.*,    
c.Title[CategoryLookup$],    
aa.TicketId[CompanyLookup$],    
d.Title[ContactLookup$],    
de.Title[DepartmentLookup$],    
d.Title[FunctionalAreaLookup$],    
i.Title[ImpactLookup$],    
f.StageTitle[ModuleStepLookup$],    
p.Title[PriorityLookup$],        
m.Title[RelationshipTypeLookup$],    
h.requesttype[RequestTypeLookup$],    
css.Title[ServiceLookUp$] ,    
s.Title[StateLookup$],    
[dbo].[fnGetusername](a.StageActionUsersUser,@TenantID)[StageActionUsersUser$],      
[dbo].[fnGetusername](a.InitiatorUser,@TenantID)[InitiatorUser$],      
[dbo].[fnGetusername](a.ORPUser,@TenantID)[ORPUser$],      
[dbo].[fnGetusername](a.OwnerUser,@TenantID)[OwnerUser$],      
[dbo].[fnGetusername](a.PRPUser,@TenantID)[PRPUser$],      
[dbo].[fnGetusername](a.PRPGroupUser,@TenantID)[PRPGroupUser$],      
[dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$],      
[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$]    
from  CRMCompany a left join CRMContact (READCOMMITTED) d on Cast(isnull(d.ID,'') as varchar)= isnull(a.ContactLookup ,'')        
left join CRMRelationshipType (READCOMMITTED) m on Cast(isnull(m.ID,'') as varchar)= isnull(a.RelationshipTypeLookup,'') and m.TenantID=@TenantID        
left join Config_Module_RequestType (READCOMMITTED) c on isnull(c.ID,'')= isnull(a.CategoryLookup,'') and c.TenantID=@TenantID        
left join Config_Module_ModuleStages (READCOMMITTED) f on isnull(f.ID,'')= isnull(a.ModuleStepLookup,'') and f.TenantID=@TenantID        
left join Config_Module_RequestType (READCOMMITTED) h on isnull(h.ID,'')= isnull(a.RequestTypeLookup,'') and h.TenantID=@TenantID         
left join Config_Module_Priority (READCOMMITTED) p on isnull(p.ID,'')= isnull(a.PriorityLookup,'') and p.TenantID=@TenantID       
left join FunctionalAreas (READCOMMITTED) fa on Cast(isnull(fa.ID,'') as varchar)= isnull(a.FunctionalAreaLookup ,'') and fa.TenantID=@TenantID      
left join Config_Module_Impact (READCOMMITTED) i on Cast(isnull(i.ID,'') as varchar)= isnull(a.ImpactLookup,'') and i.TenantID=@TenantID      
left join state (READCOMMITTED) s on Cast(isnull(s.ID,'') as varchar)= isnull(a.StateLookup,'') and s.TenantID=@TenantID      
left join Department (READCOMMITTED) de on Cast(isnull(de.ID,'') as varchar)= isnull(a.DepartmentLookup,'') and de.TenantID=@TenantID      
left join Config_Services (READCOMMITTED) css on Cast(isnull(css.ID,'') as varchar)= isnull(a.ServiceLookUp,'') and css.TenantID=@TenantID   
left join CRMCompany  (READCOMMITTED) aa on Cast(isnull(aa.ID,'') as varchar)= isnull(aa.CompanyLookup,'')        
where a.TenantID=@TenantID AND ISNULL(Cast(a.closed as char(1)),'0')= CASE WHEN LEN(@IsClosed)=0 then Isnull(Cast(a.closed as char(1)),'0') else @IsClosed END        
and (a.Deleted<>1 or a.Deleted is null)        
and  isnull(a.TicketId,'') =CASE WHEN LEN(@TicketId)=0 then isnull(a.TicketId,'')  else @TicketId END        
End 