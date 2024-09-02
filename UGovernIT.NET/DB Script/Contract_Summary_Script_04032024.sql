ALTER TABLE CRMProject
ADD ContractAssignedTo varchar(max);

ALTER TABLE Opportunity
ADD ContractAssignedTo varchar(max);

ALTER TABLE CRMProject_Archive
ADD ContractAssignedTo varchar(max);

ALTER TABLE Opportunity_Archive
ADD ContractAssignedTo varchar(max);

INSERT INTO FieldConfiguration(FieldName, DataType, Multi,SelectionSet, TenantID, Created, Modified, CreatedByUser,ModifiedByUser,Deleted)
VALUES('ContractAssignedTo','UserField', 0,'UserOnly','35525396-E5FE-4692-9239-4DF9305B915B', GETDATE(), GETDATE(),'00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',0);

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'User', N'', N'To', 1, N'ContractAssignedTo', 32, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'To', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'User', N'', N'To', 1, N'ContractAssignedTo', 29, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'To', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)


