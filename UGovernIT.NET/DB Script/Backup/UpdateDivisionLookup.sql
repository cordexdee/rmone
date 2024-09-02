alter table opportunity add StudioLookup bigint null default((0))
alter table CRMProject add DivisionLookup bigint null default((0))
alter table CRMServices add DivisionLookup bigint null default((0))
alter table CRMServices add SectorChoice nvarchar(max) null

Declare @tenantID nvarchar(128) = 'BCD2D0C9-9947-4A0B-9FBF-73EA61035069'
--query 1
update p set p.DivisionLookup = s.DivisionLookup
from CRMProject p join Studio s on p.StudioLookup=s.ID where p.TenantID=@tenantID
and s.TenantID=@tenantID
--query 2
update o set o.DivisionLookup=s.DivisionLookup, o.StudioLookup=s.ID from Opportunity o join Studio s
on o.CRMBusinessUnitChoice + ' > ' + o.StudioChoice = s.Description
where o.TenantID=@tenantID and s.TenantID=@tenantID

