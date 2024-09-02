Alter table CRMProject Add ProjectLeadUser nvarchar(max) null
Alter table CRMProject Add LeadEstimatorUser nvarchar(max) null

Alter table CRMServices Add ProjectLeadUser nvarchar(max) null
Alter table CRMServices Add LeadEstimatorUser nvarchar(max) null

Alter table Opportunity Add ProjectLeadUser nvarchar(max) null
Alter table Opportunity Add LeadEstimatorUser nvarchar(max) null

insert into FieldConfiguration(FieldName, ParentTableName, ParentFieldName, DataType, TenantID)
values('ProjectLeadUser','','','UserField','35525396-E5FE-4692-9239-4DF9305B915B') 

insert into FieldConfiguration(FieldName, ParentTableName, ParentFieldName, DataType, TenantID)
values('LeadEstimatorUser','','','UserField','35525396-E5FE-4692-9239-4DF9305B915B') 
