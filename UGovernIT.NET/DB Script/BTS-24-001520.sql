Select * from Config_Module_FormLayout where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b' 
and ModuleNameLookup in ('CNS','OPM','CPR','LEM','COM','CON')
 
Select * from Config_Module_ModuleColumns where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b' 
and CategoryName in ('CNS','OPM','CPR','LEM','COM','CON')

Select * from FieldConfiguration where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b'

update Config_Module_FormLayout set FieldName='OwnerUser' where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b' 
and ModuleNameLookup in ('CNS','OPM','CPR','LEM','COM','CON')

update Config_Module_ModuleColumns set FieldName='OwnerUser' where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b' 
and CategoryName in ('CNS','OPM','CPR','LEM','COM','CON')

update FieldConfiguration set FieldName='OwnerUser' where FieldName like '%CRMOwner%' and TenantID='35525396-e5fe-4692-9239-4df9305b915b'


EXEC sp_RENAME 'CRMCompany.CRMownerUser', 'OwnerUser', 'COLUMN'
EXEC sp_RENAME 'CRMContact.CRMownerUser', 'OwnerUser', 'COLUMN'
EXEC sp_RENAME 'Opportunity.CRMownerUser', 'OwnerUser', 'COLUMN'


select * from Config_Dashboard_DashboardPanels where DashboardPanelInfo like '%CRMOwnerUser%'
update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'CRMOwnerUser','OwnerUser') where DashboardPanelInfo like '%CRMOwnerUser%' 


update Config_Dashboard_DashboardPanels set DashboardPanelInfo= REPLACE(DashboardPanelInfo,'OwnerUserUser','OwnerUser') where DashboardPanelInfo like '%OwnerUserUser%' 