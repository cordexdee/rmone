
Declare @TenantID nvarchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B'
insert into Config_Module_ModuleUserTypes(ColumnName, ModuleNameLookup, UserTypes, Title, ITOnly, ManagerOnly, TenantID)
	select FieldName,'CPR', Name, 'CPR - ' + Name, 0, 0, TenantID from Roles where TenantID=@TenantID 
	and FieldName in (
	select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='CRMProject' and COLUMN_NAME like '%User'
	and COLUMN_NAME not in (select ColumnName from Config_Module_ModuleUserTypes where TenantID=@TenantID
	and ModuleNameLookup='CPR')
	)

insert into Config_Module_ModuleUserTypes(ColumnName, ModuleNameLookup, UserTypes, Title, ITOnly, ManagerOnly, TenantID)
	select FieldName,'OPM', Name, 'OPM - ' + Name, 0, 0, TenantID from Roles where TenantID=@TenantID 
	and FieldName in (
	select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Opportunity' and COLUMN_NAME like '%User'
	and COLUMN_NAME not in (select ColumnName from Config_Module_ModuleUserTypes where TenantID=@TenantID
	and ModuleNameLookup='OPM')
	)