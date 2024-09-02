ALTER TABLE CRMProject
ALTER COLUMN Bonus varchar(max);

ALTER TABLE CRMProject
ALTER COLUMN GeneralConditionsDelay varchar(max);

ALTER TABLE CRMProject
ALTER COLUMN WaiverDamages varchar(max);

ALTER TABLE CRMProject
ALTER COLUMN LienWaiver varchar(max);

ALTER TABLE CRMProject
ALTER COLUMN WaiverSubrogation varchar(max);

ALTER TABLE CRMProject
ALTER COLUMN DisputedWorkCap varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN Bonus varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN GeneralConditionsDelay varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN WaiverDamages varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN LienWaiver varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN WaiverSubrogation varchar(max);

ALTER TABLE Opportunity
ALTER COLUMN DisputedWorkCap varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN Bonus varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN GeneralConditionsDelay varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN WaiverDamages varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN LienWaiver varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN WaiverSubrogation varchar(max);

ALTER TABLE CRMProject_Archive
ALTER COLUMN DisputedWorkCap varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN Bonus varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN GeneralConditionsDelay varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN WaiverDamages varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN LienWaiver varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN WaiverSubrogation varchar(max);

ALTER TABLE Opportunity_Archive
ALTER COLUMN DisputedWorkCap varchar(max);

ALTER TABLE CRMProject
ADD Contingency varchar(max), SubstantialCompletion datetime, Warranties varchar(max), 
SpecialProvisions varchar(max), SignatureTitle varchar(max)

ALTER TABLE Opportunity
ADD Contingency varchar(max), SubstantialCompletion datetime, Warranties varchar(max), 
SpecialProvisions varchar(max), SignatureTitle varchar(max)

ALTER TABLE CRMProject_Archive
ADD Contingency varchar(max), SubstantialCompletion datetime, Warranties varchar(max), 
SpecialProvisions varchar(max), SignatureTitle varchar(max)

ALTER TABLE Opportunity_Archive
ADD Contingency varchar(max), SubstantialCompletion datetime, Warranties varchar(max), 
SpecialProvisions varchar(max), SignatureTitle varchar(max)


INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Contingency', 1, N'Contingency', 28, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'Contingency', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'SubstantialCompletion', 1, N'SubstantialCompletion', 29, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'SubstantialCompletion', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'Warranties', 1, N'Warranties', 30, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'Warranties', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'SpecialProvisions', 1, N'SpecialProvisions', 31, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'SpecialProvisions', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'SignatureTitle', 1, N'SignatureTitle', 32, NULL, 0, N'CPR', 0, N'', 16, NULL, NULL, NULL, 0, N'SignatureTitle', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)


INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'Contingency', 1, N'Contingency', 25, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Contingency', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'Default', N'', N'SubstantialCompletion', 1, N'SubstantialCompletion', 26, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'SubstantialCompletion', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'Warranties', 1, N'Warranties', 27, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'Warranties', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'SpecialProvisions', 1, N'SpecialProvisions', 28, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'SpecialProvisions', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

INSERT [dbo].[Config_Module_FormLayout] ( [ColumnType], [CustomProperties], [FieldDisplayName], [FieldDisplayWidth], [FieldName], [FieldSequence], [FieldType], [HideInTemplate], [ModuleNameLookup], [ShowInMobile], [SkipOnCondition], [TabId], [TargetType], [TargetURL], [ToolTip], [TrimContentAfter], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments]) 
VALUES ( N'NoteField', N'', N'SignatureTitle', 1, N'SignatureTitle', 29, NULL, 0, N'OPM', 0, N'', 13, NULL, NULL, NULL, 0, N'SignatureTitle', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-02-01T16:12:14.787' AS DateTime), CAST(N'2024-02-01T16:12:14.787' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, NULL)

UPDATE [dbo].[Config_Module_FormLayout]
SET FieldSequence = 33
WHERE FieldDisplayName = 'Contract Terms and Details' and ModuleNameLookup = 'CPR' and FieldName = '#GroupEnd#'

UPDATE [dbo].[Config_Module_FormLayout]
SET FieldSequence = 30
WHERE FieldDisplayName = 'Contract Details' and ModuleNameLookup = 'OPM' and FieldName = '#GroupEnd#'

UPDATE FieldConfiguration set Multi = 1 where FieldName = 'DiverseCertificationChoice'

UPDATE FieldConfiguration
SET Data = 'Pending;#Cancelled;#Contract Executed'
WHERE FieldName = 'ContractStatusChoice'

ALTER TABLE CRMProject
ADD ChangeOrders varchar(max)

ALTER TABLE Opportunity
ADD ChangeOrders varchar(max)

ALTER TABLE CRMProject_Archive
ADD ChangeOrders varchar(max)

ALTER TABLE Opportunity_Archive
ADD ChangeOrders varchar(max)

UPDATE Config_Module_FormLayout 
SET FieldName = 'ChangeOrders'
where FieldName = 'ApprovedChangeOrders' and TabId in (16, 13)