CReate Procedure usp_GetRelatedCompanies
@TenantID varchar(128),
@TicketId varchar(20)
as
begin
Select a.ID,a.ItemOrder,a.TicketId,a.ContactLookup, a.CRMCompanyLookup,d.Title 'CompanyName', a. CostCodeLookup,c.Title 'RelationshipType',
d.StreetAddress1 +'<br/>'+ isnull(d.City,'') +'<br/>'+ISNULL(s.Title,'') +'<br/>'+ ISNULL(d.Zip,'') Address
from RelatedCompanies a 
left join crmcontact b on a.TicketId =b.TicketId and b.TenantID=@TenantID
left join CRMRelationshipType c on c.Id =a.RelationshipTypeLookup and c.TenantID=@TenantID
left join CRMCompany d on d.TicketId =a.CRMCompanyLookup and d.TenantID=@TenantID
left join state s on s.ID=d.StateLookup and s.TenantID=@TenantID
where a.TenantID=@TenantID and a.TicketId=@TicketId
End