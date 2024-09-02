EXEC sp_rename 'dbo.CRMProject.ContractAssignedTo', 'ContractAssignedToUser', 'COLUMN';
EXEC sp_rename 'dbo.Opportunity.ContractAssignedTo', 'ContractAssignedToUser', 'COLUMN';
EXEC sp_rename 'dbo.CRMProject_Archive.ContractAssignedTo', 'ContractAssignedToUser', 'COLUMN';
EXEC sp_rename 'dbo.Opportunity_Archive.ContractAssignedTo', 'ContractAssignedToUser', 'COLUMN';
UPDATE Config_Module_FormLayout SET FieldName = 'ContractAssignedToUser' where FieldName = 'ContractAssignedTo'
UPDATE FieldConfiguration SET FieldName = 'ContractAssignedToUser' where FieldName = 'ContractAssignedTo'
