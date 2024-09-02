-- MK: 1/10/2017 
update Config_Module_ModuleStages set StageWeight = 50 where ID=30;
update Config_Modules set AuthorizedToCreate=null, AuthorizedToView=null where ModuleName='TSR';
update Config_Module_ModuleColumns set ColumnType='multiuser' where ModuleNameLookup='TSR' and FieldName in ('Owner','Requestor');
--MK: 1/15/2017
update tsr set History=REPLACE(history, 'cb174371-e8db-4847-9b7a-0bf3aa541352', 'Manish')
--MK: 1/17/2017
update Config_Module_ModuleColumns set FieldName=REPLACE(FieldName,'Ticket','') where FieldName='TicketStatus'
--MS: 1/17/2017
update Config_ClientAdminCategory set ImageUrl = replace(ImageUrl, '/_Layouts/15/Images/uGovernIT/ButtonImages/', '/Content/ButtonImages/')
update Config_PageConfiguration set Name ='Admin/Admin.aspx' where ID=2
update Config_PageConfiguration set HideLeftMenu=1 where ID=2


update TSR set AssetLookup=13 where AssetLookup=0
--MS: 1/18/2017 
update Config_ConfigurationVariable set Description='Test for the grid'
update Config_ClientAdminConfigurationLists set ListName='Config_ConfigurationVariable' where Title='Configuration Variables'

--MS: 1/19/2017
--update Config_ClientAdminConfigurationLists set ListName='Config_Modules' where Title='Modules'
--update ACRTypes set IsDeleted = 1 where ID = 3

--MS: 1/24/2017
update Config_ClientAdminConfigurationLists set ListName='Config_Module_ModuleUserTypes' where  Title='User Types'
update Config_ClientAdminConfigurationLists set ListName='config_ProjectClass' where  Title='Project Class'

--MK: 1/24/2017
update Config_Module_ModuleStages set ActionUser='Owner;#PRPGroup;#PRP' 
--MS:1/25/2017
update Config_ClientAdminConfigurationLists set ListName ='Config_BudgetCategories' where Title='Budget Categories'
update Config_ClientAdminConfigurationLists set ListName ='Config_EventCategories' where Title='Event Categories'
update Config_ClientAdminConfigurationLists set ListName ='Config_ProjectInitiative' where Title='Project Initiatives'
update Config_ClientAdminConfigurationLists set ListName ='Config_Module_Priority' where Title='Priority'
--MS:1/30/2017
update Config_ClientAdminConfigurationLists set ListName='Config_WikiLeftNavigation' where ListName='WikiLeftNavigation'
update Config_ClientAdminConfigurationLists set ListName='Config_Module_ModuleColumns' where ListName='Config_MyModuleColumns'
--MS:1/31/2017
update Config_ClientAdminConfigurationLists set ListName='TaskTemplates'  where ListName='UGITTaskTemplates'
update Config_ClientAdminConfigurationLists set ListName='Config_UserRoles' where ListName='UserRoles'

--MK:2/17/2017
update Config_Modules set ModuleTable = 'DMDocumentInfoList' where ModuleName='EDM'
update Config_Modules set ModuleTable = 'DRQ' where ModuleName='DRQ'
update TSR set IsPrivate=1
update PRS set IsPrivate=1

--MK:2/22/2017
update Config_Module_ModuleStages set StageStep=1 where ID=1
update Config_Module_ModuleStages set StageStep=2 where ID=5
update Config_Module_ModuleStages set StageStep=3 where ID=6
update Config_Module_ModuleStages set StageStep=4 where ID=7
update Config_Module_ModuleStages set StageStep=5 where ID=8

update Config_Modules set ModuleRelativePagePath='/Test.aspx' where ModuleName='TSR'

--MK:2/27/2017
update Config_Modules set ShowSummary = 1
update AspNetUsers set Department='2' where Email='admin@admin.com'

--MK: 3/3/2017
update TSR set BusinessManager='a8486028-af12-462d-b836-4081888d1b19' where BusinessManager=';#manish'
update TSR set Initiator='a8486028-af12-462d-b836-4081888d1b19' where Initiator='manish'


--MK: 3/6/2017
update Config_MenuNavigation set NavigationUrl='/SitePages/RequestList?module=TSR' where ID=5

update Config_ClientAdminConfigurationLists set ListName='Config_Module_ModuleStages' where ID=14
update Config_Module_ModuleStages set LifeCycleName='Waterfall 5-Stage' where ModuleNameLookup='TSR' 

--MK 6/20/2022
update CRMProject set StageStep = 8 where TenantID='bcd2d0c9-9947-4a0b-9fbf-73ea61035069' and EstimatedConstructionEnd > GETDATE()

--MK 9/26/2022
update CRMProject set CRMProject.CRMBusinessUnitChoice = cd.Title  from CRMProject cpr join CompanyDivisions cd
on cpr.DivisionLookup = cd.ID
where cpr.TenantID='35525396-E5FE-4692-9239-4DF9305B915B'


--MK 5/12/2022
update ResourceAllocation set SoftAllocation = (case when SUBSTRING(TicketID, 0, 4) = 'OPM' then 1 else 0 end);