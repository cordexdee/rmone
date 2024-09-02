ALTER TABLE CRMProject
Add ContractNotes varchar(max), ContractStatusChoice varchar(1000),RetainageChoice varchar(100)

ALTER TABLE Opportunity
Add ContractNotes varchar(max), ContractStatusChoice varchar(1000),RetainageChoice varchar(100)

ALTER TABLE CRMProject_Archive
Add ContractNotes varchar(max), ContractStatusChoice varchar(1000),RetainageChoice varchar(100)

ALTER TABLE Opportunity_Archive
Add ContractNotes varchar(max), ContractStatusChoice varchar(1000),RetainageChoice varchar(100)

INSERT INTO FieldConfiguration(FieldName, DataType,Data, Multi, TenantID, Created, Modified, CreatedByUser,ModifiedByUser,Deleted, TableName)
VALUES('ContractStatusChoice','Choices','Awaiting Client Response;#Cancelled;#Closed;#Budgeting Negotiated', 0,'35525396-E5FE-4692-9239-4DF9305B915B', GETDATE(), GETDATE(),'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0, 'CRMProject');

INSERT INTO FieldConfiguration(FieldName, DataType,Data, Multi, TenantID, Created, Modified, CreatedByUser,ModifiedByUser,Deleted, TableName)
VALUES('ContractStatusChoice','Choices','Awaiting Client Response;#Cancelled;#Closed;#Budgeting Negotiated', 0,'35525396-E5FE-4692-9239-4DF9305B915B', GETDATE(), GETDATE(),'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0, 'Opportunity');

INSERT INTO FieldConfiguration(FieldName, DataType,Data, Multi, TenantID, Created, Modified, CreatedByUser,ModifiedByUser,Deleted, TableName)
VALUES('RetainageChoice','Choices','0%;#5%;#10%;#Other', 0,'35525396-E5FE-4692-9239-4DF9305B915B', GETDATE(), GETDATE(),'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0, 'CRMProject');

INSERT INTO FieldConfiguration(FieldName, DataType,Data, Multi, TenantID, Created, Modified, CreatedByUser,ModifiedByUser,Deleted, TableName)
VALUES('RetainageChoice','Choices','0%;#5%;#10%;#Other', 0,'35525396-E5FE-4692-9239-4DF9305B915B', GETDATE(), GETDATE(),'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0, 'Opportunity');


INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Retainage', 1, N'RetainageChoice', 11, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'RetainageChoice', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'Notes', 3, N'ContractNotes', 24, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'Lien Waivers', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.790' AS DateTime), CAST(N'2024-02-01T16:12:14.790' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Status', 1, N'ContractStatusChoice', 24, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'Retainage', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.797' AS DateTime), CAST(N'2024-02-01T16:12:14.797' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Retainage', 1, N'RetainageChoice', 11, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Waiver of Consequential Damages', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Notes', 3, N'ContractNotes', 22, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Lien Waivers', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.790' AS DateTime), CAST(N'2024-02-01T16:12:14.790' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Status', 1, N'ContractStatusChoice', 22, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Retainage', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.797' AS DateTime), CAST(N'2024-02-01T16:12:14.797' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)



