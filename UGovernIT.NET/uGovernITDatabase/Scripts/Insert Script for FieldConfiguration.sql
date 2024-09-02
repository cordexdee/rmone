SET XACT_ABORT ON

begin transaction

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='CompanyTitleLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('CompanyTitleLookup', 'Company', 'Title', 'Lookup', NULL, NULL, 0, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='CompanyMultiLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('CompanyMultiLookup', 'Company', 'Title', 'Lookup', NULL, NULL, 1, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='DivisionLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('DivisionLookup', 'CompanyDivisions', 'Title', 'Lookup', NULL, NULL, 0, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='DivisionMultiLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('DivisionMultiLookup', 'CompanyDivisions', 'Title', 'Lookup', NULL, NULL, 1, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='LocationMultiLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('LocationMultiLookup', 'Location', 'Title', 'Lookup', NULL, NULL, 1, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='ServiceLookUp')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('ServiceLookUp', 'Config_Services', 'Title', 'Lookup', NULL, NULL, 0, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='ApplicationMultiLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('ApplicationMultiLookup', 'Applications', 'Title', 'Lookup', NULL, NULL, 1, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

GO

IF Not Exists(SELECT TOP 1 FieldName FROM FieldConfiguration WHERE FieldName='ProjectLifeCycleLookup')
BEGIN
INSERT INTO [dbo].[FieldConfiguration]
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           ('ProjectLifeCycleLookup', 'Config_ModuleLifeCycles', 'Name', 'Lookup', NULL, NULL, 0, NULL, NULL, 'c345e784-aa08-420f-b11f-2753bbebfdd5', NULL, NULL)
END

commit transaction