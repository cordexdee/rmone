

Alter Table ExperiencedTags
Add Category varchar(50)

Alter Table UserProjectExperience
add Attachments nvarchar(10), Deleted bit

ALTER TABLE UserProjectExperience
ALTER COLUMN ID bigint;

ALTER TABLE Opportunity
add TagMultiLookup nvarchar(max)

ALTER TABLE CRMServices
add TagMultiLookup nvarchar(max)

insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupStart#', 62, 0,'CPR', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'AddProjectExperienceTags', 1, '#Control#', 63, 0,'CPR', 0, 2,0, 'AddProjectExperienceTags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupEnd#', 64, 0,'CPR', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')

insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupStart#', 52, 0,'CNS', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'AddProjectExperienceTags', 1, '#Control#', 53, 0,'CNS', 0, 2,0, 'AddProjectExperienceTags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupEnd#', 54, 0,'CNS', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')

insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupStart#', 57, 0,'OPM', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'AddProjectExperienceTags', 1, '#Control#', 58, 0,'OPM', 0, 2,0, 'AddProjectExperienceTags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')


insert into Config_Module_FormLayout (ColumnType, FieldDisplayName, FieldDisplayWidth, FieldName
,FieldSequence, HideInTemplate, ModuleNameLookup, ShowInMobile, TabId,TrimContentAfter, Title,TenantID, Created,Modified, CreatedByUser, ModifiedByUser)
values('Default', 'Tags', 1, '#GroupEnd#', 59, 0,'OPM', 0, 2,0, 'Tags'
,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(), GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7')

