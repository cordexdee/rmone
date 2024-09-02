Alter table CRMProject Add LeadSuperintendentUser nvarchar(max) null

Alter table CRMServices Add LeadSuperintendentUser nvarchar(max) null

Alter table Opportunity Add LeadSuperintendentUser nvarchar(max) null

insert into FieldConfiguration(FieldName, ParentTableName, ParentFieldName, DataType, TenantID)
values('LeadSuperintendentUser','','','UserField','35525396-E5FE-4692-9239-4DF9305B915B') 
