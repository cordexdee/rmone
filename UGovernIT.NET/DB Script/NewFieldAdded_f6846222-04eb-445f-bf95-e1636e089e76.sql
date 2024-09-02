ALTER TABLE CRMProject ADD ContractExecSumChoice VARCHAR(10),ContractInERP BIT
ALTER TABLE Opportunity ADD ContractExecSumChoice VARCHAR(10),ContractInERP BIT
GO

INSERT INTO FieldConfiguration (FieldName,DataType,Data,Multi,TenantID,Created,Modified,CreatedByUser,ModifiedByUser,Attachments,TableName)
VALUES('ContractExecSumChoice','Choices','Yes;#No;#Not Applicable',0,'f6846222-04eb-445f-bf95-e1636e089e76',GETDATE(),GETDATE(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','CRMProject')
,('ContractExecSumChoice','Choices','Yes;#No;#Not Applicable',0,'f6846222-04eb-445f-bf95-e1636e089e76',GETDATE(),GETDATE(),'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','Opportunity')
GO

INSERT [dbo].[Config_Module_FormLayout] ([ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'Default', N'', N'Contract Executive Summary', 1, N'ContractExecSumChoice', 37, NULL, 0, N'CPR', 0, N'', 17, NULL, NULL, N'', 0, N'Contract Executive Summary', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T16:44:09.043' AS DateTime), CAST(N'2024-07-18T17:27:23.730' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ([ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'Default', N'', N'Contract in ERP', 1, N'ContractInERP', 38, NULL, 0, N'CPR', 0, N'', 17, NULL, NULL, NULL, 0, N'Contract in ERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T16:44:09.050' AS DateTime), CAST(N'2024-07-18T16:44:09.050' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ([ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'Default', N'', N'Contract Executive Summary', 1, N'ContractExecSumChoice', 38, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, N'', 0, N'Contract Executive Summary', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T16:46:41.073' AS DateTime), CAST(N'2024-07-18T17:28:06.513' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
INSERT [dbo].[Config_Module_FormLayout] ([ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'Default', N'', N'Contract in ERP', 1, N'ContractInERP', 39, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Contract in ERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T16:46:41.080' AS DateTime), CAST(N'2024-07-18T16:46:41.080' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)
GO


INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 0, 0, 0, N'0- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:19.897' AS DateTime), CAST(N'2024-07-18T17:43:58.770' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 1, 0, 0, N'1- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.083' AS DateTime), CAST(N'2024-07-18T17:43:58.853' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 2, 0, 0, N'2- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.263' AS DateTime), CAST(N'2024-07-18T17:43:58.927' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 3, 0, 0, N'3- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.443' AS DateTime), CAST(N'2024-07-18T17:43:59.000' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 4, 0, 0, N'4- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.620' AS DateTime), CAST(N'2024-07-18T17:43:59.070' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 5, 0, 0, N'5- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.793' AS DateTime), CAST(N'2024-07-18T17:43:59.140' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'OPM', 6, 0, 0, N'6- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:28:20.967' AS DateTime), CAST(N'2024-07-18T17:43:59.203' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 0, 0, 0, N'0- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:34.620' AS DateTime), CAST(N'2024-07-18T19:41:15.613' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 1, 0, 0, N'1- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:34.847' AS DateTime), CAST(N'2024-07-18T19:41:15.707' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 2, 0, 0, N'2- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.017' AS DateTime), CAST(N'2024-07-18T19:41:15.773' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 3, 0, 0, N'3- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.197' AS DateTime), CAST(N'2024-07-18T19:41:15.847' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 4, 0, 0, N'4- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.387' AS DateTime), CAST(N'2024-07-18T19:41:15.917' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 5, 0, 0, N'5- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.557' AS DateTime), CAST(N'2024-07-18T19:41:15.983' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 6, 0, 0, N'6- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.740' AS DateTime), CAST(N'2024-07-18T19:41:16.053' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 7, 0, 0, N'7- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:35.923' AS DateTime), CAST(N'2024-07-18T19:41:16.123' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 8, 0, 0, N'8- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:36.097' AS DateTime), CAST(N'2024-07-18T19:41:16.197' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractExecSumChoice', 0, N'CPR', 9, 0, 0, N'9- ContractExecSumChoice', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:27:36.270' AS DateTime), CAST(N'2024-07-18T19:41:16.263' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')

INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 0, 1, 1, N'0- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.370' AS DateTime), CAST(N'2024-07-19T09:13:56.370' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 1, 1, 1, N'1- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.510' AS DateTime), CAST(N'2024-07-19T09:13:56.510' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 2, 1, 1, N'2- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.633' AS DateTime), CAST(N'2024-07-19T09:13:56.633' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 3, 1, 1, N'3- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.757' AS DateTime), CAST(N'2024-07-19T09:13:56.757' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 4, 1, 1, N'4- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.880' AS DateTime), CAST(N'2024-07-19T09:13:56.880' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 5, 1, 1, N'5- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:56.993' AS DateTime), CAST(N'2024-07-19T09:13:56.993' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'OPM', 6, 1, 1, N'6- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-19T09:13:57.113' AS DateTime), CAST(N'2024-07-19T09:13:57.113' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 0, 1, 1, N'0- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:21.197' AS DateTime), CAST(N'2024-07-18T17:11:31.760' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 1, 1, 1, N'1- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:21.370' AS DateTime), CAST(N'2024-07-18T17:11:31.843' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 2, 1, 1, N'2- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:21.540' AS DateTime), CAST(N'2024-07-18T17:11:31.903' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 3, 1, 1, N'3- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:21.710' AS DateTime), CAST(N'2024-07-18T17:11:31.967' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 4, 1, 1, N'4- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:21.880' AS DateTime), CAST(N'2024-07-18T17:11:32.033' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 5, 1, 1, N'5- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:22.070' AS DateTime), CAST(N'2024-07-18T17:11:32.103' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 6, 1, 1, N'6- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:22.290' AS DateTime), CAST(N'2024-07-18T17:11:32.170' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 7, 1, 1, N'7- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:22.490' AS DateTime), CAST(N'2024-07-18T17:11:32.233' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 8, 1, 1, N'8- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:22.673' AS DateTime), CAST(N'2024-07-18T17:11:32.297' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
INSERT [dbo].[Config_Module_RequestRoleWriteAccess] ([ActionUser], [CustomProperties], [FieldMandatory], [FieldName], [HideInServiceMapping], [ModuleNameLookup], [StageStep], [ShowEditButton], [ShowWithCheckBox], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) VALUES (N'', NULL, 0, N'ContractInERP', 0, N'CPR', 9, 1, 1, N'9- ContractInERP', N'f6846222-04eb-445f-bf95-e1636e089e76', CAST(N'2024-07-18T17:11:22.840' AS DateTime), CAST(N'2024-07-18T17:11:32.367' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')

GO