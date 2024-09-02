
update Config_Module_FormLayout set FieldDisplayName = 'ProjectSummaryControlNew' where FieldDisplayName = 'ProjectSummaryControl' and ModuleNameLookup in ('CNS', 'CPR', 'OPM')
and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

update Config_Module_FormLayout set FieldDisplayName = FieldDisplayName + '_Hide', FieldName = FieldName+ 'Hide' where FieldDisplayName in ('Project Title', 'CPRProjectTitleControl', 'CPR Dashboard', 'TimelineControl_Hide'
,'Timeline_Hide', 'ProjectTeam', 'TaskGraph')  and ModuleNameLookup in ('CNS') and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

update Config_Module_FormLayout set FieldDisplayName = FieldDisplayName + '_Hide', FieldName = FieldName+ 'Hide' where FieldDisplayName in ('Project Title', 'CPRProjectTitleControl', 'CPR Dashboard', 'TimelineControl_Hide'
,'Timeline_Hide', 'ProjectTeam', 'TaskGraph')  and ModuleNameLookup in ('CPR') and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

update Config_Module_FormLayout set FieldDisplayName = FieldDisplayName + '_Hide', FieldName = FieldName+ 'Hide' where FieldDisplayName in ('Opportunity Info','Project Title', 'CPRProjectTitleControl','Dashboard', 'CPR Dashboard', 'TimelineControl_Hide'
,'Timeline_Hide', 'ProjectTeam', 'TaskGraph')  and ModuleNameLookup in ('OPM') and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'